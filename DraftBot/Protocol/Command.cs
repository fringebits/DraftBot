namespace DraftBot.Protocol
{
    using Newtonsoft.Json;

    public enum CommandState
    {
        Idle = 0,
        Waiting, // sent, waiting to be accepted
        Accepted,  // command has been accepted
        Completed,
        Failed
    }

    public abstract class Command : ICommand
    {
        static int index = 0;

        internal Command(string cmd)
        {
            this.Name = cmd;
            this.Id = $"{index++}";
            this.State = CommandState.Idle;
        }

        [JsonIgnore]
        public CommandState State { get; internal set; }

        [JsonIgnore]
        public virtual bool IsImmediate {
            get { return false; }
        }

        [JsonProperty(PropertyName = "cmd")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        public string Serialize()
        {
            var data = JsonConvert.SerializeObject(this, Formatting.None);
            return data;
        }

        public override string ToString()
        {
            return $"{this.Serialize()} state={this.State}";
        }
    }
}