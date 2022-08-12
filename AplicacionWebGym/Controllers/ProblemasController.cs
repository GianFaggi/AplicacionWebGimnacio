using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplicacionWebGym.Controllers
{
    public class ProblemasController : Controller
    {
        // GET: Problemas
        public ActionResult Index()
        {
            return View();
        }

        // GET: Problemas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Problemas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Problemas/Create
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

        // GET: Problemas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Problemas/Edit/5
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

        // GET: Problemas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Problemas/Delete/5
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
