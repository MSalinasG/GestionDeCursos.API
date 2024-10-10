using GestionDeCursos.Api.TokenConfig;
using GestionDeCursos.Data.Helpers;
using GestionDeCursos.Data.Models;
using GestionDeCursos.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace GestionDeCursos.Api.Services.Management
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;        
        private readonly IStringLocalizer<StudentService> _localizer;

        public StudentService(IUnitOfWork unitOfWork,
            IStringLocalizer<StudentService> localizer)
        {
            _unitOfWork = unitOfWork; 
            _localizer = localizer;            
        }

        public async Task<int> Create(Student model)
        {
            await ValidateStudentModel(model);
            int studentId = await _unitOfWork.StudentRepository.InsertStudentEf(model);
            return studentId;
        }

        public async Task Update(Student model)
        {
           
            await ValidateStudentModel(model);
            await _unitOfWork.StudentRepository.UpdateStudentSp(model);
        }

        private async Task ValidateStudentModel(Student model)
        {
            var objCourse = await _unitOfWork.CourseRepository.Get(model.CourseId);

            if (objCourse == null)
            {
                throw new CustomException(_localizer["CourseNotFound"].Value);
            }

            var objInstructor = await _unitOfWork.InstructorRepository.Get(model.InstructorId);

            if (objInstructor == null)
            {
                throw new CustomException(_localizer["InstructorNotFound"].Value);
            }
        }
    }
}
