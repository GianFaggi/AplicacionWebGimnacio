using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class Ejercicios_TCLS
    {
        public int IdEjercicios_T { get; set; }
        [Required(ErrorMessage = "Ingrese un Dia.")]
        [DisplayName("Dia")]
        public Nullable<dia> Dia { get; set; }
        public Nullable<int> IdDatos { get; set; }

        public virtual DatosPersona DatosPersona { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleEjercicioT> DetalleEjercicioT { get; set; }

        public enum dia
        {
            [Display(Name = "Lunes")]
            a = 1,
            [Display(Name = "Martes")]
            b = 2,
            [Display(Name = "Miercoles")]
            c = 3,
            [Display(Name = "Jueves")]
            d = 4,
            [Display(Name = "Viernes")]
            e = 5,
            [Display(Name = "Sabado")]
            f = 6
        }
    }
}