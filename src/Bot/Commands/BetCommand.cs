using Discord;
using Discord.WebSocket;
using Dohyo.Extensions;
using Dohyo.Models;
using Dohyo.Repositories;
using Microsoft.Extensions.Logging;

namespace Dohyo.Commands;

public class BetCommand : SlashCommand
{
    private readonly ILogger<BetCommand> _logger;
    private readonly CommandRepository _commandRepository;
    private readonly QueryRepository _queryRepository;

    public BetCommand(ILogger<BetCommand> logger, CommandRepository commandRepository, QueryRepository queryRepository)
    {
        _logger = logger;
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
    }

    public override string Name => "bet";

    public override Task<SlashCommandProperties> BuildCommandAsync() => Task.FromResult(
        new SlashCommandBuilder()
            .WithName(Name)
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
            .Build());

    protected override async Task<Embed> BuildResponseAsync(SocketSlashCommand command)
    {
        _logger.LogInformation("BET :: User {Username} placed a bet. Getting fight id...", command.User.Username);
        
        var fightId = await _queryRepository.GetOpenFightIdAsync();
        if (fightId is null)
        {
            _logger.LogWarning("BET :: User {Username} failed to place a bet. No fight is currently open.", command.User.Username);
            return new EmbedBuilder()
                .WithAuthor(command.User)
                .WithTitle("Bet Failed")
                .WithDescription("There is no live fight to bet on. Did you start a fight with /fight?")
                .WithFooter("You numpty.")
                .WithColor(Color.Red)
                .WithCurrentTimestamp()
                .Build();
        }
        
        var hasBet = await _queryRepository.HasBetAsync(fightId.Value, command.User.Id);
        if (hasBet)
        {
            _logger.LogWarning("BET :: User {Username} failed to place a bet. User has already placed a bet for this fight.", command.User.Username);
            return new EmbedBuilder()
                .WithAuthor(command.User)
                .WithTitle("Bet Failed")
                .WithDescription("You have already placed a bet on this fight. You cannot change your bet.")
                .WithFooter("You numpty.")
                .WithColor(Color.Red)
                .WithCurrentTimestamp()
                .Build();
        }
        
        var side = command.Data.Options.GetSide();
        if (side is null)
        {
            return new EmbedBuilder()
                .WithAuthor(command.User)
                .WithTitle("Bet Failed")
                .WithDescription($"You need to specify a valid side to bet on. Valid sides are '{Side.Left.ToString()}' and '{Side.Right.ToString()}'.")
                .WithFooter("You numpty.")
                .WithColor(Color.Red)
                .WithCurrentTimestamp()
                .Build();
        }
        
        var amount = command.Data.Options.GetAmount();
        if (amount is null || amount <= 0)
        {
            return new EmbedBuilder()
                .WithAuthor(command.User)
                .WithTitle("Bet Failed")
                .WithDescription("You need to specify a valid amount to bet. Valid amounts are positive integers.")
                .WithFooter("You numpty.")
                .WithColor(Color.Red)
                .WithCurrentTimestamp()
                .Build();
        }

        _logger.LogInformation("BET :: User {Username} placed a bet. FightId: {Id}. Side: {Side} Amount: {Amount}.", fightId, command.User.Username, side, amount);
        await _commandRepository.AddBetAsync(fightId.Value, command.User.Id, side.Value, amount.Value);

        return new EmbedBuilder()
            .WithAuthor(command.User)
            .WithTitle("Bet Placed")
            .WithDescription($"Your bet has been placed! You chose {side} and bet {amount}!")
            .WithFooter("Good luck!")
            .WithColor(Color.Green)
            .WithCurrentTimestamp()
            .Build();
    }
}