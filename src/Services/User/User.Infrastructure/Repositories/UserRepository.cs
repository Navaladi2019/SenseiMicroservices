using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application;
using User.Domain;
using MongoDB.Driver;
using ResponseHandling;

namespace User.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoContext _mongoContext;
        public UserRepository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }
        public async Task AddUser(UserDto user)
        {
          await  _mongoContext.User.InsertOneAsync(user);
        }

        public async Task<UserProfile> GetUserProfileBymailId(string email)
        {

            var filter=  Builders<User.Domain.UserDto>.Filter.Eq(x => x.EmailId, email);
            var projection = Builders<User.Domain.UserDto>.Projection.Include(x => x.UserProfile).Exclude(x=>x.Id);

            var user= await _mongoContext.User.Find(filter).Project<UserDto>(projection)?.FirstOrDefaultAsync();
            return user?.UserProfile;
        }

        public async Task<UserDto> GetUserRolesByMailIsAsync(string email)
        {
            var filter = Builders<User.Domain.UserDto>.Filter.Eq(x => x.EmailId, email);
            var projection = Builders<User.Domain.UserDto>.Projection.Include(x => x.Roles);
            return await _mongoContext.User.Find(filter).Project<UserDto>(projection)?.FirstOrDefaultAsync();
        }


        public async Task<List<string>> GetUserRolesByIdAsync(string Id)
        {
            var filter = Builders<User.Domain.UserDto>.Filter.Eq(x => x.Id, Id);
            var projection = Builders<User.Domain.UserDto>.Projection.Include(x => x.Roles).Exclude(x=>x.Id);
           var user= await _mongoContext.User.Find(filter).Project<UserDto>(projection)?.FirstOrDefaultAsync();
            return user?.Roles;
        }


        public async Task<UserDto> GetUserBymailIdAsync(string email)
        {
            var filter = Builders<User.Domain.UserDto>.Filter.Eq(x => x.EmailId, email);
            return await _mongoContext.User.Find(filter).FirstOrDefaultAsync();
        }


        public async Task<UserDto> GetUserByIdAsync(string Id)
        {
            var filter = Builders<User.Domain.UserDto>.Filter.Eq(x => x.Id, Id);
            return await _mongoContext.User.Find(filter).FirstOrDefaultAsync();
        }

        public async Task ResetForgetPassword(string id, string resetPasswordKey,string password)
        {
            var filter = Builders<User.Domain.UserDto>.Filter.Eq(x => x.Id, id);
            var update = Builders<UserDto>.Update.Set(x => x.UserCredential.SaltedPassword, password).Set(x=>x.UserProfile.IsEmailComfirmed,true);
            var result =   await _mongoContext.User.UpdateOneAsync(filter, update);
            if(result.ModifiedCount == 0)
            {
                throw new CriticalBusinessException("Unable to update password.Our Team has been notified to resolve this issue.");
            }

            return;
        }

       

        
        public async Task<UpdateResult> ResetPassword(string id, string saltedPassword)
        {
            var filter = Builders<User.Domain.UserDto>.Filter.Eq(x => x.Id, id);
            var update = Builders<UserDto>.Update.Set(x => x.UserCredential.SaltedPassword, saltedPassword);
            return await _mongoContext.User.UpdateOneAsync(filter, update);
        }

        public async Task AddRole(string Role,string UserId)
        {
            var filter = Builders<User.Domain.UserDto>.Filter.Eq(x => x.Id, UserId);
            var update = Builders<UserDto>.Update.AddToSet(x => x.Roles, Role);
            await _mongoContext.User.UpdateOneAsync(filter, update);
        }
    }
}
