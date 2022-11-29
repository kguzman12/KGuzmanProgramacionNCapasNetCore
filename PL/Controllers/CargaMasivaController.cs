using Microsoft.AspNetCore.Mvc;
using ML;
using System.IO;

namespace PL.Controllers
{
    public class CargaMasivaController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public CargaMasivaController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult UsuarioCargaMasiva()
        {
            ML.Result result = new Result();
            return View(result);
        }

        [HttpPost]
        public ActionResult CargaTxt()
        {
            IFormFile fileTxt = Request.Form.Files["archivoTxt"];


            if (fileTxt != null)
            {

                StreamReader Textfile = new StreamReader(fileTxt.OpenReadStream());

                string line;
                line = Textfile.ReadLine();
                while ((line = Textfile.ReadLine()) != null)
                {
                    string[] lines = line.Split('|');

                    ML.Usuario usuario = new ML.Usuario();

                    usuario.Nombre = lines[0];
                    usuario.ApellidoPaterno = lines[1];
                    usuario.ApellidoMaterno = lines[2];
                    usuario.FechaNacimiento = lines[3];
                    usuario.Email = lines[4];
                    usuario.Password = lines[5];
                    usuario.UserName = lines[6];
                    usuario.Sexo = lines[7];
                    usuario.Telefono = lines[8];
                    usuario.Celular = lines[9];
                    usuario.Curp = lines[10];

                    usuario.Rol = new ML.Rol();
                    usuario.Rol.IdRol = int.Parse(lines[11]);

                    usuario.Imagen = lines[12];

                    usuario.Direccion = new ML.Direccion();
                    usuario.Direccion.Calle = lines[13];
                    usuario.Direccion.NumeroInterior = lines[14];
                    usuario.Direccion.NumeroExterior = lines[15];

                    usuario.Direccion.Colonia = new ML.Colonia();
                    usuario.Direccion.Colonia.IdColonia = byte.Parse(lines[16]);

                    ML.Result result = BL.Usuario.Add(usuario);


                    if (!result.Correct)
                    {
                        string fileError = @"C:\Users\digis\Documents\Ana karen Guzman Rodriguez\ErroresLayoutUsuarios.txt";
                        //result.ErrorMessage;
                        StreamWriter errorFile = new StreamWriter(fileError);
                        
                    }
                    else
                    {


                    }

                }
            }
            return PartialView("Modal");
        }

        [HttpPost]
        public ActionResult UsuarioCargaMasiva(ML.Usuario usuario)
        {

            IFormFile archivo = Request.Form.Files["FileExcel"]; //recuperamos el archivo de excel

        //Session 
            if (HttpContext.Session.GetString("PathArchivo") == null) //sesion no existe
            {
            //validar el excel
                if (archivo.Length > 0)
                {
                //que sea .xlsx
                    string fileName = Path.GetFileName(archivo.FileName);
                    string folderPath = _configuration["PathFolder:value"];
                    string extensionArchivo = Path.GetExtension(archivo.FileName).ToLower();
                    string extensionModulo = _configuration["TipoExcel"];

                    if (extensionArchivo == extensionModulo)
                    {
                    //crear copia del archivo
                        string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, folderPath, Path.GetFileNameWithoutExtension(fileName)) + '-' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                        if (!System.IO.File.Exists(filePath))
                        {
                            using (FileStream stream = new FileStream(filePath, FileMode.Create))
                            {
                                archivo.CopyTo(stream);
                            }

                            //lectura del archivo
                            string connectionString = _configuration["ConnectionStringExcel:value"] + filePath;
                            ML.Result resultConvertExcel = BL.Usuario.ConvertirExceltoDataTable(connectionString);

                            if (resultConvertExcel.Correct)
                            {
                                ML.Result resultValidacion = BL.Usuario.ValidarExcel(resultConvertExcel.Objects);
                                if (resultValidacion.Objects.Count == 0)
                                {
                                    resultValidacion.Correct = true;
                                    HttpContext.Session.SetString("PathArchivo", filePath);
                                }
                                return View(resultValidacion);
                            }
                            else
                            {
                                ViewBag.Message = "Error al leer el archivo";
                                return View("Modal");
                            }
                        }
                    }
                }
            }
            else
            {
                string rutaArchivoExcel = HttpContext.Session.GetString("PathArchivo");
                string connectionString = _configuration["ConnectionStringExcel:value"] + rutaArchivoExcel;

                ML.Result resultData = BL.Usuario.ConvertirExceltoDataTable(connectionString);
                if (resultData.Correct)
                {
                    ML.Result resultErrores = new ML.Result();
                    resultErrores.Objects = new List<object>();

                    foreach (ML.Usuario usuarioItem in resultData.Objects)
                    {

                        ML.Result resultAdd = BL.Usuario.Add(usuarioItem);
                        if (!resultAdd.Correct)
                        {
                            resultErrores.Objects.Add("No se insertó el usuario con nombre: " + usuarioItem.Nombre + " Error: " + resultAdd.Message);
                        }
                    }
                    if (resultErrores.Objects.Count > 0)
                    {

                        //string fileError = Path.Combine(_hostingEnvironment.WebRootPath, @"~\Files\logErrores.txt");
                        string fileError = _hostingEnvironment.WebRootPath + @"\Files\logErrores.txt";
                        using (StreamWriter writer = new StreamWriter(fileError))
                        {
                            foreach (string ln in resultErrores.Objects)
                            {
                                writer.WriteLine(ln);
                            }
                        }
                        ViewBag.Message = "Los usuarios no se registraron correctamente";
                    }
                    else
                    {
                        ViewBag.Message = "Los usuarios se registraron correctamente";
                    }

                }
            }
            return PartialView("Modal");
        }

    }
}
