using System;
using System.Threading.Tasks;   
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
}