using AplicacionWebGym.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

namespace AplicacionWebGym.Controllers
{
    public class DPPersonalizadoController : Controller
    {
        // GET: DP
        public ActionResult Index()
        {
            return View();
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public ActionResult Lista_Personas_Personalizado(DPCLS odatosCLS)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else

            {
                try
                {
                    string apellido_persona = odatosCLS.lastName;
                    List<DPCLS> lista = null;
                    int IdUsuarioLocal = 0;
                    IdUsuarioLocal = (int)Session["Id"];

                    using (var db = new AppGym2Entities())
                    {
                        if (odatosCLS.lastName == null)
                        {
                            lista = (from datos in db.DatosPersona
                                     where datos.IdUsuario.ToString().Contains(IdUsuarioLocal.ToString())
                                     select new DPCLS
                                     {
                                         IdDatos = datos.IdDatos,
                                         lastName = datos.lastName,
                                         name = datos.name,
                                         age = datos.age,
                                         sex = (DPCLS.sexo?)datos.sex,
                                         IdUsuario = IdUsuarioLocal,
                                     }).ToList();

                            Session["Lista_Personas"] = lista;
                        }
                        else
                        {
                            lista = (from datos in db.DatosPersona
                                     where datos.lastName.Contains(apellido_persona) && datos.IdUsuario.ToString().Contains(IdUsuarioLocal.ToString())
                                     select new DPCLS
                                     {
                                         IdDatos = datos.IdDatos,
                                         lastName = datos.lastName,
                                         name = datos.name,
                                         age = datos.age,
                                         sex = (DPCLS.sexo?)datos.sex,
                                         IdUsuario = IdUsuarioLocal,
                                     }).ToList();
                            Session["Lista_Personas"] = lista;
                        }
                    }
                    return View(lista);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL MOSTRAR LISTA DE PERSONAS", ex);
                    return RedirectToAction("INDEX");
                }

            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Agregar Persona por clase*/

        public ActionResult Agregar_Clientes_Personalizado()
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

        public HttpSessionStateBase GetSession()
        {
            return Session;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Agregar_Clientes_Personalizado(DPCLS odatos2CLS)

        {

            if (!ModelState.IsValid)
            {
                return View(odatos2CLS);
            }
            else
            {
                try
                {

                    int IdUsuarioLocal = 0;
                    IdUsuarioLocal = (int)Session["Id"];

                    using (var db = new AppGym2Entities())
                    {
                        DatosPersona datos2 = new DatosPersona();
                        datos2.name = odatos2CLS.name;
                        datos2.lastName = odatos2CLS.lastName;
                        datos2.age = odatos2CLS.age;
                        datos2.sex = (int?)odatos2CLS.sex;
                        datos2.IdUsuario = IdUsuarioLocal;
                        db.DatosPersona.Add(datos2);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL AGREGAR CLIENTE", ex);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Lista_Personas_Personalizado");

        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Detalle Jugador */

        public ActionResult DetallePersona(int id)
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
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*delete */

        public ActionResult delete(int id)
        {
            try
            {
                using (var db = new AppGym2Entities())
                {
                    DatosPersona alu = db.DatosPersona.Find(id);
                    db.DatosPersona.Remove(alu);
                    db.SaveChanges();
                    return RedirectToAction("Lista_Personas_Personalizado");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Borrar Cliente", ex);
                return RedirectToAction("Lista_Personas_Personalizado");
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /* Editar Jugador*/

        public ActionResult Editar_Personalizado(int id)
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
                        DatosPersona datos = db.DatosPersona.Find(id);

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

        public ActionResult Editar_Personalizado(DatosPersona datos, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(datos);
            }
            try
            {
                using (var db = new AppGym2Entities())
                {
                    DatosPersona dat = db.DatosPersona.Find(datos.IdDatos);
                    dat.IdDatos = datos.IdDatos;
                    dat.name = datos.name;
                    dat.lastName = datos.lastName;
                    dat.age = datos.age;
                    dat.sex = datos.sex;
                    dat.IdUsuario = datos.IdUsuario;
                    db.SaveChanges();
                    return RedirectToAction("Lista_Personas_Personalizado");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Editar Cliente- ", ex);
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

                Paragraph titulo = new Paragraph("Listado de Clientes");
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);

                Paragraph espacio = new Paragraph(" ");
                doc.Add(espacio);

                //Columnas(tabla)
                PdfPTable tabla = new PdfPTable(3);
                //Ancho Columnas
                float[] Valores = new float[3] { 50, 50, 50 };
                //Asignamos esos anchos a la tabla
                tabla.SetWidths(Valores);
                //Creamos las celdas(ponemos contenido)- Color y alineado de centro
                PdfPCell celda1 = new PdfPCell(new Phrase("Nombre"));
                celda1.BackgroundColor = new BaseColor(255, 255, 255);
                celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla.AddCell(celda1);

                PdfPCell celda2 = new PdfPCell(new Phrase("Apellido"));
                celda2.BackgroundColor = new BaseColor(255, 255, 255);
                celda2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla.AddCell(celda2);

                PdfPCell celda3 = new PdfPCell(new Phrase("Edad"));
                celda3.BackgroundColor = new BaseColor(255, 255, 255);
                celda3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla.AddCell(celda3);

                List<DPCLS> lista2 = (List<DPCLS>)Session["Lista_Personas"];
                int nregistros = lista2.Count;
                for (int i = 0; i < nregistros; i++)
                {
                    tabla.AddCell(lista2[i].name);
                    tabla.AddCell(lista2[i].lastName);
                    tabla.AddCell(lista2[i].age.ToString());
                }
                doc.Add(tabla);
                doc.Close();
                buffer = ms.ToArray();
            }
            return File(buffer, "application/pdf");
        }

    }
}