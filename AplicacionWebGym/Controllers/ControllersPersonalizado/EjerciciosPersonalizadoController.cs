using AplicacionWebGym.Models;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplicacionWebGym.Controllers
{
    public class EjerciciosPersonalizadoController : Controller
    {
        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Listar Personas */
        public ActionResult Lista_Personas_Ejercicios_Personalizados(DatosPersona odatosPersona)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                try

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
                                     select new DPCLS
                                     {
                                         IdDatos = dp.IdDatos,
                                         name = dp.name,
                                         lastName = dp.lastName
                                     }).ToList();

                            Session["Lista_Ejercicios"] = lista;
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
                            Session["Lista_Ejercicios"] = lista;
                        }
                    }
                    return View(lista);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL LISTAR PERSONAS", ex);
                    return RedirectToAction("Index");
                }
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Listar Dias ejercicios */
        public ActionResult Lista_Dias(int id, Ejercicios_T ejercicios)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                try
                {
                    List<Ejercicios_TCLS> lista2 = null;
                    using (var db = new AppGym2Entities())
                    {
                        lista2 = (from dp in db.Ejercicios_T
                                  where dp.IdDatos == id
                                  select new Ejercicios_TCLS
                                  {
                                      IdEjercicios_T = dp.IdEjercicios_T,
                                      Dia = (Ejercicios_TCLS.dia?)dp.Dia,
                                      IdDatos = dp.IdDatos
                                  }).ToList();
                        Session["Lista_Ejercicios2"] = lista2;
                        return View(lista2);

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL MOSTRAR DETALLES DE LOS EJERCICIOS", ex);
                    return RedirectToAction("Lista_Personas_Ejercicios");
                }
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Agregar Dia */
        public ActionResult Agregar_Dias()
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

        public ActionResult Agregar_Dias(Ejercicios_TCLS oejerciciosCLS, int? id)
        {
            if (!ModelState.IsValid)
            {
                return View(oejerciciosCLS);
            }
            else
            {
                try { 
                using (var db = new AppGym2Entities())
                {
                    Ejercicios_T datos = new Ejercicios_T
                    {
                        IdEjercicios_T = oejerciciosCLS.IdEjercicios_T,
                        Dia = (int?)oejerciciosCLS.Dia,
                        IdDatos = id
                    };
                    db.Ejercicios_T.Add(datos);
                    db.SaveChanges();
                }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error al Borrar el pago", ex);
                    return RedirectToAction("Lista_Pagos_Persona");
                }
                return RedirectToAction("Lista_Personas_Ejercicios_Personalizados");
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Eliminar Dia*/
        public ActionResult Eliminar_Dia(int id, int id2)
        {
            try
            {
                using (var db = new AppGym2Entities())
                {
                    Ejercicios_T alu = db.Ejercicios_T.Find(id);
                    db.Ejercicios_T.Remove(alu);
                    db.SaveChanges();
                    return RedirectToAction("Lista_Dias", new { @id = id2 });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Borrar Ejercicio", ex);
                return RedirectToAction("Lista_Dias", new { @id = id2 });
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /* Listar Ejercicios */
        public ActionResult Lista_Ejercicios_Personalizado(int id, DetalleEjercicioTCLS ejercicios)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                try
                {
                    List<DetalleEjercicioTCLS> lista2 = null;
                    using (var db = new AppGym2Entities())
                    {
                        lista2 = (from dp in db.DetalleEjercicioT
                                  where dp.IdET == id
                                  select new DetalleEjercicioTCLS
                                  {
                                      IdDE = dp.IdDE,
                                      tipoDeEjercicio = dp.tipoDeEjercicio,
                                      repeticiones = dp.repeticiones,
                                      IdET = dp.IdET,
                                  }).ToList();
                        Session["Lista_Ejercicios"] = lista2;
                        return View(lista2);

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL MOSTRAR DETALLES DE LOS EJERCICIOS", ex);
                    return RedirectToAction("Lista_Fechas");
                }
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Agregar Ejercicio */
        public ActionResult Agregar_Ejercicio_Personalizado()
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

        public ActionResult Agregar_Ejercicio_Personalizado(DetalleEjercicioTCLS oejerciciosCLS, int? id)
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
                        DetalleEjercicioT datos = new DetalleEjercicioT
                        {
                            IdDE = oejerciciosCLS.IdDE,
                            tipoDeEjercicio = oejerciciosCLS.tipoDeEjercicio,
                            repeticiones = oejerciciosCLS.repeticiones,
                            IdET = id,
                        };
                        db.DetalleEjercicioT.Add(datos);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error al Borrar el pago", ex);
                    return RedirectToAction("Lista_Pagos_Persona");
                }
                return RedirectToAction("Lista_Ejercicios_Personalizado", new { @id = id });
            }
        }




        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Eliminar Ejercicio*/
        public ActionResult Eliminar_Ejercicio_Personalizado(int id, int id2)
        {
            try
            {
                using (var db = new AppGym2Entities())
                {
                    DetalleEjercicioT alu = db.DetalleEjercicioT.Find(id);
                    db.DetalleEjercicioT.Remove(alu);
                    db.SaveChanges();
                    return RedirectToAction("Lista_Ejercicios_Personalizado", new { @id = id2 });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Borrar Ejercicio", ex);
                return RedirectToAction("Lista_Ejercicios_Personalizado", new { @id = id2 });
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /* Editar Ejercicio*/

        public ActionResult Editar_Ejercicios_Personalizados(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {

                try
                {
                    using (var db = new AppGym2Entities())
                    {
                        DetalleEjercicioT datos = db.DetalleEjercicioT.Find(id);

                        return View(datos);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Editar_Ejercicios_Personalizados(DetalleEjercicioT datos, int id, int id2)
        {
            if (!ModelState.IsValid)
            {
                return View(datos);
            }
            try
            {
                using (var db = new AppGym2Entities())
                {
                    DetalleEjercicioT dat = db.DetalleEjercicioT.Find(datos.IdDE);
                    dat.IdDE = datos.IdDE;
                    dat.tipoDeEjercicio = datos.tipoDeEjercicio;
                    dat.repeticiones = datos.repeticiones;
                    dat.IdET = datos.IdET;
                    db.SaveChanges();
                    return RedirectToAction("Lista_Ejercicios_Personalizado", new { @id = id2 });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Editar Cliente- ", ex);
            }
            return View();


        }
        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Reporte Excel*/
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
                ew.Cells[1, 1].Value = "Rutina";
                ew.Cells[1, 2].Value = "Nombre: ";
                ew.Cells[2, 1].Value = "Tipo de ejercicio a realizar";
                ew.Cells[2, 2].Value = "Cantidad de repeticiones";
                ew.Column(1).Width = 40;
                ew.Column(2).Width = 40;
                using (var range = ew.Cells[1, 1])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.LightTrellis;
                    range.Style.Font.Color.SetColor(Color.Transparent);
                    range.Style.Fill.BackgroundColor.SetColor(Color.Transparent);
                }
                List<DetalleEjercicioTCLS> lista = (List<DetalleEjercicioTCLS>)Session["Lista_Ejercicios"];
                int nregistros = lista.Count;
                for (int i = 0; i < nregistros; i++)
                {
                    ew.Cells[i + 3, 1].Value = lista[i].tipoDeEjercicio;
                    ew.Cells[i + 3, 2].Value = lista[i].repeticiones;
                }
                ep.SaveAs(ms);
                buffer = ms.ToArray();
            }

            return File(buffer, "application/x-vnd.oasis.opendocument.spreadsheet", "Mediciones.xlsx");
        }
    }
}
