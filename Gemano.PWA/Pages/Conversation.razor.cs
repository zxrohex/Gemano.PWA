using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Components;

namespace Gemano.PWA.Pages
{
    public partial class Conversation : ComponentBase
    {
        [Parameter]
        public string Id { get; set; }

    }
}
