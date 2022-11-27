using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class DetalleEjercicioTCLS
    {
        public int IdDE { get; set; }
        [Required(ErrorMessage = "Ingrese un tipo de ejercicio.")]
        [DisplayName("Tipo de ejercicio")]
        public string tipoDeEjercicio { get; set; }
        [DisplayName("Repeticiones a realizar")]
        public Nullable<int> repeticiones { get; set; }
        public Nullable<int> IdET { get; set; }

        public virtual Ejercicios_T Ejercicios_T { get; set; }
    }
}