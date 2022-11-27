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
using static iTextSharp.text.pdf.AcroFields;

namespace AplicacionWebGym.Controllers
{

    public class MedidasController : Controller
    {
        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Listar Personas */
        public ActionResult Lista_Personas_Medidas(DatosPersona odatosPersona)
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

                            Session["Lista_Medidas"] = lista;
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
                            Session["Lista_Medidas"] = lista;
                        }
                    }
                    return View(lista);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("listar personas el pago", ex);
                    return RedirectToAction("Lista_Pagos_Persona");
                }
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /* Listar Medidas tomadas por id */
        public ActionResult Lista_Medidas(int id, Medidas medidas)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                try
                {
                    List<MedidasCLS> lista2 = null;
                    using (var db = new AppGym2Entities())
                    {
                        lista2 = (from dp in db.Medidas
                                  where dp.IdDatos == id
                                  select new MedidasCLS
                                  {
                                      IdMedidas = dp.IdMedidas,
                                      altura = (int)dp.altura,
                                      medidasAbd = (int)dp.medidasAbd,
                                      medidasCintura = (int)dp.medidasCintura,
                                      medidasPecho = (int)dp.medidasPecho,
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
        }


        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Trae nombre de personar */


        public static string nombre_persona(int? IdDatos)
        {
            using (var db = new AppGym2Entities())
            {
                return db.DatosPersona.Find(IdDatos).name;
            }
        }


        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Agregar medida */
        public ActionResult Agregar_Medidas()
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

        public ActionResult Agregar_Medidas(MedidasCLS omedidasCLS, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(omedidasCLS);
            }
            else
            {
                try
                {
                    using (var db = new AppGym2Entities())
                    {
                        Medidas datos = new Medidas
                        {
                            IdMedidas = omedidasCLS.IdMedidas,
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
                catch (Exception)
                {
                    throw;
                }

            }
            return RedirectToAction("Lista_medidas", new { @id = id });

        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Eliminar */

        public ActionResult EliminarMedida(int id, int id2)
        {
            try
            {
                using (var db = new AppGym2Entities())
                {
                    Medidas alu = db.Medidas.Find(id2);
                    db.Medidas.Remove(alu);
                    db.SaveChanges();
                    return RedirectToAction("Lista_medidas", new { @id = id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Borrar Jugador", ex);
                return RedirectToAction("Lista_medidas", new { @id = id });
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
                ep.Workbook.Worksheets.Add("Reporte Medidas");
                ExcelWorksheet ew = ep.Workbook.Worksheets[0];
                //Creamos y ponemos nombre a las columnas
                ew.Cells[1, 1].Value = "Medidas tomadas";
                ew.Cells[2, 1].Value = "Toma medidas Abdominales";
                ew.Cells[2, 2].Value = "Toma medidas Cintura";
                ew.Cells[2, 3].Value = "Toma medidas Pecho";
                ew.Cells[2, 4].Value = "fecha realizacion medidas";
                ew.Column(1).Width = 40;
                ew.Column(2).Width = 40;
                ew.Column(3).Width = 40;
                ew.Column(4).Width = 40;
                using (var range = ew.Cells[1, 1, 1, 4])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Color.SetColor(Color.Transparent);
                    range.Style.Fill.BackgroundColor.SetColor(Color.Transparent);

                }
                List<MedidasCLS> lista = (List<MedidasCLS>)Session["Lista_Medidas2"];
                int nregistros = lista.Count;
                for (int i = 0; i < nregistros; i++)
                {
                    ew.Cells[i + 3, 1].Value = lista[i].medidasAbd;
                    ew.Cells[i + 3, 2].Value = lista[i].medidasCintura;
                    ew.Cells[i + 3, 3].Value = lista[i].medidasPecho;
                    ew.Cells[i + 3, 4].Value = lista[i].fecha_med.ToString();
                }
                ep.SaveAs(ms);
                buffer = ms.ToArray();
            }

            return File(buffer, "application/x-vnd.oasis.opendocument.spreadsheet", "Mediciones.xlsx");
        }

    }
}





