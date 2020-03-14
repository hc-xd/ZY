namespace NFrmSetDAS
{
    partial class DFrmSetDAS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DFrmSetDAS));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOperationModel = new DevComponents.DotNetBar.ButtonX();
            this.rbtOneByMore = new System.Windows.Forms.RadioButton();
            this.rbtOneByOne = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnLightModel = new DevComponents.DotNetBar.ButtonX();
            this.rbtPressByManual = new System.Windows.Forms.RadioButton();
            this.rbtTurnDownBySensor = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.txtBoxWaveCodeHead = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.rbtAllowPending = new System.Windows.Forms.CheckBox();
            this.btnSpecialSetting = new DevComponents.DotNetBar.ButtonX();
            this.tBoxPendingBarcode = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtBox_Path = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnSaveSQL = new DevComponents.DotNetBar.ButtonX();
            this.btnLoad = new DevComponents.DotNetBar.ButtonX();
            this.richTxtBox_SQL = new System.Windows.Forms.RichTextBox();
            this.btnReturn = new DevComponents.DotNetBar.ButtonX();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOperationModel);
            this.groupBox1.Controls.Add(this.rbtOneByMore);
            this.groupBox1.Controls.Add(this.rbtOneByOne);
            this.groupBox1.Location = new System.Drawing.Point(1, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(995, 59);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DAS作业模式选择";
            // 
            // btnOperationModel
            // 
            this.btnOperationModel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperationModel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOperationModel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOperationModel.Location = new System.Drawing.Point(845, 13);
            this.btnOperationModel.Name = "btnOperationModel";
            this.btnOperationModel.Size = new System.Drawing.Size(129, 40);
            this.btnOperationModel.TabIndex = 7;
            this.btnOperationModel.Text = "保存";
            this.btnOperationModel.Click += new System.EventHandler(this.btnOperationModel_Click);
            // 
            // rbtOneByMore
            // 
            this.rbtOneByMore.AutoSize = true;
            this.rbtOneByMore.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtOneByMore.Location = new System.Drawing.Point(259, 25);
            this.rbtOneByMore.Name = "rbtOneByMore";
            this.rbtOneByMore.Size = new System.Drawing.Size(144, 18);
            this.rbtOneByMore.TabIndex = 1;
            this.rbtOneByMore.TabStop = true;
            this.rbtOneByMore.Text = "批量分播模式(B2B)";
            this.rbtOneByMore.UseVisualStyleBackColor = true;
            // 
            // rbtOneByOne
            // 
            this.rbtOneByOne.AutoSize = true;
            this.rbtOneByOne.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtOneByOne.Location = new System.Drawing.Point(22, 25);
            this.rbtOneByOne.Name = "rbtOneByOne";
            this.rbtOneByOne.Size = new System.Drawing.Size(144, 18);
            this.rbtOneByOne.TabIndex = 0;
            this.rbtOneByOne.TabStop = true;
            this.rbtOneByOne.Text = "逐件分播模式(B2C)";
            this.rbtOneByOne.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnLightModel);
            this.groupBox2.Controls.Add(this.rbtPressByManual);
            this.groupBox2.Controls.Add(this.rbtTurnDownBySensor);
            this.groupBox2.Location = new System.Drawing.Point(2, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(994, 60);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "灭灯模式选择";
            // 
            // btnLightModel
            // 
            this.btnLightModel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLightModel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLightModel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLightModel.Location = new System.Drawing.Point(845, 14);
            this.btnLightModel.Name = "btnLightModel";
            this.btnLightModel.Size = new System.Drawing.Size(129, 40);
            this.btnLightModel.TabIndex = 8;
            this.btnLightModel.Text = "保存";
            this.btnLightModel.Click += new System.EventHandler(this.btnLightModel_Click);
            // 
            // rbtPressByManual
            // 
            this.rbtPressByManual.AutoSize = true;
            this.rbtPressByManual.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtPressByManual.Location = new System.Drawing.Point(258, 21);
            this.rbtPressByManual.Name = "rbtPressByManual";
            this.rbtPressByManual.Size = new System.Drawing.Size(81, 18);
            this.rbtPressByManual.TabIndex = 1;
            this.rbtPressByManual.TabStop = true;
            this.rbtPressByManual.Text = "人工拍灭";
            this.rbtPressByManual.UseVisualStyleBackColor = true;
            // 
            // rbtTurnDownBySensor
            // 
            this.rbtTurnDownBySensor.AutoSize = true;
            this.rbtTurnDownBySensor.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtTurnDownBySensor.Location = new System.Drawing.Point(22, 21);
            this.rbtTurnDownBySensor.Name = "rbtTurnDownBySensor";
            this.rbtTurnDownBySensor.Size = new System.Drawing.Size(81, 18);
            this.rbtTurnDownBySensor.TabIndex = 0;
            this.rbtTurnDownBySensor.TabStop = true;
            this.rbtTurnDownBySensor.Text = "感应灭灯";
            this.rbtTurnDownBySensor.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBox1);
            this.groupBox4.Controls.Add(this.txtBoxWaveCodeHead);
            this.groupBox4.Controls.Add(this.labelX2);
            this.groupBox4.Controls.Add(this.rbtAllowPending);
            this.groupBox4.Controls.Add(this.btnSpecialSetting);
            this.groupBox4.Controls.Add(this.tBoxPendingBarcode);
            this.groupBox4.Location = new System.Drawing.Point(2, 129);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(994, 64);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "特殊功能设置";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox1.Location = new System.Drawing.Point(628, 30);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(208, 18);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.Text = "前面是否等待背后标签的拍灭";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // txtBoxWaveCodeHead
            // 
            // 
            // 
            // 
            this.txtBoxWaveCodeHead.Border.Class = "TextBoxBorder";
            this.txtBoxWaveCodeHead.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBoxWaveCodeHead.ForeColor = System.Drawing.Color.Blue;
            this.txtBoxWaveCodeHead.Location = new System.Drawing.Point(491, 27);
            this.txtBoxWaveCodeHead.Name = "txtBoxWaveCodeHead";
            this.txtBoxWaveCodeHead.Size = new System.Drawing.Size(116, 27);
            this.txtBoxWaveCodeHead.TabIndex = 14;
            // 
            // labelX2
            // 
            this.labelX2.Font = new System.Drawing.Font("宋体", 10.5F);
            this.labelX2.Location = new System.Drawing.Point(324, 30);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(173, 23);
            this.labelX2.TabIndex = 13;
            this.labelX2.Text = "波次或箱号条码开头特征";
            // 
            // rbtAllowPending
            // 
            this.rbtAllowPending.AutoSize = true;
            this.rbtAllowPending.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtAllowPending.Location = new System.Drawing.Point(10, 30);
            this.rbtAllowPending.Name = "rbtAllowPending";
            this.rbtAllowPending.Size = new System.Drawing.Size(138, 18);
            this.rbtAllowPending.TabIndex = 12;
            this.rbtAllowPending.Text = "是否允许挂起任务";
            this.rbtAllowPending.UseVisualStyleBackColor = true;
            // 
            // btnSpecialSetting
            // 
            this.btnSpecialSetting.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSpecialSetting.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSpecialSetting.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSpecialSetting.Location = new System.Drawing.Point(845, 18);
            this.btnSpecialSetting.Name = "btnSpecialSetting";
            this.btnSpecialSetting.Size = new System.Drawing.Size(129, 40);
            this.btnSpecialSetting.TabIndex = 9;
            this.btnSpecialSetting.Text = "保存";
            this.btnSpecialSetting.Click += new System.EventHandler(this.btnSpecialSetting_Click);
            // 
            // tBoxPendingBarcode
            // 
            // 
            // 
            // 
            this.tBoxPendingBarcode.Border.Class = "TextBoxBorder";
            this.tBoxPendingBarcode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tBoxPendingBarcode.ForeColor = System.Drawing.Color.Blue;
            this.tBoxPendingBarcode.Location = new System.Drawing.Point(154, 27);
            this.tBoxPendingBarcode.Name = "tBoxPendingBarcode";
            this.tBoxPendingBarcode.Size = new System.Drawing.Size(164, 27);
            this.tBoxPendingBarcode.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelX1);
            this.groupBox3.Controls.Add(this.txtBox_Path);
            this.groupBox3.Controls.Add(this.btnSaveSQL);
            this.groupBox3.Controls.Add(this.btnLoad);
            this.groupBox3.Controls.Add(this.richTxtBox_SQL);
            this.groupBox3.Location = new System.Drawing.Point(2, 193);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(984, 262);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(13, 224);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(65, 23);
            this.labelX1.TabIndex = 17;
            this.labelX1.Text = "文件路径:";
            // 
            // txtBox_Path
            // 
            // 
            // 
            // 
            this.txtBox_Path.Border.Class = "TextBoxBorder";
            this.txtBox_Path.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBox_Path.Location = new System.Drawing.Point(84, 215);
            this.txtBox_Path.Name = "txtBox_Path";
            this.txtBox_Path.Size = new System.Drawing.Size(548, 31);
            this.txtBox_Path.TabIndex = 16;
            // 
            // btnSaveSQL
            // 
            this.btnSaveSQL.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveSQL.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSaveSQL.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveSQL.Location = new System.Drawing.Point(843, 213);
            this.btnSaveSQL.Name = "btnSaveSQL";
            this.btnSaveSQL.Size = new System.Drawing.Size(129, 40);
            this.btnSaveSQL.TabIndex = 15;
            this.btnSaveSQL.Text = "保存";
            this.btnSaveSQL.Click += new System.EventHandler(this.btnSaveSQL_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoad.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLoad.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoad.Location = new System.Drawing.Point(707, 213);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(129, 40);
            this.btnLoad.TabIndex = 14;
            this.btnLoad.Text = "加载SQL语句";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // richTxtBox_SQL
            // 
            this.richTxtBox_SQL.Location = new System.Drawing.Point(10, 15);
            this.richTxtBox_SQL.Name = "richTxtBox_SQL";
            this.richTxtBox_SQL.Size = new System.Drawing.Size(968, 192);
            this.richTxtBox_SQL.TabIndex = 13;
            this.richTxtBox_SQL.Text = "";
            // 
            // btnReturn
            // 
            this.btnReturn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReturn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnReturn.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReturn.Location = new System.Drawing.Point(845, 461);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(129, 40);
            this.btnReturn.TabIndex = 13;
            this.btnReturn.Text = "返回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // DFrmSetDAS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 510);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DFrmSetDAS";
            this.Text = "[UI]-[DAS分播设置参数] V1.0";
            this.Load += new System.EventHandler(this.FrmSetDPS_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.ButtonX btnOperationModel;
        private System.Windows.Forms.RadioButton rbtOneByMore;
        private System.Windows.Forms.RadioButton rbtOneByOne;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevComponents.DotNetBar.ButtonX btnLightModel;
        private System.Windows.Forms.RadioButton rbtPressByManual;
        private System.Windows.Forms.RadioButton rbtTurnDownBySensor;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox rbtAllowPending;
        private DevComponents.DotNetBar.ButtonX btnSpecialSetting;
        private DevComponents.DotNetBar.Controls.TextBoxX tBoxPendingBarcode;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBox_Path;
        private DevComponents.DotNetBar.ButtonX btnSaveSQL;
        private DevComponents.DotNetBar.ButtonX btnLoad;
        private System.Windows.Forms.RichTextBox richTxtBox_SQL;
        private DevComponents.DotNetBar.ButtonX btnReturn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBoxWaveCodeHead;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

