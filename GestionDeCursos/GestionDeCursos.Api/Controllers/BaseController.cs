using AutoMapper;
using GestionDeCursos.Api.Models;
using GestionDeCursos.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GestionDeCursos.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IStringLocalizer<SharedResource> _sharedlocalizer;

        protected BaseController(IMapper mapper,
            IUnitOfWork unitOfWork,
            IStringLocalizer<SharedResource> sharedlocalizer)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _sharedlocalizer = sharedlocalizer;
        }

        protected IActionResult GetApiErrorResponse(string? customMessage = null, Exception? ex = null)
        {
            if (string.IsNullOrWhiteSpace(customMessage))
            {
                customMessage = _sharedlocalizer["GenerarlError"].Value;
            }

            if (ex != null) 
            {
                customMessage = ex.Message;
                // grabar la ex en un archivo de texto o tabla
            }

            return Ok(new ApiResponse<object>
            {
                ErrorMessage = customMessage,
                HasError = true
            });
        }

    }
}
