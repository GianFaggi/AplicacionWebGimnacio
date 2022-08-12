using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class ViewModel
    {
     public IEnumerable<DatosPersona> persona { get; set; }
        public IEnumerable<Pagos>  pagos { get; set; }
    }
}