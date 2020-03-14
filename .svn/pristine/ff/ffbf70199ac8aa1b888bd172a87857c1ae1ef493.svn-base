using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dllConfigApp;
using System.Data.Common;
using System.Data;
using System.Diagnostics;

namespace DAL
{
    public class SQLHelpDataBaseALL
    {
        private const string sErrCode = "错误代码:DA001";
        private ConfigApp configApp = new ConfigApp(AppDomain.CurrentDomain.BaseDirectory + @"LinkDataBase.config");
        string sMsg = string.Empty;
        ///<summary>
        ///说明：连接数据库的连接串
        /// </summary>
        public string strCon
        {
            get { return configApp["ConnectionString"].ToString(); }
        }
        /// <summary>
        /// 说明：数据库的Provider Name
        /// </summary>
        public string strProviderName
        {
            get { return configApp["ProviderName"].ToString(); }
        }

        private DbProviderFactory _objProviderFactory;
        public DbProviderFactory objProviderFactory
        {
            get { return _objProviderFactory; }
            set { _objProviderFactory = value; }
        }
        object vdbcon = new object();
        private DbConnection _dbcon;
        public DbConnection dbcon
        {
            get
            {
                lock (vdbcon)
                {
                    try
                    {
                        if (_dbcon == null)
                        {
                            objProviderFactory = DbProviderFactories.GetFactory(strProviderName);
                            _dbcon = objProviderFactory.CreateConnection();
                            _dbcon.ConnectionString = strCon;
                        }
                        else
                        {
                            if (_dbcon.ConnectionString.Trim().Length == 0)
                            {
                                _dbcon.ConnectionString = strCon;
                            }
                        }
                        return _dbcon;
                    }
                    catch (Exception ex)
                    {
                        sMsg = string.Format("异常:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                        throw new Exception(sMsg);
                    }
                }
            }
            set { _dbcon = value; }
        }
    
        /// <summary>
        /// 说明：传入SQL语句获取数据集
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        object vExecuteSql1 = new object();
        public DataSet ExecuteSql(string strSql)
        {
            lock (vExecuteSql1)
            {
                DataSet ds = new DataSet();
                try
                {
                    using (dbcon)
                    {
                        if (dbcon.State == ConnectionState.Closed)
                        {
                            dbcon.Open();
                        }
                        DbCommand cmd = dbcon.CreateCommand();
                        cmd.CommandText = strSql;
                        DbDataAdapter myAdapter = objProviderFactory.CreateDataAdapter();
                        myAdapter.SelectCommand = cmd;
                        myAdapter.Fill(ds);
                        dbcon.Close();
                    }
                }
                catch (Exception ex)
                {
                    sMsg = string.Format("异常:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                    //throw new Exception(sMsg);
                }
                return ds;
            }
        }

        /// <summary>
        /// 说明：传入SQL语句获取数据集
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        object vExecuteSql = new object();
        public DataSet ExecuteSql(string strSql,out string sMsg)
        {
            lock (vExecuteSql)
            {
                DataSet ds = new DataSet();
                sMsg = string.Empty;
                try
                {
                    using (dbcon)
                    {
                        DbCommand cmd = dbcon.CreateCommand();
                        cmd.CommandText = strSql;
                        DbDataAdapter myAdapter = objProviderFactory.CreateDataAdapter();
                        myAdapter.SelectCommand = cmd;
                        myAdapter.Fill(ds);
                    }
                }
                catch (Exception ex)
                {
                    sMsg = string.Format("异常:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                }
                return ds;
            }
        }

        /// <summary>
        /// 说明：SQL执行cmd
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        object vExecutCmd = new object();
        public bool ExecutCmd(string strSql,out string sMsg)
        {
            lock (vExecutCmd)
            {
                sMsg = string.Empty;
                try
                {
                    using (dbcon)
                    {
                        if (dbcon.State == ConnectionState.Closed)
                        {
                            dbcon.Open();
                        }
                        DbCommand cmd = dbcon.CreateCommand();
                        cmd.CommandText = strSql;
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    sMsg = string.Format("异常:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                    return false;
                }
            }
        }
        public int ExecutCmd1(string strSql, out string sMsg)
        {
            lock (vExecutCmd)
            {
                sMsg = string.Empty;
                try
                {
                    using (dbcon)
                    {
                        if (dbcon.State == ConnectionState.Closed)
                        {
                            dbcon.Open();
                        }
                        DbCommand cmd = dbcon.CreateCommand();
                        cmd.CommandText = strSql;
                        return cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    sMsg = string.Format("异常:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                    return 0;
                }
            }
        }
        /// <summary>
        /// 说明：SQL执行cmd
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        object vExecutCmd1 = new object();
        public bool ExecutCmd(string strSql)
        {
            lock (vExecutCmd1)
            {
                try
                {
                    using (dbcon)
                    {
                        if (dbcon.State == ConnectionState.Closed)
                        {
                            dbcon.Open();
                        }
                        DbCommand cmd = dbcon.CreateCommand();
                        cmd.CommandText = strSql;
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    sMsg = string.Format("异常:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                    throw new Exception(sMsg);
                }
            }
        }
        /// <summary>
        /// 说明：构建执行存储过程的cmd
        /// </summary>
        /// <param name="dbcon"></param>
        /// <param name="storedProcName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private DbCommand BuildCommand(DbConnection dbcon, string storedProcName, IDataParameter[] parameters)
        {
            try
            {
                DbCommand dbcmd = dbcon.CreateCommand();
                dbcmd.CommandType = CommandType.StoredProcedure;
                dbcmd.CommandText = storedProcName;
                foreach (DbParameter pr in parameters)
                {
                    if (pr.Value == null)
                    {
                        pr.Value = DBNull.Value;
                    }
                    dbcmd.Parameters.Add(pr);
                }
                return dbcmd;
            }
            catch(Exception ex)
            {
                sMsg = string.Format("异常:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                throw new Exception(sMsg);
            }
        }

        /// <summary>
        /// 说明：执行存储过程,返回DataSet
        /// </summary>
        /// <param name="stroreProcName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet RunProcedure(string stroreProcName, IDataParameter[] parameters)
        {
            DataSet ds = new DataSet();
            try
            {
                using (this.dbcon)
                {
                    if (dbcon.State == ConnectionState.Closed)
                    {
                        dbcon.Open();
                    }
                    DbDataAdapter dbada = objProviderFactory.CreateDataAdapter();
                    dbada.SelectCommand = BuildCommand(this.dbcon, stroreProcName, parameters);
                    dbada.Fill(ds);
                    this.dbcon.Close();
                }
                return ds;
            }
            catch (Exception ex)
            {
                sMsg = string.Format("异常:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                throw new Exception(sMsg);
            }
        }

        /// <summary>
        /// 说明：执行带有返回值的存储过程
        /// </summary>
        /// <param name="stroreProcName"></param>
        /// <param name="parameters"></param>
        /// <param name="ht"></param>
        public void RunProcedure(string stroreProcName, IDataParameter[] parameters, out System.Collections.Hashtable ht)
        {
            ht = new System.Collections.Hashtable();
            try
            {
                using (this.dbcon)
                {
                    if (dbcon.State == ConnectionState.Closed)
                    {
                        dbcon.Open();
                    }
                    DbCommand dbcmd = BuildCommand(this.dbcon, stroreProcName, parameters);
                    dbcmd.ExecuteNonQuery();//执行
                    foreach (DbParameter pr in parameters)
                    {
                        if (pr.Direction == ParameterDirection.Output || pr.Direction == ParameterDirection.InputOutput)
                        {
                            object vRtn = dbcmd.Parameters[pr.ParameterName].Value;
                            if (vRtn == null || vRtn == DBNull.Value)
                            {
                                vRtn = string.Empty;
                            }
                            ht.Add(pr.ParameterName, vRtn.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sMsg = string.Format("异常:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                throw new Exception(sMsg);
            }
        }
    }
}
