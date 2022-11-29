using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class VentaProductoController : Controller
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

    }
}
