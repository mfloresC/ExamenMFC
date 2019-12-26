using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Seguritas.Auxiliar
{
    [DataContract]
    public class UsuarioContrato
    {
        [DataMember]
        public int scUId { get; set; }

        [DataMember]
        public string scUUsuario { get; set; }

        [DataMember]
        public string scUPassword { get; set; }

        [DataMember]
        public string scUNombre { get; set; }

    }
}
