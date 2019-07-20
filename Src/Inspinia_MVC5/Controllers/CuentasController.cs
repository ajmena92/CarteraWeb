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
        private seguridadrolmodulo permiso = Parametro.VerificaPermiso("USE");
        Parametro sesion = Parametro.ObtenerSesionPagina();

        // GET: Cuentas
        public ActionResult Index()
        {            
            var cuentas = db.tcuentas.Where(m => m.Id_Usuario == sesion.Usuario.Id);
            ViewBag.Id_Moneda = new SelectList(db.tmonedas.Where(m => m.Id_Usuario == sesion.Usuario.Id && m.Activo), "Id", "Descripcion");
            return View(cuentas.ToList());
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
            tcuenta cuenta = new tcuenta();
            cuenta.Id_Usuario = sesion.Usuario.Id;
            cuenta.Activo = true;
            ViewBag.Id_Moneda = new SelectList(db.tmonedas.Where(m=> m.Id_Usuario == sesion.Usuario.Id && m.Activo), "Id", "Descripcion");
            return View(cuenta);       
        }

        // POST: Cuentas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Usuario,Id_Moneda,Nombre,Descripcion,Imagen,SaldoActual,Activo")] tcuenta tcuenta)
        {           
            if (ModelState.IsValid)
            {
                db.tcuentas.Add(tcuenta);
                db.SaveChanges();
                //Se actuliza la session del usuario con las nuevas cuentas
                seguridadusuario Usuario = db.seguridadusuarios.Where(u => u.Email == sesion.Usuario.Email).SingleOrDefault(); 
                Parametro.CrearSesionPagina(Usuario);
                return RedirectToAction("Index");
            }                        
            ViewBag.Id_Moneda = new SelectList(db.tmonedas.Where(m => m.Id_Usuario == sesion.Usuario.Id && m.Activo), "Id", "Descripcion", tcuenta.Id_Moneda);
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
            ViewBag.Id_Moneda = new SelectList(db.tmonedas.Where(m => m.Id_Usuario == sesion.Usuario.Id && m.Activo), "Id", "Descripcion");
            return View(tcuenta);
        }

        // POST: Cuentas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_Usuario,Id_Moneda,Nombre,Descripcion,Imagen,SaldoActual,Activo")] tcuenta tcuenta)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(tcuenta).State = EntityState.Modified;
                db.SaveChanges();
                seguridadusuario Usuario = db.seguridadusuarios.Where(u => u.Email == sesion.Usuario.Email).SingleOrDefault();
                Parametro.CrearSesionPagina(Usuario);
                return RedirectToAction("Index");
            }
            ViewBag.Id_Moneda = new SelectList(db.tmonedas.Where(m => m.Id_Usuario == sesion.Usuario.Id && m.Activo), "Id", "Descripcion");
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
            tcuenta cuenta = db.tcuentas.Find(id);
            int Movimientos = cuenta.tmovimientoes.Count;

            if (Movimientos > 0)
            {
                cuenta.Activo = false;
                db.Entry(cuenta).State = EntityState.Modified;
            }
            else
            {
                db.tcuentas.Remove(cuenta);
            }           
            db.SaveChanges();
            seguridadusuario Usuario = db.seguridadusuarios.Where(u => u.Email == sesion.Usuario.Email).SingleOrDefault();
            Parametro.CrearSesionPagina(Usuario);
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
