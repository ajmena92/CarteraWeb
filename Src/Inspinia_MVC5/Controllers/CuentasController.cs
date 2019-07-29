using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebCartera.Models;

using EntityState = System.Data.Entity.EntityState;
using WebCartera.OptionEnums;
using System.Reflection;
using System.ComponentModel;
using WebCartera.Helpers.OptionEnums;
using System.Data.Entity;

namespace WebCartera.Controllers
{
    public class CuentasController : Controller
    {
        private readonly CarteraEntities db = new CarteraEntities();                
        private readonly seguridadrolmodulo permiso = Parametro.VerificaPermiso("USE");
        private Parametro sesion = Parametro.ObtenerSesionPagina();

        // GET: Cuentas
        public ActionResult Index()
        {
            try
            {
                List<tcuenta> cuentas = db.tcuentas.Where(m => m.Id_Usuario == sesion.Usuario.Id).ToList();
                ViewBag.Id_Moneda = new SelectList(db.tmonedas.Where(m => m.Id_Usuario == sesion.Usuario.Id && m.Activo), "Id", "Descripcion");
                return View(cuentas);
            }
            catch {
                AddMsgWeb("Error al acceder a los datos", ToastType.Error);
                return RedirectToAction("Index","Inicio");
            }
           
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
                try { 
                db.tcuentas.Add(pCuenta);
                db.SaveChanges();
                //Se actuliza la session del usuario con las nuevas cuentas
                ActualizaCuenta(-1);
                    AddMsgWeb("Registro agregado exitosamente", ToastType.Success);                    
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ModelErr", ex.Message);
                    AddMsgWeb("Error al agregar el registro",ToastType.Error);                    
                }
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
                try {
                    db.Entry(pCuenta).State = EntityState.Modified;
                    db.SaveChanges();
                    ActualizaCuenta(pCuenta.Id);
                    AddMsgWeb("Registro editado exitosamente", ToastType.Success);                    
                    return RedirectToAction("Index");
                }
                catch (Exception ex) {
                    ModelState.AddModelError("ModelErr", ex.Message);
                    AddMsgWeb("Error al editar el registro",ToastType.Error);                    
                }              
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
            try
            {
            
                if (Movimientos > 0)
                {
                    Cuenta.Activo = false;
                    db.Entry(Cuenta).State = EntityState.Modified;
                    AddMsgWeb("Registro desactivo, cuenta con movimientos asociados", ToastType.Warning);                  
                }
                else
                {
                    db.tcuentas.Remove(Cuenta);
                    AddMsgWeb("Registro eliminado exitosamente",ToastType.Success);                
                }           
                db.SaveChanges();
                ActualizaCuenta(Cuenta.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ModelErr", ex.Message);
                AddMsgWeb("Error al eliminar el registro",ToastType.Error);            
                tcuenta tcuenta = db.tcuentas.Find(id);               
                return View(tcuenta);
            }
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
            int tipo = IntValue(TipoMovimiento.Credito);
            tmovimiento ingreso = new tmovimiento();
            ingreso.Id_Usuario = sesion.Usuario.Id;
            ingreso.TipoMovimiento = TipoMovimiento.Credito;      
            ingreso.Fecha = DateTime.Now;
            ViewBag.Id_Cuenta = new SelectList(sesion.Cuentas, "Id", "Nombre", sesion.CuentaFiltro);
            ViewBag.Id_Categoria = new SelectList(
                db.tcategorias.Where(c => c.IdUsuario == sesion.Usuario.Id && c.Tipo == tipo && c.Activo),
                "Id",
                "Nombre");
            return View(ingreso);
        }
        // POST: Cuentas/Ingreso
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ingreso([Bind(Include = "Id_Usuario,Id_Categoria,Id_Cuenta,Fecha,Descripcion,TipoMovimiento,Monto")] tmovimiento pMovimiento)
        {                         
            if (ModelState.IsValid)
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Ingreso Movimiento
                        pMovimiento.Tipo = IntValue(pMovimiento.TipoMovimiento);
                        db.tmovimientoes.Add(pMovimiento);
                        db.SaveChanges();

                        //Actulizo Saldo
                        tcuenta cuenta = db.tcuentas.Find(pMovimiento.Id_Cuenta);
                        cuenta.SaldoAnterior = cuenta.SaldoActual;
                        cuenta.SaldoActual += pMovimiento.Monto;
                        db.Entry(cuenta).State = EntityState.Modified;
                        db.SaveChanges();

                        //Cierro tracssaccion
                        transaction.Commit();
                        AddMsgWeb("Ingreso agregado exitosamente", ToastType.Success);
                        return RedirectToAction("Index", "Inicio");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("ModelErr", ex.Message);
                        AddMsgWeb("Error al agregar el registro", ToastType.Error);
                    }                  
                }
              
            }
            ViewBag.Id_Cuenta = new SelectList(sesion.Cuentas, "Id", "Nombre", pMovimiento.Id_Cuenta);
            int tipo = IntValue(TipoMovimiento.Credito);
            ViewBag.Id_Categoria = new SelectList(
              db.tcategorias.Where(c => c.IdUsuario == sesion.Usuario.Id && c.Tipo == tipo && c.Activo),
              "Id",
              "Nombre", pMovimiento.Id_Categoria);
            return View(pMovimiento);
        }

        // GET: Cuentas/Gasto
        public ActionResult Gasto()
        {
            int tipo = IntValue(TipoMovimiento.Debito);
            tmovimiento gasto = new tmovimiento();
            gasto.Id_Usuario = sesion.Usuario.Id;
            gasto.TipoMovimiento = TipoMovimiento.Debito;
            gasto.Fecha = DateTime.Now;
            ViewBag.Id_Cuenta = new SelectList(sesion.Cuentas, "Id", "Nombre", sesion.CuentaFiltro);
            ViewBag.Id_Categoria = new SelectList(db.tcategorias.Where(c => c.IdUsuario == sesion.Usuario.Id
                && c.Tipo == tipo && c.Activo), "Id", "Nombre");
            return View(gasto);
        }
        // POST: Cuentas/Ingreso
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Gasto([Bind(Include = "Id_Usuario,Id_Categoria,Id_Cuenta,Fecha,Descripcion,TipoMovimiento,Monto")] tmovimiento pMovimiento)
        {
            if (ModelState.IsValid)
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Ingreso Movimiento
                        pMovimiento.Tipo = IntValue(pMovimiento.TipoMovimiento);
                        db.tmovimientoes.Add(pMovimiento);
                        db.SaveChanges();

                        //Actulizo Saldo
                        tcuenta cuenta = db.tcuentas.Find(pMovimiento.Id_Cuenta);
                        cuenta.SaldoAnterior = cuenta.SaldoActual;
                        cuenta.SaldoActual -= pMovimiento.Monto;
                        db.Entry(cuenta).State = EntityState.Modified;
                        db.SaveChanges();

                        //Cierro tracssaccion
                        transaction.Commit();
                        AddMsgWeb("Gasto agregado exitosamente", ToastType.Success);
                        return RedirectToAction("Index", "Inicio");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("ModelErr", ex.Message);
                        AddMsgWeb("Error al agregar el registro", ToastType.Error);
                    }
                }          
            }
            ViewBag.Id_Cuenta = new SelectList(sesion.Cuentas, "Id", "Nombre", pMovimiento.Id_Cuenta);
            int tipo = IntValue(TipoMovimiento.Debito);
            ViewBag.Id_Categoria = new SelectList(db.tcategorias.Where(c => c.IdUsuario == sesion.Usuario.Id
                && c.Tipo == tipo && c.Activo), "Id", "Nombre", pMovimiento.Id_Categoria);
            return View(pMovimiento);
        }

        // GET: Cuentas/Transferencia
        public ActionResult Transferencia()
        {
            return View();
        }
        private void AddMsgWeb(string err, ToastType type)
        {
            TempData["msg"] += Notification.Show(err, position: Position.BottomRight, type: type, timeOut: 7000);
        }
        private static int IntValue(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return Convert.ToInt16(attributes[0].Description);
            }
            else
            {
                return Convert.ToInt16(value);
            }
        }
    }
}
