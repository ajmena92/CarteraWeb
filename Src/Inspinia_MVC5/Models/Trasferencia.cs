using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCartera.Models
{
    public class Transferencia
    {  
        public int CuentaOrigen { get; set; }
        public int CuentaDestino { get; set; }
        public string Descripcion { get; set; }    
        public decimal Monto { get; set; }
        public virtual tcuenta tcuenta { get; set; }
    }
}