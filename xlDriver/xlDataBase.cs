using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

//打开SQL Server 2005的管理工具，选中需要创建存储过程的数据库，
//找到“可编程性”，展开后可以看到“存储过程”。右键点击它，
//选择“新建存储过程”，右侧的编辑窗口打开了，
//里面装着微软自动生成的SQL Server创建存储过程的语句。
namespace xlTools
{
    public class xlDataBase
    {
        #region 不使用
        /// <summary>
        /// 创建一个数据库
        /// </summary>
        /// <param name="DBName"></param>
        /// <returns></returns>
        private static Boolean Create(String fileName ,String DBName)
        {
            //打开数据库连接
            SqlConnection conn = new SqlConnection("Server=localhost;Integrated security=SSPI;database=master");
            
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            String path  = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path);

            String str = "CREATE DATABASE " + DBName  + " ON PRIMARY " +
            "(NAME = changhong, " +
            "FILENAME = " + "'" + fileName + ".mdf" + "', " +
            "SIZE = 3MB, MAXSIZE = 10MB, FILEGROWTH = 10%) " +
            "LOG ON (NAME = CHdb_Log, " +
            "FILENAME = " + "'" + fileName + ".ldf" + "', " +
            "SIZE = 1MB, " +
            "MAXSIZE = 5MB, " +
            "FILEGROWTH = 10%)";

            SqlCommand cmd = new SqlCommand(str, conn);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("数据库创建成功。");
            }
            catch (SqlException ae)
            {
                MessageBox.Show("数据库创建失败。" + ae.Message.ToString());
            }

