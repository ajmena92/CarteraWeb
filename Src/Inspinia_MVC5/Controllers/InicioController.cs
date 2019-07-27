using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCartera.Models;
using WebCartera.OptionEnums;
namespace WebCartera.Controllers
{
    public class InicioController : Controller
    {
        public Parametro Sesion = Parametro.ObtenerSesionPagina();

        // GET: Inicio
        public ActionResult Index(int? id, int? rango )
        {
           if (id != null) {
                Sesion.CuentaFiltro = Convert.ToInt16(id);
            }
            if (rango != null)
            {
                Sesion.RangoFiltro = Convert.ToInt16(rango);                
            }
            
            return View();
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