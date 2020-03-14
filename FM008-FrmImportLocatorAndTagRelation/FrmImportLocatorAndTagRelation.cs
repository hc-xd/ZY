using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using operateExcel;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace NFrmImportLocatorAndTagRelation
{
    public partial class DFrmImportLocatorAndTagRelation : Form
    {
        private const string sErrCode = "错误代码:DFrmImportLocatorAndTagRelation";
        DAL.SQLHelpDataBase myShd;
        dllConfigApp.ConfigApp mycfg;
        string mysUser = string.Empty;
        string fileName = string.Empty;
        //  dllExcelImportAndExport.ExcelImportAndExport impExcel = new dllExcelImportAndExport.ExcelImportAndExport();
        operateExcel.Class1 operateExcel = new operateExcel.Class1();
        string ConnectStr = string.Empty;
        string sMsg = string.Empty;
        public DFrmImportLocatorAndTagRelation(string sUser, dllConfigApp.ConfigApp cfg, DAL.SQLHelpDataBase shd)
        {
            InitializeComponent();
            mysUser = sUser;
            mycfg = cfg;
            myShd = shd;
        }
        object objExcuteMySql = new object();
        private DataSet ExcuteMySql(string str, out string errorMsg)
        {
            lock (objExcuteMySql)
            {
                errorMsg = string.Empty;
                DataSet dt = new DataSet();
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectStr))
                    {
                        if (mySqlConnection.State == ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }
                        MySqlCommand mySqlCommand = new MySqlCommand(str, mySqlConnection);
                        MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                        mda.Fill(dt);
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    errorMsg = ex.ToString();
                    return dt;
                }
            }
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            string errorMsg = string.Empty;
            
            openFileDialog1.Filter = "Excel|*.xlsx|Excel|*.xls|所有文件|*.*";
            DialogResult dr = openFileDialog1.ShowDialog();
            openFileDialog1.RestoreDirectory = true;
            textBox1.Text = openFileDialog1.FileName;
            if (!System.IO.File.Exists(openFileDialog1.FileName))
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "选定的文件不存在.");
            }
            else
            {                
                DataTable dt = operateExcel.ExcelToDatatable(openFileDialog1.FileName,out fileName, out errorMsg);
                if (errorMsg != "")
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "异常：" + errorMsg);
                }
                dataGridViewX1.DataSource = dt;
               // ImportExcelTagLocatorToDataBase(this.dataGridViewX1, openFileDialog1.FileName);
            }
        }
        /// <summary>
        /// 说明：把数据导入到数据库中
        /// </summary>
        /// <param name="sPathFile"></param>
        private void ImportExcelTagLocatorToDataBase(DevComponents.DotNetBar.Controls.DataGridViewX dvx1, string sPathFile)
        {
            //string sMsg = string.Empty;
            //DataSet ds = impExcel.ExcelToDS(sPathFile, out sMsg);
            //if (sMsg.Trim().Length != 0)
            //{
            //    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, sMsg);
            //}
            //else
            //{
            //    lblTotal.Text = ds.Tables[0].Rows.Count.ToString();//合计的总数
            //    if (ds.Tables.Count > 0)
            //    {
            //        dvx1.DataSource = ds.Tables[0];
            //        LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, "查询成功。");
            //        for (int i = 0; i < dvx1.Rows.Count; i++)
            //        {
            //            if (i % 2 == 0)
            //            {
            //                dvx1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "Excel中没有任何数据.");
            //    }
            //}
        }
        private void InsertIntoPickTable(string pickOrder,string itemCode,string itemBase,string locator,string productionName,string production,string requireQuantity,int rowLine)
        {
            string errorMsg = string.Empty;
            try
            {            
                string str1 = string.Format(@"insert into t_bp_detail(item_code,locator,require_quantity,item_unit,item_desc,orderNo)
                                             values('{0}','{1}',{2},'{3}','{4}','{5}')", itemCode, locator, Convert.ToInt32(requireQuantity), itemBase, pickOrder, productionName);
                ExcuteMySql(str1, out errorMsg);
                if (errorMsg == "")
                {                    
                    dataGridViewX1.Rows[rowLine].DefaultCellStyle.BackColor = Color.Green;//标识为红色
                    HandleLabel(1);
                }
                else
                {
                    sMsg = string.Format("{0}1-{1}", sErrCode, sMsg);
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, sMsg);
                    dataGridViewX1.Rows[rowLine].DefaultCellStyle.BackColor = Color.Red;//标识为红色
                    HandleLabel(0);
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "异常：" + ex.ToString());
            }
        }
        private void ImportPickTable()
        {
            try
            {
                lblOK.Text = "0";
                lblFail.Text = "0";
                if (dataGridViewX1.Rows.Count == 0)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "没有任何数据可以导入.");
                }
                else
                {
                    string str = string.Format("delete from t_bp_detail");
                    ExcuteMySql(str, out sMsg);
                    if (sMsg != "")
                    {
                        LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "异常：" + sMsg);
                    }
                    dllUcShowWnDThread.UcShowWnDThread uc = new dllUcShowWnDThread.UcShowWnDThread("正在处理，请稍后……", null);
                    uc.DoWork = delegate
                    {
                        for (int i = 0; i < this.dataGridViewX1.Rows.Count; i++)
                        {
                            string pickOrder = this.dataGridViewX1.Rows[i].Cells["出库单号"].Value.ToString();
                            string itemCode = this.dataGridViewX1.Rows[i].Cells["物料编码"].Value.ToString();
                            string itemBase = this.dataGridViewX1.Rows[i].Cells["物料规格"].Value.ToString();
                            string locator = this.dataGridViewX1.Rows[i].Cells["库位"].Value.ToString();
                            string requireQuantity = this.dataGridViewX1.Rows[i].Cells["出库数量"].Value.ToString();
                            string productionName = this.dataGridViewX1.Rows[i].Cells["部品号"].Value.ToString();
                            string production = this.dataGridViewX1.Rows[i].Cells["工艺"].Value.ToString();
                            InsertIntoPickTable(production, itemCode, itemBase, locator, productionName, production, requireQuantity, i);
                        }
                    };
                    uc.RunAndShow();
                }
                this.dataGridViewX1.DataSource = null;
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "异常：" + ex.ToString());
            }            
        }
        private void InsertIntoProductionDetail(string production,string BP,int id)
        {
            try
            {
                string str2 = string.Format("select * from t_BP_detail where item_desc='{0}' and orderNo like '%{1}%'", production, BP);
                DataSet dt = ExcuteMySql(str2, out sMsg);
                if (sMsg == "")
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                        {
                            string itemCode = dt.Tables[0].Rows[i]["item_code"].ToString();
                            string locator = dt.Tables[0].Rows[i]["locator"].ToString();
                            string req_quantity = dt.Tables[0].Rows[i]["require_quantity"].ToString();
                            string str3 = string.Format(@"insert into t_production_detail(ProductionId,ItemCode,Locator,RequireQuantity,Flag)
                                                                       values({3},'{0}','{1}',{2},0)", itemCode, locator, Convert.ToInt32(req_quantity),id);
                            ExcuteMySql(str3,out sMsg);
                            if (sMsg != "")
                            {
                                LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"异常："+sMsg);
                            }
                        }
                    }
                }
            }
            catch
            { 
            
            }
        }
        private int GetInsertId(string production, string productionName, string date)
        {
            string sMsg = string.Empty;
            try
            {
                string str = string.Format("select ID from t_production where production='{0}' and productionName='{1}' and date='{2}'",production,productionName,date);
                DataSet dt = ExcuteMySql(str,out sMsg);
                if (sMsg == "")
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        int id = Convert.ToInt32(dt.Tables[0].Rows[0]["ID"].ToString());
                        return id;
                    }
                    
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }
        private void ImportProduction()
        {
            try
            {
                lblOK.Text = "0";
                lblFail.Text = "0";
                if (dataGridViewX1.Rows.Count == 0)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "没有任何数据可以导入.");
                }
                else
                {
                    string str1 = string.Format("delete from T_PRODUCTION;delete from t_production_detail");
                    ExcuteMySql(str1,out sMsg);
                    if (sMsg != "")
                    {
                        LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "异常：" + sMsg);
                        return;
                    }
                    dllUcShowWnDThread.UcShowWnDThread uc = new dllUcShowWnDThread.UcShowWnDThread("正在处理，请稍后……", null);
                    uc.DoWork = delegate
                    {
                        for (int i = 0; i < this.dataGridViewX1.Rows.Count; i++)
                        {
                            //  string production = this.dataGridViewX1.Rows[i].Cells[""].Value.ToString();
                            string date = this.dataGridViewX1.Rows[i].Cells[0].Value.ToString();
                            string production = this.dataGridViewX1.Rows[i].Cells[this.dataGridViewX1.Rows[i].Cells.Count - 1].Value.ToString();
                            for (int j = 1; j < this.dataGridViewX1.Rows[i].Cells.Count-1; j++)
                            {
                                string BP = this.dataGridViewX1.Rows[i].Cells[j].Value.ToString();
                                if (BP == "" || BP.IndexOf("Column") >= 0)
                                {
                                    continue;
                                }
                                else
                                {
                                    string str = string.Format("insert into T_PRODUCTION(production,productionName,date) values('{0}','{1}','{2}')",production,BP,date);
                                    ExcuteMySql(str,out sMsg);
                                    if (sMsg == "")
                                    {
                                        string bp = string.Empty;
                                        if (BP.IndexOf("/") >= 0)
                                        {
                                            string[] str_1 = BP.Split('/');
                                            if (str_1[0].IndexOf("-") >= 0)
                                            {
                                                string[] str_2 = str_1[0].Split('-');
                                                bp = str_2[0];
                                            }
                                            else
                                            {
                                                bp = str_1[0];
                                            }
                                        }
                                        else if (BP.IndexOf("(") >= 0)
                                        {
                                            string[] str_1 = BP.Split('(');
                                            if (str_1[0].IndexOf(" ") >= 0)
                                            {
                                                string[] str_2 = str_1[0].Split(' ');
                                                bp = str_2[0];
                                            }
                                        }
                                        else if (BP.IndexOf("（") >= 0)
                                        {
                                            string[] str_1 = BP.Split('（');
                                            if (str_1[0].IndexOf(" ") >= 0)
                                            {
                                                string[] str_2 = str_1[0].Split(' ');
                                                bp = str_2[0];
                                            }
                                        }
                                        else
                                        {
                                            bp = production;
                                        }
                                        int id = GetInsertId(production,BP,date);
                                        InsertIntoProductionDetail(production, bp,id);
                                        dataGridViewX1.Rows[i].DefaultCellStyle.BackColor = Color.Green;//标识为红色
                                        HandleLabel(1);
                                    }
                                    else
                                    {
                                        sMsg = string.Format("{0}1-{1}", sErrCode, sMsg);
                                        LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, sMsg);
                                        dataGridViewX1.Rows[i].DefaultCellStyle.BackColor = Color.Red;//标识为红色
                                        HandleLabel(0);
                                    }
                                }
                            }                 
                        }
                    };
                    uc.RunAndShow();
                }
                this.dataGridViewX1.DataSource = null;
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "ImportProduction 异常：" + ex.ToString());
            }            
        }
        private void ImportBaseData()
        {
            try
            {
                lblOK.Text = "0";
                lblFail.Text = "0";
                if (dataGridViewX1.Rows.Count == 0)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "没有任何数据可以导入.");
                }
                else
                {

                    dllUcShowWnDThread.UcShowWnDThread uc = new dllUcShowWnDThread.UcShowWnDThread("正在处理，请稍后……", null);
                    uc.DoWork = delegate
                    {
                        for (int i = 0; i < this.dataGridViewX1.Rows.Count; i++)
                        {

                            string sREGION_NO = this.dataGridViewX1.Rows[i].Cells["REGION_NO"].Value.ToString();
                            string sCOM_ID = this.dataGridViewX1.Rows[i].Cells["COM_ID"].Value.ToString();
                            string sCOM_ID_IP = this.dataGridViewX1.Rows[i].Cells["COM_ID_IP"].Value.ToString();
                            string sFINISHER_ID = this.dataGridViewX1.Rows[i].Cells["FINISHER_ID"].Value.ToString();
                            string sFINISHER_ID_IP = this.dataGridViewX1.Rows[i].Cells["FINISHER_ID_IP"].Value.ToString();
                            string sORDER_ID = this.dataGridViewX1.Rows[i].Cells["ORDER_ID"].Value.ToString();
                            string sORDER_ID_IP = this.dataGridViewX1.Rows[i].Cells["ORDER_ID_IP"].Value.ToString();
                            string sAISLE_LAMP_ID = this.dataGridViewX1.Rows[i].Cells["AISLE_LAMP_ID"].Value.ToString();
                            string sAISLE_LAMP_ID_IP = this.dataGridViewX1.Rows[i].Cells["AISLE_LAMP_ID_IP"].Value.ToString();
                            string sTAG_ID = this.dataGridViewX1.Rows[i].Cells["TAG_ID"].Value.ToString();
                            string sTAG_ID_IP = this.dataGridViewX1.Rows[i].Cells["TAG_ID_IP"].Value.ToString();
                            string sLOCATOR = this.dataGridViewX1.Rows[i].Cells["LOCATOR"].Value.ToString();
                            ForeachInsertData(i, sREGION_NO, sCOM_ID, sCOM_ID_IP, sFINISHER_ID, sFINISHER_ID_IP, sORDER_ID,
                                                sORDER_ID_IP, sAISLE_LAMP_ID, sAISLE_LAMP_ID_IP, sTAG_ID, sTAG_ID_IP, sLOCATOR);

                        }
                    };
                    uc.RunAndShow();
                }
                this.dataGridViewX1.DataSource = null;
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "ImportBaseData 异常" + ex.ToString());
            }
            
        }
        private void InsertIntoStock(string itemCode,string locator,int i)
        {
            string errorMsg = string.Empty;
            try
            {
                
                string str1 = string.Format("insert into t_stock(ItemCode,Locator) values('{0}','{1}')",itemCode,locator);
                ExcuteMySql(str1,out errorMsg);
                if (errorMsg == "")
                {
                  //  LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("标签:ID:{0} IP:{1},导入成功", TAG_ID, TAG_ID_IP));
                    dataGridViewX1.Rows[i].DefaultCellStyle.BackColor = Color.Green;//标识为红色
                    HandleLabel(1);
                }
                else
                {
                    sMsg = string.Format("{0}1-{1}", sErrCode, sMsg);
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, sMsg);
                    dataGridViewX1.Rows[i].DefaultCellStyle.BackColor = Color.Red;//标识为红色
                    HandleLabel(0);
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "InsertIntoStock 异常："+ ex.ToString());
            }
        }
        private void ImportStock()
        {
            try
            {
                lblOK.Text = "0";
                lblFail.Text = "0";
                string errorMsg = string.Empty;
                if (dataGridViewX1.Rows.Count == 0)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "没有任何数据可以导入.");
                }
                else
                {
                    string str = string.Format("delete from t_stock");
                    ExcuteMySql(str, out errorMsg);
                    if (errorMsg != "")
                    {
                        LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "异常：" + errorMsg);
                        return;
                    }
                    dllUcShowWnDThread.UcShowWnDThread uc = new dllUcShowWnDThread.UcShowWnDThread("正在处理，请稍后……", null);
                    uc.DoWork = delegate
                    {
                        for (int i = 0; i < this.dataGridViewX1.Rows.Count; i++)
                        {
                            string itemCode = this.dataGridViewX1.Rows[i].Cells[0].Value.ToString();
                            string locator = this.dataGridViewX1.Rows[i].Cells[1].Value.ToString();
                            InsertIntoStock(itemCode,locator,i);            
                        }
                    };
                    uc.RunAndShow();
                }
                this.dataGridViewX1.DataSource = null;
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "ImportStock 异常："+ex.ToString());
            }
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) //导入生产计划表
            {
                if (fileName == "生产计划表")
                {
                    ImportProduction();
                }
                else
                {
                    MessageBox.Show("请重新选择正确的表格数据导入");
                }
            }
            else if (comboBox1.SelectedIndex == 1)//导入部品信息
            {
                if (fileName == "部品明细表")
                {
                    ImportPickTable();
                }
                else
                {
                    MessageBox.Show("请重新选择正确的表格数据导入");
                }                
            }
            else if (comboBox1.SelectedIndex == 2) //电子标签基础数据
            {
                if (fileName == "基础数据表")
                {
                    ImportBaseData();
                }
                else
                {
                    MessageBox.Show("请重新选择正确的表格数据导入");
                }                                             
            }
            else if (comboBox1.SelectedIndex == 3)//入库基础信息
            {
                if (fileName == "入库信息表")
                {
                    ImportStock();
                }
                else
                {
                    MessageBox.Show("请重新选择正确的表格数据导入");
                }                
            }
            else
            {
                return;
            }
        }
        private delegate void deleForeachInsertData(int i, string REGION_NO, string COM_ID, string COM_ID_IP, string FINISHER_ID, string FINISHER_ID_IP, string ORDER_ID,
                                      string ORDER_ID_IP, string AISLE_LAMP_ID, string AISLE_LAMP_ID_IP, string TAG_ID, string TAG_ID_IP, string LOCATOR);
        private void ForeachInsertData(int i, string REGION_NO, string COM_ID, string COM_ID_IP, string FINISHER_ID, string FINISHER_ID_IP, string ORDER_ID,
                                       string ORDER_ID_IP, string AISLE_LAMP_ID, string AISLE_LAMP_ID_IP, string TAG_ID, string TAG_ID_IP, string LOCATOR)
        {
            try
            {
                System.Windows.Forms.Application.DoEvents();
                string sSQL = string.Empty;
                //先查找一次对应的货位是否存在
                sSQL = string.Format("select TAG_ID from T_TAG_LOCATOR where  TAG_ID = '{0}'and TAG_ID_IP='{1}'", TAG_ID,TAG_ID_IP);
                DataSet dsCheck = ExcuteMySql(sSQL, out sMsg);
                if (dsCheck != null && dsCheck.Tables.Count > 0)
                {
                    if (dsCheck.Tables[0].Rows.Count > 0)
                    {
                        LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("标签:id:{0} ip:{1} 已经存在.", TAG_ID,TAG_ID_IP));
                        //删除记录
                        sSQL = string.Format("delete from T_TAG_LOCATOR where  TAG_ID = '{0}' AND TAG_ID_IP='{1}'", TAG_ID,TAG_ID_IP);
                        ExcuteMySql(sSQL,out sMsg);
                        if (sMsg=="")
                        {
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("删除标签:{0} {1}.", TAG_ID, "成功"));
                        }
                        else
                        {
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("标签:{0} {1}.", TAG_ID, sMsg));
                            HandleLabel(0);
                            dataGridViewX1.Rows[i].DefaultCellStyle.BackColor = Color.Red;//标识为红色
                            return;
                        }
                    }
                }
                //插入数据
                sSQL = string.Format(@"insert into T_TAG_LOCATOR(REGION_NO,COM_ID,COM_ID_IP,FINISHER_ID,FINISHER_ID_IP,ORDER_ID,ORDER_ID_IP,AISLE_LAMP_ID,AISLE_LAMP_ID_IP,TAG_ID,TAG_ID_IP,LOCATOR)
                           values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')", REGION_NO, COM_ID, COM_ID_IP, FINISHER_ID, FINISHER_ID_IP, ORDER_ID,
                                            ORDER_ID_IP, AISLE_LAMP_ID, AISLE_LAMP_ID_IP, TAG_ID, TAG_ID_IP, LOCATOR);
                ExcuteMySql(sSQL,out sMsg);
                if (sMsg=="")
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("标签:ID:{0} IP:{1},导入成功", TAG_ID,TAG_ID_IP));
                    dataGridViewX1.Rows[i].DefaultCellStyle.BackColor = Color.Green;//标识为红色
                    HandleLabel(1);
                }
                else
                {
                    sMsg = string.Format("{0}1-{1}", sErrCode, sMsg);
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, sMsg);
                    dataGridViewX1.Rows[i].DefaultCellStyle.BackColor = Color.Red;//标识为红色
                    HandleLabel(0);
                }
            }
            catch (Exception ex)
            {
                string sMsg = string.Format("异常:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, sMsg);
                dataGridViewX1.Rows[i].DefaultCellStyle.BackColor = Color.Red;//标识为红色
            }
        }
        private delegate void delegateHandleLabel(int i);
        private void HandleLabel(int i)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new delegateHandleLabel(HandleLabel), new object[] { i });
            }
            else
            {
                if (i == 1)
                {
                    this.lblOK.Text = Convert.ToString(Convert.ToInt32(lblOK.Text) + 1);
                }
                else
                {
                    this.lblFail.Text = Convert.ToString(Convert.ToInt32(lblFail.Text) + 1);
                }
            }
        }
        private void btnExportTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                string sSql = "select REGION_NO,COM_ID,COM_ID_IP,FINISHER_ID,FINISHER_ID_IP,ORDER_ID,ORDER_ID_IP,AISLE_LAMP_ID,AISLE_LAMP_ID_IP,TAG_ID,TAG_ID_IP,LOCATOR from T_TAG_LOCATOR order by TAG_ID asc";
                DataSet dsQuery = myShd.ExecuteSql(sSql);
               // impExcel.DataTableToExcel(dsQuery.Tables[0]);
                LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, "数据导出成功!");
            }
            catch (Exception ex)
            {
                string sMsg = string.Format("异常:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, sMsg);
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
          //  impExcel.KillExcel();
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[frmCreateDataTable][已经关闭!]");
            this.Close();
            this.Dispose();
        }

        private void DFrmImportLocatorAndTagRelation_Load(object sender, EventArgs e)
        {
          ConnectStr=mycfg["ConnectionStr"];
        }

        private void dataGridViewX1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
