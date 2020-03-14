using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinUI
{
    public partial class RegisterFrm : Form
    {
        string CPUNO { get; set; }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="sCpu"></param>
        public RegisterFrm(string sCpu)
        {
            InitializeComponent();
            CPUNO = sCpu;
            txtBox_Source.Text = sCpu;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            SetRegisterNo();
        }
        private void SetRegisterNo()
        {
            string sIn = Program.des.DESDecrypt(CPUNO);
            //string sIn1 = Program.des.DESEncrypt(System.DateTime.Now.ToUniversalTime() + sIn + "RYB");
            string sInPut = Program.des.DESDecrypt(txtBox_Result.Text.Trim());
            if (sInPut.IndexOf(sIn) > 0)
            {
                MessageBox.Show("注册成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                this.DialogResult = DialogResult.Yes;
                return;
            }
            else
            {
                MessageBox.Show("注册失败,请检查", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return;
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
