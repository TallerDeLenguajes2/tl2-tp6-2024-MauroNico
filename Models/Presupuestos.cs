using System.Collections.Generic;
using System.Linq;

namespace Tienda.Models
{
    public class Presupuesto
    {
        public int IdPresupuesto { get; set; }
        public string NombreDestinatario { get; set; }
        public string FechaCreacion { get; set; }
        public List<PresupuestoDetalle> Detalle { get; set; } = new List<PresupuestoDetalle>();

       
        public int MontoPresupuesto()
        {
            return Detalle.Sum(d => d.Producto.Precio * d.Cantidad);
        }

        
        public int MontoPresupuestoConIva()
        {
            double iva = 0.21;
            return (int)(MontoPresupuesto() * (1 + iva));
        }

        
        public int CantidadProductos()
        {
            return Detalle.Sum(d => d.Cantidad);
        }
    }
}