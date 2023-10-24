
using AnonimId.Core.Client;
using AnonimId.Core.Server;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using NSubstitute;

namespace AnonimId.Core.Test
{
    public class DatabaseTest : IClassFixture<SurrealDBContainer>
    {
        private readonly AnonimId.Core.Test.SurrealDBContainer _surrealDBContainer;

        public DatabaseTest(AnonimId.Core.Test.SurrealDBContainer surrealDBContainer)
        {
            _surrealDBContainer = surrealDBContainer;
        }


       
      
    }
}