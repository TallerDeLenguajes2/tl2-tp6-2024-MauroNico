using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Tienda.Models;

namespace Tienda.Repositorios
{
        public class ProductosRepositorio : IProductosRepositorio
        {
            private string cadenaConexion = "Data Source=DB/Tienda.db;Cache=Shared";

            public void crearProducto(Producto producto)
            {
            var query = "INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio);";
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
            
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
                    command.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
                    command.ExecuteNonQuery();  // Ejecutar la inserción del producto

                    // Recuperar el idProducto recién insertado
                    var getIdQuery = "SELECT LAST_INSERT_ROWID();";
                    using (var getIdCommand = new SqliteCommand(getIdQuery, connection))
                    {
                    // Obtener el último idProducto insertado
                        producto.idProducto = Convert.ToInt32(getIdCommand.ExecuteScalar());
                    }
                }
            }
        }

        public void modificarProducto(int id, Producto producto)
        {
            var query = "UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @idProducto";
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@idProducto", id));
                    command.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
                    command.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Producto> listarProductos()
        {
            var queryString = "SELECT * FROM Productos";
            List<Producto> productos = new List<Producto>();

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(queryString, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var producto = new Producto
                        {
                            idProducto = Convert.ToInt32(reader["idProducto"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            Precio = Convert.ToInt32(reader["Precio"])
                        };
                        productos.Add(producto);
                    }
                }
            }
            return productos;
        }

        public Producto obtenerPorID(int id)
        {
            var producto = new Producto();
            var query = "SELECT * FROM Productos WHERE idProducto = @idProducto";

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@idProducto", id));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            producto.idProducto = Convert.ToInt32(reader["idProducto"]);
                            producto.Descripcion = reader["Descripcion"].ToString();
                            producto.Precio = Convert.ToInt32(reader["Precio"]);
                        }
                    }
                }
            }
            return producto;
        }

        public void borrarProducto(int id)
        {
            var query = "DELETE FROM Productos WHERE idProducto = @idProducto";

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@idProducto", id));
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}