using System;
using System.Collections.Generic;
using System.Data.SqlClient;//引用数据库客户端
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace SmartMedical.DAL
{
    public class DBHelper
    {
        IConfiguration _config;
        public DBHelper(IConfiguration config)
        {
            _config = config;
        }
        public string connstr { get { return _config.GetConnectionString("sqlconnstr"); } set { } }
        /// <summary>
        /// 返回受影响行数  
        /// 添加、删除、修改
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                try
                {
                    //打开
                    //判断状态
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    //命令对象
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    int n = cmd.ExecuteNonQuery();
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    return n;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }


        public DataSet GetDateSet( string sql) 
        {
            using (SqlConnection conn=new SqlConnection(connstr))
            {
                SqlCommand cmd = new SqlCommand(sql,conn);
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
                return ds;
            }
        }

        /// <summary>
        /// 返回受影响行数
        /// 添加、删除、修改
        /// </summary>
        /// <param name="procName">存储过程名字</param>
        /// <param name="parameter">存储过程需要的参数</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string procName, SqlParameter[] parameter = null)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                try
                {
                    //打开
                    //判断状态
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    //命令对象
                    SqlCommand cmd = new SqlCommand(procName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameter);
                    int n = cmd.ExecuteNonQuery();

                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    return n;
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

        #region 1

        public List<T> TableToList<T>(DataTable dt, bool isStoreDB = true)
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            //List<string> listColums = new List<string>();
            PropertyInfo[] pArray = type.GetProperties(); //集合属性数组
            foreach (DataRow row in dt.Rows)
            {
                T entity = Activator.CreateInstance<T>(); //新建对象实例 
                foreach (PropertyInfo p in pArray)
                {
                    if (!dt.Columns.Contains(p.Name) || row[p.Name] == null || row[p.Name] == DBNull.Value)
                    {
                        continue;  //DataTable列中不存在集合属性或者字段内容为空则，跳出循环，进行下个循环   
                    }
                    if (isStoreDB && p.PropertyType == typeof(DateTime) && Convert.ToDateTime(row[p.Name]) < Convert.ToDateTime("1753-01-01"))
                    {
                        continue;
                    }
                    try
                    {
                        var obj = Convert.ChangeType(row[p.Name], p.PropertyType);//类型强转，将table字段类型转为集合字段类型  
                        p.SetValue(entity, obj, null);
                    }
                    catch (Exception)
                    {
                        // throw;
                    }
                }
                list.Add(entity);
            }
            return list;
        }



        #endregion

        /// <summary>
        /// 返回首行首列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                try
                {
                    //打开
                    //判断状态
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    //命令对象
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    object n = cmd.ExecuteScalar();
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    return n;
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

    }
}
