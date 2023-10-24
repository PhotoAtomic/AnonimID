using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonimId.Core
{

    public record SessionKey(string Secret);
    public record SaltAndServerEphemeral(string Salt, string ServerEphemeral);
    public record SaltAndVerifier(string Salt, string Verifier);
    public record UserSaltAndVerifier(string User, string Salt, string Verifier) : SaltAndVerifier(Salt, Verifier);
    public record Proof(string Value);



    public interface IAccountServer
    {
        Task<bool> Register(string Username, string Salt, string verifier);
        Task<SaltAndServerEphemeral> Present(string username, string clientEphemeralPublic);
        Task<Proof> Proof(string proof);
    }

}
