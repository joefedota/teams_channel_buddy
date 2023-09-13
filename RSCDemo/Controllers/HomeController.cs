using Azure.AI.OpenAI;
using Azure;
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
using Microsoft.CodeAnalysis;

namespace RSCWithGraphAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private OpenAIClient client;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            client = new OpenAIClient(new Uri("https://americasopenai.azure-api.net"), new AzureKeyCredential("8f9931f759ea41a59f181b82535736ce"));
        }

        /// <summary>
        /// Hackathon
        /// </summary>
        [Route("Hackathon")]
        public IActionResult Hackathon()
        {
            // 1. call Graph API
            //string content = ...

            // 2. call Open AI
            GetCompletionResponse();

            return View();
        }

        private async Task<ViewResult> GetCompletionResponse()
        {
            //var client = new OpenAIClient(new Uri("https://americasopenai.azure-api.net"), new AzureKeyCredential("8f9931f759ea41a59f181b82535736ce"));
            string content = "Please summarize below paragraph\n" +
                "Manage a bot\r\nArticle\r\n11/15/2022\r\n8 contributors\r\nIn this article\r\nAzure Bot resource settings\r\nApplication service settings\r\nNext steps\r\nAPPLIES TO: SDK v4\r\n\r\nIn your browser, navigate to the Azure portal. Select your bot resource, such as an Azure Bot. On the navigation pane you'll see the sections described below.\r\n\r\nAzure Bot resource settings\r\nThe Azure Bot resource contains the settings described below.\r\n\r\nGeneral\r\nAt the top of the navigation pane are links for general information applicable to a bot.\r\n\r\nLink\tDescription\r\nOverview\tContains high level information about the bot, such as a bot's Subscription ID and Messaging endpoint. On the overview pane, you can also download the bot source code.\r\nActivity log\tProvides detailed diagnostic and auditing information for Azure resources and the Azure platform they depend on. For more information, see Overview of Azure platform logs.\r\nAccess control (IAM)\tDisplays the access users or other security principals have to Azure resources. For more information, see View the access a user has to Azure resources.\r\nTags\tDisplays the tags to your Azure resources, resource groups, and subscriptions to logically organize them into a taxonomy. For more information, see Use tags to organize your Azure resources.\r\nSettings\r\nIn the Settings section are links for most of your bot's management options.\r\n\r\nLink\tDescription\r\nBot profile\tManages various bot profile settings, such as display name, icon, and description.\r\nConfiguration\tManages various bot settings, such as analytics, messaging endpoint, and OAuth connection settings.\r\nChannels\tConfigures the channels your bot uses to communicate with users.\r\nPricing\tManages the pricing tier for the bot service.\r\nTest in Web Chat\tUses the integrated Web Chat control to quickly test your bot.\r\nEncryption\tManages your encryption keys.\r\nProperties\tLists your bot resource properties, such as resource ID, subscription ID, and resource group ID.\r\nLocks\tManages your resource locks.\r\nMonitoring\r\nIn the Monitoring section are links for most of your bot's monitoring options.\r\n\r\nLink\tDescription\r\nConversational analytics\tEnables analytics to view the collected data with Application Insights. This Analytics blade will be deprecated. For more information, see Add telemetry to your bot and Analyze your bot's telemetry data.\r\nAlerts\tConfigure alert rules and attend to fired alerts to efficiently monitor your Azure resources. For more information, see Overview of alerts in Microsoft Azure.\r\nMetrics\tSelect a metric to see data in the proper chart.\r\nDiagnostic settings\tDiagnostic settings are used to configure streaming export of platform logs and metrics for a resource to the destination of your choice. For more information,see diagnostic settings.\r\nLogs\tProduce insights from Azure Monitor logs.\r\nApplication service settings\r\nA bot application, also known as an application service (App Service), has a set of application settings that you can access through the Azure portal. They're environment variables passed to the bot application code. For more information, see Configure an App Service app in the Azure portal.\r\n\r\nIn your browser, navigate to the Azure portal.\r\nSearch for your bot app service and select its name.\r\nThe bot app service information is displayed.\r\n\r\nBot identity information\r\nFollow these steps to add identity information to your bot's configuration file. The file differs depending on the programming language you use to create the bot.\r\n\r\n Important\r\n\r\nThe Java and Python versions of the Bot Framework SDK only support multi-tenant bots. The C# and JavaScript versions support all three application types for managing the bot's identity.\r\n\r\nLanguage\tFile name\tNotes\r\nC#\tappsettings.json\tSupports all three application types for managing your bot's identity.\r\nJavaScript\t.env\tSupports all three application types for managing your bot's identity.\r\nJava\tapplication.properties\tOnly supports multi-tenant bots.\r\nPython\tconfig.py\tOnly supports multi-tenant bots. Provide the identity properties as arguments to the os.environ.get method calls.\r\nThe identity information you need to add depends on the bot's application type. Provide the following values in your configuration file.\r\n\r\nUser-assigned managed identity\r\nSingle-tenant\r\nMulti-tenant\r\nOnly available for C# and JavaScript bots.\r\n\r\nProperty\tValue\r\nMicrosoftAppType\tUserAssignedMSI\r\nMicrosoftAppId\tThe client ID of the user-assigned managed identity.\r\nMicrosoftAppPassword\tNot applicable. Leave this blank for a user-assigned managed identity bot.\r\nMicrosoftAppTenantId\tThe tenant ID of the user-assigned managed identity.\r\nNext steps\r\nNow that you've explored the Bot Service blade in the Azure portal, learn about the Bot Framework Service, how bots communicate with users, and about activities, channels, HTTP POST requests, and more.";

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                //Messages = { new Azure.AI.OpenAI.ChatMessage(ChatRole.System, "You are a helpful assistant.") },
                Messages = { new Azure.AI.OpenAI.ChatMessage(ChatRole.System, content) },
                MaxTokens = 100
            };

            Response<ChatCompletions> response = client.GetChatCompletions(
                deploymentOrModelName: "gpt-35-turbo-16k",
                chatCompletionsOptions);

            var viewModel = new HackathonViewModel()
            {
                //Response = response.Value.Choices[0].Message.Content
                //Console.WriteLine($"Chatbot: {completion}");

                Response = DateTime.Now.ToString() // to test tab navagation would trigger this event
            };

            return View(viewModel);
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
