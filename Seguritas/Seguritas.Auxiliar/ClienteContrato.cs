using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Seguritas.Auxiliar
{
    [DataContract]
    public class ClienteContrato
    {
        [DataMember]
        public int scCId { get; set; }

        [DataMember]
        public string scCNombre { get; set; }

        [DataMember]
        public DateTime scCFechaMod { get; set; }

        [DataMember]
        public List<int> claveP { get; set; }

        [DataMember]
        public string nombreP { get; set; }
    }
}
