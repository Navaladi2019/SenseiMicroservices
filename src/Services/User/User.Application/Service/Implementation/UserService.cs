using AutoMapper;
using ResponseHandling;
using ServiceHeader;
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
        private readonly RequestInfo _requestInfo;
        public UserService(IUserRepository userRepository, IMapper mapper,
            IAuthService authService, RequestInfo requestInfo)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _authService = authService;
            _requestInfo = requestInfo;
        }

        public async Task<SignInResponseModel> RegisterUserAsTutor(UserSignUpModel userSignUpModel)
        {
            return await RegisterUser(userSignUpModel, Constants.UserConstants.Roles.Tutor);
        }


        public async Task<UserWithRoles> getUserRoles(string email)
        {
           UserDto userDto= await _userRepository.GetUserRolesByMailIsAsync(email);
            if (userDto == null)
                return null;
            return new UserWithRoles { emailId = email, Roles = userDto.Roles };
        }
        public async Task<SignInResponseModel> RegisterUser(UserSignUpModel userSignUpModel,string role = Constants.UserConstants.Roles.Student)
        {
            
            if (await _userRepository.GetUserProfileBymailId(userSignUpModel.EmailId.ToLower()) != null)
                throw new BusinessException("User Account exists with provided email");
            UserProfile userProfile = _mapper.Map<UserProfile>(userSignUpModel);
            UserDto userdto = new UserDto { UserProfile = userProfile, EmailId = userSignUpModel.EmailId, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Roles = new List<string> { role } };
            userdto.UserCredential = new UserCredential { Salt = NavalCrypto.GenerateSalt() };
            userdto.UserCredential.SaltedPassword = NavalCrypto.HashText(userSignUpModel.Password, userdto.UserCredential.Salt);
            await _userRepository.AddUser(userdto);
            string Token= await _authService.GenerateJwtAndInsertSignInHistory(userdto.Id, userdto.Roles);
            return await _authService.FillUpSignInResponseModel(userdto, Token);
        }

       public async Task<string> CheckBecomeTutor()
        {
            var roles = await _userRepository.GetUserRolesByIdAsync(_requestInfo.UserId);
            if (roles.Contains(Constants.UserConstants.Roles.Tutor)) 
            return await _authService.GenerateJwtAndInsertSignInHistory(_requestInfo.UserId, roles);
            return null;
        }

        public async Task AddRole(string Role)
        {
            await _userRepository.AddRole(Role, _requestInfo.UserId);

        }

    }
}
