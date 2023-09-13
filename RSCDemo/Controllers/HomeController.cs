using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using RSCWithGraphAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RSCDemo.Helpers;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using MessagePack.Formatters;
using System.Threading.Channels;

namespace RSCWithGraphAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// RSC Setup
        /// </summary>
        [Route("Demo")]
        public async Task<ActionResult> Demo(string tenantId, string groupId, string channelId)
        {
            GraphServiceClient graphClient = await GetAuthenticatedClient(tenantId);
            var viewModel = new DemoViewModel()
            {
                //Channels = await GetChannelsList(graphClient, tenantId, groupId),
                //Permissions = await GetPermissionGrants(graphClient, tenantId, groupId)
                Permissions = await TempWrapper(graphClient, tenantId, groupId, channelId),
            };
            return View(viewModel);
        }

        /// <summary>
        /// Configure Tab
        /// </summary>
        [Route("ConfigureTab")]
        public IActionResult ConfigureTab()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        private async Task<List<CustomMessage>> GetMessagesList(GraphServiceClient graphClient, string tenantId, string groupId, string channelId)
        {
            //TODO: replace with MessageHistory object which will implement .Clean() to clean message history
            List<CustomMessage> messages = new List<CustomMessage>();
            var pageIterator = await GraphHelper.GetMessagesIterator(graphClient, tenantId, groupId, channelId, messages);

            await pageIterator.IterateAsync();
            GraphHelper.WriteToJsonFile("C:\\Users\\josephfedota\\Desktop\\output.json", messages, false);

            //return result.Select(r => r.Body.Content).ToList();
            return messages;
        }

        private async Task<List<string>> TempWrapper(GraphServiceClient graphClient, string tenantId, string groupId, string channelId) 
        {
            var messages = await GetMessagesList(graphClient, tenantId, groupId, channelId);
            return new List<string>() { messages[0].ToString() };
        }

        /// <summary>
        ///Get Authenticated Client
        /// </summary>
        private async Task<GraphServiceClient> GetAuthenticatedClient(string tenantId)
        {
            var accessToken = await GetToken(tenantId);
            var graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    requestMessage =>
                    {
                        // Append the access token to the request.
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                        // Get event times in the current time zone.
                        requestMessage.Headers.Add("Prefer", "outlook.timezone=\"" + TimeZoneInfo.Local.Id + "\"");

                        return Task.CompletedTask;
                    }));

            return graphClient;
        }

        /// <summary>
        /// Get Token for given tenant.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> GetToken(string tenantId)
        {
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(_configuration["ClientId"])
                                                  .WithClientSecret(_configuration["ClientSecret"])
                                                  .WithAuthority($"https://login.microsoftonline.com/{tenantId}")
                                                  .WithRedirectUri("https://daemon")
                                                  .Build();

            string[] scopes = new string[] { "https://graph.microsoft.com/.default" };

            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

            return result.AccessToken;
        }

    }
}
