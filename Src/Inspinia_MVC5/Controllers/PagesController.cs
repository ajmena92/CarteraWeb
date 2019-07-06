using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebCartera.Models;
using WebCartera.Helpers;
using System.Web.Routing;

namespace WebCartera.Controllers
{
    public class PagesController : Controller
    {

        private readonly CarteraEntities db = new CarteraEntities();
        private readonly Cifrado Security = new Cifrado();

     
     
        public ActionResult NotFoundError()
        {
            return View();
        }

        public ActionResult InternalServerError()
        {
            return View();
        }

  
   

    }
}