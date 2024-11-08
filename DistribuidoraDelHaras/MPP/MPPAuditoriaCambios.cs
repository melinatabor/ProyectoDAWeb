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
    public class MPPAuditoriaCambios
    {
        public static bool Agregar(BEAuditoriaCambios auditoriaCambios)
        {
            try
            {
                Hashtable parametros = new Hashtable();

                parametros.Add("@Entidad", auditoriaCambios.Entidad);
                parametros.Add("@IdRegistroAfectado", auditoriaCambios.IdRegistroAfectado);
                parametros.Add("@Operacion", auditoriaCambios.Operacion);
                parametros.Add("@DatosAntes", auditoriaCambios.DatosAntes ?? (object)DBNull.Value);
                parametros.Add("@DatosDespues", auditoriaCambios.DatosDespues ?? (object)DBNull.Value);
                parametros.Add("@Fecha", auditoriaCambios.Fecha);

                string query = @"INSERT INTO AuditoriaCambios (Entidad, IdRegistroAfectado, Operacion, DatosAntes, DatosDespues, Fecha) 
                             VALUES (@Entidad, @IdRegistroAfectado, @Operacion, @DatosAntes, @DatosDespues, @Fecha)";

                return Acceso.ExecuteNonQuery(query, parametros);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar registro en AuditoriaCambios: {ex.Message}");
            }
        }

        public static void Eliminar()
        {
            try
            {
                string query = "DELETE FROM AuditoriaCambios";
                Acceso.ExecuteNonQuery(query, null);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar todos los registros de AuditoriaCambios: {ex.Message}");
            }
        }

        public static List<BEAuditoriaCambios> ListarPorEntidad(string entidad)
        {
            try
            {
                List<BEAuditoriaCambios> listaAuditoria = new List<BEAuditoriaCambios>();
                Hashtable parametros = new Hashtable
                {
                    { "@Entidad", entidad }
                };

                string query = "SELECT Id, Entidad, IdRegistroAfectado, Operacion, Fecha, DatosAntes, DatosDespues FROM AuditoriaCambios WHERE Entidad = @Entidad ORDER BY Id DESC";

                DataTable table = Acceso.ExecuteDataTable(query, parametros);

                foreach (DataRow row in table.Rows)
                {
                    BEAuditoriaCambios auditoriaCambio = new BEAuditoriaCambios
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Entidad = row["Entidad"].ToString(),
                        IdRegistroAfectado = Convert.ToInt32(row["IdRegistroAfectado"]),
                        Operacion = row["Operacion"].ToString(),
                        Fecha = Convert.ToDateTime(row["Fecha"]),
                        DatosAntes = row["DatosAntes"].ToString(),
                        DatosDespues = row["DatosDespues"].ToString()
                    };

                    listaAuditoria.Add(auditoriaCambio);
                }

                return listaAuditoria;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los cambios de auditoría", ex);
            }
        }
    }
}
