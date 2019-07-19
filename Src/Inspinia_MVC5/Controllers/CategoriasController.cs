using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebCartera.Models;
using EntityState = System.Data.Entity.EntityState;

using WebCartera.Helpers;
using System.Web.Routing;
using PagedList;

namespace WebCartera.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly CarteraEntities db = new CarteraEntities();
        private seguridadrolmodulo permiso = Parametro.VerificaPermiso("USE");

        // GET: Categorias
        public ActionResult Index(string currentFilter, string searchString,int? Inactivo, int? sortType, int? page)
        {
            Parametro sesion = Parametro.ObtenerSesionPagina();      
            if (searchString != null)
            {
                searchString = searchString.ToLower();
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (sortType == null || sortType > 2 || sortType < 0)
            {
                sortType = 0;
            }
            if (Inactivo == null || Inactivo != 1)
            {
                ViewBag.Inactivo = 0;
            }
            else
            {
                if (ViewBag.Inactivo == 1)
                {
                    ViewBag.Inactivo = 0;
                    Inactivo = 0;
                }
                else
                {
                    ViewBag.Inactivo = 1;
                }
            }
            ViewBag.sortType = sortType;
            ViewBag.CurrentFilter = searchString;            
           
            var categorias = db.tcategorias.Where(c=> c.IdUsuario == sesion.Usuario.Id).AsQueryable();
            if (Inactivo == 0) {
                categorias = categorias.Where(p => p.Activo);
            }
            if (sortType > 0)
            {
                categorias = categorias.Where(p => p.Tipo == sortType);
            }         
            if (!string.IsNullOrEmpty(searchString))
            {
                categorias = categorias.Where(p => p.Nombre.ToLower().Contains(searchString));
                ViewBag.SearchString = searchString;
            }                             
            categorias = categorias.OrderBy(p => p.Tipo).ThenBy(p => p.Id);
            const int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(categorias.ToPagedList(pageNumber, pageSize));
        }
        
        public ActionResult Create()
        {
            //Precargo valores necesario para crear una categoria
            Parametro sesion = Parametro.ObtenerSesionPagina();
            tcategoria categoria = new tcategoria();
            categoria.IdUsuario = sesion.Usuario.Id;      
            categoria.Activo = true;
            ViewBag.Tipo = ListTipoCategorias();
            return View(categoria);
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdUsuario,Nombre,Tipo,Imagen,Activo")] tcategoria tcategoria)
        {
            if (ModelState.IsValid)
            {
                db.tcategorias.Add(tcategoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }      
            ViewBag.Tipo = ListTipoCategorias(tcategoria.Tipo);
            return View(tcategoria);
        }

        // GET: Categorias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcategoria tcategoria = db.tcategorias.Find(id);
            if (tcategoria == null)
            {
                return HttpNotFound();
            }
            ViewBag.Tipo = ListTipoCategorias(tcategoria.Tipo);
            return View(tcategoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdUsuario,Nombre,Tipo,Imagen,Activo")] tcategoria tcategoria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tcategoria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Tipo = ListTipoCategorias(tcategoria.Tipo);
            return View(tcategoria);
        }

        // GET: Categorias/Delete/5
        public ActionResult Delete(int? id)
        {         
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcategoria tcategoria = db.tcategorias.Find(id);
            if (tcategoria == null)
            {
                return HttpNotFound();
            }
            return View(tcategoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tcategoria categoria = db.tcategorias.Find(id);

            int Movimientos = categoria.tmovimientoes.Count;

            if (Movimientos>0)
            {
                categoria.Activo = false;
                db.Entry(categoria).State = EntityState.Modified;
            }
            else
            {
                db.tcategorias.Remove(categoria);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public static List<SelectListItem> ListTipoCategorias( int pValue = 0)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            if (pValue == 1)
            {
                items.Add(new SelectListItem
                { Text = "Ingreso", Value = "1", Selected = true });
                items.Add(new SelectListItem
                { Text = "Gastos", Value = "2", });
            }
            else if (pValue == 2)
            {
                items.Add(new SelectListItem
                { Text = "Ingreso", Value = "1" });
                items.Add(new SelectListItem
                { Text = "Gastos", Value = "2", Selected = true });
            }
            else
            {

                items.Add(new SelectListItem
                { Text = "Ingreso", Value = "1" });
                items.Add(new SelectListItem
                { Text = "Gastos", Value = "2", });
            }
            return items;
        }
    }
}
