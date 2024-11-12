using Abstraccion;
using BE;
using MPP;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLIdioma
    {
        public static bool Agregar(BEIdioma idioma)
        {
            try
            {
                return MPPIdioma.Agregar(idioma);
            }
            catch (Exception ex) { throw ex; }
        }

        public static void CambiarIdioma(int id)
        {
            try
            {
                Traductor traductor = SesionManager.GetSession().traductor;
                traductor.SetIdioma(id);
            }
            catch (Exception ex) { throw ex; }
        }

        public static List<BEIdioma> Listar()
        {
            try
            {
                return MPPIdioma.Listar();
            }
            catch (Exception ex) { throw ex; }
        }

        public static List<BEPalabra> ObtenerTags()
        {
            try
            {
                SesionManager session = SesionManager.GetSession();

                var traductor = session.traductor;

                int idioma = traductor.Idioma;

                return MPPIdioma.ObtenerTags(idioma);
            }
            catch (Exception ex) { throw ex; }
        }

        public static void RegistrarSubscriptor(ISubscriptor subscriptor)
        {
            try
            {
                Traductor traductor = SesionManager.GetSession().traductor;
                traductor.AgregarSuscriptor(subscriptor);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
