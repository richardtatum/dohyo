using Discord;
using Discord.WebSocket;

namespace Dohyo.Commands;

public class FightCommand : SlashCommand
{
    public override string Name => "fight";

    public override SlashCommandProperties BuildCommand() => new SlashCommandBuilder()
        .WithName("fight")
        .WithDescription("Start the game!")
        .WithDefaultMemberPermissions(GuildPermission.Administrator)
        .Build();

    protected override Embed BuildResponse(SocketSlashCommand command) => new EmbedBuilder()
        .WithAuthor(command.User)
        .WithTitle("Game Started")
        .WithDescription("The game has started! Ready your bets!")
        .WithFooter("Bets can be placed with /bet")
        .WithColor(Color.Gold)
        .WithCurrentTimestamp()
        .Build();
}