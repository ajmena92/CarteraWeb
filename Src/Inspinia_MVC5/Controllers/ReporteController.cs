using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;

using WebCartera.Models;
using WebCartera.OptionEnums;

namespace WebCartera.Controllers
{
    public class ReporteController : Controller
    {
        private readonly CarteraEntities db = new CarteraEntities();
        public Parametro Sesion = Parametro.ObtenerSesionPagina();

        public ActionResult Cartera()
        {
            try
            {
                List<treportecartera> cartera = db.treportecarteras.ToList();        
                return View(cartera);
            }
            catch
            {
                AddMsgWeb("Error al acceder a los datos", ToastType.Error);
                return RedirectToAction("Index", "Inicio");
            }
        }

        private void AddMsgWeb(string err, ToastType type)
        {
            TempData["msg"] += Notification.Show(err, position: Position.BottomRight, type: type, timeOut: 7000);
        }
    }
}