using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        microna2018Entities db = new microna2018Entities();
        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            var concent = db.concentrado.ToList();
            ViewBag.autores = db.usuario.Where(x => x.Status.Equals("A")).ToList();
            return View(concent);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Search(string titulo, DateTime? Y1, DateTime? Y2, List<string> autores, int?[] checkgroup, int?[] checktype)
        {
            var concent = db.concentrado.ToList();
            ViewBag.autores = db.usuario.Where(x => x.Status.Equals("A")).ToList();
            if (titulo != null)
            {
                concent = concent.Where(x => x.Titulo.Contains(titulo)).ToList();
            }
            if (Y1 != null)
            {
                concent = concent.Where(x => x.Fecha >= Y1).ToList();
            }
            if (Y2 != null)
            {
                concent = concent.Where(x => x.Fecha <= Y2).ToList();
            }
            if (checkgroup != null)
            {
                List<concentrado> cg = new List<concentrado>();
                foreach (int i in checkgroup)
                {
                    var g = db.concentrado_grupos.Where(x => x.Grupo == i).ToList();
                    foreach (var con in g)
                    {
                        concentrado sample = db.concentrado.Where(x => x.idConcentrado == con.Concentrado).FirstOrDefault();
                        cg.Add(sample);
                    }
                }
                concent = concent.Where(x => cg.Contains(x)).ToList();
            }
            if (checktype != null)
            {
                List<concentrado> ct = new List<concentrado>();
                foreach (int i in checktype)
                {
                    var g = concent.Where(x => x.TipoConcentrado == i).ToList();
                    foreach (var con in g)
                    {
                        ct.Add(con);
                    }
                }
                concent = ct;
            }
            if (autores != null)
            {
                List<concentrado> cg = new List<concentrado>();
                foreach (string s in autores)
                {
                    int i = int.Parse(s);
                    var g = db.concentrado_autores.Where(x => x.idAutor == i).ToList();
                    foreach (var cap in g)
                    {
                        concentrado sample = db.concentrado.Where(x => x.idConcentrado == cap.idConcentrado).FirstOrDefault();
                        cg.Add(sample);
                    }
                }
                concent = concent.Where(x => cg.Contains(x)).ToList();
            }
            return PartialView("_ConcentradoPartial", concent);
        }
    }
}