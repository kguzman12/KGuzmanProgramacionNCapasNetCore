using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.OleDb;

namespace BL
{
    public class Usuario
    {
        public static ML.Result GetAll(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    var usuarios = context.Usuarios.FromSqlRaw($"UsuarioGetAll '{usuario.Nombre}', '{usuario.ApellidoPaterno}'").ToList();

                    result.Objects = new List<object>();

                    if (usuarios != null)
                    {
                        foreach (var row in usuarios)
                        {
                            ML.Usuario usuarioObj = new ML.Usuario();
                            usuarioObj.IdUsuario = row.IdUsuario;
                            usuarioObj.Nombre = row.Nombre;
                            usuarioObj.ApellidoPaterno = row.ApellidoPaterno;
                            usuarioObj.ApellidoMaterno = row.ApellidoMaterno;
                            usuarioObj.FechaNacimiento = row.FechaNacimiento.Value.ToString("dd-MM-yyyy");
                            usuarioObj.Email = row.Email;
                            usuarioObj.Password = row.Password;
                            usuarioObj.UserName = row.UserName;
                            usuarioObj.Sexo = row.Sexo;
                            usuarioObj.Telefono = row.Telefono;
                            usuarioObj.Celular = row.Celular;
                            usuarioObj.Curp = row.Curp;
                            usuarioObj.Imagen = row.Imagen;

                            //Llave forenea
                            usuarioObj.Rol = new ML.Rol();
                            usuarioObj.Rol.IdRol = row.IdRol.Value;
                            usuarioObj.Rol.Nombre = row.RolNombre;

                            //Direccion
                            usuarioObj.Direccion = new ML.Direccion();
                            usuarioObj.Direccion.IdDireccion = (byte)row.IdDireccion;
                            usuarioObj.Direccion.Calle = row.Calle;
                            usuarioObj.Direccion.NumeroInterior = row.NumeroInterior;
                            usuarioObj.Direccion.NumeroExterior = row.NumeroExterior;

                            usuarioObj.Direccion.Colonia = new ML.Colonia();
                            usuarioObj.Direccion.Colonia.IdColonia = (byte)row.IdColonia;
                            usuarioObj.Direccion.Colonia.Nombre = row.NombreColonia;

                            usuarioObj.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuarioObj.Direccion.Colonia.Municipio.IdMunicipio = (byte)row.IdMunicipio;
                            usuarioObj.Direccion.Colonia.Municipio.Nombre = row.NombreMunicipio;

                            usuarioObj.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                            usuarioObj.Direccion.Colonia.Municipio.Estado.IdEstado = (byte)row.IdEstado;
                            usuarioObj.Direccion.Colonia.Municipio.Estado.Nombre = row.NombreEstado;

                            usuarioObj.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                            usuarioObj.Direccion.Colonia.Municipio.Estado.Pais.IdPais = (byte)row.IdPais;
                            usuarioObj.Direccion.Colonia.Municipio.Estado.Pais.Nombre = row.NombrePais;

                            result.Objects.Add(usuarioObj);
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

        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.Nombre}', '{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}', '{usuario.FechaNacimiento}', '{usuario.Email}', '{usuario.Password}', '{usuario.UserName}', '{usuario.Sexo}', '{usuario.Telefono}', '{usuario.Celular}', '{usuario.Curp}', {usuario.Rol.IdRol}, '{usuario.Imagen}', '{usuario.Direccion.Calle}', '{usuario.Direccion.NumeroInterior}', '{usuario.Direccion.NumeroExterior}', {usuario.Direccion.Colonia.IdColonia}");

                    if(query > 0)
                    {
                        result.Message = "El usuario se registro corectamente";
                    }
                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
                result.Message = "Error al registrar el usuarios";
            }
            return result;

        }

