using System.Reflection;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Dohyo.Data;

public class Database
{
    public static async Task EnsureCreatedAsync()
    {
        var path = GetDbPath();
        if (File.Exists(path))
        {
            return;
        }
        
        // Bootstrap the database using the provided sql file
        await using var connection = GetConnection();
        await connection.OpenAsync();
        
        var sql = await File.ReadAllTextAsync("Data/dohyo.sql");
        await connection.ExecuteAsync(sql);
    }

    internal static string GetDbPath()
    {
        #if DEBUG
        return "Data/dohyo.db";
        #else
        return Environment.GetEnvironmentVariable("DB_URI");
        #endif
    }
    
    internal static SqliteConnection GetConnection()
    {
        var path = GetDbPath();
        return new SqliteConnection($"Data Source={path}");
    }
}