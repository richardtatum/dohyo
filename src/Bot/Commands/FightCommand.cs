using Discord;
using Discord.WebSocket;

namespace Dohyo.Commands;

public class FightCommand : ICommand
{
    public string Name => "fight";

    public SlashCommandProperties BuildCommand() => new SlashCommandBuilder()
        .WithName("fight")
        .WithDescription("Start the game!")
        .WithDefaultMemberPermissions(GuildPermission.Administrator)
        .Build();

    public Embed BuildResponse(SocketSlashCommand command) => new EmbedBuilder()
        .WithAuthor(command.User)
        .WithTitle("Game Started")
        .WithDescription("The game has started! Ready your bets!")
        .WithFooter("Bets can be placed with /bet")
        .WithColor(Color.Gold)
        .WithCurrentTimestamp()
        .Build();

    public Task RespondAsync(SocketSlashCommand command)
    {
        var response = BuildResponse(command);
        return command.RespondAsync(embed: response);
    }
}