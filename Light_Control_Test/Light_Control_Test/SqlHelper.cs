using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Data;

namespace Light_Control_Test
{
    static class SqlHelper
    {
        public static readonly string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        public static bool OpenConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static int ExecuteNonQuery(string cmdText,
            params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    return ExecuteNonQuery(conn, cmdText, parameters);

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
        }

        public static object ExecuteScalar(string cmdText,
            params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    return ExecuteScalar(conn, cmdText, parameters);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
        }

        public static DataTable ExecuteDataTable(string cmdText,
            params SqlParameter[] parameters)
        {
            //try
            //{
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    return ExecuteDataTable(conn, cmdText, parameters);
                }
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //    return null;
            //}
        }

        public static int ExecuteNonQuery(SqlConnection conn, string cmdText,
           params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(SqlConnection conn, string cmdText,
            params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }

        public static DataTable ExecuteDataTable(SqlConnection conn, string cmdText,
            params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public static object ToDBValue(this object value)
        {
            return value == null ? DBNull.Value : value;
        }

        static public SqlDataReader ExecuteReader(string sql, params SqlParameter[] paramer)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connstr;
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddRange(paramer);
            SqlDataReader dr = cmd.ExecuteReader();

            return dr;

        }

        public static object FromDBValue(this object dbValue)
        {
            return dbValue == DBNull.Value ? null : dbValue;
        }

        public static void CreateTable(string db,string dt_Content,string dt_Name)
        {
            string creatTable = "use"+" "+ db+" "+ "create table"+" "+ dt_Name+ " "+"(" + dt_Content + ")";
            SqlHelper.ExecuteNonQuery(creatTable);
        }
    }
}
