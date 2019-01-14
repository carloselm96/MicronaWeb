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
    public class ArticulosController : Controller
    {
        
        DataAccess dt = new DataAccess();
        // GET: Articulos
        [Authorize]
        public ActionResult Index(string response)
        {            
            if (response != null)
            {
                ViewBag.response = int.Parse(response);
            }
            
            ViewBag.grupos = dt.getGrupos();
            ViewBag.autores = dt.getAutores();
            ViewBag.tipo = dt.getTiposArticulo();
            var articulos = dt.getAllArticulos();      
            return View(articulos);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Search(string response, string Nombre, List<string> autores, string Revista, DateTime? Y1, DateTime? Y2, List<string> grupos, string tipo)
        {
            if (response != null)
            {
                ViewBag.response = int.Parse(response);
            }
            var articulos = dt.getArticulos(Nombre, autores, Revista, Y1, Y2, grupos, tipo);
            ViewBag.grupos = dt.getGrupos();
            ViewBag.autores = dt.getAutores();
            ViewBag.tipo = dt.getTiposArticulo();
            
            return PartialView("_ArticuloPArtial", articulos);
        }
        // GET: Articulos/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Articulos", null);
            }
            
            var art=dt.getArticuloById(id);
            if (art == null)
            {
                return RedirectToAction("Index", "Articulos", null);
            }
            return View(art);
        }

        // GET: Articulos/Create
        public ActionResult Create()
        {           
            
            ViewBag.tipoarticulo = dt.getTiposArticulo();
            ViewBag.grupo = dt.getGrupos();
            ViewBag.autores = dt.getAutores();            
            return View();
        }

        // POST: Articulos/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(articulo a, HttpPostedFileBase ffile, List<string> GrupoAcademico, List<string> Autores)
        {

            if (Autores==null)
            {
                ViewBag.tipoarticulo = dt.getTiposArticulo();
                ViewBag.grupo = dt.getGrupos();
                ViewBag.autores = dt.getAutores();
                ModelState.AddModelError("Nombre", "El campo autores no puede ir vacio");
                return View(a);
            }
            a.Usuario = int.Parse(Request.Cookies["userInfo"]["id"]);
            if (ModelState.IsValid)
            {
                archivo file = null;
                try
                {
                    string dir = "~/Content/Archivos/Articulos";
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
                    a.Usuario = int.Parse(Request.Cookies["userInfo"]["id"]);

                    if (dt.createArticulo(a, file, GrupoAcademico, Autores))
                    {
                        return RedirectToAction("Index", new { response = 1 });
                    }
                    else
                    {
                        return RedirectToAction("Index", new { response = 1 });
                    }
                }
                catch
                {
                    return RedirectToAction("Index", new { response = 2 });
                }
            }
            ViewBag.tipoarticulo = dt.getTiposArticulo();
            ViewBag.grupo = dt.getGrupos();
            ViewBag.autores = dt.getAutores();
            return View(a);
        }

        // GET: Articulos/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Articulos", null);
            }
            
            var a = dt.getArticuloById(id);
            if(int.Parse(Request.Cookies["userInfo"]["id"]) != a.Usuario)
            {
                return RedirectToAction("Index");
            }
            //a.articulo_grupo = db.articulo_grupo.Where(x => x.id_articulo == id).ToList();
            ViewBag.grupos = dt.getGrupos();
            ViewBag.autores = dt.getAutores();
            ViewBag.tipoarticulo = dt.getTiposArticulo();
            return View(a);
        }

        // POST: Articulos/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id,articulo a, List<string> GrupoAcademico, HttpPostedFileBase ffile, List<string> Autores)
        {
            bool fileIsSaved = false;
            try
            {                
                if (ffile != null)
                {
                    string dir = "~/Content/Archivos/Articulos";
                    if (!Directory.Exists(dir))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(dir));
                    }
                    string fileName = Path.GetFileName(ffile.FileName);
                    string path = Path.Combine(Server.MapPath(dir), DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + fileName);
                    ffile.SaveAs(path);
                    fileIsSaved = true;
                    archivo file = new archivo();
                    file.Nombre = fileName;
                    file.url = path;
                    dt.editArticulo(id, a, GrupoAcademico, file, Autores);
                }
                else
                {
                    dt.editArticulo(id, a, GrupoAcademico, null, Autores);
                }
                return RedirectToAction("Index", new { response = 1 });
            }
            catch(Exception e)
            {
                if (fileIsSaved)
                {
                    try
                    {
                        string dir = "~/Content/Archivos/Articulos";
                        string fileName = Path.GetFileName(ffile.FileName);
                        string path = Path.Combine(Server.MapPath(dir), DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + fileName);
                        System.IO.File.Delete(path);
                    }
                    catch
                    {
                        return RedirectToAction("Index", new { response = 2 });
                    }
                }                
                return RedirectToAction("Index", new { response = 2 });
            }
        }

        // POST: Articulos/Delete/5
        //[HttpPost]
        [Authorize]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "Articulos", null);
                }
                
                var articulo = dt.getArticuloById(id);
                if (int.Parse(Request.Cookies["UserInfo"]["Id"]) != articulo.Usuario)
                {
                    return RedirectToAction("Index");
                }
                dt.removeArticulo(articulo);
                return RedirectToAction("Index", new {response=1 });
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
