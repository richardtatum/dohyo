using Discord;
using Discord.WebSocket;

namespace Dohyo;

public class SlashCommandHandler
{
    public async Task HandleAsync(SocketSlashCommand command)
    {
        var side = command.Data.Options.First(x => x.Name == "side").Value;
        var amount = command.Data.Options.First(x => x.Name == "amount").Value;
        
        var embed = new EmbedBuilder()
            .WithAuthor(command.User)
            .WithTitle("Bet Placed")
            .WithDescription($"Your bet has been placed! You chose {side} and bet {amount}!")
            .WithColor(Color.Green)
            .WithCurrentTimestamp()
            .Build();

        await command.RespondAsync(embed: embed);
    }
}