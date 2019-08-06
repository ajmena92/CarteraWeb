﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

using System.Globalization;
using System.Web.Mvc;

namespace WebCartera.Models
{
    
    public class MetaDatatreportecartera
    {    
        [Display(Name = "Descripción")]
        public string Descripcion;
    }
    public class MetaDatatmovimiento
    {            
        [Required]
        [StringLength(150, ErrorMessage = "{0} debe tener al menos {2} caracteres de longitud y maximo {1}.", MinimumLength = 0)]
        [Display(Name = "Descripción")]
        public string Descripcion;

        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime Fecha;      
        [Required]
        [Range(-9999999999.99, 9999999999.99, ErrorMessage ="Valor excede el maximo de digitos permitido")]
        public decimal Monto;
    }
        public class MetaDatatcuenta
    {
        [Required]
        [StringLength(45, ErrorMessage = "{0} debe tener al menos {2} caracteres de longitud y maximo {1}.", MinimumLength = 4)]
        public string Nombre;
        [Required]
        [StringLength(45, ErrorMessage = "{0} debe tener al menos {2} caracteres de longitud y maximo {1}.", MinimumLength = 4)]
        [Display(Name = "Descripción")]
        public string Descripcion;
        [Required]
        public string Imagen { get; set; }
        [Required]
        [Display(Name = "Saldo")]
        [Range(-9999999999.99, 9999999999.99)]
        public decimal SaldoActual;         
        public decimal SaldoAnterior;
        [Display(Name = "Estado")]
        public bool Activo;
        [Required]
        public int Id_Moneda;
    }

    public class MetaDatatmoneda
    {
        [Required]
        [StringLength(45, ErrorMessage = "{0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 4)]
        [Display(Name = "Moneda")]
        public string Descripcion;
        [Required]
        [StringLength(1, ErrorMessage = "{0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 1)]
        [Display(Name = "Símbolo")]
        public string Simbolo;        
        [Display(Name = "Estado")]
        public bool Activo;
    }
    public class MetaDatatcategoria
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 4)]        
        public string Nombre;
        [Required]
        public int Tipo;
        [Required]
        public String Imagen;
        [Display(Name = "Estado")]
        public bool Activo;

    }

    public class MetaDataUsuarios
    {
        [Required]
        [StringLength(30, ErrorMessage = "{0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 4)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electrónico")]
        public string Email;

        [Required]
        [StringLength(50, ErrorMessage = "{0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 4)]
        [Display(Name = "Nombre")]
        public string NomUsuario;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, ErrorMessage = "{0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 8)]
        [Display(Name = "Contraseña")]
        public string Clave;

        [Required]
        [Display(Name = "Tipo Cuenta")]
        public int IdRol;

        [Required]
        [Display(Name = "Estado")]
        public int Activo;
    }

    public class MetaDataPermisos
    {
        [Column(Order = 0), Key]
        [Required]
        [Display(Name = "Rol")]
        public int IdRol;

        [Column(Order = 0), Key]
        [Required]
        [Display(Name = "Permiso")]
        public int CodModulo;

        [Display(Name = "Activa Edición")]
        public bool ActivaEdicion;
    }

    public class MetaDataRoles
    {
        [Display(Name = "Código  de Rol")]
        [Required]
        public int Id;

        [Display(Name = "Tipo de Rol")]
        [Required]
        public string Descripcion;

        [Display(Name = "Caduca"), Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime FechaCreacion;

        [Display(Name = "Cantidad de Permisos")]
        public int CantPermisos;
    }
}