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
    public class CuentasController : Controller
    {
        private CarteraEntities db = new CarteraEntities();

        // GET: Cuentas
        public ActionResult Index()
        {
            var tcuentas = db.tcuentas.Include(t => t.seguridadusuario).Include(t => t.tmoneda);
            return View(tcuentas.ToList());
        }

        // GET: Cuentas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcuenta tcuenta = db.tcuentas.Find(id);
            if (tcuenta == null)
            {
                return HttpNotFound();
            }
            return View(tcuenta);
        }

        // GET: Cuentas/Create
        public ActionResult Create()
        {
            ViewBag.Id_Usuario = new SelectList(db.seguridadusuarios, "Id", "Email");
            ViewBag.Id_Moneda = new SelectList(db.tmonedas, "Id", "Descripcion");
            return View();
        }

        // POST: Cuentas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Usuario,Id_Moneda,Nombre,Descripcion,Imagen,SaldoActual,SaldoAnterior,Activo")] tcuenta tcuenta)
        {
            if (ModelState.IsValid)
            {
                db.tcuentas.Add(tcuenta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Usuario = new SelectList(db.seguridadusuarios, "Id", "Email", tcuenta.Id_Usuario);
            ViewBag.Id_Moneda = new SelectList(db.tmonedas, "Id", "Descripcion", tcuenta.Id_Moneda);
            return View(tcuenta);
        }

        // GET: Cuentas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcuenta tcuenta = db.tcuentas.Find(id);
            if (tcuenta == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Usuario = new SelectList(db.seguridadusuarios, "Id", "Email", tcuenta.Id_Usuario);
            ViewBag.Id_Moneda = new SelectList(db.tmonedas, "Id", "Descripcion", tcuenta.Id_Moneda);
            return View(tcuenta);
        }

        // POST: Cuentas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_Usuario,Id_Moneda,Nombre,Descripcion,Imagen,SaldoActual,SaldoAnterior,Activo")] tcuenta tcuenta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tcuenta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Usuario = new SelectList(db.seguridadusuarios, "Id", "Email", tcuenta.Id_Usuario);
            ViewBag.Id_Moneda = new SelectList(db.tmonedas, "Id", "Descripcion", tcuenta.Id_Moneda);
            return View(tcuenta);
        }

        // GET: Cuentas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcuenta tcuenta = db.tcuentas.Find(id);
            if (tcuenta == null)
            {
                return HttpNotFound();
            }
            return View(tcuenta);
        }

        // POST: Cuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tcuenta tcuenta = db.tcuentas.Find(id);
            db.tcuentas.Remove(tcuenta);
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
