
using GestionDeCursos.Data.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GestionDeCursos.Api.TokenConfig
{
    public class CustomToken : ICustomToken
    {
        private readonly IConfiguration _configuration;

        public CustomToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Tuple<string, DateTime> GenerateToken(string userId)
        {
            DateTime currentDate = DateTime.Now;

            int expirationInMinutes = int.Parse(_configuration["Tokens:ExpirationTime"]);

            DateTime expirationDate = currentDate.Add(TimeSpan.FromMinutes(expirationInMinutes));
            DateTime expirationDate2 = currentDate.AddMinutes(expirationInMinutes);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(GlobalHelper.CustomClaim.AppUserIdClaim, userId)
                //new Claim("EmailClaim", objUser.Email),
                //new Claim("PhoneClaim", objUser.Phone),
            };

            var jwtSecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:JwtSecretKey"]));
            var credentials = new SigningCredentials(jwtSecretKey, SecurityAlgorithms.HmacSha256);

            var objToken = new JwtSecurityToken(
                issuer: _configuration["Tokens:Issuer"],
                audience: _configuration["Tokens:Audience"],
                claims: claims,
                notBefore: currentDate,
                expires: expirationDate,
                signingCredentials: credentials
            );

            var tokenResult = new JwtSecurityTokenHandler().WriteToken(objToken);

            return new Tuple<string, DateTime>(tokenResult, expirationDate);
        }
    }
}
