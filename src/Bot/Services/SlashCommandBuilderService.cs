using System.Text.Json;
using Discord.Net;
using Discord.WebSocket;
using Dohyo.Commands;
using Microsoft.Extensions.Logging;

namespace Dohyo.Services;

public class SlashCommandBuilderService
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<SlashCommandBuilderService> _logger;
    private readonly IEnumerable<SlashCommand> _commands;

    public SlashCommandBuilderService(DiscordSocketClient client, ILogger<SlashCommandBuilderService> logger, IEnumerable<SlashCommand> commands)
    {
        _client = client;
        _logger = logger;
        _commands = commands;
    }

    public async Task OnReadyAsync()
    {
        try
        {
            // Get this from a config file?
            var guild = _client.GetGuild(492262151084572682);

            _logger.LogInformation("Creating slash command");
            foreach (var command in _commands)
            {
                await guild.CreateApplicationCommandAsync(command.BuildCommand());
            }
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