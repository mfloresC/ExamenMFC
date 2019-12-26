using Seguritas.Auxiliar;
using Seguritas.Entidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seguritas.Datos
{
    public class PlanDatos
    {
        private SeguritasEntities dataContexto = new SeguritasEntities();
        private DbConnection _storeConnection;
        private DbTransaction _transaction;

        #region Constructor
        public PlanDatos()
        {
            try
            {
                _storeConnection = dataContexto.Database.Connection;
                _storeConnection.Open();
                _transaction = _storeConnection.BeginTransaction();
            }
            catch (Exception e)
            {
                CierraConexion();
                throw e;
            }
        }
        #endregion

        #region Cierra Conexion
        public void CierraConexion()
        {
            if (_storeConnection != null)
            {
                if (_storeConnection.State != ConnectionState.Closed)
                {
                    _storeConnection.Close();
                    _storeConnection.Dispose();
                    _storeConnection = null;
                }
            }

            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (dataContexto != null)
            {
                dataContexto.Dispose();
                dataContexto = null;
            }
        }
        #endregion

        public async Task<bool> GuardaActualizaPlan(PlanContrato objPlan)
        {
            int idPlan = 0;
            bool res = false;
            using (dataContexto)
            {
                try
                {
                    dataContexto.Database.UseTransaction(_transaction);
                    idPlan = objPlan.scPId;

                    var planFound = (from p in dataContexto.scPlan
                                     where p.scPId == idPlan
                                     select p).ToList().FirstOrDefault();

                    //Coberturas Ingresadas
                    //var coberturasFound = (from co in dataContexto.scCobertura
                    //                       where objPlan.lstCOberturas.All(c => co.scCoNombre.Contains(c.scCoNombre))
                    //                       select co).ToList();

                    var coberturasFound = (from co in dataContexto.scCobertura
                                           select co).ToList().Where(co => objPlan.claveC.Contains(co.scCoId));

                    //Coberturas Existentes
                    var planCoberturaFound = (from pc in dataContexto.soPlanCobertura
                                            where pc.soPCoPId == idPlan
                                            select pc).ToList();

                    if (planFound == null && planCoberturaFound.Count() == 0)
                    {
                        scPlan entidadPlan = new scPlan();
                        entidadPlan.scPNombre = objPlan.scPNombre;
                        entidadPlan.scPFechaMod = DateTime.Now;
                        dataContexto.scPlan.Add(entidadPlan);
                        dataContexto.SaveChanges();
                        objPlan.scPId = entidadPlan.scPId;

                        foreach (var cobertura in coberturasFound)
                        {
                            soPlanCobertura entidadPlanCobertura = new soPlanCobertura();
                            entidadPlanCobertura.soPCoPId = entidadPlan.scPId;
                            entidadPlanCobertura.soPCoCoId = cobertura.scCoId;
                            entidadPlanCobertura.soPCoEstatus = true;
                            entidadPlanCobertura.soPCoFechaMod = DateTime.Now;
                            dataContexto.soPlanCobertura.Add(entidadPlanCobertura);
                            dataContexto.SaveChanges();
                        };
                    }
                    else
                    {
                        planFound.scPNombre = objPlan.scPNombre;
                        planFound.scPFechaMod = DateTime.Now;
                        dataContexto.SaveChanges();

                        var coberturasNuevas = coberturasFound.Where(nc => !planCoberturaFound.Any(vc => nc.scCoId == vc.soPCoCoId));
                        var coberturasEliminar = planCoberturaFound.Where(vc => !coberturasFound.Any(nc => vc.soPCoCoId == nc.scCoId));

                        foreach (var nueva in coberturasNuevas)
                        {
                            soPlanCobertura entidadPlanCobertura = new soPlanCobertura();
                            entidadPlanCobertura.scPlan = planFound;
                            entidadPlanCobertura.scCobertura = nueva;
                            entidadPlanCobertura.soPCoFechaMod = DateTime.Now;
                            dataContexto.soPlanCobertura.Add(entidadPlanCobertura);
                            dataContexto.SaveChanges();
                        }

                        foreach (var vieja in coberturasEliminar)
                        {
                            var modificar = planCoberturaFound.Where(cv => cv.soPCoCoId == vieja.soPCoCoId).FirstOrDefault();
                            modificar.soPCoEstatus = false;
                            dataContexto.SaveChanges();
                        }
                    }

                    _transaction.Commit();
                    res = true;
                }
                catch (Exception e)
                {
                    _transaction.Rollback();
                    res = false;
                }
                finally
                {
                    CierraConexion();
                }
            }
            return res;
        }

        public async Task<bool> EliminaPlan(int idPlan)
        {
            bool resultado = false;
            using (dataContexto)
            {
                try
                {
                    dataContexto.Database.UseTransaction(_transaction);

                    var planFound = (from p in dataContexto.scPlan
                                        where p.scPId == idPlan
                                        select p).ToList().FirstOrDefault();

                    var planCoberturaFound = (from pc in dataContexto.soPlanCobertura
                                            where pc.soPCoPId == idPlan
                                            select pc).ToList();

                    if (planFound != null && planCoberturaFound != null)
                    {
                        planCoberturaFound.ForEach(elemento => {
                            elemento.soPCoEstatus = false;
                        });
                        dataContexto.SaveChanges();
                    }
                    _transaction.Commit();
                    resultado = true;
                }
                catch (Exception e)
                {
                    _transaction.Rollback();
                    resultado = false;
                }
                finally
                {
                    CierraConexion();
                }
            }
            return resultado;
        }

        public async Task<List<PlanContrato>> ObtenerPlanes()
        {
            using (dataContexto)
            {
                try
                {
                    dataContexto.Database.UseTransaction(_transaction);

                    return (from pc in dataContexto.soPlanCobertura
                            where pc.soPCoEstatus == true
                            select new PlanContrato
                            {
                                scPId = pc.soPCoPId,
                                scPNombre = pc.scPlan.scPNombre,
                                nombreC = pc.scCobertura.scCoNombre

                            }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    CierraConexion();
                }
            }
        }

        public async Task<List<CoberturaContrato>> ObtenerCatalogoCoberturas()
        {
            using (dataContexto)
            {
                try
                {
                    dataContexto.Database.UseTransaction(_transaction);

                    return (from co in dataContexto.scCobertura
                            where co.scCoEstatus == true
                            select new CoberturaContrato
                            {
                                scCoId = co.scCoId,
                                scCoNombre = co.scCoNombre
                            }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    CierraConexion();
                }
            }
        }
    }
}
