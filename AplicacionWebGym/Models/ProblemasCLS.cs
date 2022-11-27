using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class ProblemasCLS
    {
        public int IdProblemas { get; set; }
        [Required(ErrorMessage = "Ingrese si posee un problema.")]
        [DisplayName("Posee problemas")]
        public Nullable<int> posee { get; set; }
        [Required(ErrorMessage = "Ingrese de que tipo")]
        [DisplayName("Tipo de problema")]
        public string tipo { get; set; }
        public Nullable<int> IdDatos { get; set; }

        public virtual DatosPersona DatosPersona { get; set; }
    }
}