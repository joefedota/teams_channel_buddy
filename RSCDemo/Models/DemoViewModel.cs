using System.Collections.Generic;

namespace RSCWithGraphAPI.Models
{
    public class DemoViewModel
    {
        public List<string> Channels { get; set; }

        public List<string> Permissions { get; set; }

        public string Content { get; set; }

        public string Response { get; set; }
        public List<string> Actions { get; set; }
    }
}
