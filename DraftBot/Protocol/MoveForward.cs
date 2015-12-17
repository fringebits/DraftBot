namespace DraftBot.Protocol
{
    using Newtonsoft.Json;

    public class MoveForward : Command
    {
        public MoveForward() : base("forward")
        {
        }

        [JsonProperty(PropertyName = "arg")]
        public int Distance { get; set; }
    }
}