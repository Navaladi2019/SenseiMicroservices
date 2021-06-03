using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.ViewModel.model
{
  public  class SignInResponseModel
    {

        public string AccessToken { get; set; }

        public string PreferredName { get; set; }

        public string EmailId { get; set; }
    }
}
