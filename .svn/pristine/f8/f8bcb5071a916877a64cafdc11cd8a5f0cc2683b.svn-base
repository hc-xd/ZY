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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 说明：用户登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocxLoginForm1_UserLoginHandlerEvent(object sender, EventArgs e)
        {
            this.Visible = false;
            FrmMain f = new FrmMain(this.ocxLoginForm1.sUser);
            DialogResult dr = f.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.Visible = true;
                this.Activate();
            }
            else if (dr == DialogResult.Cancel)
            {
                System.Environment.Exit(0);
            }
        }
        private void ocxLoginForm1_UserExitHandlerEvent(object sender, EventArgs e)
        {

        }

        private void ocxLoginForm1_Load(object sender, EventArgs e)
        {

        }
    }
}
