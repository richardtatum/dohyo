using Dapper;
using Dohyo.Data;
using Dohyo.Models;

namespace Dohyo.Repositories;

public class CommandRepository
{
    public async Task AddUserAsync(ulong userId, string username)
    {
        using var connection = Database.GetConnection();
        
        await connection.ExecuteAsync(
            "insert into user (id, username) values (@userId, @username);", 
            new
            {
                userId, 
                username
            });
    }

    public async Task StartFightAsync()
    {
        using var connection = Database.GetConnection();
        await connection.ExecuteAsync("insert into fight (start_date) values (datetime('now'));");
    }

    public async Task EndFightAsync(long fightId, Side winningSide)
    {
        using var connection = Database.GetConnection();
        
        // Mark the fight as ended and set the winning side
        await connection.ExecuteAsync(
            "update fight set end_date = datetime('now'), winning_side = @side where id = @fightId;",
            new
            {
                fightId,
                side = winningSide.ToString()
            });
        
        // Mark all records of correct bets as won
        await connection.ExecuteAsync(
            "update bet set winning_bet = 1 where fight_id = @fightId and side = @side;",
            new
            {
                fightId,
                side = winningSide.ToString()
            });
    }

    public async Task AddBetAsync(long fightId, ulong userId, Side side, int amount)
    {
        using var connection = Database.GetConnection();
        
        // Remove the amount from the user's balance
        await connection.ExecuteAsync(
            "update user set balance = balance - @amount where id = @userId;",
            new
            {
                userId,
                amount
            });
        
        // Add the bet to the database
        await connection.ExecuteAsync(
            "insert into bet (fight_id, user_id, side, amount) values (@fightId, @userId, @side, @amount);", 
            new
            {
                fightId, 
                userId,
                side = side.ToString(),
                amount
            });
    }
}