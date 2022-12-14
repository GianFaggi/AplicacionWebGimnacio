using AplicacionWebGym.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AplicacionWebGym.Models
{


    public class DPCLS {



        public int IdDatos { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre.")]
        [DisplayName("Nombre")]
        public string name { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre.")]
        [DisplayName("Apellido")]
        public string lastName { get; set; }
        [DisplayName("Edad")]
        public Nullable<int> age { get; set; }
        [DisplayName("Sexo")]
        public Nullable <sexo> sex { get; set; }
        [DisplayName("Horario")]
        public Nullable<turno> IdTurnos { get; set; }
        [DisplayName("Usuario")]
        public Nullable<int> IdUsuario { get; set; }

        public virtual Turnos Turnos { get; set; }

        public enum turno
        {
            [Display(Name = "2 de la tarde")]
            a = 1,
            [Display(Name = "3 de la tarde")]
            b = 2,
            [Display(Name = "7 de la tarde")]
            c = 3,
            [Display(Name = "8 de la tarde")]
            d = 4,
        }

        public enum sexo
        {
            [Display(Name = "Masculino")]
            a = 1,
            [Display(Name = "Femenino")]
            b = 2,
            [Display(Name = "X")]
            c = 3
        }

     }
    }