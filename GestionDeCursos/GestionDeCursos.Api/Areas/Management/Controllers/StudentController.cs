using AutoMapper;
using GestionDeCursos.Api.Areas.Management.Models;
using GestionDeCursos.Api.Controllers;
using GestionDeCursos.Api.Models;
using GestionDeCursos.Api.Services.Management;
using GestionDeCursos.Api.TokenConfig;
using GestionDeCursos.Data.Helpers;
using GestionDeCursos.Data.Models;
using GestionDeCursos.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GestionDeCursos.Api.Areas.Management.Controllers
{
    [Area("Management")]
    [Route("api/{culture}/[area]/students")]
    [ApiController]
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;
        public StudentController(IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IStringLocalizer<SharedResource> sharedlocalizer,
            IStudentService studentService) : base(mapper, unitOfWork, sharedlocalizer)
        {            
            _studentService = studentService;
        }

        [HttpGet("list")]
        [RoleAuthFilter([GlobalHelper.Role.Administrator, GlobalHelper.Role.Instructor, GlobalHelper.Role.Student])]
        public async Task<IActionResult> ListAll()
        {
            try
            {
                var students = await _unitOfWork.StudentRepository.GetAll();

                var convertedModel = _mapper.Map<IEnumerable<StudentSimpleResponseModel>>(students);

                var apiResponse = new ApiResponse<IEnumerable<StudentSimpleResponseModel>>{
                    Data = convertedModel
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

        [HttpGet("list-students-details")]
        [RoleAuthFilter([GlobalHelper.Role.Administrator, GlobalHelper.Role.Instructor, GlobalHelper.Role.Student])]
        public async Task<IActionResult> ListWithDetails()
        {
            try
            {
                var students = await _unitOfWork.StudentRepository.GetStudentWithCourse();

                //Mapper
                var convertedModel = _mapper.Map<IEnumerable<StudentResponseModel>>(students);

                var apiResponse = new ApiResponse<IEnumerable<StudentResponseModel>>
                {
                    Data = convertedModel
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

        [HttpGet("{id}")]
        [RoleAuthFilter([GlobalHelper.Role.Administrator, GlobalHelper.Role.Instructor, GlobalHelper.Role.Student])]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var detail = await _unitOfWork.StudentRepository.GetStudentWithCourseById(id);

                if (detail == null)
                {
                    return GetApiErrorResponse(_sharedlocalizer["RecordNotFound"].Value);
                }

                //Mapper
                var convertedModel = _mapper.Map<StudentResponseModel>(detail);

                var apiResponse = new ApiResponse<StudentResponseModel>
                {
                    Data = convertedModel
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

        [HttpPost]
        [RoleAuthFilter([GlobalHelper.Role.Administrator, GlobalHelper.Role.Instructor])]
        public async Task<IActionResult> Create([FromBody] StudentRequestModel model)
        {
            try
            {
                //Mapeo MANUAL
                //var studentModel = new Student
                //{
                //    StudentName = model.StudentName,
                //    CourseFee = model.CourseFee,
                //    CourseStartDate = model.CourseStartDate,
                //    CourseDuration = model.CourseDuration,
                //    CourseId = model.CourseId,
                //    InstructorId = model.InstructorId,
                //    BatchTime = model.CourseBatchTime,
                //};

                //Mapper
                var convertedModel = _mapper.Map<Student>(model);

                var studenId = await _studentService.Create(convertedModel);
                                 
                var apiResponse = new ApiResponse<int>
                {
                    Data = studenId
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

        [HttpPut("{id}")]
        [RoleAuthFilter([GlobalHelper.Role.Administrator, GlobalHelper.Role.Instructor])]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] StudentRequestModel model)
        {
            try
            {
                model.Id = id;

                //Mapper
                var convertedModel = _mapper.Map<Student>(model);

                await _studentService.Update(convertedModel);

                var apiResponse = new ApiResponse<bool>
                {
                    Data = true
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

        [HttpDelete("{id}")]
        [RoleAuthFilter([GlobalHelper.Role.Administrator, GlobalHelper.Role.Instructor])]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var detail = await _unitOfWork.StudentRepository.Get(id);

                if (detail == null)
                {
                    return GetApiErrorResponse(_sharedlocalizer["RecordNotFound"].Value);
                }

                await _unitOfWork.StudentRepository.DeleteStudentSp(id);

                var apiResponse = new ApiResponse<bool>
                {
                    Data = true
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
