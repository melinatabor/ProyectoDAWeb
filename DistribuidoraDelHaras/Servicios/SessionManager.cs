﻿using Abstraccion;
using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class SesionManager
    {
        private static SesionManager _session = null;

        private IUsuario _usuario { get; set; }

        public Traductor traductor { get; set; }
        private SesionManager() { }

        public static void Login(BEUsuario usuario)
        {
            try
            {
                if (_session == null)
                {
                    _session = new SesionManager();
                    _session._usuario = usuario;
                    _session.traductor = new Traductor();
                }
                else
                    throw new Exception("Sesion ya iniciada");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Logout()
        {
            try
            {
                if (_session != null) _session = null;
                else throw new Exception("La sesion no esta iniciada");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetUsername()
        {
            try
            {
                return _session._usuario.Username;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SesionManager GetSession()
        {
            SesionManager s = _session ?? null;
            return s;
        }

        public static IUsuario GetUsuario()
        {
            try
            {
                return _session?._usuario 
                    ?? throw new InvalidOperationException("La sesión no está inicializada.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
