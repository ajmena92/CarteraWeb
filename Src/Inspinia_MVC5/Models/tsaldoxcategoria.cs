//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebCartera.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tsaldoxcategoria
    {
        public int IdCategoria { get; set; }
        public int IdUsuario { get; set; }
        public int IdMoneda { get; set; }
        public Nullable<decimal> Saldo { get; set; }
    
        public virtual seguridadusuario seguridadusuario { get; set; }
        public virtual tcategoria tcategoria { get; set; }
        public virtual tmoneda tmoneda { get; set; }
    }
}