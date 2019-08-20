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
using System.Data.Entity.Validation;

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
            ViewBag.Id_TipoCuenta = new SelectList(sesion.TipoCuentas, "Id", "Descripcion");
            return View(cuenta);
        }

        // POST: Cuentas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Usuario,Id_Moneda,Nombre,Descripcion,Imagen,SaldoActual,Activo,Id_TipoCuenta")] tcuenta pCuenta)
        {           
            if (ModelState.IsValid)
            {
                try
                {
                    pCuenta.CuentaCredito = Convert.ToBoolean( pCuenta.Id_TipoCuenta);
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
            ViewBag.Id_TipoCuenta = new SelectList(sesion.TipoCuentas, "Id", "Descripcion", pCuenta.TipoCuentas);
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
            cuenta.Id_TipoCuenta = Convert.ToInt16(cuenta.CuentaCredito);
            ViewBag.Id_Moneda = new SelectList(db.tmonedas.Where(m => m.Id_Usuario == sesion.Usuario.Id && m.Activo), "Id", "Descripcion",cuenta.Id_Moneda);
            ViewBag.Id_TipoCuenta = new SelectList(sesion.TipoCuentas, "Id", "Descripcion", cuenta.Id_TipoCuenta);
            return View(cuenta);
        }

        // POST: Cuentas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_Usuario,Id_Moneda,Nombre,Descripcion,Imagen,SaldoActual,Activo,Id_TipoCuenta")] tcuenta pCuenta)
        {
            
            if (ModelState.IsValid)
            {
                try {
                    pCuenta.CuentaCredito = Convert.ToBoolean(pCuenta.Id_TipoCuenta);
                    db.tcuentas.Add(pCuenta);
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
            ViewBag.Id_TipoCuenta = new SelectList(sesion.TipoCuentas, "Id", "Descripcion", pCuenta.Id_TipoCuenta);
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
            tcuenta.TipoCuentas = sesion.TipoCuentas.Find(c=> c.Id== ((tcuenta.CuentaCredito == true) ? 1 : 0));
            return View(tcuenta);
        }

        // POST: Cuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tcuenta Cuenta = db.tcuentas.Find(id);
            int Movimientos = Cuenta.tmovimientoes.Count;
            Cuenta.TipoCuentas = sesion.TipoCuentas.Find(c => c.Id == ((Cuenta.CuentaCredito == true) ? 1 : 0));
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

        public void ActualizaCuenta(int? IdCuenta)
        {
            sesion.Cuentas = db.tcuentas.Where(c => c.Id_Usuario == sesion.Usuario.Id && c.Activo).ToList();
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
            if (pMovimiento.Monto < 0)
            {
                AddMsgWeb("Monto invalido, valor negativo", ToastType.Error);
                ModelState.AddModelError("ModelErr", "Monto invalido, valor negativo");
            }
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
                    catch (DbEntityValidationException exp)
                    {
                        transaction.Rollback();
                        foreach (var eve in exp.EntityValidationErrors)
                        {
                            string msg = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            AddMsgWeb("EntityValidationErrors: " + msg, ToastType.Error);

                            foreach (var ve in eve.ValidationErrors)
                            {
                                string msg1 = string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                                AddMsgWeb("EntityValidationErrors: " + msg1, ToastType.Error);
                            }
                        }

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
            if (pMovimiento.Monto < 0)
            {
                AddMsgWeb("Monto invalido, valor negativo", ToastType.Error);
                ModelState.AddModelError("ModelErr", "Monto invalido, valor negativo");
            }
            if (ModelState.IsValid)
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Ingreso Movimiento                        
                        pMovimiento.Tipo = IntValue(pMovimiento.TipoMovimiento);
                        db.tmovimientoes.Add(pMovimiento);
                        //db.SaveChanges();

                        //Actulizo Saldo
                        tcuenta cuenta = db.tcuentas.Find(pMovimiento.Id_Cuenta);
                        cuenta.SaldoAnterior = cuenta.SaldoActual;
                        //Cuentas de credito los montos de gastos se convierten en negativos
                        //if (cuenta.CuentaCredito == true)
                        //{
                        //    decimal MontoCredito = pMovimiento.Monto *-1;
                        //    cuenta.SaldoActual += MontoCredito;
                        //}
                        //else
                        //{
                            cuenta.SaldoActual -= pMovimiento.Monto;
                        //}                       
                        db.Entry(cuenta).State = EntityState.Modified;
                        db.SaveChanges();

                        //Cierro tracssaccion
                        transaction.Commit();
                        AddMsgWeb("Gasto agregado exitosamente", ToastType.Success);               
                        return RedirectToAction("Index", "Inicio");
                    }
                    catch (DbEntityValidationException exp)
                    {
                        transaction.Rollback();

                        foreach (var eve in exp.EntityValidationErrors)
                        {
                            string msg = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            AddMsgWeb("EntityValidationErrors: " + msg, ToastType.Error);                         

                            foreach (var ve in eve.ValidationErrors)
                            {
                                string msg1 = string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);                                
                                AddMsgWeb("EntityValidationErrors: " + msg1, ToastType.Error);
                            }
                        }

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
            ViewBag.CuentaOrigen = new SelectList(sesion.Cuentas, "Id", "Nombre");
            ViewBag.CuentaDestino = new SelectList(new List<tcuenta>(), "Id", "Nombre");
            ViewBag.Saldo = "0.0";
            return View();
        }
        // POST: Cuentas/Transferencia
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transferencia([Bind(Include = "CuentaOrigen,CuentaDestino,Descripcion,Monto")] Transferencia pTransferencia)
        {
            if (pTransferencia.Monto < 0)
            {
                AddMsgWeb("Monto invalido, valor negativo", ToastType.Error);
                ModelState.AddModelError("ModelErr", "Monto invalido, valor negativo");
            }
            tcuenta CuentaOrigen = db.tcuentas.Where(c => c.Id == pTransferencia.CuentaOrigen).SingleOrDefault();
            if (CuentaOrigen != null)
            {
                ViewBag.Saldo = CuentaOrigen.SaldoActual_Format;
                ViewBag.Simbolo = CuentaOrigen.tmoneda.Simbolo;
                if (pTransferencia.Monto > CuentaOrigen.SaldoActual && (CuentaOrigen.CuentaCredito==false)) {
                    ModelState.AddModelError("ModelErr", "Monto de transferencia mayor al saldo de la cuenta");                    
                    AddMsgWeb("Monto de transferencia mayor al saldo de la cuenta", ToastType.Error);
                }
            }
            else
            {
                ViewBag.Saldo = "0.0";
            }
            if (ModelState.IsValid)
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var Categoria = db.tcategorias.Where(c => c.IdUsuario == sesion.Usuario.Id && c.Tipo == 3).FirstOrDefault();
                        tmovimiento Gasto = new tmovimiento();
                        //Ingreso Gasto   
                        Gasto.Id_Usuario = sesion.Usuario.Id;
                        Gasto.Fecha = DateTime.Now;
                        Gasto.Id_Categoria = Categoria.Id;
                        Gasto.Id_Cuenta = pTransferencia.CuentaOrigen;
                        Gasto.Monto = pTransferencia.Monto;
                        Gasto.Descripcion = "Transferencia " + pTransferencia.Descripcion;
                        Gasto.Tipo = IntValue(TipoMovimiento.Debito);                        
                        db.tmovimientoes.Add(Gasto);
                      

                        tmovimiento Ingreso = new tmovimiento();
                        //Ingreso Gasto   
                        Ingreso.Id_Usuario = sesion.Usuario.Id;
                        Ingreso.Fecha = DateTime.Now;
                        Ingreso.Id_Categoria = Categoria.Id;
                        Ingreso.Id_Cuenta = pTransferencia.CuentaDestino;
                        Ingreso.Monto = pTransferencia.Monto;
                        Ingreso.Descripcion = "Transferencia " + pTransferencia.Descripcion;
                        Ingreso.Tipo = IntValue(TipoMovimiento.Credito);
                        db.tmovimientoes.Add(Ingreso);    
                        
                        db.SaveChanges();

                        //Actulizo Saldo
                        tcuenta COrigen = db.tcuentas.Find(pTransferencia.CuentaOrigen);
                        COrigen.SaldoAnterior = COrigen.SaldoActual;
                        //Cuentas de credito los montos de gastos se convierten en negativos
                        //if (COrigen.CuentaCredito == true)
                        //{
                        //    COrigen.SaldoActual -= (pTransferencia.Monto *= -1);                            
                        //}
                        //else {
                            COrigen.SaldoActual -= pTransferencia.Monto;
                        //}
                        db.Entry(COrigen).State = EntityState.Modified;
             

                        //Actulizo Saldo
                        tcuenta CDestino = db.tcuentas.Find(pTransferencia.CuentaDestino);
                        CDestino.SaldoAnterior = CDestino.SaldoActual;
                        CDestino.SaldoActual += pTransferencia.Monto;
                        db.Entry(CDestino).State = EntityState.Modified;

                        db.SaveChanges();

                        //Cierro tracssaccion
                        transaction.Commit();
                        AddMsgWeb("Gasto agregado exitosamente", ToastType.Success);                  
                        return RedirectToAction("Index", "Inicio");
                    }
                    catch (DbEntityValidationException exp)
                    {
                        transaction.Rollback();

                        foreach (var eve in exp.EntityValidationErrors)
                        {
                            string msg = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            AddMsgWeb("EntityValidationErrors: " + msg, ToastType.Error);

                            foreach (var ve in eve.ValidationErrors)
                            {
                                string msg1 = string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                                AddMsgWeb("EntityValidationErrors: " + msg1, ToastType.Error);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("ModelErr", ex.Message);
                        AddMsgWeb("Error al agregar el registro", ToastType.Error);
                    }
                }                
            }                     
            ViewBag.CuentaOrigen = new SelectList(sesion.Cuentas, "Id", "Nombre", pTransferencia.CuentaOrigen);           
            ViewBag.CuentaDestino = new SelectList(sesion.Cuentas.Where(c => c.Id_Usuario == sesion.Usuario.Id
                && c.Activo && c.Id != CuentaOrigen.Id && c.Id_Moneda == CuentaOrigen.Id_Moneda), "Id", "Nombre", pTransferencia.CuentaDestino);           
            return View(pTransferencia);
        }

        public ActionResult EditarMovimiento(int? IdTrasaccion)
        {
            try
            {
                tmovimiento Movimiento = new tmovimiento();
                if (IdTrasaccion != null)
                {
                    Movimiento = db
                        .tmovimientoes
                        .Where(m => m.Id_Usuario == sesion.Usuario.Id && m.Id == IdTrasaccion).Include(c => c.tcuenta)
                        .SingleOrDefault();
                    if (Movimiento != null)
                    {
                        ViewBag.Id_Categoria = new SelectList(db.tcategorias.Where(c => c.IdUsuario == sesion.Usuario.Id
                                && c.Tipo == Movimiento.Tipo && c.Activo), "Id", "Nombre", Movimiento.Id_Categoria);
                        Movimiento.MontoAnterior = Movimiento.Monto;
                        ViewBag.Transaccion = IdTrasaccion;
                    }
                    else
                    {                     
                        ViewBag.Transaccion = null;
                    }
                }               
                return View(Movimiento);
            }
            catch(Exception)
            {
                AddMsgWeb("Error al acceder a los datos", ToastType.Error);
                return RedirectToAction("Index", "Inicio");
            }
        }

        public ActionResult EliminarMovimiento(int? Id)
        {
            try
            {
                var Movimiento = db.tmovimientoes.Where(m => m.Id == Id && m.Id_Usuario == sesion.Usuario.Id).FirstOrDefault();

                if (Movimiento != null)
                {
                    using (DbContextTransaction transaction = db.Database.BeginTransaction())
                    {
                        try
                        {   //Elimino Movimiento
                            db.tmovimientoes.Remove(Movimiento);
                            db.SaveChanges();                          
                                                   
                            //Actulizo Saldo
                            tcuenta Cuenta = db.tcuentas.Find(Movimiento.Id_Cuenta);
                            Cuenta.SaldoAnterior = Cuenta.SaldoActual;
                            if (Movimiento.Tipo == 1)//Creditos
                            {
                                // Cuentas Credito de se invierte el valor del monto
                                if (Cuenta.CuentaCredito == true)
                                {
                                    Cuenta.SaldoActual += Movimiento.Monto;                               
                                }
                                else
                                {
                                    Cuenta.SaldoActual -= Movimiento.Monto;
                                }                               
                            }
                            else if (Movimiento.Tipo == 2)//Debitos
                            {
                                Cuenta.SaldoActual += Movimiento.Monto;
                            }

                            else {
                                AddMsgWeb("Las transferencias no pueden alterarse.", ToastType.Warning);
                                throw new Exception("Las transferencias no pueden alterarse.");
                            }
                            
                            db.Entry(Cuenta).State = EntityState.Modified;
                            db.SaveChanges();
                            
                            //Cierro tracssaccion
                            transaction.Commit();

                            AddMsgWeb("Registro eliminado exitosamente", ToastType.Warning);
                        }
                        catch (DbEntityValidationException exp)
                        {
                            transaction.Rollback();

                            foreach (var eve in exp.EntityValidationErrors)
                            {
                                string msg = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                AddMsgWeb("EntityValidationErrors: " + msg, ToastType.Error);

                                foreach (var ve in eve.ValidationErrors)
                                {
                                    string msg1 = string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                                    AddMsgWeb("EntityValidationErrors: " + msg1, ToastType.Error);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            ModelState.AddModelError("ModelErr", ex.Message);
                            AddMsgWeb("Error al agregar el registro", ToastType.Error);
                        }
                    }                    
                }
                else
                {
                    AddMsgWeb("Error proceso invalido !!", ToastType.Error);
                }
            }
            catch
            {
                AddMsgWeb("Error al acceder a los datos", ToastType.Error);              
            }
            return RedirectToAction("Index", "Inicio");
        }

        public ActionResult EditarCompleto([Bind(Include = "Id,Id_Usuario,Id_Categoria,Id_Cuenta,Fecha,Descripcion,Tipo,TipoMovimiento,Monto,MontoAnterior")] tmovimiento Movimiento)
        {
            try
            {
                bool has = db.tmovimientoes.Any(p => p.Id_Usuario == Movimiento.Id_Usuario && p.Id != Movimiento.Id);
                if (has)
                {
                    using (DbContextTransaction transaction = db.Database.BeginTransaction())
                    {
                        try
                    {
                        //Edito el Movimiento
                        db.Entry(Movimiento).State = EntityState.Modified;
                        db.SaveChanges();
                            if (Movimiento.Monto != Movimiento.MontoAnterior)
                            {
                                decimal Diferencia = Movimiento.Monto - Movimiento.MontoAnterior;                                                                                                                         
                                //Actulizo Saldo
                                tcuenta Cuenta = db.tcuentas.Find(Movimiento.Id_Cuenta);
                                Cuenta.SaldoAnterior = Cuenta.SaldoActual;
                                if (Movimiento.Tipo == 1)
                                {

                                    Cuenta.SaldoActual += Diferencia;
                                }
                                else if (Movimiento.Tipo == 2)
                                {
                                    Cuenta.SaldoActual -= Diferencia;
                                }
                                else
                                {
                                    AddMsgWeb("Las transferencias no pueden alterarse.", ToastType.Warning);
                                    throw new Exception("Las transferencias no pueden alterarse.");
                                }
                                db.Entry(Cuenta).State = EntityState.Modified;
                                db.SaveChanges();
                            }                    
                        //Cierro tracssaccion
                        transaction.Commit();                            
                         AddMsgWeb("Registro editado exitosamente", ToastType.Success);                            
                    }
                    catch (DbEntityValidationException exp)
                    {
                        transaction.Rollback();

                        foreach (var eve in exp.EntityValidationErrors)
                        {
                            string msg = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            AddMsgWeb("EntityValidationErrors: " + msg, ToastType.Error);

                            foreach (var ve in eve.ValidationErrors)
                            {
                                string msg1 = string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                                AddMsgWeb("EntityValidationErrors: " + msg1, ToastType.Error);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("ModelErr", ex.Message);
                        AddMsgWeb("Error al agregar el registro", ToastType.Error);
                    }
                    }                    
                }
                else {
                    AddMsgWeb("Error proceso invalido !!", ToastType.Error);
                }
            }
            catch(Exception)
            {
                AddMsgWeb("Error al acceder a los datos", ToastType.Error);               
            }
            return RedirectToAction("Index", "Inicio");
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
