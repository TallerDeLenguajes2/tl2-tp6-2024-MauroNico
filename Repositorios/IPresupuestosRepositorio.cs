
using Tienda.Models;

namespace Tienda.Repositorios
{
    public interface IPresupuestosRepositorio
    {
        void CrearPresupuesto(Presupuesto presupuesto);
        List<Presupuesto> ListarPresupuestos();
        Presupuesto ObtenerPresupuestoPorId(int id);
        void AgregarProductoAlPresupuesto(int idPresupuesto, int idProducto, int cantidad);
        void EliminarPresupuesto(int id);
    }
}