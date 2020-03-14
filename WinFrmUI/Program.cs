using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using dllConfigApp;

namespace WinUI
{
    static class Program
    {
        static dllCommon.Common dllcon = new dllCommon.Common();
        public  static  dllEncryptDES.EncryptDES des = new dllEncryptDES.EncryptDES();
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                bool isCanStart = false;

                System.Threading.Mutex mutex = new System.Threading.Mutex(false, "i", out isCanStart);
                if (!isCanStart)
                {
                    MessageBox.Show("程序已运行中。如果需要启动，请先关闭上一次的进程。Ctrl+Alt+Delete 调出任务管理器，在进程中结束。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                string sMsg = string.Empty;
                int i = dllcon.CheckExpireDate(out sMsg);
                if (i == 1)
                {
                    MessageBox.Show(sMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FrmMain("123"));
                }
                else if(i==2)
                {
                    MessageBox.Show(sMsg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    return;
                }
                else if (i == 3)
                {
                    string sCpuSN = des.GetDisk();
                    RegisterFrm f = new RegisterFrm(des.DESEncrypt(sCpuSN));
                    DialogResult dr = f.ShowDialog();
                    if (dr == DialogResult.Yes)
                    {
                        dllcon.configApp["dt3"] = des.DESEncrypt(sCpuSN);
                        Application.EnableVisualStyles();
                        Application.Run(new FrmMain("123"));
                    }
                }
                else if(i ==4)
                {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new FrmMain("123"));
                }
                else if (i == 5)
                {
                    MessageBox.Show(sMsg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    return;
                }
                else
                {
                    MessageBox.Show(sMsg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
