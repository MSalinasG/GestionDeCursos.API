using GestionDeCursos.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace GestionDeCursos.Api.Areas.Management.Models
{
    public class StudentRequestModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string? StudentName { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int InstructorId { get; set; }

        [Required]
        public int CourseFee { get; set; }

        [Required]
        public int CourseDuration { get; set; }

        [Required]
        public DateTime CourseStartDate { get; set; }

        [Required]
        public DateTime CourseBatchTime { get; set; }
    }
}
