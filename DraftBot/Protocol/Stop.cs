namespace DraftBot.Protocol
{
    using Newtonsoft.Json;

    public class Stop : Command
    {
        public Stop() : base("stop") { }

        [JsonIgnore]
        public override bool IsImmediate { get { return true; } }
    }
}