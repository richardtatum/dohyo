using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Dohyo;

public class SlashCommandHandler
{
    private readonly ILogger<SlashCommandHandler> _logger;

    public SlashCommandHandler(ILogger<SlashCommandHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(SocketSlashCommand command)
    {
        return command switch 
        {
            {Data.Name: "bet" } => HandleBetAsync(command),
            {Data.Name: "fight" } => HandleGameStartAsync(command),
            _ => Task.CompletedTask
        };
    }
    
    public Task HandleBetAsync(SocketSlashCommand command)
    {
        _logger.LogInformation("Handling bet command for user {User}", command.User.Username);

        var side = command.Data.Options.First(x => x.Name == "side").Value;
        var amount = command.Data.Options.First(x => x.Name == "amount").Value;
        
        _logger.LogInformation("User {User} bet {Amount} on {Side}", command.User.Username, amount, side);

        var embed = new EmbedBuilder()
            .WithAuthor(command.User)
            .WithTitle("Bet Placed")
            .WithDescription($"Your bet has been placed! You chose {side} and bet {amount}!")
            .WithFooter("Good luck!")
            .WithColor(Color.Green)
            .WithCurrentTimestamp()
            .Build();

        return command.RespondAsync(embed: embed);
    }
     
    public Task HandleGameStartAsync(SocketSlashCommand command)
    {
        _logger.LogInformation("Handling fight command for user {User}", command.User.Username);
        
        var embed = new EmbedBuilder()
            .WithAuthor(command.User)
            .WithTitle("Game Started")
            .WithDescription("The game has started! Ready your bets!")
            .WithFooter("Bets can be placed with /bet")
            .WithColor(Color.Gold)
            .WithCurrentTimestamp()
            .Build();

        return command.RespondAsync(embed: embed);
    }
}