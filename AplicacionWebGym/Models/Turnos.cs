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

    public partial class Turnos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Turnos()
        {
            this.DatosPersona = new HashSet<DatosPersona>();
        }
    
        public int IdTurnos { get; set; }
        [DisplayName("Horario")]
        public string Horario { get; set; }
        public Nullable<int> IdUsuario { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DatosPersona> DatosPersona { get; set; }
        public virtual login_DB login_DB { get; set; }
    }
}
