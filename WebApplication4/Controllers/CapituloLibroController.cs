using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication4.Controllers
{
    public class CapituloLibroController : Controller
    {
        // GET: CapituloLibro
        public ActionResult Index()
        {
            return View();
        }

        // GET: CapituloLibro/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CapituloLibro/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CapituloLibro/Create
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

        // GET: CapituloLibro/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CapituloLibro/Edit/5
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

        // GET: CapituloLibro/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CapituloLibro/Delete/5
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
