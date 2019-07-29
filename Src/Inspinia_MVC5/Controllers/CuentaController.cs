﻿using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


using WebCartera.Models;
using WebCartera.Helpers;
using System.Web.Routing;
using PagedList;
using WebCartera.OptionEnums;
using EntityState = System.Data.Entity.EntityState;
using WebCartera;

namespace MenuCenter.Controllers
{
    public class CuentaController : Controller
    {
        private readonly CarteraEntities db = new CarteraEntities();
        private readonly Cifrado Security = new Cifrado();

        // GET: /Account/Login        
        public ActionResult Login(string returnUrl)
        {
            if (returnUrl == null)
            {
                returnUrl = Url.Action("Index", "Inicio", this.Request.Url.Scheme);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            switch (seguridadusuario.Login(model.Email, model.Password, model.RememberMe))
            {

                case ResultLogueo.Logueo:                    
                    AddErrors("Bienvenido " + model.Email);
                    return RedirectToLocal(returnUrl);
                case ResultLogueo.Invalido:
                    ModelState.AddModelError ("ModelErr", "Intento de inicio de sesión no válido.");
                    return View(model);
                case ResultLogueo.Desactivado:
                    ModelState.AddModelError("ModelErr", "Usuario inactivo, sesión cancelada.");
                    return View(model);
                case ResultLogueo.Error:
                    ModelState.AddModelError("ModelErr", "Error al intentar conectar con el repositorio de datos, por favor comuniquese con el departamento de soporte.");
                    return View(model);
                default:
                    ModelState.AddModelError("ModelErr", "Intento de inicio de sesión no válido.");
                    return View(model);
            }
        }

        // GET: /Account/ResetPassword        
        public ActionResult ResetPassword()
        {
            Parametro sesion = Parametro.ObtenerSesionPagina();
            ResetPasswordViewModel reset = new ResetPasswordViewModel();
            reset.CodUsuario = sesion.Usuario.Id;
            reset.Email = sesion.Usuario.Email;
            return View(reset);
        }

        //
        // POST: /Account/ResetPassword

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            seguridadusuario user = db.seguridadusuarios.Find(model.CodUsuario);
            if (user == null)
            {
                // No revelar que el usuario no existe
                return RedirectToAction("ResetPasswordConfirmation", "Cuenta");
            }
            if (user.Clave == Security.Encriptar(model.OldPassword))
            {
                user.Clave = Security.Encriptar(model.Password);
                user.ConfirmarClave = user.Clave;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ResetPasswordConfirmation", "Cuenta");
            }
            else
            {
                ModelState.AddModelError("ModelErr", "Intento de inicio de sesión no válido.");
            }
            return View(model);
        }

        // GET: /Account/ResetPasswordConfirmation        
        public ActionResult ResetPasswordConfirmation()
        {
            Parametro.CerrarSesionPagina();
            return View();
        }

