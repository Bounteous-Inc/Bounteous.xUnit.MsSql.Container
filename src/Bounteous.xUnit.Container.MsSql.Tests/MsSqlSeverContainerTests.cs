using System.Threading.Tasks;
using Bounteous.xUnit.Container.MsSql.Containers;
using Bounteous.xUnit.Container.MsSql.Extensions;
using Dapper;
using Xunit;

namespace Bounteous.xUnit.Container.MsSql.Tests;

[Collection("MsSqlSeverContainer")]
public class MsSqlSeverContainerTests : IClassFixture<MsSqlSeverContainer>
{
    private readonly MsSqlSeverContainer container;

    public MsSqlSeverContainerTests(MsSqlSeverContainer container)
    {
        this.container = container;
    }

    [Fact]
    public async Task InitializeContainer()
    {
        await container.InitializeAsync();
        Assert.NotNull(container.Server);
        Assert.False(string.IsNullOrEmpty(container.ConnectionString));
    }

    [Fact]
    public async Task CreateDatabase()
    {
        const string databaseName = "TestDatabase";
        await container.WithDatabase(databaseName);

        Assert.Contains(databaseName, container.ConnectionString);
    }

    [Fact]
    public async Task RunSql()
    {
        const string sql = "CREATE TABLE TestTable (Id INT PRIMARY KEY, Name NVARCHAR(50))";
        await container.RunSql(sql);

        // Verify the table was created
        const string verifySql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TestTable'";
        await using var connection = container.OpenMsSqlConnection();
        var tableCount = await connection.ExecuteScalarAsync<int>(verifySql);

        Assert.Equal(1, tableCount);
    }
}