using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class DetalleEjercicioPCLS
    {
        public int detalle_Ejercicios_P { get; set; }
        [Required(ErrorMessage = "Ingrese un tipo de ejercicio.")]
        [DisplayName("Tipo de ejercicio")]
        public string tipo { get; set; }
        [DisplayName("Cantidad de repeticiones")]
        public Nullable<int> cantidad { get; set; }
        [DisplayName("Tiempo en segundos")]
        public Nullable<int> tiempoEnSegundos { get; set; }
        [DisplayName("Fecha realizacion")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<int> IdEjercicios_P { get; set; }

        public virtual Ejercicios_P Ejercicios_P { get; set; }
    }
}