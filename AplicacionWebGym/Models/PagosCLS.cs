using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class PagosCLS
    {
        public int IdPagos { get; set; }
        [Required(ErrorMessage = "Ingrese la fecha del pago.")]
        [DisplayName("Fecha de pago")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> fecha { get; set; }
        [DisplayName("Es asociado")]
        public Nullable <esAsoc> esAsociado { get; set; }
        [DisplayName("Nombre Cliente")]
        public Nullable<int> IdDatos { get; set; }

        public virtual DatosPersona DatosPersona { get; set; }

        public enum esAsoc
        {
            [Display(Name = "Si")]
            si = 1,
            [Display(Name = "No")]
            no = 2,
        }

    }

}