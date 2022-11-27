using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class DetalleInventarioCLS
    {
        public int IdDetalleInventario { get; set; }
        [DisplayName("Cantidad")]
        public Nullable<int> cantidad { get; set; }
        [DisplayName("Estado del material")]
        public Nullable<turno> estado { get; set; }
        public Nullable<int> IdInventario { get; set; }


        public virtual Inventario Inventario { get; set; }


        public enum turno
        {
            [Display(Name = "Bueno")]
            a = 1,
            [Display(Name = "Regular")]
            b = 2,
            [Display(Name = "Malo")]
            c = 3,
        }
    }
}