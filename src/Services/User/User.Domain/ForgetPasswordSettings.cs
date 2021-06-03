using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain
{
    public class ForgetPasswordSettings
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string EncryptKey { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public bool IsActive { get; set; }

        public int ExpireAfterMinutes { get; set; }

    }
}
