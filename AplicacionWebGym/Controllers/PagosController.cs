using AplicacionWebGym.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplicacionWebGym.Controllers
{
    public class PagosController : Controller
    {
        public List<Pagos> Pagos { get; }

        public List<DatosPersona> DatosPersona { get; }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public ActionResult Lista_Pagos(PagosCLS opagosCLS, DatosPersona odatosPersona)
        {
            {
                string apellido_persona = odatosPersona.lastName;
                List<PagosCLS> lista = null;
                using (var db = new PWGBD())
                {
                    if (odatosPersona.lastName == null)
                    {
                        lista = (from Pagos in db.Pagos
                                 select new PagosCLS
                                 {
                                     IdPagos = Pagos.IdPagos,
                                     fecha = Pagos.fecha,
                                     esAsociado = Pagos.esAsociado,
                                     IdDatos = Pagos.IdDatos
                                 }).ToList();

                        Session["Lista_Pagos"] = lista;
                    }
                    else
                    {
                        lista = (from pagos in db.Pagos
                                 where odatosPersona.lastName.Contains(apellido_persona)
                                 select new PagosCLS
                                 {
                                     IdPagos = pagos.IdPagos,
                                     fecha = pagos.fecha,
                                     esAsociado = pagos.esAsociado,
                                     IdDatos = pagos.IdDatos
                                 }).ToList();
                        Session["Lista_Personas"] = lista;
                    }
                }
                return View(lista);
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Traer nombre de persona*/

        public ActionResult ListarPersonas()
        {

            using (var db = new PWGBD())
            {
                return PartialView(db.DatosPersona.ToList());
            }

        }

        public static string nombre_persona(int? IdDatos)
        {
            using (var db = new PWGBD())
            {
                return db.DatosPersona.Find(IdDatos).name;
            }
        }


        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Agregar jugador por clase*/

        public ActionResult Agregar_Pagos()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Agregar_Pagos(PagosCLS opagosCLS)
        {
            if (!ModelState.IsValid)
            {
                return View(opagosCLS);
            }
            else
            {
                using (var db = new PWGBD())
                {
                    Pagos pagos = new Pagos();
                    pagos.fecha = opagosCLS.fecha;
                    pagos.esAsociado = opagosCLS.esAsociado;
                    pagos.IdDatos = opagosCLS.IdDatos;
                    db.Pagos.Add(pagos);
                    db.SaveChanges();
                }
            }
            return View("Lista_Pagos");

        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Detalle Jugador */

        public ActionResult DetallePago(int id)
        {
            try
            {
                using (var db = new PWGBD())
                {
                    DatosPersona datos = db.DatosPersona.Find(id);
                    return View(datos);

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ERROR AL MOSTRAR DETALLES DEL JUGADOR", ex);
                return RedirectToAction("Lista_Personas");
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*delete */

        public ActionResult delete(int id)
        {
            try
            {
                using (var db = new PWGBD())
                {
                    DatosPersona alu = db.DatosPersona.Find(id);
                    db.DatosPersona.Remove(alu);
                    db.SaveChanges();
                    return RedirectToAction("Lista_Pagos");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Borrar Jugador", ex);
                return RedirectToAction("Lista_Pagos");
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /* Editar Jugador*/

        public ActionResult Editar(int id)
        {
            try
            {
                using (var db = new PWGBD())
                {
                    Pagos datos = db.Pagos.Find(id);

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

        public ActionResult Editar(Pagos datos)
        {
            if (!ModelState.IsValid)
            {
                return View(datos);
            }
            try
            {
                using (var db = new PWGBD())
                {
                    Pagos dat = db.Pagos.Find(datos.IdPagos);
                    dat.fecha = datos.fecha;
                    dat.esAsociado  = datos.esAsociado;
                    dat.IdDatos = datos.IdDatos;
                    db.SaveChanges();
                    return RedirectToAction("Lista_Pagos");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Editar el Pago- ", ex);
            }
            return View("Lista_Pagos");


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
