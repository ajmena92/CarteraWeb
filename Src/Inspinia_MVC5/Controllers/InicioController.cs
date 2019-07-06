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
        public ActionResult Index()
        {
            return View();
        }

    }
}