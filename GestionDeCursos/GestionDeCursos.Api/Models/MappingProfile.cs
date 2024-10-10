using AutoMapper;
using GestionDeCursos.Api.Areas.Management.Models;
using GestionDeCursos.Data.CustomModel;
using GestionDeCursos.Data.Models;

namespace GestionDeCursos.Api.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StudentDto, Student>()
                .ForPath(dest => dest.Course.Id, opt => opt.MapFrom(src => src.CourseId))
                .ForPath(dest => dest.Course.CourseName, opt => opt.MapFrom(src => src.CourseName))
                .ForPath(dest => dest.Instructor.Id, opt => opt.MapFrom(src => src.InstructorId))
                .ForPath(dest => dest.Instructor.InstructorName, opt => opt.MapFrom(src => src.InstructorName));

            CreateMap<StudentRequestModel, Student>()
                .ForMember(dest => dest.BatchTime, opt => opt.MapFrom(src => src.CourseBatchTime));

            CreateMap<Student, StudentResponseModel>()
                .ForMember(dest => dest.CourseBatchTime, opt => opt.MapFrom(src => src.BatchTime))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.CourseName))
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.InstructorName))
                .ForPath(dest => dest.Course.CourseId, opt => opt.MapFrom(src => src.Course.Id))
                .ForPath(dest => dest.Course.CourseName, opt => opt.MapFrom(src => src.Course.CourseName))
                .ForPath(dest => dest.Instructor.Id, opt => opt.MapFrom(src => src.Instructor.Id))
                .ForPath(dest => dest.Instructor.Name, opt => opt.MapFrom(src => src.Instructor.InstructorName));

            CreateMap<Student, StudentSimpleResponseModel>();

        }
    }
}
