using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class ArticulosController : Controller
    {
        // GET: Articulos
        [Authorize]
        public ActionResult Index()
        {
            micronaEntities db = new micronaEntities();
            var articulos = db.articulo.ToList();
            return View(articulos);
        }

        // GET: Articulos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            micronaEntities db = new micronaEntities();
            var art=db.articulo.Where(x => x.idArticulo == id).FirstOrDefault();
            return View(art);
        }

        // GET: Articulos/Create
        public ActionResult Create()
        {           
            micronaEntities db = new micronaEntities();
            ViewBag.tipoarticulo = db.tipoarticulo.ToList();
            ViewBag.grupo = db.grupoacademico.ToList();
            return View();
        }

        // POST: Articulos/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(articulo a, HttpPostedFileBase ffile, List<string> GrupoAcademico)//, SelectList GrupoAcademico)
        {
            try
            {
                string dir = "~/Content/Archivos/ArtArbitrado";
                string fileName="";
                string path="";
                archivo file = null;
                micronaEntities db = new micronaEntities();
                if (!Directory.Exists(dir))
                {
                    DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(dir));
                }
                if (ffile != null && ffile.ContentLength > 0)
                {
                    fileName = Path.GetFileName(ffile.FileName);
                    path= Path.Combine(Server.MapPath(dir), DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + fileName);
                    ffile.SaveAs(path);
                    file = new archivo();
                    file.Nombre = fileName;
                    file.url = path;
                    db.archivo.Add(file);
                    db.SaveChanges();
                }
                
                if (file != null)
                {
                    a.Archivo = file.idarchivo;
                }                
                a.Usuario = int.Parse(Request.Cookies["userInfo"]["id"]);                                                                                            
                db.articulo.Add(a);                                    
                foreach(var s in GrupoAcademico)
                {
                    articulo_grupo ag = new articulo_grupo
                    {
                        id_articulo = a.idArticulo,
                        id_grupo = int.Parse(s)
                    };
                    db.articulo_grupo.Add(ag);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Create");
            }
        }

        // GET: Articulos/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            micronaEntities db = new micronaEntities();
            var a = db.articulo.Where(x => x.idArticulo == id).FirstOrDefault();
            return View(a);
        }

        // POST: Articulos/Edit/5
        [HttpPost]
        public ActionResult Edit(int id,articulo a)
        {
            try
            {
                micronaEntities db = new micronaEntities();
                var articulo = db.articulo.Where(x => x.idArticulo == id).FirstOrDefault();
                articulo.Nombre = a.Nombre;
                articulo.Autores = a.Autores;
                articulo.Fecha = a.Fecha;                
                articulo.ISSN = a.ISSN;
                articulo.PagFinal = a.PagFinal;
                articulo.PagInicio = a.PagInicio;
                articulo.Revista = a.Revista;
                articulo.Volumen = a.Volumen;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }       

        // POST: Articulos/Delete/5
        //[HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                micronaEntities db = new micronaEntities();
                var articulo = db.articulo.Where(x => x.idArticulo == id).FirstOrDefault();
                db.articulo.Remove(articulo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public FileResult Download(string Url, string name)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@Url);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }
    }
}
