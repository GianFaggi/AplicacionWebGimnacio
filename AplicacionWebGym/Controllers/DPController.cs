﻿using AplicacionWebGym.Models;
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
    public class DPController : Controller
    {
        // GET: DP
        public ActionResult Index()
        {
            return View();
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public ActionResult Lista_Personas(DPCLS odatosCLS)
        {
            {
                string apellido_persona = odatosCLS.lastName;
                List<DPCLS> lista = null;
                using (var db = new PWGBD())
                {
                    if (odatosCLS.lastName == null)
                    {
                        lista = (from datos in db.DatosPersona
                                 select new DPCLS
                                 {
                                     IdDatos = datos.IdDatos,
                                     lastName = datos.lastName,
                                     name = datos.name,
                                     age = datos.age,
                                     sex = datos.sex,
                                     IdTurnos = (DPCLS.turno?)datos.IdTurnos,


                                 }).ToList();

                        Session["Lista_Personas"] = lista;
                    }
                    else
                    {
                        lista = (from datos in db.DatosPersona
                                 where datos.lastName.Contains(apellido_persona)
                                 select new DPCLS
                                 {
                                     IdDatos = datos.IdDatos,
                                     lastName = datos.lastName,
                                     name = datos.name,
                                     age = datos.age,
                                     sex = datos.sex,
                                     IdTurnos = (DPCLS.turno?)datos.IdTurnos
                                 }).ToList();
                        (from turno in db.Turnos
                         select new TurnosCLS
                         {
                             IdTurnos = turno.IdTurnos,
                             Horario = (int?)turno.Horario
                         }).ToList();

                        Session["Lista_Personas"] = lista;
                    }
                }
                return View(lista);
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Agregar jugador por clase*/

        public ActionResult Agregar_Clientes()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Agregar_Clientes(DPCLS odatosCLS)
        {
            if (!ModelState.IsValid)
            {
                return View(odatosCLS);
            }
            else
            {
                using (var db = new PWGBD())
                {
                    DatosPersona datos = new DatosPersona();
                    datos.name = odatosCLS.name;
                    datos.lastName = odatosCLS.lastName;
                    datos.age = odatosCLS.age;
                    datos.sex = odatosCLS.sex;
                    datos.IdTurnos = (int?)odatosCLS.IdTurnos;
                    db.DatosPersona.Add(datos);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Lista_Personas");

        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Detalle Jugador */

        public ActionResult DetallePersona(int id)
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
                    ModelState.AddModelError("ERROR AL MOSTRAR DETALLES DEL CLIENTE", ex);
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
                        return RedirectToAction("Lista_Personas");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error al Borrar Jugador", ex);
                    return RedirectToAction("Lista_Personas");
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
                    DatosPersona datos = db.DatosPersona.Find(id);

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

            public ActionResult Editar(DatosPersona datos)
            {
                if (!ModelState.IsValid)
                {
                    return View(datos);
                }
                try
                {
                    using (var db = new PWGBD())
                    {
                    DatosPersona dat = db.DatosPersona.Find(datos.IdDatos);
                        dat.name = datos.name;
                        dat.lastName = datos.lastName;
                        dat.age = datos.age;
                        dat.sex = datos.sex;
                        dat.IdTurnos = datos.IdTurnos;


                        db.SaveChanges();
                        return RedirectToAction("Lista_Personas");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error al Editar Jugador- ", ex);
                }
                return View();


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
                float[] Valores = new float[4] {50, 50, 50, 50 };
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

    
