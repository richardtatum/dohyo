using Discord;
using Discord.WebSocket;
using Dohyo.Repositories;
using Microsoft.Extensions.Logging;

namespace Dohyo.Commands;

public class FightStartCommand : SlashCommand
{
    private readonly CommandRepository _commandRepository;
    private readonly ILogger<FightStartCommand> _logger;

    public FightStartCommand(CommandRepository commandRepository, ILogger<FightStartCommand> logger)
    {
        _commandRepository = commandRepository;
        _logger = logger;
    }

    public override string Name => "fight";

    public override Task<SlashCommandProperties> BuildCommandAsync() => Task.FromResult(
        new SlashCommandBuilder()
        .WithName(Name)
        .WithDescription("Start the game!")
        .WithDefaultMemberPermissions(GuildPermission.Administrator)
        .Build());

    protected override async Task<Embed> BuildResponseAsync(SocketSlashCommand command)
    {
        _logger.LogInformation("User {Username} commenced a fight.", command.User.Username);
        await _commandRepository.StartFightAsync();
        
        return new EmbedBuilder()
            .WithAuthor(command.User)
            .WithTitle("Fight Started")
            .WithDescription("The fight has started! Place your bets!")
            .WithFooter("Bets can be placed with /bet")
            .WithColor(Color.Green)
            .WithCurrentTimestamp()
            .Build();
    }

}