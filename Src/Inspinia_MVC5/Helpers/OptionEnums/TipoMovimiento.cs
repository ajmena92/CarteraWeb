using System.ComponentModel;

namespace WebCartera.Helpers.OptionEnums
{
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

}