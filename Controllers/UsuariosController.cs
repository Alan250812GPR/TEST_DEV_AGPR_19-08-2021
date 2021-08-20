using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Data;

using Microsoft.AspNetCore.Cors;

using TokaAPI.Models;
using TokaAPI.Helper;

namespace TokaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController:ControllerBase
    {

        public UsuariosController(TokaContext Contexto)
        {
            _Contexto = Contexto;
        }
        private readonly TokaContext _Contexto;

        //POST: api/Usuarios/Crear
        [EnableCors("Cors")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] TbUsuarios Usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            if (await _Contexto.TbUsuarios.Where(_Usuario => _Usuario.Email == Usuario.Email).AnyAsync())
            {
                return BadRequest(ErrorHelper.Response(400, $"El usuario {Usuario.Email} ya existe."));
            }

            HashedPassword Contrasenia = HashHelper.Hash(Usuario.Clave);
            Usuario.Clave = Contrasenia.Password;
            Usuario.Salt = Contrasenia.Salt;

            _Contexto.TbUsuarios.Add(Usuario);
            await _Contexto.SaveChangesAsync();
            return Ok(Usuario);
        }
    }
}