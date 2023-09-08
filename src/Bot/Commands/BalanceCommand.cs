using Discord;
using Discord.WebSocket;
using Dohyo.Repositories;
using Microsoft.Extensions.Logging;

namespace Dohyo.Commands;

public class BalanceCommand : SlashCommand
{
    private readonly ILogger<BalanceCommand> _logger;
    private readonly QueryRepository _queryRepository;
    private readonly CommandRepository _commandRepository;

    public BalanceCommand(ILogger<BalanceCommand> logger, QueryRepository queryRepository, CommandRepository commandRepository)
    {
        _logger = logger;
        _queryRepository = queryRepository;
        _commandRepository = commandRepository;
    }

    public override string Name => "balance";
    public override Task<SlashCommandProperties> BuildCommandAsync() => Task.FromResult(
        new SlashCommandBuilder()
            .WithName(Name)
            .WithDescription("Show your balance.")
            .Build()
        );
    protected override async Task<Embed> BuildResponseAsync(SocketSlashCommand command)
    {
        _logger.LogInformation("BALANCE :: User {Username} requested their balance.", command.User.Username);

        var userExists = await _queryRepository.UserExistsAsync(command.User.Id);
        if (!userExists)
        {
            _logger.LogWarning("BALANCE :: User {Username} does not exist. Creating user...", command.User.Username);
            await _commandRepository.AddUserAsync(command.User.Id, command.User.Username);
            return GetBalanceEmbed(command.User, 1000); // Default balance is currently 1000. May wish to change this to a config value.
        }
        
        var balance = await _queryRepository.GetBalanceAsync(command.User.Id);
        return GetBalanceEmbed(command.User, balance);
    }

    private Embed GetBalanceEmbed(SocketUser user, int balance) => new EmbedBuilder()
        .WithAuthor(user)
        .WithTitle("Balance")
        .WithDescription($"Your balance is \u20ab{balance:n0}.")
        .WithFooter("But is it enough?")
        .WithColor(balance > 0 ? Color.Green : Color.Red)
        .WithCurrentTimestamp()
        .Build();
}