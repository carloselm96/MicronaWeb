using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class TesisController : Controller
    {
        // GET: Tesis
        microna2018Entities db = new microna2018Entities();

        [Authorize]
        public ActionResult Index(string response)
        {
            if (response != null)
            {
                ViewBag.response = response;
            }
            ViewBag.grupos = db.grupoacademico.ToList();
            ViewBag.autores = db.usuario.Where(x => x.Status.Equals("A")).ToList();
            List<tesis> tesis = db.tesis.ToList();
            return View(tesis);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Search(string Nombre, string autor, DateTime? Y1, DateTime? Y2, List<string> grupos)
        {
            ViewBag.grupos = db.grupoacademico.ToList();
            ViewBag.autores = db.usuario.Where(x => x.Status.Equals("A")).ToList();            
            List<tesis> tesis = db.tesis.ToList();
            if (Nombre != null)
            {
                tesis = tesis.Where(x => x.titulo.Contains(Nombre)).ToList();
            }
            if (Y1 != null)
            {
                //int year1 = int.Parse(Y1);
                tesis = tesis.Where(x => x.fecha >= Y1).ToList();
            }
            if (Y2 != null)
            {
                tesis = tesis.Where(x => x.fecha <= Y2).ToList();
            }            
            if (grupos != null)
            {
                List<tesis> cg = new List<tesis>();
                foreach (string s in grupos)
                {
                    int i = int.Parse(s);
                    var g = db.tesis_grupo.Where(x => x.idgrupo == i).ToList();
                    
                    foreach (var cap in g)
                    {
                        tesis sample = db.tesis.Where(x => x.idtesis == cap.idtesis).FirstOrDefault();
                        cg.Add(sample);
                    }
                    
                }
                tesis = tesis.Where(x => cg.Contains(x)).ToList();
            }
            if (autor != null)
            {

            }
            return PartialView("_TesisPartial", tesis);
        }

        // GET: Tesis/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Capitulotesisro", null);
            }

            var tesis = db.tesis.Where(x => x.idtesis == id).FirstOrDefault();
            return View(tesis);
        }

        // GET: Tesis/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.grupo = db.grupoacademico.ToList();
            ViewBag.autores = db.usuario.Where(x => x.Status.Equals("A")).ToList();
            return View();
        }

        // POST: Tesis/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(tesis tesis, HttpPostedFileBase ffile, List<string> GrupoAcademico, List<string> Autores)
        {
            archivo file = null;

            if (!ModelState.IsValid)
            {
                ViewBag.grupo = db.grupoacademico.ToList();
                ViewBag.autores = db.usuario.Where(x => x.Status.Equals("A")).ToList();
                return View(tesis);
            }
            try
            {
                string dir = "~/Content/Archivos/Capitulos";
                string fileName = "";
                string path = "";
                if (!Directory.Exists(dir))
                {
                    DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(dir));
                }
                if (ffile != null && ffile.ContentLength > 0)
                {
                    fileName = Path.GetFileName(ffile.FileName);
                    path = Path.Combine(Server.MapPath(dir), DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + fileName);
                    ffile.SaveAs(path);
                    file = new archivo();
                    file.Nombre = fileName;
                    file.url = path;
                    db.archivo.Add(file);
                    db.SaveChanges();
                }

                if (file != null)
                {
                    tesis.archivo = file.idarchivo;
                }
                tesis.usuario = int.Parse(Request.Cookies["userInfo"]["id"]);
                db.tesis.Add(tesis);
                if (GrupoAcademico != null)
                {
                    foreach (var s in GrupoAcademico)
                    {
                        tesis_grupo ag = new tesis_grupo
                        {
                            idtesis = tesis.idtesis,
                            idgrupo = int.Parse(s)
                        };
                        db.tesis_grupo.Add(ag);
                    }
                }
                if (Autores != null)
                {
                    foreach (var s in Autores)
                    {
                        tesis_usuario lb = new capitulo_usuario
                        {
                            idCapitulo = lib.idCapituloLibro,
                            idUsuario = int.Parse(s)
                        };
                        db.capitulo_usuario.Add(lb);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { response = 1 });
            }
            catch
            {                
                return RedirectToAction("Index", new { response = 2 });
            }
        }

        // GET: Tesis/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Tesis", null);
            }

            var a = db.tesis.Where(x => x.idtesis == id).FirstOrDefault();
            if (int.Parse(Request.Cookies["userInfo"]["id"]) != a.usuario)
            {
                return RedirectToAction("Index");
            }
            a.tesis_grupo = db.tesis_grupo.Where(x => x.idtesis == id).ToList();
            ViewBag.autores = db.usuario.Where(x => x.Status.Equals("A")).ToList();
            ViewBag.grupos = db.grupoacademico.ToList();
            return View(a);
        }

        // POST: Tesis/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, tesis tesis, List<string> GrupoAcademico, HttpPostedFileBase ffile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.grupo = db.grupoacademico.ToList();
                ViewBag.autores = db.usuario.Where(x => x.Status.Equals("A")).ToList();
                return View(tesis);
            }          
            try
            {
                var l = db.tesis.Where(x => x.idtesis == id).FirstOrDefault();
                l.titulo = tesis.titulo;
                l.jurado = tesis.jurado;
                l.asesor = tesis.asesor;
                l.codirector = tesis.codirector;
                l.director = tesis.director;
                l.usuario2 = tesis.usuario2;
                l.fecha = tesis.fecha;                                
                var grupos_eliminar = db.tesis_grupo.Where(x => x.idtesis == id).ToList();
                if (grupos_eliminar != null)
                {
                    foreach (var G in grupos_eliminar)
                    {
                        db.tesis_grupo.Remove(G);
                    }
                }
                if (GrupoAcademico != null)
                {
                    foreach (var G in GrupoAcademico)
                    {
                        db.tesis_grupo.Add(new tesis_grupo { idtesis = id, idgrupo = int.Parse(G) });
                    }
                }
                if (ffile != null && ffile.ContentLength > 0)
                {
                    if (l.archivo1 != null)
                    {
                        var archivo = new archivo();
                        archivo = db.archivo.Where(x => x.idarchivo == l.archivo).FirstOrDefault();
                        System.IO.File.Delete(l.archivo1.url);
                        db.archivo.Remove(archivo);
                    }
                    string dir = "~/Content/Archivos/Capitulos";
                    if (!Directory.Exists(dir))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(dir));
                    }
                    string fileName = Path.GetFileName(ffile.FileName);
                    string path = Path.Combine(Server.MapPath(dir), DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + fileName);
                    ffile.SaveAs(path);
                    archivo file = new archivo();
                    file.Nombre = fileName;
                    file.url = path;
                    db.archivo.Add(file);
                    db.SaveChanges();
                    l.archivo = file.idarchivo;
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { response = 1 });
            }
            catch
            {
                return View();
            }
        }

        // GET: Tesis/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "Tesis", null);
                }

                var libr = db.tesis.Where(x => x.idtesis == id).FirstOrDefault();
                if (int.Parse(Request.Cookies["UserInfo"]["Id"]) != libr.usuario)
                {
                    return RedirectToAction("Index");
                }
                var a_g = db.tesis_grupo.Where(x => x.idtesis == id).ToList();
                if (a_g != null)
                {
                    foreach (var a in a_g)
                    {
                        db.tesis_grupo.Remove(a);
                    }
                }
                db.tesis.Remove(libr);
                db.SaveChanges();
                return RedirectToAction("Index", new { response = 1 });
            }
            catch
            {
                return RedirectToAction("Index", new { response = 2 });
            }
        }

        [Authorize]
        public FileResult Download(string Url, string name)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@Url);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }
    }

    
}
