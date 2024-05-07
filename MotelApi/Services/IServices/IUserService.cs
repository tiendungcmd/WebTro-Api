using MotelApi.Models;
using MotelApi.Request;
using MotelApi.Response;

namespace MotelApi.Services.IServices
{
    public interface IUserService : IServiceCommon<User>
    {
        public User Login(string username, string password);

        public Task<UserResponse> GetProfile(string username);
        public Task<bool> UpdateProfile(UserRequest request);

        public Task<List<User>> GetUsers();
        public Task<bool> LockUser(Guid userId);
        public Task<bool> ResetPassword(Guid userId);

    }
}
