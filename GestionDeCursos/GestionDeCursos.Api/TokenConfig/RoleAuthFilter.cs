using GestionDeCursos.Api.Services;
using GestionDeCursos.Data.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GestionDeCursos.Api.TokenConfig
{
    public class RoleAuthFilter : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private string[] _UserRole { get; set; }

        public RoleAuthFilter(params string[] userRoles)
        {
            _UserRole = userRoles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                if (_UserRole == null || _UserRole.Count() == 0)
                {
                    context.Result = new BadRequestResult();
                }
                else
                {
                    var userContext = context.HttpContext.Request.HttpContext.User;
                    var claimList = userContext.Claims
                        .Where(x => x.Type == GlobalHelper.CustomClaim.AppUserIdClaim);

                    if (claimList == null || claimList.Count() == 0)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                    string userId = claimList.FirstOrDefault().Value;

                    var userService = (IUserService) context.HttpContext.RequestServices.GetService(typeof(IUserService));

                    bool hasRole = await userService.ValidateUserRole(userId, _UserRole);

                    if (!hasRole)
                    {
                        context.Result = new UnauthorizedResult();
                    }
                }
            }
            catch (Exception ex)
            {
                context.Result = new BadRequestResult();
                Console.Write(ex.Message);
            }
        }
    }
}
