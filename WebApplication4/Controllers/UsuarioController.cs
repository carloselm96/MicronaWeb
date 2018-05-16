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
            microna2018Entities db = new microna2018Entities();
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
            microna2018Entities db = new microna2018Entities();
            ViewBag.TipoUsuario = db.tipoarticulo.ToList();
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        public ActionResult Create(usuario u)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
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
            microna2018Entities db = new microna2018Entities();
            var usuario = db.usuario.SingleOrDefault(x => x.idUsuario == id);            
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, usuario u)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                var user = db.usuario.Where(x => x.idUsuario == id).FirstOrDefault();
                user.Nombre = u.Nombre;
                user.Correo = u.Correo;
                user.Contraseña = u.Contraseña;
                user.TipoUsuario = u.TipoUsuario;
                user.Usuario1 = u.Usuario1;
                db.SaveChanges();
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
                microna2018Entities db = new microna2018Entities();
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
