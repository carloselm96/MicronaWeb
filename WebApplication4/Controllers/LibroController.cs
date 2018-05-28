using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class LibroController : Controller
    {
        // GET: Libro
        public ActionResult Index(string response)
        {
            if (response != null)
            {
                ViewBag.response = int.Parse(response);
            }
            microna2018Entities db = new microna2018Entities();
            var libros = db.libro.ToList();
            return View(libros);
        }

        // GET: Libro/Details/5
        public ActionResult Details(int id)
        {
            microna2018Entities db = new microna2018Entities();
            var libro = db.libro.Where(x => x.idLibro == id).FirstOrDefault();
            return View(libro);
        }

        // GET: Libro/Create
        public ActionResult Create()
        {
            microna2018Entities db = new microna2018Entities();
            ViewBag.tipolibro = db.tipolibro.ToList();
            ViewBag.grupo = db.grupoacademico.ToList();
            return View();
        }

        // POST: Libro/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(libro lib, HttpPostedFileBase ffile, List<string> GrupoAcademico)
        {
            archivo file = null;
            try
            {
                string dir = "~/Content/Archivos/Libros";
                string fileName = "";
                string path = "";
                microna2018Entities db = new microna2018Entities();
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
                db.libro.Add(lib);
                if (GrupoAcademico != null)
                {
                    foreach (var s in GrupoAcademico)
                    {
                        libro_grupo ag = new libro_grupo
                        {
                            id_libro = lib.idLibro,
                            id_grupo = int.Parse(s)
                        };
                        db.libro_grupo.Add(ag);
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

        // GET: Libro/Edit/5
        public ActionResult Edit(int id)
        {
            microna2018Entities db = new microna2018Entities();
            var a = db.libro.Where(x => x.idLibro == id).FirstOrDefault();
            if (int.Parse(Request.Cookies["userInfo"]["id"]) != a.Usuario)
            {
                return RedirectToAction("Index");
            }
            a.libro_grupo = db.libro_grupo.Where(x => x.id_libro == id).ToList();
            ViewBag.grupos = db.grupoacademico.ToList();
            ViewBag.tipolibro = db.tipolibro.ToList();
            return View(a);
        }

        // POST: Libro/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, libro lib, List<string> GrupoAcademico, HttpPostedFileBase ffile)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                var l = db.libro.Where(x => x.idLibro == id).FirstOrDefault();
                l.Nombre = lib.Nombre;
                l.ISBN = lib.ISBN;
                l.TipoLibro = lib.TipoLibro;
                l.Autores = lib.Autores;
                l.Año = lib.Año;                
                var grupos_eliminar = db.libro_grupo.Where(x => x.id_libro == id).ToList();
                if (grupos_eliminar != null)
                {
                    foreach (var G in grupos_eliminar)
                    {
                        db.libro_grupo.Remove(G);
                    }
                }
                if (GrupoAcademico != null)
                {
                    foreach (var G in GrupoAcademico)
                    {
                        db.libro_grupo.Add(new libro_grupo { id_libro = id, id_grupo = int.Parse(G) });
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
                    string dir = "~/Content/Archivos/Libros";
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
        

        // POST: Libro/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                var libr = db.libro.Where(x => x.idLibro == id).FirstOrDefault();
                if (int.Parse(Request.Cookies["UserInfo"]["Id"]) != libr.Usuario)
                {
                    return RedirectToAction("Index");
                }
                var a_g = db.libro_grupo.Where(x => x.id_libro == id).ToList();
                if (a_g != null)
                {
                    foreach (var a in a_g)
                    {
                        db.libro_grupo.Remove(a);
                    }
                }
                db.libro.Remove(libr);
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
