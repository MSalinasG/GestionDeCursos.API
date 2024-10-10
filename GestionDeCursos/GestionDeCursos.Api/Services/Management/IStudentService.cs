using GestionDeCursos.Data.Models;

namespace GestionDeCursos.Api.Services.Management
{
    public interface IStudentService
    {
        Task<int> Create(Student model);
        Task Update(Student model);

    }
}
