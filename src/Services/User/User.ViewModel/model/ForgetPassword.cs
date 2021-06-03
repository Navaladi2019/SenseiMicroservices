using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.ViewModel.model
{
   public class ForgetPassword
    {
        [Required(ErrorMessage ="Email Id is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailId { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }

    public class ForgetPasswordSerializer
    {

        public string UserId { get; set; }

        public string Key { get; set; }
    }



    public class ResetForgetPassword
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Password { get; set; }
    }

}
