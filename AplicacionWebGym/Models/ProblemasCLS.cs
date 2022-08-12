using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class ProblemasCLS
    {
        public int IdProblemas { get; set; }
        public string posee { get; set; }
        public string tipo { get; set; }
        public Nullable<int> IdDatos { get; set; }

        public virtual DatosPersona DatosPersona { get; set; }
    }
}