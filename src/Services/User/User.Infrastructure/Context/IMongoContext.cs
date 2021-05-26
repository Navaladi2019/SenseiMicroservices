using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Infrastructure
{
   public interface IMongoContext
    {
        public IMongoCollection<User.Domain.User> User { get; }

    }
}
