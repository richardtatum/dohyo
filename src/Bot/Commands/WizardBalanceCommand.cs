using Discord;
using Discord.WebSocket;
using Dohyo.Repositories;
using Microsoft.Extensions.Logging;

namespace Dohyo.Commands;

public class WizardBalanceCommand : SlashCommand
{
    private readonly ILogger<FightEndCommand> _logger;
    private readonly QueryRepository _queryRepository;
    private const ulong _wizardId = 1;

    public WizardBalanceCommand(QueryRepository queryRepository, ILogger<FightEndCommand> logger)
    {
        _queryRepository = queryRepository;
        _logger = logger;
    }


    public override string Name => "wizard";
    public override Task<SlashCommandProperties> BuildCommandAsync() => Task.FromResult(
        new SlashCommandBuilder()
            .WithName(Name)
            .WithDescription("Check The Wizards balance.")
            .Build());

    protected override async Task<Embed> BuildResponseAsync(SocketSlashCommand command)
    {
        _logger.LogInformation("WIZARD :: User {Username} requested The Wizards balance.", command.User.Username);

        var userExists = await _queryRepository.UserExistsAsync(_wizardId);
        if (!userExists)
        {
            _logger.LogError("WIZARD :: Cannot find The Wizard, all is lost");
            return new EmbedBuilder()
                .WithAuthor(command.User)
                .WithTitle("Where Wizard Wallet?")
                .WithDescription($"The Wizard has left the building, all is lost.")
                .WithFooter("What have you done!?")
                .WithColor(Color.Red)
                .WithCurrentTimestamp()
                .Build();
        }

        var balance = await _queryRepository.GetBalanceAsync(_wizardId);
        return  new EmbedBuilder()
            .WithTitle("Wizard Wallet")
            .WithDescription($"The Wizards balance is \u20ab{balance:n0}.")
            .WithFooter("But he isn't materialistic, he just wants to see a good fight.")
            .WithColor(balance > 0 ? Color.Green : Color.Red)
            .WithCurrentTimestamp()
            .Build();
    }
}