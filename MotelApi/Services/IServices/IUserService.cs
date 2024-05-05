using MotelApi.Models;

namespace MotelApi.Services.IServices
{
    public interface IUserService:IServiceCommon<User>
    {
        public User Login(string username, string password);
    }
}
