using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace DAL
{
    public class SQLHelpAccess
    {
        public SQLHelpAccess()
        {
        }
        /// <summary>
        /// 说明：数据保存文件名
        /// </summary>
        private string _DatabaseName = "db.accdb";
        /// <summary>
        /// 说明：数据库文件加载的路径
        /// </summary>
        private string mLoadDataBaseName
        {
            get { return System.AppDomain.CurrentDomain.BaseDirectory + _DatabaseName; }
        }

        private System.Data.OleDb.OleDbConnection _dbcon;
        public System.Data.OleDb.OleDbConnection dbcon
        {
            get
            {
                try
                {
                    if (_dbcon == null)
                    {
                        _dbcon = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = " + mLoadDataBaseName);
                    }
                    else
                    {
                        if (_dbcon.ConnectionString.Trim().Length == 0)
                        {
                            _dbcon.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source = " + mLoadDataBaseName;
                        }
                    }
                    return _dbcon;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            set { _dbcon = value; }
        }

        /// <summary>
        /// 说明：从数据库中查询数据，并返回给DataSet中。
        /// </summary>
        /// <param name="sSql"></param>
        /// <param name="ds"></param>
        public DataSet ExecuteSql(string sSql)
        {
            DataSet ds = new DataSet();
            try
            {
                using (dbcon)
                {
                    if (dbcon.State == ConnectionState.Closed)
                        dbcon.Open();
                    System.Data.OleDb.OleDbCommand myComm = new System.Data.OleDb.OleDbCommand(sSql, dbcon);
                    System.Data.OleDb.OleDbDataAdapter ada = new System.Data.OleDb.OleDbDataAdapter(myComm);
                    ada.Fill(ds);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 说明：保存数据到数据库中。
        /// </summary>
        /// <param name="sSql"></param>
        /// <returns></returns>
        public bool ExecutCmd(string sSql)
        {
            try
            {
                using (dbcon)
                {
                    if (dbcon.State == ConnectionState.Closed)
                        dbcon.Open();
                    System.Data.OleDb.OleDbCommand myComm = new System.Data.OleDb.OleDbCommand(sSql, dbcon);
                    myComm.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
