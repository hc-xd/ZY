using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using DevComponents.DotNetBar;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace WinUI
{
	/// <summary>
	/// ����ܴ���
	/// </summary>
	public partial class FrmMain : System.Windows.Forms.Form
	{
        private const string sErrCode = "�������:FM007";
        //�������ݿ�
        DAL.SQLHelpDataBase rybShd = new DAL.SQLHelpDataBase();

        //������д��־���ļ���
        dllEventLogFile.EventLog rybLog = new dllEventLogFile.EventLog(new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory.ToString()), "Frame");
        
        //�˵��嵥����
        DataSet dsMenu = new DataSet();

        //�������
        static  dllCore.Reflections rfs;

        //�쳣���ش�����Ϣ
        string sMsg = string.Empty;

        //ͨ������
        dllConfigApp.ConfigApp cfg = new dllConfigApp.ConfigApp("CommonConfig.xml");

        //�û���
        string sUser = string.Empty;
        string sTagRestartId = string.Empty;
        string MysqlConnectStr = string.Empty;

        ////���嶨ʱ��
        //System.Timers.Timer MoniterctrlTimer = new System.Timers.Timer();
        public FrmMain(string sUser1)
        {
            InitializeComponent();
            //��־ + Log ʱ���
            LogRichBox.LogRichBox.eventDolog += new LogRichBox.LogRichBox.deleDoLog(DisplayRichTextBox);
            //�󶨷�����Ϣ
            RYB_PTL_API.RYB_PTL.UserResultAvailable += new RYB_PTL_API.RYB_PTL.UserResultAvailableEventHandler(myPtl_UserResultAvailable);
            sUser = sUser1;
          
        }

        /// <summary>
        /// ˵��:��ǩ
        /// </summary>
        /// <param name="rs"></param>
        private void myPtl_UserResultAvailable(RYB_PTL_API.RYB_PTL.RtnValueStruct rs)
         {
            string sRtn = string.Format("�����ء�:[����IP{0}][Id��{1}][����ֵ:{2}][��λ:{3}][ֵ:{4}]", rs.Ip, rs.Tagid, rs.KeyCode, rs.Locator, rs.Number);
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, sRtn);
            
            //��������
            if (rs.Tagid == sTagRestartId && rs.Number.ToUpper() == "000F1")
            {
                RestartApplication();
            }
        }
        private void RestartApplication()
        {
            try
            {
                string strAppFileName = Process.GetCurrentProcess().MainModule.FileName;
                Process myNewProcess = new Process();
                myNewProcess.StartInfo.FileName = strAppFileName;
                myNewProcess.StartInfo.WorkingDirectory = Application.ExecutablePath;
                myNewProcess.Start();
                Application.Exit();
            }
            catch(Exception ex)
            {
                DisplayRichTextBox(Color.Blue, FontStyle.Bold, 10, string.Format("ͨ����ǩ��F1�����������쳣.{0}",ex.Message));
            }
        }
		/// <summary>
		/// ˵��:�˳�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void buttonItem3_Click(object sender, EventArgs e)
        {
            //MoniterctrlTimer.Enabled = false;
            DialogResult res = MessageBox.Show("ȷ���˳�ϵͳ��", "ϵͳ�˳�����", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (DialogResult.OK == res)
            {
                rybLog.Stop();
                System.Environment.Exit(0);
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// ˵��:д��־���б���
        /// </summary>
        /// <param name="cl"></param>
        /// <param name="fs"></param>
        /// <param name="size"></param>
        /// <param name="s"></param>
        private delegate void DelegateDisplay(Color cl, FontStyle fs, int size, string s);
        private void DisplayRichTextBox(Color cl, FontStyle fs, int size, string s)
        {
            if (richTextBox1.IsHandleCreated)
            {
                if (richTextBox1.InvokeRequired)
                {
                    this.BeginInvoke(new DelegateDisplay(DisplayRichTextBox), new object[] { cl, fs, size, s });
                }
                else
                {
                    rybLog.WriteToFile(s);
                    if (richTextBox1 != null)
                    {
                        if (richTextBox1.TextLength > 4096)
                        {
                            richTextBox1.Clear();
                        }
                        richTextBox1.SelectionColor = cl;
                        Font font = new Font(FontFamily.GenericMonospace, size, fs);
                        richTextBox1.SelectionFont = font;
                        richTextBox1.AppendText(string.Format("{0}:{1}\r\n", System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), s));
                        richTextBox1.ScrollToCaret();
                    }
                }
            }
        }
       
        /// <summary>
        /// ˵��:�������ʱ���Զ���ϵͳ�л�ȡ�˵���Ŀ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, System.EventArgs e)
        {
            try
            {
                MysqlConnectStr = cfg["ConnectionStr"];
                //System.Diagnostics.Process.Start(@"C:\Users\Administrator\Desktop\notifyicon-master\WindowsFormsApplication1\bin\Debug\WindowsFormsApplication1.exe");
                
                GetFiles();
                //string sSql = "select t.layer1, t.layer2,t.dllname,t.DLLNAMESPACE from T_MENU t order by t.layer1 asc, t.layer2 asc";
                //dsMenu = rybShd.ExecuteSql(sSql, out sMsg);
                rfs = new dllCore.Reflections(sUser,cfg,dsMenu, this.explorerBar1,rybShd);
                if (dsMenu == null || dsMenu.Tables.Count == 0)
                {
                    return;
                }
                else
                {
                    List<dllCore.Reflections.MenuLayer> listmenulayer = new List<dllCore.Reflections.MenuLayer>();
                    ArrayList alistLayer1 = new ArrayList();
                    DataView dv = new DataView();
                    dv.Table = dsMenu.Tables[0];
                    dv.Sort = "layer1,layer2";
                    dsMenu.Tables.Clear();
                    dsMenu.Tables.Add(dv.ToTable());
                    foreach (DataRow dr in dsMenu.Tables[0].Rows)
                    {
                        string sLayer1 = dr["layer1"].ToString();
                        string sLayer2 = dr["layer2"].ToString();
                        string sDllName = dr["dllname"].ToString();
                        if (!alistLayer1.Contains(sLayer1))
                        {
                            dllCore.Reflections.MenuLayer mlr = new dllCore.Reflections.MenuLayer();
                            mlr.sLayer2 = new List<string>();
                            mlr.sLayer1 = sLayer1;//һ���˵�����
                            mlr.sDllName = sDllName;//dll����
                            mlr.sLayer2.Add(sLayer2);
                            listmenulayer.Add(mlr);
                            alistLayer1.Add(sLayer1);
                        }
                        else
                        {
                            //�ҳ������ĸ�����,������Ŀ���ӽ���
                            dllCore.Reflections.MenuLayer mllrr = listmenulayer.FindLast(x => x.sLayer1 == sLayer1);
                            mllrr.sLayer2.Add(sLayer2);
                        }
                    }
                    //�������Ӳ˵�
                    rfs.GenerateMenu(listmenulayer);
                }
                //��ѯ��ǩ���λ�Ķ�Ӧ��ϵ����ȡ���еĿ�������IP��ַ
                LoadControllers();
               // rfs.myform = this;
              //  rfs.LoadMenu("NFrmDPS.DFrmDPS", "DFrmDPS.dll");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
        DataSet dsCtrl = new DataSet();
        List<ctrLayer1> controllers = new List<ctrLayer1>();
        private void LoadControllers()
        {
            try
            {
                string sSql = @"select distinct r.tag_id_ip,
                                        r.tag_id,
                                        r.aisle_lamp_id,
                                        r.aisle_lamp_id_ip,
                                        r.order_id_ip,
                                        r.order_id
                          from t_tag_locator r where r.region_no<2";
                
                dsCtrl = ExcuteMySql(sSql, out sMsg);
                if (dsCtrl == null || dsCtrl.Tables.Count == 0 || dsCtrl.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    DataView dv = new DataView(dsCtrl.Tables[0]);
                    DataTable myCtldb = dv.ToTable(true, "tag_id_ip");
                    foreach (DataRow dr in myCtldb.Rows)
                    {
                        string sTempIp = dr["tag_id_ip"].ToString();
                        Connect(sTempIp);
                    }
                    DisplayRichTextBox(Color.Blue, FontStyle.Bold, 10, "�����ء�[�̳߳�ʼ�����.]");
                }
            }
            catch (Exception ex)
            {
                DisplayRichTextBox(Color.Blue, FontStyle.Bold, 10, string.Format("�����쳣.{0}", ex.Message));
            }
           
        }
        //���ӿ�����
        private void Connect(string sTempIp)
        {
            //ping ������
            if (RYB_PTL_API.RYB_PTL.PingCtrl(sTempIp))
            {
                //ͨ�Ļ������ӿ���
                bool z = RYB_PTL_API.RYB_PTL.RYB_PTL_Connect(sTempIp, 6020);
                if (z)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("��������{0}�����ӳɹ���", sTempIp));
                }
                else
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("��������{0}��pingͨ��������ʧ�ܡ�", sTempIp));
                }
                RYB_PTL_API.RYB_PTL.RYB_PTL_CloseDigit5(sTempIp, "AAAA");//���ǩ
                RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sTempIp, "AAAB", 5, 0);
                RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sTempIp, "AAAB", 6, 0);
                RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sTempIp, "AAAB", 7, 0);

            }
            else//���粻ͨ
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "��������" + sTempIp + "�޷�pingͨ����������!");
                RYB_PTL_API.RYB_PTL.RYB_PTL_Connect(sTempIp, 6020, 1000);
            }
        }
        /// <summary>
        /// ˵�����л��û�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItemChangeUser_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("ȷ���˳�ϵͳ��", "ϵͳ�˳�����", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (DialogResult.OK == res)
            {
                rybLog.Stop();
                System.Environment.Exit(0);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// ˵���������˳�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (rybLog != null)
            {
                rybLog.Stop();
            }
            //MoniterctrlTimer.Enabled = false;//��ʱ�����ر�
            System.Environment.Exit(0);
        }

        /// <summary>
        /// ˵��:���ݴ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void explorerBar1_MouseEnter(object sender, EventArgs e)
        {
            rfs.myform = this;
        }

        private void item_407_Click(object sender, EventArgs e)
        {
            rfs.myform = this;
            rfs.LoadMenu("NFrmAutoCheckTag.DFrmAutoCheckTag", "DFrmAutoCheckTag.dll");
            
        }

        private void item_366_Click(object sender, EventArgs e)
        {
            rfs.myform = this;
            rfs.LoadMenu("NFrmInitialData.DFrmInitialData", "DFrmInitialData.dll");
        }

        private void item_405_Click(object sender, EventArgs e)
        {
            rfs.myform = this;
            rfs.LoadMenu("NFrmSetMenu.DFrmSetMenu", "DFrmSetMenu.dll");
        }

        private void item_370_Click(object sender, EventArgs e)
        {
            //rfs.myform = this;
            //rfs.LoadMenu("NFrmQueryData.DFrmQueryData", "DFrmQueryData.dll");

        }
        /// <summary>
        /// ˵������ȡ����Ŀ¼�µ�dll�ļ������ӵ��˵�����
        /// </summary>
        private void GetFiles()
        {
            try
            {
                //select t.layer1, t.layer2,t.dllname,t.DLLNAMESPACE from T_MENU t order by t.layer1 asc, t.layer2 asc
                DataTable dt = new DataTable("table1");
                dt.Columns.Add("layer1",typeof(string));
                dt.Columns.Add("layer2", typeof(string));
                dt.Columns.Add("dllname", typeof(string));
                dt.Columns.Add("DLLNAMESPACE", typeof(string));
                DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                FileInfo[] fileAll = dir.GetFiles();
                foreach (FileInfo fl in fileAll)
                {
                    if (fl.Extension.Equals(".dll") && fl.Name.StartsWith("DFrm"))
                    {
                        string sClassName = System.IO.Path.GetFileNameWithoutExtension(fl.FullName);
                        string sNameSpace = string.Format("N{0}", sClassName.Substring(1, sClassName.Trim().Length - 1));
                        System.Diagnostics.FileVersionInfo version = System.Diagnostics.FileVersionInfo.GetVersionInfo(fl.FullName);
                        string[] sSplits = version.Comments.Split(new char[] { ';'});
                        if (sSplits.Length != 2)
                        {
                            continue;
                        }
                        else
                        {
                            DataRow dr = dt.NewRow();
                            dr["layer1"] = sSplits[0];
                            dr["layer2"] = sSplits[1];
                            dr["dllname"] = sClassName + ".dll";
                            dr["DLLNAMESPACE"] = sNameSpace + "." + sClassName;
                            dt.Rows.Add(dr);
                            //bool b = CheckExistMenuFromDataBase(sSplits[1]);
                            //if (!b)
                            //{
                            //    //���뵽���ݿ���
                            //    InsertMenuFromDllComments(sSplits[0], sSplits[1], sClassName + ".dll", sNameSpace + "." + sClassName, fl.FullName);
                            //}
                        }
                    }
                }
                dsMenu.Tables.Add(dt);
            }
            catch(Exception ex)
            {
                string sMsg = string.Format("�쳣:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, sMsg);
            }
        }
        /// <summary>
        /// ˵�������dll�Ƿ���������ݿ���
        /// </summary>
        /// <param name="sLayer2"></param>
        /// <returns></returns>
        private bool CheckExistMenuFromDataBase(string sLayer2)
        {
            try
            {
                string sSql = string.Format("select * from t_menu where layer2 = '{0}'", sLayer2);
                DataSet ds = rybShd.ExecuteSql(sSql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string sMsg = string.Format("�쳣:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, sMsg);
                return false;
            }
        }
        /// <summary>
        /// ˵������DLL�ı�ע�л�ȡ��Ϣ���ӵ��˵����С�
        /// </summary>
        /// <param name="sLayer1"></param>
        /// <param name="sLayer2"></param>
        /// <param name="sDllName"></param>
        /// <param name="sDllNameSpace"></param>
        /// <param name="sDllPath"></param>
        private void InsertMenuFromDllComments(string sLayer1,string sLayer2,string sDllName,string sDllNameSpace,string sDllPath)
        {
            try
            {
              string sSql =  string.Format("insert into t_menu (layer1, layer2, dllname, dllnamespace, dllpath) values ('{0}','{1}','{2}','{3}','{4}')", sLayer1, sLayer2, sDllName, sDllNameSpace, sDllPath);
               rybShd.ExecutCmd(sSql);
            }
            catch (Exception ex)
            {
                string sMsg = string.Format("�쳣:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, sMsg);
            }
        }
        private void btnAddMenu_Click(object sender, EventArgs e)
        {

        }

        private void dockSite4_Click(object sender, EventArgs e)
        {

        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show("ȷ���˳�ϵͳ��", "ϵͳ�˳�����", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (DialogResult.Cancel == res)
                {
                    e.Cancel = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("�����˳�ʱ�����쳣��" + ex.Message);
            }
        }
	}
    public class ctrLayer1
    {
        public string Ip { get; set; }
        public int Status { get; set; }
        public List<ctrlLayer2> ctr2s { get; set; }
    }
    public class ctrlLayer2
    {
        public string tagId { get; set; }
    }

}