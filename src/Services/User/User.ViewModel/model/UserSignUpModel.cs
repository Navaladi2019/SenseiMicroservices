using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace User.ViewModel
{
    public class UserSignUpModel
    {

        [Required(ErrorMessage = "PreferredName is required")]
        public string PreferredName { get; set; }

        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailId { get; set; }

        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }
    }

    public class UserWithRoles
    {
        public string emailId { get; set; }

        public List<string> Roles { get; set; }
    }
}
