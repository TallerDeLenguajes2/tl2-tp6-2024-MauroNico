
using Tienda.Models;

namespace Tienda.Repositorios{
    public interface IProductosRepositorio{
        public void crearProducto(Producto producto);
        public void modificarProducto(int id, Producto producto);
        public List<Producto> listarProductos();
        public Producto obtenerPorID(int id);
        public void borrarProducto(int id);

    }
}