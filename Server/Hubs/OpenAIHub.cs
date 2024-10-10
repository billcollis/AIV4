using AIV4.Client;
using AIV4.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Fast.Components.FluentUI;
using Newtonsoft.Json;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Diagnostics;
using System.Numerics;
using System.Configuration;
using NeoSmart.SecureStore;



//using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Transactions;
using Microsoft.Extensions.Options;
using OpenAI.Images;
using Microsoft.VisualBasic;

namespace AIV4.Server.Hubs
{
    public class OpenAIHub:Hub
    {
        // user secret API key for OpenAI is here
        private readonly IConfiguration _config;
        public OpenAIHub(IConfiguration config)
        {
            _config = config;
        }

        [Inject]
        public IWebHostEnvironment Env { get; set; }

        private ChatClient? _chatClient;
        private ImageClient? _imageClient;
        ClientResult<GeneratedImage>? generatedImage;

        private string _model = string.Empty;
        private string _keyName = string.Empty;
        private string _receivedLine = string.Empty;

        /// <summary>
        /// Sends a message to the OpenAI API and handle the streaming response.
        /// </summary>
        /// <param name="message">JSON serialized list of conversation contents.</param>
        /// <param name="model">The model name to use with the API.</param>
        public async Task OpenAiService(string message, string model)
        {
            this._model = model;

            var secretKey = "";
            var secretPassphrase = "";
            var conversation = new List<ConversationContent>();


            //if in dev mode get the user secrets 
            try
            {
                (secretKey, secretPassphrase) = await LoadSecretsAsync();

                conversation = JsonConvert.DeserializeObject<List<ConversationContent>>(message);
              
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    if (conversation![0].PassPhrase == "")
                    {
                        conversation![0].PassPhrase = _config["OpenAI_key:secret_passphrase"];
                    }
                }
            }
            catch (Exception ex) 
            { 
            }

            // Create client based on whether client sends passphrase or their APIkey
            _chatClient = conversation![0].PassPhrase != secretPassphrase
                ? new ChatClient(model, conversation![0].PassPhrase)
                : new ChatClient(model, secretKey);
            _imageClient = conversation![0].PassPhrase != secretPassphrase
                ? new ImageClient(model, conversation![0].PassPhrase)
                : new ImageClient(model, secretKey);
            _keyName = conversation![0].PassPhrase != secretPassphrase
                ? "Using your key"
                : "Using my API key";

            // Transform conversation into chat messages
            var chatMessages = ConvertToChatMessages(conversation);


            if(conversation![0].ContentType == ContentType.Text)
            {
                // send chat messages to OpenAI API and collect streamed responses
                await SendChatMessagesToOpenAIAsync(chatMessages);
            }else if (conversation![0].ContentType == ContentType.Image)
            {
                string prompt = conversation![0].Text;
                ImageGenerationOptions options = new ImageGenerationOptions();
                options.ResponseFormat = GeneratedImageFormat.Uri;
                options.Style = GeneratedImageStyle.Natural;
                options.Quality = GeneratedImageQuality.Standard;
                options.Size = GeneratedImageSize.W256xH256;
                generatedImage = await _imageClient.GenerateImageAsync(prompt, options);
                SendResponseBackToClientAsync(generatedImage.Value.ImageUri.AbsoluteUri);
            }

        }


