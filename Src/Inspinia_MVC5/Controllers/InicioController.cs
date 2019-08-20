using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebCartera.Models;
using WebCartera.OptionEnums;

namespace WebCartera.Controllers
{
    public class InicioController : Controller
    {
        public Parametro Sesion = Parametro.ObtenerSesionPagina();
        private readonly CarteraEntities db = new CarteraEntities();
        // GET: Inicio/CambioRango
        /// <summary>
        /// Cambia el rango de los reportes en la applicacion
        /// </summary>
        /// <param name="id">1=Dia, 2= Semana, 3= Mes, 4= Ano, 9 = Filtro por Rango</param>
        /// <returns>La ventana de Inicio</returns>
        public ActionResult CambioRango(int id)
        {
            return RedirectToAction("Index", "Inicio", new { id = Sesion.CuentaFiltro, rango = id });
        }

        // GET: Inicio
        public ActionResult Index(int? id, int? rango)
        {
            Dashboard miDashboard = new Dashboard();
            DateTime Fecha, FechInicial, FechFinal;
            Fecha = DateTime.Today;
            bool FiltrarxCuenta = true;
            List<tmovimiento> Movimientos = new List<tmovimiento>();
            try
            {             
                //Actulizo valores de los filtros
                if ((id != null && Sesion.Cuentas.Any(c => c.Id == id)) || id == 0)
                {
                    Sesion.CuentaFiltro = Convert.ToInt16(id);
                }                
                if (rango == null)
                {
                    rango = Sesion.RangoFiltro;
                }


                Sesion.RangoFiltro = Convert.ToInt16(rango);
                if (Sesion.CuentaFiltro == 0)
                {
                    FiltrarxCuenta = false;
                    ViewBag.CuentaFiltro = 0;
                }
                else {
                    ViewBag.CuentaFiltro = Sesion.CuentaFiltro;
                }
                // Filtro por fecha 1= Dia 2=Semana 3=Mes 4=Ano 5=Rango
                switch (rango)
                {
                    case 1:
                        {
                            ViewBag.Rango = "Movimientos del día ";
                            ViewBag.RangoFiltro = Fecha.ToString("dd/MM/yyyy");
                            rango = 1; // Se valida el parametro
                            if (FiltrarxCuenta)
                            {
                                Movimientos = db.tmovimientoes.Where(m => m.Id_Usuario == Sesion.Usuario.Id
                                && m.Fecha.Day == Fecha.Day
                                && m.Fecha.Month == Fecha.Month
                                && m.Fecha.Year == Fecha.Year
                                && m.Id_Cuenta == Sesion.CuentaFiltro).Include(c => c.tcuenta).ToList();
                            }
                            else
                            {
                                Movimientos = db.tmovimientoes.Where(m => m.Id_Usuario == Sesion.Usuario.Id
                                && m.Fecha.Day == Fecha.Day
                                && m.Fecha.Month == Fecha.Month
                                && m.Fecha.Year == Fecha.Year).Include(c => c.tcuenta).ToList();
                            }
                            break;
                        }
                    case 2:
                        {
                            if (TempData.ContainsKey("FiltroFecha")) {
                                ViewBag.Rango = "Rango de Fechas";
                                ReporteFeha rangoFeha = (ReporteFeha)TempData["FiltroFecha"];
                                FechInicial = rangoFeha.FecFinal;
                                FechFinal = rangoFeha.FecFinal.AddHours(23.9999);
                            }
                            else {
                                ViewBag.Rango = "Semana Actual";
                                // lastMonday is always the Monday before nextSunday. When date is a
                                // Sunday, lastMonday will be tomorrow.
                                int offset = Fecha.DayOfWeek - DayOfWeek.Monday;
                                FechInicial = Fecha.AddDays(-offset);
                                FechFinal = Fecha.AddDays(7 - (int)Fecha.DayOfWeek).AddHours(23.9999);
                            }                                                                                        
                            ViewBag.RangoFiltro = FechInicial.ToString("dd/MM/yyyy") + " hasta " + FechFinal.ToString("dd/MM/yyyy");
                            if (FiltrarxCuenta)
                            {
                                Movimientos = db.tmovimientoes.Where(m => m.Id_Usuario == Sesion.Usuario.Id
                                    && m.Fecha >= FechInicial && m.Fecha <= FechFinal && m.Id_Cuenta == Sesion.CuentaFiltro).Include(c => c.tcuenta).ToList();
                            }
                            else
                            {
                                Movimientos = db.tmovimientoes.Where(m => m.Id_Usuario == Sesion.Usuario.Id
                                && m.Fecha >= FechInicial && m.Fecha <= FechFinal).Include(c => c.tcuenta).ToList();
                            }

                            break;
                        }                 
                    case 4:
                        {
                            ViewBag.Rango = "Año Actual";
                            ViewBag.RangoFiltro = Fecha.Year;
                            if (FiltrarxCuenta)
                            {
                                Movimientos = db.tmovimientoes.Where(m => m.Id_Usuario == Sesion.Usuario.Id
                                   && m.Fecha.Year == Fecha.Year
                                   && m.Id_Cuenta == Sesion.CuentaFiltro).Include(c => c.tcuenta).ToList();
                            }
                            else
                            {
                                Movimientos = db.tmovimientoes.Where(m => m.Id_Usuario == Sesion.Usuario.Id
                                    && m.Fecha.Year == Fecha.Year).Include(c => c.tcuenta).ToList();
                            }
                            break;
                        }
                    case 5:
                        {
                            ViewBag.Rango = "Lista";
                            ViewBag.RangoFiltro = "Todos los movimientos";
                            if (FiltrarxCuenta)
                            {
                                Movimientos = db.tmovimientoes.Where(m => m.Id_Usuario == Sesion.Usuario.Id
                                    && m.Id_Cuenta == Sesion.CuentaFiltro).Include(c => c.tcuenta).ToList();
                            }
                            else
                            {
                                Movimientos = db.tmovimientoes.Where(m => m.Id_Usuario == Sesion.Usuario.Id).Include(c => c.tcuenta).ToList();
                            }
                            break;
                        }
                    default:
                        {
                            ViewBag.Rango = "Mes Actual";
                            ViewBag.RangoFiltro = Fecha.Month + " del " + Fecha.Year;
                            if (FiltrarxCuenta)
                            {
                                Movimientos = db.tmovimientoes.Where(m => m.Id_Usuario == Sesion.Usuario.Id
                                    && m.Fecha.Month == Fecha.Month
                                    && m.Fecha.Year == Fecha.Year
                                    && m.Id_Cuenta == Sesion.CuentaFiltro).Include(c => c.tcuenta).ToList();
                            }
                            else
                            {
                                Movimientos = db.tmovimientoes.Where(m => m.Id_Usuario == Sesion.Usuario.Id
                                    && m.Fecha.Month == Fecha.Month
                                    && m.Fecha.Year == Fecha.Year).Include(c => c.tcuenta).ToList();
                            }
                            break;                                                    
                        }
                }
                miDashboard.Categorias = Movimientos.Select(c => c.tcategoria).Distinct().ToList();
                miDashboard.Monedas = Movimientos.Select(c => c.tcuenta.tmoneda).Distinct().ToList();
                miDashboard.Movimientos = Movimientos.OrderByDescending(c => c.Id).ToList();
                miDashboard.Cuentas = Movimientos.Select(c => c.tcuenta).Distinct().ToList();
            }
            catch
            {
                AddMsgWeb("Error crítico al acceder a los datos", ToastType.Error);
            }
            return View(miDashboard);
        }

        [HttpPost]
        public ActionResult CambioCuenta(FormCollection form)
        {
            string strDDLValue = form["ddl_cuenta"];

            return RedirectToAction("Index", "Inicio", new { id = strDDLValue });
        }


        // GET: Monedas/SeleccionRango
        public ActionResult SeleccionRango()
        {
            //Precargo valores necesario para crear una categoria            
            ReporteFeha reportefecha = new ReporteFeha();
            reportefecha.FecInicial = DateTime.Now;
            reportefecha.FecFinal = DateTime.Now;
            return PartialView("_ReporteFecha",reportefecha);
        }
        // POST: Inico/SeleccionRango
        [HttpPost]
        public ActionResult SeleccionRango([Bind(Include = "FecInicial,FecFinal")] ReporteFeha reporteFeha)
        {
            TempData["FiltroFecha"] = reporteFeha;
            return RedirectToAction("Index", "Inicio", new { id = Sesion.CuentaFiltro, rango = 2 });
        }

        private void AddMsgWeb(string err, ToastType type)
        {
            TempData["msg"] += Notification.Show(err, position: Position.BottomRight, type: type, timeOut: 7000);
        }
    }
}