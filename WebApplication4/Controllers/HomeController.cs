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
            return View();
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
            return PartialView(concent);
        }

        [AllowAnonymous]        
        public ActionResult HomeSearch(string titulo, DateTime? Y1, DateTime? Y2, List<string> autores, int?[] checkgroup, int?[] checktype)
        {
            var concent = db.concentrado.ToList();
            ViewBag.autores = dt.getAutores();
            ViewBag.grupos = dt.getGrupos();
            
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
            ViewBag.autores = dt.getAutores();
            ViewBag.grupos = dt.getGrupos();
            return View("SearchResult", concent);
        }
    }
}