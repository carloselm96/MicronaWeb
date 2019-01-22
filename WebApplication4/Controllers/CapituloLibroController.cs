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
    public class CapituloLibroController : Controller
    {

        DataAccess dt = new DataAccess();
        

        [Authorize]        
        public ActionResult Index(string response)
        {            
            if (response != null)
            {
                ViewBag.response = int.Parse(response);
            }
            
            ViewBag.grupos = dt.getGrupos();
            ViewBag.autores = dt.getAutores();
            
            List<capitulolibro> libros=dt.getAllCapitulos();          
            return View(libros);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Search(string Nombre, List<string> autores, string Lugar, DateTime? Y1, DateTime? Y2, List<string> grupos, string Libro)
        {                        
            ViewBag.grupos = dt.getGrupos();
            ViewBag.autores = dt.getAutores();

            var libros = dt.getFilteredCapitulos(Nombre, autores, Lugar, Y1, Y2, grupos, Libro);
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

            var capitulo = dt.getCapituloById(id.GetValueOrDefault());
            return View(capitulo);
        }

        // GET: CapituloLibro/Create
        [Authorize]
        public ActionResult Create()
        {
                        
            ViewBag.grupo = dt.getGrupos();
            ViewBag.autores = dt.getAutores();
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
                ViewBag.grupo = dt.getGrupos();
                ViewBag.autores = dt.getAutores();
                return View(lib);
            }
            if (Autores.Count < 1)
            {
                ViewBag.grupo = dt.getGrupos();
                ViewBag.autores = dt.getAutores();
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
                }
                lib.Usuario = int.Parse(Session["id"].ToString());
                dt.createCapitulo(lib, file, GrupoAcademico, Autores);
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
            
            var a = dt.getCapituloById(id.GetValueOrDefault());
            if (int.Parse(Session["id"].ToString()) != a.Usuario && Session["tipo"].ToString().Equals("2"))
            {
                return RedirectToAction("Index");
            }
            a.capitulo_grupo = dt.getCapitulosGrupoById(id.GetValueOrDefault());
            ViewBag.autores = dt.getAutores();
            ViewBag.grupos = dt.getGrupos();            
            return View(a);
        }

        // POST: CapituloLibro/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, capitulolibro lib, List<string> GrupoAcademico, HttpPostedFileBase ffile, List<string> Autores)
        {
            
            if (!ModelState.IsValid)
            {
                ViewBag.grupo = dt.getGrupos();
                ViewBag.autores = dt.getAutores();
                return View(lib);
            }
            if (Autores==null)
            {
                ViewBag.grupo = dt.getGrupos();
                ViewBag.autores = dt.getAutores();
                ModelState.AddModelError("Nombre", "El campo autores no puede ir vacio");
                return View(lib);
            }
            try
            {
                archivo file=null;
                if (ffile != null && ffile.ContentLength > 0)
                {
                    string dir = "~/Content/Archivos/Capitulos";
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
                dt.editCapitulo(id, lib, GrupoAcademico, Autores, file);
                return RedirectToAction("Index", new { response = 1 });
            }
            catch(Exception e)
            {
                return Content(e+"");
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

                var libr = dt.getCapituloById(id.GetValueOrDefault());
                if (int.Parse(Session["id"].ToString()) != libr.Usuario)
                {
                    return RedirectToAction("Index");
                }
                dt.removeCapitulo(libr);
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
