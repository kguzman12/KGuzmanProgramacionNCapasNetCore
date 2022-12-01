using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class ProductoController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Producto producto = new ML.Producto();
            ML.Result result = new ML.Result();

            producto.Proveedor = new ML.Proveedor();
            ML.Result resultProveedor = BL.Proveedor.GetAll();

            result = BL.Producto.GetAll(producto);

            if (result.Correct)
            {
                producto.Proveedor.Proveedores = resultProveedor.Objects;
                producto.Productos = result.Objects;
            }
            else
            {
                ViewBag.Message = "Error al realizar la consulta";
            }

            return View(producto);
        }

        [HttpPost]
        public ActionResult GetAll(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            ML.Result resultProveedor = BL.Proveedor.GetAll();
            result = BL.Producto.GetAll(producto); 

            if (result.Correct)
            {
                producto.Proveedor.Proveedores = resultProveedor.Objects;
                producto.Productos = result.Objects;
            }
            else
            {
                ViewBag.Message = "Error al realizar la consulta";
            }

            return View(producto);
        }

        [HttpGet]
        public ActionResult Form(int? idProducto)
        {
            ML.Producto producto = new ML.Producto();
          
            producto.Proveedor = new ML.Proveedor(); 
            producto.Departamento = new ML.Departamento();
            producto.Departamento.Area = new ML.Area();

            ML.Result resultProveedor = BL.Proveedor.GetAll();
            ML.Result resultDepartamento = BL.Departamento.GetAll();
            ML.Result resultArea = BL.Area.GetAll();

            if (idProducto == null)
            {
                producto.Proveedor.Proveedores = resultProveedor.Objects;
                producto.Departamento.Departamentos = resultDepartamento.Objects;
                producto.Departamento.Area.Areas = resultArea.Objects;

                return View(producto);
            }
            else
            {
                ML.Result result = BL.Producto.GetById(idProducto.Value);

                if (result.Correct)
                {
                    producto = (ML.Producto)result.Object;
                    producto.Proveedor.Proveedores = resultProveedor.Objects;
                    producto.Departamento.Departamentos = resultDepartamento.Objects;
                    producto.Departamento.Area.Areas = resultArea.Objects;

                    ML.Result resultDepartamentos = BL.Departamento.DepartamentoGetByIdArea(producto.Departamento.Area.IdArea);
                    producto.Departamento.Departamentos = resultDepartamentos.Objects;

                    return View(producto);
                }
                else
                {
                    ViewBag.Message = "Error al consultar el producto seleccionado";
                }
                return View(producto);
            }

        }

        [HttpPost]
        public ActionResult Form(ML.Producto producto)
        {
            IFormFile img = Request.Form.Files["ImagenData"];

            if (img != null)
            {
                byte[] ImagenByte = ConvertToBytes(img);

                producto.Imagen = Convert.ToBase64String(ImagenByte);
            }

            if (producto.IdProducto == 0)
            {
                ML.Result result = BL.Producto.Add(producto);

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
                ML.Result result = BL.Producto.Update(producto);

                if (result.Correct)
                {
                    ViewBag.Message = result.Message;
                }
                else
                {
                    ViewBag.Message = "Error al actualizar al producto";
                }
            }

            return PartialView("Modal");
        }

        [HttpGet]
        public ActionResult Delete(int idProducto)
        {
            if (idProducto >= 1)
            {
                ML.Result result = BL.Producto.Delete(idProducto);

                if (result.Correct)
                {
                    ViewBag.Message = result.Message;
                }
                else
                {
                    ViewBag.Message = "Error al consultar el producto seleccionado";
                }

            }
            return PartialView("Modal");

        }


        //Metodos usados en json de ajax
        public JsonResult GetDepartamento(int idArea)
        {
            var result = BL.Departamento.DepartamentoGetByIdArea(idArea);

            return Json(result.Objects);
        }

        public static byte[] ConvertToBytes(IFormFile imagen)
        {
            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }



    }
}
