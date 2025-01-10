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

        public bool IsReady => ChatInterface != null && ChatSession != null && StatusCode == 0;

        public int StatusCode = -1000;

        private List<GemanoMessage> Messages => ChatSession.Messages;

        string inputMsg = "";

        string inputName = "";

        protected override async Task OnParametersSetAsync()
        {
            

            await Initialize();
        }



        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

            }

        }

        async Task Initialize()
        {

            if (await CreateSession() != -47279)
            {
                inputName = ChatSession.Name;

            }


        }

        async Task AfterNameChanged()
        {
            await ChatSession.SetName(inputName);
        }

        async Task<int> CreateSession()
        {
            if (ChatInterface != null)
            {
                if (ChatSession == null)
                {
                    StatusCode = await ChatInterface.CreateSession();

                    await Task.Delay(2000);

                    return 1001;
                }
                else
                {

                    StatusCode = 0;

                    return 1000;
                }



            }


            StatusCode = -47279;

            return -47279;
        }

        async Task PromptAsync()
        {
            var response = await ChatSession.Prompt(inputMsg);
        }
    }
}
