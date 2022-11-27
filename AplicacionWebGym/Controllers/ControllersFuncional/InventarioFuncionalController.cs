using AplicacionWebGym.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Input;

namespace AplicacionWebGym.Controllers
{
    public class InventarioFuncionalController : Controller
    {
        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Listar Personas */
        public ActionResult Lista_Tipo_Inventario(Inventario oinventario)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
               try {
                    string TipoInventario = oinventario.TipoInventario;
                    List<InventarioCLS> lista = null;
                    int IdUsuarioLocal = 0;
                    IdUsuarioLocal = (int)Session["Id"];
                    using (var db = new AppGym2Entities())
                    {
                        if (oinventario.TipoInventario == null)
                        {
                            lista = (from dp in db.Inventario
                                     where dp.IdUsuario.ToString().Contains(IdUsuarioLocal.ToString())
                                     select new InventarioCLS
                                     {
                                         IdInventario = dp.IdInventario,
                                         TipoInventario = dp.TipoInventario,
                                         IdUsuario = dp.IdUsuario,
                                     }).ToList();

                            Session["Lista_Pagos"] = lista;
                        }
                        else
                        {
                            lista = (from dp in db.Inventario
                                     where dp.TipoInventario.Contains(TipoInventario) && dp.IdUsuario.ToString().Contains(IdUsuarioLocal.ToString())
                                     select new InventarioCLS
                                     {
                                         IdInventario = dp.IdInventario,
                                         TipoInventario = dp.TipoInventario,
                                         IdUsuario = dp.IdUsuario
                                     }).ToList();
                            Session["Lista_Pagos"] = lista;
                        }
                    }
                    return View(lista);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL MOSTRAR LISTA", ex);
                    return RedirectToAction("Lista_Pagos_Persona");
                }
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Agregar Tipo inventario*/
        public ActionResult Agregar_Tipo_Inventario()
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

        public ActionResult Agregar_Tipo_Inventario(InventarioCLS oInventarioCLS)

        {

            if (!ModelState.IsValid)
            {
                return View(oInventarioCLS);
            }
            else
                try
                {
                    {
                        int IdUsuarioLocal = 0;
                        IdUsuarioLocal = (int)Session["Id"];

                        using (var db = new AppGym2Entities())
                        {
                            Inventario datos = new Inventario();
                            datos.TipoInventario = oInventarioCLS.TipoInventario;
                            datos.IdUsuario = IdUsuarioLocal;
                            db.Inventario.Add(datos);
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Lista_Tipo_Inventario");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL AGREGAR MATERIAL", ex);
                    return RedirectToAction("Lista_Pagos_Persona");
                }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Agregar Detalle*/
        public ActionResult Agregar_Detalle_Inventario(int id)
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

        public ActionResult Agregar_Detalle_Inventario(DetalleInventarioCLS odetalleInventarioCLS, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(odetalleInventarioCLS);
            }
            else
            {
                try
                {


                    using (var db = new AppGym2Entities())
                    {
                        DetalleInventario di = new DetalleInventario
                        {
                            IdDetalleInventario = odetalleInventarioCLS.IdDetalleInventario,
                            cantidad = odetalleInventarioCLS.cantidad,
                            estado = (int?)odetalleInventarioCLS.estado,
                            IdInventario = id
                        };
                        db.DetalleInventario.Add(di);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Lista_Tipo_Inventario" /*new { @id = odetalleInventarioCLS.IdDetalleInventario }*/);
                }

                catch (Exception ex)
                {
                    ModelState.AddModelError("Error al agregar detalle", ex);
                    return RedirectToAction("Lista_Pagos_Persona");
                }

            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Eliminar */

        public ActionResult Eliminar_Tipo(int id)
        {
            try
            {
                using (var db = new AppGym2Entities())
                {
                    Inventario alu = db.Inventario.Find(id);
                    db.Inventario.Remove(alu);
                    db.SaveChanges();
                    return RedirectToAction("Lista_Tipo_Inventario");

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Borrar el pago", ex);
                return RedirectToAction("Lista_Pagos_Persona");
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Detalle Inventario */

        public ActionResult Detalle_Inventario(int id)
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
                        DetalleInventario datos = db.DetalleInventario.Find(id);
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
        /* Editar Inventario*/

        public ActionResult Editar_Detalles(int id)
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
                        DetalleInventario datos = db.DetalleInventario.Find(id);

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

        public ActionResult Editar_Detalles(DetalleInventario datos, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(datos);
            }
            try
            {
                using (var db = new AppGym2Entities())
                {
                    DetalleInventario dat = db.DetalleInventario.Find(datos.IdInventario);
                    dat.IdDetalleInventario = datos.IdDetalleInventario;
                    dat.cantidad = datos.cantidad;
                    dat.estado = datos.estado;
                    dat.IdInventario = datos.IdInventario;
                    db.SaveChanges();
                    return RedirectToAction("Detalle_Inventario", new { @id = id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Editar Cliente- ", ex);
            }
            return View();


        }


    }
}
    /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
    /*Generacion Pdf*/
    /*
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
    */
