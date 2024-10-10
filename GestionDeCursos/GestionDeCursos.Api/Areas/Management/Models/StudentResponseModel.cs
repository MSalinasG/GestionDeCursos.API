using System.ComponentModel.DataAnnotations;

namespace GestionDeCursos.Api.Areas.Management.Models
{
    public class StudentResponseModel
    {
        public int Id { get; set; }        
        public string? StudentName { get; set; }

        public CourseResponseModel? Course { get; set; }
        public int CourseId { get; set; }
        public string? CourseName{ get; set; }


        public InstructorResponseModel? Instructor { get; set; }
        public int InstructorId { get; set; }
        public string? InstructorName { get; set; }


        public int CourseFee { get; set; }
        public int CourseDuration { get; set; }
        public DateTime CourseStartDate { get; set; }
        public DateTime CourseBatchTime { get; set; }
    }
}
