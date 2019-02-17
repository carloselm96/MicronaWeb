using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Models.DataAccess
{
    public class DataAccess
    {
        microna2018Entities db;
        public DataAccess()
        {
            db = new microna2018Entities();
        }

        /*----------------- Concentrado ---------------------*/

        //Retorna todo el concentrado
        public List<concentrado> getAllConcentrado()
        {
            List<concentrado> conc = new List<concentrado>();
            conc = db.concentrado.ToList();
            return conc;
        }

        //Retorna el concentrado por Id
        public concentrado getConcentradoById(int ?id)
        {
            return db.concentrado.Where(x => x.idConcentrado == id).FirstOrDefault();
        }

        //Retorna todo el concentrado filtrado
        public List<concentrado> getFilteredConcentrado(string titulo, DateTime? Y1, DateTime? Y2, List<string> autores, int?[] checkgroup, int?[] checktype)
        {
            List<concentrado> conc = new List<concentrado>();
            conc = db.concentrado.ToList();
            if (titulo != null)
            {
                conc= conc.Where(x => x.Titulo.ToUpper().Contains(titulo.ToUpper())).ToList();
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
        
        //Retorna dependiendo los parametros de filtro
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

        /*----------------- Catalogos ---------------------*/

        public List<tipousuario> getTiposUsuario()
        {
            return db.tipousuario.ToList();
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
        
        public archivo getArchivoById(int? id)
        {
            return db.archivo.Where(x => x.idarchivo == id).FirstOrDefault();
        }

        /*----------------- Artículos ---------------------*/

        //Edit
        public bool editArticulo(int id, articulo a, List<string> GrupoAcademico, archivo file, List<string> Autores)
        {            
            var articulo = this.getArticuloById(id);
            articulo.Nombre = a.Nombre;
            articulo.Fecha = a.Fecha;
            articulo.ISSN = a.ISSN;
            articulo.PagFinal = a.PagFinal;
            articulo.PagInicio = a.PagInicio;
            articulo.Revista = a.Revista;
            articulo.Volumen = a.Volumen;
            articulo.TipoArticulo = a.TipoArticulo;
            db.articulo_grupo.RemoveRange(db.articulo_grupo.Where(x => x.id_articulo == id));
            db.articulo_usuario.RemoveRange(db.articulo_usuario.Where(x => x.idArticulo == id));
            db.SaveChanges();
            if (GrupoAcademico != null)
            {
                foreach (var G in GrupoAcademico)
                {
                    articulo.articulo_grupo.Add(new articulo_grupo { id_articulo = id, id_grupo = int.Parse(G) });
                }
            }
            
            if (Autores != null)
            {
                foreach (var G in Autores)
                {
                    articulo.articulo_usuario.Add(new articulo_usuario { idArticulo = id, idUsuario = int.Parse(G) });
                }
            }
            
            if (file != null)
            {
                if (articulo.archivo1 != null)
                {
                    var archivo = new archivo();
                    archivo = this.getArchivoById(articulo.Archivo);
                    System.IO.File.Delete(articulo.archivo1.url);
                    db.archivo.Remove(archivo);
                }                
                db.archivo.Add(file);
                db.SaveChanges();
                articulo.Archivo = file.idarchivo;
            }
            db.SaveChanges();
            return true;
        }

        //Retorna todos los artículos
        public List<articulo> getAllArticulos()
        {
            return db.articulo.ToList();
        }

        //Retorna todos los artículos filtrados
        public List<articulo> getArticulos(string Nombre, List<string> autores, string Revista, DateTime? Y1, DateTime? Y2, List<string> grupos, string tipo)
        {
            var articulos = db.articulo.ToList();
            if (Nombre != null)
            {
                articulos = articulos.Where(x => x.Nombre.Contains(Nombre)).ToList();
            }
            if (Y1 != null)
            {
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
                var aux = grupos.Select(int.Parse).ToList();
                var aux2 = articulos.Select(y => y.idArticulo).ToList();                
                var query = from art in db.articulo.Where(x => aux2.Contains(x.idArticulo))
                            join art_gru in db.articulo_grupo.Where(x => aux.Contains(x.id_grupo.HasValue ? x.id_grupo.Value : 0))
                            on art.idArticulo equals art_gru.id_articulo
                            select new
                            {
                                arts = art
                            };
                articulos = query.Select(x => x.arts).ToList();
            }
            if (!String.IsNullOrEmpty(tipo))
            {
                articulos = articulos.Where(x => x.TipoArticulo == int.Parse(tipo)).ToList();
            }
            if (autores != null)
            {
                var aux = autores.Select(int.Parse).ToList();
                var aux2 = articulos.Select(y => y.idArticulo).ToList();                
                var query = from art in db.articulo.Where(x => aux2.Contains(x.idArticulo))
                            join art_usu in db.articulo_usuario.Where(x => aux.Contains(x.idUsuario.HasValue ? x.idUsuario.Value : 0))
                            on art.idArticulo equals art_usu.idArticulo
                            select new
                            {
                                arts = art
                            };
                articulos = query.Select(x => x.arts).ToList();
            }
            return articulos;
        }

        //Retorna un artículo por id
        public articulo getArticuloById(int? id)
        {
            return db.articulo.Where(x => x.idArticulo == id).FirstOrDefault();
        }

        //Retorna la tabla intermedia de Capitulos_Grupos
        public List<capitulo_grupo> getCapGruposByCapitulo(int id)
        {
            return db.capitulo_grupo.Where(x => x.id_capitulo == id).ToList();
        }

        //Crea un Artículo
        public bool createArticulo(articulo a, archivo file, List<string> GrupoAcademico, List<string> Autores)
        {                
            try
            {                    
                if (file != null)
                {                        
                    db.archivo.Add(file);
                    db.SaveChanges();
                    a.Archivo = file.idarchivo;
                }                
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
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        //Elimina un articulo 
        public bool removeArticulo(articulo a)
        {
            db.articulo.Remove(a);
            db.SaveChanges();
            return true;
        }

        /*----------------- Capitulos ---------------------*/

        public List<capitulolibro> getAllCapitulos()
        {
            return db.capitulolibro.ToList();
        }   

        public List<capitulolibro> getFilteredCapitulos(string Nombre, List<string> autores, string Lugar, DateTime? Y1, DateTime? Y2, List<string> grupos, string Libro)
        {
            List<capitulolibro> libros = db.capitulolibro.ToList();
            if (Nombre != null)
            {
                libros = libros.Where(x => x.Nombre.Contains(Nombre)).ToList();
            }
            if (Y1 != null)
            {                
                libros = libros.Where(x => x.Año >= Y1).ToList();
            }
            if (Y2 != null)
            {
                libros = libros.Where(x => x.Año <= Y2).ToList();
            }
            if (Libro != null)
            {
                libros = libros.Where(x => x.Libro.Contains(Libro)).ToList();
            }
            if (grupos != null)
            {
                var aux = grupos.Select(int.Parse).ToList();
                var aux2 = libros.Select(y => y.idCapituloLibro).ToList();
                var query = from cap in db.capitulolibro.Where(x => aux2.Contains(x.idCapituloLibro))
                            join cap_usu in db.capitulo_grupo.Where(x => aux.Contains(x.id_grupo.HasValue ? x.id_grupo.Value : 0))
                            on cap.idCapituloLibro equals cap_usu.id_capitulo
                            select new
                            {
                                caps = cap
                            };
                libros = query.Select(x => x.caps).ToList();
            }
            if (autores != null)
            {
                var aux = autores.Select(int.Parse).ToList();
                var aux2 = libros.Select(y => y.idCapituloLibro).ToList();
                var query = from cap in db.capitulolibro.Where(x => aux2.Contains(x.idCapituloLibro))
                            join cap_usu in db.capitulo_usuario.Where(x => aux.Contains(x.idUsuario.HasValue ? x.idUsuario.Value : 0))
                            on cap.idCapituloLibro equals cap_usu.idCapitulo
                            select new
                            {
                                caps = cap
                            };
                libros = query.Select(x => x.caps).ToList();
            }
            return libros;
        }

        public bool createCapitulo(capitulolibro cap, archivo file, List<string> GrupoAcademico, List<string> Autores)
        {
            if (file != null)
            {
                db.archivo.Add(file);
                db.SaveChanges();
                cap.Archivo = file.idarchivo;
            }
            db.capitulolibro.Add(cap);
            db.SaveChanges();
            if (GrupoAcademico != null)
            {
                foreach (var s in GrupoAcademico)
                {
                    capitulo_grupo ag = new capitulo_grupo
                    {
                        id_capitulo = cap.idCapituloLibro,
                        id_grupo = int.Parse(s)
                    };
                    db.capitulo_grupo.Add(ag);
                }
            }
            if (Autores != null)
            {
                foreach (var s in Autores)
                {
                    capitulo_usuario lb = new capitulo_usuario
                    {
                        idCapitulo = cap.idCapituloLibro,
                        idUsuario = int.Parse(s)
                    };
                    db.capitulo_usuario.Add(lb);
                }
            }
            db.SaveChanges();
            return true;
        }

        public capitulolibro getCapituloById(int id)
        {
            return db.capitulolibro.Where(x => x.idCapituloLibro == id).FirstOrDefault();
        }

        public List<capitulo_grupo> getCapitulosGrupoById(int id)
        {
            return db.capitulo_grupo.Where(x => x.id_capitulo == id).ToList();
        }

        public bool editCapitulo(int id, capitulolibro cap, List<string> GrupoAcademico, List<string> Autores, archivo file)
        {

            var l = getCapituloById(id);
            l.Nombre = cap.Nombre;
            l.ISBN = cap.ISBN;
            l.Participantes = cap.Participantes;
            l.Año = cap.Año;
            db.capitulo_usuario.RemoveRange(db.capitulo_usuario.Where(x => x.idCapitulo == id));
            db.capitulo_grupo.RemoveRange(db.capitulo_grupo.Where(x => x.id_capitulo == id));
            db.SaveChanges();
            if (Autores != null)
            {
                foreach (var G in Autores)
                {
                    db.capitulo_usuario.Add(new capitulo_usuario { idCapitulo = id, idUsuario = int.Parse(G) });
                }
            }
            if (GrupoAcademico != null)
            {
                foreach (var G in GrupoAcademico)
                {
                    db.capitulo_grupo.Add(new capitulo_grupo { id_capitulo = id, id_grupo = int.Parse(G) });
                }
            }
            if (file!=null)
            {
                if (l.archivo1 != null)
                {
                    var archivo = new archivo();
                    archivo = db.archivo.Where(x => x.idarchivo == l.Archivo).FirstOrDefault();
                    System.IO.File.Delete(l.archivo1.url);
                    db.archivo.Remove(archivo);
                }                
                db.archivo.Add(file);
                db.SaveChanges();
                l.Archivo = file.idarchivo;
            }
            db.SaveChanges();
            return true;
        }

        public void removeCapitulo(capitulolibro c)
        {
            db.capitulolibro.Remove(c);
            db.SaveChanges();
        }

        /*----------------- Libro ---------------------*/

        public List<libro> getAllLibros()
        {
            return db.libro.ToList();
        }

        public libro getLibroById(int id)
        {
            return db.libro.Where(x => x.idLibro == id).FirstOrDefault();
        }

        public List<libro> getFilteredLibros(string Nombre, DateTime? Y1, DateTime? Y2, List<string> grupos, List<string> autores)
        {
            List<libro> libros = this.getAllLibros();
            if (Nombre != null)
            {
                libros = libros.Where(x => x.Nombre.Contains(Nombre)).ToList();
            }

            if (Y1 != null)
            {
                //int year1 = int.Parse(Y1);
                libros = libros.Where(x => x.Año >= Y1).ToList();
            }
            if (Y2 != null)
            {
                libros = libros.Where(x => x.Año <= Y2).ToList();
            }
            if (grupos != null)
            {
                List<libro> cg = new List<libro>();
                foreach (string s in grupos)
                {
                    int i = int.Parse(s);
                    var g = db.libro_grupo.Where(x => x.id_grupo == i).ToList();

                    foreach (var cap in g)
                    {
                        libro sample = db.libro.Where(x => x.idLibro == cap.id_libro).FirstOrDefault();
                        cg.Add(sample);
                    }

                }
                libros = libros.Where(x => cg.Contains(x)).ToList();
            }
            if (autores != null)
            {
                foreach (string s in autores)
                {
                    int i = int.Parse(s);
                    var g = db.libro_usuario.Where(x => x.idUsuario == i).ToList();
                    List<libro> cg = new List<libro>();
                    foreach (var cap in g)
                    {
                        libro sample = db.libro.Where(x => x.idLibro == cap.idLibro).FirstOrDefault();
                        cg.Add(sample);
                    }
                    libros = libros.Where(x => cg.Contains(x)).ToList();
                }
            }
            return libros;
        }

        public bool createLibro(libro lib, archivo file, List<string> GrupoAcademico, List<string> Autores)
        {
            
            if (file != null)
            {
                db.archivo.Add(file);
                db.SaveChanges();
                lib.Archivo = file.idarchivo;
            }
            db.libro.Add(lib);
            db.SaveChanges();
            if (GrupoAcademico != null)
            {
                foreach (var s in GrupoAcademico)
                {
                    libro_grupo ag = new libro_grupo
                    {
                        id_libro = lib.idLibro,
                        id_grupo = int.Parse(s)
                    };
                    db.libro_grupo.Add(ag);
                }
            }
            if (Autores != null)
            {
                foreach (var s in Autores)
                {
                    libro_usuario lb = new libro_usuario
                    {
                        idLibro = lib.idLibro,
                        idUsuario = int.Parse(s)
                    };
                    db.libro_usuario.Add(lb);
                }
            }
            db.SaveChanges();
            return true;
        }

        public List<tipolibro> getTipoLibro()
        {
            return db.tipolibro.ToList();
        }

        public List<libro_grupo> GetLibro_Grupos(int? id)
        {
            if (id != null)
            {
                return db.libro_grupo.Where(x => x.id_libro == id).ToList();
            }
            return db.libro_grupo.ToList();
        }

        public bool editLibro(int id, libro lib, List<string> GrupoAcademico, archivo file, List<string> Autores)
        {
            var l = this.getLibroById(id);
            if (l.archivo1 != null)
            {
                var archivo = new archivo();
                archivo = db.archivo.Where(x => x.idarchivo == l.Archivo).FirstOrDefault();
                System.IO.File.Delete(l.archivo1.url);
                db.archivo.Remove(archivo);
            }
            if (file !=null)
            {
                db.archivo.Add(file);
                db.SaveChanges();
                l.Archivo = file.idarchivo;
            }
            l.Nombre = lib.Nombre;
            l.Editorial = lib.Editorial;
            l.Pais = lib.Pais;
            l.ISBN = lib.ISBN;
            l.TipoLibro = lib.TipoLibro;
            l.Año = lib.Año;
            var autores_eliminar = db.libro_usuario.Where(x => x.idLibro == id).ToList();
            if (autores_eliminar != null)
            {
                foreach (var G in autores_eliminar)
                {
                    db.libro_usuario.Remove(G);
                }
            }
            var grupos_eliminar = db.libro_grupo.Where(x => x.id_libro == id).ToList();
            if (grupos_eliminar != null)
            {
                foreach (var G in grupos_eliminar)
                {
                    db.libro_grupo.Remove(G);
                }
            }
            db.SaveChanges();
            if (Autores != null)
            {
                foreach (var G in Autores)
                {
                    db.libro_usuario.Add(new libro_usuario { idLibro = id, idUsuario = int.Parse(G) });
                }
            }
            
            if (GrupoAcademico != null)
            {
                foreach (var G in GrupoAcademico)
                {
                    db.libro_grupo.Add(new libro_grupo { id_libro = id, id_grupo = int.Parse(G) });
                }
            }            
            db.SaveChanges();
            return true;
        }

        public bool removeLibro(int id)
        {
            var libr = this.getLibroById(id);
            var a_g = db.libro_grupo.Where(x => x.id_libro == id).ToList();
            if (a_g != null)
            {
                foreach (var a in a_g)
                {
                    db.libro_grupo.Remove(a);
                }
            }
            var l_a = db.libro_usuario.Where(x => x.idLibro == id).ToList();
            if (l_a != null)
            {
                foreach (var a in l_a)
                {
                    db.libro_usuario.Remove(a);
                }
            }
            db.libro.Remove(libr);
            db.SaveChanges();
            return true;
        }

        /*----------------- Usuarios ---------------------*/

        //Obtener Usuario por ID
        public usuario getUsuarioById(int id)
        {
            return db.usuario.Where(x => x.idUsuario == id && x.Status=="A").FirstOrDefault();
        }

        public bool validateUserName(string username, int? id)
        {
            if (id==null)
            {
                return db.usuario.Where(x=>x.Usuario1==username && x.Status=="A").ToList().Count>0 ? false : true;
            }
            return db.usuario.Where(x => (x.Usuario1 == username && x.Status == "A") && (x.idUsuario != id)).FirstOrDefault()!=null ? false : true;   
        }

        public bool createUsuario(usuario u)
        {
            u.Status = "A";
            db.usuario.Add(u);
            db.SaveChanges();
            return true;
        }

        public int editUsuario(usuario u, int id)
        {                      
            var user = db.usuario.Where(x => x.idUsuario == id && x.Status == "A").FirstOrDefault();
            user.Nombre = u.Nombre;
            user.Correo = u.Correo;
            user.Contraseña = u.Contraseña;
            user.TipoUsuario = u.TipoUsuario;
            u.Usuario1 = u.Usuario1;
            user.Usuario1 = u.Usuario1;
            user.Apellido_Materno = u.Apellido_Materno;
            user.Apellido_Paterno = u.Apellido_Paterno;
            db.SaveChanges();
            return user.idUsuario;
        }

        public bool deleteUsuario(int id)
        {
            var u = getUsuarioById(id);
            if (u != null)
            {
                u.Status = "B";
                db.SaveChanges();
                return true;
            }
            return false;
        }

        /*----------------- Related Data ---------------------*/

        public Dictionary<string,int> getConcentradoData()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            dic.Add("Articulos", db.articulo.Count());
            dic.Add("Libros", db.libro.Count());
            dic.Add("Capitulos", db.capitulolibro.Count());
            dic.Add("Tesis", db.tesis.Count());
            dic.Add("Trabajos", db.trabajo.Count());
            dic.Add("Proyectos", db.proyectos.Count());
            return dic;
        }

        public List<usuario> getRelatedUsers(IEnumerable<int> conc)
        {            
            var query = from con_aut in db.concentrado_autores
                        join con in db.concentrado.Where(x=> conc.Contains(x.idConcentrado))
                        on con_aut.idConcentrado equals con.idConcentrado
                        select new
                        {
                            con_a = con_aut.idAutor
                       };
            var query2 =from usua in db.usuario
                        join autores in query
                        on usua.idUsuario equals autores.con_a                         
                         select new
                         {
                             aut = usua
                         };            
            return query2.Select(x => x.aut).Distinct().ToList();
        }

        public List<grupoacademico> getRelatedGrupos(IEnumerable<int> conc)
        {
            var query = from con_grup in db.concentrado_grupos
                        join con in db.concentrado.Where(x => conc.Contains(x.idConcentrado))
                        on con_grup.Concentrado equals con.idConcentrado
                        select new
                        {
                            con_g = con_grup.Grupo
                        };
            var query2 = from grup in db.grupoacademico
                         join grupos in query
                         on grup.idGrupoAcademico equals grupos.con_g
                         select new
                         {
                             g = grup
                         };
            return query2.Select(x => x.g).Distinct().ToList();
        }

        public archivo getDownloadUrl(int id)
        {
            concentrado con = db.concentrado.Where(x => x.idConcentrado == id).FirstOrDefault();            
            archivo arc = null;            
            switch (con.TipoConcentrado)
            {
                case 1:
                    arc=db.articulo.Where(x => x.idArticulo == con.IdItem).FirstOrDefault().archivo1;                    
                    break;
                case 2:
                    arc=db.capitulolibro.Where(x => x.idCapituloLibro == con.IdItem).FirstOrDefault().archivo1;                    
                    break;
                case 3:
                    arc=db.libro.Where(x => x.idLibro == con.IdItem).FirstOrDefault().archivo1;                    
                    break;
                case 4:
                    arc=db.proyectos.Where(x => x.idProyecto == con.IdItem).FirstOrDefault().archivo1;                    
                    break;
                case 5:
                    arc=db.trabajo.Where(x => x.idTrabajo == con.IdItem).FirstOrDefault().archivo1;                    
                    break;
                case 6:
                    arc=db.tesis.Where(x => x.idtesis == con.IdItem).FirstOrDefault().archivo1;                    
                    break;
                default:
                    break;
            }
            return arc;
        }

    }
}