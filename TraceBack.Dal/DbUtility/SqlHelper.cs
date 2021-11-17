using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TraceBack.Dal.DbUtility
{
    public static class SqlHelper
    {
        //连接字符串
        public static string ConnectionString { get; set; }

        /// <summary>
        /// 1.执行增、删、改的方法：ExecuteNonQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, CommandType commandType = CommandType.Text, SqlTransaction transaction = null, params SqlParameter[] pms)
        {
            SqlConnection con;
            int iResult = 0;

            if (transaction == null)
            {
                con = new SqlConnection(ConnectionString);
            }
            else
            {
                con = transaction.Connection;
            }

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = commandType;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }

                    cmd.Transaction = transaction;
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    iResult = cmd.ExecuteNonQuery();
                    cmd.Transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (transaction == null)
                {
                    con.Dispose();
                }
            }

            return iResult;
        }

        //2.封装一个执行返回单个对象的方法：ExecuteScalar()
        public static object ExecuteScalar(string sql, CommandType commandType = CommandType.Text, SqlTransaction transaction = null, params SqlParameter[] pms)
        {
            SqlConnection con;
            object oResult = null;

            if (transaction == null)
            {
                con = new SqlConnection(ConnectionString);
            }
            else
            {
                con = transaction.Connection;
            }

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }

                    cmd.Transaction = transaction;
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    oResult = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (transaction == null)
                {
                    con.Dispose();
                }
            }
   
            return oResult;
        }

        //3.执行查询多行多列的数据的方法：ExecuteReader
        public static SqlDataReader ExecuteReader(string sql, CommandType commandType = CommandType.Text, SqlTransaction transaction = null, params SqlParameter[] pms)
        {
            SqlConnection con;
            SqlDataReader reader = null;

            if (transaction == null)
            {
                con = new SqlConnection(ConnectionString);
            }
            else
            {
                con = transaction.Connection;
            }

            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                if (pms != null)
                {
                    cmd.Parameters.AddRange(pms);
                }

                try
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    cmd.Transaction = transaction;
                    if (transaction == null)
                    {
                        reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                    else
                    {
                        reader = cmd.ExecuteReader();
                    }
                }
                catch (Exception ex)
                {
                    if (transaction == null)
                    {
                        con.Close();
                        con.Dispose();
                    }
                    throw ex;
                }
            }

            return reader;
        }

        //4.执行返回DataTable的方法
        public static DataTable ExecuteTable(string sql, CommandType commandType = CommandType.Text, SqlTransaction transaction = null, params SqlParameter[] pms)
        {
            DataTable dt = new DataTable();
            SqlConnection con;

            if (transaction == null)
            {
                con = new SqlConnection(ConnectionString);
            }
            else
            {
                con = transaction.Connection;
            }

            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, con))
            {
                if (pms != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(pms);
                }

                adapter.SelectCommand.Transaction = transaction;
                adapter.Fill(dt);
            }

            if (transaction == null)
            {
                con.Dispose();
            }

            return dt;
        }

        public static List<T> ExecuteList<T>(string sql, CommandType commandType = CommandType.Text, SqlTransaction transaction = null, params SqlParameter[] pms) where T : new()
        {
            // 创建一个新的列表
            List<T> lst = new List<T>();

            SqlDataReader reader = ExecuteReader(sql, commandType, transaction, pms);

            // 循环处理表中的每条数据，将其转换为对象并插入列表
            while (reader.Read())
            {
                // 创建对象
                T t = default(T);

                // 获取T的类型
                Type Ts = typeof(T);

                // 获取T的属性
                PropertyInfo[] infos = Ts.GetProperties();

                // 创建对象的实例
                t = new T();

                // 反射，为对象的属性赋值
                foreach (PropertyInfo info in infos)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (info.Name.ToLower() == reader.GetName(i).ToLower())
                        {
                            // 反射，为对象的属性赋值
                            if (reader[i] == DBNull.Value)
                            {
                                info.SetValue(t, null, null);
                            }
                            else
                            {
                                info.SetValue(t, reader[i], null);
                            }
                            break;
                        }
                    }
                }

                lst.Add(t);
            }

            reader.Close();

            return lst;
        }
    }

}
