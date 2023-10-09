using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace AnonimId.Core.Test
{
    public class SurrealDBContainer : IAsyncLifetime
    {


        
        public string Username { get; private set; }
        public string Password { get; private set; }
        public ushort HostPort { get; private set; }

        IContainer surrealDbContainer = null!;


        public SurrealDBContainer()
        {

            Username = RandomString.Create(10);
            Password = RandomString.Create(10);
        }

        

        public async Task InitializeAsync()
        {

            surrealDbContainer = new ContainerBuilder().WithImage("surrealdb/surrealdb:latest")
            .WithPortBinding(8000, true)
            .WithCommand("start", "--auth", "--user", Username, "--pass", Password)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8000)))
            .Build();

            await surrealDbContainer.StartAsync();

            HostPort = surrealDbContainer.GetMappedPublicPort(8000);

        }

        public async Task DisposeAsync()
        {
            await surrealDbContainer.StopAsync();

        }
    }
}
