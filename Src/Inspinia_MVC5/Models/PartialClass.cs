﻿using System.ComponentModel.DataAnnotations;
using WebCartera.Helpers.OptionEnums;
namespace WebCartera.Models
{

    [MetadataType(typeof(MetaDatatreportecartera))]
    public partial class treportecartera {
        public string Valor_Format
        {
            get
            {
                return string.Format("{0:0,0.00}", Valor);
            }
        }
    }
    [MetadataType(typeof(MetaDatatmovimiento))]
    public partial class tmovimiento
    {
        public TipoMovimiento TipoMovimiento
        {
            get; set;
        }    
        public decimal MontoAnterior {
            get; set;
        }
        public string Monto_Format
        {
            get
            {
                return string.Format("{0:0,0.00}", Monto);
            }
        }
    }
        [MetadataType(typeof(MetaDatatcategoria))]
        public partial class tcategoria
        {

        }
        [MetadataType(typeof(MetaDatatmoneda))]
        public partial class tmoneda
        {
        }
    [MetadataType(typeof(MetaDatatcuenta))]
    public partial class tcuenta
    {
        public string SaldoActual_Format
        {
            get
            {
                return string.Format("{0:0,0.00}", SaldoActual);
            }
        }        
        [Required(ErrorMessage = "Porfavor sellecione el tipo de cuenta")]
        [Range(0, int.MaxValue)]
        [Display(Name ="Tipo Cuenta")]
        public int Id_TipoCuenta { get; set; }
        public virtual TipoCuenta TipoCuentas { get; set; }
     }   
    }