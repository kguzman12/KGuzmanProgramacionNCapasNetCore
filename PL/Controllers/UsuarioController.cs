using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public UsuarioController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Result resulRol = BL.Rol.GetAll();
            ML.Result result = new ML.Result();
            ML.Usuario usuario = new ML.Usuario();

            usuario.Rol = new ML.Rol();
            resulRol.Objects = new List<object>();
            result.Objects = new List<object>();

            //result = BL.Usuario.GetAll(usuario);

            try
            {
                string urlAPI = _configuration["UrlAPI"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);

                    var responseTask = client.GetAsync("Usuario/GetAll");

                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Usuario resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(resultItem.ToString());
                            result.Objects.Add(resultItemList);
                        }

                        result.Correct = true;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }

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
            ML.Result result = new ML.Result();
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
                //ML.Result result = BL.Usuario.GetById(idUsuario.Value);
                try
                {
                    string urlAPI = System.Configuration.ConfigurationManager.AppSettings["URLapi"];
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(urlAPI);
                        var responseTask = client.GetAsync("Usuario/GetById/" + idUsuario);
                        responseTask.Wait();
                        var resultAPI = responseTask.Result;
                        if (resultAPI.IsSuccessStatusCode)
                        {
                            var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                            readTask.Wait();

                            //ML.Usuario resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(resultItem.ToString());
                            //result.Objects.Add(resultItemList);

                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.Message = "No existen registros en la tabla Departamento";
                        }

                    }
                }

                catch (Exception ex)
                {
                    result.Correct = false;
                    result.Message = ex.Message;

                }

                //return result;
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
            ML.Result result = new ML.Result();

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
                    //ML.Result result = BL.Usuario.Add(usuario);
                    string urlAPI = _configuration["UrlAPI"];
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(urlAPI);

                        //HTTP POST
                        var postTask = client.PostAsJsonAsync<ML.Usuario>("Usuario/Add", usuario);
                        postTask.Wait();

                        var resultPost = postTask.Result;
                        if (resultPost.IsSuccessStatusCode)
                        {
                            return RedirectToAction("GetAll");
                        }
                    }

                    //return View("GetAll");

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
                    //ML.Result result = BL.Usuario.Update(usuario);
                    string urlAPI = _configuration["UrlAPI"];
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(urlAPI);

                        //HTTP POST
                        var postTask = client.PostAsJsonAsync<ML.Usuario>("Usuario/update", usuario);
                        postTask.Wait();

                        var resultPost = postTask.Result;
                        if (resultPost.IsSuccessStatusCode)
                        {
                            return RedirectToAction("GetAll");
                        }
                    }

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
                //ML.Result result = BL.Usuario.Delete(idUsuario);

                ML.Result resultList = new ML.Result();
                
                string urlAPI = _configuration["UrlAPI"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);

                    //HTTP POST
                    var postTask = client.GetAsync("Usuario/delete/" + idUsuario);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        resultList = BL.Usuario.Delete(idUsuario);
                        return RedirectToAction("GetAll", resultList);
                    }
                }

                if (resultList.Correct)
                {
                    ViewBag.Message = resultList.Message;
                }
                else
                {
                    ViewBag.Message = "Error al consultar el usuario seleccionado";
                }

            }
            return PartialView("Modal");

        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            //ML.Result result = BL.Usuario.GetByUserName(userName);
            ML.Result result = new ML.Result();

            string urlAPI = _configuration["UrlAPI"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);

                //HTTP POST
                var postTask = client.GetAsync("Usuario/login/" + userName);
                postTask.Wait();

                var resultPost = postTask.Result;
                if (resultPost.IsSuccessStatusCode)
                {
                    result = BL.Usuario.GetByUserName(userName);
                    return RedirectToAction("GetAll", result);
                }
            }

            if (result.Correct)
            {
                ML.Usuario usuario = new ML.Usuario();

                if (usuario.Password == password)
                {

                }
                else
                {
                    ViewBag.Message = "El usuario o la contraseña con incorrectos";
                    return PartialView("Modal");
                }
            }
            else
            {
                ViewBag.Message = "El usuario o la contraseña con incorrectos";
                return PartialView("Modal");
            }
            return View();
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
