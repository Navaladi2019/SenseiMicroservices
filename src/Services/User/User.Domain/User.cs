using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain
{
    public  class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public UserProfile UserProfile { get; set; }
        public SocialAccounts SocialAccounts { get; set; }
        public UserCredential UserCredential { get; set; }
        public List<string> Roles { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    public class UserCredential
    {
        public string Salt { get; set; }
        public string SaltedPassword { get; set; }

        public List<ResetPassword> ResetPasswordKeys { get; set; }
    }


    public class ResetPassword
    {
        public string ResetPasswordKey { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public int IsUsed { get; set; }
    }

    public class UserProfile
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DOB { get; set; }
        public string PreferredName { get; set; }

        public string EmailId { get; set; }

        public string Country { get; set; }

        public string City { get; set; }
    }

    public class SocialAccounts
    {
        public string GitHubId { get; set; }

        public string FaceBookId { get; set; }

       
    }

}
