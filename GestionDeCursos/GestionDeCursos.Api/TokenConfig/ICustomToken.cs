namespace GestionDeCursos.Api.TokenConfig
{
    public interface ICustomToken
    {
        public Tuple<string, DateTime> GenerateToken(string userId);
    }
}
