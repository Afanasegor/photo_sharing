using PhotoSharing.Abstractions.Models.Auth;

namespace PhotoSharing.Abstractions.Interfaces.Auth
{
    public interface IAccountService
    {
        Task SignUp(RegistrationData registrationData);
    }
}
