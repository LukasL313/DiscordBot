using System.Net.Sockets;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Commands;
using DiscordBot.LoggingService;
public class Program
 {    
    private static DiscordSocketClient? _client; 
    private static LoggingService? _loggingService;
    private static CommandService? _commands;
    private static readonly char _prefix = '!';

    public static async Task Main()
    {
        // Endrer config som tillater oss å endre på meldings cache størrelse og gateway intents.
        var config = new DiscordSocketConfig 
        { 
            MessageCacheSize = 100,
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };;

        // Initialiserer CommandService & andre tjenester
        var commandService = new CommandService();

        _client = new DiscordSocketClient(config);
        
        _loggingService = new LoggingService(_client, commandService);

        _commands = new CommandService(); 


        var token = File.ReadAllText("token.txt");
        // Når .client/"" blir kjørt blir tilsvarende funksjon kjørt
        _client.MessageReceived += HandleKeyWordCmds;
        _client.MessageReceived += HandleCommandAsync;
        _client.MessageReceived += MessageReceived;
        _client.MessageUpdated += MessageUpdated;

        // Logger botten på discord & Forteller at dette er en bot ikke en bruker konto. 
        await _client.LoginAsync(TokenType.Bot, token);
        // Starter websocket gateway, som tillater får kommunikasjon
        // Mellom applikasjonene
        await _client.StartAsync();

        await _commands.AddModuleAsync<Commands>(null);

        _client.Ready += () => 
        {
            Console.WriteLine("Bot has been connected");
            return Task.CompletedTask;
        };
        await Task.Delay(-1);
    } 
    
    private static async Task HandleCommandAsync(SocketMessage messageParam)
    {
      if (!(messageParam is SocketUserMessage message) || message.Author.IsBot)
      {
        return;
      } 

      int argPos = 0; 
      if (!(message.HasCharPrefix(_prefix, ref argPos) || 
      message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
      return;
        
        var context = new SocketCommandContext(_client, message);
        await _commands.ExecuteAsync(context: context, argPos: argPos, services: null);
    }

    public static async Task<Task> HandleKeyWordCmds(SocketMessage message)
    {
      if (!(message is SocketUserMessage userMessage) || userMessage.Author.IsBot)      
      return Task.CompletedTask;
      
       Console.WriteLine($"Message received from {userMessage.Author}: {userMessage.Content}");
       string content = userMessage.Content.ToLower();

       if (content == "ping")
       {
         await userMessage.Channel.SendMessageAsync("pong");
       }
         else if (content == "hello")
       {
        await userMessage.Channel.SendMessageAsync("Ok.");
       } 
       else if (content == "help") 
       {
         await userMessage.Channel.SendMessageAsync("Reference #help or open a ticket");
       }
       else if (content.Contains("hi"))
       {
          await userMessage.Channel.SendMessageAsync("Hi");
       }
      
             return Task.CompletedTask;
    }

   private static async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
   {
        var message = await before.GetOrDownloadAsync(); 
       // Sjeker om meldingen er = null, og returnerer en melding til konsollen
       if (message == null)
       {
         Console.WriteLine($"Message Edited: [Kan ikke hente orginal melding] -> \"{after.Content}\"");
         return;
       }

       if (message.Content == after.Content)
       return;
        // Hvilkek kannel meldingen ble redigert i og hvem som redigerte meldingen
        Console.WriteLine($"Message edited in {channel.Name} by {after.Author.Username}:");
        // Før og etter meldingen ble redigert
        Console.WriteLine($"Before: \"{message.Content}\"");
        // Etter meldingen ble redigert
        Console.WriteLine($"After: \"{after.Content}\"");
    }

// Logger og forteller konsollen når en melding er motatt
    private static Task MessageReceived(SocketMessage message)
    {
        // Sjekker om meldingen er sendt av en bot
    if (!(message is SocketUserMessage userMessage) || userMessage.Author.IsBot)
        return Task.CompletedTask;
        
        // Forteller konsollen hvem som sendte meldingen og hva som ble sendt.
    Console.WriteLine($"Message received from {userMessage.Author}");
    return Task.CompletedTask;
    }
}