using System;
using System.Collections;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;

namespace DAL
{
    public class Acceso
    {
        private static SqlConnection _connection = new SqlConnection("Data Source=DESKTOP-BEA1EQV\\SQLEXPRESS;Initial Catalog=DistHaras;Integrated Security=True;TrustServerCertificate=True;Pooling=True;Max Pool Size=100;Min Pool Size=5;MultipleActiveResultSets=True;");
        private static SqlCommand _command;
        private static SqlTransaction _transaction;


        public static bool Backup()
        {
            try
            {
                string rutaDescargas = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                string rutaBackup = Path.Combine(rutaDescargas, $"DistHaras_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak");

                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-BEA1EQV\\SQLEXPRESS;Initial Catalog=DistHaras;Integrated Security=True;TrustServerCertificate=True;Pooling=True;Max Pool Size=100;Min Pool Size=5;MultipleActiveResultSets=True;"))
                {
                    connection.Open();

                    using (SqlCommand backupCmd = new SqlCommand("BACKUP DATABASE [DistHaras] TO DISK = @RutaBackup WITH FORMAT, INIT, NAME = 'Backup de Distribuidora del Haras';", connection))
                    {
                        backupCmd.Parameters.AddWithValue("@RutaBackup", rutaBackup);
                        backupCmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al realizar backup de la base de datos: {ex.Message}", ex);
            }
            finally
            {
                if (_connection.State != ConnectionState.Closed) _connection.Close();
            }
        }

        public static DataTable ExecuteDataTable(string query, Hashtable parametros, bool isStoredProcedure = false)
        {
            DataTable table = new DataTable();

            try
            {
                _command = new SqlCommand(query, _connection);

                if (isStoredProcedure)
                    _command.CommandType = CommandType.StoredProcedure;

                if (parametros != null)
                {
                    foreach (string param in parametros.Keys)
                    {
                        _command.Parameters.AddWithValue(param, parametros[param]);
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(_command);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_connection.State != ConnectionState.Closed) _connection.Close();
            }

            return table;
        }

        public static bool ExecuteNonQuery(string query, Hashtable parametros, bool isStoredProcedure = false)
        {
            try
            {
                _connection.Open();

                _transaction = _connection.BeginTransaction();

                _command = new SqlCommand(query, _connection);
                _command.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
                _command.Transaction = _transaction;

                if (parametros != null)
                {
                    foreach (string param in parametros.Keys)
                    {
                        _command.Parameters.AddWithValue(param, parametros[param]);
                    }
                }

                _command.ExecuteNonQuery();

                _transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
                throw ex;
            }
            finally
            {
                if (_connection.State != ConnectionState.Closed) _connection.Close();
            }
        }

        public static int ExecuteScalar(string query, Hashtable parametros, bool isStoredProcedure = false)
        {
            try
            {
                _connection.Open();
                _command = new SqlCommand(query, _connection);
                _command.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

                if (parametros != null)
                {
                    foreach (string param in parametros.Keys)
                    {
                        _command.Parameters.AddWithValue(param, parametros[param]);
                    }
                }

                int result = Convert.ToInt32(_command.ExecuteScalar());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_connection.State != ConnectionState.Closed) _connection.Close();
            }
        }
    }
}
