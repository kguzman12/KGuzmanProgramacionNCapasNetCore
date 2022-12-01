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

        [HttpPost]
        public ActionResult GetAll(ML.Producto producto)
        {
            producto.Nombre = (producto.Nombre == null) ? "" : producto.Nombre;
            producto.Proveedor.IdProveedor = (producto.Proveedor.IdProveedor == null) ? 0 : producto.Proveedor.IdProveedor;
            producto.Departamento.IdDepartamento = (producto.Departamento.IdDepartamento == null) ? 0 : producto.Departamento.IdDepartamento;
            //producto.Departamento.Area = (producto.Departamento.Area == null) ? "" : producto.Departamento.Area;

            ML.Result result = BL.Producto.GetAll(producto);
            producto.Productos = result.Objects;

            ML.Result resultProveedor = BL.Proveedor.GetAll();
            producto.Proveedor.Proveedores = resultProveedor.Objects;

            ML.Result resultDepartamento = BL.Departamento.GetAll();
            producto.Departamento.Departamentos = resultDepartamento.Objects;

            ML.Result resultArea = BL.Area.GetAll();
            producto.Departamento.Area.Areas = resultArea.Objects;

            return View(producto);
        }

        public IActionResult Card()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CardPost(ML.Producto producto)
        {
            ML.VentaProducto ventaProducto = new ML.VentaProducto();
            ventaProducto.VentasProductos = new List<object>();

            if (HttpContext.Session.GetString("Producto") == null)
            {
                producto.Cantidad = producto.Cantidad = 1;
                producto.Total =  producto.Total = (int)(producto.Cantidad * producto.PrecioUnitario);
                
                ventaProducto.VentasProductos.Add(producto);
                HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(ventaProducto.VentasProductos));
                var session = HttpContext.Session.GetString("Producto");
            }
            else
            {
                var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Producto"));

                foreach (var obj in ventaSession)
                {
                    ML.Producto objProducto = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(obj.ToString());
                    ventaProducto.VentasProductos.Add(objProducto);
                }
                
                foreach (ML.Producto venta in ventaProducto.VentasProductos.ToList())
                {
                    if (producto.IdProducto == venta.IdProducto)
                    {
                        venta.Cantidad = venta.Cantidad + 1;                  
                    }
                    else
                    {
                        producto.Cantidad = producto.Cantidad = 1;
                        ventaProducto.VentasProductos.Add(producto);
                    }
                }
                HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(ventaProducto.VentasProductos));
            }
            if (HttpContext.Session.GetString("Producto") != null)
            {
                ViewBag.Message = "Se ha agregado el producto al carrito";
                return PartialView("Modal");
            }
            else
            {
                ViewBag.Message = "No se pudo agregar el producto ):";
                return PartialView("Modal");
            }

        }

        [HttpGet]
        public ActionResult ResumenCompra(ML.VentaProducto ventaProducto)
        {
            if (HttpContext.Session.GetString("Producto") == null)
            {
                return View();
            }
            else
            {
                var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Producto"));
                ventaProducto.VentasProductos = new List<object>();

                foreach (var obj in ventaSession)
                {
                    ML.Producto objProducto = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(obj.ToString());
                    ventaProducto.VentasProductos.Add(objProducto);
                }

            }

            return View(ventaProducto);
        }

    }
}
