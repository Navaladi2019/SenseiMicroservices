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
    public class AuthTokenSettingRepository : IAuthTokenSettingRepository
    {
        private readonly IMongoContext _mongoContext;

        public AuthTokenSettingRepository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public Task<AuthTokenSetting> GetSettingsByIdAsync(string authSettingId)
        {
            var filter = Builders<User.Domain.AuthTokenSetting>.Filter.Eq(x => x.Id, authSettingId);
            return _mongoContext.AuthTokenSettings.Find(filter).FirstOrDefaultAsync();
        }

        public Task<AuthTokenSetting> GetSettingAsync(DateTime? dateTime = null)
        {
            var filter = Builders<User.Domain.AuthTokenSetting>.Filter.And(Builders<User.Domain.AuthTokenSetting>.Filter.Eq(x => x.IsActive, true), Builders<User.Domain.AuthTokenSetting>.Filter.Lte(x => x.EffectiveFrom, dateTime));
            return _mongoContext.AuthTokenSettings.Find(filter).FirstOrDefaultAsync();
        }

      public async  Task InsertSettingsAsync(AuthTokenSetting authTokenSetting)
        {
            await _mongoContext.AuthTokenSettings.InsertOneAsync(authTokenSetting);
        }
    }
}
