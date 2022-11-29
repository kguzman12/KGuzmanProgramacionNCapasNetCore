using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Proveedor
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    var proveedores = context.Proveedors.FromSqlRaw("ProveedorGetAll").ToList();

                    result.Objects = new List<object>();

                    if (proveedores != null)
                    {
                        foreach (var row in proveedores)
                        {
                            ML.Proveedor proveedor = new ML.Proveedor();
                            proveedor.IdProveedor = row.IdProveedor;
                            proveedor.Nombre = row.Nombre;

                            result.Objects.Add(proveedor);
                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se encontraron registros";
                    }
                }
            }//codigo que puede causar una excepcion 
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Error al consultar los registros" + result.Ex;

                throw;
            }//manejo de excepciones 
            return result;

        }

    }
}
