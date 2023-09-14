using System;
using System.Collections.Generic;

namespace RSCDemo.Helpers
{
    public class MessageHistory
    {

        //wrapper for message history will implement method for MessageHistory.CleanHistory
        public List<CustomMessage> Messages { get; set; }

        public MessageHistory() 
        { 
            Messages = new List<CustomMessage>();
        }

        public void Add(CustomMessage message) {

            //TODO: implement Add method which will add the custom message if it's content is not <systemMessage>
            if (!message.Content.Contains("systemEventMessage")) 
            {
                Messages.Add(message);
            }
        }

        // Returns a ptr to the last index (non-inclusive) of the list defining the set of new messages
        public int GetLatestPtr(DateTime appStateDT)
        {
            var ptr = 0;
            while (DateTime.Compare(Messages[ptr].lastModified, appStateDT) < 0)
            {
                ptr++;
            }
            return ptr;
        }
    }
}
