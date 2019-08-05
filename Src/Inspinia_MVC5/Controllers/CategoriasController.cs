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
        private readonly seguridadrolmodulo permiso = Parametro.VerificaPermiso("USE");
        private Parametro sesion = Parametro.ObtenerSesionPagina();


        // GET: Categorias
        public ActionResult Index()
        {                   
            var categorias = db.tcategorias.Where(m => m.IdUsuario == sesion.Usuario.Id && m.Tipo !=3);
            ViewBag.Tipo = ListTipoCategorias();
            return View(categorias.ToList());
        }        
        public ActionResult Create()
        {
            //Precargo valores necesario para crear una categoria            
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
                { Text = "Gasto", Value = "2", });
            }
            else if (pValue == 2)
            {
                items.Add(new SelectListItem
                { Text = "Ingreso", Value = "1" });
                items.Add(new SelectListItem
                { Text = "Gasto", Value = "2", Selected = true });
            }
            else
            {

                items.Add(new SelectListItem
                { Text = "Ingreso", Value = "1" });
                items.Add(new SelectListItem
                { Text = "Gasto", Value = "2", });
            }
            return items;
        }
    }
}
