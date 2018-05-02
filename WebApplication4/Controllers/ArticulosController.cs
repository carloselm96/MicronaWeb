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
        public ActionResult Index()
        {
            micronaEntities db = new micronaEntities();
            var articulos = db.articulo.ToList();
            return View(articulos);
        }

        // GET: Articulos/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
        public ActionResult Create(articulo a)
        {
            try
            {
                /*string dir = "~/Content/Archivos/ArtArbitrado";
                if (!Directory.Exists(dir))
                {
                    DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(dir));
                }
                if (archivo != null && archivo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(archivo.FileName);
                    var path = Path.Combine(Server.MapPath(dir), DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + fileName);
                    archivo.SaveAs(path);
                }   */
                micronaEntities db = new micronaEntities();
                    a.Usuario = int.Parse(Request.Cookies["userInfo"]["id"]);
                    db.articulo.Add(a);
                    db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Articulos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
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
                articulo.GrupoAcademico = a.GrupoAcademico;
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
        [HttpPost]
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
    }
}
