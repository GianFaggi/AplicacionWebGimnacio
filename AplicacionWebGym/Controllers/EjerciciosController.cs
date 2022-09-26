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
    public class EjerciciosController : Controller
    {
        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Listar Personas */
        public ActionResult Lista_Personas_Ejercicios(DatosPersona odatosPersona)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
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

                            Session["Lista_Ejercicios"] = lista;
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
                            Session["Lista_Ejercicios"] = lista;
                        }
                    }
                    return View(lista);
                }
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        public ActionResult Lista_Ejercicios(int id, Ejercicios ejercicios)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                try
                {
                    List<EjerciciosCLS> lista2 = null;
                    using (var db = new PWGBD())
                    {
                        lista2 = (from dp in db.Ejercicios
                                  where dp.IdDatos == id
                                  select new EjerciciosCLS
                                  {
                                      IdEjercicios = dp.IdEjercicios,
                                      abdominales = dp.abdominales,
                                      fecha_ej = dp.fecha_ej,
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
        /*Trae nombre de personar*/

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
        /*Agregar pago por Clase*/
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

        public ActionResult Agregar_Ejercicio(EjerciciosCLS oejerciciosCLS, int? id)
        {
            if (!ModelState.IsValid)
            {
                return View(oejerciciosCLS);
            }
            else
            {
                using (var db = new PWGBD())
                {
                    Ejercicios datos = new Ejercicios
                    {
                        IdEjercicios = oejerciciosCLS.IdEjercicios,
                        abdominales = oejerciciosCLS.abdominales,
                        fecha_ej =  oejerciciosCLS.fecha_ej,
                        IdDatos = id
                    };
                    db.Ejercicios.Add(datos);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Lista_Personas_Ejercicios");

        }


        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /* Editar Jugador*/

        public ActionResult EditarEjercicios(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                try
                {
                    using (var db = new PWGBD())
                    {
                        Ejercicios datos = db.Ejercicios.Find(id);

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

        public ActionResult EditarEjercicios(Ejercicios datos, int id, int id2)
        {
            if (!ModelState.IsValid)
            {
                return View(datos);
            }
            try
            {
                using (var db = new PWGBD())
                {
                    Ejercicios dat = db.Ejercicios.Find(datos.IdEjercicios);
                    dat.IdEjercicios = id;
                    dat.abdominales = datos.abdominales;
                    dat.fecha_ej = datos.fecha_ej;
                    dat.IdDatos = id2;

                    db.SaveChanges();
                    return RedirectToAction("Lista_Personas_Ejercicios");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Editar Jugador- ", ex);
            }
            return View();


        }


        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Detalle Pago */

        public ActionResult DetalleEjercicios(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                try
                {
                    using (var db = new PWGBD())
                    {
                        Ejercicios datos = db.Ejercicios.Find(id);
                        return View(datos);

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL MOSTRAR DETALLES DE LOS EJERCICIOS", ex);
                    return RedirectToAction("Lista_Ejercicios");
                }
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Eliminar */

        public ActionResult EliminarEjercicio(int id)
        {
            try
            {
                using (var db = new PWGBD())
                {
                    Ejercicios alu = db.Ejercicios.Find(id);
                    db.Ejercicios.Remove(alu);
                    db.SaveChanges();
                    return RedirectToAction("Lista_Personas_Ejercicios");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Borrar Ejercicio", ex);
                return RedirectToAction("Lista_Personas_Ejercicios");
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
