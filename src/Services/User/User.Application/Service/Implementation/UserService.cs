using AutoMapper;
using ResponseHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Constants;
using User.Application.Security;
using User.Application.Service.Abstract;
using User.Domain;
using User.ViewModel;
using User.ViewModel.model;

namespace User.Application.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public UserService(IUserRepository userRepository, IMapper mapper, IAuthService authService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<SignInResponseModel> RegisterUser(UserSignUpModel userSignUpModel)
        {
            
            if (await _userRepository.GetUserProfileBymailId(userSignUpModel.EmailId.ToLower()) != null)
                throw new BusinessException("User Account exists with provided email");
            UserProfile userProfile = _mapper.Map<UserProfile>(userSignUpModel);
            userProfile.EmailId = userProfile.EmailId.ToLower();
            UserDto userdto = new UserDto { UserProfile = userProfile, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Roles = new List<string> { UserConstants.Roles.Student} };
            userdto.UserCredential = new UserCredential { Salt = NavalCrypto.GenerateSalt() };
            userdto.UserCredential.SaltedPassword = NavalCrypto.HashText(userSignUpModel.Password, userdto.UserCredential.Salt);
            await _userRepository.AddUser(userdto);
            string Token= await _authService.GenerateJwtAndInsertSignInHistory(userdto.Id, userdto.Roles);
            return await _authService.FillUpSignInResponseModel(userdto, Token);
        }

       

    }
}
