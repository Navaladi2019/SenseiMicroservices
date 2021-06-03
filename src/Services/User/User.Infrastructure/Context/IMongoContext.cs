using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;

namespace User.Infrastructure
{
   public interface IMongoContext
    {
        public IMongoCollection<UserDto> User { get; }

        public IMongoCollection<AuthTokenSetting> AuthTokenSettings { get; }

        public IMongoCollection<UserSignIn> UserSignIn { get;}

        public IMongoCollection<ForgetPasswordSettings> ForgetPasswordSettings { get; }

    }
}
