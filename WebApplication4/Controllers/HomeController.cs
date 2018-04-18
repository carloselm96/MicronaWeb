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
            micronaEntities db = new micronaEntities();
            var user = db.usuario.Where(x => u.Usuario1 == x.Usuario1).FirstOrDefault();
            if (user!=null)
            {
                if (user.Status == 1 && user.Contrasena== u.Contrasena)
                {
                    FormsAuthentication.SetAuthCookie(user.tipousuario1.nombre, true);
                    HttpCookie userInfo = new HttpCookie("userInfo");
                    userInfo.Values.Add("id", user.idUsuario.ToString());
                    userInfo.Values.Add("nombre", user.Nombre + " " + user.Apelidos);
                    userInfo.Values.Add("tipo", user.tipousuario1.nombre);
                    userInfo.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(userInfo);
                    return RedirectToAction("Index","Home");                    
                }
            }          
            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        public ActionResult Logout()
        {
            if(Request.Cookies["userInfo"] != null)
            {
                Request.Cookies["userInfo"].Expires = DateTime.Now.AddDays(-1);
            }
            FormsAuthentication.SignOut();            
            return RedirectToAction("Login", "Home");
        }

    }
}