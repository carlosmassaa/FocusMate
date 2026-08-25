using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class BitacoraDal
    {
        public int Guardar(Bitacora bitacora)
        {
            if (bitacora == null)
            {
                throw new ArgumentNullException(nameof(bitacora));
            }
            if (bitacora.FechaHoraUtc < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                bitacora.FechaHoraUtc = DateTime.UtcNow;
            }

            AccesoBD accesoBD = new AccesoBD();

            bool includeId = bitacora.Id != 0;

            SqlParameter[] parameters = BuildParams(bitacora, includeId);

            object esc = accesoBD.ExecuteScalarSp("spBitacora_Guardar", parameters);
            int idScalar;
            if (esc != null && esc != DBNull.Value && int.TryParse(esc.ToString(), out idScalar) && idScalar > 0)
            {
                bitacora.Id = idScalar;
                return idScalar;
            }

            int rows = accesoBD.ExecuteNonQuerySp("spBitacora_Guardar", parameters);
            if (bitacora.Id == 0 && rows > 0)
            {
                return rows;
            }

            if (bitacora.Id != 0)
            {
                return bitacora.Id;
            }
            else
            {
                return rows;
            }
        }

        private SqlParameter[] BuildParams(Bitacora bitacora, bool includeId)
        {
            string nombreUsuario;
            if (bitacora.Usuario == null)
            {
                nombreUsuario = string.Empty;
            }
            else
            {
                nombreUsuario = bitacora.Usuario;
            }

            string modulo;
            if (bitacora.Modulo == null)
            {
                modulo = string.Empty;
            }
            else
            {
                modulo = bitacora.Modulo;
            }

            string accion;
            if (bitacora.Accion == null)
            {
                accion = string.Empty;
            }
            else
            {
                accion = bitacora.Accion;
            }

            string entidad;
            if (bitacora.Entidad == null)
            {
                entidad = string.Empty;
            }
            else
            {
                entidad = bitacora.Entidad;
            }

            string resultado;
            if (bitacora.Resultado == null)
            {
                resultado = string.Empty;
            }
            else
            {
                resultado = bitacora.Resultado;
            }

            string metadatos;
            if (bitacora.Metadatos == null)
            {
                metadatos = string.Empty;
            }
            else
            {
                metadatos = bitacora.Metadatos;
            }

            object idUsuarioParam;
            if (bitacora.UsuarioId > 0)
            {
                idUsuarioParam = bitacora.UsuarioId;
            }
            else
            {
                idUsuarioParam = DBNull.Value;
            }


            List<SqlParameter> list = new List<SqlParameter>();

            if (includeId)
            {
                list.Add(new SqlParameter("@Id_Bitacora", bitacora.Id));
            }

            list.AddRange(new[]{new SqlParameter("@FechaHoraUtc", bitacora.FechaHoraUtc),new SqlParameter("@Id_Usuario", idUsuarioParam),new SqlParameter("@NombreUsuario", nombreUsuario),new SqlParameter("@Modulo", modulo),new SqlParameter("@Accion", accion),new SqlParameter("@Entidad", entidad),new SqlParameter("@EntidadId", bitacora.EntidadId),new SqlParameter("@Resultado", resultado),new SqlParameter("@Metadatos", metadatos)});

            return list.ToArray();
        }

        public List<Bitacora> Listar()
        {
            AccesoBD accesoBD = new AccesoBD();

            DataSet dataSet = accesoBD.ExecuteDataSetSp("spBitacora_Listar");

            List<Bitacora> bitacoras = new List<Bitacora>();
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                {
                    Bitacora bitacora = new Bitacora();
                    ValorizarEntidad(bitacora, dataRow);
                    bitacoras.Add(bitacora);
                }
            }
            return bitacoras;
        }


        public List<Bitacora> BuscarFiltrado(BitacoraFiltros filtros)
        {
            if (filtros == null)
            {
                throw new ArgumentNullException(nameof(filtros));
            }

            AccesoBD accesoBD = new AccesoBD();
            List<SqlParameter> pars = new List<SqlParameter>();

            object fechaDesdeParam;
            if (filtros.FechaDesdeUtc.HasValue)
            {
                fechaDesdeParam = filtros.FechaDesdeUtc.Value;
            }
            else
            {
                fechaDesdeParam = DBNull.Value;
            }

            object fechaHastaParam;
            if (filtros.FechaHastaUtc.HasValue)
            {
                fechaHastaParam = filtros.FechaHastaUtc.Value;
            }
            else
            {
                fechaHastaParam = DBNull.Value;
            }

            object usuarioIdParam;
            if (filtros.UsuarioId.HasValue)
            {
                usuarioIdParam = filtros.UsuarioId.Value;
            }
            else
            {
                usuarioIdParam = DBNull.Value;
            }

            object entidadIdParam;
            if (filtros.EntidadId.HasValue)
            {
                entidadIdParam = filtros.EntidadId.Value;
            }
            else
            {
                entidadIdParam = DBNull.Value;
            }

            object usuarioParam;
            if (string.IsNullOrWhiteSpace(filtros.Usuario))
            {
                usuarioParam = DBNull.Value;
            }
            else
            {
                usuarioParam = filtros.Usuario.Trim();
            }

            object moduloParam;
            if (string.IsNullOrWhiteSpace(filtros.Modulo))
            {
                moduloParam = DBNull.Value;
            }
            else
            {
                moduloParam = filtros.Modulo.Trim();
            }

            object accionParam;
            if (string.IsNullOrWhiteSpace(filtros.Accion))
            {
                accionParam = DBNull.Value;
            }
            else
            {
                accionParam = filtros.Accion.Trim();
            }

            object entidadParam;
            if (string.IsNullOrWhiteSpace(filtros.Entidad))
            {
                entidadParam = DBNull.Value;
            }
            else
            {
                entidadParam = filtros.Entidad.Trim();
            }

            object resultadoParam;
            if (string.IsNullOrWhiteSpace(filtros.Resultado))
            {
                resultadoParam = DBNull.Value;
            }
            else
            {
                resultadoParam = filtros.Resultado.Trim();
            }

            object textoLibreParam;
            if (string.IsNullOrWhiteSpace(filtros.TextoLibre))
            {
                textoLibreParam = DBNull.Value;
            }
            else
            {
                textoLibreParam = filtros.TextoLibre.Trim();
            }

            pars.AddRange(new[]{new SqlParameter("@FechaDesdeUtc", fechaDesdeParam),new SqlParameter("@FechaHastaUtc", fechaHastaParam),new SqlParameter("@UsuarioId", usuarioIdParam),new SqlParameter("@Usuario", usuarioParam),new SqlParameter("@Modulo", moduloParam),new SqlParameter("@Accion", accionParam),new SqlParameter("@Entidad", entidadParam),new SqlParameter("@EntidadId", entidadIdParam),new SqlParameter("@Resultado", resultadoParam),new SqlParameter("@TextoLibre", textoLibreParam)});

            DataSet dataSet = accesoBD.ExecuteDataSetSp("spBitacora_BuscarFiltrado", pars.ToArray());

            List<Bitacora> salida = new List<Bitacora>();
            if (dataSet.Tables.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    Bitacora bitacora = new Bitacora();
                    ValorizarEntidad(bitacora, row);
                    salida.Add(bitacora);
                }
            }

            return salida;
        }


        private void ValorizarEntidad(Bitacora bitacora, DataRow dataRow)
        {
            bitacora.Id = Convert.ToInt32(dataRow["Id_Bitacora"]);
            bitacora.FechaHoraUtc = Convert.ToDateTime(dataRow["FechaHoraUtc"]);

            if (dataRow["Id_Usuario"] == DBNull.Value)
            {
                bitacora.UsuarioId = 0;
            }
            else
            {
                bitacora.UsuarioId = Convert.ToInt32(dataRow["Id_Usuario"]);
            }

            bitacora.Usuario = dataRow["NombreUsuario"].ToString();
            bitacora.Modulo = dataRow["Modulo"].ToString();
            bitacora.Accion = dataRow["Accion"].ToString();
            bitacora.Entidad = dataRow["Entidad"].ToString();

            if (dataRow["EntidadId"] == DBNull.Value)
            {
                bitacora.EntidadId = 0;
            }
            else
            {
                bitacora.EntidadId = Convert.ToInt32(dataRow["EntidadId"]);
            }

            bitacora.Resultado = dataRow["Resultado"].ToString();
            bitacora.Metadatos = dataRow["Metadatos"].ToString();
        }
    }
}
