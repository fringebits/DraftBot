namespace DraftBot.Protocol
{
    using Newtonsoft.Json;

    public class MoveBack : Command
    {
        public MoveBack() : base("back")
        {
        }

        [JsonProperty(PropertyName = "arg")]
        public int Distance { get; set; }
    }
}