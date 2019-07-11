using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WebCartera.Helpers;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebCartera.Models
{
    [Flags]
    public enum ResultLogueo
    {
        Logueo = 1,
        Invalido = 2,
        Desactivado = 3,
        Error = 4
    }

    [MetadataType(typeof(MetaDataUsuarios))]
    public partial class seguridadusuario
    {
        //[DataType(DataType.Password)]
        [Display(Name = "Confirme la contraseña")]
        [Compare("Clave", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarClave { get; set; }

        #region Metodos y Propiedades   

        public static ResultLogueo Login(string pUsuario, string pClave, bool pRecordar)
        {
            try
            {
                Cifrado Security = new Cifrado();
                CarteraEntities db = new CarteraEntities();

                //Esta variable almacena la clave cifrada pra crear la cokie con los valores ingresados del usuario
                string pass = Security.Encriptar(pClave);
                seguridadusuario Usuario = db.seguridadusuarios.Where(u => u.Email == pUsuario && u.Clave == pass).SingleOrDefault();
                if (Usuario != null)
                {
                    if (Usuario.Activo)
                    {
                        Parametro.CrearSesionPagina(Usuario);
                        if (pRecordar)
                        {
                            //Creo la cokie para mantener la session;
                            try
                            {
                                LoginViewModel UsuarioCookie = new LoginViewModel
                                {
                                    RememberMe = true,
                                    Email = Usuario.Email,
                                    Password = pClave
                                };
                                string User = JsonConvert.SerializeObject(UsuarioCookie);
                                User = Security.Encriptar(User);
                                HttpCookie cookie = new HttpCookie("SSLayerUser")
                                {
                                    Value = User
                                };
                                cookie.Expires.AddDays(10);
                                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                        return ResultLogueo.Logueo;
                    }
                    else
                    {
                        return ResultLogueo.Desactivado;
                    }
                }
                else
                {
                    return ResultLogueo.Invalido;
                }
            }
            catch (Exception)
            {
                return ResultLogueo.Error;
            }
        }
        #endregion
    }

    //public class ChangePasswordViewModel
    //{
    //    [Required]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Contraseña actual")]
    //    public string OldPassword { get; set; }

    //    [Required]
    //    [StringLength(30, ErrorMessage = "{0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Contraseña nueva")]
    //    public string NewPassword { get; set; }

    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirme la contraseña nueva")]
    //    [Compare("NewPassword", ErrorMessage = "La contraseña nueva y la contraseña de confirmación no coinciden.")]
    //    public string ConfirmPassword { get; set; }
    //}
}