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
            if (!ulong.TryParse(Environment.GetEnvironmentVariable("GUILD_ID"), out var guildId))
            {
                throw new ArgumentNullException("GUILD_ID is not set");
            }
            
            var guild = _client.GetGuild(guildId);

            _logger.LogInformation("STARTUP :: Creating slash commands...");
            foreach (var command in _commands)
            {
                var commandProperties= await command.BuildCommandAsync();
                await guild.CreateApplicationCommandAsync(commandProperties);
            }
        }
        catch (HttpException ex)
        {
            _logger.LogWarning("STARTUP :: Failed to create slash command: {Message}", ex.Message);
            var json = JsonSerializer.Serialize(ex.Errors, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            _logger.LogWarning(json);
        }
    }
}