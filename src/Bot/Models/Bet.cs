namespace Dohyo.Models;

public class Bet
{
    public ulong UserId { get; set; }
    public int Amount { get; set; }
    public Side Side { get; set; }
}