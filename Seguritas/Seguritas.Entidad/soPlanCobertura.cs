//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Seguritas.Entidad
{
    using System;
    using System.Collections.Generic;
    
    public partial class soPlanCobertura
    {
        public int soPCoId { get; set; }
        public int soPCoPId { get; set; }
        public int soPCoCoId { get; set; }
        public bool soPCoEstatus { get; set; }
        public System.DateTime soPCoFechaMod { get; set; }
    
        public virtual scCobertura scCobertura { get; set; }
        public virtual scPlan scPlan { get; set; }
    }
}
