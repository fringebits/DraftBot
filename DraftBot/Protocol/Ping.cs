namespace DraftBot.Protocol
{
    using Newtonsoft.Json;

    public class Ping : Command
    {
        public Ping() : base("ping") { }
    }
}