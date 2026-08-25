using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BE;

namespace DAL
{
    public class IdiomaDAL
    {
        public IdiomaDAL()
        {
        }

        public List<Idioma> ListarIdiomas()
        {
            List<Idioma> listaIdiomas = new List<Idioma>();

            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spIdioma_ListarActivos");
            if (dataSet.Tables.Count == 0)
            {
                return listaIdiomas;
            }

            foreach (DataRow fila in dataSet.Tables[0].Rows)
            {
                Idioma idioma = new Idioma();
                ValorizarEntidad(idioma, fila);
                listaIdiomas.Add(idioma);
            }

            return listaIdiomas;
        }

        public Dictionary<string, string> ObtenerTraducciones(int idIdioma)
        {
            Dictionary<string, string> traduccionesPorClave = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spIdioma_ObtenerTraducciones", new SqlParameter("@Id_Idioma", idIdioma));
            if (dataSet.Tables.Count == 0)
            {
                return traduccionesPorClave;
            }

            foreach (DataRow fila in dataSet.Tables[0].Rows)
            {
                string clave;

                if (fila.Table.Columns.Contains("Clave"))
                {
                    if (fila["Clave"] != DBNull.Value)
                    {
                        clave = fila["Clave"].ToString();
                    }
                    else
                    {
                        clave = string.Empty;
                    }
                }
                else if (fila.Table.Columns.Contains("Codigo"))
                {
                    if (fila["Codigo"] != DBNull.Value)
                    {
                        clave = fila["Codigo"].ToString();
                    }
                    else
                    {
                        clave = string.Empty;
                    }
                }
                else
                {
                    clave = string.Empty;
                }

                string texto = string.Empty;
                if (fila.Table.Columns.Contains("Texto"))
                {
                    if (fila["Texto"] != DBNull.Value)
                    {
                        texto = fila["Texto"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(clave))
                {
                    traduccionesPorClave[clave] = texto;
                }
            }

            return traduccionesPorClave;
        }

        public List<EtiquetaTraduccion> ObtenerTraduccionesPorClaves(int idIdioma, IEnumerable<string> claves)
        {
            List<EtiquetaTraduccion> listaTraducciones = new List<EtiquetaTraduccion>();

            List<string> listaClaves;
            if (claves == null)
            {
                listaClaves = new List<string>(Array.Empty<string>());
            }
            else
            {
                listaClaves = new List<string>(claves);
            }

            if (listaClaves.Count == 0)
            {
                return listaTraducciones;
            }

            foreach (string clave in listaClaves.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                AccesoBD accesoBD = new AccesoBD();
                DataSet dataSet = accesoBD.ExecuteDataSetSp("spIdioma_ObtenerTraduccionPorClave", new SqlParameter("@Id_Idioma", idIdioma), new SqlParameter("@Codigo", clave));

                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow fila = dataSet.Tables[0].Rows[0];

                    string codigo = clave;
                    if (fila.Table.Columns.Contains("Codigo") && fila["Codigo"] != DBNull.Value)
                    {
                        codigo = fila["Codigo"].ToString();
                    }

                    string textoBase = clave;
                    if (fila.Table.Columns.Contains("TextoBase") && fila["TextoBase"] != DBNull.Value)
                    {
                        textoBase = fila["TextoBase"].ToString();
                    }

                    string texto = null;
                    if (fila.Table.Columns.Contains("Texto") && fila["Texto"] != DBNull.Value)
                    {
                        texto = fila["Texto"].ToString();
                    }

                    listaTraducciones.Add(new EtiquetaTraduccion { Clave = codigo, TextoBase = textoBase, Texto = texto });
                }
                else
                {
                    listaTraducciones.Add(new EtiquetaTraduccion { Clave = clave, TextoBase = clave, Texto = null });
                }
            }

            return listaTraducciones;
        }

        public void UpsertTraducciones(int idIdioma, List<EtiquetaTraduccion> filas)
        {
            if (filas == null || filas.Count == 0)
            {
                return;
            }

            foreach (EtiquetaTraduccion filaTraduccion in filas)
            {
                string codigo;
                if (filaTraduccion.Clave == null)
                {
                    codigo = string.Empty;
                }
                else
                {
                    codigo = filaTraduccion.Clave;
                }

                string textoBase;
                if (string.IsNullOrWhiteSpace(filaTraduccion.TextoBase))
                {
                    textoBase = codigo;
                }
                else
                {
                    textoBase = filaTraduccion.TextoBase;
                }

                string texto;
                if (filaTraduccion.Texto == null)
                {
                    texto = string.Empty;
                }
                else
                {
                    texto = filaTraduccion.Texto;
                }

                AccesoBD accesoBD = new AccesoBD();
                accesoBD.ExecuteNonQuerySp("spEtiqueta_Asegurar", new SqlParameter("@Codigo", codigo), new SqlParameter("@TextoBase", textoBase));

                DataSet dataSet = accesoBD.ExecuteDataSetSp("spIdioma_ObtenerTraduccionPorClave", new SqlParameter("@Id_Idioma", idIdioma), new SqlParameter("@Codigo", codigo));

                bool existe = false;
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow fila = dataSet.Tables[0].Rows[0];
                    existe = fila.Table.Columns.Contains("Texto") && fila["Texto"] != DBNull.Value;
                }

                if (string.IsNullOrWhiteSpace(texto))
                {
                    if (existe)
                    {
                        accesoBD.ExecuteNonQuerySp("spIdioma_EliminarTraduccion", new SqlParameter("@Id_Idioma", idIdioma), new SqlParameter("@Codigo", codigo));
                    }

                    continue;
                }

                if (!existe)
                {
                    accesoBD.ExecuteNonQuerySp("spIdioma_GuardarTraduccion", new SqlParameter("@Id_Idioma", idIdioma), new SqlParameter("@Codigo", codigo), new SqlParameter("@Texto", texto));
                }
                else
                {
                    accesoBD.ExecuteNonQuerySp("spIdioma_EditarTraduccion", new SqlParameter("@Id_Idioma", idIdioma), new SqlParameter("@Codigo", codigo), new SqlParameter("@Texto", texto));
                }
            }
        }

