using RSCDemo.Helpers;

namespace RSCDemo.Models
{
    public class AppStateModel
    {
        public int ChannelId { get; set; }
        public string Summary { get; set; }
        public string ActionItem { get; set; }
        public CustomMessage LastMessage { get; set; }
    }
}
