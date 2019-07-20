using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCartera.Models;
namespace WebCartera.Controllers
{
    public class InicioController : Controller
    {
        public Parametro sesion = Parametro.ObtenerSesionPagina();

        // GET: Inicio
        public ActionResult Index(int? cuenta, int? rango )
        {
            if (cuenta != null) {
                sesion.CuentaFiltro = Convert.ToInt16( cuenta);
            }
            if (rango != null)
            {
                sesion.RangoFiltro = Convert.ToInt16(rango);                
            }

            return View();
        }

    }
}