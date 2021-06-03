using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.IRepository
{
  public  interface ISignInRepository
    {
        public Task InsertSignIn(string AuthSettingId,string userId,string jti,DateTime LoggedIn, DateTime expiresAt);
    }
}
