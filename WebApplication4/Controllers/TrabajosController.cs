using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class TrabajosController : Controller
    {
        // GET: Trabajos
        public ActionResult Index()
        {
            micronaEntities db = new micronaEntities();
            var trabajos = db.trabajo.ToList();
            return View(trabajos);
        }

        // GET: Trabajos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Trabajos/Create
        public ActionResult Create()
        {
            micronaEntities db = new micronaEntities();
            ViewBag.tipo = db.tipotrabajo.ToList();
            ViewBag.grupo = db.grupoacademico.ToList();
            return View();
        }

        // POST: Trabajos/Create
        [HttpPost]
        public ActionResult Create(trabajo t)
        {
            try
            {
                micronaEntities db = new micronaEntities();
                t.Usuario = int.Parse(Request.Cookies["userInfo"]["id"]);
                db.trabajo.Add(t);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Trabajos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Trabajos/Edit/5
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

        // GET: Trabajos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Trabajos/Delete/5
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
