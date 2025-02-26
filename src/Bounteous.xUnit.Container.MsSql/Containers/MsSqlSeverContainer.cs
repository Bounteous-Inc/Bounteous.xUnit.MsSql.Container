using System.Threading.Tasks;
using Bounteous.Core.Validations;
using Bounteous.xUnit.Accelerator.Containers;
using Bounteous.xUnit.Container.MsSql.Extensions;
using Dapper;
using Testcontainers.MsSql;
using Xunit;

namespace Bounteous.xUnit.Container.MsSql.Containers;

public class MsSqlSeverContainer : IMsSqlContainer, IAsyncLifetime
{
    public MsSqlContainer Server { get; private set; } = null!;
    public string ConnectionString { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        Server = new MsSqlBuilder()
            .WithPassword("Welcome@123.") // Replace with a strong password
            .Build();
        await Server.StartAsync();
        ConnectionString = Server.GetConnectionString();
    }

    public async Task DisposeAsync()
        => await Server.DisposeAsync();

    public async Task<ISqlContainer> WithDatabase(string databaseName)
    {
        Validate.Begin().IsNotEmpty(databaseName, nameof(databaseName)).Check();
        await using var connection = this.OpenMsSqlConnection();
        await connection.CreateDatabaseIfNotExists(databaseName);
        ConnectionString = ConnectionString.Replace("master", databaseName);
        ConnectionStringProvider.Configure(ConnectionString);
        return this;
    }

    public async Task<ISqlContainer> RunSql(string sql)
    {
        await using var connection = this.OpenMsSqlConnection();
        await connection.ExecuteAsync(sql);
        return this;
    }
}