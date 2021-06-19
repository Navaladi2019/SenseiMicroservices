using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Infrastructure
{
   public class MongoContext : IMongoContext
    {
        public MongoContext(IMongoClient mongoClient, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase(configuration.GetValue<string>("ConnectionStrings:DatabaseName"));
           
        }
     
    }
}
