﻿using BE;
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
    public class MPPBitacora
    {
        public static bool Agregar(BEBitacora bitacora)
        {
            try
            {
                Hashtable parametros = new Hashtable();

                parametros.Add("@Usuario", bitacora.Usuario);
                parametros.Add("@Tipo", bitacora.Tipo);
                parametros.Add("@Mensaje", bitacora.Mensaje);
                parametros.Add("@DVH", bitacora.DigitoVerificadorH);
                parametros.Add("@Fecha", bitacora.Fecha);

                string query = $"INSERT INTO Bitacora (Usuario, Tipo, Mensaje, Fecha, DigitoVerificadorH) VALUES (@Usuario, @Tipo, @Mensaje, @Fecha, @DVH)";

                return Acceso.ExecuteNonQuery(query, parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static BEBitacoraFiltrada Llenar(DataRow fila, BEBitacoraFiltrada bitacora)
        {
            try
            {
                bitacora.Id = Convert.ToInt32(fila["Id"]);
                bitacora.Usuario = fila["Usuario"].ToString();
                bitacora.Mensaje = fila["Mensaje"].ToString();
                bitacora.Fecha = Convert.ToDateTime(fila["Fecha"]);

                return bitacora;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<BEBitacoraFiltrada> Filtrar(BEBitacoraCriteria criteria)
        {
            try
            {
                List<BEBitacoraFiltrada> lista = new List<BEBitacoraFiltrada>();

                Hashtable parametros = ExtraerParametros(criteria);

                string query = $"SELECT b.id AS 'Id', u.username AS 'Usuario', b.Mensaje, b.Fecha FROM Bitacora AS b INNER JOIN Usuario u ON u.id = b.usuario WHERE (@Usuario IS NULL OR u.Username = @Usuario) AND (b.Fecha >= @Desde AND b.Fecha <= @Hasta) AND (b.Tipo = @Tipo)";

                DataTable table = Acceso.ExecuteDataTable(query, parametros);

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow fila in table.Rows)
                    {
                        BEBitacoraFiltrada bitacora = new BEBitacoraFiltrada();
                        lista.Add(Llenar(fila, bitacora));
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static Hashtable ExtraerParametros(BEBitacoraCriteria criteria)
        {
            try
            {
                Hashtable parametros = new Hashtable();

                parametros.Add("@Tipo", criteria.Tipo ?? (object)DBNull.Value);
                parametros.Add("@Usuario", criteria.Usuario ?? (object)DBNull.Value);
                parametros.Add("@Desde", criteria.Desde);
                parametros.Add("@Hasta", criteria.Hasta);

                parametros["@Page"] = (criteria.Page - 1) * criteria.RowPerPage;
                parametros["@Filas"] = criteria.RowPerPage;

                if (string.IsNullOrEmpty(criteria.Usuario))
                    parametros["@Usuario"] = DBNull.Value;
                if (criteria.Tipo == 0 || criteria.Tipo == null)
                    parametros["@Tipo"] = DBNull.Value;

                return parametros;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<BEBitacora> ListarTodo()
        {
            try
            {
                List<BEBitacora> lista = new List<BEBitacora>();

                string query = "SELECT Mensaje, Fecha FROM Bitacora";

                DataTable table = Acceso.ExecuteDataTable(query, null);

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow fila in table.Rows)
                    {
                        BEBitacora bitacora = new BEBitacora();
                        bitacora.Mensaje = fila["Mensaje"].ToString();
                        bitacora.Fecha = Convert.ToDateTime(fila["Fecha"]);
                        lista.Add(bitacora);
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ActualizarDVH(BEBitacora bitacora)
        {
            try
            {
                Hashtable parametros = new Hashtable();
                parametros.Add("@Id", bitacora.Id);
                parametros.Add("@DVH", bitacora.DigitoVerificadorH);

                string query = "UPDATE Bitacora SET DigitoVerificadorH = @DVH WHERE Id = @Id";

                Acceso.ExecuteNonQuery(query, parametros);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar DVH de bitacora: {ex.Message}");
            }
        }

        public static int ObtenerUltimoId()
        {
            try
            {
                string query = "SELECT TOP 1 Id FROM Bitacora ORDER BY Id DESC";

                object result = Acceso.ExecuteScalar(query, null);

                if (result != null && int.TryParse(result.ToString(), out int ultimoId))
                    return ultimoId;

                throw new Exception("No se encontró ningún registro en la tabla Bitacora.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el último Id de Bitacora: {ex.Message}");
            }
        }

        public static List<BEBitacora> ListarParaRecalcular()
        {
            try
            {
                List<BEBitacora> lista = new List<BEBitacora>();

                string query = "SELECT Id, Usuario, Mensaje, Fecha FROM Bitacora";

                DataTable table = Acceso.ExecuteDataTable(query, null);

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow fila in table.Rows)
                    {
                        BEBitacora bitacora = new BEBitacora();
                        bitacora.Id = Convert.ToInt32(fila["Id"]);
                        bitacora.Usuario = Convert.ToInt32(fila["Usuario"]);
                        bitacora.Mensaje = fila["Mensaje"].ToString();
                        bitacora.Fecha = Convert.ToDateTime(fila["Fecha"]);
                        lista.Add(bitacora);
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<BEBitacora> ListarParaVerificarCambios()
        {
            try
            {
                List<BEBitacora> lista = new List<BEBitacora>();

                string query = "SELECT Id, Usuario, Mensaje, Fecha FROM Bitacora";

                DataTable table = Acceso.ExecuteDataTable(query, null);

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow fila in table.Rows)
                    {
                        BEBitacora bitacora = new BEBitacora();
                        bitacora.Id = Convert.ToInt32(fila["Id"]);
                        bitacora.Usuario = Convert.ToInt32(fila["Usuario"]);
                        bitacora.Mensaje = fila["Mensaje"].ToString();
                        bitacora.Fecha = Convert.ToDateTime(fila["Fecha"]);
                        lista.Add(bitacora);
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
