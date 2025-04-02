using Microsoft.JSInterop;

namespace Gemano.PWA.Core.AI
{
    public class LLMSessionManager
    {
        private IJSRuntime jsRuntime;

        private IJSObjectReference jsModule;

        private IJSObjectReference jsObject;

        private DotNetObjectReference<LLMSessionManager> dotNetObjectReference;

        public string AvailabilityStatus { get; set; }

        public delegate void DownloadProgress(int loaded, int total);
        public event DownloadProgress OnDownloadProgress;

        public delegate void DownloadComplete();
        public event DownloadComplete OnDownloadComplete;

        public LLMSessionManager(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task<bool> Initialize()
        {
            jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/ai-llm.js");

            dotNetObjectReference = DotNetObjectReference.Create(this);

            jsObject = await jsModule.InvokeAsync<IJSObjectReference>("LLMSessionManager.init", dotNetObjectReference);

            await jsObject.InvokeVoidAsync("test");

            AvailabilityStatus = await IsAvailable();

            return true;
        }

        public async Task<string> IsAvailable()
        {
            AvailabilityStatus = await jsObject.InvokeAsync<string>("isAvailable");

            return AvailabilityStatus;
        }

        public async Task<LLMSession> CreateSession()
        {
            var jsSession = await jsObject.InvokeAsync<IJSObjectReference>("createSession");

            return new LLMSession(jsSession);
        }

        [JSInvokable]
        public void Test(int a)
        {
            Console.WriteLine(a);
        }

        [JSInvokable]
        public void JSDownloadProgress(int l, int t)
        {
            OnDownloadProgress?.Invoke(l, t);

            Console.WriteLine($"{l} -- {t}");

            if (l == t)
            {
                OnDownloadComplete?.Invoke();
            }
        }
    }

    public class LLMSession
    {
        private IJSObjectReference jsObject;

        public LLMSession(IJSObjectReference jsObject)
        {
            this.jsObject = jsObject;
        }

        public async Task<LLMMessage> Prompt(string content)
        {
            var response = await jsObject.InvokeAsync<string>("prompt", content);

            return LLMMessage.FromAssistant(response);
        }
    }

    public class LLMMessage
    {
        public string Role { get; set; }

        public string Content { get; set; }

        public LLMMessage(string role, string content)
        {
            Role = role;
            Content = content;
        }

        public static LLMMessage FromUser(string content)
        {
            return new LLMMessage("user", content);
        }

        public static LLMMessage FromAssistant(string content)
        {
            return new LLMMessage("assistant", content);
        }
    }

    public class LLMConversation
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public List<LLMMessage> Messages { get; set; }

        public LLMConversation()
        {

        }

        public LLMConversation(string name)
        {
            Name = name;
            Id = Guid.NewGuid().ToString();
            Messages = new List<LLMMessage>();
        }
    }
}
