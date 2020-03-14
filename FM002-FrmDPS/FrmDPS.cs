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
using System.Threading;
using System.Web.Script.Serialization;
using MySql.Data.MySqlClient;
using MySql.Data;


namespace NFrmDPS
{
    public partial class DFrmDPS : Form
    {
        /*=====================标准变量===================================*/
        private const string sErrCode = "错误代码:FM002";
        //数据查询对象
        DAL.SQLHelpDataBase myShd;
        //数据库访问对象
        BLL.CreatBaseTable cltBase = new BLL.CreatBaseTable();
        //操作日志的保存
        BLL.CreatLogTable cltlog = new BLL.CreatLogTable();
        //用户名
        string mysUser;
        //访问通用配置文件
        dllConfigApp.ConfigApp mycfg;
        /*======================end=======================================*/

        /*==========================从控制器返回的变量====================*/
        const string sKeyValueFromScanner = "80";
        const string sKeyValueFromEnd = "FE";
        const string sKeyValueFromDPSTag = "12";
        const string sKeyValueFromDASTag = "11";
        const string sKeyValueFromTriggerPrint = "91";
        const string sKeyValueFromDASF2 = "91";
        const string sKeyValueFromPending = "FA";

        const string sKeyValueFromTag_RED = "11";
        const string sKeyValueFromTag_GREEN = "12";
        const string sKeyValueFromTag_BLUE = "13";
        const string sKeyValueFromTag_YELLOW = "14";
        const string sKeyValueFromTag_PINK = "15";
        const string sKeyValueFromTag_QING = "16";
        const string sKeyValueFromTag_WHITE = "17";


        /*==========================end ===================================*/
  
        /*=====================读取已有的配置===================================*/
        bool bOperateModel { get; set; } //true 为按订单拣料 false为分区拣料
        bool bLightModel { get; set; } //true 为递减模式 false拍灭即灭
        bool bAddBox { get; set; } // true 允许加箱子 false不允许加箱
        bool bLocator { get; set; } //true 显示货位的前2位  false 不显示货位前2位
        string sPendingBarcode { get; set; }//挂起条码
        /*=====================end===================================*/
        private DataTable regionTable = new DataTable();
        bool isWork = false;
        string production = string.Empty;//记录当前工艺
        string date = string.Empty;//记录当前日期
        string BP = string.Empty;//记录当前部品（零件）
       
        List<AisleLamp> lamps = new List<AisleLamp>();
      
        DataTable dbPick = new DataTable();//正在拣料单据清单
        List<DataModelDPS.DPSLayer1> listDataModel = new List<DataModelDPS.DPSLayer1>();//亮灯集合汇总

        NFrmDAS.Print print = new NFrmDAS.Print();
        string printerName = string.Empty;
        operateExcel.Class1 operateExcel = new operateExcel.Class1();
        string MysqlConnectStr = string.Empty;
        string OnOFF_TagId = string.Empty;
        string OnOFF_TagIp = string.Empty;

        public DFrmDPS(string sUser, dllConfigApp.ConfigApp cfg, DAL.SQLHelpDataBase shd)
        {
            RYB_PTL_API.RYB_PTL.UserResultAvailable -= new RYB_PTL_API.RYB_PTL.UserResultAvailableEventHandler(ptl_UserResultAvailable);
            InitializeComponent();
            mysUser = sUser;
            mycfg = cfg;
            myShd = shd;
            //绑定标签的返回值
            RYB_PTL_API.RYB_PTL.UserResultAvailable += new RYB_PTL_API.RYB_PTL.UserResultAvailableEventHandler(ptl_UserResultAvailable);
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[FrmDPS.dll][已经成功加载!]");
        }

        /// <summary>
        /// 说明:标签拍灭触发
        /// </summary>
        /// <param name="rs"></param>
        private void ptl_UserResultAvailable(RYB_PTL_API.RYB_PTL.RtnValueStruct rs)
        {
            string sIp = rs.Ip;
            string sTag = rs.Tagid;
            string sKeyCode = rs.KeyCode.ToUpper();
            string sLocator = rs.Locator;
            string sValue = rs.Number;
            HandleDataFromScanner(sIp, sTag, sKeyCode, sLocator, sValue);
        }
        object v1 = new object();
        object v2 = new object();
        object v3 = new object();
        object v4 = new object();
        object v5 = new object();
        object v6 = new object();
        private void HandleDataFromScanner(string sIp,string sTagId ,string sKeyCode,string sLocator,string sNumber)
        {
            switch (sKeyCode.ToUpper())
            {
                    //从扫描枪扫描返回
                case sKeyValueFromScanner:
                    {
                        lock (v1)
                        {
                            DosKeyValueFromScanner(sIp, sTagId, sKeyCode, sLocator, sNumber);
                        }
                        break;
                    }
                    //处理DPS亮灯模式返回
                case sKeyValueFromTag_RED:
                case sKeyValueFromTag_GREEN:
                    {
                        lock (v1)
                        {
                            DosKeyValueFromTag(sIp, sTagId, sKeyCode, sLocator, sNumber);
                        }
                        break;
                    }
                    //处理End返回
                case sKeyValueFromEnd:
                    {
                        lock (v4)
                        {
                            DosKeyValueFromEnd(sIp, sTagId, sKeyCode, sLocator, sNumber);
                        }
                        break;
                    }
                default:
                    break;
            }
        }
        
        private void CallAPIByPost()
        { 
         
        }
        private void DoKeyValueFromPrint(string sIp, string sTagId, string sKeyCode, string sLocator, string sValue)
        {
           
        }

