using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class InventarioCLS
    {
        public int IdInventario { get; set; }
        [Required(ErrorMessage = "Ingrese un material.")]
        [DisplayName("Material")]
        public string TipoInventario { get; set; }
        public Nullable<int> IdUsuario { get; set; }

        public virtual DatosPersona DatosPersona { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleInventario> DetalleInventario { get; set; }

    }
}