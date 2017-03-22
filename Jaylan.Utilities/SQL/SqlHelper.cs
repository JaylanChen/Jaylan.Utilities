using System.Data;
using System.Data.SqlClient;

namespace Jaylan.Utilities.SQL
{
    public static class SqlHelper
    {
        /// <summary>
        /// 执行非查询语句，对于增删改返回受影响行数，否则返回-1
        /// </summary>
        /// <param name="conStr">SQL连接字符串</param>
        /// <param name="sqlStr">执行的SQL语句</param>
        /// <param name="cmdType">执行的类型(SQL语句、存储过程)</param>
        /// <param name="pms">参数化的绑定参数值</param>
        /// <returns>受影响行数</returns>
        private static int ExecuteNonQuery(string conStr, string sqlStr, CommandType cmdType, params SqlParameter[] pms)
        {
            using (var con = new SqlConnection(conStr))
            {
                using (var cmd = new SqlCommand(sqlStr, con))
                {
                    cmd.CommandType = cmdType;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 调用普通的sql语句的ExecuteNonQuery，受影响行数
        /// </summary>
        /// <param name="conStr">SQL连接字符串</param>
        /// <param name="sqlStr">执行的SQL语句</param>
        /// <param name="pms">SQL语句参数</param>
        /// <returns>受影响行数</returns>
        public static int ExecuteNonQuery(string conStr, string sqlStr, params SqlParameter[] pms)
        {
            return ExecuteNonQuery(conStr, sqlStr, CommandType.Text, pms);
        }

        /// <summary>
        /// 调用存储的ExecuteNonQuery，受影响行数
        /// </summary>
        /// <param name="conStr">SQL连接字符串</param>
        /// <param name="sqlStr">执行的SQL语句</param>
        /// <param name="pms">SQL语句参数</param>
        /// <returns>受影响行数</returns>
        public static int ExecuteNonQuerySp(string conStr, string sqlStr, params SqlParameter[] pms)
        {
            return ExecuteNonQuery(conStr, sqlStr, CommandType.StoredProcedure, pms);
        }

        /// <summary>
        /// 一般执行结果只有一行一列的语句，只返回第一行第一列
        /// </summary>
        /// <param name="conStr">SQL连接字符串</param>
        /// <param name="sqlStr">执行的SQL语句</param>
        /// <param name="cmdType">执行的类型(SQL语句、存储过程)</param>
        /// <param name="pms">SQL语句参数</param>
        /// <returns>执行结果的第一行第一列</returns>
        private static object ExecuteScalar(string conStr, string sqlStr, CommandType cmdType, params SqlParameter[] pms)
        {
            using (var con = new SqlConnection(conStr))
            {
                using (var cmd = new SqlCommand(sqlStr, con))
                {
                    cmd.CommandType = cmdType;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 调用普通的sql语句的ExecuteScalar，执行结果的第一行第一列
        /// </summary>
        /// <param name="conStr">SQL连接字符串</param>
        /// <param name="sqlStr">执行的SQL语句</param>
        /// <param name="pms">SQL语句参数</param>
        /// <returns>执行结果的第一行第一列</returns>
        public static object ExecuteScalar(string conStr, string sqlStr, params SqlParameter[] pms)
        {
            return ExecuteScalar(conStr, sqlStr, CommandType.Text, pms);
        }

        /// <summary>
        /// 调用存储过程的ExecuteScalar，执行结果的第一行第一列
        /// </summary>
        /// <param name="conStr">SQL连接字符串</param>
        /// <param name="sqlStr">执行的SQL语句</param>
        /// <param name="pms">SQL语句参数</param>
        /// <returns>执行结果的第一行第一列</returns>
        public static object ExecuteScalarSp(string conStr, string sqlStr, params SqlParameter[] pms)
        {
            return ExecuteScalar(conStr, sqlStr, CommandType.StoredProcedure, pms);
        }

        /// <summary>
        /// 执行结果多行多列的语句，一般只是查询语句，返回一个结果的读取器
        /// </summary>
        /// <param name="conStr">SQL连接字符串</param>
        /// <param name="sqlStr">执行的SQL语句</param>
        /// <param name="cmdType">执行的类型(SQL语句、存储过程)</param>
        /// <param name="pms">参数化的绑定参数值</param>
        /// <returns>返回结果的一个读取器</returns>
        private static SqlDataReader ExecuteReader(string conStr, string sqlStr, CommandType cmdType, params SqlParameter[] pms)
        {
            var con = new SqlConnection(conStr);
            using (var cmd = new SqlCommand(sqlStr, con))
            {
                cmd.CommandType = cmdType;
                try
                {
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch
                {
                    con.Close();
                    con.Dispose();
                    throw;
                }
            }
        }

        /// <summary>
        /// 调用普通的sql语句的ExecuteReader
        /// </summary>
        /// <param name="conStr">SQL连接字符串</param>
        /// <param name="sqlStr">执行的SQL语句</param>
        /// <param name="pms">SQL语句参数</param>
        /// <returns>返回结果的一个读取器</returns>
        public static SqlDataReader ExecuteReader(string conStr, string sqlStr, params SqlParameter[] pms)
        {
            return ExecuteReader(conStr, sqlStr, CommandType.Text, pms);
        }

        /// <summary>
        /// 调用普存储过程的ExecuteReader
        /// </summary>
        /// <param name="conStr">SQL连接字符串</param>
        /// <param name="sqlStr">执行的SQL语句</param>
        /// <param name="pms">SQL语句参数</param>
        /// <returns>返回结果的一个读取器</returns>
        public static SqlDataReader ExecuteReaderSp(string conStr, string sqlStr, params SqlParameter[] pms)
        {
            return ExecuteReader(conStr, sqlStr, CommandType.StoredProcedure, pms);
        }

        /// <summary>
        /// 执行查询语句，返回存储查询结果集的DataTable
        /// </summary>
        /// <param name="conStr">SQL连接字符串</param>
        /// <param name="sqlStr">执行的SQL语句</param>
        /// <param name="cmdType">执行的类型(SQL语句、存储过程)</param>
        /// <param name="pms">参数化的绑定参数值</param>
        /// <returns>返回存储查询结果集的DataTable</returns>
        private static DataTable ExecuteSqlDataAdapter(string conStr, string sqlStr, CommandType cmdType, params SqlParameter[] pms)
        {
            var dt = new DataTable();
            using (var adapter = new SqlDataAdapter(sqlStr, conStr))
            {
                adapter.SelectCommand.CommandType = cmdType;
                if (pms != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(pms);
                }
                adapter.Fill(dt);
            }
            return dt;
        }

        /// <summary>
        /// 调用普通的sql语句的ExecuteSqlDataAdapter
        /// </summary>
        /// <param name="conStr">SQL连接字符串</param>
        /// <param name="sqlStr">执行的SQL语句</param>
        /// <param name="pms">SQL语句参数</param>
        /// <returns>返回存储查询结果集的DataTable</returns>
        public static DataTable ExecuteSqlDataAdapter(string conStr, string sqlStr, params SqlParameter[] pms)
        {
            return ExecuteSqlDataAdapter(conStr, sqlStr, CommandType.Text, pms);
        }

        /// <summary>
        /// 调用存储过程的ExecuteSqlDataAdapter
        /// </summary>
        /// <param name="conStr">SQL连接字符串</param>
        /// <param name="sqlStr">执行的SQL语句</param>
        /// <param name="pms">SQL语句参数</param>
        /// <returns>返回存储查询结果集的DataTable</returns>
        public static DataTable ExecuteSqlDataAdapterSp(string conStr, string sqlStr, params SqlParameter[] pms)
        {
            return ExecuteSqlDataAdapter(conStr, sqlStr, CommandType.StoredProcedure, pms);
        }
    }
}
