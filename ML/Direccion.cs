using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Direccion
    {
        public byte IdDireccion { get; set; }
        public string Calle { get; set; }
        [DisplayName("Num. Interior")]
        public string NumeroInterior { get; set; }
        [DisplayName("Num. Exterior")]
        public string NumeroExterior { get; set; }


        public ML.Colonia Colonia { get; set; } //Foreign key

        public ML.Usuario Usuario { get; set; } //Foreign key
    }
}
