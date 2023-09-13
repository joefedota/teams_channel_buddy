using Microsoft.Graph;

namespace RSCDemo.Helpers
{
    //placeholder in case we want to impose structure / parse the data into cleaner form
    public class CustomMessage
    {
        public string rawJson;
        public System.DateTimeOffset? lastModified;
        public CustomMessage(string rawJson, System.DateTimeOffset? lastModified) 
        { 
            this.rawJson = rawJson;
            this.lastModified = lastModified;
        }
    }
}
