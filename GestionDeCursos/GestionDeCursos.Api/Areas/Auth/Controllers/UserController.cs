using AutoMapper;
using GestionDeCursos.Api.Areas.Auth.Models;
using GestionDeCursos.Api.Controllers;
using GestionDeCursos.Api.Models;
using GestionDeCursos.Api.Services;
using GestionDeCursos.Data.Helpers;
using GestionDeCursos.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GestionDeCursos.Api.Areas.Auth.Controllers
{
    [Area("Auth")]
    [Route("api/{culture}/[area]/users")]
    [ApiController]
    public class UserController : BaseController
    {        
        protected readonly IUserService _userService;

        public UserController(IMapper mapper, 
            IUserService userService,
            IUnitOfWork unitOfWork,
            IStringLocalizer<SharedResource> sharedlocalizer) : base(mapper,unitOfWork, sharedlocalizer) 
        { 
            _userService = userService;
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var test = _sharedlocalizer["GenerarlError"].Value;
                var serviceResponse = await _userService.ValidateLogin(model);

                var apiResponse = new ApiResponse<LoggedUserModel>
                {
                    Data = serviceResponse
                };

                return Ok(apiResponse);
            }
            catch (CustomException ex)
            {
                return GetApiErrorResponse(ex.Message);
            }
            catch (Exception ex)
            {
                return GetApiErrorResponse(null, ex);
            }
        }
    }
}
