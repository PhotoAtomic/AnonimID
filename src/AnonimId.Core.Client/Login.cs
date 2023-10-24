using SecureRemotePassword;
using Superpower.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AnonimId.Core.Client
{



    public class Login
    {
        private readonly string username;
        private readonly string password;
        private readonly IAccountServer channel;
        private readonly SrpClient client;

        public Login(string username, string password, IAccountServer channel)
        {
            this.username = username;
            this.password = password;
            this.channel = channel;
            client = new SrpClient();
        }

        public async Task<IResult> SignUp()
        {
            
            var salt = client.GenerateSalt();
            var privateKey = client.DerivePrivateKey(salt, username, password);
            var verifier = client.DeriveVerifier(privateKey);

            try
            {
                await channel.Register(username, salt, verifier);
                return new Success();
            }
            catch(Exception ex)
            {
                return new Failure(ex);
            }
        }

        public async Task<IResult<string>> SignIn()
        {
            
            //present
            
            var clientEphemeral = client.GenerateEphemeral();

            SaltAndServerEphemeral saltAndServerEphemeral;
            try
            {
                saltAndServerEphemeral = await channel.Present(username, clientEphemeral.Public);
            }
            catch (Exception ex)
            {
                return new Failure<string>(new Exception("Error during presentation phase",ex));
            }

            


            var privateKey = client.DerivePrivateKey(saltAndServerEphemeral.Salt, username, password);
            var clientSession = client.DeriveSession(clientEphemeral.Secret, saltAndServerEphemeral.ServerEphemeral, saltAndServerEphemeral.Salt, username, privateKey);


            //send proof

            Proof serverSessionProof;

            try
            {
                serverSessionProof = await channel.Proof(clientSession.Proof);
            }
            catch(Exception ex)
            {
                return new Failure<string>(new Exception("Error while receiving server proof", ex));
            }


            //final verification
            try
            {
                client.VerifySession(clientEphemeral.Public, clientSession, serverSessionProof.Value);
            }
            catch(Exception ex)
            {
                return new Failure<string>(new Exception("Error during server proof phase", ex));
            }

            return new Success<string>(serverSessionProof.Value);

        }
        

    }
}
