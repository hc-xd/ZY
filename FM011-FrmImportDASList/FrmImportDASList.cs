using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;

namespace NFrmImportDASList
{
    public partial class DFrmImportDASList : Form
    {
        private const string sErrCode = "错误代码:FM011";
        DAL.SQLHelpDataBase myShd;
        dllConfigApp.ConfigApp mycfg;
        string mysUser = string.Empty;
        dllExcelImportAndExport.ExcelImportAndExport impExcel = new dllExcelImportAndExport.ExcelImportAndExport();
        string sMsg = string.Empty;
        public DFrmImportDASList(string sUser, dllConfigApp.ConfigApp cfg, DAL.SQLHelpDataBase shd)
        {
            InitializeComponent();
            mysUser = sUser;
            mycfg = cfg;
            myShd = shd;
        }

        private void btnSelectFileDPS_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Excel|*.xlsx|Excel|*.xls|所有文件|*.*";
            DialogResult dr = openFileDialog1.ShowDialog();
            openFileDialog1.RestoreDirectory = true;
            txtBoxFileDPS.Text = openFileDialog1.FileName;
            if (!System.IO.File.Exists(openFileDialog1.FileName))
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "选定的文件不存在.");
            }
            else
            {
                ImportExcelDASToDataBase(this.dataGridViewXDPS, openFileDialog1.FileName);
            }
        }
        private void ImportExcelDASToDataBase(DevComponents.DotNetBar.Controls.DataGridViewX dvx1, string sPathFile)
        {

           DataSet  ds = impExcel.ExcelToDS(sPathFile, out sMsg);
            if (sMsg.Trim().Length != 0)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, sMsg);
            }
            else
            {
                lblTotal.Text = ds.Tables[0].Rows.Count.ToString();//合计的总数
                if (ds.Tables.Count > 0)
                {
                    dvx1.DataSource = ds.Tables[0];
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, "查询成功。");
                    for (int i = 0; i < dvx1.Rows.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            dvx1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                        }
                    }
                }
                else
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "Excel中没有任何数据.");
                }
            }
        }

        private void btnImportDPS_Click(object sender, EventArgs e)
        {
            lblOK.Text = "0";
            lblFail.Text = "0";
            if (dataGridViewXDPS.Rows.Count == 0)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "没有任何数据可以导入.");
            }
            else
            {

                dllUcShowWnDThread.UcShowWnDThread uc = new dllUcShowWnDThread.UcShowWnDThread("正在处理，请稍后……", null);
                uc.DoWork = delegate
                {
                    System.Data.DataTable dt = this.dataGridViewXDPS.DataSource as System.Data.DataTable;
                    DataView dv = new DataView(dt);
                    System.Data.DataTable dtFilter = dv.ToTable(true, new string[] { "WAVE_NO", "REGION_NO"});//过滤掉重复的记录行
                    ArrayList alistOrder = new ArrayList();//过滤重复的订单号
                    foreach (DataRow dr in dtFilter.Rows)
                    {
                        string sWAVE_NO = dr["WAVE_NO"].ToString();
                        string sREGION_NO = dr["REGION_NO"].ToString();
                        //插入数据到头表中
                        if (InsertDASHead(sWAVE_NO,sREGION_NO, out sMsg))
                        {
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, string.Format("插入播种任务头表:波次号:{0},区域号:{1},返回信息：{2}成功", sWAVE_NO, sREGION_NO,sMsg));
                            //获取插入后的头表的Id号
                            int iId = GetHeadId(sWAVE_NO,sREGION_NO);
                            //插入行表数据
                            DataRow[] drs = dt.Select(string.Format("WAVE_NO= '{0}' and REGION_NO = '{1}'", sWAVE_NO, sREGION_NO));
                       
                            foreach (DataRow dr1 in drs)
                            {
                                string sBox_No = dr1["BOX_NO"].ToString();
                                string sOrder_no = dr1["order_no"].ToString();
                                string sLocator = dr1["locator"].ToString();
                                string sback_locator = dr1["back_locator"].ToString();
                                if (!alistOrder.Contains(sOrder_no + iId.ToString() + sBox_No))
                                {
                                    alistOrder.Add(sOrder_no + iId.ToString() + sBox_No);
                                    bool bRtn = InsertDASLine(iId, sOrder_no, sLocator, sback_locator,sBox_No, out  sMsg);
                                    if (bRtn)
                                    {
                                        LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, string.Format("插入播种任务行表:头id：{0},订单号:{1},货位:{2},对应背后货位:{3},返回信息：{4} 成功", iId, sOrder_no, sLocator, sback_locator, sMsg));
                                        //再插入明细表
                                        int iIdLine = GetLineId(sOrder_no, sLocator, iId, sBox_No);
                                        DataRow[] drsDetail = dt.Select(string.Format("WAVE_NO= '{0}' and  REGION_NO = '{1}' and  BOX_NO = '{2}' and order_no = '{3}' and locator = '{4}'", sWAVE_NO, sREGION_NO, sBox_No, sOrder_no, sLocator));
                                        foreach (DataRow dr2 in drsDetail)
                                        {
                                            string sItemCode = dr2["item_code"].ToString();
                                            string sBarcode = dr2["item_barcode"].ToString();
                                            int iRequestQty = Convert.ToInt32(dr2["require_qty"]);
                                            int iActualQty = Convert.ToInt32(dr2["actual_qty"]);
                                            string sItemUnit = dr2["item_unit"].ToString();
                                            bool bRtnDetail = InsertDASDetail(iIdLine, sItemCode, sBarcode, iRequestQty, iActualQty, sItemUnit, out  sMsg);
                                            if (bRtnDetail)
                                            {
                                                HandleLabel(1);
                                            }
                                            else
                                            {
                                                HandleLabel(0);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("插入播种任务行表:头id：{0},订单号:{1},货位:{2},对应背后货位:{3},返回信息：{4} 失败", iId, sOrder_no, sLocator, sback_locator, sMsg));

                                    }
                                }
                            }
                        }
                        else
                        {
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("插入播种任务头表:波次号:{0},区域号:{1},返回信息：{2}失败", sWAVE_NO,  sREGION_NO, sMsg));
                            continue;
                        }
                    }
                };
                uc.RunAndShow();
            }
            //
            this.dataGridViewXDPS.DataSource = null;
        }

        /// <summary>
        /// 说明：插入记录到行表中
        /// </summary>
        /// <param name="ihead_id">传入的头表ID号</param>
        /// <param name="sOrder_no">订单号</param>
        /// <param name="sLocator">货位号</param>
        /// <param name="sback_locator">此货位对应的背后的货位</param>
        /// <param name="sg">返回的信息</param>
        /// <returns></returns>
        private bool InsertDASLine(int ihead_id, string sOrder_no, string sLocator, string sback_locator,string sBoxNo,out string sg)
        {

            string sSql = string.Format(@"insert into t_das_line (head_id, order_no, locator, flag_line, back_locator,BOX_NO) values ({0}, '{1}', '{2}', 0, '{3}','{4}')", ihead_id, sOrder_no, sLocator, sback_locator, sBoxNo);
            return myShd.ExecutCmd(sSql, out sg);
        }

        /// <summary>
        /// 说明：把数据插入到明细表中
        /// </summary>
        /// <param name="iLine">行表的ID</param>
        /// <param name="sitem_code">物料编码</param>
        /// <param name="sitem_barcode">物料条码</param>
        /// <param name="irequire_qty">需求数量</param>
        /// <param name="actual_qty">实际数量</param>
        /// <param name="sitem_unit">物料单位</param>
        /// <param name="sg"></param>
        /// <returns></returns>
        private bool InsertDASDetail(int iLine, string sitem_code, string sitem_barcode, int irequire_qty,int actual_qty,string sitem_unit, out string sg)
        {
            string sSql = string.Format(@"insert into t_das_detail(line_id, item_code, item_barcode, require_qty, actual_qty, item_unit) values ({0}, '{1}', '{2}', {3}, 0, '{4}')",iLine, sitem_code, sitem_barcode,  irequire_qty, actual_qty, sitem_unit);
            return myShd.ExecutCmd(sSql, out sg);
        }
 
        /// <summary>
        ///  说明：根据波次号获取头表的Id
        /// </summary>
        /// <param name="sWave_No">波次号</param>
        /// <param name="sRegion_No">播种墙区域号</param>
        /// <returns></returns>
        private int GetHeadId(string sWave_No,string sRegion_No)
        {
            string sSql = string.Format("select head_id from  t_das_head h where h.WAVE_NO = '{0}' and h.REGION_NO= '{1}'", sWave_No, sRegion_No);
            DataSet ds = myShd.ExecuteSql(sSql);
            if (ds == null)
            {
                return 0;
            }
            else if (ds.Tables.Count == 0)
            {
                return 0;
            }
            else
            {
                int id = Convert.ToInt32(ds.Tables[0].Rows[0]["head_id"].ToString());
                return id;
            }
        }

        /// <summary>
        /// 说明：获取任务单行表的ID
        /// </summary>
        /// <param name="sORDER_NO">订单号</param>
        /// <param name="sLOCATOR">货位</param>
        /// <param name="iHeadId">头表的ID</param>
        /// <param name="sBoxNo">箱号</param>
        /// <returns>是否成功</returns>
        private int GetLineId(string sORDER_NO, string sLOCATOR, int iHeadId,string sBoxNo)
        {
            string sSql = string.Format("select line_id from  t_das_line  l where l.ORDER_NO = '{0}' and l.LOCATOR = '{1}' and l.head_id = {2} and Box_No = '{3}'", sORDER_NO, sLOCATOR, iHeadId, sBoxNo);
            DataSet ds = myShd.ExecuteSql(sSql);
            if (ds == null)
            {
                return 0;
            }
            else if (ds.Tables.Count == 0)
            {
                return 0;
            }
            else
            {
                int id = Convert.ToInt32(ds.Tables[0].Rows[0]["line_id"].ToString());
                return id;
            }
        }
        private delegate void delegateHandleLabel(int i);
        private void HandleLabel(int i)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new delegateHandleLabel(HandleLabel), new object[] { i });
            }
            else
            {
                if (i == 1)
                {
                    this.lblOK.Text = Convert.ToString(Convert.ToInt32(lblOK.Text) + 1);
                }
                else
                {
                    this.lblFail.Text = Convert.ToString(Convert.ToInt32(lblFail.Text) + 1);
                }
            }
        }

        /// <summary>
        ///  说明：把数据保存到DAS的头表中
        /// </summary>
        /// <param name="sWAVE_NO">波次号</param>
        /// <param name="sREGION_NO">播种墙的编号</param>
        /// <param name="sg">返回的信息</param>
        /// <returns></returns>
        private bool InsertDASHead(string sWAVE_NO,string sREGION_NO,out string sg)
        {
            //先执行删除
            string sSql = string.Format("delete from T_DAS_HEAD  where WAVE_NO = '{0}' and  region_no = '{1}'", sWAVE_NO, sREGION_NO);
            myShd.ExecutCmd(sSql, out sg);

            //执行插入
            sSql = string.Format("insert into t_das_head (wave_no,flag_head, region_no) values ('{0}',0, {1})", sWAVE_NO, sREGION_NO);
            return myShd.ExecutCmd(sSql, out sg);
        }

        private void btnExportPickOrder_Click(object sender, EventArgs e)
        {
            try
            {
                string sSql = @"select h.wave_no,
                                   h.region_no,
                                   l.box_no,
                                   l.order_no,
                                   l.locator,
                                   l.back_locator,
                                   d.item_code,
                                   d.item_barcode,
                                   d.require_qty,
                                   d.actual_qty,
                                   d.item_unit
                              from t_das_head h, t_das_line l, t_das_detail d
                             where h.head_id = l.head_id
                               and l.line_id = d.line_id
                               and h.flag_head = 0";
                DataSet dsQuery = myShd.ExecuteSql(sSql);
                impExcel.DataTableToExcel(dsQuery.Tables[0]);
                LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, "数据导出成功!");
            }
            catch (Exception ex)
            {
                string sMsg = string.Format("异常:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message);
                LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, sMsg);
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            impExcel.KillExcel();
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[frmCreateDataTable][已经关闭!]");
            this.Close();
            this.Dispose();
        }

        private void dataGridViewXDPS_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int rownum = (e.RowIndex + 1);
            System.Drawing.Rectangle rct = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y + 4, ((DataGridView)sender).RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, rownum.ToString(), ((DataGridView)sender).RowHeadersDefaultCellStyle.Font, rct, ((DataGridView)sender).RowHeadersDefaultCellStyle.ForeColor, System.Drawing.Color.Transparent, TextFormatFlags.HorizontalCenter);
        }
    }
}
