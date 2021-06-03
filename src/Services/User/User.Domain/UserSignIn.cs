using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain
{
    public   class UserSignIn
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string AppSettingsId { get; set; }

        public string UserId { get; set; }

        public List<Token> Tokens { get; set; }
    }

    public class Token
    {
        public string Id { get; set; }

        public DateTime LoggedInAt { get; set; }
        public DateTime? LoggedOutAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
