using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Rol
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    var query = context.Rols.FromSqlRaw("RolGetAll").ToList();

                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        foreach (var row in query)
                        {
                            ML.Rol rol = new ML.Rol();
                            rol.IdRol = (byte)row.IdRol;
                            rol.Nombre = row.Nombre;

                            result.Objects.Add(rol);
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



    }
}

