namespace GestionDeCursos.Api.Areas.Management.Models
{
    public class StudentSimpleResponseModel
    {
        public int Id { get; set; }
        public string? StudentName { get; set; }
        public string? CourseName { get; set; }
        public string? InstructorName { get; set; }
        public int CourseFee { get; set; }
        public int CourseDuration { get; set; }
        public DateTime CourseStartDate { get; set; }
        public DateTime BatchTime { get; set; }
    }
}
