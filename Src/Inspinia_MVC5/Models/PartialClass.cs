using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection;

namespace WebCartera.Models
{
    //
    public enum TipoMovimiento
    {
        //0= Anulacion, 1 = Credito, 2= Debito
        [Description("0")]
        Anulacion,

        [Description("1")]
        Credito,

        [AmbientValue(2)]
        Debito
    }


    [MetadataType(typeof(MetaDatatmovimiento))]
    public partial class tmovimiento
    {
        public TipoMovimiento TipoMovimiento
        {
            get
            {
                return TipoMovimiento;
            }

            set => Tipo = IntValue(TipoMovimiento);

        }
        private static int IntValue(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return Convert.ToInt16(attributes[0].Description);
            }
            else
            {
                return Convert.ToInt16(value);
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