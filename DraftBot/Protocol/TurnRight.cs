namespace DraftBot.Protocol
{
    using Newtonsoft.Json;

    public class TurnRight : Command
    {
        public TurnRight() : base("right")
        {
        }

        [JsonProperty(PropertyName = "arg")]
        public int Angle { get; set; }
    }
}