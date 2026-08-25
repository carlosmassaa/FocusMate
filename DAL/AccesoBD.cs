using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    internal class AccesoBD
    {
        private readonly SqlConnection mCon = new SqlConnection("Data Source=.;Initial Catalog=FocusMateTDFinalFinal;Integrated Security=True;Encrypt=False");

        internal int ExecuteNonQuerySp(string procName, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(procName, mCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    mCon.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (mCon.State != ConnectionState.Closed) mCon.Close();
            }
        }



        internal object ExecuteScalarSp(string procName, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(procName, mCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    mCon.Open();
                    return cmd.ExecuteScalar();
                }
            }
            finally
            {
                if (mCon.State != ConnectionState.Closed)
                    mCon.Close();
            }
        }

        internal DataSet ExecuteDataSetSp(string procName, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = new SqlCommand(procName, mCon);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (parameters != null && parameters.Length > 0)
                    {
                        da.SelectCommand.Parameters.AddRange(parameters);
                    }
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            finally
            {
                if (mCon.State != ConnectionState.Closed) mCon.Close();
            }
        }


        internal int ExecuteNonQuerySpMaster(string procName, params SqlParameter[] parameters)
        {
            string csMaster = "Data Source=.;Initial Catalog=master;Integrated Security=True;Encrypt=False";
            using (SqlConnection con = new SqlConnection(csMaster))
            using (SqlCommand cmd = new SqlCommand(procName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}