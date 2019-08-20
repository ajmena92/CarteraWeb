using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebCartera.Models
{
    public class Dashboard {
        public List<tcategoria> Categorias { get; set; }
        public List<tmovimiento> Movimientos { get; set; }
        public List<tcuenta> Cuentas { get; set; }
        public List<tmoneda> Monedas { get; set; }        
    }
    public class TipoCuenta
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Valor { get; set; } 

    }    
    public class Transferencia
    {  
        public int CuentaOrigen { get; set; }
        public int CuentaDestino { get; set; }
        public string Descripcion { get; set; }    
        public decimal Monto { get; set; }
        public virtual tcuenta tcuenta { get; set; }
    }
    public class ReporteFeha
    {
        [Display(Name = "Fecha Inicial")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FecInicial { get; set; }
        [Display(Name = "Fecha Final")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FecFinal{ get; set; }      
    }
}