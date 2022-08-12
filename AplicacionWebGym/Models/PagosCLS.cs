using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class PagosCLS
    {
        public int IdPagos { get; set; }
        public Nullable<System.DateTime> fecha { get; set; }
        public string esAsociado { get; set; }
        public Nullable<int> IdDatos { get; set; }

        public virtual ICollection<DatosPersona> DatosPersona { get; set; }


    }

}