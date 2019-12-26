using Newtonsoft.Json;
using Seguritas.Auxiliar;
using Seguritas.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Seguritas.ApiRest.Controllers
{
    public class UsuarioController : ApiController
    {
        public async Task<bool> RegistraUsuario(HttpRequestMessage jsonTransaccion)
        {
            try
            {
                var request = jsonTransaccion.Content.ReadAsStringAsync().Result;
                bool usuarioResponse = false;
                var objUsuario = JsonConvert.DeserializeObject<UsuarioContrato>(request);
                UsuarioDatos objDatos = new UsuarioDatos();
                usuarioResponse = await objDatos.GuardaActualizaUsuario(objUsuario);
                return usuarioResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<UsuarioContrato> FirmarUsuario(HttpRequestMessage jsonTransaccion)
        {
            try
            {
                var request = jsonTransaccion.Content.ReadAsStringAsync().Result;
                var objUsuario = JsonConvert.DeserializeObject<UsuarioContrato>(request);
                UsuarioDatos objDatos = new UsuarioDatos();
                var usuarioResponse = await objDatos.FirmarUsuario(objUsuario);
                return usuarioResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

//objRespuesta.Content = new ObjectContent(typeof(object), objUsuario, GlobalConfiguration.Configuration.Formatters.XmlFormatter);
//IEnumerable<string> header = jsonTransaccion.Headers.GetValues("metodo");
//string metodo = header.FirstOrDefault().ToString();