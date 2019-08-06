using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCartera.Models;
using WebCartera.OptionEnums;
namespace WebCartera.Controllers
{
    public class InicioController : Controller
    {
        private readonly CarteraEntities db = new CarteraEntities();
        public Parametro Sesion = Parametro.ObtenerSesionPagina();

        // GET: Inicio
        public ActionResult Index(int? id, int? rango )
        {
            DateTime Fecha, FechInicial, FechFinal;
            Fecha = DateTime.Today;      
            IQueryable<tmovimiento> Consulta;
            List<tmovimiento> Movimientos = new List<tmovimiento>();
            try
            {
                //Actulizo valores de los filtros
                if ((id != null && Sesion.Cuentas.Any(c => c.Id == id)) || id ==0)
                {
                    Sesion.CuentaFiltro = Convert.ToInt16(id);
                }
                if (rango == null) {
                    rango = Sesion.RangoFiltro;
                }
            // Filtro por fecha 1= Dia 2=Semana 3=Mes 4=Ano 5=Rango
            switch (rango){
                case 2:
                    {
                        // lastMonday is always the Monday before nextSunday.
                        // When date is a Sunday, lastMonday will be tomorrow.     
                        int offset = Fecha.DayOfWeek - DayOfWeek.Monday;
                        FechInicial = Fecha.AddDays(-offset);                      
                        FechFinal = Fecha.AddDays(7 - (int)Fecha.DayOfWeek).AddHours(23.9999);
                        ViewBag.Rango = "Semana Actual";
                        ViewBag.RangoFiltro = FechInicial.ToString("dd/MM/yyyy") + " hasta " + FechFinal.ToString("dd/MM/yyyy");
                        Consulta = db.tmovimientoes.Where(m => m.Id_Usuario == Sesion.Usuario.Id
                        && m.Fecha >= FechInicial && m.Fecha <= FechFinal).Include(c => c.tcuenta);                  
                                            break;
                    }
                case 3: {
                        ViewBag.Rango = "Mes Actual";
                        ViewBag.RangoFiltro = Fecha.Month + " del " + Fecha.Year;
                        Consulta = db.tmovimientoes.Where(m => m.Id_Usuario == Sesion.Usuario.Id                            
                            && m.Fecha.Month == Fecha.Month
                            && m.Fecha.Year == Fecha.Year).Include(c => c.tcuenta);
                        break;
                    }
                case 4:
                    {
                        ViewBag.Rango = "Año Actual";
                        ViewBag.RangoFiltro = Fecha.Year;
                        Consulta = db.tmovimientoes.Where(m => m.Id_Usuario == Sesion.Usuario.Id                        
                            && m.Fecha.Year == Fecha.Year).Include(c => c.tcuenta);                        
                            break;                         
                    }
                default:
                    {
                        ViewBag.Rango = "Movimientos del día ";
                        ViewBag.RangoFiltro = Fecha.ToString("dd/MM/yyyy");
                        rango = 1; // Se valida el parametro
                        Consulta = db.tmovimientoes.Where(m => m.Id_Usuario == Sesion.Usuario.Id
                            && m.Fecha.Day == Fecha.Day
                            && m.Fecha.Month == Fecha.Month
                            && m.Fecha.Year == Fecha.Year).Include(c => c.tcuenta);
                        break;
                    }
                }
                Sesion.RangoFiltro = Convert.ToInt16(rango);
                if (Sesion.CuentaFiltro == 0)
                {
                    Movimientos = Consulta.Where(m => m.Id_Usuario == Sesion.Usuario.Id).ToList();
                }
                else {
                    Movimientos = Consulta.Where(m => m.Id_Usuario == Sesion.Usuario.Id && m.Id_Cuenta == Sesion.CuentaFiltro).ToList();
                }
                ViewBag.Monedas = Movimientos.Select(c => c.tcuenta.tmoneda).Distinct();           
            }
            catch
            {
                AddMsgWeb("Error crítico al acceder a los datos", ToastType.Error);          
            }
            return View(Movimientos.OrderByDescending(c=> c.Id));
        }

        private void AddMsgWeb(string err, ToastType type)
        {
            TempData["msg"] += Notification.Show(err, position: Position.BottomRight, type: type, timeOut: 7000);
        }

        [HttpPost]
        public ActionResult CambioCuenta(FormCollection form)
        {
            string strDDLValue = form["ddl_cuenta"];

            return RedirectToAction("Index", "Inicio", new { id=strDDLValue});
        }
        // GET: Inicio/CambioRango
        /// <summary>
        /// Cambia el rango de los reportes en la applicacion 
        /// </summary>
        /// <param name="id">1=Dia, 2= Semana, 3= Mes, 4= Ano, 9 = Filtro por Rango</param>
        /// <returns>La ventana de Inicio</returns>
        public ActionResult CambioRango(int id)
        {
            return RedirectToAction("Index", "Inicio", new { id = Sesion.CuentaFiltro, rango=id});
        }
        // POST: Inico/SeleccionRango
        [HttpPost]
        public ActionResult SeleccionRango(FormCollection form)
        {
            return RedirectToAction("Index", "Inicio", new { id = Sesion.CuentaFiltro, rango = 9 });
        }

    }
}