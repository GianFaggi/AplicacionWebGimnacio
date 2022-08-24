using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class MedidasCLS
    {
        public int idMedidas { get; set; }
        public Nullable<int> altura { get; set; }
        public Nullable<int> medidasAbd { get; set; }
        public Nullable<int> medidasCintura { get; set; }
        public Nullable<int> medidasPecho { get; set; }
        public Nullable<int> IdDatos { get; set; }

        public Nullable<System.DateTime> fecha_med { get; set; }

        public virtual DatosPersona DatosPersona { get; set; }

    }
}