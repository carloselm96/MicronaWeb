using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;
using WebApplication4.Models.DataAccess;

namespace WebApplication4.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        DataAccess dt = new DataAccess();


        // GET: Usuario
        
        public ActionResult Index()
        {
            microna2018Entities db = new microna2018Entities();
            var usuarios = db.usuario.Where(x => x.Status == "A").ToList();            
            return View(usuarios);
        }
                

        // GET: Usuario/Create
        
        public ActionResult Create(int? response)
        {
            microna2018Entities db = new microna2018Entities();
            ViewBag.TipoUsuario = dt.getTiposUsuario();
            return View();
        }

        // POST: Usuario/Create
        
        [HttpPost]
        public ActionResult Create(usuario u)
        {
            try
            {
                if (validateAccess())
                {
                    if (u.TipoUsuario == 3 && Session["tipo"].ToString().Equals("3"))
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
        
        public ActionResult Edit(int? id, int? response)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "Home", null);
                }
                if (validateAccess())
                {
                    ViewBag.TipoUsuario = dt.getTiposUsuario();
                    var usuario = dt.getUsuarioById(id.GetValueOrDefault());
                    return View(usuario);
                }
                else
                {
                    return RedirectToAction("Index", "Home", null);
                }
            }
            catch(Exception e)
            {
                return RedirectToAction("Index", "Home", null);
            }
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        
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
                dt.editUsuario(u, id);
                if (int.Parse(Session["id"].ToString()) == id)
                {
                    HttpCookie cookie = new HttpCookie("userInfo");
                    cookie["id"] = Session["id"].ToString();
                    cookie["nombre"] = u.Nombre;
                    cookie["user"] = u.Usuario1;
                    cookie["tipo"] = Session["tipo"].ToString();
                    Response.Cookies.Add(cookie);
                }
                return RedirectToAction("Index", new { response = 1 });
            }
            catch
            {
                return RedirectToAction("Index", new { response = 2 });
            }
        }

        // POST: Usuario/Delete/5
                
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
                return RedirectToAction("Index", new { response = 1 });
            }
            catch
            {
                return RedirectToAction("Index",new { response=2 });
            }
        }

        public bool validateAccess()
        {
            if (Session["tipo"].ToString() == "2" ||  Session["tipo"].ToString() == "3")
            {
                return true;
            }
            return false;
        }
    }
}
