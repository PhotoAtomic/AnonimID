using AnonimId.Core.Client;
using AnonimId.Core.Server;
using NSubstitute;
using SecureRemotePassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonimId.Core.Test
{
    public class SecureRemotePasswordTest
    {
        

        [Fact]
        public async Task SignUpAndSignIn()
        {
            var store = Substitute.For<IUserStore>();
            UserSaltAndVerifier storage = new(string.Empty, string.Empty, string.Empty);

            store.Register(Arg.Any<UserSaltAndVerifier>()).Returns(info =>
            {
                storage = info.Arg<UserSaltAndVerifier>();
                return true;
            });
            store.GetSaltAndVerifier(Arg.Any<string>()).Returns(info=>storage);

            LoginVerifier loginServer = new LoginVerifier(store);
            Login login = new Login("usr1", "passwd1", loginServer);


            var registered = await login.SignUp();
            var session = await login.SignIn();

            Assert.True(registered.IsSuccess);
            Assert.True(session.IsSuccess);

        }


        [Fact]
        public void MockTest()
        {

            var username = "USer1";
            var password = "Pazzwor2";

            var client = new SrpClient();
            var server = new SrpServer();

            // sign up
            var salt = client.GenerateSalt();
            var privateKey = client.DerivePrivateKey(salt, username, password);
            var verifier = client.DeriveVerifier(privateKey);

            // authenticate
            var clientEphemeral = client.GenerateEphemeral();
            var serverEphemeral = server.GenerateEphemeral(verifier);
            var clientSession = client.DeriveSession(clientEphemeral.Secret, serverEphemeral.Public, salt, username, privateKey);
            var serverSession = server.DeriveSession(serverEphemeral.Secret, clientEphemeral.Public, salt, username, verifier, clientSession.Proof);
            client.VerifySession(clientEphemeral.Public, clientSession, serverSession.Proof);

            // both the client and the server have the same session key
            Assert.Equal(clientSession.Key, serverSession.Key);
        }
    }
}
