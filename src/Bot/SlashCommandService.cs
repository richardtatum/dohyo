using System.Text.Json;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Dohyo;

public class SlashCommandService
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<SlashCommandService> _logger;

    public SlashCommandService(DiscordSocketClient client, ILogger<SlashCommandService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task OnReadyAsync()
    {
        var guild = _client.GetGuild(492262151084572682);

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

        try
        {
            _logger.LogInformation("Creating slash command");
            await guild.CreateApplicationCommandAsync(command);
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