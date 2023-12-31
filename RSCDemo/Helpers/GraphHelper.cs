﻿using Microsoft.Graph;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RSCDemo.Helpers
{
    public class GraphHelper
    {
        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static async Task<PageIterator<ChatMessage>> GetMessagesIterator(GraphServiceClient graphClient, string tenantId, string groupId, string channelId, List<CustomMessage> messages)
        {

            var result = await graphClient.Teams[groupId].Channels[channelId].Messages
                .Request()
                .GetAsync();


            var pageIterator = PageIterator<ChatMessage>
                .CreatePageIterator(
                    graphClient,
                    result,
                    (msg) =>
                    {
                        CustomMessage newMessage = new CustomMessage(JsonConvert.SerializeObject(msg), msg.LastModifiedDateTime);
                        //TODO: replace with MessageHistory Object so can run Clean from controller
                        messages.Add(newMessage);
                        //populate object to pass to OpenAI helper with required fields
                        return true;
                    });

            //return result.Select(r => r.Body.Content).ToList();
            return pageIterator;
        }
    }
}
