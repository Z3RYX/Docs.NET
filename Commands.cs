using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using System.Net;
using Traceback;

namespace docsNET
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        #region Log
        public void Log(ref int x)
        {
            Console.WriteLine(x);
            x++;
        }
        public void Log(string str)
        {
            Logger.Write("Command: " + str);
            Console.WriteLine(str);
        }
        public void Log(string[] str)
        {
            Console.WriteLine(string.Join('\n', str));
        }
        #endregion Log

        public T GetJSONfromURL<T>(string URL)
        {
            T json;

            Log($"Getting JSON from {URL}");

            try
            {
                using (WebClient wc = new WebClient())
                {
                    json = JsonConvert.DeserializeObject<T>(wc.DownloadString(URL));
                }
            }
            catch (Exception)
            {
                using (WebClient wc = new WebClient())
                {
                    Log("JSON not found");
                    json = default(T);
                }
            }

            return json;
        }
        public T GetJSONfromURL<T>(string URL, string URLnotfound)
        {
            T json;

            Log($"Getting JSON from {URL}");

            try
            {
                using (WebClient wc = new WebClient())
                {
                    json = JsonConvert.DeserializeObject<T>(wc.DownloadString(URL));
                }
            }
            catch (Exception)
            {
                using (WebClient wc = new WebClient())
                {
                    Log("JSON not found");
                    json = JsonConvert.DeserializeObject<T>(wc.DownloadString(URLnotfound));
                }
            }

            return json;
        }

        [Command("info")]
        public async Task InfoAsync()
        {
            await ReplyAsync("I'm a bot that helps you with C#\n" +
                "To invite me to your server use the link below.\n" +
                "https://discordapp.com/api/oauth2/authorize?client_id=490558394747584544&permissions=18432&scope=bot\n\n" +
                "By using Docs.NET you agree that the bot collects some data for debugging purposes.\n" +
                "Collected data is:\n" +
                "- Username + descriminator of the user who called a command\n" +
                "- Name of the server where the command was called\n" +
                "- Time and content of the message with the command\n\n" +
                "Data that is not collected:\n" +
                "- Any kind of IDs\n" +
                "- Messages without the bots prefix");
        }

        [Command("help")]
        public async Task HelpAsync()
        {
            await ReplyAsync("**List of commands:**\n" +
                "info - Info about the bot and the data it collects.\n" +
                "help - What you see right now.\n" +
                "docs <method/class> - Gives info about specific methods or classes.");
        }

        [Command("docs")]
        public async Task DocsAsync()
        {
            await ReplyAsync("I'll give you information for any method or class. Just add it to the command like: `c#docs string`");
        }

        [Command("docs")]
        public async Task DocsAsync([Remainder] string input)
        {
            async void Rep(string msg)
            {
                await ReplyAsync(msg);
            }

            input = input.Trim();

            if (input == "")
            {
                await ReplyAsync("I'll give you information for any method or class. Just add it to the command like: `c#docs string`");
                return;
            }

            Docs docs = GetJSONfromURL<Docs>($"http://zeryx.xyz/api/Docs/{input}.json", $"http://zeryx.xyz/api/Docs/notfound.json");

            Rep($"{docs.docClass}\n{docs.docDesc}\n{docs.docLink}");
        }

        [Command("end"), RequireOwner]
        public async Task EndAsync()
        {
            await ReplyAsync("Goodbye");
            Environment.Exit(0);
        }
    }
}
