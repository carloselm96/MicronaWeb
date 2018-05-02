using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            micronaEntities db = new micronaEntities();
            var usuarios = db.usuario.ToList();
            return View(usuarios);
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            micronaEntities db = new micronaEntities();
            ViewBag.TipoUsuario = db.tipoarticulo.ToList();
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        public ActionResult Create(usuario u)
        {
            try
            {
                micronaEntities db = new micronaEntities();
                db.usuario.Add(u);
                db.SaveChanges();                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            micronaEntities db = new micronaEntities();
            var usuario = db.usuario.SingleOrDefault(x => x.idUsuario == id);            
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                micronaEntities db = new micronaEntities();
                var usuario = db.usuario.Where(x => x.idUsuario == id);
                
                if (TryUpdateModel(usuario, "", collection))
                {
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        

        // POST: Usuario/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                micronaEntities db = new micronaEntities();
                var usuario=db.usuario.Where(x => x.idUsuario == id).FirstOrDefault();
                db.usuario.Remove(usuario);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
