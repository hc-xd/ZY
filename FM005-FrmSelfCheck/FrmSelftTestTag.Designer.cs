namespace NFrmSelfCheck
{
    partial class DFrmSelfCheck
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DFrmSelfCheck));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSelfTest = new DevComponents.DotNetBar.ButtonX();
            this.btnLoadData = new DevComponents.DotNetBar.ButtonX();
            this.btnReturn = new DevComponents.DotNetBar.ButtonX();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.LOCATOR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TAG_ID_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TAG_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.REGION_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COM_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COM_ID_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FINISHER_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FINISHER_ID_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ORDER_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ORDER_ID_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AISLE_LAMP_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AISLE_LAMP_ID_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnSendCommand = new DevComponents.DotNetBar.ButtonX();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.lblFail = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.lblPass = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.lblTotal = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnRegionSelfTest = new DevComponents.DotNetBar.ButtonX();
            this.btnRegionLoadData = new DevComponents.DotNetBar.ButtonX();
            this.comBoxRegion = new System.Windows.Forms.ComboBox();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSelfTest);
            this.groupBox3.Controls.Add(this.btnLoadData);
            this.groupBox3.Controls.Add(this.btnReturn);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 12F);
            this.groupBox3.ForeColor = System.Drawing.Color.Navy;
            this.groupBox3.Location = new System.Drawing.Point(534, 431);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(467, 80);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "全部自检";
            // 
            // btnSelfTest
            // 
            this.btnSelfTest.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelfTest.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelfTest.Font = new System.Drawing.Font("宋体", 10.5F);
            this.btnSelfTest.Location = new System.Drawing.Point(171, 20);
            this.btnSelfTest.Name = "btnSelfTest";
            this.btnSelfTest.Size = new System.Drawing.Size(129, 50);
            this.btnSelfTest.TabIndex = 1;
            this.btnSelfTest.Text = "标签自检";
            this.btnSelfTest.Click += new System.EventHandler(this.btnSelfTest_Click);
            // 
            // btnLoadData
            // 
            this.btnLoadData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoadData.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLoadData.Font = new System.Drawing.Font("宋体", 10.5F);
            this.btnLoadData.Location = new System.Drawing.Point(17, 20);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(129, 50);
            this.btnLoadData.TabIndex = 0;
            this.btnLoadData.Text = "查询对应关系";
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReturn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnReturn.Font = new System.Drawing.Font("宋体", 10.5F);
            this.btnReturn.Location = new System.Drawing.Point(325, 20);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(129, 50);
            this.btnReturn.TabIndex = 0;
            this.btnReturn.Text = "返回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.AllowUserToDeleteRows = false;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LOCATOR,
            this.TAG_ID_IP,
            this.TAG_ID,
            this.REGION_NO,
            this.COM_ID,
            this.COM_ID_IP,
            this.FINISHER_ID,
            this.FINISHER_ID_IP,
            this.ORDER_ID,
            this.ORDER_ID_IP,
            this.AISLE_LAMP_ID,
            this.AISLE_LAMP_ID_IP});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(7, 20);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowTemplate.Height = 23;
            this.dataGridViewX1.Size = new System.Drawing.Size(987, 329);
            this.dataGridViewX1.TabIndex = 5;
            this.dataGridViewX1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewX1_RowPostPaint);
            // 
            // LOCATOR
            // 
            this.LOCATOR.DataPropertyName = "LOCATOR";
            this.LOCATOR.HeaderText = "货位地址";
            this.LOCATOR.Name = "LOCATOR";
            // 
            // TAG_ID_IP
            // 
            this.TAG_ID_IP.DataPropertyName = "TAG_ID_IP";
            this.TAG_ID_IP.HeaderText = "标签对应IP";
            this.TAG_ID_IP.Name = "TAG_ID_IP";
            // 
            // TAG_ID
            // 
            this.TAG_ID.DataPropertyName = "TAG_ID";
            this.TAG_ID.HeaderText = "标签ID";
            this.TAG_ID.Name = "TAG_ID";
            // 
            // REGION_NO
            // 
            this.REGION_NO.DataPropertyName = "REGION_NO";
            this.REGION_NO.HeaderText = "区域编号";
            this.REGION_NO.Name = "REGION_NO";
            // 
            // COM_ID
            // 
            this.COM_ID.DataPropertyName = "COM_ID";
            this.COM_ID.HeaderText = "串口板ID";
            this.COM_ID.Name = "COM_ID";
            // 
            // COM_ID_IP
            // 
            this.COM_ID_IP.DataPropertyName = "COM_ID_IP";
            this.COM_ID_IP.HeaderText = "串口板对应IP";
            this.COM_ID_IP.Name = "COM_ID_IP";
            // 
            // FINISHER_ID
            // 
            this.FINISHER_ID.DataPropertyName = "FINISHER_ID";
            this.FINISHER_ID.HeaderText = "完成器ID";
            this.FINISHER_ID.Name = "FINISHER_ID";
            // 
            // FINISHER_ID_IP
            // 
            this.FINISHER_ID_IP.DataPropertyName = "FINISHER_ID_IP";
            this.FINISHER_ID_IP.HeaderText = "完成器ID";
            this.FINISHER_ID_IP.Name = "FINISHER_ID_IP";
            // 
            // ORDER_ID
            // 
            this.ORDER_ID.DataPropertyName = "ORDER_ID";
            this.ORDER_ID.HeaderText = "订单显示器ID";
            this.ORDER_ID.Name = "ORDER_ID";
            // 
            // ORDER_ID_IP
            // 
            this.ORDER_ID_IP.DataPropertyName = "ORDER_ID_IP";
            this.ORDER_ID_IP.HeaderText = "订单显示器IP";
            this.ORDER_ID_IP.Name = "ORDER_ID_IP";
            // 
            // AISLE_LAMP_ID
            // 
            this.AISLE_LAMP_ID.DataPropertyName = "AISLE_LAMP_ID";
            this.AISLE_LAMP_ID.HeaderText = "巷道灯ID";
            this.AISLE_LAMP_ID.Name = "AISLE_LAMP_ID";
            // 
            // AISLE_LAMP_ID_IP
            // 
            this.AISLE_LAMP_ID_IP.DataPropertyName = "AISLE_LAMP_ID_IP";
            this.AISLE_LAMP_ID_IP.HeaderText = "巷道灯对应IP";
            this.AISLE_LAMP_ID_IP.Name = "AISLE_LAMP_ID_IP";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridViewX1);
            this.groupBox1.Location = new System.Drawing.Point(5, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1000, 357);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.labelX2);
            this.groupBox2.Controls.Add(this.lblFail);
            this.groupBox2.Controls.Add(this.labelX5);
            this.groupBox2.Controls.Add(this.lblPass);
            this.groupBox2.Controls.Add(this.labelX4);
            this.groupBox2.Controls.Add(this.lblTotal);
            this.groupBox2.Controls.Add(this.labelX1);
            this.groupBox2.Location = new System.Drawing.Point(5, 358);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1000, 73);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnSendCommand);
            this.groupBox4.Controls.Add(this.comboBox1);
            this.groupBox4.Location = new System.Drawing.Point(627, 14);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(363, 53);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "组件检测";
            // 
            // btnSendCommand
            // 
            this.btnSendCommand.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSendCommand.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSendCommand.Font = new System.Drawing.Font("宋体", 10.5F);
            this.btnSendCommand.Location = new System.Drawing.Point(221, 16);
            this.btnSendCommand.Name = "btnSendCommand";
            this.btnSendCommand.Size = new System.Drawing.Size(129, 32);
            this.btnSendCommand.TabIndex = 5;
            this.btnSendCommand.Text = "发送指令";
            this.btnSendCommand.Click += new System.EventHandler(this.btnSendCommand_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1.点亮【所有巷道灯】",
            "2.熄灭【所有巷道灯】",
            "3.点亮【订单显示器】",
            "4.熄灭【订单显示器】",
            "5.点亮【完成器】"});
            this.comboBox1.Location = new System.Drawing.Point(19, 20);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(196, 20);
            this.comboBox1.TabIndex = 4;
            // 
            // labelX2
            // 
            this.labelX2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelX2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX2.ForeColor = System.Drawing.Color.Blue;
            this.labelX2.Location = new System.Drawing.Point(492, 29);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(114, 23);
            this.labelX2.TabIndex = 3;
            this.labelX2.Text = "(查看失败标签)";
            this.labelX2.Click += new System.EventHandler(this.labelX2_Click);
            // 
            // lblFail
            // 
            this.lblFail.Font = new System.Drawing.Font("宋体", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFail.ForeColor = System.Drawing.Color.Red;
            this.lblFail.Location = new System.Drawing.Point(413, 26);
            this.lblFail.Name = "lblFail";
            this.lblFail.Size = new System.Drawing.Size(53, 23);
            this.lblFail.TabIndex = 2;
            this.lblFail.Text = "0";
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(321, 26);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(62, 23);
            this.labelX5.TabIndex = 1;
            this.labelX5.Text = "失败数量:";
            // 
            // lblPass
            // 
            this.lblPass.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPass.ForeColor = System.Drawing.Color.Blue;
            this.lblPass.Location = new System.Drawing.Point(248, 23);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(49, 23);
            this.lblPass.TabIndex = 2;
            this.lblPass.Text = "0";
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(159, 26);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(62, 23);
            this.labelX4.TabIndex = 1;
            this.labelX4.Text = "通过数量:";
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTotal.ForeColor = System.Drawing.Color.Blue;
            this.lblTotal.Location = new System.Drawing.Point(85, 23);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(62, 23);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "0";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(7, 26);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(62, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "标签总数:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnRegionSelfTest);
            this.groupBox5.Controls.Add(this.btnRegionLoadData);
            this.groupBox5.Controls.Add(this.comBoxRegion);
            this.groupBox5.Font = new System.Drawing.Font("宋体", 12F);
            this.groupBox5.ForeColor = System.Drawing.Color.Navy;
            this.groupBox5.Location = new System.Drawing.Point(5, 431);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(524, 80);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "区域自检";
            this.groupBox5.Visible = false;
            // 
            // btnRegionSelfTest
            // 
            this.btnRegionSelfTest.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRegionSelfTest.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRegionSelfTest.Enabled = false;
            this.btnRegionSelfTest.Font = new System.Drawing.Font("宋体", 10.5F);
            this.btnRegionSelfTest.Location = new System.Drawing.Point(337, 18);
            this.btnRegionSelfTest.Name = "btnRegionSelfTest";
            this.btnRegionSelfTest.Size = new System.Drawing.Size(129, 50);
            this.btnRegionSelfTest.TabIndex = 3;
            this.btnRegionSelfTest.Text = "标签自检";
            this.btnRegionSelfTest.Click += new System.EventHandler(this.btnRegionSelfTest_Click);
            // 
            // btnRegionLoadData
            // 
            this.btnRegionLoadData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRegionLoadData.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRegionLoadData.Font = new System.Drawing.Font("宋体", 10.5F);
            this.btnRegionLoadData.Location = new System.Drawing.Point(183, 18);
            this.btnRegionLoadData.Name = "btnRegionLoadData";
            this.btnRegionLoadData.Size = new System.Drawing.Size(129, 50);
            this.btnRegionLoadData.TabIndex = 2;
            this.btnRegionLoadData.Text = "查询对应关系";
            this.btnRegionLoadData.Click += new System.EventHandler(this.btnRegionLoadData_Click);
            // 
            // comBoxRegion
            // 
            this.comBoxRegion.FormattingEnabled = true;
            this.comBoxRegion.Location = new System.Drawing.Point(9, 27);
            this.comBoxRegion.Name = "comBoxRegion";
            this.comBoxRegion.Size = new System.Drawing.Size(138, 24);
            this.comBoxRegion.TabIndex = 0;
            this.comBoxRegion.Text = "请选择";
            // 
            // DFrmSelfCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 517);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DFrmSelfCheck";
            this.Text = "[UI]-[标签自检] V1.0";
            this.Load += new System.EventHandler(this.DFrmSelfCheck_Load);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private DevComponents.DotNetBar.ButtonX btnReturn;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.ButtonX btnLoadData;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOCATOR;
        private System.Windows.Forms.DataGridViewTextBoxColumn TAG_ID_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn TAG_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn REGION_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn COM_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn COM_ID_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn FINISHER_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FINISHER_ID_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn ORDER_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ORDER_ID_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn AISLE_LAMP_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn AISLE_LAMP_ID_IP;
        private DevComponents.DotNetBar.ButtonX btnSelfTest;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevComponents.DotNetBar.LabelX lblTotal;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX lblFail;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX lblPass;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.GroupBox groupBox4;
        private DevComponents.DotNetBar.ButtonX btnSendCommand;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox comBoxRegion;
        private DevComponents.DotNetBar.ButtonX btnRegionSelfTest;
        private DevComponents.DotNetBar.ButtonX btnRegionLoadData;
    }
}

