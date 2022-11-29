using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Estado
    {
        public static ML.Result EstadoGetByIdPais(int idPais)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    var usuarios = context.Estados.FromSqlRaw($"EstadoGetByIdPais {idPais}").ToList();
                    result.Objects = new List<object>();
                    if (usuarios != null)
                    {
                        foreach (var row in usuarios)
                        {
                            ML.Estado estado = new ML.Estado();
                            estado.IdEstado = (byte)row.IdEstado;
                            estado.Nombre = row.Nombre;

                            estado.Pais = new ML.Pais();
                            estado.Pais.IdPais = (byte)idPais;
                            result.Objects.Add(estado);

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
