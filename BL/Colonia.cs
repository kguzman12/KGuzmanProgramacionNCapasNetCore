using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Colonia
    {
        public static ML.Result ColoniaGetByIdMunicipio(int idMunicipio)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    var usuarios = context.Colonia.FromSqlRaw($"ColoniaGetByIdMunicipio {idMunicipio}").ToList();
                    result.Objects = new List<object>();
                    if (usuarios != null)
                    {
                        foreach (var row in usuarios)
                        {
                            ML.Colonia colonia = new ML.Colonia();
                            colonia.IdColonia = (byte)row.IdColonia;
                            colonia.Nombre = row.Nombre;

                            colonia.Municipio = new ML.Municipio();
                            colonia.Municipio.IdMunicipio = (byte)idMunicipio;

                            result.Objects.Add(colonia);

                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se ha podido realizar la consulta";

                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }
            return result;
        }


    }
}
