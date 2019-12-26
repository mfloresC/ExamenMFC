using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Seguritas.Auxiliar
{
    [DataContract]
    public class CoberturaContrato
    {
        [DataMember]
        public int scCoId { get; set; }

        [DataMember]
        public string scCoNombre { get; set; }

        [DataMember]
        public string scCobSuma { get; set; }

        [DataMember]
        public DateTime scCoFechaMod { get; set; }
    }
}
