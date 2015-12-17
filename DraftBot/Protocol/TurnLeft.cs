namespace DraftBot.Protocol
{
    using Newtonsoft.Json;

    public class TurnLeft : Command
    {
        public TurnLeft() : base("left")
        {
        }

        [JsonProperty(PropertyName = "arg")]
        public int Angle { get; set; }
    }
}