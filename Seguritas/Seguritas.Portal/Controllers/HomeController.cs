using Newtonsoft.Json;
using RestSharp;
using Seguritas.Auxiliar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Seguritas.Portal.Controllers
{
    public class HomeController : Controller
    {
        public static string apiURl = "http://localhost/Seguritas.Servicio/api/Usuario";
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult RegistraUsuario(string jsonTransaccion) 
        {
            try 
            {
                JsonRespond objRespuesta = new JsonRespond();
                var clienteRWS = new RestClient(apiURl);
                var request = new RestRequest("RegistraUsuario", Method.POST);
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

        public JsonResult FirmarUsuario(string jsonTransaccion)
        {
            try
            {
                JsonRespond objRespuesta = new JsonRespond();
                var clienteRWS = new RestClient(apiURl);
                var request = new RestRequest("FirmarUsuario", Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", jsonTransaccion, ParameterType.RequestBody);
                clienteRWS.ExecuteAsync(request, (response) =>
                {
                    try
                    {
                        var usuarioResponse = JsonConvert.DeserializeObject<UsuarioContrato>(response.Content);
                        if (usuarioResponse != null)
                        {
                            Session["usuario"] = usuarioResponse;
                            objRespuesta.mensaje = "Allow";
                            objRespuesta.respuesta = "S/M";
                        }
                        else
                        {
                            objRespuesta.mensaje = "Error";
                            objRespuesta.respuesta = "No se encontraron registros, con las credenciales proporcionadas.";
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