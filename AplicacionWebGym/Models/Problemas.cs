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
    
    public partial class Problemas
    {
        public int IdProblemas { get; set; }
        public string posee { get; set; }
        public string tipo { get; set; }
        public Nullable<int> IdDatos { get; set; }
    
        public virtual DatosPersona DatosPersona { get; set; }
    }
}