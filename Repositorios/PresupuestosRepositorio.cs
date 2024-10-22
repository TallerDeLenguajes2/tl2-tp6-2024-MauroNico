using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Tienda.Models;

namespace Tienda.Repositorios
{
    public class PresupuestosRepositorio : IPresupuestosRepositorio
    {
        private string cadenaConexion = "Data Source=DB/Tienda.db;Cache=Shared";

        
        public void CrearPresupuesto(Presupuesto presupuesto)
        {
            var query = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@NombreDestinatario, @FechaCreacion)";
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuesto.NombreDestinatario));
                    command.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuesto.FechaCreacion));
                    command.ExecuteNonQuery();
                }
            }

            
            foreach (var detalle in presupuesto.Detalle)
            {
                AgregarProductoAlPresupuesto(presupuesto.IdPresupuesto, detalle.Producto.idProducto, detalle.Cantidad);
            }
        }

        
        public List<Presupuesto> ListarPresupuestos()
        {
            var queryString = "SELECT * FROM Presupuestos";
            List<Presupuesto> presupuestos = new List<Presupuesto>();

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(queryString, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var presupuesto = new Presupuesto
                        {
                            IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                            NombreDestinatario = reader["NombreDestinatario"].ToString(),
                            FechaCreacion = reader["FechaCreacion"].ToString()
                        };
                        presupuestos.Add(presupuesto);
                    }
                }
            }
            return presupuestos;
        }

        public Presupuesto ObtenerPresupuestoPorId(int id)
        {
            var presupuesto = new Presupuesto();
            var query = "SELECT * FROM Presupuestos WHERE idPresupuesto = @idPresupuesto";

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                            presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                            presupuesto.FechaCreacion = reader["FechaCreacion"].ToString();
                        }
                    }
                }
            }

            
            var detalles = ObtenerDetallesPresupuesto(id);
            presupuesto.Detalle = detalles;
            
            return presupuesto;
        }

        
        private List<PresupuestoDetalle> ObtenerDetallesPresupuesto(int idPresupuesto)
        {
            var detalles = new List<PresupuestoDetalle>();
            var query = "SELECT pd.idProducto, pd.Cantidad, p.Descripcion, p.Precio FROM PresupuestosDetalle pd " +
                        "JOIN Productos p ON pd.idProducto = p.idProducto WHERE pd.idPresupuesto = @idPresupuesto";

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var detalle = new PresupuestoDetalle
                            {
                                Producto = new Producto
                                {
                                    idProducto = Convert.ToInt32(reader["idProducto"]),
                                    Descripcion = reader["Descripcion"].ToString(),
                                    Precio = Convert.ToInt32(reader["Precio"])
                                },
                                Cantidad = Convert.ToInt32(reader["Cantidad"])
                            };
                            detalles.Add(detalle);
                        }
                    }
                }
            }
            return detalles;
        }

       
        public void AgregarProductoAlPresupuesto(int idPresupuesto, int idProducto, int cantidad)
        {
            var query = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @Cantidad)";

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
                    command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
                    command.Parameters.Add(new SqliteParameter("@Cantidad", cantidad));
                    command.ExecuteNonQuery();
                }
            }
        }

        
        public void EliminarPresupuesto(int id)
        {
            var queryDetalle = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @idPresupuesto";
            var queryPresupuesto = "DELETE FROM Presupuestos WHERE idPresupuesto = @idPresupuesto";

            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();

                
                using (var command = new SqliteCommand(queryDetalle, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
                    command.ExecuteNonQuery();
                }

                
                using (var command = new SqliteCommand(queryPresupuesto, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}