        // GET: Cuenta
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {         
            seguridadrolmodulo permiso = Parametro.VerificaPermiso("ADM");
            Parametro sesion = Parametro.ObtenerSesionPagina();
            ViewBag.Edit = permiso.ActivaEdicion;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.EmailSortParm = string.IsNullOrEmpty(sortOrder) ? "email_desc" : "";
            ViewBag.NomSortParm = sortOrder == "nom" ? "nom_desc" : "nom";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            ViewBag.currentFilter = searchString;
            IQueryable<seguridadusuario> usuarios;


            if (!string.IsNullOrEmpty(searchString))
            {
                usuarios = db.seguridadusuarios.Where(p => p.NomUsuario.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                usuarios = db.seguridadusuarios;
            }
            switch (sortOrder)
            {
                case "email_desc":
                    usuarios = usuarios.OrderByDescending(s => s.Email).ThenBy(s => s.NomUsuario);
                    break;
                case "nom":
                    usuarios = usuarios.OrderBy(s => s.NomUsuario).ThenBy(s => s.Email);
                    break;
                case "nom_desc":
                    usuarios = usuarios.OrderByDescending(s => s.NomUsuario).ThenBy(s => s.Email);
                    break;
                default:  // Presentacion ascending 
                    usuarios = usuarios.OrderBy(s => s.Email).ThenBy(s => s.NomUsuario);
                    break;
            }
            const int pageSize = 6;
            int pageNumber = (page ?? 1);
            if (Request.IsAjaxRequest())
            {
                return PartialView(usuarios.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View(usuarios.ToPagedList(pageNumber, pageSize));
            }

        }

        // GET: Cuenta/Detalles/5                                       
        public ActionResult Detalles(int? id)
        {
            
            seguridadrolmodulo permiso = Parametro.VerificaPermiso( "USE");
            Parametro sesion = Parametro.ObtenerSesionPagina();
            seguridadusuario usuario = db.seguridadusuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return  PartialView(usuario);
            }
            else
            {
                return View(usuario);
            }

        }


        // GET: Cuenta/Create
        public ActionResult Create()
        {
            
            seguridadrolmodulo permiso = Parametro.VerificaPermiso("ADM");
            Parametro sesion = Parametro.ObtenerSesionPagina();
            VerificaEdit(permiso);
            ViewBag.IdRol = new SelectList(db.seguridadrols, "Id", "Descripcion");
            return PartialView("_Create");
        }

        // POST: Cuenta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,NomUsuario,FechaCreacion,Clave,ConfirmarClave,IdRol,Activo")] seguridadusuario usuario)
        {
            string Olpas = usuario.Clave;
            if (ModelState.IsValid)
            {
                try
                {
                    bool has = db.seguridadusuarios.Any(p => p.Email == usuario.Email);
                    if (has)
                    {
                        ModelState.AddModelError("ModelErr", "El correo ingresado ya esta registrado en nuestro sistema. Este dato no se puede duplicar.");
                    }
                    else
                    {                        
                        usuario.FechaCreacion = DateTime.Now;
                        usuario.NomUsuario = usuario.NomUsuario.ToUpper();
                        usuario.Clave = Security.Encriptar(usuario.Clave);
                        usuario.ConfirmarClave = usuario.Clave;
                        db.seguridadusuarios.Add(usuario);
                        db.SaveChanges();
                        string url = Url.Action("Index", "Cuenta");
                        return Json(new { success = true, url });
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ModelErr", ex.Message);
                }
                usuario.ClaveAnterior = usuario.Clave;                
            }
            ViewBag.IdRol = new SelectList(db.seguridadrols, "Id", "Descripcion", usuario.IdRol);
            return PartialView("_Create", usuario);
        }

        // GET: Cuenta/Edit/5
        public ActionResult Edit(int? id)
        {            
            seguridadrolmodulo permiso = Parametro.VerificaPermiso("USE");
            Parametro sesion = Parametro.ObtenerSesionPagina();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            seguridadusuario Usuario = db.seguridadusuarios.Find(id);
            if (Usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdRol = new SelectList(db.seguridadrols, "Id", "Descripcion", Usuario.IdRol);
            ViewBag.Editar = permiso.ActivaEdicion;
            Usuario.Clave = Usuario.Clave;
            Usuario.ConfirmarClave = Usuario.Clave;
            Usuario.ClaveAnterior = Usuario.Clave;
            return PartialView("_Edit", Usuario);
        }

        // POST: Cuenta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,NomUsuario,Clave,ConfirmarClave,ClaveAnterior,IdRol,Activo")] seguridadusuario usuario)
        {            
            seguridadrolmodulo permiso = Parametro.VerificaPermiso("USE");
            Parametro sesion = Parametro.ObtenerSesionPagina();
            if (ModelState.IsValid)
            {
                try
                {

                    bool has = db.seguridadusuarios.Any(p => p.Email == usuario.Email && p.Id != usuario.Id);
                    if (has)
                    {
                        ModelState.AddModelError("ModelErr", "El correo ingresado ya esta registrado en nuestro sistema. Este dato no se puede duplicar.");
                    }
                    else
                    {                  
                        usuario.NomUsuario = usuario.NomUsuario.ToUpper();
                        if (usuario.Clave != "secret123")
                        {
                            usuario.Clave = Security.Encriptar(usuario.Clave);                         
                        }
                        else
                        {
                            usuario.Clave = usuario.ClaveAnterior;
                        }
                        usuario.ConfirmarClave = usuario.Clave; //Evita error en la valiadacion.
                        db.Entry(usuario).State = EntityState.Modified;
                        db.SaveChanges();
                        string url = Url.Action("Detalles", "Cuenta", new { id = usuario.Id });
                        return Json(new { success = true, url });
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ModelErr", ex.Message);
                }
            }
            ViewBag.IdRol = new SelectList(db.seguridadrols, "Id", "Descripcion", usuario.IdRol);
            ViewBag.Editar = permiso.ActivaEdicion;
            return PartialView("_Edit", usuario);
        }

        public ActionResult Delete(int? id)
        {            
            seguridadrolmodulo permiso = Parametro.VerificaPermiso("USE");
            Parametro sesion = Parametro.ObtenerSesionPagina();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            seguridadusuario seguridadUsuario = db.seguridadusuarios.Find(id);
            if (seguridadUsuario == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", seguridadUsuario);
        }

        // POST: Empaques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            seguridadusuario usuario = db.seguridadusuarios.Find(id);
            try
            {
                string url;
                Parametro sesion = Parametro.ObtenerSesionPagina();
                usuario.Activo = false;
                usuario.ConfirmarClave = usuario.Clave;
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                if (sesion.Usuario.Id == id)
                {
                    url = Url.Action("LogOff", "Cuenta");
                    return Json(new { success = true, redirect = true, url });
                }
                else
                {
                     url = Url.Action("Index", "Cuenta");
                    return Json(new { success = true, url });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ModelErr", ex.Message);
            }
            return PartialView("_Delete", usuario);
        }
        //Account/LogOff
        public ActionResult LogOff()
        {
            try
            {

                Parametro.CerrarSesionPagina();
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["SSLayerUser"];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                }

            }
            catch (Exception)
            {
                throw;
             }
            return RedirectToAction("Login", "Cuenta");
        }

        public void VerificaEdit(seguridadrolmodulo permiso)
        {
            if (!permiso.ActivaEdicion)
            {
                var context = new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current), new RouteData());
                var urlHelper = new UrlHelper(context);
                var url = urlHelper.Action("Index", "Cuenta", new { Logout = 1 });
                Response.Redirect(url);
            }
        }



        #region Aplicaciones auxiliares
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private void AddErrors(string err)
        {
            TempData["msg"] += Notification.Show(err, position: Position.BottomRight, type: ToastType.Info, timeOut: 7000);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Inicio");
        }
        #endregion

    }
}
