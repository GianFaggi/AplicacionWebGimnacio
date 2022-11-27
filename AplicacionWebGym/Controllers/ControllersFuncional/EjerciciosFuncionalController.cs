using AplicacionWebGym.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplicacionWebGym.Controllers
{
    public class EjerciciosFuncionalController : Controller
    {
        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Listar Clientes */
        public ActionResult Lista_Personas_Ejercicios(DatosPersona odatosPersona)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                try
                {

                    {
                        string apellido_persona = odatosPersona.lastName;
                        List<DPCLS> lista = null;
                        int IdUsuarioLocal = 0;
                        IdUsuarioLocal = (int)Session["Id"];
                        using (var db = new AppGym2Entities())
                        {
                            if (odatosPersona.lastName == null)
                            {
                                lista = (from dp in db.DatosPersona
                                         where dp.IdUsuario.ToString().Contains(IdUsuarioLocal.ToString())
                                         where dp.IdUsuario == IdUsuarioLocal
                                         select new DPCLS
                                         {
                                             IdDatos = dp.IdDatos,
                                             name = dp.name,
                                             lastName = dp.lastName
                                         }).ToList();

                                Session["Lista_Personas_Ejercicio"] = lista;
                            }
                            else
                            {
                                lista = (from dp in db.DatosPersona
                                         where dp.lastName.Contains(apellido_persona) && dp.IdUsuario.ToString().Contains(IdUsuarioLocal.ToString())
                                         select new DPCLS
                                         {
                                             IdDatos = dp.IdDatos,
                                             name = dp.name,
                                             lastName = dp.lastName
                                         }).ToList();
                                Session["Lista_Personas_Ejercicio"] = lista;
                            }
                        }
                        return View(lista);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL MOSTRAR LISTA DE PERSONAS", ex);
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Listar fechas ejercicios */
        public ActionResult Lista_Fechas(int id, Ejercicios_P ejercicios)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                try
                {
                    List<Ejercicios_PCLS> lista2 = null;
                    using (var db = new AppGym2Entities())
                    {
                        lista2 = (from dp in db.Ejercicios_P
                                  where dp.IdDatos == id
                                  select new Ejercicios_PCLS
                                  {
                                      IdEjercicios_P = dp.IdEjercicios_P,
                                      fecha_ej_p = dp.fecha_ej_p,
                                      IdDatos = dp.IdDatos
                                  }).ToList();
                        Session["Lista_Fechas"] = lista2;
                        return View(lista2);

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL MOSTRAR LISTA DE FECHAS", ex);
                    return RedirectToAction("Lista_Personas_Ejercicios");
                }
            }
        }


        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Trae nombre de personar*/

        public ActionResult ListarPersonas()
        {

            using (var db = new AppGym2Entities())
            {
                return PartialView(db.DatosPersona.ToList());
            }

        }

        public static string nombre_persona(int? IdDatos)
        {
            using (var db = new AppGym2Entities())
            {
                return db.DatosPersona.Find(IdDatos).name;
            }
        }


        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Agregar Fecha */
        public ActionResult Agregar_Fecha()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Agregar_Fecha(Ejercicios_PCLS oejerciciosCLS, int? id)
        {
            if (!ModelState.IsValid)
            {
                return View(oejerciciosCLS);
            }
            else
            {
                try
                {


                    using (var db = new AppGym2Entities())
                    {
                        Ejercicios_P datos = new Ejercicios_P
                        {
                         IdEjercicios_P = oejerciciosCLS.IdEjercicios_P,
                         fecha_ej_p = oejerciciosCLS.fecha_ej_p,
                         IdDatos = id
                        };
                         db.Ejercicios_P.Add(datos);
                         db.SaveChanges();
                        }
                    
                return RedirectToAction("Lista_Personas_Ejercicios");
                }
                 catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL MOSTRAR AGREGARO FECHA", ex);
                    return RedirectToAction("Lista_Personas_Ejercicios");
                }

            }
         }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Eliminar Fecha */
        public ActionResult EliminarFecha(int id, int id2)
        {
            try
            {
                using (var db = new AppGym2Entities())
                {
                    Ejercicios_P alu = db.Ejercicios_P.Find(id);
                    db.Ejercicios_P.Remove(alu);
                    db.SaveChanges();
                    return RedirectToAction("Lista_Fechas", new { @id = id2 });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Borrar Ejercicio", ex);
                return RedirectToAction("Lista_Fechas", new { @id = id2 });
            }
        }
        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /* Listar Ejercicios */
        public ActionResult Lista_Ejercicios(int id, DetalleEjercicioPCLS ejercicios)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                try
                {
                    List<DetalleEjercicioPCLS> lista2 = null;
                    using (var db = new AppGym2Entities())
                    {
                        lista2 = (from dp in db.DetalleEjerciciosP
                                  where dp.IdEjercicios_P == id
                                  select new DetalleEjercicioPCLS
                                  {
                                      detalle_Ejercicios_P = dp.detalle_Ejercicios_P,
                                      tipo = dp.tipo,
                                      cantidad = dp.cantidad,
                                      tiempoEnSegundos = dp.tiempoEnSegundos,
                                      IdEjercicios_P = dp.IdEjercicios_P
                                  }).ToList();
                        Session["Lista_Ejercicios"] = lista2;
                        return View(lista2);

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL MOSTRAR LISTA DE LOS EJERCICIOS", ex);
                    return RedirectToAction("Lista_Fechas");
                }
            }
        }


        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Trae fecha*/

        public ActionResult ListarFechas()
        {

            using (var db = new AppGym2Entities())
            {
                return PartialView(db.Ejercicios_P.ToList());
            }

        }

        public static string fecha(int? IdEjercicios_P)
        {
            using (var db = new AppGym2Entities())
            {
                return db.Ejercicios_P.Find(IdEjercicios_P).fecha_ej_p.ToString();
            }
        }


        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Agregar Ejercicio */
        public ActionResult Agregar_Ejercicio()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Agregar_Ejercicio(DetalleEjercicioPCLS oejerciciosCLS, int? id)
        {
            if (!ModelState.IsValid)
            {
                return View(oejerciciosCLS);
            }
            else
            {
                try
                {
                    using (var db = new AppGym2Entities())
                    {
                        DetalleEjerciciosP datos = new DetalleEjerciciosP
                        {
                            detalle_Ejercicios_P = oejerciciosCLS.detalle_Ejercicios_P,
                            tipo = oejerciciosCLS.tipo,
                            cantidad = oejerciciosCLS.cantidad,
                            tiempoEnSegundos = oejerciciosCLS.tiempoEnSegundos,
                            IdEjercicios_P = id
                        };
                        db.DetalleEjerciciosP.Add(datos);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Lista_Ejercicios", new { @id = id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL MOSTRAR DETALLES DE LOS EJERCICIOS", ex);
                    return RedirectToAction("Lista_Personas_Ejercicios");
                }
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Eliminar */
        public ActionResult EliminarEjercicio(int id , int id2)
        {
            try
            {
                using (var db = new AppGym2Entities())
                {
                    DetalleEjerciciosP alu = db.DetalleEjerciciosP.Find(id);
                    db.DetalleEjerciciosP.Remove(alu);
                    db.SaveChanges();
                    return RedirectToAction("Lista_Ejercicios", new { @id = id2 });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Borrar Ejercicio", ex);
                return RedirectToAction("Lista_Ejercicios", new {@id = id2} );
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Reporte Excel*/
        public FileResult generarExcel()
        {
            byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                ExcelPackage ep = new ExcelPackage();
                //Crear hoja
                ep.Workbook.Worksheets.Add("Reporte Ejercicios");
                ExcelWorksheet ew = ep.Workbook.Worksheets[0];
                //Creamos y ponemos nombre a las columnas
                ew.Cells[1, 1].Value = "Ejercicios Realizados";
                ew.Cells[2, 1].Value = "Tipo de ejercicio realizado";
                ew.Cells[2, 2].Value = "Cantidad de repeticiones";
                ew.Cells[2, 3].Value = "Tiempo en segundos";
                ew.Column(1).Width = 40;
                ew.Column(2).Width = 40;
                ew.Column(3).Width = 40;
                using (var range = ew.Cells[1, 1])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.LightTrellis;
                    range.Style.Font.Color.SetColor(Color.Transparent);
                    range.Style.Fill.BackgroundColor.SetColor(Color.Transparent);
                }
                List<DetalleEjercicioPCLS> lista = (List<DetalleEjercicioPCLS>)Session["Lista_Ejercicios"];
                int nregistros = lista.Count;
                for (int i = 0; i < nregistros; i++)
                {
                    ew.Cells[i + 3, 1].Value = lista[i].tipo;
                    ew.Cells[i + 3, 2].Value = lista[i].cantidad;
                    ew.Cells[i + 3, 3].Value = lista[i].tiempoEnSegundos;
                }
                ep.SaveAs(ms);
                buffer = ms.ToArray();
            }

            return File(buffer, "application/x-vnd.oasis.opendocument.spreadsheet", "Mediciones.xlsx");
        }
    }
}
