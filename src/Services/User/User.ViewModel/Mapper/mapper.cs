using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;
using User.ViewModel.model;

namespace User.ViewModel
{
  public  class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<UserSignUpModel, UserProfile>().ReverseMap();
            CreateMap<TutorBio, Bio>().ReverseMap();
        }
    }
}
