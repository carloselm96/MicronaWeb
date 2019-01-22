using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;
using WebApplication4.Models.DataAccess;

namespace WebApplication4.Controllers
{
    public class LibroController : Controller
    {
        //microna2018Entities db = new microna2018Entities();
        DataAccess dt = new DataAccess();

        [Authorize]
        public ActionResult Index(string response)
        {
            if (response != null)
            {
                ViewBag.response = int.Parse(response);
            }
            ViewBag.autores = dt.getAutores();
            ViewBag.grupos = dt.getGrupos();
            List<libro> libros = dt.getAllLibros();
            return View(libros);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Search(string Nombre, DateTime? Y1, DateTime? Y2, List<string> grupos, List<string> autores)
        {            
            
            ViewBag.autores = dt.getAutores();
            ViewBag.grupos = dt.getGrupos();
            var libros = dt.getFilteredLibros(Nombre, Y1, Y2, grupos, autores);
            return PartialView("_LibroPartial",libros);
        }
        // GET: Libro/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Libro", null);
            }
            
            var libro = dt.getLibroById(id.GetValueOrDefault());
            if (libro==null)
            {
                return RedirectToAction("Index", "Libro", null); 
            }
            return View(libro);
        }

        // GET: Libro/Create
        public ActionResult Create()
        {
            
            ViewBag.tipolibro = dt.getTipoLibro();
            ViewBag.grupo = dt.getGrupos();
            ViewBag.autores = dt.getAutores();
            return View();
        }

        // POST: Libro/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(libro lib, HttpPostedFileBase ffile, List<string> GrupoAcademico, List<string> Autores)
        {
            archivo file = null;
            try
            {
                string dir = "~/Content/Archivos/Libros";
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
                }
                lib.Usuario = int.Parse(Session["id"].ToString());
                dt.createLibro(lib, file, GrupoAcademico, Autores);
                return RedirectToAction("Index", new { response = 1 });
            }
            catch(Exception e)
            {
                //return Content("" + e);
                return RedirectToAction("Index", new { response = 2 });
            }
        }

        // GET: Libro/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Libro", null);
            }
            
            var a = dt.getLibroById(id.GetValueOrDefault());
            if (int.Parse(Session["id"].ToString()) != a.Usuario && Session["tipo"].ToString().Equals("2"))
            {
                return RedirectToAction("Index");
            }
            a.libro_grupo = dt.GetLibro_Grupos(id);
            ViewBag.grupos = dt.getGrupos();
            ViewBag.tipolibro = dt.getTipoLibro();
            ViewBag.autores = dt.getAutores();
            return View(a);
        }

        // POST: Libro/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, libro lib, List<string> GrupoAcademico, HttpPostedFileBase ffile, List<string> Autores)
        {
            try
            {
                archivo file = null;
                if (ffile != null && ffile.ContentLength > 0)
                {

                    string dir = "~/Content/Archivos/Libros";
                    if (!Directory.Exists(dir))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(dir));
                    }
                    string fileName = Path.GetFileName(ffile.FileName);
                    string path = Path.Combine(Server.MapPath(dir), DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + fileName);
                    ffile.SaveAs(path);
                    file = new archivo();
                    file.Nombre = fileName;
                    file.url = path;
                }
                dt.editLibro(id, lib, GrupoAcademico, file, Autores);
                return RedirectToAction("Index", new { response = 1 });
            }
            catch
            {
                return View();
            }
        }
        

        // POST: Libro/Delete/5        
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "Libro", null);
                }
                
                var libr = dt.getLibroById(id.GetValueOrDefault());
                if (int.Parse(Session["id"].ToString()) != libr.Usuario)
                {
                    return RedirectToAction("Index");
                }
                dt.removeLibro(id.GetValueOrDefault());
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
