using Newtonsoft.Json;
using RestSharp;
using Seguritas.Auxiliar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Seguritas.Portal.Controllers
{
    public class ClienteController : Controller
    {

        public static string apiURl = "http://localhost/Seguritas.Servicio/api/Cliente";
        public ActionResult Page()
        {
            bool terminarP1 = false;
            bool terminarP2= false;
            JsonRespond objRespuesta = new JsonRespond();
            var clienteRWS = new RestClient(apiURl);
            var request = new RestRequest("ObtenerClientes", Method.POST);
            clienteRWS.ExecuteAsync(request, (response) =>
            {
                try
                {
                    List<ClienteContrato> clienteResponse = JsonConvert.DeserializeObject<List<ClienteContrato>>(response.Content);
                    if (clienteResponse.Count() > 0)
                    {
                        ViewBag.lstClientes = clienteResponse;
                        objRespuesta.mensaje = "OK";
                    }
                    else
                    {
                        objRespuesta.mensaje = "Info";
                        objRespuesta.respuesta = "No se encontraron Resultados.";
                    }
                    terminarP1 = true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            });

            var cliente = new RestClient(apiURl);
            request = new RestRequest("ObtenerCatalogoPlan", Method.POST);
            cliente.ExecuteAsync(request, (response) =>
            {
                try
                {
                    List<PlanContrato> planResponse = JsonConvert.DeserializeObject<List<PlanContrato>>(response.Content);
                    if (planResponse.Count() > 0)
                    {
                        ViewBag.lstPlan = planResponse;
                        objRespuesta.mensaje = "OK";
                    }
                    else
                    {
                        objRespuesta.mensaje = "Info";
                        objRespuesta.respuesta = "No se encontraron Resultados.";
                    }
                    terminarP2 = true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            });

            while (terminarP1 == false && terminarP2 == false)
            {
                Task.Delay(1);
            }
            Thread.Sleep(2000);   
            return View();
        }

        public JsonResult RegistraCliente(string jsonTransaccion)
        {
            try
            {
                JsonRespond objRespuesta = new JsonRespond();
                var clienteRWS = new RestClient(apiURl);
                var request = new RestRequest("RegistraCliente", Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", jsonTransaccion, ParameterType.RequestBody);
                clienteRWS.ExecuteAsync(request, (response) =>
                {
                    try
                    {
                        bool usuarioResponse = bool.Parse(response.Content);
                        if (usuarioResponse)
                        {
                            objRespuesta.mensaje = "OK";
                            objRespuesta.respuesta = "Transaccion realizada con éxito";
                        }
                        else
                        {
                            objRespuesta.mensaje = "Error";
                            objRespuesta.respuesta = "Transaccion fallida";
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                });

                while (objRespuesta.respuesta == null)
                {
                    Task.Delay(1);
                }
                return Json(objRespuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { respuesta = "Error", mensaje = e.InnerException }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult EliminaCliente(int cliente)
        {
            try
            {
                JsonRespond objRespuesta = new JsonRespond();
                var clienteRWS = new RestClient(apiURl);
                var request = new RestRequest("EliminaCliente", Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", cliente, ParameterType.RequestBody);
                clienteRWS.ExecuteAsync(request, (response) =>
                {
                    try
                    {
                        bool usuarioResponse = bool.Parse(response.Content);
                        if (usuarioResponse)
                        {
                            objRespuesta.mensaje = "OK";
                            objRespuesta.respuesta = "Transaccion realizada con éxito";
                        }
                        else
                        {
                            objRespuesta.mensaje = "Error";
                            objRespuesta.respuesta = "Transaccion fallida";
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                });

                while (objRespuesta.respuesta == null)
                {
                    Task.Delay(1);
                }
                return Json(objRespuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { respuesta = "Error", mensaje = e.InnerException }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
