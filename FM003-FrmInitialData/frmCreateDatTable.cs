using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Microsoft.Office.Interop.Excel;

namespace NFrmInitialData
{
    public partial class DFrmInitialData : Form
    {
        private const string sErrCode = "错误代码:DFrmInitialData";
        DAL.SQLHelpDataBase myShd;
        BLL.CreatBaseTable cltBase = new BLL.CreatBaseTable();
        BLL.CreateDPSTable cltDPS = new BLL.CreateDPSTable();
        BLL.CreateDASTable cltDAS = new BLL.CreateDASTable();
        BLL.CreatLogTable cltlog = new BLL.CreatLogTable();
        BLL.CreateItem cltItem = new BLL.CreateItem();

        string mysUser;
        dllConfigApp.ConfigApp mycfg;
        int iType = 0;
        string sMsg = string.Empty;
        DataSet ds = new DataSet();
        dllExcelImportAndExport.ExcelImportAndExport impExcel = new dllExcelImportAndExport.ExcelImportAndExport();
        public DFrmInitialData(string sUser, dllConfigApp.ConfigApp cfg, DAL.SQLHelpDataBase shd)
        {
            InitializeComponent();
            myShd = shd;
            mysUser = sUser;
            mycfg = cfg;
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[frmCreateDataTable.dll][已经成功加载!]");
            //获取数据库的类型
            CheckDataBaseType(myShd.strProviderName);

            //增加表名到数据库中
            LoadTableName();

            //先创建表
            btnCreate.Enabled = true;
        }


        /// <summary>
        /// 说明：判断数据库加载的类型
        /// </summary>
        /// <param name="sp"></param>
        private void CheckDataBaseType(string sp)
        {
            if (sp.ToUpper() == "SYSTEM.DATA.ORACLECLIENT")
            {
                this.lblDataBaseName.Text = "Oracle";
                iType = 0;
            }
            else if (sp.ToUpper() == "SYSTEM.DATA.OLEDB")
            {
                this.lblDataBaseName.Text = "Access";
                iType = 2;
            }
            else if (sp.ToUpper() == "SYSTEM.DATA.SQLCLIENT")
            {
                this.lblDataBaseName.Text = "SQL Server";
                iType = 1;
            }
        }
        private void LoadTableName()
        {
            int i = 0;
            foreach( DictionaryEntry  de in cltBase.ht)
            {
                i++;
                ListViewItem lvi = new ListViewItem(new string[] { i.ToString(),de.Key.ToString(),de.Value.ToString()});
                listViewEx1.Items.Add(lvi);
            }
        }
        private void btnReturn_Click(object sender, EventArgs e)
        {
            impExcel.KillExcel();
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[frmCreateDataTable][已经关闭!]");
            this.Close();
            this.Dispose();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            dllUcShowWnDThread.UcShowWnDThread uc = new dllUcShowWnDThread.UcShowWnDThread("正在创建表，请稍后……", null);
            uc.DoWork = delegate
            {
                switch (iType)
                {
                        //oracle
                    case 0:
                        {
                            cltBase.CreatBaseTableToOracle(out sMsg); //创建货位与标签对应关系表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            //cltBase.CreatMenuToOracle(out sMsg); //创建菜单表
                            //LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltlog.CreatLogTableToOracle(out sMsg); //创建日志表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDPS.CreatPickOrderHeadToOracle(out sMsg);//拣料任务头表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDPS.CreatPickOrderLineToOracle(out sMsg);//拣料任务行表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);
                            //
                            cltDAS.CreatDASHeadToOracle(out sMsg);//播种任务头表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDAS.CreatDASLineToOracle(out sMsg);//播种任务行表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDAS.CreatDASDetailToOracle(out sMsg);//播种任务明细表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltItem.CreatItemTableToOracle(out sMsg);//创建物料编码表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            break;
                        }
                        //sqlServer
                    case 1:
                        {
                            cltBase.CreatBaseTableToSQLServer(out sMsg); ///创建货位与标签对应关系表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            //cltBase.CreatMenuToSQLServer(out sMsg); //创建菜单表
                            //LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltlog.CreatLogTableToSQLServer(out sMsg); //创建日志表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDPS.CreatPickOrderHeadToSql(out sMsg);//拣料任务头表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDPS.CreatPickOrderLineToSql(out sMsg);//拣料任务行表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);
                            //
                            cltDAS.CreatDASHeadToSql(out sMsg);//播种任务头表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDAS.CreatDASLineToSql(out sMsg);//播种任务行表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDAS.CreatDASDetailToSql(out sMsg);//播种任务明细表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltItem.CreatItemTableToSQLServer(out sMsg);//创建物料编码表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);
                            break;
                        }
                        //Access
                    case 2:
                        {
                            cltBase.CreatBaseTableToAccess(out sMsg);///创建货位与标签对应关系表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            //cltBase.CreatMenuToAccess(out sMsg);//创建菜单表
                            //LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltlog.CreatLogTableToAccess(out sMsg);//创建日志表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDPS.CreatPickOrderHeadToAccess(out sMsg);//拣料任务头表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDPS.CreatPickOrderLineToAccess(out sMsg);//拣料任务行表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDAS.CreatDASHeadToAccess(out sMsg);//播种任务头表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDAS.CreatDASLineToAccess(out sMsg);//播种任务行表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDAS.CreatDASDetailToAccess(out sMsg);//播种任务明细表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltItem.CreatItemTableToAccess(out sMsg);//创建物料编码表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            break;
                        }
                    default:
                        {
                            cltBase.CreatBaseTableToSQLServer(out sMsg);
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            //cltBase.CreatMenuToSQLServer(out sMsg);
                            //LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltlog.CreatLogTableToSQLServer(out sMsg);
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDPS.CreatPickOrderHeadToSql(out sMsg);//拣料任务头表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDPS.CreatPickOrderLineToSql(out sMsg);//拣料任务行表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDAS.CreatDASHeadToSql(out sMsg);//播种任务头表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDAS.CreatDASLineToSql(out sMsg);//播种任务行表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltDAS.CreatDASDetailToSql(out sMsg);//播种任务明细表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            cltItem.CreatItemTableToSQLServer(out sMsg);//创建物料编码表
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sMsg);

                            break;
                        }

                }
            };
            uc.RunAndShow();
        }

