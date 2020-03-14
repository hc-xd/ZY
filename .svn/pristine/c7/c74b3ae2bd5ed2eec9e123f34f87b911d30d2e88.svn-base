using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NFrmAutoCheckTag
{
    public partial class DFrmAutoCheckTag : Form
    {
        private const string sErrCode = "错误代码:FM001";
        string mysUser;
        dllConfigApp.ConfigApp mycfg;

        public DFrmAutoCheckTag(string sUser, dllConfigApp.ConfigApp cfg, DAL.SQLHelpDataBase shd1)
        {
            InitializeComponent();
            mysUser = sUser;
            mycfg = cfg;
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[frmAutoDspId][已成功加载!]");
        }
        private void btnReturn_Click(object sender, EventArgs e)
        {
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[frmAutoDspId][已经关闭!]");
            this.Close();
            this.Dispose();
        }
        private void btnDspId_Click(object sender, EventArgs e)
        {
            findController1.DoStart();
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[btnDspId_Click][正在检索中.....]");
            btnDspId.Enabled = false;
            btnStop.Enabled = true;

        }
        private delegate void DelegateNotifyIPConnected(string s);

        private void findController1_NotifyIPConnected_1(string sIp)
        {
            
            if (InvokeRequired)
            {
                this.BeginInvoke(new DelegateNotifyIPConnected(findController1_NotifyIPConnected_1), new object[] { sIp });
            }
            else
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("已成功检索到IP:{0} 的控制器!", sIp));
                BtnSendComand.Enabled = true;
            }
        }
        private void findController1_NotifyIpDisconnect_1(string sIp)
        {
           
            if (InvokeRequired)
            {
                this.BeginInvoke(new DelegateNotifyIPConnected(findController1_NotifyIpDisconnect_1), new object[] { sIp });
            }
            else
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("IP:{0} 的控制器,已断开!", sIp));
                BtnSendComand.Enabled = false;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            findController1.DoStop();
            btnDspId.Enabled = true;
            btnStop.Enabled = false;
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[btnDspId_Click][已经结束检索.]");
        }

        private void btnDspTagId_Click(object sender, EventArgs e)
        {
            DoDspTag(9);
        }
        private void DoDspTag(int iT)
        {
            int i = this.findController1.listViewIp.Items.Count;
            if (i == 0)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[btnDspTagId_Click][没有搜索到任何控制器.]");
                return;
            }
            else
            {
                bool bSelect = false;
                foreach (ListViewItem lvi in this.findController1.listViewIp.Items)
                {
                    if (lvi.Checked)
                    {
                        RYB_PTL_API.RYB_PTL.RYB_PTL_SetMode(lvi.SubItems[1].Text, "AAAA", iT);
                        LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, string.Format("IP:{0} 指令特征值:{1},已经执行完成.", lvi.SubItems[1].Text, iT));
                        bSelect = true;
                    }
                }
                if (!bSelect)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "没有选中控制器.");
                }
            }
        }

        /// <summary>
        /// 说明：熄灭所有的标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseTag_Click(object sender, EventArgs e)
        {
            int i = this.findController1.listViewIp.Items.Count;
            if (i == 0)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[btnDspTagId_Click][没有搜索到任何控制器.]");
                return;
            }
            else
            {
                bool bSelect = false;
                foreach (ListViewItem lvi in this.findController1.listViewIp.Items)
                {
                    if (lvi.Checked)
                    {
                        RYB_PTL_API.RYB_PTL.RYB_PTL_CloseDigit5(lvi.SubItems[1].Text, "AAAA");
                        RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(lvi.SubItems[1].Text, "AAAB", 5, 0);
                        RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(lvi.SubItems[1].Text, "AAAB", 7, 0);
                        RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(lvi.SubItems[1].Text, "AAAB", 8, 0);
                        LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "灭灯指令已完成.");
                        bSelect = true;
                    }
                }
                if (!bSelect)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "没有选中控制器.");
                }
            }
        }
        private void BtnSendComand_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >=0)
            {
                DoDspTag(comboBox1.SelectedIndex + 1);
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            try
            {
                int i = this.findController1.listViewIp.Items.Count;
                if (i == 0)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[btnDspTagId_Click][没有搜索到任何控制器.]");
                    return;
                }
                else
                {
                    bool bSelect = false;
                    foreach (ListViewItem lvi in this.findController1.listViewIp.Items)
                    {
                        if (lvi.Checked)
                        {
                            //RYB_PTL_API.RYB_PTL.RYB_PTL_SetMode(lvi.SubItems[1].Text, "AAAA", iT);
                            string sIp = lvi.SubItems[1].Text;
                            RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(sIp, "AAAA", 88888, 1, 2);
                            RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sIp, "AAAB", 5, 1);
                            RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sIp, "AAAB", 6, 1);
                            RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sIp, "AAAB", 7, 1);
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, string.Format("IP:{0} 点亮所有标签,已经执行完成.", sIp));
                            bSelect = true;
                        }
                    }
                    if (!bSelect)
                    {
                        LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "没有选中控制器.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "点亮所有标签发生异常：" + ex.Message);
            }
        }
    }
}

