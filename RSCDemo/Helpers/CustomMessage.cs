using NuGet.Packaging.Core;

namespace RSCDemo.Helpers
{
    //placeholder in case we want to impose structure / parse the data into cleaner form
    public class CustomMessage
    {
        public string rawJson;
        public string Content;
        public string User;
        public System.DateTime lastModified;
        public CustomMessage(string rawJson, System.DateTimeOffset? lastModified, string content, string user)
        {
            this.rawJson = rawJson;
            this.Content = content;
            this.User = user;
            this.lastModified = lastModified.HasValue ? lastModified.Value.DateTime : new System.DateTime();
        }
    }
}
