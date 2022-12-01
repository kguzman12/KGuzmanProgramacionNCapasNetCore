using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class VentaProducto
    {
        public int IdVentaproducto { get; set; }
        public string Venta { get; set; }
        public int Cantidad { get; set; }

        public string Total { get; set; }

        public List<Object> VentasProductos { get; set; }

        ML.Producto Producto { get; set; }
    }
}
