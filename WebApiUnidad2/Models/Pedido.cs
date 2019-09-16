using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUnidad2.Models
{
    public class Pedido
    {
        public int idPlatillo { get; set; }
        public string nombreCompleto { get; set; }
        public string direccion { get; set; }
        public string comuna { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public int idTipoPago { get; set; }
    }
}
