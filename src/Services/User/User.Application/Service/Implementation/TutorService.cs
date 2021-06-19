using AutoMapper;
using ResponseHandling;
using ServiceHeader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepository;
using User.Application.Service.Abstract;
using User.Domain;
using User.ViewModel.model;

namespace User.Application.Service.Implementation
{
    public class TutorService : ITutorService
    {
        private readonly RequestInfo _requestInfo;
        private readonly ITutorRepository _tutorRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        public TutorService(RequestInfo requestInfo, ITutorRepository tutorRepository,
            IMapper mapper, IUserService userService, IAuthService authService, IUserRepository userRepository)
        {
            _requestInfo = requestInfo;
            _tutorRepository = tutorRepository;
            _mapper = mapper;
            _userService = userService;
            _authService = authService;
            _userRepository = userRepository;
        }

        public async Task<string> BecomeATutor(TutorBio TutorBio)
        {
           Tutor tutor = await _tutorRepository.getTutorbyUserIdAsync(_requestInfo.UserId);
            if (tutor != null)
                throw new BusinessException("This user is already a tutor.");
            tutor = new Tutor();
            tutor.UserId = _requestInfo.UserId;
            tutor.Bio =  _mapper.Map<Bio>(TutorBio);
            tutor.CreatedDate = DateTime.UtcNow;
            await _userService.AddRole(Constants.UserConstants.Roles.Tutor);
            await _tutorRepository.InsertTutorAsync(tutor);
            List<string> roles = await _userRepository.GetUserRolesByIdAsync(_requestInfo.UserId);
            return await _authService.GenerateJwtAndInsertSignInHistory(_requestInfo.UserId, roles);
        }

       
    }
}
