using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class EjerciciosCLS
    {
        public int IdEjercicios { get; set; }
        public Nullable<int> abdominales { get; set; }
        public Nullable<int> IdDatos { get; set; }
        public Nullable<System.DateTime> fecha_ej { get; set; }

        public virtual DatosPersona DatosPersona { get; set; }
    }
}