        /// <summary>
        /// 说明：处理从扫描枪扫描返回的数据
        /// </summary>
        private void DosKeyValueFromScanner(string sIp, string sTagId, string sKeyCode, string sLocator, string sValue)
        {
            
        }

        
        /// <summary>
        /// 第一步：按分区作业：判断当前分区是否可以扫描下一张订单或按订单作业：判断能否扫描新的订单进来
        /// </summary>
        /// <param name="sComId"></param>
        /// <param name="sScanValue"></param>
        private bool CheckRegionBusyOrFree(string sComId)
        {
            if (bOperateModel)
            {
                //按订单
                if (listDataModel.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                //按分区作业
                if (listDataModel.Count == 0)
                {
                    return true;
                }
                else
                {
                   //查找当前分区是否有任务正在作业
                    return false;
                }
            }
        }
        struct structAisleLamp
        {
            public string sAisleLampIP { get; set; }
            public string sAisleId { get; set; }
            public string sOrderId { get; set; }
            public string sOrderIp { get; set; }
        }
        /// <summary>
        /// 把巷道灯红灯点亮3秒，提醒
        /// </summary>
        /// <param name="sAisleLampIP"></param>
        /// <param name="sAisleId"></param>
        private void LightAisleRed(object v)
        {
            structAisleLamp sp = (structAisleLamp)v;
            string sAisleLampIP = sp.sAisleLampIP;
            string sAisleId = sp.sAisleId;
            if (sAisleLampIP.Trim().Length > 0 && sAisleId.Trim().Length > 0)
            {
                RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAisleLampIP, sAisleId, 6, 3);
                System.Threading.Thread.Sleep(3000);
                RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAisleLampIP, sAisleId, 6, 0);
            }
        }
        /// <summary>
        /// 把巷道灯红灯点亮3秒并点亮订单显示器提醒
        /// </summary>
        /// <param name="sAisleLampIP"></param>
        /// <param name="sAisleId"></param>
        private void LightAisleRedAndShowOrderLED(object v)
        {
            structAisleLamp sp = (structAisleLamp)v;
            string sAisleLampIP = sp.sAisleLampIP;
            string sAisleId = sp.sAisleId;
            string sOrderIp = sp.sOrderIp;
            string sOrderId = sp.sOrderId;
            bool isShowOrderLED = false;
            bool isShowAisle = false;
            if (sOrderIp.Trim().Length > 0 && sOrderId.Trim().Length > 0)
            {
                RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sOrderIp, sOrderId, "No Order", 1);
                isShowOrderLED = true;
            }
            if (sAisleLampIP.Trim().Length > 0 && sAisleId.Trim().Length > 0)
            {
                RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAisleLampIP, sAisleId, 6, 3);
                isShowAisle = true;
            }
            System.Threading.Thread.Sleep(3000);
            if (isShowOrderLED)
            {
                RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sOrderIp, sOrderId, "----------", 1);
            }
            if (isShowAisle)
            {
                RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAisleLampIP, sAisleId, 6, 0);
            }
        }
        private void Updatet_pick_sap_LINE(string ids)
        {
            string sMsg = string.Empty;
            try
            {
                string str = string.Format("update t_pick_sap_line set flag=2 where line_id in ({0})",ids);//获取到数据
                myShd.ExecuteSql(str,out sMsg);
                if (sMsg != "")
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, " 更新line表状态2异常，异常信息：" + sMsg);
                }
                else
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Green,FontStyle.Regular,12,"更新line表状态2成功,ids:"+ids);
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "Updatet_pick_sap_LINE 异常：" + ex.ToString());
            }
        }
       
        private void UpdateFailCount(int line_id)
        {
            try
            {
                string sMsg = string.Empty;
                string sSql = string.Format("update t_pick_sap_line l set l.fail_count = l.fail_count + 1,l.last_update_date = sysdate where line_id = {0}", line_id);
                if (!myShd.ExecutCmd(sSql, out sMsg))
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "UpdateFailCount更新失败：" + sMsg);
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "UpdateFailCount 异常：" + ex.Message);
            }
        }
        private bool CheckOver()
        {
            try
            {
                if (listDataModel.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"异常："+ex.ToString());
                return false;
            }
        }
        private void UpdateProduction(int id)
        {
            string errorMsg = string.Empty;
            try
            {
                string str = string.Format("update t_production set Flag=1 where ID={0}", id);
                ExcuteMySql(str, out errorMsg);
                if (errorMsg == "")
                {

                }
            }
            catch(Exception ex)
            { 
            
            }
        }
        private void UpdateProductionDetail(int id,int number)
        {
            string errorMsg=string.Empty;
            try
            {
                string str = string.Format("update t_production_detail set Flag=1,AcqualQuantity={1} where ID={0}", id,number);
                ExcuteMySql(str,out errorMsg);
                if (errorMsg == "")
                { 
                 
                }
            }
            catch(Exception ex)
            { 
            
            }
        }
        private bool CheckProductionOver()
        {
            try
            {
                if (dataGridView2.Rows.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"异常:"+ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// 说明:处理从标签返回后拍灭值
        /// </summary>
        /// <param name="sIp"></param>
        /// <param name="sTagId"></param>
        /// <param name="sKeyCode"></param>
        /// <param name="sLocator"></param>
        /// <param name="sValue"></param>
        private void DosKeyValueFromTag(string sIp, string sTagId, string sKeyCode, string sLocator, string sValue)
        {
            try
            {
                /* 1.拍灭标签后,在集合中查找已经亮灯的此标签。
                 * 2.判断亮灯模式是一次还是递减模式，更新数据库的数量要么是1，要么是sValue 值
                 * 3.如果是递减模式，继续发亮次标签。如果发送的次数已够，进入第4步
                 * 4.查找是否还需要继续亮此标签（一对多的情况），如果没有移除集合
                 * 5.判断巷道灯下的标签是否已经全部被拍灭（集合是否为0），最后一个标签显示end
                 */
                if (sTagId == OnOFF_TagId && sIp == OnOFF_TagIp)
                {
                    int lightColor = Convert.ToInt32(sKeyCode.Substring(1, 1));
                    if (lightColor == 1)
                    {
                        if (CheckOver())
                        {
                            LightOnOff(2);
                            Thread.Sleep(500);
                            button2_Click_1(null, null);
                        }
                        else
                        {
                            LightOnOff(lightColor);
                        }
                    }
                    else
                    {
                        LightOnOff(lightColor);
                    }                                       
                }
                LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, string.Format(string.Format("标签拍灭.:[IP:{0}][Id:{1}][数量:{2}]", sIp, sTagId, sValue)));
                foreach (DataModelDPS.DPSLayer1 dy1 in listDataModel.ToArray())
                {
                    foreach (DataModelDPS.DPSLayer2 dy2 in dy1.dps2.ToArray())
                    {
                        DataModelDPS.DPSLayer3 dy3 = dy2.dps3.Find(x => (x.tag_id_ip == sIp) && (x.tag_id == sTagId) && (x.isLight == 1));
                        if (dy3 != null)
                        {
                            if (dy3.require_quantity < Convert.ToInt32(sValue))
                            {
                                RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(dy3.tag_id_ip, dy3.tag_id, dy3.require_quantity, 1, 2); 
                                return;
                            }
                            dy3.actual_quantity = Convert.ToInt32(sValue);
                            //更新数据到数据库
                            UpdateProductionDetail(dy3.ID,dy3.actual_quantity);
                            string sErrMsg = string.Empty;
                            dy2.dps3.Remove(dy3);
                            if (dy2.dps3.Count == 0)
                            {
                                dy1.dps2.Remove(dy2);
                                if (dy1.dps2.Count == 0)
                                {                                    
                                    listDataModel.Remove(dy1);
                                    UpdateProduction(dy1.id);
                                    UpdateTableAndRemoveItem(date, production, BP);
                                }
                                RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(dy2.aisle_lamp_id_ip, dy2.aisle_lamp_id, 5, 0);
                                RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(dy2.aisle_lamp_id_ip, dy2.aisle_lamp_id, 6, 0);
                                RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(dy2.aisle_lamp_id_ip, dy2.aisle_lamp_id, 7, 0);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("异常:[{0}6][{1}]", sErrCode, ex.Message));
            }
        }

        private void UpdateMiddleTable(int flag, int ID)
        {
            string sMsg = string.Empty;
            try
            {
                string str = string.Format("update t_pick_sap set flag={1} where ID={0}",ID,flag);
                myShd.ExecuteSql(str,out sMsg);
                if (sMsg != "")
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"异常："+sMsg);
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"异常："+ex);
            }
        }
        public delegate void updateTableAndRemoveItemEventHandle(string date, string production, string BP);
        private void UpdateTableAndRemoveItem(string date,string production,string BP)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new updateTableAndRemoveItemEventHandle(UpdateTableAndRemoveItem), new object[] { date, production, BP });
                return;
            }
            string errorMsg = string.Empty;
            try
            {
                string str = string.Format("update t_production set flag=1 where date='{0}' and productionName like '%{1}%' and production='{2}'",date,BP,production);
                ExcuteMySql(str,out errorMsg);
                if (errorMsg != "")
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"异常："+errorMsg);
                }
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Cells[0].Value.ToString() == production && dataGridView2.Rows[i].Cells[1].Value.ToString().IndexOf(BP) >= 0)
                    {
                        dataGridView2.Rows.RemoveAt(i);
                        break;
                    }
                }                                                
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"异常："+ex.ToString());
            }
        }
        /// <summary>
        /// 说明:处理拍灭End后的标签处理
        /// </summary>
        /// <param name="sIp"></param>
        /// <param name="sTagId"></param>
        /// <param name="sKeyCode"></param>
        /// <param name="sLocator"></param>
        /// <param name="sValue"></param>
        private void DosKeyValueFromEnd(string sIp, string sTagId, string sKeyCode, string sLocator, string sValue)
        {
            string sMsg = string.Empty;
            try
            {
                /* 1.从集合中查找对应的最后一个标签
                 * 2.根据此标签，灭掉巷道灯,灭订单显示器，播放音乐完成器
                 * 3.更新数据库中行表的状态
                 * 4.移除集合
                 */
                foreach (DataModelDPS.DPSLayer1 dy1 in listDataModel)
                {
                    DataModelDPS.DPSLayer2 dy2 = dy1.dps2.Find(x => x.tag_id_End == sTagId && x.tag_id_ip_End == sIp);
                    if (dy2 != null)
                    {
                        string sAileId = dy2.aisle_lamp_id;
                        string sAileIP = dy2.aisle_lamp_id_ip;
                        RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(dy2.aisle_lamp_id_ip, dy2.aisle_lamp_id, 7, 0);
                        RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(dy2.aisle_lamp_id_ip, dy2.aisle_lamp_id, 5, 0);
                        RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(dy2.aisle_lamp_id_ip, dy2.aisle_lamp_id, 6, 1);
                        listDataModel.Remove(dy1);
                        UpdateTableAndRemoveItem(date, production, BP);
                        if(CheckProductionOver())
                        {
                            foreach (DataGridViewRow dr in dataGridView1.Rows)
                            {
                                if (dr.Cells[0].Value.ToString() == production)
                                {
                                    dataGridView1.Rows.Remove(dr);
                                    break;
                                }
                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("异常:[{0}9][{1}]", sErrCode, ex.Message));
            }
        }
        /// <summary>
        /// 防止跨线程操作，直接赋值给TextBox文本框
        /// </summary>
        /// <param name="s"></param>
        private delegate void delegateDisplayTxtBoxScanBarcode(DevComponents.DotNetBar.Controls.TextBoxX tx,string s);
        private void DisplayTxtBoxScanBarcode(DevComponents.DotNetBar.Controls.TextBoxX tx,string s)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new delegateDisplayTxtBoxScanBarcode(DisplayTxtBoxScanBarcode), new object[] {tx,s });
            }
            else
            {
                tx.Text = s;
            }
        }

       
       
        private void btnReturn_Click(object sender, EventArgs e)
        {
            RYB_PTL_API.RYB_PTL.UserResultAvailable -= new RYB_PTL_API.RYB_PTL.UserResultAvailableEventHandler(ptl_UserResultAvailable);
            this.Close();
            this.Dispose();
        }
        private void dataGridViewX1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int rownum = (e.RowIndex + 1);
            System.Drawing.Rectangle rct = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y + 4, ((DataGridView)sender).RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, rownum.ToString(), ((DataGridView)sender).RowHeadersDefaultCellStyle.Font, rct, ((DataGridView)sender).RowHeadersDefaultCellStyle.ForeColor, System.Drawing.Color.Transparent, TextFormatFlags.HorizontalCenter);
        }

        private void FrmDPS_Load(object sender, EventArgs e)
        {
            //加载已配置的文件
            LoadSetting();
            LoadRegionTable();
        }

        private void FinishPickTasg(string sRegion, string pick_order)
        {
            try
            {
                string sMsg = string.Empty;
                string sSql = string.Format(@"update t_pick_sap_line l
                                               set l.flag = 1,
                                                   l.actual_quantity  = l.require_quantity,
                                                   l.last_update_date = sysdate 
                                             where l.region_no = '{0}'
                                               and l.pick_order = '{1}'", sRegion, pick_order);
                if (myShd.ExecutCmd(sSql, out sMsg))
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("区域：{0}，单号：{1}，强制完成成功!", sRegion, pick_order));
                    UpdatePickStatus(sRegion, pick_order, 4);
                }
                else
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("区域：{0}，单号：{1}，强制完成失败，原因：{2}", sRegion, pick_order,sMsg));
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "FinishPickTasg 异常：" + ex.Message);
            }
        }

        private void UpdateMiddleTable(string ID)
        {
            string sMsg = string.Empty;
            try
            {
                string str = string.Format("update t_pick_sap set flag=1 where ID in ({0})",ID);
                myShd.ExecuteSql(str,out sMsg);
                if (sMsg != "")
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"查询异常："+sMsg);
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "UpdateMiddleTable 异常：" + ex);
            }
        }
        private void LightTag(string sRegion, string pick_order)
        {
            try
            {
                string sMsg = string.Empty;
                string sSql = string.Format(@"select l.pick_order,
                                                     l.wms_id,
                                                     l.line_id,
                                                     l.item_code,
                                                     l.item_desc,
                                                     l.item_unit,
                                                     l.item_batch,
                                                     l.item_spec,
                                                     l.item_factory,
                                                     l.locator,
                                                     l.require_quantity,
                                                     l.actual_quantity,
                                                     l.flag,
                                                     t.region_no,
                                                     t.aisle_lamp_id_ip,
                                                     t.aisle_lamp_id,
                                                     t.tag_id_ip,
                                                     t.tag_id
                                                  from pickinfo p, t_pick_sap_line l, t_tag_locator t
                                                 where p.sgroupno = l.pick_order
                                                   and l.locator = t.locator
                                                   and p.iregionno = t.region_no
                                                   and p.sgroupno = '{1}'
                                                   and t.region_no = '{0}'
                                                   and (flag = 0 or flag=2) and l.region_no='{2}' order by l.locator", sRegion, pick_order,sRegion);
                DataSet ds = myShd.ExecuteSql(sSql, out sMsg);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && sMsg.Length == 0)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("LightTag 获取到区域：{0}，单号：{1} 准备亮灯！", sRegion, pick_order));
                  //  InsertListByDataTable(ds.Tables[0]);
                    UpdatePickStatus(sRegion, pick_order, 10);
                }
                else if (sMsg.Length != 0)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("LightTag 区域：{0}，单号：{1}查询数据失败：{2}", sRegion, pick_order, sMsg));
                }
                else
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("LightTag 区域：{0}，单号：{1}没有查询到数据！", sRegion, pick_order));
                    UpdatePickStatus(sRegion, pick_order, 10);//没有数据直接更新
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "LightTag 亮灯异常：" + ex.Message);
            }
        }

        private void UpdatePickStatus(string sRegion, string pick_order,int flag)
        {
            try
            {
                string sMsg = string.Empty;
                string sSql = string.Format("update pickinfo set iType = {0} where iregionno = '{1}' and sgroupno = '{2}'", flag, sRegion, pick_order);
                if (myShd.ExecutCmd(sSql, out sMsg))
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("区域：{0}，单号：{1}，更新状态:{2}成功!", sRegion, pick_order, flag));
                }
                else
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("区域：{0}，单号：{1}，更新状态:{2}失败，原因：{3}", sRegion, pick_order, flag, sMsg));
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "UpdatePickStatus 异常：" + ex.Message);
            }
        }

        private void LightAisleLamp(string sRegion)
        {
            DataRow[] drs = regionTable.Select(string.Format("region_no = '{0}'", sRegion));
            if (drs.Length > 0)
            {
                AisleLamp aisle = lamps.Find(x => x.region == sRegion);
                if (aisle != null)
                {
                    if (aisle.count > 2)
                    {
                        aisle.lightCount = 1;
                        return;
                    }
                    string sIp = drs[0]["aisle_lamp_id_ip"].ToString();
                    string saisle_lamp_id = drs[0]["aisle_lamp_id"].ToString();
                    RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sIp, saisle_lamp_id, 6, 0);
                    RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sIp, saisle_lamp_id, 7, 0);
                    RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sIp, saisle_lamp_id, 5, 3);
                    aisle.count++;
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, string.Format("区域：{0}有任务未完成，已点亮巷道灯{1}！", sRegion, saisle_lamp_id));
                }
                else
                {
                    aisle = new AisleLamp();
                    aisle.region = sRegion;
                    aisle.count = 0;
                    lamps.Add(aisle);
                    LightAisleLamp(sRegion);//点亮
                }
            }
        }

        /// <summary>
        /// 加载基础数据
        /// </summary>
        private void LoadRegionTable()
        {
            try
            {
                string sMsg = string.Empty;
                string sSql = "select distinct t.region_no,t.aisle_lamp_id_ip,t.aisle_lamp_id,t.tag_id,t.tag_id_ip from t_tag_locator t";
                DataSet ds = ExcuteMySql(sSql,out sMsg);
                if (sMsg.Length == 0 && ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    regionTable = ds.Tables[0];
                    Thread threadOffTag = new Thread(OffTag);
                    threadOffTag.IsBackground = true;
                    threadOffTag.Name = "OffTag";
                    threadOffTag.Start();
                   // OffTag();
                    DataRow[] dr = ds.Tables[0].Select(string.Format("region_no=2"));
                    foreach (DataRow dr1 in dr)
                    {
                        OnOFF_TagId = dr1["tag_id"].ToString();
                        OnOFF_TagIp = dr1["tag_id_ip"].ToString();
                    }
                }
                else if (sMsg.Length != 0)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "LoadRegionTable 加载基础数据出错：" + sMsg);
                }
                else
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "LoadRegionTable 没有查询到基础数据，请检查是否已经维护基础数据");
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "LoadRegionTable 异常：" + ex.Message);
            }
        }
        private void LightOnOff(int lightColor)
        {
            try
            {
                  RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(OnOFF_TagIp,OnOFF_TagId, 1, 1, lightColor);
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"异常："+ex.ToString());
            }
        }
        private void GetTodayProduction()
        {
            try
            {
                dataGridView2.Rows.Clear();
                string Year = dateTimePicker1.Value.Year.ToString();
                string Month = dateTimePicker1.Value.Month.ToString();
                string Day = dateTimePicker1.Value.Day.ToString();
                string date = Year + "-" + Month.PadLeft(2, '0') + "-" + Day.PadLeft(2, '0');
                this.date = date;
                GetProductionByDate(date);
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"异常："+ex.ToString());
            }
        }
        private void LoadSetting()
        {
            MysqlConnectStr = mycfg["ConnectionStr"];
            printerName = mycfg["printerName"];
            GetTodayProduction();
            LightOnOff(2);
        }
        string sQueryDataByPickOrder = @"select h.head_id,
                                               l.line_id,
                                               h.pick_order,
                                               h.box_no,
                                               h.status,
                                               l.item_code,
                                               l.item_desc,
                                               l.pick_user,
                                               l.flag,
                                               l.require_quantity,
                                               l.actual_quantity,
                                               l.item_unit,
                                               r.region_no,
                                               r.com_id,
                                               r.com_id_ip,
                                               r.finisher_id,
                                               r.finisher_id_ip,
                                               r.order_id,
                                               r.order_id_ip,
                                               r.aisle_lamp_id,
                                               r.aisle_lamp_id_ip,
                                               r.tag_id,
                                               r.tag_id_ip,
											   r.locator
                                          from t_pick_sap_head h, t_pick_sap_line l, t_tag_locator r
                                         where h.head_id = l.head_id
                                           and l.locator = r.locator
                                           and h.pick_order = '{0}'
                                           and r.com_id = '{1}'
                                           and h.status <>2";

        private void dataGridViewX1_RowPostPaint_1(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int rownum = (e.RowIndex + 1);
            System.Drawing.Rectangle rct = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y + 4, ((DataGridView)sender).RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, rownum.ToString(), ((DataGridView)sender).RowHeadersDefaultCellStyle.Font, rct, ((DataGridView)sender).RowHeadersDefaultCellStyle.ForeColor, System.Drawing.Color.Transparent, TextFormatFlags.HorizontalCenter);
        }

        private void DFrmDPS_FormClosing(object sender, FormClosingEventArgs e)
        {            
            RYB_PTL_API.RYB_PTL.UserResultAvailable -= new RYB_PTL_API.RYB_PTL.UserResultAvailableEventHandler(ptl_UserResultAvailable);
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[FrmDPS.dll][已经关闭!]");
        }

        private void SendData4831(string sIp, string sTagId, string item_desc,string item_spec,string item_batch,string locator,int num,int colorIndex)
        {
            try
            {
                if (item_desc.Length > 15)
                {
                    item_desc = item_desc.Substring(0,13);
                }
                string sContext = string.Format("名称:{0}^规格:{1}^批号:{2}^货位:{3}", item_desc, item_spec, item_batch, locator);
               // string lo = locator.Substring(locator.Length - 1, 1);
               // if (Convert.ToInt32(lo) <= 5)
               // {
                 //   RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit4831(sIp, sTagId, "B0" + lo, num, 2, 1, 1, 1, sContext);
              //  }
             //   else
              //  {
                    RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit4831(sIp, sTagId, num, colorIndex, 1, 1, 1, sContext);
               /// }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "SendData4831 异常：" + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        

        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            string sMsg = string.Empty;
            try
            {
                string str = string.Format("select * from t_pick_sap_line where flag=0");
                DataSet dt = myShd.ExecuteSql(str,out sMsg);
                if (sMsg != "")
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"异常："+sMsg);
                }
                else
                { 
                 
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"异常："+ex.ToString());
            }
        }
        private DataSet GetLightDetail(string lineNo)
        {
            DataSet dt = new DataSet();  

            string sMsg = string.Empty;
            try
            {
                string str = string.Format(@"select l.ID,
                                                    l.line_no,
                                                    l.pick_date,
                                                    l.pick_time,
                                                    l.item_code,
                                                    l.item_desc,
                                                    l.locator,
                                                    l.require_quantity,
                                                    z.tag_id,
                                                    z.tag_id_ip, 
                                                    z.aisle_lamp_id_ip,
                                                    z.aisle_lamp_id
                                             from t_pick_sap l,t_tag_locator z 
                                             where (flag=0 or flag=1) and z.locator=l.locator and l.line_no='{0}' order by pick_time", lineNo);
                dt = myShd.ExecuteSql(str,out sMsg);
                if (sMsg != "")
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"查询异常："+sMsg);
                }
                return dt;
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "GetLightDetail 异常：" + ex.ToString());
                return dt;
            }
        }
       
        private void InsertIntoDataModel(DataSet dt)
        {
            try
            {
                if (dt == null || dt.Tables[0].Rows.Count==0)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"数据为Null或没有数据");
                    return;
                }
                DataModelDPS.DPSLayer1 layer1 = new DataModelDPS.DPSLayer1();
                DataView dv = new DataView(dt.Tables[0]);
                DataTable dt1 = dv.ToTable(true,new string[]{"production","productionName","date","productionId"});
                layer1.date = dt1.Rows[0]["date"].ToString();
                layer1.production = dt1.Rows[0]["production"].ToString();
                layer1.productionName = dt1.Rows[0]["productionName"].ToString();
                layer1.id = Convert.ToInt32(dt1.Rows[0]["productionId"].ToString());
                layer1.dps2 = new List<DataModelDPS.DPSLayer2>();

                DataView dv1 = new DataView(dt.Tables[0]);
                DataTable dt2 = dv1.ToTable(true, new string[] { "aisle_lamp_id_ip", "aisle_lamp_id" });
                foreach (DataRow dr in dt2.Rows)
                {
                    DataModelDPS.DPSLayer2 layer2 = new DataModelDPS.DPSLayer2();
                    layer2.aisle_lamp_id_ip = dr["aisle_lamp_id_ip"].ToString();
                    layer2.aisle_lamp_id = dr["aisle_lamp_id"].ToString();
                    layer2.dps3 = new List<DataModelDPS.DPSLayer3>();

                    DataRow[] dr1 = dt.Tables[0].Select(string.Format("aisle_lamp_id_ip='{0}' and aisle_lamp_id='{1}'",layer2.aisle_lamp_id_ip,layer2.aisle_lamp_id));
                    foreach (DataRow dr2 in dr1)
                    {
                        DataModelDPS.DPSLayer3 layer3 = new DataModelDPS.DPSLayer3();
                       // layer3.ID =Convert.ToInt32(dr2["ID"]);
                        layer3.isLight = 0;
                        layer3.item_code = dr2["ItemCode"].ToString();
                       // layer3.item_desc = dr2["item_desc"].ToString();
                        layer3.require_quantity =Convert.ToInt32(dr2["RequireQuantity"]);
                        layer3.tag_id = dr2["tag_id"].ToString();
                        layer3.tag_id_ip = dr2["tag_id_ip"].ToString();
                        layer3.ID = Convert.ToInt32(dr2["ID"].ToString());
                        layer2.dps3.Add(layer3);
                    }
                    layer1.dps2.Add(layer2);
                }
                LightTag(layer1);
                listDataModel.Add(layer1);
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "InsertIntoDataModel 异常：" + ex.ToString());
            }
        }
        private void LightTag(DataModelDPS.DPSLayer1 layer1)
        {
            string id = string.Empty;
            try
            {
                ArrayList list = new ArrayList();
                foreach (DataModelDPS.DPSLayer2 layer2 in layer1.dps2)
                {
                    foreach (DataModelDPS.DPSLayer3 layer3 in layer2.dps3.ToArray())
                    {
                        if (list.Contains(layer3.tag_id))
                        {
                            continue;
                        }
                        RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(layer3.tag_id_ip, layer3.tag_id, layer3.require_quantity, 1, 2);
                        list.Add(layer3.tag_id);
                        layer3.isLight = 1;                        
                    }
                    RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(layer2.aisle_lamp_id_ip, layer2.aisle_lamp_id, 7, 0);
                    RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(layer2.aisle_lamp_id_ip, layer2.aisle_lamp_id, 5, 0);
                    RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(layer2.aisle_lamp_id_ip, layer2.aisle_lamp_id, 6, 1);
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "LightTag 异常：" + ex.ToString());
            }
        }
      
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void PendingOrder()
        {
            try
            {
                foreach (DataModelDPS.DPSLayer1 layer1 in listDataModel.ToArray())
                {
                    foreach (DataModelDPS.DPSLayer2 layer2 in layer1.dps2.ToArray())
                    {
                        foreach (DataModelDPS.DPSLayer3 layer3 in layer2.dps3.ToArray())
                        {
                            RYB_PTL_API.RYB_PTL.RYB_PTL_CloseDigit5(layer3.tag_id_ip,layer3.tag_id);
                        }
                        RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(layer2.aisle_lamp_id_ip, layer2.aisle_lamp_id, 5, 0);
                        RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(layer2.aisle_lamp_id_ip, layer2.aisle_lamp_id, 6, 0);
                        RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(layer2.aisle_lamp_id_ip, layer2.aisle_lamp_id, 7, 0);
                    }
                    listDataModel.Remove(layer1);
                }
            }
            catch(Exception ex) 
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "PendingOrder异常， 异常信息：" + ex);
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                PendingOrder();
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "挂起异常， 异常信息：" + ex);
            }
        }       
        
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
        private bool checkLineNoBusy()
        {
            try
            {
                if (listDataModel.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "checkLineNoBusy 异常,异常信息：" + ex);
                return false;
            }
        }
        private void ShowDatagridView(DataTable dt)
        {
            try
            {
                DataView dv = new DataView(dt);
                DataTable dt1 = dv.ToTable(true, new string[] { "region_no" });
                if (dt1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Add(dt1.Rows.Count);
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow[] dr = dt.Select(string.Format("region_no='{0}'", dt1.Rows[i]["region_no"].ToString()));
                        int count = dr.Length;
                        dataGridView1.Rows[i].Cells[0].Value = dt1.Rows[i]["region_no"].ToString();
                        dataGridView1.Rows[i].Cells[1].Value = count;
                        dataGridView1.Rows[i].Cells[2].Value = "待拣选";
                    }
                }
                else
                {
                    return;
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "ShowDatagridView 异常：" + ex.ToString());
            }
        }
        object objExcuteMySql = new object();
        private DataSet ExcuteMySql(string str,out string errorMsg)
        { 
         lock(objExcuteMySql)
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
                     MySqlCommand mySqlCommand = new MySqlCommand(str,mySqlConnection);
                     MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                     mda.Fill(dt);
                     return dt;
                 }
             }
             catch(Exception ex)
             {
                 errorMsg = ex.ToString();
                 return dt;
             }
         }
        }
        private void InsertIntoDataBase(DataTable dt)
        {
            try
            {

            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "InsertIntoDataBase 异常：" + ex.ToString());
            }
        }
        private void DealDatatable(DataTable dt)
        {
            try
            {
                ShowDatagridView(dt);
                InsertIntoDataBase(dt);                
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "DealDatatable 异常：" + ex);
            }
        }
        private void button1_Click_2(object sender, EventArgs e)
        {

        }
        private void GetDetailToShow(string prodoction,string date)
        {
            string sMsg = string.Empty;
            try
            {
                string str = string.Format("select * from T_PRODUCTION where production='{0}' and date='{1}' and flag=0", prodoction,date);
                DataSet dt = ExcuteMySql(str,out sMsg);
                if (sMsg != "")
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "查询明细数据 异常：" + sMsg);
                }
                else
                {
                    if (dt.Tables[0].Rows.Count == 0)
                    {
                        return;
                    }
                    dataGridView2.Rows.Clear();
                    dataGridView2.Rows.Add(dt.Tables[0].Rows.Count);
                    for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                    {
                        dataGridView2.Rows[i].Cells[0].Value = dt.Tables[0].Rows[i]["production"].ToString();
                        dataGridView2.Rows[i].Cells[1].Value = dt.Tables[0].Rows[i]["productionName"].ToString();
                        string flag = dt.Tables[0].Rows[i]["flag"].ToString();
                        dataGridView2.Rows[i].Cells[2].Value = "待作业";
                       // dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.Red;//标识为红色
                    }
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "GetDetailToShow 异常：" + ex.ToString());
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewSelectedRowCollection drCollection = dataGridView1.SelectedRows;
                foreach (DataGridViewRow dr in drCollection)
                {
                    string production = dr.Cells[0].Value.ToString();
                    GetDetailToShow(production,date);
                    this.production = production;
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"双击查看配膳明细 异常："+ex.ToString());
            }
        }
        private void GetProductionByDate(string date)
        {
            string errorMsg = string.Empty;
            try
            {
                string str = string.Format("select distinct production,flag from T_PRODUCTION WHERE date='{0}' and flag=0",date);
                DataSet dt = ExcuteMySql(str,out errorMsg);
                if (errorMsg != "")
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "异常：" + errorMsg);
                }
                else
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        dataGridView1.Rows.Clear();
                        dataGridView1.Rows.Add(dt.Tables[0].Rows.Count);
                        for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                        {
                            dataGridView1.Rows[i].Cells[0].Value = dt.Tables[0].Rows[i]["production"].ToString();
                            string flag = dt.Tables[0].Rows[i]["flag"].ToString();
                            if (flag == "0")
                            {
                                dataGridView1.Rows[i].Cells[1].Value = "待作业";
                            }
                            else if (flag == "1")
                            {
                                dataGridView1.Rows[i].Cells[1].Value = "待作业";
                            }
                            else if (flag == "2")
                            {
                                dataGridView1.Rows[i].Cells[1].Value = "已完成";
                            }
                        }
                    }                    
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "异常：" + ex.ToString());
            }
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (isWork)
                {
                    string[] str = this.date.Split('-');
                    int year =Convert.ToInt32(str[0]);
                    int month = Convert.ToInt32(str[1]);
                    int day = Convert.ToInt32(str[2]);
                    dateTimePicker1.Value = new DateTime(year,month,day);
                    return;
                }
                dataGridView2.Rows.Clear();
                dataGridView1.Rows.Clear();
                string Year = dateTimePicker1.Value.Year.ToString();
                string Month = dateTimePicker1.Value.Month.ToString();
                string Day = dateTimePicker1.Value.Day.ToString();
                string date = Year + "-" + Month.PadLeft(2, '0') + "-" + Day.PadLeft(2, '0');
                this.date = date;
                GetProductionByDate(date);
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"异常："+ex.ToString());
            }
            
        }
        public delegate void ClickEventHandle(object sender,EventArgs e);
        private void button2_Click_1(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ClickEventHandle(button2_Click_1), new object[] { sender, e });
                return;
            }
            string errorMsg = string.Empty;
            string production = string.Empty;
            try
            {
                if (dataGridView1.Rows.Count > 0 && dataGridView2.Rows.Count > 0)
                {
                    string text = string.Empty;
                    DataGridViewSelectedRowCollection drCollection = dataGridView2.SelectedRows;
                    foreach (DataGridViewRow dr in drCollection)
                    {
                        production = dr.Cells[1].Value.ToString();                      
                    }
                    if (production.IndexOf("/") >= 0)
                    {
                        string[] str = production.Split('/');
                        text = str[0];
                    }
                    else if (production.IndexOf("(") >= 0)
                    {
                        string[] str = production.Split('(');                        
                            //string[] str1 = str[0].Split(' ');
                            text = str[0];                        
                    }
                    else if (production.IndexOf("（") >= 0)
                    {
                        string[] str = production.Split('（');
                        //string[] str1 = str[0].Split(' ');
                        text = str[0];
                    }
                    else
                    {
                        text = production;
                    }
                    print.PrintBarcode(text,printerName,this.production, out errorMsg);
                    textBox1.Focus();
                    this.BP = text;
                    LightOnOff(1);
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "button2_Click_1 异常：" + ex.ToString());
            }
        }
        private void GetBP_DetailByBPAndProduction(string production, string BP,string date)
        {
            string sMsg = string.Empty;
            try
            {
                string str = string.Format(@"select p.production,
                                                    p.productionName,
                                                    p.date,                                                    
                                                    d.ItemCode,
                                                    d.Locator,
                                                    d.RequireQuantity,
                                                    d.ID,
                                                    d.productionId,
                                                    l.aisle_lamp_id,
                                                    l.aisle_lamp_id_ip,
                                                    l.tag_id,
                                                    l.tag_id_ip
                                            from t_production p,
                                                 t_production_detail d,
                                                  t_tag_locator l
                                            where p.ID=d.ProductionId and 
                                                  p.production='{0}' and 
                                                  p.productionName like '%{1}%' and 
                                                  p.date='{2}' AND
                                                  d.Locator=l.locator and l.region_no='1' and d.flag=0", production, BP,date);
                DataSet dt = ExcuteMySql(str,out sMsg);
                if (sMsg != "")
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "异常：" +sMsg);
                }
                else
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        InsertIntoDataModel(dt);
                        isWork = true;
                    }
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "GetBP_DetailByBPAndProduction 异常：" + ex.ToString());
                isWork = false;
            }
        }
        public delegate void OffTagEventHandle(object obj);
        private void OffTag(object obj)
        {
            //if (this.InvokeRequired)
            //{
            //    this.BeginInvoke(new OffTagEventHandle(OffTag), new object[] { obj });
            //    return;
            //}
            try
            {                
                DataView dv = new DataView(regionTable);
                DataTable dt1 = dv.ToTable(true,new string[]{"tag_id_ip"});
                foreach (DataRow dr in dt1.Rows)
                {
                    string ip = dr["tag_id_ip"].ToString();
                    RYB_PTL_API.RYB_PTL.RYB_PTL_CloseDigit5(ip, "AAAA");//灭标签
                    RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(ip, "AAAB", 5, 0);
                    RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(ip, "AAAB", 6, 0);
                    RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(ip, "AAAB", 7, 0);
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "OffTag 异常：" + ex.ToString());
            }
        }
        private void GetBpDetailToShow(string production,string BP)
        {
            string errorMsg = string.Empty;
            try
            {
                string str = string.Format(@"select p.production,
                                                    p.productionName,
                                                    p.date,
                                                    d.ItemCode,
                                                    d.Locator,
                                                    d.RequireQuantity 
                                             from t_production p,t_production_detail d
                                             where p.ID=d.productionId and 
                                                   p.production='{0}' and 
                                                   p.productionName like '%{1}%' and 
                                                   p.date='{2}' and d.flag=0 order by d.locator", production,BP,date);
                DataSet dt = ExcuteMySql(str,out errorMsg);
                if (errorMsg != "") 
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "查询 异常：" + errorMsg);
                }
                else
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        FrmDetailData frmDetail = new FrmDetailData(dt.Tables[0]);
                        frmDetail.Show();
                    }
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "GetBpDetailToShow 异常：" + ex.ToString());
            }
        
        }
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewSelectedRowCollection drCollection = dataGridView2.SelectedRows;
                foreach (DataGridViewRow dr in drCollection)
                {
                    string productionName = dr.Cells[1].Value.ToString();
                    if (productionName.IndexOf("/") >= 0)
                    {
                        string[] str = productionName.Split('/');
                        string text = str[0];
                        GetBpDetailToShow(production, text);        
                    }
                    else if (productionName.IndexOf("(") >= 0)
                    {
                        string[] str = productionName.Split('(');
                            GetBpDetailToShow(production,str[0]);                  
                    }
                    else if (productionName.IndexOf("（") >= 0)
                    {
                        string[] str = productionName.Split('（');
                         GetBpDetailToShow(production, str[0]);                      
                    }                                               
                }               
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "dataGridView2_CellDoubleClick 异常：" + ex.ToString());
            }            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    //if (BP.IndexOf("-") >= 0)
                    //{
                    //    string[] str = BP.Split('-');
                    //    string b = str[0];
                    GetBP_DetailByBPAndProduction(production, BP, date);
                    textBox1.SelectAll();
                    // }

                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "textBox1_KeyDown 异常：" + ex.ToString());
            }
            //finally
            //{
            //    
            //}
        }
        private void TurnOffTag()
        {
            try
            {
                RYB_PTL_API.RYB_PTL.RYB_PTL_CloseDigit5(OnOFF_TagIp, OnOFF_TagId);
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "OffTag 异常：" + ex.ToString());
            }
        }
        private void button1_Click_3(object sender, EventArgs e)
        {
            try
            {
                if (listDataModel.Count > 0)
                {
                    PendingOrder();
                }
                isWork = false;
                TurnOffTag();
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "button1_Click_3 异常：" + ex.ToString());
            }
        }
    }
}
