using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Seguritas.Auxiliar
{
    [DataContract]
    public class PlanContrato
    {
        [DataMember]
        public int scPId { get; set; }
        [DataMember]
        public string scPNombre { get; set; }
        [DataMember]
        public string scPClave { get; set; }
        [DataMember]
        public DateTime scPFechaMod { get; set; }
        [DataMember]
        public List<int> claveC { get; set; }
        [DataMember]
        public string nombreC { get; set; }
    }
}
