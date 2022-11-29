using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Municipio
    {
        public static ML.Result MunicipioGetByIdEstado(int idEstado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    var usuarios = context.Municipios.FromSqlRaw($"MunicipioGetByIdEstado {idEstado}").ToList();

                    result.Objects = new List<object>();
                    if (usuarios != null)
                    {
                        foreach (var row in usuarios)
                        {
                            ML.Municipio municipio = new ML.Municipio();
                            municipio.IdMunicipio = (byte)row.IdMunicipio;
                            municipio.Nombre = row.Nombre;

                            municipio.Estado = new ML.Estado();
                            municipio.Estado.IdEstado = (byte)idEstado;

                            result.Objects.Add(municipio);

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
