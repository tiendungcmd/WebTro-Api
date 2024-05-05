using MotelApi.Models;

namespace MotelApi.Repository
{
    public interface IUserRepository
    {
        public User CreateUser(User user);
        public User DeleteUser(User user);
    }
}
