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
    public class CoberturaController : ApiController
    {
        public async Task<List<CoberturaContrato>> ObtenerCoberturas()
        {
            try
            {
                CoberturaDatos objDatos = new CoberturaDatos();
                var coberturaResponse = await objDatos.ObtenerCoberturas();
                return coberturaResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> RegistraCobertura(HttpRequestMessage jsonTransaccion)
        {
            try
            {
                var request = jsonTransaccion.Content.ReadAsStringAsync().Result;
                bool coberturaResponse = false;
                var objCobertura = JsonConvert.DeserializeObject<CoberturaContrato>(request);
                CoberturaDatos objDatos = new CoberturaDatos();
                coberturaResponse = await objDatos.GuardaActualizaCobertura(objCobertura);
                return coberturaResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> EliminaCobertura(HttpRequestMessage jsonTransaccion)
        {
            try
            {
                var request = jsonTransaccion.Content.ReadAsStringAsync().Result;
                bool coberturaResponse = false;
                var objCobertura = Convert.ToInt32(request);
                CoberturaDatos objDatos = new CoberturaDatos();
                coberturaResponse = await objDatos.EliminaCobertura(objCobertura);
                return coberturaResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
