using AplicacionWebGym.Models;
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

        public ActionResult Login(loginDB objUser)
        {
            if (ModelState.IsValid)
            {
                using (PWGBD db = new PWGBD())
                {
                    var obj = db.loginDB.Where(a => a.usuario.Equals(objUser.usuario) && a.contraseña.Equals(objUser.contraseña)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.id.ToString();
                        Session["UserName"] = obj.usuario.ToString();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(objUser);
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


