using Discord;
using Discord.Commands;
using Microsoft.Extensions.Logging;

namespace Dohyo;

public class LoggingService
{
    private readonly ILogger<LoggingService> _logger;
    
    public LoggingService(ILogger<LoggingService> logger)
    {
        _logger = logger;
    }

    public Task LogAsync(LogMessage message)
    {
        if (message.Exception is CommandException cmdException)
        {
            _logger.LogWarning($"[Command/{message.Severity}] {cmdException.Command.Aliases.First()}"
                              + $" failed to execute in {cmdException.Context.Channel}.");
            _logger.LogWarning(cmdException.ToString());
        }
        else
        {
            _logger.LogWarning($"[General/{message.Severity}] {message}");
        }

        return Task.CompletedTask;
    }
}