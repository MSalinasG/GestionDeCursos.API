using GestionDeCursos.Api.Areas.Auth.Models;
using GestionDeCursos.Api.TokenConfig;
using GestionDeCursos.Data.Helpers;
using GestionDeCursos.Data.Models;
using GestionDeCursos.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace GestionDeCursos.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly ICustomToken _customToken;
        private readonly IStringLocalizer<UserService> _localizer;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public UserService(IUnitOfWork unitOfWork,
            IPasswordHasher<AppUser> passwordHasher,
            ICustomToken customToken,
            IStringLocalizer<UserService> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _customToken = customToken;
            _localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
        }

        public async Task<LoggedUserModel> ValidateLogin(LoginModel model)
        {
            var objUser = await _unitOfWork.DatabaseContext.Users
                .FirstOrDefaultAsync(x => x.Username == model.Username);

            if (objUser == null)
            {
                throw new CustomException(_localizer["UserNotFound"].Value);
            }

            if (!objUser.IsActive)
            {
                throw new CustomException(_localizer["InactiveAccount"].Value);
            }

            var passwordResponse = _passwordHasher.VerifyHashedPassword(objUser, objUser.Password, model.Password);
            if (passwordResponse != PasswordVerificationResult.Success)
            {
                throw new CustomException(_localizer["IncorrectCredentials"].Value);
            }

            var tokenResult = _customToken.GenerateToken(objUser.Id.ToString());

            return new LoggedUserModel
            {
                UserId = objUser.Id,
                Username = objUser.Username,
                Token = tokenResult.Item1,
                ExpirationDate = tokenResult.Item2
            };
        }

        public async Task<bool> ValidateUser(string userId)
        {
            var objUser = await _unitOfWork.DatabaseContext.Users.FindAsync(new Guid(userId));
            return objUser != null;
        }

        public async Task<bool> ValidateUserRole(string userId, string[] roles)
        {
            var objUser = await _unitOfWork.DatabaseContext.Users.FindAsync(new Guid(userId));
            if (objUser == null)
            {
                return false;
            }
            var objRole = await _unitOfWork.DatabaseContext.Roles.FindAsync(objUser.RoleId);
            if (objRole == null)
            {
                return false;
            }
            bool hasRole = roles.Any(x => x == objRole.Name);
            return hasRole;
        }
    }
}
