﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraccion
{

    public interface IUsuario
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}
