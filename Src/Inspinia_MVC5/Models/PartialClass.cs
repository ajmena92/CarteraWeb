using System.ComponentModel.DataAnnotations;
using WebCartera.Helpers.OptionEnums;
namespace WebCartera.Models
{


    [MetadataType(typeof(MetaDatatmovimiento))]
    public partial class tmovimiento
    {
        public TipoMovimiento TipoMovimiento
        {
            get;set;
        }

        public System.DateTime Hora
        {
            get => Fecha;
            set
            {
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
        }   
    }