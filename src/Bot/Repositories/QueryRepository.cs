using Dapper;
using Dohyo.Data;
using Dohyo.Models;

namespace Dohyo.Repositories;

public class QueryRepository
{
    public async Task<int> GetBalanceAsync(ulong userId)
    {
        using var connection = Database.GetConnection();
        
        return await connection.QueryFirstOrDefaultAsync<int>(
            "select balance from user where id = @userId;", 
            new
            {
                userId
            });
    }
    
    public async Task<long?> GetOpenFightIdAsync()
    {
        using var connection = Database.GetConnection();
        
        return await connection.QueryFirstOrDefaultAsync<long?>(
            "select id from fight where end_date is null;");
    }
    
    public async Task<bool> HasBetAsync(long fightId, ulong userId)
    {
        using var connection = Database.GetConnection();
        
        return await connection.QueryFirstOrDefaultAsync<bool>(
            "select count(*) from bet where fight_id = @fightId and user_id = @userId;",
            new
            {
                fightId,
                userId
            });
    }

    public async Task<bool> UserExistsAsync(ulong userId)
    {
        using var connection = Database.GetConnection();
        
        return await connection.QueryFirstOrDefaultAsync<bool>(
            "select count(*) from user where id = @userId;",
            new
            {
                userId
            });
    }

    public async Task<Bet[]> GetAllBetsAsync(long fightId)
    {
        using var connection = Database.GetConnection();
        
        var result = await connection.QueryAsync<Bet>(
            "select user_id as userid, amount, side from bet where fight_id = @fightId;",
            new
            {
                fightId
            });
        
        return result.ToArray();
    }
}