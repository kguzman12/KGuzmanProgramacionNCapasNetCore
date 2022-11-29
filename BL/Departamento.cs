using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Departamento
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    var departamentos = context.Departamentos.FromSqlRaw("DepartamentoGetAll").ToList();

                    result.Objects = new List<object>();

                    if (departamentos != null)
                    {
                        foreach (var row in departamentos)
                        {
                            ML.Departamento departamento = new ML.Departamento();
                            departamento.IdDepartamento = row.IdDepartamento;
                            departamento.Nombre = row.Nombre;

                        //Llave forenea
                            departamento.Area = new ML.Area();
                            departamento.Area.IdArea = (byte)row.IdArea;
                            departamento.Area.Nombre = row.NombreArea;

                            result.Objects.Add(departamento);
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

        public static ML.Result Add(ML.Departamento departamento)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"DepartamentoAdd '{departamento.Nombre}', {departamento.Area.IdArea}");

                    if (query > 0)
                    {
                        result.Message = "El departamento se registro corectamente";
                    }
                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
                result.Message = "Error al registrar el departamento";
            }
            return result;

        }

        public static ML.Result GetById(int idDepartamento)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    var query = context.Departamentos.FromSqlRaw($"DepartamentoGetById {idDepartamento}").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Departamento departamento = new ML.Departamento();

                        departamento.IdDepartamento = (byte)query.IdDepartamento;
                        departamento.Nombre = query.Nombre;
                        
                        departamento.Area = new ML.Area();
                        departamento.Area.IdArea = (byte)query.IdArea;
                        departamento.Area.Nombre = query.NombreArea;

                        result.Object = departamento;
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

        public static ML.Result Update(ML.Departamento departamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"DepartamentoUpdate {departamento.IdDepartamento}, '{departamento.Nombre}', {departamento.Area.IdArea}");

                    if (query > 0)
                    {
                        result.Message = "El departamento se actualizo corectamente";
                    }

                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Error al actualizar el departamento" + result.Ex;

                throw;
            }
            return result;

        }

        public static ML.Result Delete(int idDepartamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"DepartamentoDelete {idDepartamento}");

                    if (query > 0)
                    {
                        result.Message = "El departamento se elimino corectamente";
                    }

                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Error al eliminar el departamento" + result.Ex;

                throw;
            }
            return result;

        }


    //Drop down list       
        public static ML.Result DepartamentoGetByIdArea(int idArea)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    var departamentos = context.Departamentos.FromSqlRaw($"DepartamentoGetByIdArea {idArea}").ToList();
                    result.Objects = new List<object>();
                    if (departamentos != null)
                    {
                        foreach (var row in departamentos)
                        {
                            ML.Departamento departamento = new ML.Departamento();
                            departamento.IdDepartamento = row.IdDepartamento;
                            departamento.Nombre = row.Nombre;

                            departamento.Area = new ML.Area();
                            departamento.Area.IdArea = (byte)row.IdArea;

                            result.Objects.Add(departamento);

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
