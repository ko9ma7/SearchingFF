using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace MCF.Classes.Data
{
    public class MySqlHelper
    {
        private static string _connectionString = null;
        private static string _connectionString2 = null;
        /// <summary>
        /// GW서버 접속정보
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    _connectionString = string.Format(@"server={0};port={1};user={2};password={3};database={4};allow zero datetime=yes;respect binary flags=false;allow user variables=true", "1.234.36.115", "3306", "root", "wh126147", "FIFA");

                    //_connectionString = string.Format("Host={0};User ID={1};Password={2};Sid={3};Port={4}", DBInfo.IP, DBInfo.ID, DBInfo.PW, DBInfo.SID, DBInfo.PORT);
                }

                return _connectionString;
            }
        }

        #region Return Connection

        /// <summary>
        /// Connection을 반환
        /// </summary>
        /// <returns></returns>
        public static MySqlConnection GetConnection()
        {
            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = MySqlHelper.ConnectionString;

                //conn.Direct = true;
                conn.Open();
            }
            catch (MySqlException me)
            {
                conn = null;
                //throw new Exception("" + me.Message);
                Console.WriteLine("" + me.Message);
                //throw new FxRuntimeException(oe.Message, System.Reflection.MethodInfo.GetCurrentMethod());
            }

            return conn;
        }

        #endregion

        #region ExecuteDataTable

        public static DataTable ExecuteDataTable(string commandText)
        {
            return ExecuteDataTable(commandText, (MySqlParameter[])null, CommandType.Text, null);
        }

        public static DataTable ExecuteDataTable(string commandText, MySqlParameter[] sqlParams)
        {
            return ExecuteDataTable(commandText, sqlParams, CommandType.Text, null);
        }

        public static DataTable ExecuteDataTable(string commandText, MySqlParameter[] sqlParams, CommandType commandType)
        {
            return ExecuteDataTable(commandText, sqlParams, commandType, null);
        }

        public static DataTable ExecuteDataTable(string commandText, string tableName)
        {
            return ExecuteDataTable(commandText, (MySqlParameter[])null, CommandType.Text, tableName);
        }

        public static DataTable ExecuteDataTable(string commandText, MySqlParameter[] sqlParams, string tableName)
        {
            return ExecuteDataTable(commandText, sqlParams, CommandType.Text, tableName);
        }

        public static DataTable ExecuteDataTable(string commandText, CommandType commandType, string tableName)
        {
            return ExecuteDataTable(commandText, (MySqlParameter[])null, commandType, tableName);
        }

        /// <summary>
        /// SQL문을 실행하고 데이터테이블을 반환
        /// </summary>
        /// <param name="commandText">데이터소스</param>
        /// <param name="oracleParams">파라미터</param>
        /// <param name="commandType">명령속성</param>
        /// <param name="tableName">반환되는 테이블명</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string commandText, MySqlParameter[] sqlParams, CommandType commandType, string tableName)
        {
            MySqlConnection conn = null;
            DataTable dt = null;

            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = MySqlHelper.ConnectionString;
                //conn.Direct = true;
                conn.Open();

                if (commandText.Contains("GROUP_CONCAT"))
                {
                    MySqlCommand comm = new MySqlCommand();
                    comm.Connection = conn;
                    comm.CommandType = commandType;
                    comm.CommandText = "set @@group_concat_max_len = 500000;";
                    try
                    {
                        int c = comm.ExecuteNonQuery();
                    }
                    finally
                    {

                    }
                }

                
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(commandText, conn))
                {
                    adapter.SelectCommand.CommandType = commandType;

                    if (sqlParams != null && sqlParams.Length > 0)
                        adapter.SelectCommand.Parameters.AddRange(sqlParams);
                    if (tableName == null)
                        tableName = "table1";

                    dt = new DataTable(tableName);
  
                    adapter.Fill(dt);
                }
            }
            catch (MySqlException ex)
            {
                //return null;
                //throw new Exception("" + ex.Message);
            }
            finally
            {
                if (null != conn && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }

        #endregion ExecuteDataTable

        #region ExecuteNonQuery

        public static int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, (MySqlParameter[])null, CommandType.Text);
        }

        public static int ExecuteNonQuery(string commandText, MySqlParameter[] sqlParams)
        {
            return ExecuteNonQuery(commandText, sqlParams, CommandType.Text);
        }

        public static int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            return ExecuteNonQuery(commandText, (MySqlParameter[])null, commandType);
        }

        /// <summary>
        /// SQL문을 실행하고 영향 받는 행 번호를 반환
        /// </summary>`
        /// <param name="commandText">데이터소스</param>
        /// <param name="oracleParams">파리미터</param>
        /// <param name="commandType">명령속성</param>
        /// <returns>영향받은행수</returns>
        public static int ExecuteNonQuery(string commandText, MySqlParameter[] sqlParams, CommandType commandType)
        {
            int count = 0;
            MySqlConnection conn = null;
            MySqlCommand comm = null;

            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = MySqlHelper.ConnectionString;
                //conn.Direct = true;
                conn.Open();

                comm = new MySqlCommand();
                comm.Connection = conn;
                comm.CommandType = commandType;
                comm.CommandText = commandText;
                if (sqlParams != null && sqlParams.Length > 0)
                    comm.Parameters.AddRange(sqlParams);

                try
                {
                    count = comm.ExecuteNonQuery();
                }
                catch
                {
                    count = -1;
                }
            }
            catch (MySqlException ex)
            {
                
throw new Exception("" + ex.Message);
                //throw new FxRuntimeException(ex.Message, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                if (null != comm)
                    comm.Dispose();
                if (null != conn && conn.State == ConnectionState.Open)
                    conn.Close();
            }

            // 영향받은 행의수 반환
            return count;
        }


        /// <summary>
        /// Execute an OracleCommand (that returns no resultset) against the specified OracleTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">a valid OracleTransaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>  
        /// <param name="commandParameters">an array of OracleParameters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(MySqlConnection connection, string commandText, params MySqlParameter[] sqlParams)
        {
            int count = 0;
            MySqlCommand comm = null;

            try
            {
                comm = new MySqlCommand();
                comm.Connection = connection;
                comm.CommandType = CommandType.Text;
                comm.CommandText = commandText;
                if (sqlParams != null && sqlParams.Length > 0)
                    comm.Parameters.AddRange(sqlParams);

                count = comm.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }

            return count;
        }

        #endregion ExecuteNonQuery

        #region ExecuteScalar

        public static string ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, (MySqlParameter[])null, CommandType.Text);
        }

        public static string ExecuteScalar(string commandText, MySqlParameter[] sqlParams)
        {
            return ExecuteScalar(commandText, sqlParams, CommandType.Text);
        }

        public static string ExecuteScalar(string commandText, CommandType commandType)
        {
            return ExecuteScalar(commandText, (MySqlParameter[])null, commandType);
        }

        public static string ExecuteScalar(string commandText, MySqlParameter[] sqlParams, CommandType commandType)
        {
            object value;
            MySqlConnection conn = null;
            MySqlCommand comm = null;

            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = MySqlHelper.ConnectionString;
                //conn.Direct = true;
                conn.Open();

                comm = new MySqlCommand();
                comm.Connection = conn;
                comm.CommandType = commandType;
                comm.CommandText = commandText;
                if (sqlParams != null && sqlParams.Length > 0)
                    comm.Parameters.AddRange(sqlParams);

                value = comm.ExecuteScalar();
            }
            catch (MySqlException ex)
            {
                throw new Exception("" + ex.Message);
            }
            finally
            {
                if (null != comm)
                    comm.Dispose();
                if (null != conn && conn.State == ConnectionState.Open)
                    conn.Close();
            }

            // 리턴값
            return value.ToString();
        }

        #endregion

        public static DataTable SelectQueryToDataSet(string p)
        {
            throw new NotImplementedException();
        }
    }
}
