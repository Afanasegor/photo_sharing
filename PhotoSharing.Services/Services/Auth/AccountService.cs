using Microsoft.EntityFrameworkCore;
using PhotoSharing.Abstractions.Interfaces.Auth;
using PhotoSharing.Abstractions.Models.Auth;
using PhotoSharing.Core.Interfaces.Repositories;
using PhotoSharing.Core.Models.Auth;

namespace PhotoSharing.Services.Services.Auth
{
    public class AccountService : IAccountService
    {
        private const string UserRole = "user";

        private readonly IUserRepository _usersRepository;
        private readonly IBaseRepository<Role> _rolesRepository;

        public AccountService(IUserRepository usersRepository, IBaseRepository<Role> rolesRepository)
        {
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
        }

        public async Task SignUp(RegistrationData registrationData)
        {
            if (registrationData == null)
                throw new ArgumentException("Input data can't be null");

            if (string.IsNullOrWhiteSpace(registrationData.Name) ||
                string.IsNullOrWhiteSpace(registrationData.Email) || 
                string.IsNullOrWhiteSpace(registrationData.Password))
            {
                throw new ArgumentException("One of parameters is not filled (name / email / password)");
            }

            var isEmailTaken = await _usersRepository.Get().AnyAsync(u => u.Email == registrationData.Email);

            if (isEmailTaken)
                throw new ArgumentException("Email has already been taken");

            var user = await _usersRepository.Create(new() { Name = registrationData.Name, Email = registrationData.Email });
            await _usersRepository.SetPassword(user, registrationData.Password);

            user.RoleId = (await _rolesRepository.Get().FirstOrDefaultAsync(r => r.Name == UserRole)).Id;

            await _usersRepository.Update(user);
        }
    }
}
