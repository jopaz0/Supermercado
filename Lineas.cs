//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Supermercado
{
    using System;
    using System.Collections.Generic;
    
    public partial class Lineas
    {
        public int idLinea { get; set; }
        public int idVenta { get; set; }
        public int idArticulo { get; set; }
        public Nullable<int> cantidad { get; set; }
        public Nullable<double> precioUn { get; set; }
        public Nullable<double> monto { get; set; }
    
        public virtual Articulos Articulos { get; set; }
        public virtual Ventas Ventas { get; set; }
    }
}
