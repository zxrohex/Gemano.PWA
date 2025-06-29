namespace Gemano.PWA.Core.Models
{
    public abstract class ChatMessageContent
    {
        public abstract object Content { get; set; }

        public abstract string Type { get; }
    }

    public class TextChatMessageContent : ChatMessageContent
    {
        public override object Content { get; set; }

        public override string Type => "text";

        public string TextContent { get; set; }

        public TextChatMessageContent(string content)
        {
            Content = content;

            TextContent = content;
        }
    }

    public class ImageChatMessageContent : ChatMessageContent
    {
        public override object Content { get; set; }

        public override string Type => "image";

        public byte[] ImageContent { get; set; }



        public ImageChatMessageContent(byte[] imageContent)
        {
            Content = imageContent;

            ImageContent = imageContent;
        }
    }

   public static class ImageChatMessageContentExtensions
    {
        public static string AsBase64Src(this ImageChatMessageContent content)
        {
            return $"data:image/png;base64,{Convert.ToBase64String(content.ImageContent)}";
        }
    }

    public class ChatMessage
    {
        public string Role { get; set; }

        public List<ChatMessageContent> Contents { get; set; }

        public string Content => Contents != null ? Contents.OfType<TextChatMessageContent>().FirstOrDefault().TextContent : null;

        public ChatMessage(string role, string content)
        {
            Role = role;

            Contents = new List<ChatMessageContent>
            {
                new TextChatMessageContent(content)
            };
        }

        public ChatMessage(string role, string textContent, byte[] imageContent)
        {
            Role = role;

            Contents = new List<ChatMessageContent>
            {
                new TextChatMessageContent(textContent),
                new ImageChatMessageContent(imageContent)
            };
        }

        public ChatMessage(string role, List<ChatMessageContent> contents)
        {
            Role = role;

            Contents = contents ?? new List<ChatMessageContent>();
        }

        public static ChatMessage FromSystem(string content)
        {
            return new ChatMessage("system", content);
        }

        public static ChatMessage FromUser(string content)
        {
            return new ChatMessage("user", content);
        }

        public static ChatMessage FromUser(string textContent, byte[] imageContent)
        {
            return new ChatMessage("user", textContent, imageContent);
        }

        public static ChatMessage FromAssistant(string content)
        {
            return new ChatMessage("assistant", content);
        }
    }
}
