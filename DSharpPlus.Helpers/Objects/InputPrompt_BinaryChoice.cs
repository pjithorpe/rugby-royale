using System;
using System.Collections.Generic;
using System.Text;

namespace DSharpPlus.Helpers
{
    public class InputPrompt_BinaryChoice
    {
        public string Title { get; set; }
        public string PromptText { get; set; }
        public string AcceptPrompt { get; set; }
        public string RejectPrompt { get; set; }
    }
}
