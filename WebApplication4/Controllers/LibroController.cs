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
            return View();
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
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Libro/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Libro/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