        /// <summary>
        /// Convert a list of received ConversationContent into a lits of ChatMessages
        /// </summary>
        /// <param name="conversation"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private List<ChatMessage> ConvertToChatMessages(List<ConversationContent> conversation)
        {
            //tRy some linq - NOT WORKING
            //var chatMessages = conversation
                //.Where(cc => cc.ContentType == ContentType.Text)
                //.Select(cc =>
                //{
                //    switch (cc.Role)
                //    {
                //        case Role.System:
                //            return new SystemChatMessage(cc.Text);
                //        case Role.User:
                //            return new UserChatMessage(cc.Text);
                //        case Role.Assistant:
                //            return new AssistantChatMessage(cc.Text);
                //        default:
                //            throw new ArgumentOutOfRangeException();
                //    }
                //})
                //.ToList();

            var chatMessages = new List<ChatMessage>();

            foreach (var cc in conversation)
            {
                if (cc.ContentType == ContentType.Text)
                {
                    switch (cc.Role)
                    {
                        case Role.System:
                            chatMessages.Add(new SystemChatMessage(cc.Text));
                            break;
                        case Role.User:
                            chatMessages.Add(new UserChatMessage(cc.Text));
                            break;
                        case Role.Assistant:
                            chatMessages.Add(new AssistantChatMessage(cc.Text));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            return chatMessages;
        }


        private async Task SendImageRequestToOpenAIAsync(string prompt, ImageGenerationOptions options )
        {
            generatedImage = await _imageClient.GenerateImageAsync(prompt, options);
            SendResponseBackToClientAsync(generatedImage.Value.ImageUri.AbsoluteUri);
        }



        /// <summary>
        /// Load the secrets from secrets.json in Data folder 
        /// </summary>
        /// <returns>secretPassphrase and OpenAI API key</returns>
        private async Task<(string secretKey, string secretPassphrase)> LoadSecretsAsync()
        {
            string secretKey = "nokey";
            string secretPassphrase = "nophrase";
            try
            {
                using (var sman = SecretsManager.LoadStore("Data\\secrets.json"))
                {
                    sman.LoadKeyFromFile("Data\\secrets.key");
                    secretPassphrase = sman.Get("secret_passphrase");
                    secretKey = sman.Get("OpenAI_key");
                }
            }
            catch (FormatException ex)
            {
                Debug.WriteLine($"Failed to load secrets: {ex.Message}");
            }
            return (secretKey, secretPassphrase);
        }


        private async Task SendChatMessagesToOpenAIAsync(List<ChatMessage> chatMessages)
        {
            var asyncChatUpdates = _chatClient.CompleteChatStreamingAsync(chatMessages);

            // Collect tokens as they come in
            try
            {
                await foreach (var chatUpdate in asyncChatUpdates)
                {
                    await HandleChatUpdateAsync(chatUpdate);
                }
            }
            catch (Exception ex)
            {
                await SendResponseBackToClientAsync(ex.Message);
            }

        }



        /// <summary>
        /// Collect streamed - token - responses from OpenAI API
        /// Wait for new line before sending them 
        /// this could be done in the front end in each app to get the users browser to do the work
        /// </summary>
        /// <param name="chatUpdate"></param>
        /// <returns></returns>
        private async Task HandleChatUpdateAsync(StreamingChatCompletionUpdate chatUpdate)
        {
            foreach (var contentPart in chatUpdate.ContentUpdate)
            {
                if (!string.IsNullOrEmpty(contentPart.Text))
                {
                    _receivedLine += contentPart.Text;

                    if (_receivedLine.Contains("\n"))
                    {
                        await SendResponseBackToClientAsync(MarkDown.Parse(_receivedLine));
                        _receivedLine = string.Empty;
                    }
                }
            }

            if (chatUpdate.Usage != null)
            {
                //openAI is not always sending a CRLF at the end, so this was not sending the last line so need to add \n and send last line
                _receivedLine += "\n";
                await SendResponseBackToClientAsync(MarkDown.Parse(_receivedLine));
                _receivedLine = string.Empty;

                await SendResponseBackToClientAsync($"\n\n\nData...Token Usage In:{chatUpdate.Usage!.InputTokenCount} Out:{chatUpdate.Usage!.OutputTokenCount} Total:{chatUpdate.Usage!.TotalTokenCount}\nModel:{chatUpdate.Model}\nKey: {_keyName}");
            }
        }


        /// <summary>
        /// Send the response from OpenAI back to the calling app
        /// note: Clients.Caller not Clients.All
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task SendResponseBackToClientAsync(string message)
        {
            await Clients.Caller.SendAsync("ai_response", message);
        }
    }
}
