using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();

            usuario.Rol = new ML.Rol();
            usuario.Direccion = new ML.Direccion();

            ML.Result result = BL.Usuario.GetAll(usuario);
            

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
                              
        [HttpPost("GetAll")]
        public IActionResult GetAll(string nombre, string apellidoPaterno)
        {
            ML.Usuario usuario = new ML.Usuario();

            usuario.Nombre = (usuario.Nombre == null) ? "" : nombre;
            usuario.ApellidoPaterno = (usuario.ApellidoPaterno == null) ? "" : apellidoPaterno;

            usuario.Rol = new ML.Rol();
            usuario.Direccion = new ML.Direccion();

            ML.Result result = BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetById/{idUsuario}")]
        public IActionResult GetById(int idUsuario)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            ML.Result result = BL.Usuario.GetById(idUsuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.Add(usuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("Update")]
        public IActionResult Update([FromBody] ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.Update(usuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpGet("Delete/{idUsuario}")]
        public IActionResult Delete(int idUsuario)
        {
            ML.Result result = BL.Usuario.Delete(idUsuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }


        // DELETE api/<UsuarioController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
