using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Producto
    {
        public static ML.Result GetAll(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                        producto.Proveedor.IdProveedor = (producto.Proveedor.IdProveedor == null) ? 0 : producto.Proveedor.IdProveedor;
                    var productos = context.Productos.FromSqlRaw($"ProductoGetAll '{producto.Nombre}', {producto.Proveedor.IdProveedor}").ToList();

                    result.Objects = new List<object>();

                    if (productos != null)
                    {
                        foreach (var row in productos)
                        {
                            ML.Producto productoObj = new ML.Producto();
                            productoObj.IdProducto = row.IdProducto;
                            productoObj.Nombre = row.Nombre;
                            productoObj.PrecioUnitario = row.PrecioUnitario;
                            productoObj.Stock = row.Stock;
                            productoObj.Descripcion = row.Descripcion;
                            productoObj.Imagen = row.Imagen;

                        //Llave forenea
                            productoObj.Proveedor = new ML.Proveedor();
                            productoObj.Proveedor.IdProveedor = (int)row.IdProveedor.Value;
                            productoObj.Proveedor.Nombre = row.NombreProveedor;
                            productoObj.Proveedor.Telefono = row.Telefono;

                            productoObj.Departamento = new ML.Departamento();
                            productoObj.Departamento.IdDepartamento = (int)row.IdDepartamento.Value;
                            productoObj.Departamento.Nombre = row.NombreDepartamento;

                            result.Objects.Add(productoObj);
                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se encontraron registros";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Error al consultar los registros" + result.Ex;

                throw;
            }
            return result;

        }

        public static ML.Result Add(ML.Producto producto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"ProductoAdd '{producto.Nombre}', '{producto.PrecioUnitario}', '{producto.Stock}', '{producto.Descripcion}', '{producto.Imagen}', '{producto.Proveedor.IdProveedor}', '{producto.Departamento.IdDepartamento}'");

                    if (query > 0)
                    {
                        result.Message = "El producto se registro corectamente";
                    }
                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
                result.Message = "Error al registrar el producto";
            }
            return result;

        }

        public static ML.Result GetById(int idProducto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    var query = context.Productos.FromSqlRaw($"ProductoGetById {idProducto}").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Producto producto = new ML.Producto();

                        producto.IdProducto = query.IdProducto;
                        producto.Nombre = query.Nombre;
                        producto.PrecioUnitario = query.PrecioUnitario;
                        producto.Stock = query.Stock;
                        producto.Descripcion = query.Descripcion;
                        producto.Imagen = query.Imagen;

                        producto.Proveedor = new ML.Proveedor();
                        producto.Proveedor.IdProveedor = (byte)query.IdProveedor;
                        producto.Proveedor.Nombre = query.NombreProveedor;

                        producto.Departamento = new ML.Departamento();
                        producto.Departamento.IdDepartamento = (byte)query.IdDepartamento;
                        producto.Departamento.Nombre = query.NombreDepartamento;

                        result.Object = producto;
                    }
                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public static ML.Result Update(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"ProductoUpdate {producto.IdProducto}, '{producto.Nombre}', '{producto.PrecioUnitario}', '{producto.Stock}', '{producto.Descripcion}', '{producto.Imagen}', '{producto.Proveedor.IdProveedor}', {producto.Departamento.IdDepartamento}");


                    if (query > 0)
                    {
                        result.Message = "El producto se actualizo corectamente";
                    }

                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Error al actualizar el producto" + result.Ex;

                throw;
            }
            return result;

        }

        public static ML.Result Delete(int idProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"ProductoDelete {idProducto}");

                    if (query > 0)
                    {
                        result.Message = "El producto se elimino corectamente";
                    }

                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Error al elimino el producto" + result.Ex;

                throw;
            }
            return result;

        }

    }
}
