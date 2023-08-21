using Discord;
using Discord.WebSocket;

namespace Dohyo.Commands;

public interface ICommand
{
    public string Name { get; }
    public SlashCommandProperties BuildCommand();
    public Embed BuildResponse(SocketSlashCommand command);

    public Task RespondAsync(SocketSlashCommand command);
}