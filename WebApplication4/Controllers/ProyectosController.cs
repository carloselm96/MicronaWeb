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
        microna2018Entities db = new microna2018Entities();
        // GET: Proyectos
        [Authorize]
        public ActionResult Index(string response)
        {
            if (response != null)
            {
                ViewBag.response = int.Parse(response);
            }
            
            ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
            ViewBag.grupos = db.grupoacademico.ToList();            
            List<proyectos> proyect = db.proyectos.ToList();            
            return View(proyect);
        }

        
        [Authorize]
        [HttpGet]
        public ActionResult Search(string Nombre, List<string> autores, string Lugar, DateTime? Y1, DateTime? Y2, List<string> grupos)
        {            
            
            ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
            ViewBag.grupos = db.grupoacademico.ToList();
            List<proyectos> proyect = db.proyectos.ToList();
            if (Nombre != null)
            {
                proyect = proyect.Where(x => x.nombre.Contains(Nombre)).ToList();
            }
            if (autores != null)
            {
                List<proyectos> cg = new List<proyectos>();
                foreach (string s in autores)
                {
                    int i = int.Parse(s);
                    var g = db.proyecto_usuario.Where(x => x.idusuario == i).ToList();
                    
                    foreach (var cap in g)
                    {
                        proyectos sample = db.proyectos.Where(x => x.idProyecto == cap.idproyecto).FirstOrDefault();
                        cg.Add(sample);
                    }
                    
                }
                proyect = proyect.Where(x => cg.Contains(x)).ToList();
            }
            if (Y1 != null)
            {
                //int year1 = int.Parse(Y1);
                proyect = proyect.Where(x => x.FechaInicio >= Y1).ToList();
            }
            if (Y2 != null)
            {
                proyect = proyect.Where(x => x.FechaFinal <= Y2).ToList();
            }
            if (grupos != null)
            {
                foreach (string s in grupos)
                {
                    int i = int.Parse(s);
                    var g = db.proyecto_grupo.Where(x => x.id_grupo == i).ToList();
                    List<proyectos> cg = new List<proyectos>();
                    foreach (var cap in g)
                    {
                        proyectos sample = db.proyectos.Where(x => x.idProyecto == cap.id_proyecto).FirstOrDefault();
                        cg.Add(sample);
                    }
                    proyect = proyect.Where(x => cg.Contains(x)).ToList();
                }
            }
            return PartialView("_ProyectosPartial",proyect);
        }
        // GET: Proyectos/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "Proyectos", null);
                }
                
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
                        
            ViewBag.grupo = db.grupoacademico.ToList();
            ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
            return View();
        }

        // POST: Proyectos/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(proyectos pro, HttpPostedFileBase ffile, List<string> GrupoAcademico, List<string> Autores)
        {
            archivo file = null;
            try
            {
                string dir = "~/Content/Archivos/Proyectos";
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
                    pro.Archivo = file.idarchivo;
                }
                pro.Usuario = int.Parse(Session["id"].ToString());
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
                if (Autores != null)
                {
                    foreach (var s in Autores)
                    {
                        proyecto_usuario lb = new proyecto_usuario
                        {
                            idproyecto = pro.idProyecto,
                            idusuario = int.Parse(s)
                        };
                        db.proyecto_usuario.Add(lb);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { response = 1 });
            }
            catch (Exception e)
            {
                return Content("" + e);
                //return RedirectToAction("Index", new { response = 2 });
            }
        }

        // GET: Proyectos/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "Proyectos", null);
                }
                
                var a = db.proyectos.Where(x => x.idProyecto == id).FirstOrDefault();
                if (int.Parse(Session["id"].ToString()) != a.Usuario && Session["tipo"].ToString().Equals("2"))
                {
                    return RedirectToAction("Index");
                }
                a.proyecto_grupo = db.proyecto_grupo.Where(x => x.id_proyecto == id).ToList();
                ViewBag.grupos = db.grupoacademico.ToList();
                ViewBag.autores = db.usuario.Where(x=> x.Status.Equals("A")).ToList();
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
        public ActionResult Edit(int id, proyectos pro, List<string> GrupoAcademico, HttpPostedFileBase ffile, List<string> Autores)
        {
            try
            {
                
                var p = db.proyectos.Where(x => x.idProyecto == id).FirstOrDefault();
                p.nombre = pro.nombre;                
                p.FechaFinal = pro.FechaFinal;
                p.FechaInicio = pro.FechaInicio;
                p.Financiamiento = pro.Financiamiento;
                var autores_eliminar = db.proyecto_usuario.Where(x => x.idproyecto == id).ToList();
                if (autores_eliminar != null)
                {
                    foreach (var G in autores_eliminar)
                    {
                        db.proyecto_usuario.Remove(G);
                    }
                }
                if (Autores != null)
                {
                    foreach (var G in Autores)
                    {
                        db.proyecto_usuario.Add(new proyecto_usuario { idproyecto = id, idusuario = int.Parse(G) });
                    }
                }
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
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "Proyectos", null);
                }
                
                var proyecto = db.proyectos.Where(x => x.idProyecto == id).FirstOrDefault();
                if (int.Parse(Session["id"].ToString()) != proyecto.Usuario)
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

        [Authorize]
        public FileResult Download(string Url, string name)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@Url);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }
    }
}
