using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NFrmSetDAS
{
    public partial class DFrmSetDAS : Form
    {
        private const string sErrCode = "错误代码:FM012";
        //用户名
        string mysUser;
        //访问通用配置文件
        dllConfigApp.ConfigApp mycfg;
        //数据查询对象
        DAL.SQLHelpDataBase myShd;
        //数据库访问对象
        BLL.CreatBaseTable cltBase = new BLL.CreatBaseTable();
        //操作日志的保存
        BLL.CreatLogTable cltlog = new BLL.CreatLogTable();
        dllConfigApp.ConfigApp cfgsql = new dllConfigApp.ConfigApp("SQL_DAS_GetByWaveNo.xml");
        dllConvertBase64.ConvertBase64 base64 = new dllConvertBase64.ConvertBase64();
        public DFrmSetDAS(string sUser, dllConfigApp.ConfigApp cfg, DAL.SQLHelpDataBase shd)
        {
            InitializeComponent();
            mysUser = sUser;
            mycfg = cfg;
            myShd = shd;
        }

        private void btnOperationModel_Click(object sender, EventArgs e)
        {
            //作业模式
            if (rbtOneByOne.Checked)
            {
                mycfg["DASOperateModel"] = "One";
            }
            else if (rbtOneByMore.Checked)
            {
                mycfg["DASOperateModel"] = "More";
            }
        }

        private void btnLightModel_Click(object sender, EventArgs e)
        {
            //亮灯模式
            if (rbtTurnDownBySensor.Checked)
            {
                mycfg["DASLight"] = "Sensor";
            }
            else if (rbtPressByManual.Checked)
            {
                mycfg["DASLight"] = "Manual";
            }
        }

        private void btnSpecialSetting_Click(object sender, EventArgs e)
        {
             //挂起任务
            if (rbtAllowPending.Checked)
            {
                mycfg["DASPending"] = tBoxPendingBarcode.Text.Trim();
            }
            else
            {
                mycfg["DASPending"] = string.Empty;
            }

            mycfg["DASWaveHead"] = txtBoxWaveCodeHead.Text.Trim();

            //是否等待背后标签的拍灭
            if (checkBox1.Checked)
            {
                mycfg["DASWait"] = "Y";
            }
            else
            {
                mycfg["DASWait"] = "N";
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Config|*.xml|所有文件|*.*";
            DialogResult dr = openFileDialog1.ShowDialog();
            openFileDialog1.RestoreDirectory = true;
            txtBox_Path.Text = openFileDialog1.FileName;
            if (!System.IO.File.Exists(openFileDialog1.FileName))
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("文件：{0}不存在.", openFileDialog1.FileName));
            }
            else
            {
                GetSQLFromFile(txtBox_Path.Text);
            }
        }

        private void btnSaveSQL_Click(object sender, EventArgs e)
        {
            if (txtBox_Path.TextLength == 0)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "不是从配置文件中打开的SQL");
                return;
            }
            else if (!System.IO.File.Exists(this.txtBox_Path.Text))
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "配置文件不存在!");
                return;
            }
            dllConfigApp.ConfigApp cfgsql = new dllConfigApp.ConfigApp(txtBox_Path.Text);
            cfgsql["sql"] = base64.StringToBase64string(this.richTxtBox_SQL.Text);
            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, "保存SQL语句成功!");
            richTxtBox_SQL.Clear();
            this.txtBox_Path.Text = string.Empty;
        }
        private void GetSQLFromFile(string sPath)
        {
            try
            {
                dllConfigApp.ConfigApp cfgsql = new dllConfigApp.ConfigApp(sPath);
                this.richTxtBox_SQL.Text = base64.Base64StringTostring(cfgsql["sql"].ToString());
                LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "加载SQL语句成功!");
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("异常:[{0}0][{1}]", sErrCode, ex.Message));
            }
        }
        private void btnReturn_Click(object sender, EventArgs e)
        {
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[FrmSetDAS.dll][已经关闭!]");
            this.Close();
            this.Dispose();
        }

        private void FrmSetDPS_Load(object sender, EventArgs e)
        {
            //加载基本的设置参数
            LoadSetting();
            this.Text = string.Format("{0}[{1}]",this.Text,"V1.0");
        }
        private void LoadSetting()
        {
            //作业模式
            if (mycfg["DASOperateModel"] == "One")
            {
                rbtOneByOne.Checked = true;
            }
            else
            {
                rbtOneByMore.Checked = true;
            }

            //亮灯模式
            if (mycfg["DASLight"] == "Sensor")
            {
                rbtTurnDownBySensor.Checked = true;
            }
            else
            {
                rbtPressByManual.Checked = true;
            }

            //挂起任务
            if (mycfg["DASPending"].Trim().Length == 0)
            {
                rbtAllowPending.Checked = false;
            }
            else
            {
                rbtAllowPending.Checked = true;
                tBoxPendingBarcode.Text = mycfg["DASPending"];
            }
            txtBoxWaveCodeHead.Text = mycfg["DASWaveHead"];//波次条码的特征开头

            if (mycfg["DASWait"] == "Y")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[FrmSetDAS.dll][已经加载!]");
        }
    }
}
