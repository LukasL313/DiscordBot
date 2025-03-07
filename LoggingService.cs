using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot.LoggingService;
public class LoggingService
{
    public LoggingService(DiscordSocketClient client, CommandService command)
    { 
      // Når enten client.log/command.log blir kjørt blir hendelsen dokumentert av log 
      client.Log += LogAsync;
      command.Log += LogAsync;
    }   

    public Task LogAsync(LogMessage message)
    {
       if (message.Exception is CommandException cmdExpection)
       {
         Console.WriteLine($"[Command/{message.Severity}] {cmdExpection.Command.Aliases.First()}"
         + $" failed to execute in {cmdExpection.Context.Channel}.");
         Console.WriteLine(cmdExpection);   
       }
       else
       {
         Console.WriteLine($"[General/{message.Severity}] {message}");
         if (message.Exception != null)
         {
            Console.WriteLine($"Expection: {message.Exception}");
         }
       }
       return Task.CompletedTask;
    }
}
