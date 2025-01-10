using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using static Gemano.PWA.Core.JS.GemanoChatSession;

namespace Gemano.PWA.Core.JS
{
    public class GemanoChatInterface
    {
        public IJSObjectReference JSModule { get; set; }

        public GemanoChatSession CurrentSession { get; set; }

        public event SessionChanged? OnSessionChanged;

        public delegate void SessionChanged();

        public GemanoChatInterface(IJSObjectReference jsModule)
        {
            JSModule = jsModule;
        }

        public async Task<bool> KillSession()
        {
            if (CurrentSession != null)
            {
                await CurrentSession.Destroy();
                CurrentSession = null;
                OnSessionChanged?.Invoke();
                return true;
            }

            return false;
        }
        
        public async Task<int> GetStatus()
        {
            if (JSModule != null)
            {
                string status = await JSModule.InvokeAsync<string>("GemanoSession.getStatus");

                switch (status)
                {
                    case "readily":
                        return 0;

                    case "after-download":
                        return 1;

                    case "no":
                        return -2;

                    default:
                    case "undefined":
                        return -3;
                }
            }

            return -9999;
        }

        public async Task<int> CreateSession()
        {
            if (JSModule != null)
            {
                int status;

                if ((status = await GetStatus()) != 0)
                {
                    return status;
                }

                if (CurrentSession != null)
                {
                    await KillSession();
                }

                var sessionObject = await JSModule.InvokeAsync<IJSObjectReference>("GemanoSession.createSession");

                CurrentSession = await GemanoChatSession.Create(sessionObject);

                OnSessionChanged?.Invoke();

                return 0;
            }

            return -1;
        }

        public async Task<List<GemanoChatConversation>> GetAllChats()
        {
            var chats = await JSModule.InvokeAsync<List<GemanoChatConversation>>("GemanoChatConversationManager.getAll");

     

            return chats;
        }

        public async Task<GemanoChatConversation> GetChat(string id)
        {
            var chat = await JSModule.InvokeAsync<GemanoChatConversation>("GemanoChatConversationManager.getChat", id);

            Console.WriteLine(chat.Id);

            return chat;
        }


    }

    public class GemanoChatConversation
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public GemanoMessage[] Messages { get; set; }

        

    }

    public class GemanoChatSession
    {
        public IJSObjectReference JSObject { get; private set; }

        public List<GemanoMessage> Messages { get; private set; } = new List<GemanoMessage>();

        public string Id { get; private set; }

        public string Name { get; private set; }

        public int TokensLeft { get; private set; }

        public int TokensSoFar { get; private set; }

        public int MaxTokens { get; private set; }

        private GemanoChatSession(IJSObjectReference jSObjectReference)
        {
            JSObject = jSObjectReference;
        }

        public static async Task<GemanoChatSession> Create(IJSObjectReference jsSessionReference)
        {
            var obj = new GemanoChatSession(jsSessionReference);

            await obj.RefreshStats();

            return obj;
        }

        public async Task<GemanoMessage> Prompt(string message)
        {
            Messages.Add(GemanoMessage.FromUser(message));

            string response = await JSObject.InvokeAsync<string>("prompt", message);

            var responseMessage = GemanoMessage.FromLLM(response);

            Messages.Add(responseMessage);

            await RefreshStats();

            return responseMessage;
        }

        public async Task RefreshStats()
        {
            Name = await GetName();

            TokensLeft = await GetTokensLeft();

            TokensSoFar = await GetTokensSoFar();

            MaxTokens = await GetMaxTokens();
        }

        private async Task<int> GetTokensLeft()
        {
            return await JSObject.InvokeAsync<int>("tokensLeft");
        }

        private async Task<int> GetTokensSoFar()
        {
            return await JSObject.InvokeAsync<int>("tokensSoFar");
        }

        private async Task<int> GetMaxTokens()
        {
            return await JSObject.InvokeAsync<int>("maxTokens");
        }

        private async Task<string> GetName()
        {
            return await JSObject.InvokeAsync<string>("getName");
        }

        public async Task<bool> SetName(string name)
        {
            await JSObject.InvokeVoidAsync("setName", name);

            await RefreshStats();

            return true;
        }


        public async Task Destroy()
        {
            await JSObject.InvokeVoidAsync("destroy");

            Dispose();
        }

        public async void Dispose()
        {
            if (JSObject != null)
            {
                await JSObject.DisposeAsync();

                JSObject = null;
            }

            GC.SuppressFinalize(this);
        }



    }

    public class GemanoMessage
    {
        public string Role { get; set; }

        public string Content { get; set; }

        public GemanoMessage(string role, string content)
        {
            Role = role;
            Content = content;
        }

        public GemanoMessage()
        {

        }

        public static GemanoMessage FromUser(string content)
        {
            return new GemanoMessage("user", content);
        }

        public static GemanoMessage FromLLM(string content)
        {
            return new GemanoMessage("llm", content);
        }
    }
}
