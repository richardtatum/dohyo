using Discord;
using Discord.WebSocket;

namespace Dohyo.Commands;

public class FightCommand : SlashCommand
{
    public override string Name => "fight";

    public override Task<SlashCommandProperties> BuildCommandAsync() => Task.FromResult(
        new SlashCommandBuilder()
        .WithName("fight")
        .WithDescription("Start the game!")
        .WithDefaultMemberPermissions(GuildPermission.Administrator)
        .Build());

    protected override Task<Embed> BuildResponseAsync(SocketSlashCommand command) => Task.FromResult(
        new EmbedBuilder()
            .WithAuthor(command.User)
            .WithTitle("Fight Started")
            .WithDescription("The fight has started! Place your bets!")
            .WithFooter("Bets can be placed with /bet")
            .WithColor(Color.Green)
            .WithCurrentTimestamp()
            .Build());
}