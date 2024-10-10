namespace GestionDeCursos.Api.Areas.Auth.Models
{
    public class LoggedUserModel
    {
        public Guid UserId { get; set; }
        public string? Username { get; set; }
        public string? Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
