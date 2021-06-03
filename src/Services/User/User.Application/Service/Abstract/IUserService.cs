using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;
using User.ViewModel;
using User.ViewModel.model;

namespace User.Application.Service.Abstract
{
   public interface IUserService
    {

        public Task<SignInResponseModel> RegisterUser(UserSignUpModel userSignUpModel );



    }
}
