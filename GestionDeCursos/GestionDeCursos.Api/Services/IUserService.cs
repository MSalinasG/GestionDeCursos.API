using GestionDeCursos.Api.Areas.Auth.Models;

namespace GestionDeCursos.Api.Services
{
    public interface IUserService
    {
        Task<bool> ValidateUser(string userId);
        Task<bool> ValidateUserRole(string userId, string[] roles);
        Task<LoggedUserModel> ValidateLogin(LoginModel model);
    }
}