        public static ML.Result GetById(int idUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetById {idUsuario}").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Nombre = query.Nombre;
                        usuario.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.FechaNacimiento = query.FechaNacimiento.Value.ToString("dd-MM-yyyy");
                        usuario.Email = query.Email;
                        usuario.Password = query.Password;
                        usuario.UserName = query.UserName;
                        usuario.Sexo = query.Sexo;
                        usuario.Telefono = query.Telefono;
                        usuario.Celular = query.Celular;
                        usuario.Curp = query.Curp;
                        usuario.Imagen = query.Imagen;

                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = query.IdRol.Value;
                        usuario.Rol.Nombre = query.RolNombre;

                        usuario.Direccion = new ML.Direccion();
                        usuario.Direccion.IdDireccion = (byte)query.IdDireccion;
                        usuario.Direccion.Calle = query.Calle;
                        usuario.Direccion.NumeroInterior = query.NumeroInterior;
                        usuario.Direccion.NumeroExterior = query.NumeroExterior;

                        usuario.Direccion.Colonia = new ML.Colonia();
                        usuario.Direccion.Colonia.IdColonia = (byte)query.IdColonia;
                        usuario.Direccion.Colonia.Nombre = query.NombreColonia;

                        usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                        usuario.Direccion.Colonia.Municipio.IdMunicipio = (byte)query.IdMunicipio;
                        usuario.Direccion.Colonia.Municipio.Nombre = query.NombreMunicipio;

                        usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                        usuario.Direccion.Colonia.Municipio.Estado.IdEstado = (byte)query.IdEstado;
                        usuario.Direccion.Colonia.Municipio.Estado.Nombre = query.NombreEstado;

                        usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = (byte)query.IdPais;
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = query.NombrePais;

                        result.Object = usuario;
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

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"UsuarioUpdate {usuario.IdUsuario}, '{usuario.Nombre}', '{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}', '{usuario.FechaNacimiento}', '{usuario.Email}', '{usuario.Password}', '{usuario.UserName}', '{usuario.Sexo}', '{usuario.Telefono}', '{usuario.Celular}', '{usuario.Curp}', {usuario.Rol.IdRol}, '{usuario.Imagen}', '{usuario.Direccion.Calle}', '{usuario.Direccion.NumeroInterior}', '{usuario.Direccion.NumeroExterior}', {usuario.Direccion.Colonia.IdColonia}");

                    if (query > 0)
                    {
                        result.Message = "El usuario se actualizo corectamente";
                    }

                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Error al actualizar el usuario" + result.Ex;

