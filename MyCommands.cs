using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace PeeweeBot
{
    public class MyCommands
    {
        [Command("setup")]
        public async Task Setup(CommandContext ctx)
        {
            if (Program.channel == null)
            {
                await ctx.RespondAsync($"👋 Hi, {ctx.User.Mention}! Thanks for setting me up!");
                Program.channel = ctx.Channel;
            }
        }
    }
}