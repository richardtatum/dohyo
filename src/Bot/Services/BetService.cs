using Dohyo.Models;
using Dohyo.Repositories;
using Microsoft.Extensions.Logging;

namespace Dohyo.Services;

public class BetService
{
    private readonly QueryRepository _query;
    private readonly CommandRepository _command;
    private readonly ILogger<BetService> _logger;
    
    public BetService(QueryRepository query, CommandRepository command, ILogger<BetService> logger)
    {
        _query = query;
        _command = command;
        _logger = logger;
    }

    public async Task<Dictionary<ulong, int>> CalculateWinningsAsync(long fightId, Side winningSide)
    {
        _logger.LogInformation("BET :: Calculating winnings for fight {FightId}.", fightId);
        var bets = await _query.GetAllBetsAsync(fightId);
        _logger.LogInformation("BET :: Found {BetCount} total bets for fight {FightId}.", bets.Length, fightId);
        
        var total = bets.Sum(x => x.Amount);
        _logger.LogInformation("BET :: Total bet amount for fight {FightId} is {Total}.", fightId, total);
        
        var winningBets = bets
            .Where(x => x.Side == winningSide)
            .ToArray();
        _logger.LogInformation("BET :: Found {WinningBetCount} winning bets for fight {FightId}.", winningBets.Length, fightId);

        var winningPot = bets.Where(x => x.Side == winningSide).Sum(x => x.Amount);
        var losingPot = bets.Where(x => x.Side != winningSide).Sum(x => x.Amount);
        
        return CalculateWinnings(winningBets, losingPot, winningPot);
    }

    public Dictionary<ulong, int> CalculateWinnings(IEnumerable<Bet> bets, int losingPot, int winningPot)
    {
        _logger.LogInformation("BET :: Calculating winnings distribution for {BetCount} bets and {Winnings} total.", bets.Count(), losingPot + winningPot);
        
        var winnings = new Dictionary<ulong, int>();
        foreach (var bet in bets)
        {
            // Get the total percentage of the bet amount from the winning pool, thats the percentage of the loosing pool you get
            var betPercentage = (double)bet.Amount / winningPot;
            _logger.LogInformation("BET :: Calculating winnings for user {Username} with bet amount {BetAmount} and percentage {BetPercentage}.", bet.Username, bet.Amount, betPercentage);
            
            var individualWinnings = bet.Amount + (int)Math.Round(betPercentage * losingPot);
            _logger.LogInformation("BET :: Calculated winnings for user {Username} with bet amount {BetAmount} and percentage {BetPercentage} as {IndividualWinnings}.", bet.Username, bet.Amount, betPercentage, individualWinnings);
            winnings.Add(bet.UserId, individualWinnings);
        }

        return winnings;
    }
    
    public async Task DistributeWinningsAsync(long fightId, Side winningSide)
    {
        var calculatedWinnings = await CalculateWinningsAsync(fightId, winningSide);
        if (!calculatedWinnings.Any())
        {
            _logger.LogInformation("BET :: No winnings to distribute for fight {FightId}.", fightId);
            return;
        }
        
        _logger.LogInformation("BET :: Distributing winnings for fight {FightId}.", fightId);
        await DistributeWinningsAsync(calculatedWinnings);
    }
    
    public async Task DistributeWinningsAsync(Dictionary<ulong, int> winnings)
    {
        foreach (var (userId, amount) in winnings)
        {
            _logger.LogInformation("BET :: Distributing winnings for user {UserId} with amount {Amount}.", userId, amount);
            await _command.AddBalanceAsync(userId, amount);
        }
    }
}