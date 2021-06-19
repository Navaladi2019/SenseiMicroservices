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
  public  class TutorRepository : ITutorRepository
    {

        private readonly IMongoContext _mongoContext;
        public TutorRepository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }


        public async Task<Tutor> getTutorbyUserIdAsync(string userId)
        {
            var filter = Builders<User.Domain.Tutor>.Filter.Eq(x => x.UserId, userId);
            return await _mongoContext.Tutor.Find(filter).FirstOrDefaultAsync();
        }


       public async Task InsertTutorAsync(Tutor tutor)
        {
             await _mongoContext.Tutor.InsertOneAsync(tutor);
        }
    }
}
