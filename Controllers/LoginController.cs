using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cors;

using TokaAPI.Models;
using TokaAPI.Models.ViewModel;
using TokaAPI.Helper;


namespace TokaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {

        public LoginController(TokaContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }
        private readonly TokaContext _context;
        public IConfiguration Configuration { get; }

        //POST: api/Login/IniciarSesion
        [EnableCors("Cors")]
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginVM login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            var usuario = await _context.TbUsuarios.Where(x => x.Email == login.Email).FirstOrDefaultAsync();
            if (usuario == null)
            {
                return NotFound(ErrorHelper.Response(404, "Usuario no encontrado"));
            }

            if (HashHelper.CheckHash(login.Clave, usuario.Clave, usuario.Salt))
            {
                //Realizamos la autenticacion utilizando JWT
                var secretKey = Configuration.GetValue<string>("SecretKey");
                var key = Encoding.ASCII.GetBytes(secretKey);

                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, login.Email));
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var createdToken = tokenHandler.CreateToken(tokenDescriptor);
                string bearer_token = tokenHandler.WriteToken(createdToken);

                var Id = usuario.IdUsuario;
                var Nombre = usuario.Nombre;
                var Token = bearer_token; 

                return Ok(new {
                    Token,
                    Id,
                    Nombre
                });

            }
            else
            {
                return Forbid();
            }
        }

    }
}