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
  public  class ForgetPasswordSettingsRepository : IForgetPasswordSettingsRepository
  {

        private readonly IMongoContext _mongoContext;
        public ForgetPasswordSettingsRepository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public Task<ForgetPasswordSettings> GetSettingsByIdAsync(string SettingId)
        {
            var filter = Builders<ForgetPasswordSettings>.Filter.Eq(x => x.Id, SettingId);
            return _mongoContext.ForgetPasswordSettings.Find(filter).FirstOrDefaultAsync();
        }

        public Task<ForgetPasswordSettings> GetSettingAsync(DateTime? dateTime = null)
        {
            if (dateTime == null)
                dateTime = DateTime.UtcNow;
            var filter = Builders<User.Domain.ForgetPasswordSettings>.Filter.And(Builders<User.Domain.ForgetPasswordSettings>.Filter.Eq(x => x.IsActive, true), Builders<User.Domain.ForgetPasswordSettings>.Filter.Lte(x => x.EffectiveFrom, dateTime));
            return _mongoContext.ForgetPasswordSettings.Find(filter).FirstOrDefaultAsync();
        }

        public async Task InsertSettingsAsync(ForgetPasswordSettings ForgetPasswordSettings)
        {
            await _mongoContext.ForgetPasswordSettings.InsertOneAsync(ForgetPasswordSettings);
        }

       
    }
}
