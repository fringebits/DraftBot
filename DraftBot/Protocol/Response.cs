namespace DraftBot.Protocol
{
    using Newtonsoft.Json;

    public class Response
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; private set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "msg")]
        public string Message { get; private set; }

        public override string ToString()
        { 
            var data = JsonConvert.SerializeObject(this, Formatting.None);
            return data;
        }

    }
}