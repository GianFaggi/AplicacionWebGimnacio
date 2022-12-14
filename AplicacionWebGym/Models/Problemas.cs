//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AplicacionWebGym.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Problemas
    {
        public int IdProblemas { get; set; }
        [Required(ErrorMessage = "Ingrese si posee o no un problema.")]
        [DisplayName("Posee un problema")]
        public Nullable<int> posee { get; set; }
        [Required(ErrorMessage = "Ingrese el tipo de problema que posee.")]
        [DisplayName("Tipo de problema")]
        public string tipo { get; set; }
        public Nullable<int> IdDatos { get; set; }
    
        public virtual DatosPersona DatosPersona { get; set; }
    }
}
