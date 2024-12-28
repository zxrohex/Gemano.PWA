using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Gemano.PWA.Core.JS
{
    public class GemanoChatInterface
    {
        public IJSObjectReference JSModule { get; set; }

        public GemanoChatSession CurrentSession { get; set; }

        public GemanoChatInterface(IJSObjectReference jsModule)
        {
            JSModule = jsModule;
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
                        return -1;

                    case "no":
                        return -2;

                    default:
                    case "undefined":
                        return -3;
                }
            }

            return -9999;
        }

        public async Task<bool> CreateSession()
        {
            if (JSModule != null)
            {
                if (await GetStatus() != 0)
                {
                    return false;
                }

                if (CurrentSession != null)
                {
                    await CurrentSession.Destroy();

                    CurrentSession = null;
                }

                var sessionObject = await JSModule.InvokeAsync<IJSObjectReference>("GemanoSession.createSession");

                CurrentSession = await GemanoChatSession.Create(sessionObject);

                return true;
            }

            return false;
        }

    }

    public class GemanoChatSession : IDisposable
    {
        public IJSObjectReference JSObject { get; private set; }

        public List<GemanoMessage> Messages { get; private set; } = new List<GemanoMessage>();

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

        public class GemanoMessage
        {
            public string Role { get; private set; }

            public string Content { get; private set; }

            public GemanoMessage(string role, string content)
            {
                Role = role;
                Content = content;
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
}