        public int CrearIdiomaConPlaceholdersTextoBase(string nombre, string codigoISO)
        {
            AccesoBD accesoBD = new AccesoBD();
            object resultadoEscalar = accesoBD.ExecuteScalarSp("spIdioma_Crear", new SqlParameter("@Nombre", nombre), new SqlParameter("@Codigo", codigoISO), new SqlParameter("@EstaActivo", 1));

            if (resultadoEscalar == null || resultadoEscalar == DBNull.Value)
            {
                throw new InvalidOperationException("No fue posible crear el idioma.");
            }

            int idiomaId;
            if (!int.TryParse(resultadoEscalar.ToString(), out idiomaId) || idiomaId <= 0)
            {
                throw new InvalidOperationException("Id de idioma inválido al crear.");
            }

            accesoBD.ExecuteNonQuerySp("spIdioma_InicializarTraduccionesConTextoBase", new SqlParameter("@Id_Idioma", idiomaId));

            return idiomaId;
        }

        private void ValorizarEntidad(Idioma idioma, DataRow fila)
        {
            int idiomaId = 0;
            if (fila.Table.Columns.Contains("Id") && fila["Id"] != DBNull.Value)
            {
                int.TryParse(fila["Id"].ToString(), out idiomaId);
            }
            else if (fila.Table.Columns.Contains("Id_Idioma") && fila["Id_Idioma"] != DBNull.Value)
            {
                int.TryParse(fila["Id_Idioma"].ToString(), out idiomaId);
            }

            string nombre = string.Empty;
            if (fila.Table.Columns.Contains("Nombre") && fila["Nombre"] != DBNull.Value)
            {
                nombre = fila["Nombre"].ToString();
            }

            string codigoIso = null;
            if (fila.Table.Columns.Contains("Codigo") && fila["Codigo"] != DBNull.Value)
            {
                codigoIso = fila["Codigo"].ToString();
            }

            idioma.Id = idiomaId;
            idioma.Nombre = nombre;
            idioma.CodigoISO = codigoIso;
        }
    }
}
