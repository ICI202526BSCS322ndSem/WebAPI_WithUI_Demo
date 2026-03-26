using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly string _secretKey = "THIS_IS_A_SAMPLE_KEY_THIS_IS_A_SAMPLE_KEY";

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var permissions = new List<string>();
            
            if (request.Email == "view@test.com" && request.Password == "123")
            {
                permissions.AddRange(new[] { "view_product" });
                var token = GenerateJwtToken(request.Email, permissions);

                return Ok(new
                {
                    token = token,
                    email = request.Email
                });
            }
            if (request.Email == "viewadd@test.com" && request.Password == "123")
            {
                permissions.AddRange(new[] { "view_product", "add_product"});
                var token = GenerateJwtToken(request.Email, permissions);

                return Ok(new
                {
                    token = token,
                    email = request.Email
                });
            }
            if (request.Email == "viewaddedit@test.com" && request.Password == "123")
            {
                permissions.AddRange(new[] { "view_product", "add_product", "edit_product" });
                var token = GenerateJwtToken(request.Email, permissions);

                return Ok(new
                {
                    token = token,
                    email = request.Email
                });
            }
            if (request.Email == "viewaddeditdelete@test.com" && request.Password == "123")
            {
                permissions.AddRange(new[] { "view_product", "add_product", "edit_product", "delete_product" });
                var token = GenerateJwtToken(request.Email, permissions);

                return Ok(new
                {
                    token = token,
                    email = request.Email
                });
            }
            else
            {
                return Unauthorized();
            }

        }


        private string GenerateJwtToken(string email,List<string> permissions)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var claims = new List<Claim>();
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission));
            }



            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
