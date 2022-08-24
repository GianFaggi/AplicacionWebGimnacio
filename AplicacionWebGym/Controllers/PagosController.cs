using AplicacionWebGym.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplicacionWebGym.Controllers
{
    public class PagosController : Controller
    {
        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Listar Personas */
        public ActionResult Lista_Pagos_Persona(DatosPersona odatosPersona)
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

                        Session["Lista_Pagos"] = lista;
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
                        Session["Lista_Pagos"] = lista;
                    }
                }
                return View(lista);
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        public ActionResult Lista_Pagos(int id, Pagos pagos)
        {

            try
            {
                List<PagosCLS> lista2 = null;
                using (var db = new PWGBD())
                {
                    lista2 = (from dp in db.Pagos
                             where dp.IdDatos == id
                             select new PagosCLS
                             {
                                 IdPagos = dp.IdPagos,
                                 esAsociado  = dp.esAsociado,
                                 fecha = dp.fecha,
                                 IdDatos = dp.IdDatos
                             }).ToList();
                    Session["Lista_Pagos2"] = lista2;
                    return View(lista2);

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ERROR AL MOSTRAR DETALLES DEL JUGADOR", ex);
                return RedirectToAction("Lista_Pagos");
            }
        }


        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Trae nombre de personar *construccion* */

        public static string nombre_persona(int? IdDatos)
        {
            using (var db = new PWGBD())
            {
                return db.DatosPersona.Find(IdDatos).name;
            }
        }


        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Agregar pago por Clase*/
        public ActionResult Agregar_Pagos()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Agregar_Pagos(PagosCLS opagosCLS, int? id)
        {
            if (!ModelState.IsValid)
            {
                return View(opagosCLS);
            }
            else
            {
                using (var db = new PWGBD())
                {
                    Pagos datospago = new Pagos
                    {
                        IdPagos = opagosCLS.IdPagos,
                        fecha = opagosCLS.fecha,
                        esAsociado = opagosCLS.esAsociado,
                        IdDatos = id
                    };
                    db.Pagos.Add(datospago);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Lista_Pagos_Persona");

        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Detalle Pago */

        public ActionResult DetallePago(int id)
        {
            try
            {
                using (var db = new PWGBD())
                {
                    Pagos datos = db.Pagos.Find(id);
                    return View(datos);

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ERROR AL MOSTRAR DETALLES DEL PAGO", ex);
                return RedirectToAction("Lista_Pagos");
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Eliminar */

        public ActionResult EliminarPago(int id)
        {
            try
            {
                using (var db = new PWGBD())
                {
                    Pagos alu = db.Pagos.Find(id);
                    db.Pagos.Remove(alu);
                    db.SaveChanges();
                    return RedirectToAction("Lista_Pagos_Persona");

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Borrar el pago", ex);
                return RedirectToAction("Lista_Pagos_Persona");
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

                Paragraph titulo = new Paragraph("Listado de Jugadores");
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);

                Paragraph espacio = new Paragraph(" ");
                doc.Add(espacio);

                //Columnas(tabla)
                PdfPTable tabla = new PdfPTable(4);
                //Ancho Columnas
                float[] Valores = new float[4] { 50, 50, 50, 50 };
                //Asignamos esos anchos a la tabla
                tabla.SetWidths(Valores);
                //Creamos las celdas(ponemos contenido)- Color y alineado de centro
                PdfPCell celda1 = new PdfPCell(new Phrase("Nombre"));
                celda1.BackgroundColor = new BaseColor(130, 130, 130);
                celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla.AddCell(celda1);

                PdfPCell celda2 = new PdfPCell(new Phrase("Apellido"));
                celda1.BackgroundColor = new BaseColor(130, 130, 130);
                celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla.AddCell(celda2);

                PdfPCell celda3 = new PdfPCell(new Phrase("Edad"));
                celda1.BackgroundColor = new BaseColor(130, 130, 130);
                celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla.AddCell(celda3);

                PdfPCell celda4 = new PdfPCell(new Phrase("Genero"));
                celda1.BackgroundColor = new BaseColor(130, 130, 130);
                celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla.AddCell(celda4);

                List<DPCLS> lista2 = (List<DPCLS>)Session["Lista_Personas"];
                int nregistros = lista2.Count;
                for (int i = 0; i < nregistros; i++)
                {
                    tabla.AddCell(lista2[i].name);
                    tabla.AddCell(lista2[i].lastName);
                    tabla.AddCell(lista2[i].age.ToString());
                    tabla.AddCell(lista2[i].sex);

                }
                doc.Add(tabla);
                doc.Close();
                buffer = ms.ToArray();
            }
            return File(buffer, "application/pdf");
        }

    }
}
