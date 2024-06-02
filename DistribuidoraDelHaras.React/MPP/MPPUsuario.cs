using BE;
using System;
using System.Collections.Generic;
using System.Data;
using DAL;
using System.Collections;

namespace MPP
{
    public class MPPUsuario
    {
        public static bool Agregar(BEUsuario usuario)
        {
            try
            {
                Hashtable parametros = new Hashtable();

                parametros.Add("@Nombre", usuario.Nombre);
                parametros.Add("@Apellido", usuario.Apellido);
                parametros.Add("@Email", usuario.Email);
                parametros.Add("@Username", usuario.Username);
                parametros.Add("@Password", usuario.Password);
                parametros.Add("@Activo", usuario.Activo);

                string query = $"INSERT INTO Usuario (Nombre, Apellido, Email, Username, Password, Activo) VALUES (@Nombre, @Apellido, @Email, @Username, @Password, @Activo)";

                return Acceso.ExecuteNonQuery(query, parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static BEUsuario BuscarUsuario(BEUsuario usuario)
        {
            try
            {
                Hashtable parametros = new Hashtable();

                parametros.Add("@Password", usuario.Password);
                parametros.Add("@Username", usuario.Username);

                string query = $"SELECT * FROM Usuario WHERE Username = @Username AND Password = @Password";

                DataTable table = Acceso.ExecuteDataTable(query, parametros);

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow fila in table.Rows)
                    {
                        BEUsuario u = new BEUsuario();
                        return Llenar(fila, u);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Editar(BEUsuario usuario)
        {
            try
            {
                Hashtable parametros = new Hashtable();

                parametros.Add("@Nombre", usuario.Nombre);
                parametros.Add("@Apellido", usuario.Apellido);
                parametros.Add("@Email", usuario.Email);
                parametros.Add("@Username", usuario.Username);
                parametros.Add("@Id", usuario.Id);

                string query = $"UPDATE Usuario SET Nombre = @Nombre, Apellido = @Apellido, Email = @Email, Username = @Username WHERE Id = @Id";

                return Acceso.ExecuteNonQuery(query, parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<BEUsuario> Listar()
        {
            try
            {
                List<BEUsuario> lista = new List<BEUsuario>();

                string query = "SELECT Id, Nombre, Apellido, Email, Username, Password, Activo FROM Usuario";

                DataTable table = Acceso.ExecuteDataTable(query, null);

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow fila in table.Rows)
                    {
                        BEUsuario usuario = new BEUsuario();
                        Llenar(fila, usuario);
                        lista.Add(usuario);
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static BEUsuario Llenar(DataRow row, BEUsuario usuario)
        {
            try
            {
                usuario.Id       = Convert.ToInt32(row["Id"].ToString());
                usuario.Nombre   = row["Nombre"].ToString();
                usuario.Apellido = row["Apellido"].ToString();
                usuario.Email    = row["Email"].ToString();
                usuario.Username = row["Username"].ToString();
                usuario.Password = row["Password"].ToString();
                usuario.Activo   = Convert.ToBoolean(row["Activo"]);
                return usuario;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
