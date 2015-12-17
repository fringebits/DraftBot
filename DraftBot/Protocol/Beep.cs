namespace DraftBot.Protocol
{
    using Newtonsoft.Json;

    public class Beep : Command
    {
        public Beep() : base("beep") { }

        [JsonProperty(PropertyName = "arg")]
        public int Duration { get; set; }
    }
}