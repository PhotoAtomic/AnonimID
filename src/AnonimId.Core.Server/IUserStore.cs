
namespace AnonimId.Core.Server
{
    public interface IUserStore
    {
        Task<SaltAndVerifier> GetSaltAndVerifier(string username);
        Task<bool> Register(UserSaltAndVerifier userSaltAndVerifier);
    }
}