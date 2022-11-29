using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Colonia
    {
        public byte IdColonia { get; set; }

        public string Nombre { get; set; }

        public int CodigoPostal { get; set; }

        public List<object> Colonias { get; set; }


        public ML.Municipio Municipio { get; set; } //Foreign key



    }
}
