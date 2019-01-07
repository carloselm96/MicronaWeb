using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;
using WebApplication4.Models.DataAccess;

namespace WebApplication4.Controllers
{
    public class UsuarioController : Controller
    {
        DataAccess dt = new DataAccess();


        // GET: Usuario
        [Authorize]
        public ActionResult Index()
        {
            microna2018Entities db = new microna2018Entities();
            var usuarios = db.usuario.Where(x => x.Status == "A").ToList();            
            return View(usuarios);
        }
                

        // GET: Usuario/Create
        [Authorize]
        public ActionResult Create(int? response)
        {
            microna2018Entities db = new microna2018Entities();
            ViewBag.TipoUsuario = dt.getTiposUsuario();
            return View();
        }

        // POST: Usuario/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(usuario u)
        {
            try
            {
                if (validateAccess())
                {
                    if (u.TipoUsuario == 3 && Request.Cookies["userInfo"]["tipo"] != "Super-administrador")
                    {
                        return RedirectToAction("Index", "Home", null);
                    }                    
                    if (!ModelState.IsValid)
                    {
                        ViewBag.TipoUsuario = dt.getTiposUsuario();
                        return View(u);
                    }
                    if (!dt.validateUserName(u.Usuario1,null))
                    {
                        ModelState.AddModelError("Usuario1", "El usuario ya existe");
                        ViewBag.TipoUsuario = dt.getTiposUsuario();
                        return View(u);
                    }
                    dt.createUsuario(u);
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
            if (validateAccess())
            {
                var usuario = dt.getUsuarioById(id.GetValueOrDefault());
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
            if (!validateAccess())
            {
                return RedirectToAction("Index", "Home", null);
            }
            try
            {
                if (dt.getUsuarioById(id) == null)
                {
                    return RedirectToAction("Index", "Home", null);
                }
                if (!dt.validateUserName(u.Usuario1, id))
                {
                    ModelState.AddModelError("Usuario1", "El usuario ya existe");
                    ViewBag.TipoUsuario = dt.getTiposUsuario();
                    return View(u);
                }
                
                if (int.Parse(Request.Cookies["userInfo"]["id"]) == id)
                {
                    HttpCookie cookie = new HttpCookie("userInfo");
                    cookie["id"] = Request.Cookies["userInfo"]["id"];
                    cookie["nombre"] = u.Nombre;
                    cookie["user"] = u.Usuario1;
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
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id==null)
                {
                    return RedirectToAction("Index", "Home", null);
                }
                if (!validateAccess())
                {
                    return RedirectToAction("Index", "Home", null);
                }
                if (!dt.deleteUsuario(id.GetValueOrDefault()))
                {
                    return RedirectToAction("Index", new { response = 2 });
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index",new { response=2 });
            }
        }

        public bool validateAccess()
        {
            if (Request.Cookies["userInfo"]["tipo"] != "Administrador" && Request.Cookies["userInfo"]["tipo"] != "Super-administrador")
            {
                return false;
            }
            return true;
        }
    }
}
