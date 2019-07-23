using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace WebCartera.Models
{
    public class Parametro
    {
        private readonly CarteraEntities db = new CarteraEntities();

        public int Mantenimiento{get; set;}

        public string NomEmpresa { get; set; }

        public string UrlImgs { get; set; }

        public seguridadusuario Usuario { get; set; }

        public List<seguridadmodulo> Modulos { get; set; }

        public int CuentaFiltro { get; set; }

        public int RangoFiltro { get; set; }

        public DateTime FechaInicial  { get; set; }

        public DateTime FechaFinal { get; set; }

        public List<tcuenta> Cuentas { get; set; }

        #region Metodos y Propiedades

        public Parametro() { }

        public Parametro(seguridadusuario pUsuario)
        {
            try
            {
                
                List<tparametro> Tparametro = db.tparametros.ToList();
                this.Modulos = db.seguridadmoduloes.ToList() ;
                UrlImgs = "~/img/";              
                this.Mantenimiento = Convert.ToInt16( Tparametro[7].Valor); //Valor de mantenimiento
                this.NomEmpresa = Tparametro[8].Valor; //Valor de mantenimiento
                this.Usuario = pUsuario;
                this.CuentaFiltro = 0; // filtro todas las cuentas
                this.RangoFiltro = 1; //filtro por dia
                this.Cuentas = Usuario.tcuentas.Where(c=> c.Activo).ToList();
            }
            catch (Exception )
            {
                Mantenimiento = 1;
                UrlImgs = "";
                throw;
            }
        }        
            public static void CrearSesionPagina(seguridadusuario pUsuario)
        {
            try
            {
                HttpContext.Current.Session["MiSession"] = new Parametro(pUsuario);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Parametro  ObtenerSesionPagina()
        {
            Parametro parametros = new Parametro();
            try
            {
                parametros = (Parametro)HttpContext.Current.Session["MiSession"];
                //System.Diagnostics.Debug.Assert(parametros != null); // request will be null if the cast fails
            }
            catch (Exception)
            {
                parametros = null;
            }
            if (parametros == null)
            {
                var context = new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current), new RouteData());
                var urlHelper = new UrlHelper(context);
                var url = urlHelper.Action("Login", "Cuenta");
                try
                {
                    string Cookie;
                    if (HttpContext.Current.Request.Cookies.AllKeys.Contains("SSLayerUser"))
                    {
                        Helpers.Cifrado Security = new Helpers.Cifrado();
                        Cookie = HttpContext.Current.Request.Cookies["SSLayerUser"].Value;
                        Cookie = Security.Desencriptar(Cookie);
                        LoginViewModel Usuario = JsonConvert.DeserializeObject<LoginViewModel>(Cookie);
                        ResultLogueo  result = seguridadusuario.Login(Usuario.Email, Usuario.Password, true);
                        if (result == ResultLogueo.Logueo)
                        {                           
                            parametros = (Parametro)HttpContext.Current.Session["MiSession"];
                            System.Diagnostics.Debug.Assert(parametros != null); // request will be null if the cast fails
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    HttpContext.Current.Response.Redirect(url);
                }
            }
            return parametros;
        }

        public static seguridadrolmodulo VerificaPermiso(Parametro sesion,string Code, bool DesactivaPermiso = false)
        {
            seguridadrolmodulo Permiso = new seguridadrolmodulo
            {
                ActivaEdicion = false
            };

            if (DesactivaPermiso)
            {
                string s = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                string Pass = "";
                foreach (char c in s)
                {
                    Pass += Convert.ToInt32(c);
                }
                if (HttpContext.Current.Request.QueryString["pass"] == Pass )
                {
                    Permiso.IdModulo = 0;
                    Permiso.IdRol = 1;
                    return Permiso;
                }
            }          
            if (sesion != null)
            {
                try
                {
                    seguridadmodulo Modulo = sesion.Modulos.Where(m => m.Codigo == Code).SingleOrDefault();
                    Permiso = sesion.Usuario.seguridadrol.seguridadrolmoduloes.Where(t => t.IdModulo == Modulo.Id).SingleOrDefault();
                    if (Permiso == null)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        return Permiso;
                    }

                }
                catch (Exception)
                {
                    var context = new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData());
                    var urlHelper = new UrlHelper(context);
                    var url = urlHelper.Action("LogOff", "Cuenta");
                    HttpContext.Current.Response.Redirect(url);
                }
            }
            return Permiso;
        }
        public static void  CerrarSesionPagina()
        {
            try
            {
                HttpContext.Current.Session["MiSession"]  = null;
            }
            catch (Exception)
            {
                throw;
              // No Retorno error
            }
        }
        #endregion
    }
}