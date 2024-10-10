namespace GestionDeCursos.Api.Models
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public bool HasError { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
