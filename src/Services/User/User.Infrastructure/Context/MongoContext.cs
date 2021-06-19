using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Domain;

namespace User.Infrastructure
{
   public class MongoContext : IMongoContext
    {
        public MongoContext(IMongoClient mongoClient, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase(configuration.GetValue<string>("ConnectionStrings:DatabaseName"));
            User = database.GetCollection<UserDto>("User");
            AuthTokenSettings = database.GetCollection<AuthTokenSetting>("AuthTokenSetting");
            UserSignIn = database.GetCollection<UserSignIn>("UserSignIn");
            ForgetPasswordSettings = database.GetCollection<ForgetPasswordSettings>("ForgetPasswordSettings");
            Tutor = database.GetCollection<Tutor>("Tutor");
            ResetPassword = database.GetCollection<ResetPassword>("ResetPassword");
        }
        public IMongoCollection<UserDto> User { get;  }

        public IMongoCollection<Tutor> Tutor { get; }

        public IMongoCollection<AuthTokenSetting> AuthTokenSettings { get; }

        public IMongoCollection<UserSignIn> UserSignIn { get; }

        public IMongoCollection<ForgetPasswordSettings> ForgetPasswordSettings { get; }

        public IMongoCollection <ResetPassword> ResetPassword { get; }
    }
}
