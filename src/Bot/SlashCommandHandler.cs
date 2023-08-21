using Discord;
using Discord.WebSocket;
using Dohyo.Commands;
using Microsoft.Extensions.Logging;

namespace Dohyo;

public class SlashCommandHandler
{
    private readonly ILogger<SlashCommandHandler> _logger;
    private readonly IEnumerable<ICommand> _commands;

    public SlashCommandHandler(ILogger<SlashCommandHandler> logger, IEnumerable<ICommand> commands)
    {
        _logger = logger;
        _commands = commands;
    }

    public Task HandleAsync(SocketSlashCommand command) => _commands
        .Where(x => x.Name == command.Data.Name)
        .Select(x =>
        {
            _logger.LogInformation("Handling command {Command} for user {User}", command.Data.Name, command.User.Username);
            return x.RespondAsync(command);
        })
        .FirstOrDefault() ?? Task.CompletedTask;
}