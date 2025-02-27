using Bounteous.xUnit.Container.MsSql.Containers;
using Xunit;

namespace Bounteous.xUnit.Container.MsSql;

[CollectionDefinition("MsSqlSeverContainer")]
public class MsSqlSeverContainerCollection : ICollectionFixture<MsSqlServerContainer>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}