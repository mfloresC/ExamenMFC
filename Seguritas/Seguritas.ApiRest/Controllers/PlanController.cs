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
    public class PlanController : ApiController
    {
        public async Task<List<PlanContrato>> ObtenerPlanes()
        {
            try
            {
                PlanDatos objDatos = new PlanDatos();
                var planResponse = await objDatos.ObtenerPlanes();
                return planResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<CoberturaContrato>> ObtenerCatalogoCoberturas()
        {
            try
            {
                PlanDatos objDatos = new PlanDatos();
                var planResponse = await objDatos.ObtenerCatalogoCoberturas();
                return planResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> RegistraPlan(HttpRequestMessage jsonTransaccion)
        {
            try
            {
                var request = jsonTransaccion.Content.ReadAsStringAsync().Result;
                bool planResponse = false;
                var objPlan = JsonConvert.DeserializeObject<PlanContrato>(request);
                PlanDatos objDatos = new PlanDatos();
                planResponse = await objDatos.GuardaActualizaPlan(objPlan);
                return planResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> EliminaPlan(HttpRequestMessage jsonTransaccion)
        {
            try
            {
                var request = jsonTransaccion.Content.ReadAsStringAsync().Result;
                bool planResponse = false;
                var objPlan = Convert.ToInt32(request);
                PlanDatos objDatos = new PlanDatos();
                planResponse = await objDatos.EliminaPlan(objPlan);
                return planResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
