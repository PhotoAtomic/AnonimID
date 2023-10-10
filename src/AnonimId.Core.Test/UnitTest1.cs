
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace AnonimId.Core.Test
{
    public class DatabaseTest : IClassFixture<SurrealDBContainer>
    {
        private readonly AnonimId.Core.Test.SurrealDBContainer _surrealDBContainer;

        public DatabaseTest(AnonimId.Core.Test.SurrealDBContainer surrealDBContainer)
        {
            _surrealDBContainer = surrealDBContainer;
        }

        [Fact]
        public void Test1()
        {
            int i = 0;
        }
        [Fact]
        public void Test2()
        {
            int i = 0;
        }
        [Fact]
        public void Test3()
        {
            int i = 0;
        }
    }
}