using Domain.DTO;
using Domain.Entities;
using Domain.Ports.Repositories;
using Domain.Ports.Services;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
        public UserService(IBaseRepository<User> userRepository)
        {
            _userRepository = userRepository;        
        }

        public async Task AddUser(User user)
        {
            user.AddUser();
            await _userRepository.InsertOneAsync(user);
        }
        public async Task<User> GetUser(LoginDTO login)
        {
            var user = await _userRepository.FindOneAsync(u => u.Username == login.UserName || u.Email == login.UserName);
            if (user is not null)
            {
                if (BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
                    return user;
                else
                    throw new Exception("Verifica Senha");
            }
            else
                throw new Exception("Verifica Usuario");

        }

    }
}
