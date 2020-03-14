using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using DevComponents.DotNetBar;
namespace WinUI
{
    partial class FrmMain
    {
        private DevComponents.DotNetBar.DotNetBarManager dotNetBarManager1;
        private DevComponents.DotNetBar.DockSite barLeftDockSite;
        private DevComponents.DotNetBar.DockSite barRightDockSite;
        private DevComponents.DotNetBar.DockSite barTopDockSite;
        private DevComponents.DotNetBar.DockSite barBottomDockSite;
        private System.Windows.Forms.ImageList imageList1;
        private System.ComponentModel.IContainer components;
        private DevComponents.DotNetBar.DockSite dockSite2;
        private DevComponents.DotNetBar.DockSite dockSite3;
        private DevComponents.DotNetBar.DockSite dockSite4;
        private DevComponents.DotNetBar.DockSite dockSite5;
        private DevComponents.DotNetBar.Bar mainmenu;
        private DevComponents.DotNetBar.ButtonItem item_67;
        private DevComponents.DotNetBar.ButtonItem item_78;
        private DevComponents.DotNetBar.ButtonItem item_166;
        private DevComponents.DotNetBar.ButtonItem item_167;
        private DevComponents.DotNetBar.Bar bar4;
        private DevComponents.DotNetBar.ButtonItem btnAddMenu;
        private DevComponents.DotNetBar.ButtonItem item_366;
        private DevComponents.DotNetBar.ButtonItem item_370;
        private DevComponents.DotNetBar.ButtonItem item_405;
        private DevComponents.DotNetBar.ButtonItem item_407;
        private DevComponents.DotNetBar.Bar bar2;
        private DevComponents.DotNetBar.DockContainerItem dTaskList;
        private DevComponents.DotNetBar.DockContainerItem dOutput;
        private DevComponents.DotNetBar.PanelDockContainer panelDockContainer1;
        private DevComponents.DotNetBar.PanelDockContainer panelDockContainer2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ImageList imageList2;
        private DevComponents.DotNetBar.ButtonItem item_61;
        private DevComponents.DotNetBar.ButtonItem item_239;
        private DevComponents.DotNetBar.ButtonItem item_299;
        private Bar bar3;
        private PanelDockContainer panelDockContainer5;
        private DockContainerItem dockContainerItem1;
        private ButtonItem buttonItem1;
        private ButtonItem buttonItem2;
        private ButtonItem buttonItem3;
        private ExplorerBar explorerBar1;
        private RichTextBox richTextBox1;
   

        /// <summary>
        /// Required designer variable.
        /// </summary>
        //private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>

