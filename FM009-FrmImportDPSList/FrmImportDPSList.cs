using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace NFrmImportDPSList
{
    public partial class DFrmImportDPSList : Form
    {
        private const string sErrCode = "错误代码:FM009";
        DAL.SQLHelpDataBase myShd;
        dllConfigApp.ConfigApp mycfg;
        string mysUser = string.Empty;
        dllExcelImportAndExport.ExcelImportAndExport impExcel = new dllExcelImportAndExport.ExcelImportAndExport();
        string sMsg = string.Empty;
        string MysqlConnectStr = string.Empty;
        string date = string.Empty;
        public DFrmImportDPSList(string sUser, dllConfigApp.ConfigApp cfg, DAL.SQLHelpDataBase shd)
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
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MysqlConnectStr))
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
        private void btnReturn_Click(object sender, EventArgs e)
        {
            impExcel.KillExcel();
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[frmCreateDataTable][已经关闭!]");
            this.Close();
            this.Dispose();
        }
       
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string sMsg = string.Empty;
            try
            {
                if (comboBox2.SelectedIndex == 2)
                {
                    return;
                }
                string Year = dateTimePicker1.Value.Year.ToString();
                string Month = dateTimePicker1.Value.Month.ToString();
                string Day = dateTimePicker1.Value.Day.ToString();
                string date = Year + "-" + Month.PadLeft(2, '0') + "-" + Day.PadLeft(2, '0');
                this.date = date;
                comboBox1.Items.Clear();
                comboBox3.Items.Clear();
                dataGridViewXDPS.Columns.Clear();
                dataGridViewXDPS.Rows.Clear();
                //if (comboBox2.SelectedIndex == 0)//出库单
                //{
                //    GetOrderByDate(date,0);
                //}
                //else if (comboBox2.SelectedIndex == 1)//入库单
                //{
                //    GetOrderByDate(date, 1);
                //}
                
               
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "dateTimePicker1_ValueChanged 异常：" + ex);
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (comboBox1.Text == "")
                    {
                        return;
                    }
                    else
                    {
                        string str = string.Format("select * from T_PRODUCTION where production='{0}' and date='{1}'", comboBox1.Text, date);
                        DataSet dt = ExcuteMySql(str, out sMsg);
                        if (sMsg != "")
                        {
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "查询明细数据 异常：" + sMsg);
                        }
                        else
                        {
                            if (dt.Tables[0].Rows.Count > 0)
                            {
                                comboBox3.Items.Clear();
                                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                                {
                                    comboBox3.Items.Add(dt.Tables[0].Rows[i]["productionName"].ToString());
                                }
                            }
                        }
                    }

                    if (comboBox2.SelectedIndex == 0)//出库
                    {
                        dataGridViewXDPS.Columns.Clear();
                       // AddColumnsToDataGridView(0);

                    }
                    else
                    {
                        return;
                    }

                }
                catch (Exception ex)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "comboBox1_KeyDown 异常：" + ex);
                }
            }            
        }
        private void CreateColumn(List<string> listColumnName)
        {
            try
            {
                foreach (string s in listColumnName)
                {
                    DataGridViewColumn dataGridViewColumn1 = new DataGridViewTextBoxColumn();
                    dataGridViewColumn1.HeaderText = s;
                    dataGridViewColumn1.ReadOnly = true;
                    dataGridViewXDPS.Columns.Add(dataGridViewColumn1);
                }
                dataGridViewXDPS.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "CreateColumn 异常：" + ex);
            }
        }
        private void AddColumnsToDataGridView(int flag)
        {
            List<string> listColumnName = new List<string>();
            try
            {
                if (flag == 0)//出库
                {
                    listColumnName.Add("物料货位");
                    listColumnName.Add("物料编码");
                    listColumnName.Add("需求数量");
                    listColumnName.Add("出库数量");
                }
                else if (flag == 2)//库存
                {
                    listColumnName.Add("物料名称");
                    listColumnName.Add("物料编码");
                    listColumnName.Add("货位");
                    listColumnName.Add("库存数量");
                }
                CreateColumn(listColumnName);
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "AddColumnsToDataGridView 异常：" + ex.ToString());
            }
        }
        private void InsertIntoDataGridView(DataTable dt,int flag)
        {
            try
            {
                dataGridViewXDPS.Rows.Add(dt.Rows.Count);
                if (flag == 0) //出库
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridViewXDPS.Rows[i].Cells[0].Value = dt.Rows[i]["ItemName"].ToString();
                        dataGridViewXDPS.Rows[i].Cells[1].Value = dt.Rows[i]["ItemCode"].ToString();
                        dataGridViewXDPS.Rows[i].Cells[2].Value = dt.Rows[i]["locator"].ToString();
                        dataGridViewXDPS.Rows[i].Cells[3].Value = dt.Rows[i]["Number"].ToString();
                    }
                }
                else if (flag == 1)//入库
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridViewXDPS.Rows[i].Cells[0].Value = dt.Rows[i]["物料名称"].ToString();
                        dataGridViewXDPS.Rows[i].Cells[1].Value = dt.Rows[i]["物料编码"].ToString();
                        dataGridViewXDPS.Rows[i].Cells[2].Value = dt.Rows[i]["货位"].ToString();
                        dataGridViewXDPS.Rows[i].Cells[3].Value = dt.Rows[i]["库存数量"].ToString();
                    }
                }
                else if (flag == 2)//库存
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridViewXDPS.Rows[i].Cells[0].Value = dt.Rows[i]["ItemName"].ToString();
                        dataGridViewXDPS.Rows[i].Cells[1].Value = dt.Rows[i]["ItemCode"].ToString();
                        dataGridViewXDPS.Rows[i].Cells[2].Value = dt.Rows[i]["Locator"].ToString();
                        dataGridViewXDPS.Rows[i].Cells[3].Value = dt.Rows[i]["Number"].ToString();                        
                    }
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "InsertIntoDataGridView 异常：" + ex.ToString());
            }
        }
        private void GetStockAndShow()
        {
            string sMsg = string.Empty;
            try
            {
                string str = string.Format("select ItemCode,ItemName,Locator,Number from T_STOCK order by locator");
                DataSet dt = ExcuteMySql(str,out sMsg);
                if (sMsg == "")
                {
                    if (dt != null && dt.Tables.Count > 0)
                    {
                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            dataGridViewXDPS.Rows.Clear();
                            dataGridViewXDPS.Columns.Clear();
                            AddColumnsToDataGridView(2);
                           InsertIntoDataGridView(dt.Tables[0],2);
                        }
                    }
                }
                else
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"异常："+sMsg);
                }

            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "GetStockAndShow 异常：" + ex.ToString());
            }
        }
        private void comboBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (comboBox2.SelectedIndex == 1)//库存查询
                    {
                        GetStockAndShow();
                    }
                }
                catch (Exception ex)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "异常：" + ex.ToString());
                }   
            }
          
        }

        private void DFrmImportDPSList_Load(object sender, EventArgs e)
        {
            MysqlConnectStr = mycfg["ConnectionStr"];
            string Year = dateTimePicker1.Value.Year.ToString();
            string Month = dateTimePicker1.Value.Month.ToString();
            string Day = dateTimePicker1.Value.Day.ToString();
            string date = Year + "-" + Month.PadLeft(2, '0') + "-" + Day.PadLeft(2, '0');
            this.date = date;
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            string errorMsg = string.Empty;
            try
            {
                string str = string.Format("select distinct production from T_PRODUCTION WHERE date='{0}'", date);
                DataSet dt = ExcuteMySql(str, out errorMsg);
                if (errorMsg != "")
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "异常：" + errorMsg);
                }
                else
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        comboBox1.Items.Clear();
                        for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                        {
                            comboBox1.Items.Add(dt.Tables[0].Rows[i]["production"].ToString());
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "comboBox1_DropDown 异常：" + ex.ToString());
            }
        }

        private void comboBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string sMsg = string.Empty;
                try
                {
                    dataGridViewXDPS.Columns.Clear();
                    dataGridViewXDPS.Rows.Clear();
                    if (comboBox3.Text == "")
                    {
                        return;
                    }
                    string str = string.Format(@"select * from t_production_detail where productionId=
                                             (select ID from t_production where date='{0}' and production='{1}' and productionName='{2}')", date, comboBox1.Text, comboBox3.Text);
                    DataSet dt = ExcuteMySql(str, out sMsg);
                    if (sMsg == "")
                    {
                        if (dt.Tables[0].Rows.Count > 0)
                        {                           
                            AddColumnsToDataGridView(0);                            
                            dataGridViewXDPS.Rows.Add(dt.Tables[0].Rows.Count);
                            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                            {
                                dataGridViewXDPS.Rows[i].Cells[0].Value = dt.Tables[0].Rows[i]["ItemCode"].ToString();
                                dataGridViewXDPS.Rows[i].Cells[1].Value = dt.Tables[0].Rows[i]["Locator"].ToString();
                                dataGridViewXDPS.Rows[i].Cells[2].Value = dt.Tables[0].Rows[i]["RequireQuantity"].ToString();
                                dataGridViewXDPS.Rows[i].Cells[3].Value = dt.Tables[0].Rows[i]["AcqualQuantity"].ToString();
                            }
                        }
                    }
                    else
                    {
                        LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "查询 异常：" + sMsg);
                    }
                }
                catch (Exception ex)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "comboBox3_KeyDown 异常：" + ex.ToString());
                }
            }            
        }       
    }
}
