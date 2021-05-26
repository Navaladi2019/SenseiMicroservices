using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Infrastructure
{
   public class MongoContext : IMongoContext
    {
        public MongoContext(IMongoClient mongoClient, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase(configuration.GetSection("ConnectionStrings:DatabaseName").Value);
            User = database.GetCollection<User.Domain.User>("User");
        }
        public IMongoCollection<User.Domain.User> User { get;  }

    }
}
