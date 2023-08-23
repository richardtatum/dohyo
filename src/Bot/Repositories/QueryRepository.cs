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

    public async Task<int> GetBalanceAsync(long userId)
    {
        using var connection = Database.GetConnection();
        
        return await connection.QueryFirstOrDefaultAsync<int>(
            "select balance from user where id = @userId;", 
            new
            {
                userId
            });
    }
    
    public async Task<long> GetOpenFightIdAsync()
    {
        using var connection = Database.GetConnection();
        
        return await connection.QueryFirstOrDefaultAsync<long>(
            "select id from fight where end_date is null;");
    }
}