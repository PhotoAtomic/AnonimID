
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace AnonimId.Core.Test
{
    public class DatabaseTest : IAsyncLifetime
    {
        string username = "user";
        string password = "pa$$";
        IContainer surrealDbContainer= null!;




        public async Task InitializeAsync()
        {
            

            surrealDbContainer = new ContainerBuilder().WithImage("surrealdb/surrealdb:latest")
            .WithPortBinding(8000, true)            
            .WithCommand("start", "--auth", "--user", username, "--pass", password)            
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8000)))
            .Build();

            await surrealDbContainer.StartAsync();

            var hostPort =surrealDbContainer.GetMappedPublicPort(8000);

        }

        public async Task DisposeAsync()
        {
            await surrealDbContainer.StopAsync();

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