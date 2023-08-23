using Discord;
using Discord.WebSocket;

namespace Dohyo.Commands;

public abstract class SlashCommand
{
    public abstract string Name { get; }
    public abstract Task<SlashCommandProperties> BuildCommandAsync();
    protected abstract Task<Embed> BuildResponseAsync(SocketSlashCommand command);
    public virtual async Task RespondAsync(SocketSlashCommand command)
    {
        var response = await BuildResponseAsync(command);
        await command.RespondAsync(embed: response);
    }
}