using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class loginDBCLS
    {
        public int id { get; set; }
        [DisplayName("Usuario")]
        public string usuario { get; set; }
        [DisplayName("Constraseña")]
        public string contraseña { get; set; }
    }
}