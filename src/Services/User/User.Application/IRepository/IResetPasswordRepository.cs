using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;

namespace User.Application.IRepository
{
   public interface IResetPasswordRepository
    {
        Task Add(ResetPassword resetPassword);
        Task<ResetPassword> GetResetPasswordByUserIdAndKey(string UserId,string Key);
    }
}
