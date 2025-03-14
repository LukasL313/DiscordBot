  using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace DiscordBot.Commands;

public class Commands : ModuleBase<SocketCommandContext>
{
    [Command("Hi")]
    public async Task HiAsync()
    {
        await ReplyAsync("Hi");
     }


    [Command("Ping")]
    public async Task FunnyaAsync()
    {
        await ReplyAsync("Pong");
    }

    // Utfører ulike operasjoner

    [Command("Addition")]
    [Summary("Adds two numbers together.")]
     public async Task AdditionAsync(int num1, int num2)
     {
         await Context.Channel.SendMessageAsync($"{num1} + {num2} = {num1 + num2}");
     } 

     [Command("Minus")]
     [Summary("Subtracts two numbers.")]
     public async Task MinusAsync(int num1, int num2)
     {
        await Context.Channel.SendMessageAsync($"{num1} - {num2} = {num1 - num2}");
     }

     [Command("Multiply")]
     [Summary("Multiplies two numbers.")]
     public async Task MulitplyAsync(int num1, int num2)
     {
            await Context.Channel.SendMessageAsync($"{num1} * {num2} = {num1 * num2}");
     }

     // Informasjon om bruker
    [Command("Userinfo")]
    [Summary("Displays information about the user.")]
    [Alias("user", "whois")]


    public async Task UserInfoAsync(SocketUser? user = null)
    {
    var userInfo = user ?? Context.Client.CurrentUser;
    await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
    }
}