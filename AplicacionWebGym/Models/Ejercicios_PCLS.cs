using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class Ejercicios_PCLS
    {
        public int IdEjercicios_P { get; set; }
        [Required(ErrorMessage = "Ingrese una fecha.")]
        [DisplayName("Fecha de realizacion")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> fecha_ej_p { get; set; }
        [DisplayName("Nombre cliente")]
        public Nullable<int> IdDatos { get; set; }

        public virtual DatosPersona DatosPersona { get; set; }
    }
}