using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Traceback;

namespace docsNET
{
    class Program
    {
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task MainAsync()
        {
            Logger.Write("Booting up");
            string TOKEN = File.ReadAllLines("TOKEN.txt")[0];
            Logger.Write("Token loaded");
            _client = new DiscordSocketClient();
            Logger.Write("Client initialized");
            _commands = new CommandService();
            Logger.Write("Command Service initialized");

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();
            Logger.Write("Service initialized");
            _client.Log += Log;

            IActivity activity = new Game("Prefix: c#") as IActivity;
            await _client.SetActivityAsync(activity);
            Logger.Write("Game set");
            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, TOKEN);
            Logger.Write("Client logged in");
            await _client.StartAsync();
            Logger.Write("Client started");

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Logger.Write("Logging: " + msg.ToString());
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage msg)
        {
            var message = msg as SocketUserMessage;

            if (message == null || message.Author.IsBot || message.Author.IsWebhook)
                return;

            var context = new SocketCommandContext(_client, message);

            int argPos = 0;

            if (message.HasStringPrefix("c#", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                Logger.Write($"Input by {context.User.Username}#{context.User.Discriminator} in {context.Guild.Name}: {context.Message}");

                var result = await _commands.ExecuteAsync(context: context, argPos: argPos, services: null);

                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                    Logger.Write("ERROR: " + result.ErrorReason);
                }
            }
        }
    }
}
