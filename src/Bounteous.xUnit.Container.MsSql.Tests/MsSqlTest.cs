using System.Threading.Tasks;
using Bounteous.xUnit.Container.MsSql.Containers;
using Bounteous.xUnit.Container.MsSql.Extensions;
using Dapper;
using Xunit;

namespace Bounteous.xUnit.Container.MsSql.Tests
{

    namespace Bounteous.xUnit.Tests
    {
        public class MsSqlSeverContainerTests : IAsyncLifetime
        {
            private readonly MsSqlSeverContainer container;

            public MsSqlSeverContainerTests()
            {
                container = new MsSqlSeverContainer();
            }

            public async Task InitializeAsync()
            {
                await container.InitializeAsync();
            }

            public async Task DisposeAsync()
            {
                await container.DisposeAsync();
            }

            [Fact]
            public async Task ShouldInitializeContainer()
            {
                Assert.NotNull(container.Server);
                Assert.False(string.IsNullOrEmpty(container.ConnectionString));
            }

            [Fact]
            public async Task ShouldCreateDatabase()
            {
                const string databaseName = "TestDatabase";
                await container.WithDatabase(databaseName);

                Assert.Contains(databaseName, container.ConnectionString);
            }

            [Fact]
            public async Task ShouldRunSql()
            {
                const string sql = "CREATE TABLE TestTable (Id INT PRIMARY KEY, Name NVARCHAR(50))";
                await container.RunSql(sql);

                // Verify the table was created
                const string verifySql =
                    "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TestTable'";
                await using var connection = container.OpenMsSqlConnection();
                var tableCount = await connection.ExecuteScalarAsync<int>(verifySql);

                Assert.Equal(1, tableCount);
            }
        }
    }
}