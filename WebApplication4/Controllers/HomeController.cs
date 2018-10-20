using System;
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
        microna2018Entities db = new microna2018Entities();
        // GET: Home        
        [Authorize]
        public ActionResult Index()
        {         
            var concent = db.concentrado.ToList();
            ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();            
            return View(concent);
        }


        [Authorize]
        [HttpGet]
        public ActionResult Search(string titulo, DateTime? Y1, DateTime? Y2, List<string> autores, int?[] checkgroup, int?[] checktype)
        {            
            var concent = db.concentrado.ToList();
            ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
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
            if (checkgroup != null)
            {
                List<concentrado> cg = new List<concentrado>();
                foreach (int i in checkgroup)
                {                    
                    var g = db.concentrado_grupos.Where(x => x.Grupo == i).ToList();                    
                    foreach (var con in g)
                    {
                        concentrado sample = db.concentrado.Where(x => x.idConcentrado == con.Concentrado).FirstOrDefault();
                        cg.Add(sample);
                    }                    
                }
                concent = concent.Where(x => cg.Contains(x)).ToList();
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
            if (autores != null)
            {
                List<concentrado> cg = new List<concentrado>();
                foreach (string s in autores)
                {
                    int i = int.Parse(s);
                    var g = db.concentrado_autores.Where(x => x.idAutor == i).ToList();                    
                    foreach (var cap in g)
                    {
                        concentrado sample = db.concentrado.Where(x => x.idConcentrado == cap.idConcentrado).FirstOrDefault();
                        cg.Add(sample);
                    }                    
                }
                concent = concent.Where(x => cg.Contains(x)).ToList();
            }
            return PartialView("_ConcentradoPartial", concent);
        }

        [Authorize]
        [HttpGet]
        public ActionResult getPieData()
        {
            
            var data = new List<pieDataModel>();
            var articulos = db.articulo.ToList();            
            var libros = db.libro.ToList();            
            var trabajos = db.trabajo.ToList();
            var proyectos = db.proyectos.ToList();
            var capitulos = db.capitulolibro.ToList();
            data.Add(new pieDataModel { nameSection = "Articulos", valueNumber = articulos.Count, color= "#f56954" });
            data.Add(new pieDataModel { nameSection = "Libros", valueNumber = libros.Count, color= "#00a65a" });
            data.Add(new pieDataModel { nameSection = "Trabajos", valueNumber = trabajos.Count, color= "#f39c12" });
            data.Add(new pieDataModel { nameSection = "proyectos", valueNumber = proyectos.Count, color= "#00c0ef" });
            data.Add(new pieDataModel { nameSection = "Capitulos", valueNumber = capitulos.Count, color= "#3c8dbc" });
            var dataForChart = data.Select(x => new { value = x.valueNumber, colorl = x.color, label =x.nameSection });
            return Json(dataForChart, JsonRequestBehavior.AllowGet);
        }


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
            
            ViewBag.tipos_art = db.tipoarticulo.ToList();
            ViewBag.tipos_tra = db.tipotrabajo.ToList();
            ViewBag.tipos_lib = db.tipolibro.ToList();
            ViewBag.tipos_grup = db.grupoacademico.ToList();
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
            var user = db.usuario.Where(x => u.Usuario1.ToUpper() == x.Usuario1.ToUpper() && x.Status.Equals("A")).FirstOrDefault();
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
                    return RedirectToAction("Index", "Home");
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
                Request.Cookies["userInfo"].Expires = DateTime.Now.AddDays(-1);
            }
            FormsAuthentication.SignOut();            
            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        public ActionResult nuevoTipo(string name, string type)
        {

            try
            {
                switch (type)
                {
                    case "1":
                        tipoarticulo t = new tipoarticulo { Nombre = name };
                        db.tipoarticulo.Add(t);
                        break;
                    case "2":
                        tipotrabajo a = new tipotrabajo { Nombre = name };
                        db.tipotrabajo.Add(a);
                        break;
                    case "3":
                        tipolibro b = new tipolibro { Nombre = name };
                        db.tipolibro.Add(b);
                        break;
                    case "4":
                        grupoacademico g = new grupoacademico { Nombre = name };
                        db.grupoacademico.Add(g);
                        break;

                }
                db.SaveChanges();
                return RedirectToAction("Configure", new { result = "1" });
            }
            catch
            {
                return RedirectToAction("Configure", new { result = "2" });
            }
        }

        [Authorize]
        public ActionResult editarTipo(string name, string type, int id)
        {
                        
            switch (type)
            {
                case "1":
                    tipoarticulo t = db.tipoarticulo.Where(x => x.idTipoArticulo == id).FirstOrDefault();
                    t.Nombre = name;
                    break;
                case "2":
                    tipotrabajo a = db.tipotrabajo.Where(x => x.idTipoTrabajo == id).FirstOrDefault();
                    a.Nombre = name;
                    break;
                case "3":
                    tipolibro b  = db.tipolibro.Where(x => x.idTipoLibro == id).FirstOrDefault();
                    b.Nombre = name;
                    break;
                case "4":
                    grupoacademico g = db.grupoacademico.Where(x => x.idGrupoAcademico == id).FirstOrDefault();
                    g.Nombre = name;
                    break;

            }
            db.SaveChanges();
            return RedirectToAction("Configure");
        }


        [Authorize]
        public ActionResult eliminarTipo(string tipo, int id)
        {
            
            switch (tipo)
            {
                case "1":
                    tipoarticulo t = db.tipoarticulo.Where(x => x.idTipoArticulo == id).FirstOrDefault();
                    db.tipoarticulo.Remove(t);                    
                    break;
                case "2":
                    tipotrabajo a = db.tipotrabajo.Where(x => x.idTipoTrabajo == id).FirstOrDefault();
                    db.tipotrabajo.Remove(a);
                    break;
                case "3":
                    tipolibro b = db.tipolibro.Where(x => x.idTipoLibro == id).FirstOrDefault();
                    db.tipolibro.Remove(b);
                    break;
                case "4":
                    grupoacademico g = db.grupoacademico.Where(x => x.idGrupoAcademico == id).FirstOrDefault();
                    db.grupoacademico.Remove(g);
                    break;

            }
            db.SaveChanges();
            return RedirectToAction("Configure");
        }
    }
}