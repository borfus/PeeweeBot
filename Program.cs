using System;
using System.Threading.Tasks;
using System.IO;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;

namespace PeeweeBot
{
    static public class Program
    {
        static DiscordClient discord;
        static public DiscordChannel channel;
        static CommandsNextModule commands;

        static string wordOfTheDay = null;

        static string[] lines = File.ReadAllLines("WordLibrary.txt"); // Store in exe location
        static Random rand = new Random();

        static DateTime tomorrow = DateTime.Today.AddDays(1);
        static public DateTime Tomorrow { get; }

        static void Main(string[] args)
        {
            NewWord();
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "[Bot Token Here]",
                TokenType = TokenType.Bot
            });

            if (tomorrow == DateTime.Today && channel != null)
            {
                await CheckDate(channel);
            }

            discord.MessageCreated += async e =>
            {
                // !wotd command
                if (e.Message.Content.ToLower().Contains("!wotd") && !e.Message.Author.IsBot)
                {
                    await e.Message.RespondAsync(String.Format("The word of the day is: {0}!", wordOfTheDay));
                }

                // respond to wotd
                if (e.Message.Content.ToLower().Contains(wordOfTheDay) && !e.Message.Author.IsBot)
                {
                    await e.Message.RespondAsync("AAAAAAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHHH!!!");
                }
            };

            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = "!"
            });

            commands.RegisterCommands<MyCommands>();

            async Task CheckDate(DiscordChannel channel)
            {
                tomorrow = DateTime.Today.AddDays(1);
                NewWord();
                await discord.SendMessageAsync(channel, String.Format("The word of the day has been reset! The word of the day is now: {0}", wordOfTheDay), false, null);
            }

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
        
        static void NewWord()
        {
            wordOfTheDay = lines[rand.Next(lines.Length)];
        }
    }
}
