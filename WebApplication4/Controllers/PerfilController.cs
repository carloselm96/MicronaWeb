using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class PerfilController : Controller
    {
        [StringLength(20, MinimumLength = 5)]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Caracteres especiales no permitidos")]
        public string newpassword { get; set; }
        // GET: Perfil
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            microna2018Entities db = new microna2018Entities();
            int? id = int.Parse(Session["id"].ToString());
            if (id != null)
            {
                var user = db.usuario.Where(x => x.idUsuario == id).FirstOrDefault();
                return View(user);
            }
            return RedirectToAction("Login", "Home", null);
        }
                

        // GET: Perfil/Edit/5
        public ActionResult Edit(int? id, int? response)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home", null);
            }
            if (id != int.Parse(Session["id"].ToString()))
            {
                return RedirectToAction("Index", "Home", null);
            }
            if (response != null)
            {
                ViewBag.result = "El nombre de usuario ya existe";
            }
            microna2018Entities db = new microna2018Entities();
            var user = db.usuario.Where(x => x.idUsuario == id).FirstOrDefault();
            return View(user);
        }

        // POST: Perfil/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(usuario u, string newpass)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                if (Session["id"].ToString() == null)
                {
                    return RedirectToAction("Index", "Home", null);
                }
                int id = int.Parse(Session["id"].ToString());                
                var user = db.usuario.Where(x => x.idUsuario == id && x.Contraseña==u.Contraseña).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("Contraseña","La contraseña anterior es incorrecta");
                    return View(u);

                }
                if (!string.IsNullOrEmpty(newpass))
                {
                    user.Contraseña = newpass;
                }
                if (ModelState.IsValid)
                {
                    var aux = db.usuario.Where(x => x.Usuario1.ToUpper() == u.Usuario1.ToUpper()).FirstOrDefault();
                    if (aux != null)
                    {
                        if (aux.idUsuario != id)
                        {
                            return RedirectToAction("Edit", new { id = id, response = 2 });
                        }
                    }
                    user.Apellido_Materno = u.Apellido_Materno;
                    user.Apellido_Paterno = u.Apellido_Paterno;
                    user.Nombre = u.Nombre;
                    user.Correo = u.Correo;
                    user.Usuario1 = u.Usuario1;
                    db.SaveChanges();
                    HttpCookie cookie = new HttpCookie("userInfo");
                    cookie["id"] = Session["id"].ToString();
                    cookie["nombre"] = user.Nombre;
                    cookie["user"] = user.Usuario1.ToLower();
                    cookie["tipo"] = Session["tipo"].ToString();
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Index", user.idUsuario);
                }
                else
                {
                    return View(u);
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }       
        
        }
    }
}
