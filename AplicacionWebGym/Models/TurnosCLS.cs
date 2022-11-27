using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{
    public class TurnosCLS
    {
        public int IdTurnos { get; set; }
        [Required(ErrorMessage = "Ingrese Horario")]
        [DisplayName("Horario")]
        public string Horario { get; set; }

        public Nullable<int> IdUsuario { get; set; }
    }
}