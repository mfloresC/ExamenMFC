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
    public class CoberturaDatos
    {
        private SeguritasEntities dataContexto = new SeguritasEntities();
        private DbConnection _storeConnection;
        private DbTransaction _transaction;

        #region Constructor
        public CoberturaDatos()
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

        public async Task<bool> GuardaActualizaCobertura(CoberturaContrato objCobertura)
        {
            int idCobertura = 0;
            bool res = false;
            using (dataContexto)
            {
                try
                {
                    dataContexto.Database.UseTransaction(_transaction);
                    idCobertura = objCobertura.scCoId;

                    var coberturaFound = (from co in dataContexto.scCobertura
                                        where co.scCoId == idCobertura
                                        select co).ToList().FirstOrDefault();

                    if (coberturaFound == null)
                    {
                        scCobertura entidadCobertura = new scCobertura();
                        entidadCobertura.scCoNombre = objCobertura.scCoNombre;
                        entidadCobertura.scCoFechaMod = DateTime.Now;
                        entidadCobertura.scCobSuma = objCobertura.scCobSuma;
                        entidadCobertura.scCoEstatus = true;
                        dataContexto.scCobertura.Add(entidadCobertura);
                        dataContexto.SaveChanges();
                    }
                    else
                    {
                        coberturaFound.scCoNombre = objCobertura.scCoNombre;
                        coberturaFound.scCobSuma = objCobertura.scCobSuma;
                        coberturaFound.scCoFechaMod = DateTime.Now;
                        coberturaFound.scCoEstatus = true;
                        dataContexto.SaveChanges();
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

        public async Task<bool> EliminaCobertura(int idCobertura)
        {
            bool resultado = false;
            using (dataContexto)
            {
                try
                {
                    dataContexto.Database.UseTransaction(_transaction);

                    var coberturaFound = (from p in dataContexto.scCobertura
                                     where p.scCoId == idCobertura
                                     select p).ToList().FirstOrDefault();
                    
                    if (coberturaFound != null)
                    {
                        coberturaFound.scCoEstatus = false;  
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

        public async Task<List<CoberturaContrato>> ObtenerCoberturas()
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
                                scCoNombre = co.scCoNombre,
                                scCobSuma = co.scCobSuma

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
