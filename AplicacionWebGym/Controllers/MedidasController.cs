using AplicacionWebGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplicacionWebGym.Controllers
{

    public class MedidasController : Controller
    {
        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Listar Personas */
        public ActionResult Lista_Personas_Medidas(DatosPersona odatosPersona)
        {
            {
                string apellido_persona = odatosPersona.lastName;
                List<DPCLS> lista = null;
                using (var db = new PWGBD())
                {
                    if (odatosPersona.lastName == null)
                    {
                        lista = (from dp in db.DatosPersona
                                 select new DPCLS
                                 {
                                     IdDatos = dp.IdDatos,
                                     name = dp.name,
                                     lastName = dp.lastName
                                 }).ToList();

                        Session["Lista_Medidas"] = lista;
                    }
                    else
                    {
                        lista = (from dp in db.DatosPersona
                                 where dp.lastName.Contains(apellido_persona)
                                 select new DPCLS
                                 {
                                     IdDatos = dp.IdDatos,
                                     name = dp.name,
                                     lastName = dp.lastName
                                 }).ToList();
                        Session["Lista_Medidas"] = lista;
                    }
                }
                return View(lista);
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /* Listar Medidas tomadas por id */
        public ActionResult Lista_Medidas(int id, Medidas medidas)
        {

            try
            {
                List<MedidasCLS> lista2 = null;
                using (var db = new PWGBD())
                {
                    lista2 = (from dp in db.Medidas
                              where dp.IdDatos == id
                              select new MedidasCLS
                              {
                                  idMedidas = dp.idMedidas,
                                  altura = dp.altura,
                                  medidasAbd = dp.medidasAbd,
                                  medidasCintura = dp.medidasCintura,
                                  medidasPecho = dp.medidasPecho,
                                  fecha_med = dp.fecha_med,
                                  IdDatos = id,
                              }).ToList();
                    Session["Lista_Medidas2"] = lista2;
                    return View(lista2);

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ERROR AL MOSTRAR DETALLES DE LA MEDIDA", ex);
                return RedirectToAction("Lista_Medidas2");
            }
        }


        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Trae nombre de personar */


        public static string nombre_persona(int? IdDatos)
        {
            using (var db = new PWGBD())
            {
                return db.DatosPersona.Find(IdDatos).name;
            }
        }


        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Agregar medida por Clase*/
        public ActionResult Agregar_Medidas()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Agregar_Medidas(MedidasCLS omedidasCLS, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(omedidasCLS);
            }
            else
            {
                using (var db = new PWGBD())
                {
                    Medidas datos = new Medidas
                    {
                        idMedidas = omedidasCLS.idMedidas,
                        altura = omedidasCLS.altura,
                        medidasAbd = omedidasCLS.medidasAbd,
                        medidasCintura = omedidasCLS.medidasCintura,
                        medidasPecho = omedidasCLS.medidasPecho,
                        fecha_med = omedidasCLS.fecha_med,
                        IdDatos = id
                    };
                    db.Medidas.Add(datos);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Lista_Personas_Medidas");

        }
        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /* Editar Jugador*/

        public ActionResult EditarMedidas(int id)
        {
            try
            {
                using (var db = new PWGBD())
                {
                    Medidas datos = db.Medidas.Find(id);

                    return View(datos);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult EditarMedidas(Medidas datos, int id, int id2)
        {
            if (!ModelState.IsValid)
            {
                return View(datos);
            }
            try
            {
                using (var db = new PWGBD())
                {
                    Medidas dat = db.Medidas.Find(datos.idMedidas);
                    dat.idMedidas = id;
                    dat.medidasPecho = datos.medidasPecho;
                    dat.altura = datos.altura;  
                    dat.medidasAbd = datos.medidasAbd;
                    dat.medidasCintura = datos.medidasCintura;
                    dat.fecha_med = datos.fecha_med;
                    dat.IdDatos = id2;

                    db.SaveChanges();
                    return RedirectToAction("Lista_Personas_Medidas");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Editar las Medidas- ", ex);
            }
            return View();


        }


        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Detalle Pago */

        public ActionResult DetalleMedidas(int id)
        {
            try
            {
                using (var db = new PWGBD())
                {
                    Medidas datos = db.Medidas.Find(id);
                    return View(datos);

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ERROR AL MOSTRAR DETALLES DE LA MEDIDA", ex);
                return RedirectToAction("Lista_Medidas");
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Eliminar */

        public ActionResult EliminarMedida(int id)
        {
            try
            {
                using (var db = new PWGBD())
                {
                    Medidas alu = db.Medidas.Find(id);
                    db.Medidas.Remove(alu);
                    db.SaveChanges();
                    return RedirectToAction("Lista_Pagos");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Borrar Jugador", ex);
                return RedirectToAction("Lista_Medidas");
            }
        }
    }
}





