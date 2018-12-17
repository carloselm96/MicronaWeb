using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models.DataAccess
{
    public class DataAccess
    {
        microna2018Entities db;
        public DataAccess()
        {
            db = new microna2018Entities();
        }
        
        public List<concentrado> getConcentrado()
        {
            List<concentrado> conc = new List<concentrado>();
            conc = db.concentrado.ToList();
            return conc;
        }
        
        public List<concentrado> getAllConcentrado(string titulo, DateTime? Y1, DateTime? Y2, List<string> autores, int?[] checkgroup, int?[] checktype)
        {
            List<concentrado> conc = new List<concentrado>();
            conc = db.concentrado.ToList();
            if (titulo != null)
            {
                conc= conc.Where(x => x.Titulo.Contains(titulo)).ToList();
            }
            if (Y1 != null)
            {
                conc= conc.Where(x => x.Fecha >= Y1).ToList();
            }
            if (Y2 != null)
            {
                conc= conc.Where(x => x.Fecha <= Y2).ToList();
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
                conc= conc.Where(x => cg.Contains(x)).ToList();
            }
            if (checktype != null)
            {
                List<concentrado> ct = new List<concentrado>();
                foreach (int i in checktype)
                {
                    var g = conc.Where(x => x.TipoConcentrado == i).ToList();
                    foreach (var con in g)
                    {
                        ct.Add(con);
                    }
                }
                conc= ct;
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
                conc= conc.Where(x => cg.Contains(x)).ToList();
            }
            return conc;
        }
        
        public List<concentrado> getConcentrado(string titulo, DateTime? Y1, DateTime? Y2, List<string> autores, int?[] checkgroup, int?[] checktype)
        {
            List<concentrado> conc = new List<concentrado>();
            conc = conc.Where(x => x.Titulo.Contains(titulo)).ToList();
            if (Y1 != null)
            {
                conc = conc.Where(x => x.Fecha >= Y1).ToList();
            }
            if (Y2 != null)
            {
                conc = conc.Where(x => x.Fecha <= Y2).ToList();
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
                conc = conc.Where(x => cg.Contains(x)).ToList();
            }
            if (checktype != null)
            {
                List<concentrado> ct = new List<concentrado>();
                foreach (int i in checktype)
                {
                    var g = conc.Where(x => x.TipoConcentrado == i).ToList();
                    foreach (var con in g)
                    {
                        ct.Add(con);
                    }
                }
                conc = ct;
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
                conc = conc.Where(x => cg.Contains(x)).ToList();
            }
            return conc;
        }

        public List<usuario> getAutores()
        {
            List<usuario> autores = new List<usuario>();
            autores= db.usuario.Where(x => x.Status.Equals("A")).ToList();
            return autores;
        }

        public List<grupoacademico> getGrupos()
        {
            List<grupoacademico> grupos_academicos = new List<grupoacademico>();
            grupos_academicos=db.grupoacademico.ToList();
            return grupos_academicos;
        }


        public List<tipoarticulo> getTiposArticulo()
        {
            List<tipoarticulo> tipos_art = db.tipoarticulo.ToList();
            return tipos_art;
        }

        public List<tipolibro> getTiposLibro()
        {
            List<tipolibro> tipos_lib = db.tipolibro.ToList();
            return tipos_lib;
        }

        public List<tipotrabajo> getTiposTrabajo()
        {
            List<tipotrabajo> tipos_trab = db.tipotrabajo.ToList();
            return tipos_trab;
        }

        public bool addTipo(string name, string type)
        {

            try
            {
                switch (type)
                {
                    case "1":
                        tipoarticulo t = new tipoarticulo { Nombre = name };
                        db.tipoarticulo.Add(t);
                        break;
                    case "2":
                        tipotrabajo a = new tipotrabajo { Nombre = name };
                        db.tipotrabajo.Add(a);
                        break;
                    case "3":
                        tipolibro b = new tipolibro { Nombre = name };
                        db.tipolibro.Add(b);
                        break;
                    case "4":
                        grupoacademico g = new grupoacademico { Nombre = name };
                        db.grupoacademico.Add(g);
                        break;

                }
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }            
        }

        public bool editTipo(string name, string type, int id)
        {

            switch (type)
            {
                case "1":
                    tipoarticulo t = db.tipoarticulo.Where(x => x.idTipoArticulo == id).FirstOrDefault();
                    t.Nombre = name;
                    break;
                case "2":
                    tipotrabajo a = db.tipotrabajo.Where(x => x.idTipoTrabajo == id).FirstOrDefault();
                    a.Nombre = name;
                    break;
                case "3":
                    tipolibro b = db.tipolibro.Where(x => x.idTipoLibro == id).FirstOrDefault();
                    b.Nombre = name;
                    break;
                case "4":
                    grupoacademico g = db.grupoacademico.Where(x => x.idGrupoAcademico == id).FirstOrDefault();
                    g.Nombre = name;
                    break;

            }
            db.SaveChanges();
            return true;
        }

        public bool deleteTipo(string tipo, int id)
        {

            switch (tipo)
            {
                case "1":
                    tipoarticulo t = db.tipoarticulo.Where(x => x.idTipoArticulo == id).FirstOrDefault();
                    db.tipoarticulo.Remove(t);
                    break;
                case "2":
                    tipotrabajo a = db.tipotrabajo.Where(x => x.idTipoTrabajo == id).FirstOrDefault();
                    db.tipotrabajo.Remove(a);
                    break;
                case "3":
                    tipolibro b = db.tipolibro.Where(x => x.idTipoLibro == id).FirstOrDefault();
                    db.tipolibro.Remove(b);
                    break;
                case "4":
                    grupoacademico g = db.grupoacademico.Where(x => x.idGrupoAcademico == id).FirstOrDefault();
                    db.grupoacademico.Remove(g);
                    break;

            }
            db.SaveChanges();
            return true;
        }

        public List<articulo> getAllArticulos()
        {
            return db.articulo.ToList();
        }
    }
}