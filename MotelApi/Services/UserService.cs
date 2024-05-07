using Microsoft.EntityFrameworkCore;
using MotelApi.DBContext;
using MotelApi.Models;
using MotelApi.Request;
using MotelApi.Response;
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

            var userRole = new UserRole();
            userRole.UserId = user.Id;
            userRole.RoleId = new Guid("55d19a48-cc09-4725-82b0-0e5b74c84634");

            await _context.UserRoles.AddAsync(userRole);
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

        public async Task<UserResponse> GetProfile(string username)
        {
            var result = new UserResponse();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            var userDetail = await _context.UserDetails.FirstOrDefaultAsync(x => x.Id == user.UserDetailId);
            if (user != null)
            {
                result.Id = user.Id;
                result.UserName = username;
                result.IsActive = (bool)user.IsActive;
                result.Name = user.Name;
                if (userDetail != null)
                {
                    result.Sex = userDetail.Sex;
                    result.Phone = (int)userDetail.Phone;
                    result.CCCD = (int)userDetail.CCCD;
                    result.BirthDay = (DateTime)userDetail.BirthDay;
                }

            }
            return result;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.Where(x => x.UserName != "admin").ToListAsync();
        }

        public async Task<bool> LockUser(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            user.IsActive = !user.IsActive;
            _context.Users.Update(user);
            _context.SaveChanges();

            return user != null;
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

        public async Task<bool> ResetPassword(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            user.PasswordHash = "1";
            _context.SaveChanges();
            return user != null;
        }

        public Task<User> Update(User model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateProfile(UserRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);
            user.Name = request.Name;


            var userDetailExist = await _context.UserDetails.FirstOrDefaultAsync(x => x.Id == user.UserDetailId);
            if (userDetailExist != null)
            {
                userDetailExist.Sex = request.Sex;
                userDetailExist.Phone = request.Phone;
                userDetailExist.CCCD = request.CCCD;
                userDetailExist.BirthDay = request.BirthDay;
                _context.UserDetails.Update(userDetailExist);
            }
            else
            {
                var userDetail = new UserDetail();
                userDetail.Id = Guid.NewGuid();
                userDetail.Sex = request.Sex;
                userDetail.Phone = request.Phone;
                userDetail.CCCD = request.CCCD;
                userDetail.BirthDay = request.BirthDay;
                _context.UserDetails.Add(userDetail);

                user.UserDetailId = userDetail.Id;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
            return user != null;
        }

        Task<List<User>> IServiceCommon<User>.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
