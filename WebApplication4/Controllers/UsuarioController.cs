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
        public ActionResult Create(int? response)
        {
            microna2018Entities db = new microna2018Entities();
            ViewBag.TipoUsuario = db.tipoarticulo.ToList();
            if (response != null)
            {
                ViewBag.result = "El nombre de usuario ya existe";
            }
            return View();
        }

        // POST: Usuario/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(usuario u)
        {
            try
            {
                if (Request.Cookies["userInfo"]["tipo"] == "Administrador" || Request.Cookies["userInfo"]["tipo"] == "Super-administrador")
                {
                    if (u.TipoUsuario == 3 && Request.Cookies["userInfo"]["tipo"] != "Super-administrador")
                    {
                        return RedirectToAction("Index", "Home", null);
                    }
                    microna2018Entities db = new microna2018Entities();
                    if (db.usuario.Where(x => x.Usuario1 == u.Usuario1).FirstOrDefault() != null)
                    {
                        return RedirectToAction("Create", new { response = 1 });
                    }
                    db.usuario.Add(u);
                    db.SaveChanges();
                    return RedirectToAction("Index",new {response=1 });
                }
                return RedirectToAction("Index", "Home", null);                
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Edit/5
        [Authorize]
        public ActionResult Edit(int? id, int? response)
        {
            if (id==null)
            {
                return RedirectToAction("Index", "Home", null);
            }
            if (Request.Cookies["userInfo"]["tipo"] == "Administrador" || Request.Cookies["userInfo"]["tipo"] == "Super-administrador")
            {

                microna2018Entities db = new microna2018Entities();
                if (response !=null)
                {
                    ViewBag.result = "El nombre de usuario ya existe";
                }
                var usuario = db.usuario.SingleOrDefault(x => x.idUsuario == id);
                return View(usuario);
            }
            else
            {
                return RedirectToAction("Index", "Home", null);
            }            
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, usuario u)
        {
            if (Request.Cookies["userInfo"]["tipo"] != "Administrador" && Request.Cookies["userInfo"]["tipo"] != "Super-administrador")
            {
                return RedirectToAction("Index", "Home", null);
            }
            try
            {
                microna2018Entities db = new microna2018Entities();
                var aux = db.usuario.Where(x => x.Usuario1 == u.Usuario1).FirstOrDefault();
                if(aux!=null)
                {
                    if (aux.idUsuario != id)
                    {
                        return RedirectToAction("Edit", new { id = id, response = 2 });
                    }                    
                }
                var user = db.usuario.Where(x => x.idUsuario == id).FirstOrDefault();
                user.Nombre = u.Nombre;
                user.Correo = u.Correo;
                user.Contraseña = u.Contraseña;
                user.TipoUsuario = u.TipoUsuario;
                user.Usuario1 = u.Usuario1;
                user.Apellido_Materno = u.Apellido_Materno;
                user.Apellido_Paterno = u.Apellido_Paterno;
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
        [Authorize]
        [HttpPost]
        public ActionResult validateUserName(string usr)
        {
            string output = "True";
            microna2018Entities db = new microna2018Entities();
            if (db.usuario.Where(x => x.Usuario1 == usr).FirstOrDefault()!=null)
            {
                output = "False";
            }
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        // POST: Usuario/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id==null)
                {
                    return RedirectToAction("Index", "Home", null);
                }
                if (Request.Cookies["userInfo"]["tipo"] != "Administrador" && Request.Cookies["userInfo"]["tipo"] != "Super-administrador")
                {
                    return RedirectToAction("Index", "Home", null);
                }
                microna2018Entities db = new microna2018Entities();
                var usuario=db.usuario.Where(x => x.idUsuario == id).FirstOrDefault();
                db.usuario.Remove(usuario);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index",new { response=2 });
            }
        }
    }
}
