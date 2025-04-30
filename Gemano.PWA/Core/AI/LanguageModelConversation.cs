using Gemano.PWA.Common.Helpers.Extensions;

namespace Gemano.PWA.Core.AI
{
    public class LanguageModelConversation
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string SystemPrompt { get; set; }

        public List<LanguageModelMessage> Messages { get; set; }

        public LanguageModelConversation()
        {

        }

        public LanguageModelConversation(string name, string systemPrompt = "", List<LanguageModelMessage> initialPrompts = null)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            SystemPrompt = systemPrompt;
            Messages = initialPrompts ?? new List<LanguageModelMessage>()
            {
               LanguageModelMessage.FromSystem(systemPrompt)
            };
        }

        public LanguageModelMessage AddMessage(string role, string content)
        {
            return Messages.Push(new LanguageModelMessage(role, content));
        }

        public LanguageModelMessage AddMessage(LanguageModelMessage message)
        {
            return Messages.Push(message);
        }

        public LanguageModelMessage AddUserMessage(string content)
        {
            return Messages.Push(LanguageModelMessage.FromUser(content));
        }

        public LanguageModelMessage AddAssistantMessage(string content)
        {
            return Messages.Push(LanguageModelMessage.FromAssistant(content));
        }

        public IEnumerable<LanguageModelMessage> GetAllMessages(bool includeSystemPrompt = false)
        {
            return includeSystemPrompt ? Messages : Messages.Skip(1);
        }


        public class LanguageModelMessage         
        {
            public string Role { get; set; }
            public string Content { get; set; }

            public LanguageModelMessage()
            {

            }

            public LanguageModelMessage(string role, string content)
            {
                Role = role;
                Content = content;
            }

            public static LanguageModelMessage FromUser(string content)
            {
                return new LanguageModelMessage("user", content);
            }
            public static LanguageModelMessage FromAssistant(string content)
            {
                return new LanguageModelMessage("assistant", content);
            }

            public static LanguageModelMessage FromSystem(string content)
            {
                return new LanguageModelMessage("system", content);
            }
        }
    }
}
