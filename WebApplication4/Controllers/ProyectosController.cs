using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class ProyectosController : Controller
    {
        // GET: Proyectos
        [Authorize]
        public ActionResult Index(string response)
        {
            if (response != null)
            {
                ViewBag.response = int.Parse(response);
            }
            microna2018Entities db = new microna2018Entities();
            var proyectos = db.proyectos.ToList();
            return View(proyectos);
        }

        // GET: Proyectos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                var proyecto = db.proyectos.Where(x => x.idProyecto == id).FirstOrDefault();
                return View(proyecto);
            }
            catch
            {
                return RedirectToAction("Index");
            }            
        }

        // GET: Proyectos/Create
        [Authorize]
        public ActionResult Create()
        {
            microna2018Entities db = new microna2018Entities();            
            ViewBag.grupo = db.grupoacademico.ToList();
            return View();
        }

        // POST: Proyectos/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(proyectos pro, HttpPostedFileBase ffile, List<string> GrupoAcademico)
        {
            archivo file = null;
            try
            {
                string dir = "~/Content/Archivos/Proyectos";
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
                    pro.Archivo = file.idarchivo;
                }
                pro.Usuario = int.Parse(Request.Cookies["userInfo"]["id"]);
                db.proyectos.Add(pro);
                if (GrupoAcademico != null)
                {
                    foreach (var s in GrupoAcademico)
                    {
                        proyecto_grupo ag = new proyecto_grupo
                        {
                            id_proyecto = pro.idProyecto,
                            id_grupo = int.Parse(s)
                        };
                        db.proyecto_grupo.Add(ag);
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

        // GET: Proyectos/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                var a = db.proyectos.Where(x => x.idProyecto == id).FirstOrDefault();
                if (int.Parse(Request.Cookies["userInfo"]["id"]) != a.Usuario)
                {
                    return RedirectToAction("Index");
                }
                a.proyecto_grupo = db.proyecto_grupo.Where(x => x.id_proyecto == id).ToList();
                ViewBag.grupos = db.grupoacademico.ToList();
                return View(a);
            }
            catch
            {
                return RedirectToAction("Index");
            }            
        }

        // POST: Proyectos/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, proyectos pro, List<string> GrupoAcademico, HttpPostedFileBase ffile)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                var p = db.proyectos.Where(x => x.idProyecto == id).FirstOrDefault();
                p.nombre = pro.nombre;
                p.Responsables = pro.Responsables;
                p.FechaFinal = pro.FechaFinal;
                p.FechaInicio = pro.FechaInicio;
                p.Financiamiento = pro.Financiamiento;                
                var grupos_eliminar = db.proyecto_grupo.Where(x => x.id_proyecto == id).ToList();
                if (grupos_eliminar != null)
                {
                    foreach (var G in grupos_eliminar)
                    {
                        db.proyecto_grupo.Remove(G);
                    }
                }
                if (GrupoAcademico != null)
                {
                    foreach (var G in GrupoAcademico)
                    {
                        db.proyecto_grupo.Add(new proyecto_grupo { id_proyecto = id, id_grupo = int.Parse(G) });
                    }
                }
                if (ffile != null && ffile.ContentLength > 0)
                {
                    if (pro.archivo1 != null)
                    {
                        var archivo = new archivo();
                        archivo = db.archivo.Where(x => x.idarchivo == p.Archivo).FirstOrDefault();
                        System.IO.File.Delete(pro.archivo1.url);
                        db.archivo.Remove(archivo);
                    }
                    string dir = "~/Content/Archivos/Proyectos";
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
                    p.Archivo = file.idarchivo;
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { response = 1 });
            }
            catch
            {
                return RedirectToAction("Index", new { response = 2 });
            }
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                var proyecto = db.proyectos.Where(x => x.idProyecto == id).FirstOrDefault();
                if (int.Parse(Request.Cookies["UserInfo"]["Id"]) != proyecto.Usuario)
                {
                    return RedirectToAction("Index");
                }
                var a_g = db.proyecto_grupo.Where(x => x.id_proyecto == id).ToList();
                if (a_g != null)
                {
                    foreach (var a in a_g)
                    {
                        db.proyecto_grupo.Remove(a);
                    }
                }
                if (proyecto.archivo1 != null)
                {
                    var archivo = new archivo();
                    archivo = db.archivo.Where(x => x.idarchivo == proyecto.Archivo).FirstOrDefault();
                    System.IO.File.Delete(proyecto.archivo1.url);
                    db.archivo.Remove(archivo);
                }
                db.proyectos.Remove(proyecto);
                db.SaveChanges();
                return RedirectToAction("Index", new { response = 1 });
            }
            catch
            {
                return RedirectToAction("Index", new { response = 2 });
            }
        }
    }
}
