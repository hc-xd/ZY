﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using RYB_PTL_API;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace NFrmSelfCheck
{
    public partial class DFrmSelfCheck : Form
    {
        private const string sErrCode = "错误代码:DFrmSelfCheck";
        //访问数据库
        DAL.SQLHelpDataBase myShd;
        //数据查询结果集
        DataSet dsQuery = new DataSet();
        string mysUser;
        dllConfigApp.ConfigApp mycfg;
        string MysqlConnectStr = string.Empty;

        public DFrmSelfCheck(string sUser, dllConfigApp.ConfigApp cfg, DAL.SQLHelpDataBase shd)
        {
            InitializeComponent();
            this.myShd = shd;
            sUser = mysUser;
            mycfg = cfg;
            mysUser = sUser;
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[FrmSelftTest][已成功加载!]");

            //处理标签的返回
            RYB_PTL_API.RYB_PTL.UserResultAvailable += new RYB_PTL_API.RYB_PTL.UserResultAvailableEventHandler(ptl_UserResultAvailable);
            btnSelfTest.Enabled = false;
            btnSendCommand.Enabled = false;
        }
        List<string> listDRs = new List<string>();
        object v = new object();
        private void ptl_UserResultAvailable(RYB_PTL_API.RYB_PTL.RtnValueStruct rs)
        {
            lock (v)
            {
                string sIp = rs.Ip;
                string sTagId = rs.Tagid;
                if (dsQuery.Tables.Count == 0)
                {
                    return;
                }
                else
                {
                    DataRow[] drs = dsQuery.Tables[0].Select(string.Format("TAG_ID_IP = '{0}'and TAG_ID ='{1}'", sIp, sTagId));
                    if (drs.Length > 0)
                    {
                        foreach (DataRow dr in drs)
                        {
                            string sTid = dr["TAG_ID"].ToString();
                          //  string sIP = dr["TAG_ID_IP"].ToString();
                            if (!listDRs.Contains(sTid))
                            {
                                listDRs.Add(sTid);
                                ShowResult();
                            }
                        }
                    }
                   
                }
            }
        }
        private delegate void delegateShowResult();
        private void ShowResult()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new delegateShowResult(ShowResult), new object[] { });
            }
            else
            {
                lock (v)
                {
                    this.lblPass.Text = Convert.ToString(Convert.ToInt32(this.lblPass.Text) + 1);
                    this.lblFail.Text = Convert.ToString(Convert.ToInt32(lblTotal.Text) - Convert.ToInt32(lblPass.Text));
                }
            }
        }
        private void btnReturn_Click(object sender, EventArgs e)
        {
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[FrmSelftTestTag][已经关闭!]");
            //释放绑定
            RYB_PTL_API.RYB_PTL.UserResultAvailable -= new RYB_PTL.UserResultAvailableEventHandler(ptl_UserResultAvailable);
            this.Close();
            this.Dispose();
        }

        private void dataGridViewX1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int rownum = (e.RowIndex + 1);
            System.Drawing.Rectangle rct = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y + 4, ((DataGridView)sender).RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, rownum.ToString(), ((DataGridView)sender).RowHeadersDefaultCellStyle.Font, rct, ((DataGridView)sender).RowHeadersDefaultCellStyle.ForeColor, System.Drawing.Color.Transparent, TextFormatFlags.HorizontalCenter);
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
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            string errorMsg = string.Empty;
            try
            {
                listDRs.Clear();
                btnLoadData.Enabled = false;
                string sSql = "select TAG_ID,TAG_ID_IP,LOCATOR,REGION_NO,COM_ID,COM_ID_IP,FINISHER_ID,FINISHER_ID_IP,ORDER_ID,ORDER_ID_IP,AISLE_LAMP_ID,AISLE_LAMP_ID_IP from T_TAG_LOCATOR WHERE region_no <2 order by TAG_ID asc,LOCATOR asc";
                dsQuery = ExcuteMySql(sSql,out errorMsg);
                if (dsQuery != null && dsQuery.Tables.Count > 0)
                {
                    this.lblFail.Text = "0";
                    this.lblPass.Text = "0";
                    DataView dv = new DataView(dsQuery.Tables[0]);//去除一对多标签的情况
                    DataTable d = dv.ToTable(true, "TAG_ID","TAG_ID_IP"); //根据TAG_ID列进行过滤
                    this.lblTotal.Text = d.Rows.Count.ToString();//过滤之后的数据
                    this.dataGridViewX1.DataSource = dsQuery.Tables[0];
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, "数据查询成功!");
                    for (int i = 0; i < dataGridViewX1.Rows.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            dataGridViewX1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                        }
                    }
                    btnSelfTest.Enabled = true;
                    btnSendCommand.Enabled = true;
                }
                else
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, "数据查询失败!");
                }
            }
            catch (Exception ex)
            {
                string sMsg = string.Format("异常:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, sMsg);
            }
            finally
            {
                btnLoadData.Enabled = true;
               
            }
        }

        private void btnSelfTest_Click(object sender, EventArgs e)
        {
            btnSelfTest.Enabled = false;
            lblPass.Text = "0";
            lblFail.Text = "0";
            listDRs.Clear();
            dllUcShowWnDThread.UcShowWnDThread uc = new dllUcShowWnDThread.UcShowWnDThread("正在自检中，请稍等……", null);
            uc.DoWork = delegate
            {
                DoTest();
            };
            uc.RunAndShow();
            btnSelfTest.Enabled = true;
        }

        /// <summary>
        /// 说明:验证标签
        /// </summary>
        private void DoTest()
        {
            System.Collections.ArrayList alist = new System.Collections.ArrayList();
            for (int i = 0; i < dataGridViewX1.Rows.Count; i++)
            {
                string sTagIp = this.dataGridViewX1.Rows[i].Cells["TAG_ID_IP"].Value.ToString();
                string sTagId = this.dataGridViewX1.Rows[i].Cells["TAG_ID"].Value.ToString();
                if (!alist.Contains(sTagIp + sTagId))
                {
                    RYB_PTL_API.RYB_PTL.RYB_PTL_SelfTest(sTagIp, sTagId);
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("控制器IP:{0},标签ID：{1}发送自检指令成功!", sTagIp, sTagId));
                    alist.Add(sTagIp + sTagId);
                }
            }
        }
        private void labelX2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listDRs.Count > 0)
                {
                    foreach (string s in listDRs)
                    {
                        DataRow[] drones = this.dsQuery.Tables[0].Select(string.Format("TAG_ID= '{0}'", s));
                        foreach (DataRow dre in drones)
                        {
                            this.dsQuery.Tables[0].Rows.Remove(dre);
                        }
                    }
                    this.dataGridViewX1.DataSource = this.dsQuery.Tables[0];
                }
                else
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "没有失败数据.");
                }
            }
            catch(Exception ex)
            {
                string sMsg = string.Format("异常:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, sMsg);
            }
        }

        private void btnSendCommand_Click(object sender, EventArgs e)
        {
            dllUcShowWnDThread.UcShowWnDThread uc = new dllUcShowWnDThread.UcShowWnDThread("正在处理中，请稍后……", null);
            uc.DoWork = delegate
             {
                 HandleCommand();
             };
            uc.RunAndShow();
        }
        private delegate void delegateHandleCommand();
        private void HandleCommand()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new delegateHandleCommand(HandleCommand), new object[] { });
            }
            else
            {
                Application.DoEvents();
                //点亮巷道灯
                try
                {
                    if (dataGridViewX1.Rows.Count == 0)
                    {
                        return;
                    }
                    else
                    {
                        System.Collections.ArrayList alist = new System.Collections.ArrayList();
                        foreach (DataGridViewRow dr in this.dataGridViewX1.Rows)
                        {

                            Application.DoEvents();
                            string sFINISHER_ID = string.Empty;
                            string sFINISHER_ID_IP = string.Empty;
                            string sORDER_ID = string.Empty;
                            string sORDER_ID_IP = string.Empty;
                            string sAISLE_LAMP_ID = string.Empty;
                            string sAISLE_LAMP_ID_IP = string.Empty;

                            //巷道灯IP地址
                            sAISLE_LAMP_ID_IP = dr.Cells["AISLE_LAMP_ID_IP"].Value.ToString();
                            //巷道灯ID地址
                            sAISLE_LAMP_ID = dr.Cells["AISLE_LAMP_ID"].Value.ToString();
                            //订单显示器IP
                            sORDER_ID_IP = dr.Cells["ORDER_ID_IP"].Value.ToString();
                            //订单显示器ID
                            sORDER_ID = dr.Cells["ORDER_ID"].Value.ToString();
                            //完成器IP
                            sFINISHER_ID_IP = dr.Cells["FINISHER_ID_IP"].Value.ToString();
                            //完成器ID
                            sFINISHER_ID = dr.Cells["FINISHER_ID"].Value.ToString();
                            if (comboBox1.SelectedIndex == 0)
                            {
                                if (sAISLE_LAMP_ID_IP.Trim().Length == 0 || sAISLE_LAMP_ID.Trim().Length == 0)
                                {
                                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("控制器IP:{0},巷道灯地址:{1}.不能为空!", sAISLE_LAMP_ID_IP, sAISLE_LAMP_ID));
                                }
                                else
                                {
                                    if (!alist.Contains(sAISLE_LAMP_ID_IP + sAISLE_LAMP_ID+"1"))
                                    {
                                        alist.Add(sAISLE_LAMP_ID_IP + sAISLE_LAMP_ID+"1");
                                        RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAISLE_LAMP_ID_IP, sAISLE_LAMP_ID, 5, 1);
                                        RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAISLE_LAMP_ID_IP, sAISLE_LAMP_ID, 6, 1);
                                        RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAISLE_LAMP_ID_IP, sAISLE_LAMP_ID, 7, 1);
                                        LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("控制器IP:{0},巷道灯地址:{1}.已发送成功!", sAISLE_LAMP_ID_IP, sAISLE_LAMP_ID));
                                    }
                                }
                            }
                            else if (comboBox1.SelectedIndex == 1)
                            {
                                if (sAISLE_LAMP_ID_IP.Trim().Length == 0 || sAISLE_LAMP_ID.Trim().Length == 0)
                                {
                                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("控制器IP:{0},巷道灯地址:{1}.不能为空!", sAISLE_LAMP_ID_IP, sAISLE_LAMP_ID));
                                }
                                else
                                {
                                    if (!alist.Contains(sAISLE_LAMP_ID_IP + sAISLE_LAMP_ID + "0"))
                                    {
                                        alist.Add(sAISLE_LAMP_ID_IP + sAISLE_LAMP_ID + "0");
                                         RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAISLE_LAMP_ID_IP, sAISLE_LAMP_ID, 5, 0);
                                         RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAISLE_LAMP_ID_IP, sAISLE_LAMP_ID, 6, 0);
                                         RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAISLE_LAMP_ID_IP, sAISLE_LAMP_ID, 7, 0);
                                        LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("控制器IP:{0},巷道灯地址:{1}.灭灯成功!", sAISLE_LAMP_ID_IP, sAISLE_LAMP_ID));
                                    }
                                }
                            }
                            else if (comboBox1.SelectedIndex == 2)
                            {
                                if (sORDER_ID_IP.Trim().Length == 0 || sORDER_ID.Trim().Length == 0)
                                {
                                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("控制器IP:{0},订单显示器:{1}.不能为空!", sORDER_ID_IP, sORDER_ID));
                                }
                                else
                                {
                                    if (!alist.Contains(sORDER_ID_IP + sORDER_ID + "1"))
                                    {
                                        alist.Add(sORDER_ID_IP + sORDER_ID + "1");
                                         RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sORDER_ID_IP, sORDER_ID, "12345678", 1);
                                        LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("控制器IP:{0},订单显示器:{1}.已发送成功!", sORDER_ID_IP, sORDER_ID));
                                    }
                                }
                            }
                            else if (comboBox1.SelectedIndex == 3)
                            {
                                if (sORDER_ID_IP.Trim().Length == 0 || sORDER_ID.Trim().Length == 0)
                                {
                                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("控制器IP:{0},订单显示器:{1}.不能为空!", sORDER_ID_IP, sORDER_ID));
                                }
                                else
                                {
                                    if (!alist.Contains(sORDER_ID_IP + sORDER_ID + "0"))
                                    {
                                        alist.Add(sORDER_ID_IP + sORDER_ID + "0");
                                         RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sORDER_ID_IP, sORDER_ID, "12345678", 0);
                                        LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("控制器IP:{0},订单显示器:{1}.熄灭订单显示成功!", sORDER_ID_IP, sORDER_ID));
                                    }
                                }
                            }
                            else if (comboBox1.SelectedIndex == 4)
                            {
                                if (sFINISHER_ID_IP.Trim().Length == 0 || sFINISHER_ID.Trim().Length == 0)
                                {
                                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("控制器IP:{0},完成器:{1}.不能为空!", sFINISHER_ID_IP, sFINISHER_ID));
                                }
                                else
                                {
                                    if (!alist.Contains(sFINISHER_ID_IP + sFINISHER_ID + "1"))
                                    {
                                        alist.Add(sFINISHER_ID_IP + sFINISHER_ID + "1");
                                         RYB_PTL_API.RYB_PTL.RYB_PTL_PlayFinish2(sFINISHER_ID_IP, sFINISHER_ID, 1);
                                        LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("控制器IP:{0},完成器:{1}.已发送成功!", sFINISHER_ID_IP, sFINISHER_ID));
                                    }
                                }
                            }
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string sMsg = string.Format("异常:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, sMsg);
                }
            }
        }

        private void DFrmSelfCheck_Load(object sender, EventArgs e)
        {
        MysqlConnectStr=mycfg["ConnectionStr"];
        }

        private void btnRegionLoadData_Click(object sender, EventArgs e)
        {
            try
            {
                string region = comBoxRegion.Text;
                listDRs.Clear();
                btnLoadData.Enabled = false;
                string sSql = string.Format("select TAG_ID,TAG_ID_IP,LOCATOR,REGION_NO,COM_ID,COM_ID_IP,FINISHER_ID,FINISHER_ID_IP,ORDER_ID,ORDER_ID_IP,AISLE_LAMP_ID,AISLE_LAMP_ID_IP from T_TAG_LOCATOR where region_no = '{0}' order by TAG_ID asc,LOCATOR asc", region);
                dsQuery = myShd.ExecuteSql(sSql);
                if (dsQuery != null && dsQuery.Tables.Count > 0)
                {
                    this.lblFail.Text = "0";
                    this.lblPass.Text = "0";
                    DataView dv = new DataView(dsQuery.Tables[0]);//去除一对多标签的情况
                    DataTable d = dv.ToTable(true, "TAG_ID"); //根据TAG_ID列进行过滤
                    this.lblTotal.Text = d.Rows.Count.ToString();//过滤之后的数据
                    this.dataGridViewX1.DataSource = dsQuery.Tables[0];
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, "数据查询成功!");
                    for (int i = 0; i < dataGridViewX1.Rows.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            dataGridViewX1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                        }
                    }
                    btnRegionSelfTest.Enabled = true;
                }
                else
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, "数据查询失败!");
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "btnRegionLoadData_Click  异常：" + ex.Message);
            }
        }

        private void btnRegionSelfTest_Click(object sender, EventArgs e)
        {
            try
            {
                btnRegionSelfTest.Enabled = false;
                lblPass.Text = "0";
                lblFail.Text = "0";
                listDRs.Clear();
                dllUcShowWnDThread.UcShowWnDThread uc = new dllUcShowWnDThread.UcShowWnDThread("正在自检中，请稍等……", null);
                uc.DoWork = delegate
                {
                    DoTest();
                };
                uc.RunAndShow();
                btnRegionSelfTest.Enabled = true;
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "btnRegionSelfTest_Click 异常：" + ex.Message);
            }
        }
    }
}
