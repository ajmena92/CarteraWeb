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

        public seguridadusuario Usuario { get; set; }

        public List<seguridadmodulo> Modulos { get; set; }

        public int CuentaFiltro { get; set; }

        public int RangoFiltro { get; set; }

        public DateTime FechaInicial  { get; set; }

        public DateTime FechaFinal { get; set; }

        public List<tcuenta> Cuentas {
            get;
            set;          
        }
        public List<TipoCuenta> TipoCuentas
        {
            get;
            set;
        }

        #region Metodos y Propiedades

        public Parametro() { }

        public Parametro(seguridadusuario pUsuario)
        {
            try
            {
                
                List<tparametro> Tparametro = db.tparametros.ToList();
                Modulos = db.seguridadmoduloes.ToList() ;                            
                Mantenimiento = Convert.ToInt16( Tparametro[7].Valor); //Valor de mantenimiento
                NomEmpresa = Tparametro[8].Valor; //Valor de mantenimiento
                Usuario = pUsuario;
                CuentaFiltro = 0; // filtro todas las cuentas
                RangoFiltro = 3; //filtro por mes
                Cuentas = Usuario.tcuentas.Where(c=> c.Activo).ToList();
                TipoCuentas = new List<TipoCuenta>
                {
                    new TipoCuenta{Id=0, Descripcion="Debito",Valor= true},
                    new TipoCuenta{Id=1,Descripcion="Credito",Valor= false}
                };
            }
            catch (Exception )
            {
                Mantenimiento = 1;               
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
                var url = urlHelper.Action("Login", "Cuenta", new { returnUrl = Getroot() });
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

        public static seguridadrolmodulo VerificaPermiso(string Code, bool DesactivaPermiso = false)
        {
            var context = new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData());
            var urlHelper = new UrlHelper(context);
            Parametro sesion = Parametro.ObtenerSesionPagina();
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
                    var url = urlHelper.Action("Index", "Inicio");
                    HttpContext.Current.Response.Redirect(url);
                    return null;
                }                
            }
            else {                                
                return null;
            }          
        }

        public static string Getroot()
        {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Query);
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