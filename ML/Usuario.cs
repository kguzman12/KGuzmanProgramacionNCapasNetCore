using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ML
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        [Required]
        public string Nombre { get; set; }
        [DisplayName("Apellido Paterno")]
        public string ApellidoPaterno { get; set; }
        [DisplayName("Apellido Materno")]
        public string ApellidoMaterno { get; set; }
        [DisplayName("Fecha de nacimiento")]
        public string FechaNacimiento { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Sexo { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Curp { get; set; }
        public string Imagen { get; set; }
        public bool Status { get; set; }

        public ML.Rol Rol { get; set; } //Foreign key


        public List<Object> Usuarios { get; set; }

    //Propiedades de navegacion
        public ML.Direccion Direccion { get; set; }




    }
}