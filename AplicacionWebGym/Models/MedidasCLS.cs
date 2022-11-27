using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class MedidasCLS
    {
        public int IdMedidas { get; set; }
        [DisplayName("Altura")]
        public int altura { get; set; }
        [DisplayName("Toma medidas Abdominales")]
        public int medidasAbd { get; set; }
        [DisplayName("Toma medidas Cintura")]
        public int medidasCintura { get; set; }
        [DisplayName("Toma medidas Pecho")]
        public int medidasPecho { get; set; }
        [DisplayName("Nombre Cliente")]
        public Nullable<int> IdDatos { get; set; }
        [Required(ErrorMessage = "Ingrese la fecha de la toma de las medidas.")]
        [DisplayName("Fecha de las mediciones")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> fecha_med { get; set; }

        public virtual DatosPersona DatosPersona { get; set; }

    }
}