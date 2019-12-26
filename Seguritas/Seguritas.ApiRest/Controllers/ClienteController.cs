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
    public class ClienteController : ApiController
    {
        public async Task<List<ClienteContrato>> ObtenerClientes()
        {
            try
            {
                ClienteDatos objDatos = new ClienteDatos();
                var clienteResponse = await objDatos.ObtenerClientes();
                return clienteResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<PlanContrato>> ObtenerCatalogoPlan()
        {
            try
            {
                ClienteDatos objDatos = new ClienteDatos();
                var planResponse = await objDatos.ObtenerCatalogoPlan();
                return planResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> RegistraCliente(HttpRequestMessage jsonTransaccion)
        {
            try
            {
                var request = jsonTransaccion.Content.ReadAsStringAsync().Result;
                bool clienteResponse = false;
                var objCliente = JsonConvert.DeserializeObject<ClienteContrato>(request);
                ClienteDatos objDatos = new ClienteDatos();
                clienteResponse = await objDatos.GuardaActualizaCliente(objCliente);
                return clienteResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> EliminaCliente(HttpRequestMessage jsonTransaccion)
        {
            try
            {
                var request = jsonTransaccion.Content.ReadAsStringAsync().Result;
                bool clienteResponse = false;
                var objCliente = Convert.ToInt32(request);
                ClienteDatos objDatos = new ClienteDatos();
                clienteResponse = await objDatos.EliminaCliente(objCliente);
                return clienteResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
