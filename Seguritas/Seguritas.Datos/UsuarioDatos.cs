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
    public class UsuarioDatos
    {
        private SeguritasEntities dataContexto = new SeguritasEntities();
        private DbConnection _storeConnection;
        private DbTransaction _transaction;

        #region Constructor
        public UsuarioDatos()
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

        public async Task<bool> GuardaActualizaUsuario(UsuarioContrato objUsuario)
        {
            int idUsuario = 0;
            bool res = false;
            using (dataContexto)
            {
                try
                {
                    dataContexto.Database.UseTransaction(_transaction);
                    idUsuario = objUsuario.scUId;

                    var usuarioFound = (from u in dataContexto.scUsuario
                                       where u.scUId == idUsuario
                                       select u).ToList().FirstOrDefault();

                    if (usuarioFound == null)
                    {
                        scUsuario entidadUsuario = new scUsuario();
                        entidadUsuario.scUNombre = objUsuario.scUNombre;
                        entidadUsuario.scUUsuario = objUsuario.scUUsuario;
                        entidadUsuario.scUPassword = objUsuario.scUPassword;
                        dataContexto.scUsuario.Add(entidadUsuario);
                        dataContexto.SaveChanges();
                        objUsuario.scUId = entidadUsuario.scUId;
                    }
                    else
                    {
                        usuarioFound.scUUsuario = objUsuario.scUUsuario;
                        usuarioFound.scUNombre = objUsuario.scUNombre;
                        usuarioFound.scUPassword = objUsuario.scUPassword;
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
        public async Task<UsuarioContrato> FirmarUsuario(UsuarioContrato objUsuario)
        {
            using (dataContexto)
            {
                try
                {
                    dataContexto.Database.UseTransaction(_transaction);
                    return (from u in dataContexto.scUsuario
                                            where u.scUUsuario == objUsuario.scUUsuario && u.scUPassword == objUsuario.scUPassword
                                            select new UsuarioContrato
                                            {
                                                scUId = u.scUId,
                                                scUNombre = u.scUNombre,
                                                scUUsuario = u.scUUsuario,
                                                scUPassword = u.scUPassword
                                            }).FirstOrDefault();
                }
                catch (Exception e)
                {
                    _transaction.Rollback();
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
