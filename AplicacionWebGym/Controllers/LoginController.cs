using AplicacionWebGym.Models;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplicacionWebGym.Controllers
{
    public class LoginController : Controller
    {
        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /* estructura login*/
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]


        public ActionResult Login(login_DB oUser)
        {
            int Tipo = 1;
            if (ModelState.IsValid)
            {
                using (AppGym2Entities db = new AppGym2Entities())
                {
                    var obj = db.login_DB.Where(a => a.usuario.Equals(oUser.usuario) && a.contraseña.Equals(oUser.contraseña)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.id.ToString();
                        Session["UserName"] = obj.usuario.ToString();
                        Session["Id"] = obj.id;
                        if (Tipo.Equals(obj.tipo)){
                            return RedirectToAction("Index", "HomeFuncional");
                        }
                        else
                        {
                            return RedirectToAction("Index", "HomePersonalizado");
                        }
                    }
                }
            }
            return View(oUser);
        }



        /*---------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /* cerrar sesion. */
        public ActionResult Logaut()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login");
        }


    }
}


