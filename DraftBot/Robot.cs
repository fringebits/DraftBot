namespace DraftBot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows;
    using Newtonsoft.Json;
    using Protocol;
    using WebSocketSharp;
    using WebSocket = WebSocketSharp.WebSocket;

    // http://codereview.stackexchange.com/questions/41591/websockets-client-code-and-making-it-production-ready

    public enum RobotState
    {
        Unknown = 0,
        Ready,
        Running,
        Error
    }

    public enum PenState
    {
        Unknown = 0,
        Up,
        Down
    }

    public class Robot : IDisposable
    {
        private WebSocket socket;
        private readonly string uri;

        public PenState PenState { get; private set; }

        private int heading = 0;
        private Point position = new Point(0, 0);

        public bool AutoPenUpOnTurns { get; set; }

        public Point Position
        {
            get { return this.position; }
        }

        public int Heading
        {
            get { return this.heading; }
            private set
            {
                this.heading = FixHeading(value);
            }
        }

        public Vector Direction
        {
            get
            {
                var angle = Math.PI / 180.0 * this.Heading;
                var h = new Vector(Math.Cos(angle), Math.Sin(angle));
                h.Normalize();
                return h;
            }
        }

        // At some point, we'll surface these to the caller
        // public event EventHandler<RobotEventArgs> RobotReady = delegate { }; 

        private readonly List<Command> queue = new List<Command>();

        public RobotState State { get; private set; } = RobotState.Unknown;

        //public Robot(string ipAddress = @"10.0.0.213")
        public Robot(string ipAddress = @"192.168.4.1")
        {
            this.uri = $"ws://{ipAddress}:8899/websocket";
        }

        public void Connect()
        {
            this.socket = new WebSocket(this.uri);

            this.socket.OnMessage += this.Socket_OnMessage;
            this.socket.OnOpen += this.Socket_OnOpen;
            this.socket.OnError += this.Socket_OnError;
            this.socket.OnClose += this.Socket_OnClose;

            this.socket.Connect();
        }

        public void Close()
        {
            if (this.socket != null)
            {
                this.socket.OnMessage -= this.Socket_OnMessage;
                this.socket.OnOpen -= this.Socket_OnOpen;
                this.socket.OnError -= this.Socket_OnError;
                this.socket.OnClose -= this.Socket_OnClose;

                // no 'disposal' ??
                this.socket.Close();
                this.socket = null;
            }
        }

        private void Socket_OnClose(object sender, CloseEventArgs e)
        {
            Trace.TraceInformation("Socket_OnClose: {0}, clean={1}", e.Reason, e.WasClean);
        }

        private void Socket_OnError(object sender, ErrorEventArgs e)
        {
            Trace.TraceInformation("Socket_OnError: {0}", e.Message);
        }

        private void Socket_OnOpen(object sender, EventArgs e)
        {
            Trace.TraceInformation("Socket_OnOpen.");
            this.State = RobotState.Ready;
            this.Stop();
            this.PenUp();
        }

        public int JobCount
        {
            get { return this.queue.Count; }
        }

        public int JobsComplete
        {
            get { return this.queue.Count(t => t.State == CommandState.Completed); }
        }

        public bool IsIdle
        {
            get { return !this.queue.Any(t => t.State < CommandState.Completed); }
        }

        public double Scale { get; set; } = 1.0;

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            Trace.TraceInformation("Socket_OnMessage.");
            if (e.IsText)
            {
                var text = Encoding.Default.GetString(e.RawData);
                var response = JsonConvert.DeserializeObject<Response>(text);
                Trace.TraceInformation("Receive: {0}", response);

                if (response.Status == "accepted")
                {
                    var cmd = this.queue.FirstOrDefault((t) => t.Id == response.Id);
                    if (cmd != null)
                    {
                        // we're in an error state if we don't find one.
                        Trace.TraceInformation("Accepted! {0}", cmd);
                        cmd.State = CommandState.Accepted;
                    }
                }
                else if (response.Status == "complete")
                {
                    var cmd = this.queue.FirstOrDefault((t) => t.Id == response.Id);
                    if (cmd != null)
                    {
                        // we're in an error state if we don't find one.
                        Trace.TraceInformation("Completed! {0}", cmd);
                        cmd.State = CommandState.Completed;

                        int distance;
                        var dir = this.Direction;

                        switch (cmd.Name)
                        {
                            case "forward":
                                distance = (cmd as MoveForward).Distance;
                                this.position.X += distance * dir.X;
                                this.position.Y += distance * dir.Y;
                                this.position = new Point(Math.Round(this.position.X, 0), Math.Round(this.position.Y, 0));
                                break;

                            case "back":
                                distance = (cmd as MoveBack).Distance;
                                this.position.X -= distance * dir.X;
                                this.position.Y -= distance * dir.Y;
                                this.position = new Point(Math.Round(this.position.X, 0), Math.Round(this.position.Y, 0));
                                break;

                            case "left":
                                this.Heading += (cmd as TurnLeft).Angle;
                                break;

                            case "right":
                                this.Heading -= (cmd as TurnRight).Angle;
                                break;

                            case "penup":
                                this.PenState = PenState.Up;
                                break;

                            case "pendown":
                                this.PenState = PenState.Down;
                                break;
                        }
                    }

                    Trace.TraceInformation("{0} of {1} jobs complete.", this.JobsComplete, this.JobCount);
                    Trace.TraceInformation("Robot Position={0}, Heading={1}, Direction={2}", this.Position, this.Heading, this.Direction);

                    // We're back in the ready state.
                    this.State = RobotState.Ready;
                    Task.Run(() => this.ProcessQueue());

                }
                else if (response.Status == "error")
                {
                    // depending on the error, we'll try to recover
                    throw new NotImplementedException();
                }
            }
        }

        private void Send(Command cmd)
        {
            if (cmd.IsImmediate)
            {
                this.SendImmediate(ref cmd);
            }
            else
            {
                this.queue.Add(cmd);
                this.ProcessQueue();
            }
        }

        private void ProcessQueue()
        {
            if (this.State == RobotState.Ready)
            {
                var cmd = this.queue.FirstOrDefault((t) => t.State == CommandState.Idle);
                if (cmd != null)
                {
                    this.SendImmediate(ref cmd);
                }
            }
        }

        private void SendImmediate(ref Command cmd)
        {
            this.State = RobotState.Running;
            Trace.TraceInformation("Sending: {0}", cmd);
            this.socket.Send(cmd.Serialize());
            cmd.State = CommandState.Waiting;
        }

        public bool WaitForComplete()
        {
            return this.WaitForComplete(TimeSpan.FromDays(1));
        }

        public bool WaitForComplete(TimeSpan timeout)
        {
            var delta = TimeSpan.FromMilliseconds(10);

            // todo: replace this with proper thread/wait
            while (!this.IsIdle && (timeout > TimeSpan.Zero))
            {
                Thread.Sleep(delta);
                timeout -= delta;
            }

            if (timeout <= TimeSpan.Zero)
            {
                Trace.TraceError("WaitForCompelte timed out.");
            }

            return this.IsIdle;
        }

        public void Beep(int duration)
        {
            this.Send(new Beep { Duration = duration });
        }

        public void PenUp()
        {
            this.Send(new PenUp());
        }

        public void PenDown()
        {
            this.Send(new PenDown());
        }

        public void MoveForward(int value)
        {
            value = (int)Math.Round(value * this.Scale);
            this.Send(new MoveForward { Distance = value });
        }

        public void MoveBack(int value)
        {
            value = (int)Math.Round(value * this.Scale);
            this.Send(new MoveBack { Distance = value });
        }

        public void Move(int value)
        {
            if (value > 0)
            {
                this.MoveForward(value);
            }
            else if (value < 0)
            {
                this.MoveBack(-value);
            }
        }

        public void TurnLeft(int value)
        {
            this.Turn(value);
        }

        public void TurnRight(int value)
        {
            this.Turn(-value);
        }

        public void Turn(int value)
        {
            var penIsDown = false;
            if (this.AutoPenUpOnTurns && this.PenState == PenState.Down)
            {
                penIsDown = true;
                this.PenUp();
            }

            if (value > 0)
            {
                this.Send(new TurnLeft { Angle = value });
            }
            else if (value < 0)
            {
                this.Send(new TurnRight { Angle = -value });
            }

            if (penIsDown)
            {
                this.PenDown();
            }
        }

        public void GotoHeading(int heading)
        {
            var delta = FixHeading(heading - this.Heading);

            if (delta > 180)
            {
                delta = 360 - delta;
            }

            if (delta < -180)
            {
                delta = 360 + delta;
            }

            this.Turn((int)delta);
            this.WaitForComplete();
        }

        public void GotoPoint(Point point)
        {
            // compute heading to point
            var v = point - this.Position;
            var distance = v.Length;
            v.Normalize();

            var h = Math.Atan2(v.Y, v.X) * (180.0 / Math.PI);
            var delta = FixHeading((int)Math.Round(h - this.Heading));

            this.Turn(delta);
            this.MoveForward((int)Math.Round(distance));
            this.WaitForComplete();
        }

        public void Stop()
        {
            this.Send(new Stop());
        }

        public void Dispose()
        {
            this.Close();
        }

        private static int FixHeading(int h)
        {
            while (h >= 360)
            {
                h -= 360;
            }
            while (h < -360)
            {
                h += 360;
            }
            return h;
        }
    }
}