         //<summary>
         //说明：把数据导入到数据库中
         //</summary>
         //<param name="sPathFile"></param>
        private void ImportExcelTagLocatorToDataBase(DevComponents.DotNetBar.Controls.DataGridViewX dvx1,string sPathFile)
        {
          
            ds =  impExcel.ExcelToDS(sPathFile, out sMsg);
            if (sMsg.Trim().Length != 0)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, sMsg);
            }
            else
            {
                lblTotal.Text = ds.Tables[0].Rows.Count.ToString();//合计的总数
                if (ds.Tables.Count > 0)
                {
                    dvx1.DataSource = ds.Tables[0];
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, "查询成功。");
                    for (int i = 0; i < dvx1.Rows.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            dvx1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                        }
                    }
                }
                else
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "Excel中没有任何数据.");
                }
            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                btnCreate.Enabled = true;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                btnCreate.Enabled = false;
            }
        }

        private void dataGridViewX1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int rownum = (e.RowIndex + 1);
            System.Drawing.Rectangle rct = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y + 4, ((DataGridView)sender).RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, rownum.ToString(), ((DataGridView)sender).RowHeadersDefaultCellStyle.Font, rct, ((DataGridView)sender).RowHeadersDefaultCellStyle.ForeColor, System.Drawing.Color.Transparent, TextFormatFlags.HorizontalCenter);
        }

        private void btnExportTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                string sSql = "select REGION_NO,COM_ID,COM_ID_IP,FINISHER_ID,FINISHER_ID_IP,ORDER_ID,ORDER_ID_IP,AISLE_LAMP_ID,AISLE_LAMP_ID_IP,TAG_ID,TAG_ID_IP,LOCATOR from T_TAG_LOCATOR order by TAG_ID asc";
                DataSet dsQuery = myShd.ExecuteSql(sSql);
                impExcel.DataTableToExcel(dsQuery.Tables[0]);
                LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, "数据导出成功!");
            }
            catch(Exception ex)
            {
                string sMsg = string.Format("{0}3-{1}", sErrCode, ex.Message);
                LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10,sMsg);
            }
        }

        private void btnCreateBaseMenu_Click(object sender, EventArgs e)
        {
//            //创建菜单数据到数据库中
//            myShd.ExecutCmd("delete from T_MENU", out sMsg);
//            myShd.ExecutCmd(@"insert into T_MENU (layer1, layer2, dllname, dllnamespace, dllpath, enable)
//                                values ('A-基础数据配置', '2.初始化数据库', 'FrmInitialData.dll', 'FrmInitialData.frmCreateDataTable', 'E:\2016-Code\PTLControlLib\WinFrmUI\bin\Debug', 1)", out sMsg);
//            myShd.ExecutCmd(@" insert into T_MENU (layer1, layer2, dllname, dllnamespace, dllpath, enable, create_by, create_date, last_update_by, last_update_date)
//                                values ('A-基础数据配置', '1.设置菜单', 'FrmSetMenu.dll', 'FrmSetMenu.frmSetMenu', 'E:\2016-Code\PTLControlLib\WinFrmUI\bin\Debug', 1, null, null, null, null)", out sMsg);
//            myShd.ExecutCmd(@" insert into T_MENU (layer1, layer2, dllname, dllnamespace, dllpath, enable, create_by, create_date, last_update_by, last_update_date)
//                                values ('B-工具:设置标签及组件', '1.设置标签功能', 'frmAutoCheckTag.dll', 'frmAutoCheckTag.frmAutoDspId', 'E:\2016-Code\PTLControlLib\WinFrmUI\bin\Debug', 1, null, null, null, null)", out sMsg);
//            myShd.ExecutCmd(@"  insert into T_MENU (layer1, layer2, dllname, dllnamespace, dllpath, enable, create_by, create_date, last_update_by, last_update_date)
                                //values ('B-工具:设置标签及组件', '2.标签组件自检', 'FrmSelfTest.dll', 'FrmSelfTest.FrmSelftTest', 'E:\2016-Code\PTLControlLib\WinFrmUI\bin\Debug', 1, null, null, null, null)", out sMsg);
        }

        private void dataGridViewXDPS_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int rownum = (e.RowIndex + 1);
            System.Drawing.Rectangle rct = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y + 4, ((DataGridView)sender).RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, rownum.ToString(), ((DataGridView)sender).RowHeadersDefaultCellStyle.Font, rct, ((DataGridView)sender).RowHeadersDefaultCellStyle.ForeColor, System.Drawing.Color.Transparent, TextFormatFlags.HorizontalCenter);
        }

    }
}

