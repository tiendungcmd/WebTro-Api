using MotelApi.DBContext;
using MotelApi.Models;
using MotelApi.Services.IServices;

namespace MotelApi.Services
{
    public class UserService : IUserService
    {
        public readonly MotelContext _context;
        public UserService(MotelContext context)
        {
            _context = context;
        }
        public async Task<User> Create(User user)
        {
            var result = _context.Users.FirstOrDefault(x => x.UserName == user.UserName);
            if (result != null) { return null; }
            await _context.Users.AddAsync(user);
            _context.SaveChanges();
            return user;
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public User Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName == username);
            if (user != null && user.PasswordHash == password)
            {
                return user;
            }
            return null;
        }

        public Task<User> Update(User model)
        {
            throw new NotImplementedException();
        }

        Task<List<User>> IServiceCommon<User>.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
