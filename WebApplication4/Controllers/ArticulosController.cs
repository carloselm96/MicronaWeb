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
            microna2018Entities db = new microna2018Entities();
            var articulos = db.articulo.ToList();
            return View(articulos);
        }

        // GET: Articulos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            microna2018Entities db = new microna2018Entities();
            var art=db.articulo.Where(x => x.idArticulo == id).FirstOrDefault();
            return View(art);
        }

        // GET: Articulos/Create
        public ActionResult Create()
        {           
            microna2018Entities db = new microna2018Entities();
            ViewBag.tipoarticulo = db.tipoarticulo.ToList();
            ViewBag.grupo = db.grupoacademico.ToList();
            return View();
        }

        // POST: Articulos/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(articulo a, HttpPostedFileBase ffile, List<string> GrupoAcademico)
        {
            archivo file = null;
            try
            {
                string dir = "~/Content/Archivos/ArtArbitrado";
                string fileName="";
                string path="";                
                microna2018Entities db = new microna2018Entities();
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
                if (GrupoAcademico != null)
                {
                    foreach (var s in GrupoAcademico)
                    {
                        articulo_grupo ag = new articulo_grupo
                        {
                            id_articulo = a.idArticulo,
                            id_grupo = int.Parse(s)
                        };
                        db.articulo_grupo.Add(ag);
                    }
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
            microna2018Entities db = new microna2018Entities();
            var a = db.articulo.Where(x => x.idArticulo == id).FirstOrDefault();
            if(int.Parse(Request.Cookies["userInfo"]["id"]) != a.Usuario)
            {
                return RedirectToAction("Index");
            }
            a.articulo_grupo = db.articulo_grupo.Where(x => x.id_articulo == id).ToList();
            ViewBag.grupos = db.grupoacademico.ToList();
            ViewBag.tipoarticulo = db.tipoarticulo.ToList();
            return View(a);
        }

        // POST: Articulos/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id,articulo a, List<string> GrupoAcademico)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                var articulo = db.articulo.Where(x => x.idArticulo == id).FirstOrDefault();
                articulo.Nombre = a.Nombre;
                articulo.Autores = a.Autores;
                articulo.Fecha = a.Fecha;                
                articulo.ISSN = a.ISSN;
                articulo.PagFinal = a.PagFinal;
                articulo.PagInicio = a.PagInicio;
                articulo.Revista = a.Revista;
                articulo.Volumen = a.Volumen;
                articulo.TipoArticulo = a.TipoArticulo;                
                var grupos_eliminar= db.articulo_grupo.Where(x=> x.id_articulo==id).ToList();
                if (grupos_eliminar != null)
                {
                    foreach (var G in grupos_eliminar)
                    {
                        db.articulo_grupo.Remove(G);
                    }
                }
                if (GrupoAcademico != null)
                {
                    foreach(var G in GrupoAcademico)
                    {
                        db.articulo_grupo.Add(new articulo_grupo { id_articulo = id, id_grupo = int.Parse(G) });
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Articulos/Delete/5
        //[HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                var articulo = db.articulo.Where(x => x.idArticulo == id).FirstOrDefault();
                if (int.Parse(Request.Cookies["UserInfo"]["Id"]) != articulo.Usuario)
                {
                    return RedirectToAction("Index");
                }
                var a_g = db.articulo_grupo.Where(x => x.id_articulo == id).ToList();
                if (a_g != null)
                {
                    foreach (var a in a_g)
                    {
                        db.articulo_grupo.Remove(a);
                    }
                }                
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
