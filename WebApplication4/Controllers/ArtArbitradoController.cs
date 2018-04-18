using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class ArtArbitradoController : Controller
    {
        // GET: ArtArbitrado
        [Authorize]
        public ActionResult Index()
        {
            micronaEntities db = new micronaEntities();
            var art = db.artarbitrado.ToList();
            return View(art);
        }

        // GET: ArtArbitrado/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ArtArbitrado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArtArbitrado/Create
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

        // GET: ArtArbitrado/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ArtArbitrado/Edit/5
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

        // GET: ArtArbitrado/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ArtArbitrado/Delete/5
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
