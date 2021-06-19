using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain
{
   public class Tutor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public Bio Bio { get; set; }

        public DateTime? CreatedDate { get; set; }

    }

    public class Bio
    {
        public string About { get; set; }

        public List<string> ProfessionalSkills { get; set; }
    }

}
