using AplicacionWebGym.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplicacionWebGym.Controllers.ControllersPersonalizado
{
    public class PagosPersonalizadoController : Controller
    {
        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Listar Personas */
        public ActionResult Lista_Pagos_Persona_Personalizado(DatosPersona odatosPersona)
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

                            Session["Lista_Pagos"] = lista;
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
                            Session["Lista_Pagos"] = lista;
                        }
                    }
                    return View(lista);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL MOSTRAR LISTA CLIENTES", ex);
                    return RedirectToAction("Index");
                }
            }
        }
        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        public ActionResult Lista_Pagos_Personalizado(int id, Pagos pagos)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                try
                {
                    List<PagosCLS> lista2 = null;
                    using (var db = new AppGym2Entities())
                    {
                        lista2 = (from dp in db.Pagos
                                  where dp.IdDatos == id
                                  select new PagosCLS
                                  {
                                      IdPagos = dp.IdPagos,
                                      esAsociado = (PagosCLS.esAsoc?)dp.esAsociado,
                                      fecha = dp.fecha,
                                      IdDatos = dp.IdDatos
                                  }).ToList();
                        Session["Lista_Pagos2"] = lista2;
                        return View(lista2);

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL MOSTRAR LISTA DE PAGOS", ex);
                    return RedirectToAction("Lista_Pagos_Persona_Personalizado");
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
        /*Agregar pago por Clase*/
        public ActionResult Agregar_Pagos_Personalizado()
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

        public ActionResult Agregar_Pagos_Personalizado(PagosCLS opagosCLS, int? id)
        {
            if (!ModelState.IsValid)
            {
                return View(opagosCLS);
            }
            else
            {
                using (var db = new AppGym2Entities())
                {
                    Pagos datospago = new Pagos
                    {
                        IdPagos = opagosCLS.IdPagos,
                        fecha = opagosCLS.fecha,
                        esAsociado = (int?)opagosCLS.esAsociado,
                        IdDatos = id
                    };
                    db.Pagos.Add(datospago);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Lista_Pagos_Persona_Personalizado");

        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Eliminar */

        public ActionResult EliminarPago(int id, int id2)
        {
            try
            {
                using (var db = new AppGym2Entities())
                {
                    Pagos alu = db.Pagos.Find(id);
                    db.Pagos.Remove(alu);
                    db.SaveChanges();
                    return RedirectToAction("Lista_Pagos_Personalizado", new { @id = id2 });

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Borrar el pago", ex);
                return RedirectToAction("Lista_Pagos_Personalizado");
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Generacion Pdf*/
        public FileResult generarPDF()
        {
            Document doc = new Document();
            byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                Paragraph titulo = new Paragraph("Listado de Pagos realizados");
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);

                Paragraph espacio = new Paragraph(" ");
                doc.Add(espacio);

                //Columnas(tabla)
                PdfPTable tabla = new PdfPTable(2);
                //Ancho Columnas
                float[] Valores = new float[2] { 50, 50 };
                //Asignamos esos anchos a la tabla
                tabla.SetWidths(Valores);
                //Creamos las celdas(ponemos contenido)- Color y alineado de centro
                PdfPCell celda1 = new PdfPCell(new Phrase("Fecha de Pago"));
                celda1.BackgroundColor = new BaseColor(255, 255, 255);
                celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla.AddCell(celda1);

                PdfPCell celda2 = new PdfPCell(new Phrase("Socio"));
                celda2.BackgroundColor = new BaseColor(255, 255, 255);
                celda2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla.AddCell(celda2);

                List<PagosCLS> lista2 = (List<PagosCLS>)Session["Lista_Pagos2"];
                int nregistros = lista2.Count;
                for (int i = 0; i < nregistros; i++)
                {
                    tabla.AddCell(lista2[i].fecha.ToString());
                    tabla.AddCell(lista2[i].esAsociado.ToString());

                }
                doc.Add(tabla);
                doc.Close();
                buffer = ms.ToArray();
            }
            return File(buffer, "application/pdf");
        }
    }
}