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

    public partial class login_DB
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public login_DB()
        {
            this.DatosPersona = new HashSet<DatosPersona>();
            this.Inventario = new HashSet<Inventario>();
            this.Turnos = new HashSet<Turnos>();
        }
    
        public int id { get; set; }
        [DisplayName("Usuario")]
        public string usuario { get; set; }
        [DisplayName("Contraseña")]
        public string contraseña { get; set; }
        public int tipo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DatosPersona> DatosPersona { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventario> Inventario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Turnos> Turnos { get; set; }
    }
}
