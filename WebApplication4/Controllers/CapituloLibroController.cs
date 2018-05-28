﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class CapituloLibroController : Controller
    {
        // GET: CapituloLibro
        [Authorize]
        public ActionResult Index(string response)
        {
            if (response != null)
            {
                ViewBag.response = int.Parse(response);
            }
            microna2018Entities db = new microna2018Entities();
            List<capitulolibro> libros = db.capitulolibro.ToList();
            return View(libros);
        }

        // GET: CapituloLibro/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            microna2018Entities db = new microna2018Entities();
            var capitulo = db.capitulolibro.Where(x => x.idCapituloLibro == id).FirstOrDefault();
            return View(capitulo);
        }

        // GET: CapituloLibro/Create
        [Authorize]
        public ActionResult Create()
        {
            microna2018Entities db = new microna2018Entities();            
            ViewBag.grupo = db.grupoacademico.ToList();
            return View();
        }

        // POST: CapituloLibro/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(capitulolibro lib, HttpPostedFileBase ffile, List<string> GrupoAcademico)
        {
            archivo file = null;
            try
            {
                string dir = "~/Content/Archivos/Capitulos";
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
                    lib.Archivo = file.idarchivo;
                }
                lib.Usuario = int.Parse(Request.Cookies["userInfo"]["id"]);
                db.capitulolibro.Add(lib);
                if (GrupoAcademico != null)
                {
                    foreach (var s in GrupoAcademico)
                    {
                        capitulo_grupo ag = new capitulo_grupo
                        {
                            id_capitulo = lib.idCapituloLibro,
                            id_grupo = int.Parse(s)
                        };
                        db.capitulo_grupo.Add(ag);
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

        // GET: CapituloLibro/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            microna2018Entities db = new microna2018Entities();
            var a = db.capitulolibro.Where(x => x.idCapituloLibro == id).FirstOrDefault();
            if (int.Parse(Request.Cookies["userInfo"]["id"]) != a.Usuario)
            {
                return RedirectToAction("Index");
            }
            a.capitulo_grupo = db.capitulo_grupo.Where(x => x.id_capitulo == id).ToList();
            ViewBag.grupos = db.grupoacademico.ToList();            
            return View(a);
        }

        // POST: CapituloLibro/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, capitulolibro lib, List<string> GrupoAcademico, HttpPostedFileBase ffile)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                var l = db.capitulolibro.Where(x => x.idCapituloLibro == id).FirstOrDefault();
                l.Nombre = lib.Nombre;
                l.ISBN = lib.ISBN;                
                l.Autores = lib.Autores;
                l.Participantes = lib.Participantes;
                l.Año = lib.Año;
                var grupos_eliminar = db.capitulo_grupo.Where(x => x.id_capitulo == id).ToList();
                if (grupos_eliminar != null)
                {
                    foreach (var G in grupos_eliminar)
                    {
                        db.capitulo_grupo.Remove(G);
                    }
                }
                if (GrupoAcademico != null)
                {
                    foreach (var G in GrupoAcademico)
                    {
                        db.capitulo_grupo.Add(new capitulo_grupo { id_capitulo = id, id_grupo = int.Parse(G) });
                    }
                }
                if (ffile != null && ffile.ContentLength > 0)
                {
                    if (l.archivo1 != null)
                    {
                        var archivo = new archivo();
                        archivo = db.archivo.Where(x => x.idarchivo == l.Archivo).FirstOrDefault();
                        System.IO.File.Delete(l.archivo1.url);
                        db.archivo.Remove(archivo);
                    }
                    string dir = "~/Content/Archivos/Capitulos";
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
                    l.Archivo = file.idarchivo;
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { response = 1 });
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                microna2018Entities db = new microna2018Entities();
                var libr = db.capitulolibro.Where(x => x.idCapituloLibro == id).FirstOrDefault();
                if (int.Parse(Request.Cookies["UserInfo"]["Id"]) != libr.Usuario)
                {
                    return RedirectToAction("Index");
                }
                var a_g = db.capitulo_grupo.Where(x => x.id_capitulo == id).ToList();
                if (a_g != null)
                {
                    foreach (var a in a_g)
                    {
                        db.capitulo_grupo.Remove(a);
                    }
                }
                db.capitulolibro.Remove(libr);
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
