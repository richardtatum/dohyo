using System.Net.Mail;
using System.Text.Json;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Dohyo;

public class SlashCommandBuilderService
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<SlashCommandBuilderService> _logger;

    public SlashCommandBuilderService(DiscordSocketClient client, ILogger<SlashCommandBuilderService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task OnReadyAsync()
    {
        var guild = _client.GetGuild(492262151084572682);

        // Create any commands that may be missing
        var command = new SlashCommandBuilder()
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
        
        var fightCommand = new SlashCommandBuilder()
            .WithName("fight")
            .WithDescription("Start the game!")
            .WithDefaultMemberPermissions(GuildPermission.Administrator)
            .Build();

        try
        {
            _logger.LogInformation("Creating slash command");
            await guild.CreateApplicationCommandAsync(command);
            await guild.CreateApplicationCommandAsync(fightCommand);
        }
        catch (HttpException ex)
        {
            _logger.LogWarning("Failed to create slash command");
            var json = JsonSerializer.Serialize(ex.Errors, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            _logger.LogWarning(json);
        }
    }
}