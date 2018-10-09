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
        microna2018Entities db = new microna2018Entities();
        // GET: Articulos
        [Authorize]
        public ActionResult Index(string response)
        {            
            if (response != null)
            {
                ViewBag.response = int.Parse(response);
            }
            
            ViewBag.grupos = db.grupoacademico.ToList();
            ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
            ViewBag.tipo = db.tipoarticulo.ToList();
            var articulos = db.articulo.ToList();      
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

            ViewBag.grupos = db.grupoacademico.ToList();
            ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
            ViewBag.tipo = db.tipoarticulo.ToList();
            var articulos = db.articulo.ToList();
            if (Nombre != null)
            {
                articulos = articulos.Where(x => x.Nombre.Contains(Nombre)).ToList();
            }
            if (Y1 != null)
            {
                //int year1 = int.Parse(Y1);
                articulos = articulos.Where(x => x.Fecha >= Y1).ToList();
            }
            if (Y2 != null)
            {
                articulos = articulos.Where(x => x.Fecha <= Y2).ToList();
            }
            if (Revista != null)
            {
                articulos = articulos.Where(x => x.Revista.Contains(Revista)).ToList();
            }
            if (grupos != null)
            {
                List<articulo> cg = new List<articulo>();
                foreach (string s in grupos)
                {
                    int i = int.Parse(s);
                    var g = db.articulo_grupo.Where(x => x.id_grupo == i).ToList();
                    
                    foreach (var cap in g)
                    {
                        articulo sample = db.articulo.Where(x => x.idArticulo == cap.id_articulo).FirstOrDefault();
                        cg.Add(sample);
                    }                    
                }
                articulos = articulos.Where(x => cg.Contains(x)).ToList();
            }
            if (tipo != null)
            {
                articulos = articulos.Where(x => x.TipoArticulo == int.Parse(tipo)).ToList();
            }
            if (autores != null)
            {
                foreach (string s in autores)
                {
                    int i = int.Parse(s);
                    var g = db.articulo_usuario.Where(x => x.idUsuario == i).ToList();
                    List<articulo> cg = new List<articulo>();
                    foreach (var cap in g)
                    {
                        articulo sample = db.articulo.Where(x => x.idArticulo == cap.idArticulo).FirstOrDefault();
                        cg.Add(sample);
                    }
                    articulos = articulos.Where(x => cg.Contains(x)).ToList();
                }
            }
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
            
            var art=db.articulo.Where(x => x.idArticulo == id).FirstOrDefault();
            return View(art);
        }

        // GET: Articulos/Create
        public ActionResult Create()
        {           
            
            ViewBag.tipoarticulo = db.tipoarticulo.ToList();
            ViewBag.grupo = db.grupoacademico.ToList();
            ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();            
            return View();
        }

        // POST: Articulos/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(articulo a, HttpPostedFileBase ffile, List<string> GrupoAcademico, List<string> Autores)
        {
            if (Autores==null)
            {
                ViewBag.tipoarticulo = db.tipoarticulo.ToList();
                ViewBag.grupo = db.grupoacademico.ToList();
                ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
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
                    if (Autores != null)
                    {
                        foreach (var s in Autores)
                        {
                            articulo_usuario lb = new articulo_usuario
                            {
                                idArticulo = a.idArticulo,
                                idUsuario = int.Parse(s)
                            };
                            db.articulo_usuario.Add(lb);
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
            ViewBag.tipoarticulo = db.tipoarticulo.ToList();
            ViewBag.grupo = db.grupoacademico.ToList();
            ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
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
            
            var a = db.articulo.Where(x => x.idArticulo == id).FirstOrDefault();
            if(int.Parse(Request.Cookies["userInfo"]["id"]) != a.Usuario)
            {
                return RedirectToAction("Index");
            }
            a.articulo_grupo = db.articulo_grupo.Where(x => x.id_articulo == id).ToList();
            ViewBag.grupos = db.grupoacademico.ToList();
            ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
            ViewBag.tipoarticulo = db.tipoarticulo.ToList();
            return View(a);
        }

        // POST: Articulos/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id,articulo a, List<string> GrupoAcademico, HttpPostedFileBase ffile, List<string> Autores)
        {            
            try
            {
                
                var articulo = db.articulo.Where(x => x.idArticulo == id).FirstOrDefault();
                articulo.Nombre = a.Nombre;                
                articulo.Fecha = a.Fecha;                
                articulo.ISSN = a.ISSN;
                articulo.PagFinal = a.PagFinal;
                articulo.PagInicio = a.PagInicio;
                articulo.Revista = a.Revista;
                articulo.Volumen = a.Volumen;
                articulo.TipoArticulo = a.TipoArticulo;
                var autores_eliminar = db.articulo_usuario.Where(x => x.idArticulo == id).ToList();
                if (autores_eliminar != null)
                {
                    foreach (var G in autores_eliminar)
                    {
                        db.articulo_usuario.Remove(G);
                    }
                }
                if (Autores != null)
                {
                    foreach (var G in Autores)
                    {
                        db.articulo_usuario.Add(new articulo_usuario { idArticulo = id, idUsuario = int.Parse(G) });
                    }
                }
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
                if (ffile != null && ffile.ContentLength > 0)
                {
                    if (articulo.archivo1 != null)
                    {
                        var archivo = new archivo();
                        archivo = db.archivo.Where(x => x.idarchivo == articulo.Archivo).FirstOrDefault();
                        System.IO.File.Delete(articulo.archivo1.url);
                        db.archivo.Remove(archivo);
                    }
                    string dir = "~/Content/Archivos/Articulos";
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
                    articulo.Archivo = file.idarchivo;
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { response = 1 });
            }
            catch
            {
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

        

        [Authorize]
        [HttpPost]
        public ActionResult Configure(List<string> tipos_eliminar, List<string> tipos_añadir)
        {
            
            List<tipoarticulo> tipos = db.tipoarticulo.ToList();

            if (tipos_eliminar != null)
            {
                foreach(var ide in tipos_eliminar)
                {
                    tipoarticulo aux = null;
                    aux=tipos.Where(x => x.idTipoArticulo == int.Parse(ide)).FirstOrDefault();
                    if (aux != null)
                    {
                        tipos.Remove(aux);
                    }
                }
            }
            if (tipos_añadir != null)
            {
                foreach(var a in tipos_añadir)
                {
                    tipos.Add(new tipoarticulo { Nombre = a });
                }
            }
            db.SaveChanges();
            return View();
        }
    }
}
