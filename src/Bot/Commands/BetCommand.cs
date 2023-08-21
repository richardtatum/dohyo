using Discord;
using Discord.WebSocket;

namespace Dohyo.Commands;

public class BetCommand : ICommand
{
    public string Name => "bet";

    public SlashCommandProperties BuildCommand() => new SlashCommandBuilder()
        .WithName("bet")
        .WithDescription("Place your bets!")
        .AddOptions(new[]
        {
            new SlashCommandOptionBuilder()
                .WithName("side")
                .WithDescription("Which side are you going to bet on?")
                .WithRequired(true)
                .AddChoice("Left", "left")
                .AddChoice("Right", "right")
                .WithType(ApplicationCommandOptionType.String),
            new SlashCommandOptionBuilder()
                .WithName("amount")
                .WithDescription("How much are you going to bet?")
                .WithRequired(true)
                .WithType(ApplicationCommandOptionType.Integer)
        })
        .Build();

    public Embed BuildResponse(SocketSlashCommand command)
    {
        var side = command.Data.Options.First(x => x.Name == "side").Value;
        var amount = command.Data.Options.First(x => x.Name == "amount").Value;
        
        return new EmbedBuilder()
            .WithAuthor(command.User)
            .WithTitle("Bet Placed")
            .WithDescription($"Your bet has been placed! You chose {side} and bet {amount}!")
            .WithFooter("Good luck!")
            .WithColor(Color.Green)
            .WithCurrentTimestamp()
            .Build();
    }

    public Task RespondAsync(SocketSlashCommand command)
    {
        var response = BuildResponse(command);
        return command.RespondAsync(embed: response);
    }
}