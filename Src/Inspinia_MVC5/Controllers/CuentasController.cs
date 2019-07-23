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
using toastr.Net.OptionEnums;

namespace WebCartera.Controllers
{
    public class CuentasController : Controller
    {
        private readonly CarteraEntities db = new CarteraEntities();
        private static Parametro sesion = Parametro.ObtenerSesionPagina();
        private readonly seguridadrolmodulo permiso = Parametro.VerificaPermiso(sesion,"USE");
        

        // GET: Cuentas
        public ActionResult Index()
        {            
            var cuentas = db.tcuentas.Where(m => m.Id_Usuario == sesion.Usuario.Id).ToList();
            ViewBag.Id_Moneda = new SelectList(db.tmonedas.Where(m => m.Id_Usuario == sesion.Usuario.Id && m.Activo), "Id", "Descripcion");
            return View(cuentas);
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
        public ActionResult Create([Bind(Include = "Id_Usuario,Id_Moneda,Nombre,Descripcion,Imagen,SaldoActual,Activo")] tcuenta pCuenta)
        {           
            if (ModelState.IsValid)
            {
                db.tcuentas.Add(pCuenta);
                db.SaveChanges();
                //Se actuliza la session del usuario con las nuevas cuentas
                ActualizaCuenta(-1);                
                return RedirectToAction("Index");
            }                        
            ViewBag.Id_Moneda = new SelectList(db.tmonedas.Where(m => m.Id_Usuario == sesion.Usuario.Id && m.Activo), "Id", "Descripcion", pCuenta.Id_Moneda);
            return View(pCuenta);
        }

        // GET: Cuentas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcuenta cuenta = db.tcuentas.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Moneda = new SelectList(db.tmonedas.Where(m => m.Id_Usuario == sesion.Usuario.Id && m.Activo), "Id", "Descripcion",cuenta.Id_Moneda);
            return View(cuenta);
        }

        // POST: Cuentas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_Usuario,Id_Moneda,Nombre,Descripcion,Imagen,SaldoActual,Activo")] tcuenta pCuenta)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(pCuenta).State = EntityState.Modified;
                db.SaveChanges();
                ActualizaCuenta(pCuenta.Id);
                ViewBag.Message = Notification.Show("Hello World", position: Position.BottomCenter, type: ToastType.Error, timeOut: 7000);
                return RedirectToAction("Index");
            }
            ViewBag.Id_Moneda = new SelectList(db.tmonedas.Where(m => m.Id_Usuario == sesion.Usuario.Id && m.Activo), "Id", "Descripcion",pCuenta.Id_Moneda);
            return View(pCuenta);
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
            tcuenta Cuenta = db.tcuentas.Find(id);
            int Movimientos = Cuenta.tmovimientoes.Count;

            if (Movimientos > 0)
            {
                Cuenta.Activo = false;
                db.Entry(Cuenta).State = EntityState.Modified;
            }
            else
            {
                db.tcuentas.Remove(Cuenta);
            }           
            db.SaveChanges();
            ActualizaCuenta(Cuenta.Id);
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

        private void ActualizaCuenta(int IdCuenta) {
            sesion.Cuentas = db.tcuentas.Where(c=> c.Id_Usuario == sesion.Usuario.Id && c.Activo).ToList();
            if (IdCuenta == sesion.CuentaFiltro)
            {
                sesion.CuentaFiltro = 0;
            }
        }

        // GET: Cuentas/Ingreso
        public ActionResult Ingreso()
        {                       
           return View();
        }
        // GET: Cuentas/Gasto
        public ActionResult Gasto()
        {
            return View();
        }
        // GET: Cuentas/Transferencia
        public ActionResult Transferencia()
        {
            return View();
        }     
    }
}
