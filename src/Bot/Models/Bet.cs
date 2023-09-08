namespace Dohyo.Models;

public class Bet
{
    public ulong UserId { get; set; }
    public string Username { get; set; }
    public int Amount { get; set; }
    public Side Side { get; set; }
}