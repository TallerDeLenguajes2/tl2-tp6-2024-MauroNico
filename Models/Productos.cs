using System.Text.Json.Serialization;

namespace Tienda.Models{
    public class Producto
{
    
    public int idProducto { get; set; }  


    public string Descripcion { get; set; }
    public int Precio { get; set; }
}
}