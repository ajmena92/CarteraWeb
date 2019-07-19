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

namespace WebCartera.Controllers
{
    public class MonedasController : Controller
    {
        private CarteraEntities db = new CarteraEntities();
        private seguridadrolmodulo permiso = Parametro.VerificaPermiso("USE");

        // GET: Monedas
        public ActionResult Index()
        {
            Parametro sesion = Parametro.ObtenerSesionPagina();
            var tmonedas = db.tmonedas.Where(m => m.Id_Usuario == sesion.Usuario.Id);
            return View(tmonedas.ToList());
        }
 
        // GET: Monedas/Create
        public ActionResult Create()
        {
            //Precargo valores necesario para crear una categoria
            Parametro sesion = Parametro.ObtenerSesionPagina();
            tmoneda moneda = new tmoneda();
            moneda.Id_Usuario = sesion.Usuario.Id;
            moneda.Activo = true;        
            return View(moneda);                        
        }

        // POST: Monedas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Usuario,Descripcion,Simbolo,Activo")] tmoneda tmoneda)
        {
            if (ModelState.IsValid)
            {
                db.tmonedas.Add(tmoneda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }            
            return View(tmoneda);
        }

        // GET: Monedas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tmoneda tmoneda = db.tmonedas.Find(id);
            if (tmoneda == null)
            {
                return HttpNotFound();
            }            
            return View(tmoneda);
        }

        // POST: Monedas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_Usuario,Descripcion,Simbolo,Activo")] tmoneda tmoneda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tmoneda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }            
            return View(tmoneda);
        }

        // GET: Monedas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tmoneda tmoneda = db.tmonedas.Find(id);
            if (tmoneda == null)
            {
                return HttpNotFound();
            }
            return View(tmoneda);
        }

        // POST: Monedas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            tmoneda moneda = db.tmonedas.Find(id);

            int Movimientos = moneda.tcuentas.Count;

            if (Movimientos > 0)
            {
                moneda.Activo = false;
                db.Entry(moneda).State = EntityState.Modified;
            }
            else
            {
                db.tmonedas.Remove(moneda);
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
    }
}
