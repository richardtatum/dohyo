using Dapper;
using Dohyo.Data;
using Dohyo.Models;

namespace Dohyo.Repositories;

public class QueryRepository
{
    public async Task<User[]> GetUsersAsync()
    {
        using var connection = Database.GetConnection();
        var result = await connection.QueryAsync<User>("select * from user;");

        var x = result.ToArray();
        return x;
    }

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
}