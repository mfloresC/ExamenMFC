using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seguritas.Auxiliar;
using Seguritas.Entidad;

namespace Seguritas.Datos
{
    public class ClienteDatos
    {
        private SeguritasEntities dataContexto = new SeguritasEntities();
        private DbConnection _storeConnection;
        private DbTransaction _transaction;

        public string mensaje { get; set; }

        #region Constructor
        public ClienteDatos()
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

        public async Task<bool> GuardaActualizaCliente(ClienteContrato objCliente)
        {
            int idCliente = 0;
            bool res = false;
            using (dataContexto)
            {
                try
                {
                    dataContexto.Database.UseTransaction(_transaction);
                    idCliente = objCliente.scCId;

                    var clienteFound = (from c in dataContexto.scCliente
                                        where c.scCId == idCliente
                                        select c).ToList().FirstOrDefault();

                    var clienteFoundNombre = (from c in dataContexto.scCliente
                                              where c.scCNombre == objCliente.scCNombre
                                              select c).ToList().FirstOrDefault();

                    var clientePlanFound = (from cp in dataContexto.soClientePlan
                                            where cp.soCPCId == idCliente
                                            select cp).ToList();

                    var planFound = (from p in dataContexto.scPlan
                                     select p).ToList().Where(p => objCliente.claveP.Contains(p.scPId));

                    if (clienteFound == null && clientePlanFound.Count() == 0 && clienteFoundNombre == null)
                    {
                        scCliente entidadCliente = new scCliente();
                        entidadCliente.scCNombre = objCliente.scCNombre;
                        entidadCliente.scCFechaMod = DateTime.Now;
                        dataContexto.scCliente.Add(entidadCliente);
                        dataContexto.SaveChanges();
                        objCliente.scCId = entidadCliente.scCId;

                        foreach (var plan in planFound)
                        {
                            soClientePlan entidadClientePlan = new soClientePlan();
                            entidadClientePlan.soCPCId = entidadCliente.scCId;
                            entidadClientePlan.soCPPId = plan.scPId;
                            entidadClientePlan.soCPEstatus = true;
                            entidadClientePlan.soCPFechaMod = DateTime.Now;
                            dataContexto.soClientePlan.Add(entidadClientePlan);
                            dataContexto.SaveChanges();
                        }
                        _transaction.Commit();
                        res = true;
                    }
                    else
                    {
                        if (clienteFoundNombre.scCNombre == objCliente.scCNombre)
                        {
                            mensaje = "EL nombre insertado coincide con otro registro de la Base, intenta con otro nombre";
                            res = false;
                        }
                        else
                        {
                            clienteFound.scCNombre = objCliente.scCNombre;
                            clienteFound.scCFechaMod = DateTime.Now;
                            dataContexto.SaveChanges();

                            var planesNuevos = planFound.Where(nc => !clientePlanFound.Any(vc => nc.scPId == vc.soCPCId));
                            var planesEliminar = clientePlanFound.Where(vc => !planFound.Any(nc => vc.soCPCId == nc.scPId));

                            foreach (var nuevo in planesNuevos)
                            {
                                soClientePlan entidadClientePlan = new soClientePlan();
                                entidadClientePlan.scPlan = nuevo;
                                entidadClientePlan.scCliente = clienteFound;
                                entidadClientePlan.soCPFechaMod = DateTime.Now;
                                dataContexto.soClientePlan.Add(entidadClientePlan);
                                dataContexto.SaveChanges();
                            }

                            foreach (var viejo in planesEliminar)
                            {
                                var modificar = clientePlanFound.Where(cv => cv.soCPPId == viejo.soCPPId).FirstOrDefault();
                                modificar.soCPEstatus = false;
                                dataContexto.SaveChanges();
                            }
                            _transaction.Commit();
                            res = true;
                        }
                    }
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

        public async Task<bool> EliminaCliente(int idCliente)
        {
            bool resultado = false;
            using (dataContexto)
            {
                try
                {
                    dataContexto.Database.UseTransaction(_transaction);

                    var clienteFound = (from c in dataContexto.scCliente
                                        where c.scCId == idCliente
                                        select c).ToList().FirstOrDefault();

                    var clientePlanFound = (from cp in dataContexto.soClientePlan
                                            where cp.soCPCId == idCliente
                                            select cp).ToList();

                    if (clienteFound != null && clientePlanFound != null)
                    {
                        clientePlanFound.ForEach(elemento => {
                            elemento.soCPEstatus = false;
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

        public async Task<List<ClienteContrato>> ObtenerClientes()
        {
            using (dataContexto)
            {
                try
                {
                    dataContexto.Database.UseTransaction(_transaction);
                    
                    return (from c in dataContexto.soClientePlan
                            where c.soCPEstatus == true
                            select new ClienteContrato
                            {
                                scCId = c.soCPCId,
                                scCNombre = c.scCliente.scCNombre,
                                nombreP = c.scPlan.scPNombre

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

        public async Task<List<PlanContrato>> ObtenerCatalogoPlan()
        {
            using (dataContexto)
            {
                try
                {
                    dataContexto.Database.UseTransaction(_transaction);

                    return (from p in dataContexto.scPlan
                            select new PlanContrato
                            {
                                scPId = p.scPId,
                                scPNombre = p.scPNombre,
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
