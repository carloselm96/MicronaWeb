using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class CapituloLibroController : Controller
    {

        microna2018Entities db = new microna2018Entities();

        [Authorize]        
        public ActionResult Index(string response)
        {            
            if (response != null)
            {
                ViewBag.response = int.Parse(response);
            }
            
            ViewBag.grupos = db.grupoacademico.ToList();
            ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
            ViewBag.tipo = db.tipolibro.ToList();
            List<capitulolibro> libros = db.capitulolibro.ToList();            
            return View(libros);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Search(string Nombre, List<string> autores, string Lugar, DateTime? Y1, DateTime? Y2, List<string> grupos, string Libro)
        {                        
            ViewBag.grupos = db.grupoacademico.ToList();
            ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
            ViewBag.tipo = db.tipolibro.ToList();
            List<capitulolibro> libros = db.capitulolibro.ToList();
            if (Nombre != null)
            {
                libros = libros.Where(x => x.Nombre.Contains(Nombre)).ToList();
            }
            if (Y1 != null)
            {
                //int year1 = int.Parse(Y1);
                libros = libros.Where(x => x.Año >= Y1).ToList();
            }
            if (Y2 != null)
            {
                libros = libros.Where(x => x.Año <= Y2).ToList();
            }
            if (Libro != null)
            {
                libros = libros.Where(x => x.Libro.Contains(Libro)).ToList();
            }
            if (grupos != null)
            {
                List<capitulolibro> cg = new List<capitulolibro>();
                foreach (string s in grupos)
                {
                    int i = int.Parse(s);
                    var g = db.capitulo_grupo.Where(x => x.id_grupo == i).ToList();
                    
                    foreach (var cap in g)
                    {
                        capitulolibro sample = db.capitulolibro.Where(x => x.idCapituloLibro == cap.id_capitulo).FirstOrDefault();
                        cg.Add(sample);
                    }
                    
                }
                libros = libros.Where(x => cg.Contains(x)).ToList();
            }
            if (autores != null)
            {
                foreach (string s in autores)
                {
                    int i = int.Parse(s);
                    var g = db.capitulo_usuario.Where(x => x.idUsuario == i).ToList();
                    List<capitulolibro> cg = new List<capitulolibro>();
                    foreach (var cap in g)
                    {
                        capitulolibro sample = db.capitulolibro.Where(x => x.idCapituloLibro == cap.idCapitulo).FirstOrDefault();
                        cg.Add(sample);
                    }
                    libros = libros.Where(x => cg.Contains(x)).ToList();
                }
            }
            return PartialView("_CapituloPartial",libros);
        }

        // GET: CapituloLibro/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "CapituloLibro", null);
            }
            
            var capitulo = db.capitulolibro.Where(x => x.idCapituloLibro == id).FirstOrDefault();
            return View(capitulo);
        }

        // GET: CapituloLibro/Create
        [Authorize]
        public ActionResult Create()
        {
                        
            ViewBag.grupo = db.grupoacademico.ToList();
            ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
            return View();
        }

        // POST: CapituloLibro/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(capitulolibro lib, HttpPostedFileBase ffile, List<string> GrupoAcademico, List<string> Autores)
        {
            archivo file = null;
            
            if (!ModelState.IsValid)
            {
                ViewBag.grupo = db.grupoacademico.ToList();
                ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
                return View(lib);
            }
            if (Autores.Count < 1)
            {
                ViewBag.grupo = db.grupoacademico.ToList();
                ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
                ModelState.AddModelError("Nombre", "El campo autores no puede ir vacio");
                return View(lib);
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
                    lib.Archivo = file.idarchivo;
                }
                lib.Usuario = int.Parse(Request.Cookies["userInfo"]["id"]);
                db.capitulolibro.Add(lib);
                if (GrupoAcademico != null)
                {
                    foreach (var s in GrupoAcademico)
                    {
                        capitulo_grupo ag = new capitulo_grupo
                        {
                            id_capitulo = lib.idCapituloLibro,
                            id_grupo = int.Parse(s)
                        };
                        db.capitulo_grupo.Add(ag);
                    }
                }
                if (Autores != null)
                {
                    foreach (var s in Autores)
                    {
                        capitulo_usuario lb = new capitulo_usuario
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

        // GET: CapituloLibro/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "CapituloLibro", null);
            }
            
            var a = db.capitulolibro.Where(x => x.idCapituloLibro == id).FirstOrDefault();
            if (int.Parse(Request.Cookies["userInfo"]["id"]) != a.Usuario)
            {
                return RedirectToAction("Index");
            }
            a.capitulo_grupo = db.capitulo_grupo.Where(x => x.id_capitulo == id).ToList();
            ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
            ViewBag.grupos = db.grupoacademico.ToList();            
            return View(a);
        }

        // POST: CapituloLibro/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, capitulolibro lib, List<string> GrupoAcademico, HttpPostedFileBase ffile, List<string> Autores)
        {
            
            if (!ModelState.IsValid)
            {
                ViewBag.grupo = db.grupoacademico.ToList();
                ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
                return View(lib);
            }
            if (Autores==null)
            {
                ViewBag.grupo = db.grupoacademico.ToList();
                ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
                ModelState.AddModelError("Nombre", "El campo autores no puede ir vacio");
                return View(lib);
            }
            try
            {                
                var l = db.capitulolibro.Where(x => x.idCapituloLibro == id).FirstOrDefault();
                l.Nombre = lib.Nombre;
                l.ISBN = lib.ISBN;                                
                l.Participantes = lib.Participantes;
                l.Año = lib.Año;
                var autores_eliminar = db.capitulo_usuario.Where(x => x.idCapitulo == id).ToList();
                if (autores_eliminar != null)
                {
                    foreach (var G in autores_eliminar)
                    {
                        db.capitulo_usuario.Remove(G);
                    }
                }
                if (Autores != null)
                {
                    foreach (var G in Autores)
                    {
                        db.capitulo_usuario.Add(new capitulo_usuario { idCapitulo = id, idUsuario = int.Parse(G) });
                    }
                }
                var grupos_eliminar = db.capitulo_grupo.Where(x => x.id_capitulo == id).ToList();
                if (grupos_eliminar != null)
                {
                    foreach (var G in grupos_eliminar)
                    {
                        db.capitulo_grupo.Remove(G);
                    }
                }
                if (GrupoAcademico != null)
                {
                    foreach (var G in GrupoAcademico)
                    {
                        db.capitulo_grupo.Add(new capitulo_grupo { id_capitulo = id, id_grupo = int.Parse(G) });
                    }
                }
                if (ffile != null && ffile.ContentLength > 0)
                {
                    if (l.archivo1 != null)
                    {
                        var archivo = new archivo();
                        archivo = db.archivo.Where(x => x.idarchivo == l.Archivo).FirstOrDefault();
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
                    l.Archivo = file.idarchivo;
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { response = 1 });
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "CapituloLibro", null);
                }
                
                var libr = db.capitulolibro.Where(x => x.idCapituloLibro == id).FirstOrDefault();
                if (int.Parse(Request.Cookies["UserInfo"]["Id"]) != libr.Usuario)
                {
                    return RedirectToAction("Index");
                }
                var a_g = db.capitulo_grupo.Where(x => x.id_capitulo == id).ToList();
                if (a_g != null)
                {
                    foreach (var a in a_g)
                    {
                        db.capitulo_grupo.Remove(a);
                    }
                }
                db.capitulolibro.Remove(libr);
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
