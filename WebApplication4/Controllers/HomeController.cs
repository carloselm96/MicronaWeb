using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;
using WebApplication4.Models.DataAccess;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        microna2018Entities db = new microna2018Entities();
        DataAccess dt = new DataAccess();
        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            //var concent = db.concentrado.ToList();
            ViewBag.autores = dt.getAutores();
            ViewBag.grupos = dt.getGrupos();
            ViewBag.objectsArray = dt.getConcentradoData();
            return View();
        }

        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var concentrado = dt.getConcentradoById(id);
            return View(concentrado);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Search(string titulo, DateTime? Y1, DateTime? Y2, List<string> autores, int?[] checkgroup, int?[] checktype)
        {
            var concent = dt.getConcentrado(titulo, Y1, Y2, autores, checkgroup, checktype);
            ViewBag.autores = dt.getAutores();
           
            return PartialView("_ConcentradoPartial", concent);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult SearchResult(string titulo, DateTime? Y1, DateTime? Y2, List<string> autores, int?[] checkgroup, int?[] checktype)
        {
            var concent = dt.getConcentrado(titulo,Y1,Y2,autores,checkgroup,checktype);
            ViewBag.grupos = dt.getGrupos();
            ViewBag.autores = dt.getAutores();
            ViewBag.autoresRel = dt.getRelatedUsers(concent.Select(x => x.idConcentrado).ToList());
            ViewBag.gruposRel = dt.getRelatedGrupos(concent.Select(x => x.idConcentrado).ToList());
            return PartialView(concent);
        }

        [AllowAnonymous]        
        public ActionResult HomeSearch(string titulo, DateTime? Y1, DateTime? Y2, List<string> autores, int?[] checkgroup, int?[] checktype)
        {
            List<concentrado> concent = dt.getFilteredConcentrado(titulo, Y1, Y2, autores, checkgroup, checktype);            
            ViewBag.autores = dt.getAutores();
            ViewBag.grupos = dt.getGrupos();
            ViewBag.autoresRel = dt.getRelatedUsers(concent.Select(x => x.idConcentrado).ToList());
            ViewBag.gruposRel = dt.getRelatedGrupos(concent.Select(x => x.idConcentrado).ToList());

            return View("SearchResult",concent);
        }

        [AllowAnonymous]
        public ActionResult HomeSearchType(string type)
        {
            var concent = db.concentrado.ToList();
            switch (type)
            {
                case "articulo":
                    concent=concent.Where(x => x.tipoconcentrado1.idtipoconcentrado == 1).ToList();
                    break;
                case "capitulo":
                    concent=concent.Where(x => x.tipoconcentrado1.idtipoconcentrado == 2).ToList();
                    break;
                case "libro":
                    concent=concent.Where(x => x.tipoconcentrado1.idtipoconcentrado == 3).ToList();
                    break;
                case "proyecto":
                    concent=concent.Where(x => x.tipoconcentrado1.idtipoconcentrado == 4).ToList();
                    break;
                case "tesis":
                    concent=concent.Where(x => x.tipoconcentrado1.idtipoconcentrado == 6).ToList();
                    break;
                case "trabajo":
                    concent=concent.Where(x => x.tipoconcentrado1.idtipoconcentrado == 5).ToList();
                    break;
                default:
                    return RedirectToAction("Index");
            }
            ViewBag.autoresRel = dt.getRelatedUsers(concent.Select(x => x.idConcentrado).ToList());
            ViewBag.gruposRel = dt.getRelatedGrupos(concent.Select(x => x.idConcentrado).ToList());
            ViewBag.autores = dt.getAutores();
            ViewBag.grupos = dt.getGrupos();
            return View("SearchResult", concent);
        }
    }
}