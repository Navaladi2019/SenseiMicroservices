using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;

namespace User.Application
{
   public interface IUserRepository
    {
        public Task<UserProfile> GetUserProfileBymailId(string email);

        public Task<UserDto> GetUserBymailIdAsync(string email);

        public Task<UserDto> GetUserByIdAsync(string Id);
        public  Task AddUser(UserDto user);
        Task ResetForgetPassword(string id, string resetPasswordKey,string password);
        Task<UpdateResult> ResetPassword(string id, string saltedPassword);

        Task<UserDto> GetUserRolesByMailIsAsync(string email);

        Task<List<string>> GetUserRolesByIdAsync(string Id);
        Task AddRole(string tutor,string UserId);
    }
}
