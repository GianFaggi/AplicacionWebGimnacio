using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplicacionWebGym.Controllers
{
    public class EjerciciosController : Controller
    {
        // GET: Ejercicios
        public ActionResult Index()
        {
            return View();
        }

        // GET: Ejercicios/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Ejercicios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ejercicios/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Ejercicios/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Ejercicios/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Ejercicios/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Ejercicios/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
