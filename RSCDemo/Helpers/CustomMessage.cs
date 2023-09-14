namespace RSCDemo.Helpers
{
    //placeholder in case we want to impose structure / parse the data into cleaner form
    public class CustomMessage
    {
        public string Content;
        public string User;
        public System.DateTime lastModified;

        public CustomMessage(System.DateTimeOffset? lastModified, string content, string user)
        {
            this.Content = content;
            this.User = user;
            this.lastModified = lastModified.HasValue ? lastModified.Value.DateTime : new System.DateTime();
        }
    }
}
