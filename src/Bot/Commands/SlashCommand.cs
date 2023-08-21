using Discord;
using Discord.WebSocket;

namespace Dohyo.Commands;

public abstract class SlashCommand
{
    public abstract string Name { get; }
    public abstract SlashCommandProperties BuildCommand();
    protected abstract Embed BuildResponse(SocketSlashCommand command);
    public virtual Task RespondAsync(SocketSlashCommand command)
    {
        var response = BuildResponse(command);
        return command.RespondAsync(embed: response);
    }
}