using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class TrabajosController : Controller
    {
        // GET: Trabajos
  
        [Authorize]
        public ActionResult Index(string Nombre, string Autores, string Lugar, int? Y1, int? Y2, string response, List<string> grupos)
        {
            if (response != null)
            {
                ViewBag.response = int.Parse(response);
            }
            microna2018Entities db = new microna2018Entities();
            ViewBag.grupos = db.grupoacademico.ToList();
            List<trabajo> trabajos = db.trabajo.ToList();
            if (Nombre != null)
            {
                trabajos = trabajos.Where(x => x.Nombre.Contains(Nombre)).ToList();
            }
            if (Autores != null)
            {
                trabajos = trabajos.Where(x => x.Autores.Contains(Autores)).ToList();
            }
            if (Lugar != null)
            {
                trabajos = trabajos.Where(x => x.Presentacion.Contains(Lugar)).ToList();
            }
            if (Y1 != null)
            {
                //int year1 = int.Parse(Y1);
                trabajos = trabajos.Where(x => x.Año >= Y1).ToList();
            }
            if (Y2 != null)
            {
                trabajos = trabajos.Where(x => x.Año <= Y2).ToList();
            }
            if (grupos != null)
            {
                foreach (string s in grupos)
                {
                    int i = int.Parse(s);
                    var g = db.trabajo_grupo.Where(x => x.id_grupo == i).ToList();
                    List<trabajo> cg = new List<trabajo>();
                    foreach (var cap in g)
                    {
                        trabajo sample = db.trabajo.Where(x => x.idTrabajo == cap.id_trabajo).FirstOrDefault();
                        cg.Add(sample);
                    }
                    trabajos = trabajos.Where(x => cg.Contains(x)).ToList();
                }
            }
            return View(trabajos);
        }

        // GET: Trabajos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                var trabajo = db.trabajo.Where(x => x.idTrabajo == id).FirstOrDefault();
                return View(trabajo);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Trabajos/Create
        [Authorize]
        public ActionResult Create()
        {
            microna2018Entities db = new microna2018Entities();
            ViewBag.tipo = db.tipotrabajo.ToList();
            ViewBag.grupo = db.grupoacademico.ToList();
            return View();
        }

        // POST: Trabajos/Create
        [HttpPost]
        public ActionResult Create(trabajo t, HttpPostedFileBase ffile, List<string> GrupoAcademico)
        {
            archivo file = null;
            try
            {
                string dir = "~/Content/Archivos/Trabajo";
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
                    t.Archivo = file.idarchivo;
                }
                t.Usuario = int.Parse(Request.Cookies["userInfo"]["id"]);
                db.trabajo.Add(t);                
                if (GrupoAcademico != null)
                {
                    foreach (var s in GrupoAcademico)
                    {
                        trabajo_grupo ag = new trabajo_grupo
                        {
                            id_trabajo = t.idTrabajo,
                            id_grupo = int.Parse(s)
                        };
                        db.trabajo_grupo.Add(ag);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { result = 1 });
            }
            catch(Exception e)
            {
                return RedirectToAction("Index", new { result = 2 });
            }
        }

        // GET: Trabajos/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            microna2018Entities db = new microna2018Entities();
            var a = db.trabajo.Where(x => x.idTrabajo == id).FirstOrDefault();
            if (int.Parse(Request.Cookies["userInfo"]["id"]) != a.Usuario)
            {
                return RedirectToAction("Index");
            }
            a.trabajo_grupo = db.trabajo_grupo.Where(x => x.id_trabajo == id).ToList();
            ViewBag.grupos = db.grupoacademico.ToList();
            ViewBag.tipo = db.tipotrabajo.ToList();
            return View(a);
        }

        // POST: Trabajos/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, trabajo t, List<string> GrupoAcademico, HttpPostedFileBase ffile)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                var trabajo = db.trabajo.Where(x => x.idTrabajo == id).FirstOrDefault();
                trabajo.Nombre = t.Nombre;
                trabajo.Pais = t.Pais;
                trabajo.Presentacion = t.Presentacion;
                trabajo.TipoTrabajo = t.TipoTrabajo;                
                var grupos_eliminar = db.trabajo_grupo.Where(x => x.id_trabajo == id).ToList();
                if (grupos_eliminar != null)
                {
                    foreach (var G in grupos_eliminar)
                    {
                        db.trabajo_grupo.Remove(G);
                    }
                }
                if (GrupoAcademico != null)
                {
                    foreach (var G in GrupoAcademico)
                    {
                        db.trabajo_grupo.Add(new trabajo_grupo { id_trabajo = id, id_grupo = int.Parse(G) });
                    }
                }
                if (ffile != null && ffile.ContentLength > 0)
                {
                    if (trabajo.archivo1 != null)
                    {
                        var archivo = new archivo();
                        archivo = db.archivo.Where(x => x.idarchivo == trabajo.Archivo).FirstOrDefault();
                        System.IO.File.Delete(trabajo.archivo1.url);
                        db.archivo.Remove(archivo);
                    }
                    string dir = "~/Content/Archivos/Trabajos";
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
                    trabajo.Archivo = file.idarchivo;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return Content("" + e);
            }
        }
        

        // POST: Trabajos/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                var trabajo = db.trabajo.Where(x => x.idTrabajo == id).FirstOrDefault();
                if (int.Parse(Request.Cookies["UserInfo"]["Id"]) != trabajo.Usuario)
                {
                    return RedirectToAction("Index");
                }
                var a_g = db.trabajo_grupo.Where(x => x.id_trabajo == id).ToList();
                if (a_g != null)
                {
                    foreach (var a in a_g)
                    {
                        db.trabajo_grupo.Remove(a);
                    }                    
                }
                if (trabajo.archivo1 != null)
                {
                    var archivo = new archivo();
                    archivo = db.archivo.Where(x => x.idarchivo == trabajo.Archivo).FirstOrDefault();                    
                    System.IO.File.Delete(trabajo.archivo1.url);
                    db.archivo.Remove(archivo);
                }                
                db.trabajo.Remove(trabajo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {                
                return RedirectToAction("Index");
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
