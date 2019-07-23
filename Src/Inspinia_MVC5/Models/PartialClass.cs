using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;


namespace WebCartera.Models
{

    [MetadataType(typeof(MetaDatatcategoria))]
    public partial class tcategoria
    {
       
    }
    [MetadataType(typeof(MetaDatatmoneda))]
    public partial class tmoneda {
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