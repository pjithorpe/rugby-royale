﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RugbyRoyale.Commands
{
    [Group("royale")]
    class Group_Royale : BaseCommandModule
    {
        [Command("hi"), Aliases("hello", "howdy")]
        public async Task Hi(CommandContext context)
        {
            await context.RespondAsync($"👋 Hi, {context.User.Mention}!");
        }
    }
}
