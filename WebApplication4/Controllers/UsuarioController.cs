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
        [Authorize]
        public ActionResult Index()
        {
            microna2018Entities db = new microna2018Entities();
            var usuarios = db.usuario.ToList();
            return View(usuarios);
        }
                

        // GET: Usuario/Create
        [Authorize]
        public ActionResult Create()
        {
            microna2018Entities db = new microna2018Entities();
            ViewBag.TipoUsuario = db.tipoarticulo.ToList();
            return View();
        }

        // POST: Usuario/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(usuario u)
        {
            try
            {
                if (Request.Cookies["userInfo"]["tipo"] != "Administrador")
                {
                    return RedirectToAction("Index", "Home", null);
                }
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
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id==null)
            {
                return RedirectToAction("Index", "Home", null);
            }
            if (Request.Cookies["userInfo"]["tipo"] != "Administrador")
            {
                return RedirectToAction("Index", "Home", null);
            }
            microna2018Entities db = new microna2018Entities();
            var usuario = db.usuario.SingleOrDefault(x => x.idUsuario == id);            
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, usuario u)
        {
            if (Request.Cookies["userInfo"]["tipo"] != "Administrador")
            {
                return RedirectToAction("Index", "Home", null);
            }
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
                if (int.Parse(Request.Cookies["userInfo"]["id"]) == id)
                {
                    HttpCookie cookie = new HttpCookie("userInfo");
                    cookie["id"] = Request.Cookies["userInfo"]["id"];
                    cookie["nombre"] = user.Nombre;
                    cookie["user"] = user.Usuario1;
                    cookie["tipo"] = Request.Cookies["userInfo"]["tipo"];
                    Response.Cookies.Add(cookie);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }


        // POST: Usuario/Delete/5
        [Authorize]
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
                return RedirectToAction("Index");
            }
        }
    }
}