                throw;
            }
            return result;

        }

        public static ML.Result Delete(int idUsuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"UsuarioDelete {idUsuario}");

                    if (query > 0)
                    {
                        result.Message = "El usuario se elimino corectamente";
                    }

                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Error al elimino el usuario" + result.Ex;

                throw;
            }
            return result;

        }


        //Metodos usados para cargar y validar el excel
        public static ML.Result ConvertirExceltoDataTable(string connString)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (OleDbConnection context = new OleDbConnection(connString))
                {
                    string query = "SELECT * FROM [Sheet1$]";
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Connection = context;

                        OleDbDataAdapter da = new OleDbDataAdapter();
                        da.SelectCommand = cmd;

                        DataTable tableUsuario = new DataTable();

                        da.Fill(tableUsuario);

                        if (tableUsuario.Rows.Count > 0)
                        {
                            result.Objects = new List<object>();

                            foreach (DataRow row in tableUsuario.Rows)
                            {
                                ML.Usuario usuario = new ML.Usuario();

                                usuario.Nombre = row[0].ToString();
                                usuario.ApellidoPaterno = row[1].ToString();
                                usuario.ApellidoMaterno = row[2].ToString();
                                usuario.FechaNacimiento = row[3].ToString();
                                usuario.Email = row[4].ToString();
                                usuario.Password = row[5].ToString();
                                usuario.UserName = row[6].ToString();
                                usuario.Sexo = row[7].ToString();
                                usuario.Telefono = row[8].ToString();
                                usuario.Celular = row[9].ToString();
                                usuario.Curp = row[10].ToString();

                                usuario.Rol = new ML.Rol();
                                usuario.Rol.IdRol = int.Parse(row[11].ToString());

                                //usuario.Imagen = row[12].ToString();

                                usuario.Direccion = new ML.Direccion();
                                usuario.Direccion.Calle = row[12].ToString();
                                usuario.Direccion.NumeroInterior = row[13].ToString();
                                usuario.Direccion.NumeroExterior = row[14].ToString();

                                usuario.Direccion.Colonia = new ML.Colonia();
                                usuario.Direccion.Colonia.IdColonia = byte.Parse(row[15].ToString());

                                result.Objects.Add(usuario);
                            }
                            result.Correct = true;
                        }

                        if (tableUsuario.Rows.Count > 1)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.Message = "No existen registros en el archivo de excel";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Error al registrar los usuarios" + result.Ex;

            }
            return result;
        }
    
        public static ML.Result ValidarExcel(List<object> Object)
        {
            ML.Result result = new ML.Result();

            try
            {
                result.Objects = new List<object>();
                int i = 1;

                foreach (ML.Usuario usuario in Object)
                {
                    ML.ErrorExcel error = new ML.ErrorExcel();
                    error.IdRegistro = i++;

                    usuario.Nombre = (usuario.Nombre == "") ? error.Mensaje += "Ingresar nombre " : usuario.Nombre;
                    usuario.ApellidoPaterno = (usuario.ApellidoPaterno == "") ? error.Mensaje += "Ingresar apellido paterno " : usuario.ApellidoPaterno;
                    usuario.ApellidoMaterno = (usuario.ApellidoMaterno == "") ? error.Mensaje += "Ingresar apellido materno " : usuario.ApellidoMaterno;
                    usuario.FechaNacimiento = (usuario.FechaNacimiento == "") ? error.Mensaje += "Ingresar fecha de nacimiento " : usuario.FechaNacimiento;
                    usuario.Email = (usuario.Email == "") ? error.Mensaje += "Ingresar email " : usuario.Email;
                    usuario.Password = (usuario.Password == "") ? error.Mensaje += "Ingresar password " : usuario.Password;
                    usuario.UserName = (usuario.UserName == "") ? error.Mensaje += "Ingresar username " : usuario.UserName;
                    usuario.Sexo = (usuario.Sexo == "") ? error.Mensaje += "Ingresar sexo " : usuario.Sexo;
                    usuario.Telefono = (usuario.Telefono == "") ? error.Mensaje += "Ingresar telefono " : usuario.Telefono;
                    usuario.Celular = (usuario.Celular == "") ? error.Mensaje += "Ingresar celular " : usuario.Celular;
                    usuario.Curp = (usuario.Curp == "") ? error.Mensaje += "Ingresar curp " : usuario.Curp;
                    //usuario.Rol.IdRol = (usuario.Rol.IdRol.ToString() == "") ? error.Mensaje += "Ingresar el rol" : usuario.Rol.IdRol;
                    if (usuario.Rol.IdRol.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el rol ";
                    }
                    usuario.Direccion.Calle = (usuario.Direccion.Calle == "") ? error.Mensaje += "Ingresar calle " : usuario.Direccion.Calle;
                    usuario.Direccion.NumeroInterior = (usuario.Direccion.NumeroInterior == "") ? error.Mensaje += "Ingresar numero interior " : usuario.Direccion.NumeroInterior;
                    usuario.Direccion.NumeroExterior = (usuario.Direccion.NumeroExterior == "") ? error.Mensaje += "Ingresar numero exterior " : usuario.Direccion.NumeroExterior;
                    //usuario.Direccion.Colonia.IdColonia = (usuario.Direccion.Colonia.IdColonia.ToString() == "") ? error.Mensaje += "Ingresar la colonia" : usuario.Direccion.Colonia.IdColonia;
                    if (usuario.Direccion.Colonia.IdColonia.ToString() == "")
                    {
                        error.Mensaje += "Ingresar la colonia ";
                    }

                    if (error.Mensaje != null)
                    {
                        result.Objects.Add(error);
                    }

                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Error en los datos del usuario" + result.Ex;
            }
            return result;
        }

        public static ML.Result ChangeStatus(int idUsuario, int status)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.KguzmanProgramacionNcapasContext context = new DL.KguzmanProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioChangeStatus {idUsuario}, {status}");

                    if (query > 0)
                    {
                        result.Message = "El status se actaulizo corectamente";
                    }
                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
                result.Message = "Error al actualizar el status";
            }
            return result;

        }


    }
}