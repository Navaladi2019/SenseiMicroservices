using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepository;
using User.Domain;

namespace User.Infrastructure.Repositories
{
   public class ResetPasswordRepository : IResetPasswordRepository
    {
        private readonly IMongoContext _mongoContext;
        public ResetPasswordRepository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task Add(ResetPassword resetPassword)
        {
           await _mongoContext.ResetPassword.InsertOneAsync(resetPassword);
        }

        public async Task<ResetPassword> GetResetPasswordByUserIdAndKey(string UserId, string Key)
        {
            var UserIdfilter = Builders<ResetPassword>.Filter.Eq(x => x.UserId, UserId);
            var Keyfilter = Builders<ResetPassword>.Filter.Eq(x => x.ResetPasswordKey, Key);
            var filter = Builders<ResetPassword>.Filter.And(UserIdfilter, Keyfilter);
            return await _mongoContext.ResetPassword.Find(filter).FirstOrDefaultAsync();
        }
    }
}
