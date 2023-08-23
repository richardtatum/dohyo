using Discord.WebSocket;
using Dohyo.Models;

namespace Dohyo.Extensions;

public static class ReadOnlyCollectionExtensions
{
    
    public static string? GetValue(this IReadOnlyCollection<SocketSlashCommandDataOption> collection, string name)
    {
        return collection.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))?.Value?.ToString();
    }
    
    public static Side? GetSide(this IReadOnlyCollection<SocketSlashCommandDataOption> collection, string name = "side")
    {
        return Enum.TryParse(typeof(Side), collection.GetValue(name), true, out var side) ? (Side)side : null;
    }
    
    public static int? GetAmount(this IReadOnlyCollection<SocketSlashCommandDataOption> collection)
    {
        return int.TryParse(collection.GetValue("amount"), out var amount) ? amount : null;
    }
    
}