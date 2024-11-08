using BE;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP
{
    public class MPPProducto
    {
        public static bool Agregar(BEProducto producto)
        {
            try
            {
                Hashtable hashtable = new Hashtable
                {
                    { "@Nombre", producto.Nombre },
                    { "@Precio", producto.Precio },
                    { "@Descripcion", producto.Descripcion },
                    { "@ImagenUrl", producto.ImagenUrl },
                    { "@DVH", producto.DigitoVerificadorH }
                };

                string query = "INSERT INTO Producto (Nombre, Precio, Descripcion, ImagenUrl, DigitoVerificadorH) VALUES (@Nombre, @Precio, @Descripcion, @ImagenUrl, @DVH)";

                return Acceso.ExecuteNonQuery(query, hashtable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Editar(BEProducto producto)
        {
            try
            {
                Hashtable hashtable = new Hashtable
                {
                    { "@Id", producto.Id },
                    { "@Nombre", producto.Nombre },
                    { "@Precio", producto.Precio },
                    { "@Descripcion", producto.Descripcion },
                    { "@ImagenUrl", producto.ImagenUrl },
                    { "@DVH", producto.DigitoVerificadorH }
                };

                string query = "UPDATE Producto SET Nombre = @Nombre, Precio = @Precio, Descripcion = @Descripcion, ImagenUrl = @ImagenUrl, DigitoVerificadorH = @DVH WHERE Id = @Id";

                return Acceso.ExecuteNonQuery(query, hashtable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<BEProducto> Listar()
        {
            try
            {
                List<BEProducto> lista = new List<BEProducto>();

                string query = "SELECT Id, Nombre, Precio, Descripcion, ImagenUrl, DigitoVerificadorH FROM Producto";

                DataTable table = Acceso.ExecuteDataTable(query, null);

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow fila in table.Rows)
                    {
                        BEProducto producto = new BEProducto();
                        Llenar(fila, producto);
                        lista.Add(producto);
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static BEProducto ObtenerPorId(int id)
        {
            try
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("@Id", id);

                string query = "SELECT Id, Nombre, Precio, Descripcion, ImagenUrl, DigitoVerificadorH FROM Producto WHERE Id = @Id";

                DataTable table = Acceso.ExecuteDataTable(query, hashtable);

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow fila in table.Rows)
                    {
                        BEProducto producto = new BEProducto();
                        Llenar(fila, producto);
                        return producto;
                    }
                }

                return null;
            }
            catch (Exception ex) { throw ex; }
        }

        public static List<BEProducto> FiltrarPorNombre(string nombre)
        {
            try
            {
                List<BEProducto> lista = new List<BEProducto>();

                Hashtable hashtable = new Hashtable
                {
                    { "@Nombre", $"%{nombre}%" }
                };

                string query = "SELECT Id, Nombre, Precio, Descripcion, ImagenUrl, DigitoVerificadorH FROM Producto WHERE Nombre LIKE @Nombre";

                DataTable table = Acceso.ExecuteDataTable(query, hashtable);

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow fila in table.Rows)
                    {
                        BEProducto producto = new BEProducto();
                        Llenar(fila, producto);
                        lista.Add(producto);
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static BEProducto Llenar(DataRow row, BEProducto producto)
        {
            try
            {
                producto.Id = Convert.ToInt32(row["Id"].ToString());
                producto.Nombre = row["Nombre"].ToString();
                producto.Precio = Convert.ToDecimal(row["Precio"].ToString());
                producto.Descripcion = row["Descripcion"].ToString();
                producto.ImagenUrl = row["ImagenUrl"].ToString();
                producto.DigitoVerificadorH = row["DigitoVerificadorH"].ToString();
                return producto;
            }
            catch (Exception ex) { throw ex; }
        }

        public static void ActualizarDVH(BEProducto producto)
        {
            try
            {
                Hashtable parametros = new Hashtable();
                parametros.Add("@Id", producto.Id);
                parametros.Add("@DVH", producto.DigitoVerificadorH);

                string query = "UPDATE Producto SET DigitoVerificadorH = @DVH WHERE Id = @Id";

                Acceso.ExecuteNonQuery(query, parametros);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar DVH del producto: {ex.Message}");
            }
        }

        public static int ObtenerUltimoId()
        {
            try
            {
                string query = "SELECT TOP 1 Id FROM Producto ORDER BY Id DESC";

                object result = Acceso.ExecuteScalar(query, null);

                if (result != null && int.TryParse(result.ToString(), out int ultimoId))
                    return ultimoId;

                throw new Exception("No se encontró ningún registro en la tabla Producto.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el último Id de Producto: {ex.Message}");
            }
        }
    }
}
