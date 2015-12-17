namespace DraftBot
{
    using Protocol;

    public class RobotEventArgs
    {
        public Response Response { get; set; }

        public string Status
        {
            get { return this.Response.Status; }
        }
    }
}