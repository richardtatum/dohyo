using Discord;
using Discord.WebSocket;
using Dohyo.Extensions;
using Dohyo.Models;
using Dohyo.Repositories;
using Dohyo.Services;
using Microsoft.Extensions.Logging;

namespace Dohyo.Commands;

public class FightEndCommand : SlashCommand
{
    private readonly ILogger<FightEndCommand> _logger;
    private readonly CommandRepository _commandRepository;
    private readonly QueryRepository _queryRepository;
    private readonly BetService _betService; // TODO: Move other queries to this service

    public FightEndCommand(ILogger<FightEndCommand> logger, CommandRepository commandRepository, QueryRepository queryRepository, BetService betService)
    {
        _logger = logger;
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
        _betService = betService;
    }

    public override string Name => "end";

    public override Task<SlashCommandProperties> BuildCommandAsync() => Task.FromResult(
        new SlashCommandBuilder()
            .WithName(Name)
            .WithDescription("End the game!")
            .AddOption(
                new SlashCommandOptionBuilder()
                    .WithName("winner")
                    .WithDescription("Which side won?")
                    .WithRequired(true)
                    .AddChoice("Left", "left")
                    .AddChoice("Right", "right")
                    .WithType(ApplicationCommandOptionType.String)
                )
            .WithDefaultMemberPermissions(GuildPermission.Administrator)
            .Build());

    // Winners always get their bet back
    // Losers lose their bet
    // Winners get a share of an arbitrary pot
    // How to set pot size?
    protected override async Task<Embed> BuildResponseAsync(SocketSlashCommand command)
    {
        _logger.LogInformation("FIGHT :: User {Username} ended a fight. Getting fight id...", command.User.Username);

        var fightId = await _queryRepository.GetOpenFightIdAsync();
        if (fightId is null)
        {
            _logger.LogWarning("FIGHT :: User {Username} failed to end a fight. No fight is currently open.", command.User.Username);
            return new EmbedBuilder()
                .WithAuthor(command.User)
                .WithTitle("Fight End Failed")
                .WithDescription($"No fight is currently live. Did you start a fight with /fight?")
                .WithFooter("You numpty.")
                .WithColor(Color.Red)
                .WithCurrentTimestamp()
                .Build();
        }

        var winningSide = command.Data.Options.GetSide("winner");
        if (winningSide is null)
        {
            _logger.LogWarning("FIGHT :: User {Username} failed to end a fight. Invalid side specified.", command.User.Username);
            return new EmbedBuilder()
                .WithAuthor(command.User)
                .WithTitle("Fight End Failed")
                .WithDescription($"You need to specify a valid side to win. Valid sides are '{Side.Left.ToString()}' and '{Side.Right.ToString()}'.")
                .WithFooter("You numpty.")
                .WithColor(Color.Red)
                .WithCurrentTimestamp()
                .Build();
        }
        
        _logger.LogInformation("FIGHT :: Ending fight. Id: {Id}. Winner: {Winner}", fightId, winningSide.Value.ToString());
        await _commandRepository.EndFightAsync(fightId.Value, winningSide.Value);
        
        _logger.LogInformation("FIGHT :: Distributing winnings for fight {Id}", fightId);
        await _betService.DistributeWinningsAsync(fightId.Value, winningSide.Value);
        
        return new EmbedBuilder()
            .WithAuthor(command.User)
            .WithTitle("Fight Ended")
            .WithDescription($"The fight has ended! The winner is {winningSide.Value.ToString()}!")
            .WithFooter("Congratulations to the winners!")
            .WithColor(Color.Green)
            .WithCurrentTimestamp()
            .Build();
    }
}