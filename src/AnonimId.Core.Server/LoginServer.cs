using SecureRemotePassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonimId.Core.Server
{
    public class LoginServer(IUserStore userStore) : IAccountServer
    {

        public string ServerEphemeral => serverEphemeral?.Public ?? String.Empty;
        private SrpEphemeral? serverEphemeral;
        private string? clientEphemeralPublic;
        private string? username;
        private SaltAndVerifier? saltAndVerifier;
        private SrpServer server = new SrpServer();

        public async Task<SaltAndServerEphemeral> Present(string username, string clientEphemeralPublic)
        {
            saltAndVerifier = await userStore.GetSaltAndVerifier(username);
            
            serverEphemeral = server.GenerateEphemeral(saltAndVerifier.Verifier);
            this.clientEphemeralPublic = clientEphemeralPublic;
            this.username = username;
            return new SaltAndServerEphemeral(saltAndVerifier.Salt, serverEphemeral.Public);
        }

        public Task<Proof> Proof(string proof)
        {            
            var serverSession = server.DeriveSession(serverEphemeral?.Secret, clientEphemeralPublic, saltAndVerifier?.Salt, username, saltAndVerifier?.Verifier, proof);
            return Task.FromResult(new Proof(serverSession.Proof));
        }

        public Task<bool> Register(string username, string salt, string verifier)
        {
            return userStore.Register(new UserSaltAndVerifier(username, salt, verifier));
        }
    }
}