        #endregion


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.dotNetBarManager1 = new DevComponents.DotNetBar.DotNetBarManager(this.components);
            this.barBottomDockSite = new DevComponents.DotNetBar.DockSite();
            this.bar2 = new DevComponents.DotNetBar.Bar();
            this.panelDockContainer2 = new DevComponents.DotNetBar.PanelDockContainer();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panelDockContainer1 = new DevComponents.DotNetBar.PanelDockContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dTaskList = new DevComponents.DotNetBar.DockContainerItem();
            this.dOutput = new DevComponents.DotNetBar.DockContainerItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.barLeftDockSite = new DevComponents.DotNetBar.DockSite();
            this.bar3 = new DevComponents.DotNetBar.Bar();
            this.panelDockContainer5 = new DevComponents.DotNetBar.PanelDockContainer();
            this.explorerBar1 = new DevComponents.DotNetBar.ExplorerBar();
            this.dockContainerItem1 = new DevComponents.DotNetBar.DockContainerItem();
            this.barRightDockSite = new DevComponents.DotNetBar.DockSite();
            this.dockSite5 = new DevComponents.DotNetBar.DockSite();
            this.dockSite2 = new DevComponents.DotNetBar.DockSite();
            this.dockSite3 = new DevComponents.DotNetBar.DockSite();
            this.dockSite4 = new DevComponents.DotNetBar.DockSite();
            this.mainmenu = new DevComponents.DotNetBar.Bar();
            this.item_67 = new DevComponents.DotNetBar.ButtonItem();
            this.item_78 = new DevComponents.DotNetBar.ButtonItem();
            this.item_166 = new DevComponents.DotNetBar.ButtonItem();
            this.item_167 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem3 = new DevComponents.DotNetBar.ButtonItem();
            this.bar4 = new DevComponents.DotNetBar.Bar();
            this.item_366 = new DevComponents.DotNetBar.ButtonItem();
            this.item_405 = new DevComponents.DotNetBar.ButtonItem();
            this.item_407 = new DevComponents.DotNetBar.ButtonItem();
            this.item_370 = new DevComponents.DotNetBar.ButtonItem();
            this.btnAddMenu = new DevComponents.DotNetBar.ButtonItem();
            this.btnItemChangeUser = new DevComponents.DotNetBar.ButtonItem();
            this.barTopDockSite = new DevComponents.DotNetBar.DockSite();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.item_61 = new DevComponents.DotNetBar.ButtonItem();
            this.item_239 = new DevComponents.DotNetBar.ButtonItem();
            this.item_299 = new DevComponents.DotNetBar.ButtonItem();
            this.explorerBarGroupItem1 = new DevComponents.DotNetBar.ExplorerBarGroupItem();
            this.bToday = new DevComponents.DotNetBar.ButtonItem();
            this.barBottomDockSite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bar2)).BeginInit();
            this.bar2.SuspendLayout();
            this.panelDockContainer2.SuspendLayout();
            this.panelDockContainer1.SuspendLayout();
            this.barLeftDockSite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bar3)).BeginInit();
            this.bar3.SuspendLayout();
            this.panelDockContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.explorerBar1)).BeginInit();
            this.dockSite4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainmenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar4)).BeginInit();
            this.SuspendLayout();
            // 
            // dotNetBarManager1
            // 
            this.dotNetBarManager1.BottomDockSite = this.barBottomDockSite;
            this.dotNetBarManager1.DefinitionName = "";
            this.dotNetBarManager1.Images = this.imageList1;
            this.dotNetBarManager1.LeftDockSite = this.barLeftDockSite;
            this.dotNetBarManager1.ParentForm = this;
            this.dotNetBarManager1.RightDockSite = this.barRightDockSite;
            this.dotNetBarManager1.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.dotNetBarManager1.ToolbarBottomDockSite = this.dockSite5;
            this.dotNetBarManager1.ToolbarLeftDockSite = this.dockSite2;
            this.dotNetBarManager1.ToolbarRightDockSite = this.dockSite3;
            this.dotNetBarManager1.ToolbarTopDockSite = this.dockSite4;
            this.dotNetBarManager1.TopDockSite = this.barTopDockSite;
            // 
            // barBottomDockSite
            // 
            this.barBottomDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.barBottomDockSite.Controls.Add(this.bar2);
            this.barBottomDockSite.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barBottomDockSite.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer(new DevComponents.DotNetBar.DocumentBaseContainer[] {
            ((DevComponents.DotNetBar.DocumentBaseContainer)(new DevComponents.DotNetBar.DocumentBarContainer(this.bar2, 926, 144)))}, DevComponents.DotNetBar.eOrientation.Vertical);
            this.barBottomDockSite.Location = new System.Drawing.Point(0, 307);
            this.barBottomDockSite.Name = "barBottomDockSite";
            this.barBottomDockSite.Size = new System.Drawing.Size(926, 147);
            this.barBottomDockSite.TabIndex = 4;
            this.barBottomDockSite.TabStop = false;
            // 
            // bar2
            // 
            this.bar2.AccessibleDescription = "DotNetBar Bar (bar2)";
            this.bar2.AccessibleName = "DotNetBar Bar";
            this.bar2.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.bar2.AutoHideAnimationTime = 0;
            this.bar2.AutoSyncBarCaption = true;
            this.bar2.CanDockTop = false;
            this.bar2.CloseSingleTab = true;
            this.bar2.Controls.Add(this.panelDockContainer2);
            this.bar2.Controls.Add(this.panelDockContainer1);
            this.bar2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bar2.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Caption;
            this.bar2.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.dTaskList,
            this.dOutput});
            this.bar2.LayoutType = DevComponents.DotNetBar.eLayoutType.DockContainer;
            this.bar2.Location = new System.Drawing.Point(0, 3);
            this.bar2.Name = "bar2";
            this.bar2.SelectedDockTab = 1;
            this.bar2.Size = new System.Drawing.Size(926, 144);
            this.bar2.Stretch = true;
            this.bar2.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.bar2.TabIndex = 0;
            this.bar2.TabStop = false;
            this.bar2.Text = "Output";
            // 
            // panelDockContainer2
            // 
            this.panelDockContainer2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.panelDockContainer2.Controls.Add(this.richTextBox1);
            this.panelDockContainer2.Location = new System.Drawing.Point(3, 23);
            this.panelDockContainer2.Name = "panelDockContainer2";
            this.panelDockContainer2.Size = new System.Drawing.Size(920, 93);
            this.panelDockContainer2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelDockContainer2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelDockContainer2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.panelDockContainer2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.panelDockContainer2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelDockContainer2.Style.GradientAngle = 90;
            this.panelDockContainer2.TabIndex = 2;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(920, 93);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // panelDockContainer1
            // 
            this.panelDockContainer1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.panelDockContainer1.Controls.Add(this.listView1);
            this.panelDockContainer1.Location = new System.Drawing.Point(3, 23);
            this.panelDockContainer1.Name = "panelDockContainer1";
            this.panelDockContainer1.Size = new System.Drawing.Size(920, 93);
            this.panelDockContainer1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelDockContainer1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelDockContainer1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.panelDockContainer1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.panelDockContainer1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelDockContainer1.Style.GradientAngle = 90;
            this.panelDockContainer1.TabIndex = 1;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(920, 93);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Function";
            this.columnHeader3.Width = 162;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Description";
            this.columnHeader4.Width = 636;
            // 
            // dTaskList
            // 
            this.dTaskList.Control = this.panelDockContainer1;
            this.dTaskList.GlobalItem = true;
            this.dTaskList.GlobalName = "dTaskList";
            this.dTaskList.Name = "dTaskList";
            this.dTaskList.Text = "Task List";
            // 
            // dOutput
            // 
            this.dOutput.Control = this.panelDockContainer2;
            this.dOutput.GlobalItem = true;
            this.dOutput.GlobalName = "dOutput";
            this.dOutput.Name = "dOutput";
            this.dOutput.Text = "Output";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "");
            this.imageList1.Images.SetKeyName(8, "");
            this.imageList1.Images.SetKeyName(9, "");
            this.imageList1.Images.SetKeyName(10, "");
            this.imageList1.Images.SetKeyName(11, "");
            this.imageList1.Images.SetKeyName(12, "");
            this.imageList1.Images.SetKeyName(13, "");
            this.imageList1.Images.SetKeyName(14, "");
            this.imageList1.Images.SetKeyName(15, "");
            this.imageList1.Images.SetKeyName(16, "");
            this.imageList1.Images.SetKeyName(17, "");
            this.imageList1.Images.SetKeyName(18, "Security.ico");
            // 
            // barLeftDockSite
            // 
            this.barLeftDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.barLeftDockSite.Controls.Add(this.bar3);
            this.barLeftDockSite.Dock = System.Windows.Forms.DockStyle.Left;
            this.barLeftDockSite.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer(new DevComponents.DotNetBar.DocumentBaseContainer[] {
            ((DevComponents.DotNetBar.DocumentBaseContainer)(new DevComponents.DotNetBar.DocumentBarContainer(this.bar3, 213, 255)))}, DevComponents.DotNetBar.eOrientation.Horizontal);
            this.barLeftDockSite.Location = new System.Drawing.Point(0, 52);
            this.barLeftDockSite.Name = "barLeftDockSite";
            this.barLeftDockSite.Size = new System.Drawing.Size(216, 255);
            this.barLeftDockSite.TabIndex = 1;
            this.barLeftDockSite.TabStop = false;
            // 
            // bar3
            // 
            this.bar3.AccessibleDescription = "DotNetBar Bar (bar3)";
            this.bar3.AccessibleName = "DotNetBar Bar";
            this.bar3.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.bar3.AutoSyncBarCaption = true;
            this.bar3.CloseSingleTab = true;
            this.bar3.Controls.Add(this.panelDockContainer5);
            this.bar3.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Caption;
            this.bar3.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.dockContainerItem1});
            this.bar3.LayoutType = DevComponents.DotNetBar.eLayoutType.DockContainer;
            this.bar3.Location = new System.Drawing.Point(0, 0);
            this.bar3.Name = "bar3";
            this.bar3.Size = new System.Drawing.Size(213, 255);
            this.bar3.Stretch = true;
            this.bar3.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.bar3.TabIndex = 0;
            this.bar3.TabStop = false;
            this.bar3.Text = "功能列表";
            // 
            // panelDockContainer5
            // 
            this.panelDockContainer5.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.panelDockContainer5.Controls.Add(this.explorerBar1);
            this.panelDockContainer5.Location = new System.Drawing.Point(3, 23);
            this.panelDockContainer5.Name = "panelDockContainer5";
            this.panelDockContainer5.Size = new System.Drawing.Size(207, 229);
            this.panelDockContainer5.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelDockContainer5.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelDockContainer5.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.panelDockContainer5.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.panelDockContainer5.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelDockContainer5.Style.GradientAngle = 90;
            this.panelDockContainer5.TabIndex = 0;
            // 
            // explorerBar1
            // 
            this.explorerBar1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.explorerBar1.BackColor = System.Drawing.SystemColors.Control;
            // 
            // 
            // 
            this.explorerBar1.BackStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ExplorerBarBackground2;
            this.explorerBar1.BackStyle.BackColorGradientAngle = 90;
            this.explorerBar1.BackStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ExplorerBarBackground;
            this.explorerBar1.Cursor = System.Windows.Forms.Cursors.Default;
            this.explorerBar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.explorerBar1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.explorerBar1.GroupImages = null;
            this.explorerBar1.Images = null;
            this.explorerBar1.Location = new System.Drawing.Point(0, 0);
            this.explorerBar1.Name = "explorerBar1";
            this.explorerBar1.Size = new System.Drawing.Size(202, 229);
            this.explorerBar1.StockStyle = DevComponents.DotNetBar.eExplorerBarStockStyle.SystemColors;
            this.explorerBar1.TabIndex = 1;
            this.explorerBar1.TabStop = false;
            this.explorerBar1.Text = "explorerBar1";
            this.explorerBar1.ThemeAware = true;
            this.explorerBar1.MouseEnter += new System.EventHandler(this.explorerBar1_MouseEnter);
            // 
            // dockContainerItem1
            // 
            this.dockContainerItem1.Control = this.panelDockContainer5;
            this.dockContainerItem1.Name = "dockContainerItem1";
            this.dockContainerItem1.Text = "功能列表";
            // 
            // barRightDockSite
            // 
            this.barRightDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.barRightDockSite.Dock = System.Windows.Forms.DockStyle.Right;
            this.barRightDockSite.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.barRightDockSite.Location = new System.Drawing.Point(926, 52);
            this.barRightDockSite.Name = "barRightDockSite";
            this.barRightDockSite.Size = new System.Drawing.Size(0, 255);
            this.barRightDockSite.TabIndex = 2;
            this.barRightDockSite.TabStop = false;
            // 
            // dockSite5
            // 
            this.dockSite5.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dockSite5.Location = new System.Drawing.Point(0, 454);
            this.dockSite5.Name = "dockSite5";
            this.dockSite5.Size = new System.Drawing.Size(926, 0);
            this.dockSite5.TabIndex = 11;
            this.dockSite5.TabStop = false;
            // 
            // dockSite2
            // 
            this.dockSite2.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite2.Dock = System.Windows.Forms.DockStyle.Left;
            this.dockSite2.Location = new System.Drawing.Point(0, 52);
            this.dockSite2.Name = "dockSite2";
            this.dockSite2.Size = new System.Drawing.Size(0, 402);
            this.dockSite2.TabIndex = 8;
            this.dockSite2.TabStop = false;
            // 
            // dockSite3
            // 
            this.dockSite3.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite3.Dock = System.Windows.Forms.DockStyle.Right;
            this.dockSite3.Location = new System.Drawing.Point(926, 52);
            this.dockSite3.Name = "dockSite3";
            this.dockSite3.Size = new System.Drawing.Size(0, 402);
            this.dockSite3.TabIndex = 9;
            this.dockSite3.TabStop = false;
            // 
            // dockSite4
            // 
            this.dockSite4.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite4.Controls.Add(this.mainmenu);
            this.dockSite4.Controls.Add(this.bar4);
            this.dockSite4.Dock = System.Windows.Forms.DockStyle.Top;
            this.dockSite4.Location = new System.Drawing.Point(0, 0);
            this.dockSite4.Name = "dockSite4";
            this.dockSite4.Size = new System.Drawing.Size(926, 52);
            this.dockSite4.TabIndex = 10;
            this.dockSite4.TabStop = false;
            this.dockSite4.Click += new System.EventHandler(this.dockSite4_Click);
            // 
            // mainmenu
            // 
            this.mainmenu.AccessibleDescription = "DotNetBar Bar (mainmenu)";
            this.mainmenu.AccessibleName = "DotNetBar Bar";
            this.mainmenu.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.mainmenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.mainmenu.DockSide = DevComponents.DotNetBar.eDockSide.Top;
            this.mainmenu.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.mainmenu.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.item_67,
            this.item_167,
            this.buttonItem1,
            this.buttonItem2,
            this.buttonItem3});
            this.mainmenu.Location = new System.Drawing.Point(0, 0);
            this.mainmenu.LockDockPosition = true;
            this.mainmenu.MenuBar = true;
            this.mainmenu.Name = "mainmenu";
            this.mainmenu.Size = new System.Drawing.Size(926, 26);
            this.mainmenu.Stretch = true;
            this.mainmenu.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.mainmenu.TabIndex = 0;
            this.mainmenu.TabStop = false;
            this.mainmenu.Text = "Main Menu";
            // 
            // item_67
            // 
            this.item_67.GlobalName = "item_67";
            this.item_67.ImagePaddingHorizontal = 8;
            this.item_67.Name = "item_67";
            this.item_67.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.item_78,
            this.item_166});
            this.item_67.Text = "文件(&F)";
            // 
            // item_78
            // 
            this.item_78.GlobalName = "item_78";
            this.item_78.ImagePaddingHorizontal = 8;
            this.item_78.Name = "item_78";
            this.item_78.Text = "&Add New Form";
            // 
            // item_166
            // 
            this.item_166.BeginGroup = true;
            this.item_166.GlobalName = "item_166";
            this.item_166.ImagePaddingHorizontal = 8;
            this.item_166.Name = "item_166";
            this.item_166.Text = "&Exit";
            // 
            // item_167
            // 
            this.item_167.GlobalName = "item_167";
            this.item_167.ImagePaddingHorizontal = 8;
            this.item_167.Name = "item_167";
            this.item_167.Text = "编辑(&E)";
            // 
            // buttonItem1
            // 
            this.buttonItem1.ImagePaddingHorizontal = 8;
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.Text = "报表(&V)";
            // 
            // buttonItem2
            // 
            this.buttonItem2.ImagePaddingHorizontal = 8;
            this.buttonItem2.Name = "buttonItem2";
            this.buttonItem2.Text = "工具(&T)";
            // 
            // buttonItem3
            // 
            this.buttonItem3.ImagePaddingHorizontal = 8;
            this.buttonItem3.Name = "buttonItem3";
            this.buttonItem3.Text = "退出(&X)";
            this.buttonItem3.Click += new System.EventHandler(this.buttonItem3_Click);
            // 
            // bar4
            // 
            this.bar4.AccessibleDescription = "DotNetBar Bar (bar4)";
            this.bar4.AccessibleName = "DotNetBar Bar";
            this.bar4.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.bar4.CanHide = true;
            this.bar4.DockLine = 1;
            this.bar4.DockSide = DevComponents.DotNetBar.eDockSide.Top;
            this.bar4.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.bar4.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Office2003;
            this.bar4.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.item_366,
            this.item_405,
            this.item_407,
            this.item_370,
            this.btnAddMenu,
            this.btnItemChangeUser});
            this.bar4.Location = new System.Drawing.Point(0, 27);
            this.bar4.Name = "bar4";
            this.bar4.Size = new System.Drawing.Size(153, 25);
            this.bar4.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.bar4.TabIndex = 1;
            this.bar4.TabStop = false;
            this.bar4.Text = "Commands";
            // 
            // item_366
            // 
            this.item_366.GlobalName = "item_366";
            this.item_366.ImageIndex = 3;
            this.item_366.ImagePaddingHorizontal = 8;
            this.item_366.Name = "item_366";
            this.item_366.Text = "Open File";
            this.item_366.Tooltip = "初始化系统表";
            this.item_366.Click += new System.EventHandler(this.item_366_Click);
            // 
            // item_405
            // 
            this.item_405.GlobalName = "item_405";
            this.item_405.ImageIndex = 1;
            this.item_405.ImagePaddingHorizontal = 8;
            this.item_405.Name = "item_405";
            this.item_405.Text = "Project Properties";
            this.item_405.Tooltip = "配置菜单";
            this.item_405.Click += new System.EventHandler(this.item_405_Click);
            // 
            // item_407
            // 
            this.item_407.GlobalName = "item_407";
            this.item_407.ImageIndex = 16;
            this.item_407.ImagePaddingHorizontal = 8;
            this.item_407.Name = "item_407";
            this.item_407.Text = "Toolbox";
            this.item_407.Tooltip = "设置标签功能";
            this.item_407.Click += new System.EventHandler(this.item_407_Click);
            // 
            // item_370
            // 
            this.item_370.GlobalName = "item_370";
            this.item_370.ImageIndex = 4;
            this.item_370.ImagePaddingHorizontal = 8;
            this.item_370.Name = "item_370";
            this.item_370.Text = "Copy";
            this.item_370.Tooltip = "查询数据";
            this.item_370.Click += new System.EventHandler(this.item_370_Click);
            // 
            // btnAddMenu
            // 
            this.btnAddMenu.GlobalName = "btnAddMenu";
            this.btnAddMenu.ImageIndex = 2;
            this.btnAddMenu.ImagePaddingHorizontal = 8;
            this.btnAddMenu.Name = "btnAddMenu";
            this.btnAddMenu.Text = "Add New Menu";
            this.btnAddMenu.Click += new System.EventHandler(this.btnAddMenu_Click);
            // 
            // btnItemChangeUser
            // 
            this.btnItemChangeUser.ImageIndex = 18;
            this.btnItemChangeUser.ImagePaddingHorizontal = 8;
            this.btnItemChangeUser.Name = "btnItemChangeUser";
            this.btnItemChangeUser.Text = "buttonItem4";
            this.btnItemChangeUser.Click += new System.EventHandler(this.btnItemChangeUser_Click);
            // 
            // barTopDockSite
            // 
            this.barTopDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.barTopDockSite.Dock = System.Windows.Forms.DockStyle.Top;
            this.barTopDockSite.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.barTopDockSite.Location = new System.Drawing.Point(0, 52);
            this.barTopDockSite.Name = "barTopDockSite";
            this.barTopDockSite.Size = new System.Drawing.Size(926, 0);
            this.barTopDockSite.TabIndex = 3;
            this.barTopDockSite.TabStop = false;
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList2.Images.SetKeyName(0, "");
            this.imageList2.Images.SetKeyName(1, "");
            this.imageList2.Images.SetKeyName(2, "");
            this.imageList2.Images.SetKeyName(3, "");
            this.imageList2.Images.SetKeyName(4, "");
            this.imageList2.Images.SetKeyName(5, "");
            this.imageList2.Images.SetKeyName(6, "");
            this.imageList2.Images.SetKeyName(7, "");
            this.imageList2.Images.SetKeyName(8, "");
            // 
            // item_61
            // 
            this.item_61.GlobalName = "item_61";
            this.item_61.ImageIndex = 0;
            this.item_61.ImagePaddingHorizontal = 8;
            this.item_61.Name = "item_61";
            this.item_61.Text = "&Refresh";
            this.item_61.Tooltip = "Refresh";
            // 
            // item_239
            // 
            this.item_239.BeginGroup = true;
            this.item_239.GlobalName = "item_239";
            this.item_239.ImageIndex = 1;
            this.item_239.ImagePaddingHorizontal = 8;
            this.item_239.Name = "item_239";
            this.item_239.Text = "Show All Files";
            this.item_239.Tooltip = "Show All Files";
            // 
            // item_299
            // 
            this.item_299.BeginGroup = true;
            this.item_299.GlobalName = "item_299";
            this.item_299.ImageIndex = 2;
            this.item_299.ImagePaddingHorizontal = 8;
            this.item_299.Name = "item_299";
            this.item_299.Text = "Properties";
            this.item_299.Tooltip = "Properties";
            // 
            // explorerBarGroupItem1
            // 
            // 
            // 
            // 
            this.explorerBarGroupItem1.BackStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(223)))), ((int)(((byte)(247)))));
            this.explorerBarGroupItem1.BackStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.explorerBarGroupItem1.BackStyle.BorderBottomWidth = 1;
            this.explorerBarGroupItem1.BackStyle.BorderColor = System.Drawing.Color.White;
            this.explorerBarGroupItem1.BackStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.explorerBarGroupItem1.BackStyle.BorderLeftWidth = 1;
            this.explorerBarGroupItem1.BackStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.explorerBarGroupItem1.BackStyle.BorderRightWidth = 1;
            this.explorerBarGroupItem1.Cursor = System.Windows.Forms.Cursors.Default;
            this.explorerBarGroupItem1.Expanded = true;
            this.explorerBarGroupItem1.Image = ((System.Drawing.Image)(resources.GetObject("explorerBarGroupItem1.Image")));
            this.explorerBarGroupItem1.Name = "explorerBarGroupItem1";
            this.explorerBarGroupItem1.StockStyle = DevComponents.DotNetBar.eExplorerBarStockStyle.SystemColors;
            this.explorerBarGroupItem1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.bToday});
            this.explorerBarGroupItem1.Text = "Shortcuts";
            // 
            // 
            // 
            this.explorerBarGroupItem1.TitleHotStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(73)))), ((int)(((byte)(181)))));
            this.explorerBarGroupItem1.TitleHotStyle.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(93)))), ((int)(((byte)(206)))));
            this.explorerBarGroupItem1.TitleHotStyle.CornerDiameter = 3;
            this.explorerBarGroupItem1.TitleHotStyle.CornerTypeTopLeft = DevComponents.DotNetBar.eCornerType.Rounded;
            this.explorerBarGroupItem1.TitleHotStyle.CornerTypeTopRight = DevComponents.DotNetBar.eCornerType.Rounded;
            this.explorerBarGroupItem1.TitleHotStyle.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(142)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.explorerBarGroupItem1.TitleStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(73)))), ((int)(((byte)(181)))));
            this.explorerBarGroupItem1.TitleStyle.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(93)))), ((int)(((byte)(206)))));
            this.explorerBarGroupItem1.TitleStyle.CornerDiameter = 3;
            this.explorerBarGroupItem1.TitleStyle.CornerTypeTopLeft = DevComponents.DotNetBar.eCornerType.Rounded;
            this.explorerBarGroupItem1.TitleStyle.CornerTypeTopRight = DevComponents.DotNetBar.eCornerType.Rounded;
            this.explorerBarGroupItem1.TitleStyle.TextColor = System.Drawing.Color.White;
            this.explorerBarGroupItem1.XPSpecialGroup = true;
            // 
            // bToday
            // 
            this.bToday.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bToday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bToday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(93)))), ((int)(((byte)(198)))));
            this.bToday.HotFontUnderline = true;
            this.bToday.HotForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(142)))), ((int)(((byte)(255)))));
            this.bToday.HotTrackingStyle = DevComponents.DotNetBar.eHotTrackingStyle.None;
            this.bToday.Icon = ((System.Drawing.Icon)(resources.GetObject("bToday.Icon")));
            this.bToday.ImageIndex = 0;
            this.bToday.ImagePaddingHorizontal = 8;
            this.bToday.Name = "bToday";
            this.bToday.Text = "Today";
            // 
            // FrmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(926, 454);
            this.Controls.Add(this.barLeftDockSite);
            this.Controls.Add(this.barRightDockSite);
            this.Controls.Add(this.barTopDockSite);
            this.Controls.Add(this.barBottomDockSite);
            this.Controls.Add(this.dockSite2);
            this.Controls.Add(this.dockSite3);
            this.Controls.Add(this.dockSite4);
            this.Controls.Add(this.dockSite5);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "FrmMain";
            this.Text = "iWCS PlatForm 平台";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.barBottomDockSite.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bar2)).EndInit();
            this.bar2.ResumeLayout(false);
            this.panelDockContainer2.ResumeLayout(false);
            this.panelDockContainer1.ResumeLayout(false);
            this.barLeftDockSite.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bar3)).EndInit();
            this.bar3.ResumeLayout(false);
            this.panelDockContainer5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.explorerBar1)).EndInit();
            this.dockSite4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainmenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar4)).EndInit();
            this.ResumeLayout(false);

        }

        private ExplorerBarGroupItem explorerBarGroupItem1;
        private ButtonItem bToday;
        private ButtonItem btnItemChangeUser;
    }
        #endregion
}