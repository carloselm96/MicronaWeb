using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class PerfilController : Controller
    {
        // GET: Perfil
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            microna2018Entities db = new microna2018Entities();
            int? id = int.Parse(Request.Cookies["userInfo"]["id"]);
            if (id != null)
            {
                var user = db.usuario.Where(x => x.idUsuario == id).FirstOrDefault();
                return View(user);
            }
            return RedirectToAction("Login", "Home", null);
        }
                

        // GET: Perfil/Edit/5
        public ActionResult Edit(int id)
        {
            if (id != int.Parse(Request.Cookies["userInfo"]["id"]))
            {
                return RedirectToAction("Index", "Home", null);
            }
            microna2018Entities db = new microna2018Entities();
            var user = db.usuario.Where(x => x.idUsuario == id).FirstOrDefault();
            return View(user);
        }

        // POST: Perfil/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int? id, usuario u)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                var user = db.usuario.Where(x => x.idUsuario == id).FirstOrDefault();
                user.Nombre = u.Nombre;
                user.Correo = u.Correo;
                user.Contraseña = u.Contraseña;                
                user.Usuario1 = u.Usuario1;
                db.SaveChanges();
                HttpCookie cookie = new HttpCookie("userInfo");
                cookie["id"] = Request.Cookies["userInfo"]["id"];
                cookie["nombre"] = user.Nombre;
                cookie["user"] = user.Usuario1;
                cookie["tipo"] = Request.Cookies["userInfo"]["tipo"];
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index",user.idUsuario);
            }
            catch
            {
                return RedirectToAction("Index",id);
            }       
        
        }
    }
}
