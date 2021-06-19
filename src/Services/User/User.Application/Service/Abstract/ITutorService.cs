using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.ViewModel.model;

namespace User.Application.Service.Abstract
{
   public interface ITutorService
    {
        Task<string> BecomeATutor(TutorBio tutorProfile);
    }
}
