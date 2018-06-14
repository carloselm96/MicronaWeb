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
        public ActionResult Index(string titulo, int? Y1, int? Y2, string autores, int?[] checkgroup, int?[] checktype)
        {
            microna2018Entities db = new microna2018Entities();
            var concent = db.concentrado.ToList();
            if (titulo != null)
            {
                concent = concent.Where(x => x.Titulo.Contains(titulo)).ToList();
            }
            if (Y1 != null)
            {
                concent = concent.Where(x => x.Fecha >= Y1).ToList();
            }
            if (Y2 != null)
            {
                concent = concent.Where(x => x.Fecha <= Y2).ToList();
            }
            if (checkgroup!=null)
            {
                foreach (int i in checkgroup)
                {
                    var g = db.concentrado_grupos.Where(x => x.Grupo == i).ToList();
                    List<concentrado> cg = new List<concentrado>();
                    foreach (var con in g)
                    {
                        concentrado sample = db.concentrado.Where(x => x.idConcentrado == con.Concentrado).FirstOrDefault();
                        cg.Add(sample);
                    }
                    concent = concent.Where(x => cg.Contains(x)).ToList();
                }
            }
            if (checktype != null)
            {
                List<concentrado> ct = new List<concentrado>();
                foreach (int i in checktype)
                {
                    var g = concent.Where(x => x.TipoConcentrado == i).ToList();
                    foreach (var con in g)
                    {                        
                        ct.Add(con);
                    }                    
                }
                concent = ct;
            }
            return View(concent);
        }


        /*[Authorize]
        public ActionResult Index(List<String> grupo)
        {
            microna2018Entities db = new microna2018Entities();
            var concentrado= db.concentrado.ToList();
            if (grupo[0] != null)
            {
                concentrado = concentrado.Where(x => x.concentrado_grupos.Where(y => y.Grupo == 1).ToList() != null).ToList();
            }
            if (grupo[2] != null)
            {
                concentrado = concentrado.Where(x => x.concentrado_grupos.Where(y => y.Grupo == 2).ToList() != null).ToList();
            }
            return View(concent);
        }*/

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(usuario u)
        {
            microna2018Entities db = new microna2018Entities();
            var user = db.usuario.Where(x => u.Usuario1 == x.Usuario1).FirstOrDefault();            
            if (user!=null)
            {
                if (user.Contraseña== u.Contraseña)
                {
                    //FormsAuthentication.SetAuthCookie(user.Usuario1, false);
                    FormsAuthenticationTicket authTicket = new
                        FormsAuthenticationTicket(1, //version
                        user.Usuario1, // user name
                        DateTime.Now,             //creation
                        DateTime.Now.AddDays(1), //Expiration (you can set it to 1 month
                        false,  //Persistent
                        user.tipousuario1.Nombre);
                    HttpCookie cookie1 = new HttpCookie(
                    FormsAuthentication.FormsCookieName,
                    FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie1);
                    HttpCookie userInfo = new HttpCookie("userInfo");
                    userInfo.Values.Add("id", user.idUsuario.ToString());
                    userInfo.Values.Add("nombre", user.Nombre);
                    userInfo.Values.Add("tipo", user.tipousuario1.Nombre);
                    userInfo.Values.Add("user", user.Usuario1);
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