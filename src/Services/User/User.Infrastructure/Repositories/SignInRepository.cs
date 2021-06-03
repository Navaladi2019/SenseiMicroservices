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
    public class SignInRepository : ISignInRepository
    {
        private readonly IMongoContext _mongoContext;
        public SignInRepository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }
        public async Task InsertSignIn(string AuthSettingId, string userId, string jti, DateTime LoggedIn,DateTime expiresAt)
        {
             FieldDefinition<UserSignIn,Token> token = "Tokens";
             var filter = Builders<UserSignIn>.Filter.And(Builders<UserSignIn>.Filter.Eq(x => x.AppSettingsId, AuthSettingId), Builders<UserSignIn>.Filter.Eq(x => x.UserId, userId));
            var update = Builders<UserSignIn>.Update.Push(token, new Token { Id = jti, LoggedInAt = LoggedIn, ExpiresAt = expiresAt });
            UpdateResult updateResult=  await _mongoContext.UserSignIn.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }
    }
}
