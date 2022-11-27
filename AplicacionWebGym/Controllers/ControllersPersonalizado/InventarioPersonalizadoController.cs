using AplicacionWebGym.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplicacionWebGym.Controllers
{
    public class InventarioPersonalizadoController : Controller
    {
        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Listar Personas */
        public ActionResult Lista_Tipo_Inventario_Personalizado(Inventario oinventario)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                try
                {
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
                    ModelState.AddModelError("ERROR AL LISTAR ITEMS", ex);
                    return RedirectToAction("Index");
                }
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Agregar Tipo inventario*/
        public ActionResult Agregar_Tipo_Inventario_Personalizado()
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

        public ActionResult Agregar_Tipo_Inventario_Personalizado(InventarioCLS oInventarioCLS)

        {

            if (!ModelState.IsValid)
            {
                return View(oInventarioCLS);
            }
            else
            {
                try
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
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL AGREGAR MATERIAL", ex);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Lista_Tipo_Inventario_Personalizado");
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Agregar Detalle*/
        public ActionResult Agregar_Detalle_Inventario_Personalizado(int id)
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Agregar_Detalle_Inventario_Personalizado(DetalleInventarioCLS odetalleInventarioCLS, int id)
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
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ERROR AL AGREGAR DETALLE INVENTARIO", ex);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Lista_Tipo_Inventario_Personalizado" /*new { @id = odetalleInventarioCLS.IdDetalleInventario }*/);

            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Eliminar */

        public ActionResult Eliminar_Tipo_Personalizado(int id)
        {
            try
            {
                using (var db = new AppGym2Entities())
                {
                    Inventario alu = db.Inventario.Find(id);
                    db.Inventario.Remove(alu);
                    db.SaveChanges();
                    return RedirectToAction("Lista_Tipo_Inventario_Personalizado");

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Borrar el pago", ex);
                return RedirectToAction("Lista_Tipo_Inventario_Personalizado");
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*Detalle Inventario */

        public ActionResult Detalle_Inventario_Personalizado(int id)
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
                    return RedirectToAction("Lista_Tipo_Inventario_Personalizado");
                }
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /* Editar Inventario*/

        public ActionResult Editar_Detalles_Personalizado(int id)
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

        public ActionResult Editar_Detalles_Personalizado(DetalleInventario datos, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(datos);
            }
            try
            {
                using (var db = new AppGym2Entities())
                {
                    DetalleInventario dat = db.DetalleInventario.Find(datos.IdDetalleInventario);
                    dat.IdDetalleInventario = datos.IdDetalleInventario;
                    dat.cantidad = datos.cantidad;
                    dat.estado = datos.estado;
                    dat.IdInventario = datos.IdInventario;
                    db.SaveChanges();
                    return RedirectToAction("Detalle_Inventario_Personalizado", new { @id = id });
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