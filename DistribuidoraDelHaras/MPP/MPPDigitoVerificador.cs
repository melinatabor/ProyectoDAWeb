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
    public class MPPDigitoVerificador
    {
        public static void ActualizarDVV(string dvvCalculado, string entidad)
        {
            try
            {
                Hashtable parametros = new Hashtable();
                parametros.Add("@DVV", dvvCalculado);
                parametros.Add("@Entidad", entidad);

                string query = "UPDATE DigitoVerificadorVertical SET DigitoVerificadorVertical = @DVV WHERE Entidad = @Entidad";

                Acceso.ExecuteNonQuery(query, parametros);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el DVV de la entidad {entidad} en la base de datos: {ex.Message}");
            }
        }

        public static string ObtenerDigitoVerificadorVertical(string entidad)
        {
            try
            {
                Hashtable parametros = new Hashtable();
                parametros.Add("@Entidad", entidad);

                string query = "SELECT DigitoVerificadorVertical FROM DigitoVerificadorVertical WHERE Entidad = @Entidad";

                DataTable table = Acceso.ExecuteDataTable(query, parametros);

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow fila in table.Rows)
                        return fila["DigitoVerificadorVertical"].ToString();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
