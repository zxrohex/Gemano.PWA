using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Gemano.PWA.Core.JS;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;


namespace Gemano.PWA.Controls
{
    public partial class ChatConversationView : ComponentBase
    {
        [CascadingParameter(Name = "GemanoChatInterface")]
        public GemanoChatInterface ChatInterface { get; set; }



        [Parameter]
        public string Id { get; set; }

        public GemanoChatConversation Conversation;

        public bool IsReady => ChatInterface != null && Conversation != null;

        private List<GemanoMessage> Messages => Conversation.Messages.ToList();

        protected override async Task OnParametersSetAsync()
        {
            Conversation = await ChatInterface.GetChat(Id);
        }
    }
}
