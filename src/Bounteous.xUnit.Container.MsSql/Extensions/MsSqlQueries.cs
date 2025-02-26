using System.Data;
using System.Threading.Tasks;
using Bounteous.xUnit.Accelerator.Containers;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Bounteous.xUnit.Container.MsSql.Extensions;

public static class MsSqlQueries
{
    public static async Task CreateDatabaseIfNotExists(this IDbConnection connection, string databaseName)
    {
        var sql =
            $"""
               IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{databaseName}') 
                 BEGIN
                   CREATE DATABASE [{databaseName}]
                 END;
             """;
        await connection.ExecuteAsync(sql);
    }

    public static SqlConnection OpenMsSqlConnection(this ISqlContainer container)
    {
        var connection = new SqlConnection(container.ConnectionString);
        connection.Open();
        return connection;
    }
}