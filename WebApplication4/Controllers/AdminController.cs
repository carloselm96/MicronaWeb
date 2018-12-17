using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication4.Models;
using WebApplication4.Models.DataAccess;

namespace WebApplication4.Controllers
{
    public class AdminController : Controller
    {
        DataAccess dt = new DataAccess();
        // GET: Home        
        [Authorize]
        public ActionResult Index()
        {
            var concent = dt.getConcentrado();
            ViewBag.grupos = dt.getGrupos();
            ViewBag.autores = dt.getAutores();
            return View(concent);
        }


        [Authorize]
        [HttpGet]
        public ActionResult Search(string titulo, DateTime? Y1, DateTime? Y2, List<string> autores, int?[] checkgroup, int?[] checktype)
        {
            var concent = dt.getAllConcentrado(titulo, Y1, Y2, autores, checkgroup, checktype);
            ViewBag.grupos = dt.getConcentrado();
            ViewBag.autores = dt.getAutores();            
            return PartialView("_ConcentradoPartial", concent);
        }

     /*   [Authorize]
        [HttpGet]
        public ActionResult getPieData()
        {
            
            var data = new List<pieDataModel>();
            int articulos = dt.getAllArticulos().Count;           
            var libros = db.libro.ToList();            
            var trabajos = db.trabajo.ToList();
            var proyectos = db.proyectos.ToList();
            var capitulos = db.capitulolibro.ToList();
            data.Add(new pieDataModel { nameSection = "Articulos", valueNumber = articulos, color= "#f56954" });
            data.Add(new pieDataModel { nameSection = "Libros", valueNumber = libros.Count, color= "#00a65a" });
            data.Add(new pieDataModel { nameSection = "Trabajos", valueNumber = trabajos.Count, color= "#f39c12" });
            data.Add(new pieDataModel { nameSection = "proyectos", valueNumber = proyectos.Count, color= "#00c0ef" });
            data.Add(new pieDataModel { nameSection = "Capitulos", valueNumber = capitulos.Count, color= "#3c8dbc" });
            var dataForChart = data.Select(x => new { value = x.valueNumber, colorl = x.color, label =x.nameSection });
            return Json(dataForChart, JsonRequestBehavior.AllowGet);
        }*/


        [Authorize]
        public PartialViewResult getModal(string type, string modal, int? id, string name)
        {
            switch (modal)
            {
                case "Create":
                    return PartialView("_CreateModal", new TypeModel { type=type });                    
                case "Edit":
                    return PartialView("_EditModal", new TypeModel { id=int.Parse(id+""), type=type, name=name });
               
            }
            return null;           
        }

        [Authorize]
        public ActionResult Configure()
        {
            
            ViewBag.tipos_art = dt.getTiposArticulo();
            ViewBag.tipos_tra = dt.getTiposTrabajo();
            ViewBag.tipos_lib = dt.getTiposLibro();
            ViewBag.tipos_grup = dt.getGrupos();
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
            if (!ModelState.IsValidField("Usuario1") || !ModelState.IsValidField("Contraseña"))
            {
                return View();
            }                                    
            var user = dt.getAutores().Where(x => u.Usuario1.ToUpper() == x.Usuario1.ToUpper() && x.Status.Equals("A")).FirstOrDefault();
            if (user != null)
            {
                if (user.Contraseña == u.Contraseña)
                {                        
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
                    userInfo.Values.Add("user", user.Usuario1.ToLower());
                    userInfo.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(userInfo);
                    return RedirectToAction("Index", "Admin");
                }
            }
            ModelState.AddModelError("Contraseña", "Contraseña o Usuario Incorrectos");
            return View();                        
        }

        [Authorize]
        public ActionResult Logout()
        {
            if(Request.Cookies["userInfo"] != null)
            {
                HttpCookie currentUserCookie = Request.Cookies["userInfo"];
                Response.Cookies.Remove("userInfo");
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                Response.SetCookie(currentUserCookie);
                Request.Cookies["userInfo"].Expires = DateTime.Now.AddDays(-1);
            }
            FormsAuthentication.SignOut();            
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult nuevoTipo(string name, string type)
        {

            if (dt.addTipo(name, type))
            {
                return RedirectToAction("Configure", new { result = "1" });
            }
            return RedirectToAction("Configure", new { result = "2" });             
        }

        [Authorize]
        public ActionResult editarTipo(string name, string type, int id)
        {
            dt.editTipo(name, type, id);
            return RedirectToAction("Configure");
        }


        [Authorize]
        public ActionResult eliminarTipo(string tipo, int id)
        {

            dt.deleteTipo(tipo, id);
            return RedirectToAction("Configure");
        }
    }
}