//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inspinia_MVC5.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tcuenta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tcuenta()
        {
            this.tmovimientoes = new HashSet<tmovimiento>();
        }
    
        public int Id { get; set; }
        public int Id_Usuario { get; set; }
        public int Id_Moneda { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public decimal SaldoActual { get; set; }
        public decimal SaldoAnterior { get; set; }
        public string Activo { get; set; }
    
        public virtual seguridadusuario seguridadusuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tmovimiento> tmovimientoes { get; set; }
        public virtual tmoneda tmoneda { get; set; }
    }
}
