
using ML;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

ReadFile();
Console.ReadKey();

static void ReadFile()
{
    string file = @"C:\Users\digis\Documents\Ana karen Guzman Rodriguez\layoutUsuarios.txt";
    if (File.Exists(file))
    {
        StreamReader TextFile = new StreamReader(file);
        string line;
        line = TextFile.ReadLine();

        ML.Result resultErrores = new ML.Result();
        resultErrores.Objects = new List<object>();

        while ((line = TextFile.ReadLine()) != null)
        {
            string[] lines = line.Split('|');

            ML.Usuario usuario = new ML.Usuario();

            usuario.Nombre = lines[0];
            usuario.ApellidoPaterno = lines[1];
            usuario.ApellidoMaterno = lines[2];
            usuario.FechaNacimiento = lines[3];
            usuario.Email = lines[4];
            usuario.Password = lines[5];
            usuario.UserName = lines[6];
            usuario.Sexo = lines[7];
            usuario.Telefono = lines[8];
            usuario.Celular = lines[9];
            usuario.Curp = lines[10];

            usuario.Rol = new ML.Rol();
            usuario.Rol.IdRol = int.Parse(lines[11]);

            usuario.Imagen = lines[12];

            usuario.Direccion = new ML.Direccion();
            usuario.Direccion.Calle = lines[13];
            usuario.Direccion.NumeroInterior = lines[14];
            usuario.Direccion.NumeroExterior = lines[15];

            usuario.Direccion.Colonia = new ML.Colonia();
            usuario.Direccion.Colonia.IdColonia = byte.Parse(lines[16]);

            ML.Result result = BL.Usuario.Add(usuario);

            if (result.Correct)
            {
                Console.WriteLine("Se inserto el usuario correctamente");
                Console.ReadKey();
            }
        }
        if (resultErrores.Objects != null)
        {

        }
        TextWriter fileError = new StreamWriter(@"C:\Users\digis\Documents\Ana karen Guzman Rodriguez\ErroresLayoutUsuarios.txt");
        foreach (string error in resultErrores.Objects)
        {
            fileError.WriteLine(error);
        }
        fileError.Close();
    }
    
}
