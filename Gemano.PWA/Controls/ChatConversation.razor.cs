using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Gemano.PWA.Core.JS;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;


namespace Gemano.PWA.Controls
{
    public partial class ChatConversation : ComponentBase
    {
        [CascadingParameter(Name = "GemanoChatInterface")]
        public GemanoChatInterface ChatInterface { get; set; }

        public GemanoChatSession ChatSession => ChatInterface.CurrentSession;

        public bool IsReady => ChatInterface != null && ChatSession != null;

        string inputMsg = "";

        protected override async Task OnParametersSetAsync()
        {
            //await CreateSession();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            { 
               
            }
        }

        async Task CreateSession()
        {
            if (ChatInterface != null)
            {
                await ChatInterface.CreateSession();
            }
        }

        async Task PromptAsync()
        {
            var response = await ChatSession.Prompt(inputMsg);
        }
    }
}
