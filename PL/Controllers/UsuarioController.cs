using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            ML.Result result = new ML.Result();
            result = BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;

            }
            else
            {
                ViewBag.Message = "Error al realizar la consulta";
            }

            return View(usuario);
        }

        [HttpPost]
        public ActionResult GetAll(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            result = BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;

            }
            else
            {
                ViewBag.Message = "Error al realizar la consulta";
            }

            return View(usuario);
        }

        [HttpGet]
        public ActionResult Form(int? idUsuario)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();

            usuario.Direccion = new ML.Direccion();
            usuario.Direccion.Colonia = new ML.Colonia();
            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
            usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();

            ML.Result resultRol = BL.Rol.GetAll();
            ML.Result resultPais = BL.Pais.GetAll();

            if (idUsuario == null)
            {
                usuario.Rol.Roles = resultRol.Objects;
                usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;

                return View(usuario);
            }
            else
            {
                ML.Result result = BL.Usuario.GetById(idUsuario.Value);

                if (result.Correct)
                {
                    usuario = (ML.Usuario)result.Object;
                    usuario.Rol.Roles = resultRol.Objects;
                    usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;

                    ML.Result resultEstados = BL.Estado.EstadoGetByIdPais(usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais);
                    usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstados.Objects;

                    ML.Result resultMunicipio = BL.Municipio.MunicipioGetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
                    usuario.Direccion.Colonia.Municipio.Municipios = resultMunicipio.Objects;

                    ML.Result resultColonia = BL.Colonia.ColoniaGetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);
                    usuario.Direccion.Colonia.Colonias = resultColonia.Objects;

                    return View(usuario);
                }
                else
                {
                    ViewBag.Message = "Error al consultar el usuario seleccionado";
                }
                return View(usuario);
            }

        }

        [HttpPost]
        public ActionResult Form(ML.Usuario usuario)
        {
            IFormFile image = Request.Form.Files["ImagenData"];

            if (image != null)
            {
                byte[] ImagenByte = ConvertToBytes(image);

                usuario.Imagen = Convert.ToBase64String(ImagenByte);
            }

            if (!ModelState.IsValid)
            {
                if (usuario.IdUsuario == 0)
                {
                    ML.Result result = BL.Usuario.Add(usuario);

                    if (result.Correct)
                    {
                        ViewBag.Message = result.Message;
                    }
                    else
                    {
                        ViewBag.Message = "Error: " + result.Message;
                    }
                }
                else
                {
                    ML.Result result = BL.Usuario.Update(usuario);

                    if (result.Correct)
                    {
                        ViewBag.Message = result.Message;
                    }
                    else
                    {
                        ViewBag.Message = "Error al actualizar al usuario";
                    }
                }
                return PartialView("Modal");
            }
            else
            {
                usuario.Rol = new ML.Rol();
                usuario.Direccion = new ML.Direccion();
                usuario.Direccion.Colonia = new ML.Colonia();
                usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();

                ML.Result resultRol = BL.Rol.GetAll();
                ML.Result resultPais = BL.Pais.GetAll();

                usuario.Rol.Roles = resultRol.Objects;
                usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;

                return View(usuario);
            }
        }

        [HttpGet]
        public ActionResult Delete(int idUsuario)
        {
            if (idUsuario >= 1)
            {
                ML.Result result = BL.Usuario.Delete(idUsuario);

                if (result.Correct)
                {
                    ViewBag.Message = result.Message;
                }
                else
                {
                    ViewBag.Message = "Error al consultar el usuario seleccionado";
                }

            }
            return PartialView("Modal");

        }


    //Metodos usados en json de ajax
        public JsonResult GetEstado(int idPais)
        {
            var result = BL.Estado.EstadoGetByIdPais(idPais);

            return Json(result.Objects);
        }

        public JsonResult GetMunicipio(int idEstado)
        {
            var result = BL.Municipio.MunicipioGetByIdEstado(idEstado);

            return Json(result.Objects);
        }

        public JsonResult GetColonia(int idMunicipio)
        {
            var result = BL.Colonia.ColoniaGetByIdMunicipio(idMunicipio);

            return Json(result.Objects);
        }

        public static byte[] ConvertToBytes(IFormFile imagen)
        {
            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }

        public JsonResult CambiarStatus(int idUsuario, int status)
        {
            var result = BL.Usuario.ChangeStatus(idUsuario, status);

            return Json(result);
        }


    }
}
