using Discord;
using Discord.WebSocket;
using Dohyo.Extensions;
using Dohyo.Repositories;
using Microsoft.Extensions.Logging;

namespace Dohyo.Commands;

public class TipCommand : SlashCommand
{
    private readonly ILogger<TipCommand> _logger;
    private readonly CommandRepository _commandRepository;
    
    public TipCommand(ILogger<TipCommand> logger, CommandRepository commandRepository)
    {
        _logger = logger;
        _commandRepository = commandRepository;
    }

    public override string Name => "tip";
    public override Task<SlashCommandProperties> BuildCommandAsync() => Task.FromResult(
        new SlashCommandBuilder()
            .WithName(Name)
            .WithDescription("Tip the Wizard, he certainly deserves it!")
            .AddOption(
                new SlashCommandOptionBuilder()
                    .WithName("amount")
                    .WithDescription("How much are you going to tip?")
                    .WithRequired(true)
                    .WithType(ApplicationCommandOptionType.Integer)
                )
            .Build()
    );

    protected override async Task<Embed> BuildResponseAsync(SocketSlashCommand command)
    {

        var amount = command.Data.Options.GetAmount();
        if (amount is null || amount <= 0)
        {
            _logger.LogError("BET :: User {Username} failed to tip the wizard. User passed an invalid amount. Amount: {Amount}.", command.User.Username, amount);
            return new EmbedBuilder()
                .WithAuthor(command.User)
                .WithTitle("Tip Failed")
                .WithDescription("You need to specify a valid amount to tip. Valid amounts are positive integers.")
                .WithFooter("You fumbled tipping The Wizard, he probably thinks you're an idiot...")
                .WithColor(Color.Red)
                .WithCurrentTimestamp()
                .Build();
        }

        _logger.LogInformation("TIP :: User {Username} tipped the wizard {Amount:n0}.", command.User.Username, amount);
        await _commandRepository.TipWizardAsync(command.User.Id, amount.Value);
        
        return new EmbedBuilder()
            .WithAuthor(command.User)
            .WithTitle("Tip the Wizard")
            .WithDescription($"You tipped the Wizard \u20ab{amount:n0}! You legend.")
            .WithFooter("The Wizard tips his hat in thanks, what a class act.")
            .WithColor(Color.Gold)
            .WithCurrentTimestamp()
            .Build();
    }
}