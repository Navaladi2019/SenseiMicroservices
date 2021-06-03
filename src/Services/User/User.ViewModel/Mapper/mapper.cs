using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;

namespace User.ViewModel
{
  public  class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<UserSignUpModel, UserProfile>().ReverseMap();

        }
    }
}