            return false;
        }

        private static Boolean Create(String DBName, String TableName, String sqlstr)
        {
            //打开数据库连接
            SqlConnection conn = new SqlConnection("Server=localhost;Initial Catalog=" + DBName + ";Integrated security=SSPI");
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            String sql = "CREATE TABLE " + TableName + "(" + sqlstr + ")";

            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ae)
            {
                MessageBox.Show("表创建失败。" + ae.Message.ToString());    
            }

            conn.Close();
            return false;
        }

        private static Boolean Update(String DBName, String TableName, String sqlstr)
        {
            //打开数据库连接
            SqlConnection conn = new SqlConnection("Server=localhost;Initial Catalog=" + DBName + ";Integrated security=SSPI");
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            String sql = "update " + TableName + " set " + sqlstr;

            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ae)
            {
                MessageBox.Show("表创建失败。" + ae.Message.ToString());
            }
            conn.Close();
            return false;
        }

        private static Boolean Insert(String DBName, String TableInfo, String sqlstr)
        {
            //打开数据库连接
            SqlConnection conn = new SqlConnection("Server=localhost;Initial Catalog=" + DBName + ";Integrated security=SSPI");
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            String sql = "insert into " + TableInfo + " values(" + sqlstr + ")";

            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ae)
            {
                MessageBox.Show("表插入失败。" + ae.Message.ToString());
            }
            conn.Close();
            return false;
        }

        private static Boolean Select(String DBName, String TableName, out DataSet _ds)
        {
            //打开数据库连接
            SqlConnection conn = new SqlConnection("Server=localhost;Initial Catalog=" + DBName + ";Integrated security=SSPI");
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            String sql = "select * from " + TableName;

            SqlDataAdapter objDataAdpter = new SqlDataAdapter();
            objDataAdpter.SelectCommand = new SqlCommand(sql, conn);

            _ds = new DataSet();
            DataSet ds = new DataSet();

            try
            {
                objDataAdpter.Fill(ds, TableName);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    foreach (DataColumn mDc in ds.Tables[0].Columns)
                    {
                        mDr[mDc] = mDr[mDc].ToString().Trim();
                    }
                }
                
                _ds = ds;
            }
            catch (SqlException ae)
            {
                MessageBox.Show("表创建失败。" + ae.Message.ToString());
            }
            conn.Close();
            return false;
        }
        #endregion

        #region 最新的数据库访问API
        public static String Read(String DBName, String TableName, String strWhere, String strIndex, String defaultValue)
        {
            String connStr = "Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True";
            String strSql = "select * from " + TableName + " " + strWhere;
            String Result = "";
            
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader sqlDr = comm.ExecuteReader();

                        //从reader中读取下一行数据,如果没有数据,reader.Read()返回flase  
                        //sqlDr.Read();
                        if (sqlDr.Read())
                        {
                            Result = (String)sqlDr[strIndex];
                        }  
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        comm.Dispose();
                    }
                }
            }

            return Result.Trim();
        }

        public static Byte[] Read(String DBName, String TableName, String strWhere, String strIndex)
        {
            String connStr = "Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True";
            String strSql = "select * from " + TableName +  " " + strWhere;
            Byte[] Result = new Byte[0];
            
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader sqlDr = comm.ExecuteReader();
                        //从reader中读取下一行数据,如果没有数据,reader.Read()返回flase  
                        //sqlDr.Read();
                        if (sqlDr.Read())
                        {
                            Result = (Byte[])sqlDr[strIndex];
                        }
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        comm.Dispose();
                    }
                }
            }
            return Result;
        }

        public static DataSet ReadAll(String DBName, String TableName)
        {
            String connStr = "Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True";
            String strSql = "select * from " + TableName;
            DataSet Result = new DataSet();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        //conn.Open();
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = comm;

                        sda.Fill(Result);
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return Result;
        }
        
        public static DataSet ReadAll(String DBName, String TableName, String strWhere)
        {
            String connStr = "Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True";
            String strSql = "select * from " + TableName;
            if ((strWhere.Trim() != "") && (strWhere != null))
            {
                strSql += " where " + strWhere;
            }

            DataSet Result = new DataSet();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = comm;

                        sda.Fill(Result);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        comm.Dispose();
                    }
                }
                
            }
            return Result;
        }
        
        #region 获取分页的数据 在用
        /// <summary>
        /// 获取分页的数据
        /// </summary>
        /// <param name="TblName">数据表名</param>
        /// <param name="Fields">要读取的字段 不能为空</param>
        /// <param name="OrderField">排序字段</param>
        /// <param name="SqlWhere">查询条件 可以为空</param>
        /// <param name="PageSize">每页显示多少条数据</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="TotalPage">返回值，共有多少页</param>
        /// <returns></returns>
        /// sosoft.cnblogs.com
        public static DataSet ReadPage(String DBName, String TableName, String Fields, String OrderField, String SqlWhere, int PageSize, int pageIndex, out int TotalPage)
        {
            TotalPage = 0;
            String connString = "Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True";
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand comm = new SqlCommand("Page", conn))
                {
                    try
                    {
                        comm.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar, 100));
                        comm.Parameters[0].Value = TableName;

                        comm.Parameters.Add(new SqlParameter("@Fields", SqlDbType.NVarChar, 1000));
                        comm.Parameters[1].Value = Fields;

                        comm.Parameters.Add(new SqlParameter("@OrderField", SqlDbType.NVarChar, 1000));
                        comm.Parameters[2].Value = OrderField;

                        comm.Parameters.Add(new SqlParameter("@sqlWhere", SqlDbType.NVarChar, 1000));
                        comm.Parameters[3].Value = SqlWhere;

                        comm.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.Int));
                        comm.Parameters[4].Value = PageSize;

                        comm.Parameters.Add(new SqlParameter("@pageIndex", SqlDbType.Int));
                        comm.Parameters[5].Value = pageIndex;


                        comm.Parameters.Add(new SqlParameter("@TotalPage", SqlDbType.Int));
                        comm.Parameters[6].Direction = ParameterDirection.Output;

                        comm.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(comm);
                        dataAdapter.Fill(ds);
                        TotalPage = (int)comm.Parameters[6].Value;
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        comm.Dispose();
                    }
                }
            }
            return ds;
        }
        #endregion

        #region 获取分页的数据
        /// <summary>
        /// 获取分页的数据
        /// </summary>
        /// <param name="TblName">数据表名</param>
        /// <param name="Fields">要读取的字段</param>
        /// <param name="OrderField">排序字段</param>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="PageSize">每页显示多少条数据</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="TotalPage">返回值，共有多少页</param>
        /// <returns></returns>
        /// sosoft.cnblogs.com
        private static DataSet PageData(String DBName, String TableName, String Fields, string OrderField, string SqlWhere, int PageSize, int pageIndex, out int TotalPage)
        {
            TotalPage = 0;
            String connString = "Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True";
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand comm = new SqlCommand("Page", conn))
                {
                    try
                    {
                        comm.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar, 100));
                        comm.Parameters[0].Value = TableName;

                        comm.Parameters.Add(new SqlParameter("@Fields", SqlDbType.NVarChar, 1000));
                        comm.Parameters[1].Value = Fields;

                        comm.Parameters.Add(new SqlParameter("@OrderField", SqlDbType.NVarChar, 1000));
                        comm.Parameters[2].Value = OrderField;

                        comm.Parameters.Add(new SqlParameter("@sqlWhere", SqlDbType.NVarChar, 1000));
                        comm.Parameters[3].Value = SqlWhere;

                        comm.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.Int));
                        comm.Parameters[4].Value = PageSize;

                        comm.Parameters.Add(new SqlParameter("@pageIndex", SqlDbType.Int));
                        comm.Parameters[5].Value = pageIndex;


                        comm.Parameters.Add(new SqlParameter("@TotalPage", SqlDbType.Int));
                        comm.Parameters[6].Direction = ParameterDirection.Output;
                                           
                        comm.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(comm);
                        dataAdapter.Fill(ds);
                        TotalPage = (int)comm.Parameters[6].Value;
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        comm.Dispose();

                    }
                }
            }
            return ds;
        }
        #endregion
        
        public static void Replace(String DBName, String TableName, String strWhere, String strIndex, String strValue)
        {
            String connStr = "Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True";
            String strSql = "update " + TableName + " set " + strIndex + " = @" + strIndex + " " + strWhere;
            
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        conn.Open();

                        SqlParameter par = new SqlParameter("@" + strIndex, SqlDbType.NChar);
                        par.Value = strValue;
                        comm.Parameters.Add(par);
                        comm.ExecuteNonQuery();

                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        comm.Dispose();
                    }
                }
            }
        }

        public static void Replace(String DBName, String TableName, String sqlWhere, String[] sqlItems)
        {
            if (sqlWhere.Trim() == "")
            {
                throw new NullReferenceException();
            }
            if (sqlItems == null)
            {
                throw new NullReferenceException();
            }
            String items = "";
            foreach (String str in sqlItems)
            {
                items += str + ",";
            }
            //Int32 startIndex = items.Length - 1;
            items = items.Remove(items.Length - 1);
            String connStr = "Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True";
            String strSql = "update " + TableName + " set " + items + " where " + sqlWhere;
            
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                            
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        comm.Dispose();
                    }
                }
            }
        }

        public static void Replace(String DBName, String TableName, String strWhere, String strIndex, Byte[] byteValue)
        {
            String connStr = "Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True";
            String strSql = "update " + TableName + " set " + strIndex + " = @" + strIndex + " " + strWhere;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        conn.Open();

                        SqlParameter par = new SqlParameter("@" + strIndex, SqlDbType.Image);
                        par.Value = byteValue;
                        comm.Parameters.Add(par);
                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        comm.Dispose();
                    }
                }
            }
        }
        
        public static Boolean CreateDB(String DBName)
        {
            //打开数据库连接
            SqlConnection conn = new SqlConnection("Data Source=(local);Integrated security=SSPI;Integrated Security=True");

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            String strSql = "CREATE DATABASE " + DBName;

            SqlCommand cmd = new SqlCommand(strSql, conn);
            try
            {
                cmd.ExecuteNonQuery();
                //MessageBox.Show("数据库创建成功。");
            }
            catch (SqlException)
            {
                throw;
            }

            return false;
        }

        public static Boolean CreateTable(String DBName,String TableName,String TableItems)
        {
            //打开数据库连接
            SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True");

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            String strSql = "CREATE TABLE " + TableName + "(" + TableItems + ")";

            SqlCommand cmd = new SqlCommand(strSql, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw;
            }

            return false;
        }

        public static Boolean Insert(String DBName, String TableName, String TableItems, String Values)
        {
            //打开数据库连接
            String strSql = "INSERT INTO " + TableName + "(" + TableItems + ") VALUES (" + Values + ")";
            
            using (SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True"))
            {
                using (SqlCommand comm = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        comm.Dispose();
                    }
                }

            }
            return false;
        }

        /// <summary>
        /// 删除数据库数据
        /// </summary>
        /// <param name="DBName"></param>
        /// <param name="TableName"></param>
        /// <param name="strWhere">为"" 或null删除所有内容</param>
        public static void Delete(String DBName, String TableName, String strWhere)
        {
            try
            {
                //打开数据库连接
                SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True");

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                String strSql = "delete from " + TableName;
                if ((strWhere.Trim() != "") && (strWhere != null))
                {
                    strSql += " where " + strWhere;
                }

                SqlCommand cmd = new SqlCommand(strSql, conn);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }


        #region 20171215添加
        public static DataSet Last(String DBName, String TableName, String OrderItemLable)
        {
            //打开数据库连接
            String connString = "Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True";
            String strSql = String.Format("select top 1 * from {0} order by {1} desc", TableName, OrderItemLable);
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand comm = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = comm;

                        sda.Fill(ds);
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        comm.Dispose();
                    }
                }
            }

            return ds;
        }
        public static DataSet Last(String DBName, String TableName, String OrderItemLable ,String Fields)
        {
            //打开数据库连接
            String connString = "Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True";
            String strSql = String.Format("select top 1 {2} from {0} order by {1} desc", TableName, OrderItemLable, Fields);
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand comm = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = comm;

                        sda.Fill(ds);
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        comm.Dispose();
                    }
                }
            }

            return ds;
        }

        public static DataSet First(String DBName, String TableName, String ItemLable)
        {
            //打开数据库连接
            String connString = "Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True";
            String strSql = String.Format("select top 1 * from {0} order by {1} asc", TableName, ItemLable);
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand comm = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = comm;

                        sda.Fill(ds);
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        comm.Dispose();
                    }
                }
            }

            return ds;
        }
        public static DataSet First(String DBName, String TableName, String ItemLable, String Fields)
        {
            //打开数据库连接
            String connString = "Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True";
            String strSql = String.Format("select top 1 {2} from {0} order by {1} asc", TableName, ItemLable, Fields);
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand comm = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = comm;

                        sda.Fill(ds);
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        comm.Dispose();
                    }
                }
            }

            return ds;
        }

        #endregion

        #region 20171218添加
        public static DataSet Last(String DBName, String TableName, String OrderItemLable, String Fields, String SqlWhere)
        {
            //打开数据库连接
            String connString = "Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True";
            String strWhere = "";
            if (SqlWhere != "")
            {
                strWhere = " where " + SqlWhere;
            }
            String strSql = String.Format("select top 1 {2} from {0} {3} order by {1} desc", TableName, OrderItemLable, Fields, strWhere);
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand comm = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = comm;

                        sda.Fill(ds);
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        comm.Dispose();
                    }
                }
            }

            return ds;
        }

        public static DataSet First(String DBName, String TableName, String OrderItemLable, String Fields, String SqlWhere)
        {
            //打开数据库连接
            String connString = "Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True";
            String strWhere = "";
            if (SqlWhere != "")
            {
                strWhere = " where " + SqlWhere;
            }
            String strSql = String.Format("select top 1 {2} from {0} {3} order by {1} asc", TableName, OrderItemLable, Fields, strWhere);
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand comm = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = comm;

                        sda.Fill(ds);
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        comm.Dispose();
                    }
                }
            }

            return ds;
        }

        //public static Boolean Insert(String DBName, String TableName, DataSet _ds)
        //{
        //    //打开数据库连接
        //    String strSql = "INSERT INTO " + TableName + "(" + TableItems + ") VALUES (" + Values + ")";
        //    //SqlConnection sc = new SqlConnection("数据库连接字符串");
        //    //sc.Open();
        //    //SqlBulkCopy SqlBulkCopy sbc = new SqlBulkCopy(sc);
        //    //sbc.DestinationTableName = "数据库名.dbo.表名";//你想往哪个数据库里的，哪个表，的里面插数据，就填哪个表的完整名
        //    //sbc.WriteToServer(ds.Tables[0]);//这句才是关键，顶TM各种循环，顶你到你爽乎！
        //    //sc.Close(); 
        //    using (SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=" + DBName + ";Integrated Security=True"))
        //    {
        //        using (SqlCommand comm = new SqlCommand(strSql, conn))
        //        {
        //            try
        //            {
        //                conn.Open();
        //                comm.ExecuteNonQuery();
        //            }
        //            catch (SqlException)
        //            {
        //                throw;
        //            }
        //            finally
        //            {
        //                conn.Close();
        //                conn.Dispose();
        //                comm.Dispose();
        //            }
        //        }

        //    }
        //    return false;
        //}

        #endregion
        #endregion
    }
}
