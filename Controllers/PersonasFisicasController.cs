using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

using TokaAPI.Models;
using TokaAPI.Helper;

namespace TokaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonasFisicasController : ControllerBase
    {

        public PersonasFisicasController(TokaContext Contexto, IConfiguration Configuracion)
        {
            _Contexto = Contexto;
            _Configuracion = Configuracion;
        }
        private readonly TokaContext _Contexto;
        public IConfiguration _Configuracion { get; }

        //GET: api/PersonasFisicas/Listar
        [EnableCors("Cors")]
        [HttpGet("[action]")]
        
        public async Task<IEnumerable<TbPersonasFisica>> Listar()
        {
            return await _Contexto.TbPersonasFisicas.ToListAsync();
        }

        //GET: api/PersonasFisicas/Paginar/1/1
        [EnableCors("Cors")]
        [HttpGet("[action]/{NumeroPagina}/{TamanioPagina}")]
        public IEnumerable<TbPersonasFisica> Paginar([FromRoute] int NumeroPagina, [FromRoute] int TamanioPagina)
        {
            var Colleccion = _Contexto.TbPersonasFisicas as IQueryable<TbPersonasFisica>;
            return PagedList<TbPersonasFisica>.Create(Colleccion, NumeroPagina, TamanioPagina);
        }

        //GET: api/PersonasFisicas/Consultar/1
        [EnableCors("Cors")]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Consultar([FromRoute] int id)
        {
            var PersonaFisica = await _Contexto.TbPersonasFisicas.FindAsync(id);
            if (PersonaFisica == null)
            {
                return NotFound(ErrorHelper.Response(404, $"Persona fisica con id {id} no encontrado."));
            }

            return Ok(PersonaFisica);
        }

        //POST: api/PersonasFisicas/Crear
        [EnableCors("Cors")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] TbPersonasFisica PersonaFisica)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            int Error = 0;
            String MensajeError = "";

            using (SqlConnection Conexion = new SqlConnection(_Configuracion.GetConnectionString("Connection")))
            {
                using (SqlCommand Comando = new SqlCommand("sp_AgregarPersonaFisica", Conexion))
                {
                    Comando.CommandType = CommandType.StoredProcedure;

                    Comando.Parameters.AddWithValue("@Nombre", SqlDbType.VarChar).Value = PersonaFisica.Nombre;
                    Comando.Parameters.AddWithValue("@ApellidoPaterno", SqlDbType.VarChar).Value = PersonaFisica.ApellidoPaterno;
                    Comando.Parameters.AddWithValue("@ApellidoMaterno", SqlDbType.VarChar).Value = PersonaFisica.ApellidoMaterno;
                    Comando.Parameters.AddWithValue("@RFC", SqlDbType.VarChar).Value = PersonaFisica.Rfc;
                    Comando.Parameters.AddWithValue("@FechaNacimiento", SqlDbType.Date).Value = PersonaFisica.FechaNacimiento;
                    Comando.Parameters.AddWithValue("@UsuarioAgrega", SqlDbType.Bit).Value = PersonaFisica.UsuarioAgrega;

                    Conexion.Open();

                    SqlDataReader Lector = await Comando.ExecuteReaderAsync();

                    if (Lector.HasRows)
                    {
                        while (Lector.Read())
                        {
                            Error = Lector.GetInt32(0);
                            MensajeError = Lector.GetString(1);
                        }
                    }
                    Lector.Close();
                    Conexion.Close();

                    if (Error == -50000)
                    {
                        return BadRequest(ErrorHelper.Response(400, MensajeError));
                    }
                    return CreatedAtAction(nameof(Consultar), new { id = Error }, PersonaFisica);
                }
            }
        }

        //PUT: api/PersonasFisicas/Actualizar/1
        [EnableCors("Cors")]
        [HttpPut("[action]/{id}")]

        public async Task<IActionResult> Actualizar([FromRoute] int id, [FromBody] TbPersonasFisica PersonaFisica)
        {
            if (PersonaFisica.IdPersonaFisica != id)
            {
                return BadRequest(ErrorHelper.Response(400, "No coindicen las claves de la persona fisica."));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            int Error = 0;
            String MensajeError = "";

            using (SqlConnection Conexion = new SqlConnection(_Configuracion.GetConnectionString("Connection")))
            {
                using (SqlCommand Comando = new SqlCommand("sp_ActualizarPersonaFisica", Conexion))
                {
                    Comando.CommandType = CommandType.StoredProcedure;

                    Comando.Parameters.AddWithValue("@IdPersonaFisica", SqlDbType.Int).Value = PersonaFisica.IdPersonaFisica;
                    Comando.Parameters.AddWithValue("@Nombre", SqlDbType.VarChar).Value = PersonaFisica.Nombre;
                    Comando.Parameters.AddWithValue("@ApellidoPaterno", SqlDbType.VarChar).Value = PersonaFisica.ApellidoPaterno;
                    Comando.Parameters.AddWithValue("@ApellidoMaterno", SqlDbType.VarChar).Value = PersonaFisica.ApellidoMaterno;
                    Comando.Parameters.AddWithValue("@RFC", SqlDbType.VarChar).Value = PersonaFisica.Rfc;
                    Comando.Parameters.AddWithValue("@FechaNacimiento", SqlDbType.Date).Value = PersonaFisica.FechaNacimiento;
                    Comando.Parameters.AddWithValue("@UsuarioAgrega", SqlDbType.Bit).Value = PersonaFisica.UsuarioAgrega;

                    Conexion.Open();

                    SqlDataReader Lector = await Comando.ExecuteReaderAsync();

                    if (Lector.HasRows)
                    {
                        while (Lector.Read())
                        {
                            Error = Lector.GetInt32(0);
                            MensajeError = Lector.GetString(1);
                        }
                    }
                    Lector.Close();
                    Conexion.Close();

                    if (Error == -50000)
                    {
                        return BadRequest(ErrorHelper.Response(400, MensajeError));
                    }
                    return NoContent();
                }
            }

        }

        //DELETE: api/PersonasFisicas/Eliminar/1
        [EnableCors("Cors")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            int Error = 0;
            String MensajeError = "";

            using (SqlConnection Conexion = new SqlConnection(_Configuracion.GetConnectionString("Connection")))
            {
                using (SqlCommand Comando = new SqlCommand("sp_EliminarPersonaFisica", Conexion))
                {
                    Comando.CommandType = CommandType.StoredProcedure;

                    Comando.Parameters.AddWithValue("@IdPersonaFisica", SqlDbType.Int).Value = id;
                    Conexion.Open();

                    SqlDataReader Lector = await Comando.ExecuteReaderAsync();

                    if (Lector.HasRows)
                    {
                        while (Lector.Read())
                        {
                            Error = Lector.GetInt32(0);
                            MensajeError = Lector.GetString(1);
                        }

                        if (Error == -50000)
                        {
                            return BadRequest(ErrorHelper.Response(400, MensajeError));
                        }
                    }

                    Lector.Close();
                    Conexion.Close();

                    return NoContent();
                }
            }
        }

    }
}