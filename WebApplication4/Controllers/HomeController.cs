using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(usuario u)
        {
            micronaEntities2 db = new micronaEntities2();
            var user = db.usuario.Where(x => u.Usuario1 == x.Usuario1).FirstOrDefault();
            if (user!=null)
            {
                if (user.Status == 1 && user.Contrasena== u.Contrasena)
                {
                    FormsAuthentication.SetAuthCookie(user.TipoUsuario+"", true);
                    HttpContext.Session["idUser"] = user.idUsuario;
                    HttpContext.Session["nombreUser"]= user.Nombre+ " " +user.Apelidos;
                    HttpContext.Session["tipoUser"] = user.tipousuario1.nombre;
                    return RedirectToAction("Index","Home");                    
                }
            }          
            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        public ActionResult Logout()
        {
            HttpContext.Session["idUser"] = null; 
            HttpContext.Session["nombreUser"] = null;
            HttpContext.Session["tipoUser"] = null;
            FormsAuthentication.SignOut();            
            return RedirectToAction("Login", "Home");
        }

    }
}