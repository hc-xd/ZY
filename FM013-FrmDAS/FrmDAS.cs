﻿using System;
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
using System.Timers;

namespace NFrmDAS
{
    public partial class DFrmDAS : Form
    {
        /*=====================标准变量===================================*/
        private const string sErrCode = "错误代码:FM013";
        //数据查询对象
        DAL.SQLHelpDataBase myShd;
        //用户名
        string mysUser;
        //访问通用配置文件
        dllConfigApp.ConfigApp mycfg;
        /*======================end=======================================*/

        /*==========================从控制器返回的变量====================*/
        const string sKeyValueFromScanner = "80";//扫描枪扫描返回
        const string sKeyValueFromEnd = "FE";//从End标签返回
        const string sKeyValueFromDASTagGreen = "12"; //手工拍灭返回或感应灭灯的返回值
        const string sKeyValueFromDASTagRed = "11"; //手工拍灭返回或感应灭灯的返回值
        const string sKeyValueFromDASTagSensorError = "A1"; //本不应该亮，被误触发后，返回的指令
        const string sKeyValueFromDASTagSensorErrorOff = "A2"; //标签被误触发点亮后，拍灭后的返回值
        const string sKeyValueFromDASTotalTag = "14"; //拍灭汇总标签
        const string sKeyValueFromDASBackTag = "15"; //拍灭背后的单灯标签
        const string sKeyValueFromDASBackTag1 = "51"; //拍灭背后的单灯标签(拍灭汇总标签后，背后显示红色快速闪烁)
        const string sKeyValueFromDASTotalTagF2 = "91"; //汇总标签被拍灭后，重新加载回来此波次的数据
        /*==========================end ===================================*/
  
        /*=====================读取已有的配置===================================*/
        bool bOperateModel { get; set; } //分播的作业模式
        bool bLightModel { get; set; }  // 亮灯模式
        string sPendingBarcode { get; set; }//挂起条码
        string sWaveCodeHead { get; set; } //波次条码的特征
        bool bFrontAndBackSync { get; set; } //前后标签同步
        int iDownload { get; set; } //下载数据的时间间隔
        int iUpload { get; set; } // 上载数据的时间间隔
        /*=====================end===================================*/

        /* =========================标签、货位、控制器的对应关系表数据，提前查询出来================*/
        DataSet dsBase = new DataSet();

        /*============================end============================================================*/

        dllConfigApp.ConfigApp cfgsql = new dllConfigApp.ConfigApp("SQL_DAS_GetByWaveNo.xml");//sql语句所在的位置
        dllConvertBase64.ConvertBase64 base64 = new dllConvertBase64.ConvertBase64();

        DataTable dbDAS = new DataTable();//正在分播单据
        List<DataModelDAS.DASLayer1> listDataModel = new List<DataModelDAS.DASLayer1>();//亮灯集合汇总

        public DFrmDAS(string sUser, dllConfigApp.ConfigApp cfg, DAL.SQLHelpDataBase shd)
        {
            InitializeComponent();
            RYB_PTL_API.RYB_PTL.UserResultAvailable -= new RYB_PTL_API.RYB_PTL.UserResultAvailableEventHandler(ptl_UserResultAvailable);
            mysUser = sUser;
            mycfg = cfg;
            myShd = shd;
            //绑定标签的返回值
            RYB_PTL_API.RYB_PTL.UserResultAvailable += new RYB_PTL_API.RYB_PTL.UserResultAvailableEventHandler(ptl_UserResultAvailable);
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[FrmDAS.dll][已经成功加载!]");
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
        object v7 = new object();
        object v8 = new object();
        object v9 = new object();
        object v10 = new object();
        object v11 = new object();
       /// <summary>
       /// 处理返回的数据
       /// </summary>
       /// <param name="sIp"></param>
       /// <param name="sTagId"></param>
       /// <param name="sKeyCode"></param>
       /// <param name="sLocator"></param>
       /// <param name="sNumber"></param>
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
                    //本不应该亮，被误触发后返回的值
                case sKeyValueFromDASTagSensorError:
                    {
                        lock (v6)
                        {
                            DoKeyValueFromDASTagSensorError(sIp, sTagId, sKeyCode, sLocator, sNumber);
                        }
                        break;
                    }
                //标签被误触发点亮后，拍灭后的返回值
                case sKeyValueFromDASTagSensorErrorOff:
                    {
                        lock (v7)
                        {
                            DoKeyValueFromDASTagSensorErrorOff(sIp, sTagId, sKeyCode, sLocator, sNumber);
                        }
                        break;
                    }
                // 拍灭汇总标签亮对应的数据
                case sKeyValueFromDASTotalTag:
                    {
                        lock (v8)
                        {
                            DoKeyValueFromDASTotalTag(sIp, sTagId, sKeyCode, sLocator, sNumber);
                        }
                        break;
                    }
                //处理DAS亮灯模式返回
                case sKeyValueFromDASTagGreen:
                    {
                        lock (v2)
                        {
                            DoKeyValueFromDASTagGreen(sIp, sTagId, sKeyCode, sLocator, sNumber);
                        }
                        break;
                    }
                 //拍灭汇总标签,格口上的标签亮起红色后，再被拍灭后的处理
                case sKeyValueFromDASTagRed:
                    {
                        lock (v9)
                        {
                            DoKeyValueFromDASTagRed(sIp, sTagId, sKeyCode, sLocator, sNumber);
                        }
                        break;
                    }
                //拍灭背后标签的处理
                case sKeyValueFromDASBackTag:
                case sKeyValueFromDASBackTag1:
                    {
                        lock (v10)
                        {
                            DoKeyValueFromDASBackTag(sIp, sTagId, sKeyCode, sLocator, sNumber);
                        }
                        break;
                    }
                case sKeyValueFromDASTotalTagF2:
                    {
                        lock (v11)
                        {
                            DoKeyValueFromDASTotalTagF2(sIp, sTagId, sKeyCode, sLocator, sNumber);
                        }
                        break;
                    }
                default:
                    break;
            }
        }
        private void DoKeyValueFromDASTotalTagF2(string sIp, string sTagId, string sKeyCode, string sLocator, string sNumber)
        {
            try
            {
                //从大集合中找此汇总标签的集合
                DataModelDAS.DASLayer1 dy1 = listDataModel.Find(x => x.sTagIpTotal == sIp && x.sTagIdTotal == sTagId);
                if (dy1 != null)
                {
                    //
                    string sComIp = dy1.Com_Ip;
                    string sCompId = dy1.Com_id;
                    string sOrder = dy1.wave_no;
                    //调用挂起指令
                    TodoPendingBarcode(sComIp, sCompId, sKeyCode, sLocator,sPendingBarcode);
                    //重新把波次加载进来
                    TodoWaveBarcode(sComIp, sCompId, sKeyCode, sLocator, sOrder);
                }
            }
              catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
            }
        }
        /// <summary>
        /// 说明：
        /// </summary>
        /// <param name="sIp"></param>
        /// <param name="sTagId"></param>
        /// <param name="sKeyCode"></param>
        /// <param name="sLocator"></param>
        /// <param name="sValue"></param>
        private void DoKeyValueFromDASBackTag(string sIp, string sTagId, string sKeyCode, string sLocator, string sValue)
        {
            try
            {
                string sOrderIp = string.Empty;
                string sOrderId = string.Empty;
                string sAisleIp = string.Empty;
                string sAisleId = string.Empty;
                //根据背后标签获取巷道灯的IP、巷道灯地址
                //把背后的标签反推到对应的前面标签ID
                string sTagIdFront = "0"+sTagId.Substring(1, sTagId.Trim().Length - 1);
                GetBaseInforFromDataBase(string.Empty, sIp, sTagIdFront, out sOrderIp, out sOrderId, out sAisleIp, out sAisleId);
                //从集合中找第一层
                DataModelDAS.DASLayer1 dy1 = listDataModel.Find(x => x.aisle_lamp_id_ip == sAisleIp && x.aisle_lamp_id == sAisleId);
                if (dy1 == null)
                {
                    return;
                }
                else
                {
                    List<DataModelDAS.DASLayer2> dy2 = dy1.DAS2.FindAll(x => x.tag_back_ip == sIp && x.tag_back_id == sTagId);
                    if (dy2.Count ==0)
                    {
                        return;
                    }
                    else
                    {

                        //
                        foreach (DataModelDAS.DASLayer2 dd2 in dy2)
                        {
                            myShd.ExecutCmd(string.Format("update t_das_line set flag_line=2 where line_id={0}", dd2.line_id));
                            dy1.DAS2.Remove(dd2);
                            if (dy1.DAS2.Count == 0) //当前分区的波次订单已经全部分播完成
                            {
                                //灭巷道灯
                                RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAisleIp, sAisleId, 7, 0);
                                //灭订单显示器
                                RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sOrderIp, sOrderId, "End", 0);
                                RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sOrderIp, sOrderId, "End", 1);

                                //更新波次的状态

                                string sSql2 = string.Format("update t_das_head set flag_head = 2,last_update_date = getdate() where head_id = {0}", dy1.head_id);
                                string sMsg2 = string.Empty;
                                bool bExecut2 = myShd.ExecutCmd(sSql2, out sMsg2);
                                if (bExecut2)
                                {
                                    //从大集合中移除第一层
                                    listDataModel.Remove(dy1);
                                    LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("波次号:{0},Head_Id:{1}更新状态为2，提交数据成功.", dy1.wave_no, dy1.head_id));
                                }
                                else
                                {
                                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("波次号:{0},Head_Id:{1}更新状态为2，提交数据失败.", dy1.wave_no, dy1.head_id));
                                    bExecut2 = myShd.ExecutCmd(sSql2, out sMsg2);//再提交一次
                                    listDataModel.Remove(dy1);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
            }
        }
        /// <summary>
        /// 说明：拍灭汇总标签，格口的标签被点亮为红色后，再被拍灭处理
        /// </summary>
        /// <param name="sIp"></param>
        /// <param name="sTagId"></param>
        /// <param name="sKeyCode"></param>
        /// <param name="sLocator"></param>
        /// <param name="sValue"></param>
        private void DoKeyValueFromDASTagRed(string sIp, string sTagId, string sKeyCode, string sLocator, string sValue)
        {
            try
            {
                /*  1. 首先找到被拍灭格口标签对应的dylayer2
                 *  2. 把格口背后的单灯标签点亮为红色（正常的为绿色）
                 *  3. 从列表中移除此记录行
                 *  4. 从集成中移除
                  */
                foreach (DataModelDAS.DASLayer1 dy1 in listDataModel)
                {
                    DataModelDAS.DASLayer2 dy2 = dy1.DAS2.Find(x => x.tag_id_ip == sIp && x.tag_id == sTagId);
                    if (dy2 == null)
                    {
                        continue;
                    }
                    else
                    {
                        //如果有异常，不能处理。
                        if (dy1.ListlarmTag.Count > 0)
                        {
                            if (dy1.ListlarmTag.Contains(sTagId))
                            {
                                dy1.ListlarmTag.Remove(sTagId);
                                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("IP:{0},Id:{1}，不能触发此标签.需要先处理异常的标签.", sIp, sTagId));
                                //重新点亮此标签
                                RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(sIp, sTagId, Convert.ToInt32(sValue), 1, 1);
                            }
                            else
                            {
                                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("IP:{0},Id:{1}，不能触发此标签.需要先处理异常的标签.", sIp, sTagId));
                                //重新点亮此标签
                                RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(sIp, sTagId, Convert.ToInt32(sValue), 1, 1);
                                return;
                            }
                        }
                        //点亮背后的格口标签为红色
                        RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(dy2.tag_back_ip, dy2.tag_back_id, 1, 5, 1);
                        //从列表中移除
                        foreach (DataModelDAS.DASLayer3 dy3 in dy2.DAS3)
                        {
                            RemoveListFromDataGridViewXDetailId(dy3.detail_id);
                        }
                        //从集合中移除
                        dy1.DAS2.Remove(dy2);
                        if (dy1.DAS2.Count == 0)
                        {
                            //熄灭巷道灯
                            RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(dy1.aisle_lamp_id_ip, dy1.aisle_lamp_id,7, 0);
                            //熄灭订单显示器
                            RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(dy1.order_id_ip, dy1.order_id, "End", 0);
                            RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(dy1.order_id_ip, dy1.order_id, "End", 1);
                            //更新波次的状态

                            string sSql2 = string.Format("update t_das_head set flag_head = 2,last_update_date = getdate() where head_id = {0}", dy1.head_id);
                            string sMsg2 = string.Empty;
                            bool bExecut2 = myShd.ExecutCmd(sSql2, out sMsg2);
                            if (bExecut2)
                            {
                                LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("波次号:{0},Head_Id:{1}更新状态为1，提交数据成功.", dy1.wave_no, dy1.head_id));
                            }
                            listDataModel.Remove(dy1);
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
            }
        }

        object vGetBaseInforFromDataBase = new object();
        /// <summary>
        /// 说明：为了提高访问的速度，把标签对应关系放入到内存表中
        /// </summary>
        /// <param name="sIp">控制器的IP地址</param>
        /// <param name="sTagId">标签地址</param>
        private void GetBaseInforFromDataBase(string sComId,string sIp,string sTagId,out string sorder_id_ip, out string sorder_id, out string sAISLE_LAMP_ID_IP, out string AISLE_LAMP_ID)
        {
            lock (vGetBaseInforFromDataBase)
            {
                sorder_id_ip = string.Empty;
                sorder_id = string.Empty;
                AISLE_LAMP_ID = string.Empty;
                sAISLE_LAMP_ID_IP = string.Empty;
                try
                {
                    string sSql = string.Empty;
                    if (dsBase == null || dsBase.Tables.Count == 0 || dsBase.Tables[0].Rows.Count == 0)
                    {
                        sSql = "select distinct order_id_ip, order_id, AISLE_LAMP_ID_IP, AISLE_LAMP_ID,tag_id_ip,tag_id,com_id,com_id_ip from T_TAG_LOCATOR";
                        string sMsg = string.Empty;
                        dsBase = myShd.ExecuteSql(sSql, out sMsg);
                        if (dsBase == null || dsBase.Tables.Count == 0 || dsBase.Tables[0].Rows.Count == 0)
                        {
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "此扫描枪扫描的数据没有查询到巷道灯、订单显示器的信息，无法提示报警");
                        }
                    }
                    if (dsBase != null && dsBase.Tables.Count > 0 && dsBase.Tables[0].Rows.Count >0)
                    {
                        DataRow[] drs;
                        if (sComId.Trim().Length > 0)
                        {
                            drs = dsBase.Tables[0].Select(string.Format("com_id = '{0}'", sComId));
                        }
                        else
                        {
                            drs = dsBase.Tables[0].Select(string.Format("tag_id_ip = '{0}' and tag_id = '{1}'", sIp, sTagId));
                        }
                        if (drs.Length > 0)
                        {
                            sorder_id_ip = drs[0]["order_id_ip"].ToString();
                            sorder_id = drs[0]["order_id"].ToString();
                            sAISLE_LAMP_ID_IP = drs[0]["AISLE_LAMP_ID_IP"].ToString();
                            AISLE_LAMP_ID = drs[0]["AISLE_LAMP_ID"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
                }
            }
        }

        /// <summary>
        /// 说明：处理误触发标签，也是说亮灯位置与实际放的物位置不一致，引起的标签亮起并报警。
        /// </summary>
        /// <param name="sIp"></param>
        /// <param name="sTagId"></param>
        /// <param name="sKeyCode"></param>
        /// <param name="sLocator"></param>
        /// <param name="sValue"></param>
    private void DoKeyValueFromDASTagSensorError(string sIp, string sTagId, string sKeyCode, string sLocator, string sValue)
        {
            try
            {
                /* 1.根据此亮灯标签位置，从后台系统种找到对应的巷道灯
                 * 2.把红色巷道灯一直点亮为快速闪烁
                 * 3.把此错误的标签放入到泛型集合中，记录着此标签，直接处理了此标签后，才可以执行下一步的动作
                 */
                string sOrderIp = string.Empty;
                string sOrderId = string.Empty;
                string sAisleIp = string.Empty;
                string sAisleId = string.Empty;
                GetBaseInforFromDataBase(string.Empty, sIp, sTagId, out sOrderIp, out sOrderId, out sAisleIp, out sAisleId);
                RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAisleIp, sAisleId, 6, 1);//把巷道灯亮为红色，提醒误触发了标签。
                //把此错误的标签放入到泛型集合中
                if (listDataModel != null && listDataModel.Count != 0)
                {
                    DataModelDAS.DASLayer1 dy1 = listDataModel.Find(x => x.aisle_lamp_id_ip == sAisleIp && x.aisle_lamp_id == sAisleId);
                    dy1.ListlarmTag.Add(sTagId);
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("误触发了标签,IP：{0},ID:{1}", sIp, sTagId));
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
            }
        }

        /// <summary>
        /// 说明：标签被误触发点亮后，拍灭后的返回值。
        /// </summary>
        /// <param name="sIp"></param>
        /// <param name="sTagId"></param>
        /// <param name="sKeyCode"></param>
        /// <param name="sLocator"></param>
        /// <param name="sValue"></param>
        private void DoKeyValueFromDASTagSensorErrorOff(string sIp, string sTagId, string sKeyCode, string sLocator, string sValue)
        {
            try
            {
                string sOrderIp = string.Empty;
                string sOrderId = string.Empty;
                string sAisleIp = string.Empty;
                string sAisleId = string.Empty;
                GetBaseInforFromDataBase(string.Empty, sIp, sTagId, out sOrderIp, out sOrderId, out sAisleIp, out sAisleId);
                RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAisleIp, sAisleId, 6, 0);//把巷道灯的红灯灭掉

                //把此错误的标签从泛型集合中移除
                if (listDataModel.Count != 0)
                {
                    DataModelDAS.DASLayer1 dy1 = listDataModel.Find(x => x.aisle_lamp_id_ip == sAisleIp && x.aisle_lamp_id == sAisleId);
                    if (dy1 != null)
                    {
                        dy1.ListlarmTag.Remove(sTagId);
                    }
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, string.Format("误触发了标签,IP：{0},ID:{1}已被拍灭", sIp, sTagId));

                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
            }
        }

        /// <summary>
        /// 说明：拍灭汇总标签,需要显示各自对应的位置标签数量
        /// </summary>
        /// <param name="sIp"></param>
        /// <param name="sTagId"></param>
        /// <param name="sKeyCode"></param>
        /// <param name="sLocator"></param>
        /// <param name="sValue"></param>
        private void DoKeyValueFromDASTotalTag(string sIp, string sTagId, string sKeyCode, string sLocator, string sValue)
        {
            try
            {
                /* 1.首先从泛型集中找到次汇总标签对应的集合。
                 * 2.把每个订单对应的未分播的记录行数据累加
                 * 3.把数据显示在标签上
                 * */
                DataModelDAS.DASLayer1 dy1 = this.listDataModel.Find(x => x.sTagIpTotal == sIp && x.sTagIdTotal == sTagId);
                if (dy1 == null)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, string.Format("IP：{0},ID:{1} 拍灭汇总标签，从集合中未查询到。", sIp, sTagId));
                    return;
                }
                else
                {
                    //判断在拍灭汇总标签前，是否存在异常的标签，如果存在不允许拍灭汇总标签，直接返回去点亮。
                    if (dy1.ListlarmTag.Count > 0)
                    {
                        LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("IP：{0},ID:{1} 拍灭汇总标签前，先处理异常的标签", sIp, sTagId));
                        RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(sIp, sTagId, Convert.ToInt32(sValue), 1, 4);
                        return;
                    }
                    //根据波次对应的dy1，把对应的标签放入到集合中
                    ArrayList alistFrontTags = new ArrayList();
                    foreach (DataModelDAS.DASLayer2 dy2 in dy1.DAS2)
                    {
                        string sTag_Id = dy2.tag_id;
                        if (!alistFrontTags.Contains(sTag_Id))
                        {
                            alistFrontTags.Add(sTag_Id);
                        }
                    }
                    foreach (string s in alistFrontTags)
                    {
                        List<DataModelDAS.DASLayer2> listdy2 = dy1.DAS2.FindAll(x => x.tag_id == s);
                        if (listdy2 == null)
                        {
                            continue;
                        }
                        else
                        {
                            int iNum = 0;
                            string sTag_Ip = string.Empty;
                            string sTag_Id = string.Empty;
                            foreach (DataModelDAS.DASLayer2 dy2 in listdy2)
                            {
                                sTag_Ip = dy2.tag_id_ip;
                                sTag_Id = dy2.tag_id;
                                foreach (DataModelDAS.DASLayer3 dy3 in dy2.DAS3)
                                {
                                    iNum += dy3.require_qty - dy3.actual_qty;
                                }
                            }
                            if (iNum > 0)
                            {
                                //点亮标签
                                RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(sTag_Ip, sTag_Id, iNum, 1, 1);
                                dy1.iStatus = 1;//被拍灭过，不能扫描下一个物料
                            }
                        }
                    }
                    //foreach (DataModelDAS.DASLayer2 dy2 in dy1.DAS2)
                    //{
                    //    //同一个波次、同一个客户物料，可能会装到不同的箱子里，在行表中会存在多行。2016-11-09
                    //    string sTag_Ip = dy2.tag_id_ip;
                    //    string sTag_Id = dy2.tag_id;
                    //    int iNum = 0;
                    //    foreach (DataModelDAS.DASLayer3 dy3 in dy2.DAS3)
                    //    {
                    //        iNum += dy3.require_qty - dy3.actual_qty;
                    //    }
                    //    if (iNum > 0)
                    //    {
                    //        //点亮标签
                    //        RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(sTag_Ip, sTag_Id, iNum, 1, 1);
                    //        dy1.iStatus = 1;//被拍灭过，不能扫描下一个物料
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
            }
        }
        /// <summary>
        /// 说明：处理从扫描枪扫描返回的数据
        /// </summary>
        private void DosKeyValueFromScanner(string sIp, string sTagId, string sKeyCode, string sLocator, string sValue)
        {
            try
            {
                //处理扫描枪返回的数据
                sValue = sValue.TrimEnd(new char[] { '\r', '\n', '\0' });
                LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, string.Format("扫描到条码数据为:[{0}]", sValue));
                DisplayTxtBoxScanBarcode(txtBoxScanBarcode, sValue);//显示数据到文本框中
                displayLED(sIp, sTagId, sValue);
                //判断条码是波次条码、商品条码、挂起条码
                if (sValue.StartsWith(sWaveCodeHead)) //波次条码  箱号HKZ
                {
                    TodoWaveBarcode(sIp, sTagId, sKeyCode, sLocator, sValue);
                }
                else if (sValue == sPendingBarcode) //挂起条码
                {
                    TodoPendingBarcode(sIp, sTagId, sKeyCode, sLocator, sValue);
                }
                else  //商品条码
                {
                    TodoItemBarcode(sIp, sTagId, sKeyCode, sLocator, sValue);
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
            }
        }
        private void displayLED(string Ip, string comId, string sMsg)
        {
            try
            {
                string sNo = comId.Substring(comId.Length - 1, 1);
                string sLedId = string.Format("FD0{0}", sNo);
                RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(Ip, sLedId, sMsg, 0);
                RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(Ip, sLedId, sMsg, 1);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 说明：防止跨线程操作，直接赋值给TextBox文本框
        /// </summary>
        /// <param name="s"></param>
        private delegate void delegateDisplayTxtBoxScanBarcode(DevComponents.DotNetBar.Controls.TextBoxX tx, string s);
        private void DisplayTxtBoxScanBarcode(DevComponents.DotNetBar.Controls.TextBoxX tx, string s)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new delegateDisplayTxtBoxScanBarcode(DisplayTxtBoxScanBarcode), new object[] { tx, s });
            }
            else
            {
                tx.Text = s;
            }
        }
        /// <summary>
        /// 说明：判断当前分区是否可以扫描下一个波次的订单
        /// </summary>
        /// <param name="sComId"></param>
        /// <param name="sScanValue"></param>
        private bool CheckRegionBusyOrFree(string sComId)
        {
            //判断当前工位是否有播种任务
            if (listDataModel.Count == 0)
            {
                return true;
            }
            else
            {
                DataModelDAS.DASLayer1 dy1 = listDataModel.Find(x => x.Com_id == sComId);
                if (dy1 != null)
                {
                    structAisleLamp sp = new structAisleLamp();
                    sp.sAisleLampIP = dy1.aisle_lamp_id_ip;
                    sp.sAisleId = dy1.aisle_lamp_id;
                    System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LightAisleRed));
                    th.Start(sp);
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 说明：处理是波次条码的情况
        /// </summary>
        /// <param name="sIp"></param>
        /// <param name="sTagId"></param>
        /// <param name="sKeyCode"></param>
        /// <param name="sLocator"></param>
        /// <param name="sValue"></param>
        private void TodoWaveBarcode(string sIp, string sTagId, string sKeyCode, string sLocator, string sValue)
        {
            try
            {
                if (!CheckRegionBusyOrFree(sTagId)) //当前工位已经存在正在播种的波次订单
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, "当前分区正在分拣中,不允许扫描新的箱号");
                    AlarmInformationMsg(sIp, sTagId, sKeyCode, sLocator, sValue, "当前分区正在分拣中,不允许扫描新的箱号", "上一个波次还未完成!");
                    return;
                }
                else
                {
                    DataSet ds = new DataSet();
                    string sMsg = string.Empty;
                    string sSql = string.Format(sQueryDataByPickOrder, sValue);
                    ds = myShd.ExecuteSql(sSql, out sMsg);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        AlarmInformationMsg(sIp, sTagId, sKeyCode, sLocator, sValue, "根据波次在系统中没有查询到数据", string.Format("Not find data.\r\n by waveNo :{0}", sValue));
                        return;
                    }
                    else
                    {
                        //RYB_PTL_API.RYB_PTL.RYB_PTL_CloseDigit5(sIp, "AAAA");
                        int iRowCount = ds.Tables[0].Rows.Count;
                        LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, string.Format("获取到分播总条数为:{0}", iRowCount));
                        dbDAS.Merge(ds.Tables[0], false);//合并追加数据
                        dbDAS.AcceptChanges();
                        DisplayDataGridView(dbDAS);//显示到DataGridview中
                        InsertListByDataTable(ds.Tables[0], iRowCount);//把数据加入到集合中
                        //更新波次开始作业的时间
                        try
                        {
                            string sUpdateWaveNo = string.Format("update t_das_head set create_date = getdate(),flag_Head = 1 where wave_no = '{0}'", sValue); //拣料中
                            myShd.ExecuteSql(sUpdateWaveNo, out sMsg);
                            if (sMsg.Trim().Length != 0)
                            {
                                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("更新波次开始扫描时间失败:{0}", sMsg));
                            }
                        }
                        catch (Exception ex)
                        {
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
            }
        }

        /// <summary>
        /// 说明：根据箱号、区域编号,获取波次号
        /// </summary>
        /// <param name="sBoxNo"></param>
        /// <param name="sRegionNo"></param>
        /// <param name="sWaveNo"></param>
        /// <returns></returns>
        private bool GetWaveNoByBoxNo(string sBoxNo,string sRegionNo,out string sWaveNo)
        {
            sWaveNo = string.Empty;
            try
            {
                string sGetWaveNo = string.Format("select top 1 h.wave_no from t_das_head h, t_das_line l where h.head_id = l.head_id and l.box_no = '{0}' and h.region_no = '{1}' and flag_head in (0,1) order by h.create_date asc", sBoxNo, sRegionNo);
                DataSet dsGetWaveNo = myShd.ExecuteSql(sGetWaveNo);
                if (dsGetWaveNo.Tables.Count == 0 || dsGetWaveNo.Tables[0].Rows.Count == 0)
                {
                    //没有查询到数据
                    return true;
                }
                else
                {
                    sWaveNo = dsGetWaveNo.Tables[0].Rows[0]["wave_no"].ToString();//获取到的波次号
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
                return false;
            }
        }
        struct structAisleLamp
        {
            public string sAisleLampIP { get; set; }
            public string sAisleId { get; set; }
            public string sOrderId { get; set; }
            public string sOrderIp { get; set; }
            public string sWaveNo { get; set; } //记录老的波次号
            public string sErrMsg { get; set; }
        }
        /// <summary>
        /// 把巷道灯红灯点亮3秒并点亮订单显示器提醒
        /// </summary>
        /// <param name="sAisleLampIP"></param>
        /// <param name="sAisleId"></param>
        private void LightAisleRedAndShowOrderLEDMsg(object v)
        {
            try
            {
                structAisleLamp sp = (structAisleLamp)v;
                string sAisleLampIP = sp.sAisleLampIP;
                string sAisleId = sp.sAisleId;
                string sOrderIp = sp.sOrderIp;
                string sOrderId = sp.sOrderId;
                string sMsg = sp.sErrMsg;
                bool isShowOrderLED = false;
                bool isShowAisle = false;
                if (sOrderIp.Trim().Length > 0 && sOrderId.Trim().Length > 0)
                {
                    RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sOrderIp, sOrderId, sMsg, 0);
                    RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sOrderIp, sOrderId, sMsg, 1);
                    isShowOrderLED = true;
                }
                if (sAisleLampIP.Trim().Length > 0 && sAisleId.Trim().Length > 0)
                {
                    RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAisleLampIP, sAisleId, 6, 1);
                    isShowAisle = true;
                }
                System.Threading.Thread.Sleep(5000);
                if (isShowAisle)
                {
                    RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAisleLampIP, sAisleId, 6, 0);
                }
                if (isShowOrderLED)
                {
                    if (sp.sWaveNo == null ||sp.sWaveNo.Trim().Length == 0)
                    {
                        RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sOrderIp, sOrderId, "----------", 0);
                        RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sOrderIp, sOrderId, "----------", 1);
                    }
                    else
                    {
                        RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sOrderIp, sOrderId, sp.sWaveNo.PadLeft(11, '0').Substring(1, 10), 1);
                    }
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
            }
        }

        /// <summary>
        /// 说明：处理是挂起条码的情况
        /// </summary>
        /// <param name="sIp"></param>
        /// <param name="sTagId"></param>
        /// <param name="sKeyCode"></param>
        /// <param name="sLocator"></param>
        /// <param name="sValue"></param>
        private void TodoPendingBarcode(string sIp, string sTagId, string sKeyCode, string sLocator, string sValue)
        {
            try
            {
                //if (listDataModel.Count == 0)
                //{
                //AlarmInformationMsg(sIp, sTagId, sKeyCode, sLocator, sValue, "没有任务可以执行挂起", "No List");
                string sOrderIp = string.Empty;
                string sOrderId = string.Empty;
                string sAisleIp = string.Empty;
                string sAisleId = string.Empty;
                //把前后的标签也灭一遍
                GetBaseInforFromDataBase(sTagId, string.Empty, string.Empty, out sOrderIp, out sOrderId, out sAisleIp, out sAisleId);
                DataRow[] drs = dsBase.Tables[0].Select(string.Format("com_id = '{0}' and com_id_ip = '{1}'", sTagId, sIp));
                foreach (DataRow dr in drs)
                {
                    string sTagFrontId = dr["tag_id"].ToString();
                    string sTagIp = dr["tag_id_ip"].ToString();
                    string sTagBackId = "5" + sTagFrontId.PadLeft(4, '0').Substring(1, 3);
                    //熄灭前后的标签
                    RYB_PTL_API.RYB_PTL.RYB_PTL_CloseDigit5(sTagIp, sTagFrontId);//货位上的标签熄灭
                    Thread.Sleep(100);
                    RYB_PTL_API.RYB_PTL.RYB_PTL_CloseDigit5(sTagIp, sTagBackId); //背后的标签熄灭
                }
                //熄灭巷道灯
                RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAisleIp, sAisleId, 6, 0);
                RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAisleIp, sAisleId, 7, 0);

                //熄灭订单显示器
                RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sOrderIp, sOrderId, "--", 0);
                RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sOrderIp, sOrderId, "挂起成功，请再扫描箱号继续播种", 1);

                DataModelDAS.DASLayer1 dy1 = this.listDataModel.Find(x => x.Com_id == sTagId);
                if (dy1 == null)
                {
                    return;
                }
                else
                {
                    //先熄灭订到灯
                    string sAilseIp = dy1.aisle_lamp_id_ip;
                    string sAileId = dy1.aisle_lamp_id;
                    RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAilseIp, sAileId, 6, 0);
                    RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAilseIp, sAileId, 7, 0);

                    //熄灭订单显示器
                    sOrderIp = dy1.order_id_ip;
                    sOrderId = dy1.order_id;
                    RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sOrderIp, sOrderId, "--", 0);
                    RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sOrderIp, sOrderId, "挂起成功，请再扫描箱号继续播种", 1);

                    //熄灭汇总标签
                    string sTagIpTotal = dy1.sTagIpTotal;
                    string sTagIdTotal = dy1.sTagIdTotal;
                    RYB_PTL_API.RYB_PTL.RYB_PTL_CloseDigit5(sTagIpTotal, sTagIdTotal);//汇总的标签熄灭

                    //熄灭标签
                    foreach (DataModelDAS.DASLayer2 dy2 in dy1.DAS2)
                    {
                        string sTagIpFront = dy2.tag_id_ip;
                        string sTagIdFront = dy2.tag_id;
                        string sTadIpBack = dy2.tag_back_ip;
                        string sTagIdBack = dy2.tag_back_id;
                        RYB_PTL_API.RYB_PTL.RYB_PTL_CloseDigit5(sTagIpFront, sTagIdFront);//货位上的标签熄灭
                        RYB_PTL_API.RYB_PTL.RYB_PTL_CloseDigit5(sTadIpBack, sTagIdBack); //背后的标签熄灭
                        foreach (DataModelDAS.DASLayer3 dy3 in dy2.DAS3)
                        {
                            RemoveListFromDataGridViewXDetailId(dy3.detail_id);
                        }
                    }
                    //把异常标签也挂起
                    int i = dy1.ListlarmTag.Count;
                    if (i > 0)
                    {
                        foreach (string sTagAlarm in dy1.ListlarmTag)
                        {
                            RYB_PTL_API.RYB_PTL.RYB_PTL_CloseDigit5(dy1.aisle_lamp_id_ip, sTagAlarm);
                        }
                    }
                    listDataModel.Remove(dy1);
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, "当前分区分播任务，已经挂起成功");
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
            }
        }

        /// <summary>
        /// 说明：扫描的是商品条码的情况
        /// </summary>
        /// <param name="sIp"></param>
        /// <param name="sTagId"></param>
        /// <param name="sKeyCode"></param>
        /// <param name="sLocator"></param>
        /// <param name="sValue"></param>
        private void TodoItemBarcode(string sIp, string sTagId, string sKeyCode, string sLocator, string sValue)
        {
            try
            {
                /* 1.根据扫描的条码，从集合中查找
                 * 2.根据是B2B还是B2C进行亮灯
                 */
                if (listDataModel.Count == 0)
                {
                    AlarmInformationMsg(sIp, sTagId, sKeyCode, sLocator, sValue,"请先扫描波次条码,再进行分播区的绑定.","请扫描波次条码");
                }
                else
                {
                    DataModelDAS.DASLayer1 dy1 = listDataModel.Find(x => x.Com_Ip == sIp && x.Com_id == sTagId);
                    if (dy1 == null)
                    {
                        AlarmInformationMsg(sIp, sTagId, sKeyCode, sLocator, sValue, "需要先扫描箱子", "需要先扫描箱子！");
                        return;
                    }
                    //有异常的标签需要处理，才能扫描物料条码
                    if(dy1.ListlarmTag.Count >0)
                    {
                        AlarmInformationMsg( sIp,  sTagId,  sKeyCode,  sLocator,  sValue,"有异常的灯还没有处理完成，不能扫描物料条码","有异常灯亮起");
                        return;
                    }
                    //判断是否已经有亮的灯，如果有，则不能亮灯
                    DataModelDAS.DASLayer2 dy20 = dy1.DAS2.Find(x=>x.isLight2 ==1);
                    if (dy20 != null)
                    {
                        AlarmInformationMsg( sIp,  sTagId,  sKeyCode,  sLocator,  sValue,"已经有亮灯,不能扫描物料条码。需要处理完上一个灯，才能执行下一个灯","已经指示灯亮.");
                        return;
                    }
                    //开始亮灯
                    bool bExist = false;
                    foreach (DataModelDAS.DASLayer2 dy21 in dy1.DAS2)
                    {
                        //DataModelDAS.DASLayer3 dy3 = dy21.DAS3.Find(x => x.item_barcode == sValue); 20170603 修改
                        DataModelDAS.DASLayer3 dy3 = dy21.DAS3.Find(x => x.item_barcode.IndexOf(sValue)>=0);
                        if (bOperateModel) //逐件分播
                        {
                            if (dy1.iStatus != 1)
                            {
                                if (dy3 != null)
                                {
                                    if (dy3.require_qty - dy3.actual_qty > 0)
                                    {
                                        //点亮灯 标签前两位显示此商品在此格口待播种的数量，后三位显示当前需要放入的数量1
                                        //RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(dy21.tag_id_ip, dy21.tag_id, (dy3.require_qty - dy3.actual_qty).ToString(), 1, 1, 2);
                                        dy21.isLight2 = 1;//标识为已亮过
                                        dy3.isLight3 = 1;//标识为已被点过.
                                    }
                                    bExist = true;
                                    break;
                                }
                            }
                            else
                            {
                                AlarmInformationMsg(sIp, sTagId, sKeyCode, sLocator, sValue, "汇总标签已经被拍灭亮了格口的标签，需要挂起再操作。", "Error!");
                                return;
                            }
                        }
                        else //批量分播
                        {
                            if (dy3 != null)
                            {
                                if (dy3.require_qty - dy3.actual_qty > 0)
                                {
                                    RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(dy21.tag_id_ip, dy21.tag_id, dy3.require_qty - dy3.actual_qty, 1, 2);
                                    dy21.isLight2 = 1;//标识为已亮过
                                    dy3.isLight3 = 1;//标识为已被点过.
                                }
                                bExist = true;
                            }
                        }
                    }
                    //不存在此物料的分播情况
                    if (!bExist)
                    {
                        AlarmInformationMsg(sIp, sTagId, sKeyCode, sLocator, sValue, "此物料已经分播完成或不存在此物料","不存在此物料编码");
                    }
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
            }
        }

        /// <summary>
        /// 说明:当扫描为空时，进行报警提醒
        /// </summary>
        /// <param name="sIp"></param>
        /// <param name="sTagId"></param>
        /// <param name="sKeyCode"></param>
        /// <param name="sLocator"></param>
        /// <param name="sValue"></param>
        private void AlarmInformationMsg(string sIp, string sTagId, string sKeyCode, string sLocator, string sValue,string sErr,string sOrderLedMsg)
        {
            try
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, sErr);
                DataModelDAS.DASLayer1 dy1 = listDataModel.Find(x => x.Com_Ip == sIp && x.Com_id == sTagId);
                if (dy1 == null)
                {
                    //根据扫描枪的ID，找对应的订单显示器、巷道灯地址
                    string sOrderIp = string.Empty;
                    string sOrderId = string.Empty;
                    string sAisleIp = string.Empty;
                    string sAisleId = string.Empty;
                    GetBaseInforFromDataBase(sTagId,string.Empty, string.Empty, out sOrderIp, out sOrderId, out sAisleIp, out sAisleId);
                    structAisleLamp sap = new structAisleLamp();
                    sap.sAisleLampIP = sAisleIp;
                    sap.sAisleId = sAisleId;
                    sap.sOrderIp = sOrderIp;
                    sap.sOrderId = sOrderId;
                    sap.sErrMsg = sOrderLedMsg;//订单显示器上显示内容
                    System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LightAisleRedAndShowOrderLEDMsg));
                    th.Start(sap);
                }
                else
                {
                    structAisleLamp sap = new structAisleLamp();
                    sap.sAisleLampIP = dy1.aisle_lamp_id_ip;
                    sap.sAisleId = dy1.aisle_lamp_id;
                    sap.sOrderIp = dy1.order_id_ip;
                    sap.sOrderId = dy1.order_id;
                    sap.sWaveNo = dy1.wave_no;
                    sap.sErrMsg = sOrderLedMsg;//订单显示器上显示内容
                    System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LightAisleRedAndShowOrderLEDMsg));
                    th.Start(sap);
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
            }
        }


        private delegate void delegateRemoveListFromDataGridViewXDetailId(int iDetailID);
        object vdataGridViewX1 = new object();
        private void RemoveListFromDataGridViewXDetailId(int iDetailID)
        {
           
                //lock (vdataGridViewX1)
                //{
                //    try
                //    {
                //        if (this.InvokeRequired)
                //        {
                //            this.BeginInvoke(new delegateRemoveListFromDataGridViewXDetailId(RemoveListFromDataGridViewXDetailId), new object[] { iDetailID });
                //        }
                //        else
                //        {
                //            if (!checkBox1.Checked)
                //            {
                //                return;
                //            }
                //            try
                //            {
                //                //从列表中移除集合
                //                for (int i = 0; i < dataGridViewX1.Rows.Count; i++)
                //                {
                //                    int ld = Convert.ToInt32(dataGridViewX1.Rows[i].Cells["detail_id"].Value);
                //                    if (ld == iDetailID)
                //                    {
                //                        dataGridViewX1.Rows.RemoveAt(i);
                //                        dataGridViewX1.Refresh();
                //                        lblCount.Text = dataGridViewX1.Rows.Count.ToString();
                //                        break;
                //                    }
                //                }
                //            }
                //            catch (Exception ex)
                //            {
                //                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
                //            }
                //        }
                //    }
                //    catch
                //    {

                //    }
                //}
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
        /// 说明:把获取的数据清单放入到泛型集合中
        /// </summary>
        /// <param name="db"></param>
        private void InsertListByDataTable(DataTable db,int iRowTotal)
        {
            try
            {
                if (db == null || db.Rows.Count == 0)
                {
                    return;
                }
                //第1层处理数据
                DataView dvLayer1 = new DataView(db);
                DataTable dbLayer1 = dvLayer1.ToTable(true, new string[] { "head_id", "wave_no", "flag_head", "com_id_ip", "com_id", "aisle_lamp_id_ip", "aisle_lamp_id", "order_id_ip", "order_id","region_no"});//过滤掉重复的
                ArrayList alistLineId = new ArrayList();
                ArrayList alistDettailId = new ArrayList();
                foreach (DataRow dr1 in dbLayer1.Rows)
                {
                    string shead_id = dr1["head_id"].ToString();
                    string swave_no = dr1["wave_no"].ToString();
                    string sflag_head = dr1["flag_head"].ToString();
                    string scom_id_ip = dr1["com_id_ip"].ToString();
                    string scom_id = dr1["com_id"].ToString();
                    string saisle_lamp_id_ip = dr1["aisle_lamp_id_ip"].ToString();
                    string saisle_lamp_id = dr1["aisle_lamp_id"].ToString();
                    string sorder_id_ip = dr1["order_id_ip"].ToString();
                    string sorder_id = dr1["order_id"].ToString();
                    string sRegionNo = dr1["region_no"].ToString();//分区号
                    DataModelDAS.DASLayer1 dy1 = new DataModelDAS.DASLayer1();
                    dy1.head_id = Convert.ToInt32(shead_id);
                    dy1.wave_no = swave_no;
                    dy1.flag_head = Convert.ToInt32(sflag_head);
                    dy1.Com_Ip = scom_id_ip;
                    dy1.Com_id = scom_id;
                    dy1.aisle_lamp_id_ip = saisle_lamp_id_ip;
                    dy1.aisle_lamp_id = saisle_lamp_id;
                    dy1.order_id_ip = sorder_id_ip;
                    dy1.order_id = sorder_id;
                    dy1.sTagIpTotal = sorder_id_ip;//与订单显示器公用一个IP
                    dy1.sTagIdTotal = "1"+ sRegionNo.PadLeft(3, '0'); //汇总标签的地址式样1001,与区域编号相联系
                    dy1.TotalNum = 0; //记录需求总量
                    dy1.iStatus = 0;//没有被拍灭过

                    DataRow[] drs2 = db.Select(string.Format("head_id= '{0}'", shead_id));
                    dy1.DAS2 = new List<DataModelDAS.DASLayer2>();
                    dy1.ListlarmTag = new List<string>();
                    foreach (DataRow dr2 in drs2)
                    {
                        int iLineId = Convert.ToInt32(dr2["line_id"].ToString());
                        if (alistLineId.Contains(iLineId))
                        {
                            continue;
                        }
                        else
                        {
                            alistLineId.Add(iLineId);
                        }
                        //第2层
                        DataModelDAS.DASLayer2 dy2 = new DataModelDAS.DASLayer2();
                        dy2.line_id = Convert.ToInt32(iLineId);//line_id
                        dy2.order_no = dr2["order_no"].ToString();
                        dy2.locator = dr2["locator"].ToString();
                        dy2.back_locator = dr2["back_locator"].ToString();
                        dy2.flag_line = Convert.ToInt32(dr2["flag_line"].ToString());
                        dy2.tag_id_ip = dr2["tag_id_ip"].ToString();
                        dy2.tag_id = dr2["tag_id"].ToString();
                        dy2.tag_back_ip = dr2["tag_id_ip"].ToString();
                        dy2.tag_back_id = "5" + dr2["tag_id"].ToString().Substring(1, dr2["tag_id"].ToString().Length - 1);//背后的标签是前面的标签加5000，例如0001，背后为5001
                        dy2.isLight2 = 0;

                        //第3层
                        DataRow[] drs3 = db.Select(string.Format("head_id= '{0}' and line_id = '{1}'", shead_id, iLineId));
                        dy2.DAS3 = new List<DataModelDAS.DASLayer3>();
                        foreach (DataRow dr3 in drs3)
                        {
                            int iDetailId = Convert.ToInt32(dr3["detail_id"].ToString());
                            if (alistDettailId.Contains(iDetailId))
                            {
                                continue;
                            }
                            else
                            {
                                alistDettailId.Add(iDetailId);
                                DataModelDAS.DASLayer3 dy3 = new DataModelDAS.DASLayer3();
                                dy3.detail_id = iDetailId;
                                dy3.item_code = dr3["item_code"].ToString();
                                dy3.item_barcode = dr3["item_barcode"].ToString();
                                dy3.require_qty = Convert.ToInt32(dr3["require_qty"].ToString());
                                dy3.actual_qty = Convert.ToInt32(dr3["actual_qty"].ToString());
                                dy1.TotalNum = dy1.TotalNum + dy3.require_qty - dy3.actual_qty;//显示总件数
                                dy3.isLight3 = 0;
                                dy2.DAS3.Add(dy3);
                            }
                        }
                        //订单已经完成的，不需要再亮，直接亮背后的标签即可。
                        if (dy2.flag_line == 2)
                        {
                            //直接点亮背后的标签
                            RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(dy2.tag_back_ip, dy2.tag_back_id, 1, 1,5);
                            foreach (DataModelDAS.DASLayer3 dy23 in dy2.DAS3)
                            {
                                RemoveListFromDataGridViewXDetailId(dy23.detail_id);
                            }
                        }
                        else
                        {
                            int iRemanQty =0;
                            foreach (DataModelDAS.DASLayer3 dy300 in dy2.DAS3)
                            {
                                iRemanQty = iRemanQty + dy300.require_qty - dy300.actual_qty;
                            }
                            if (iRemanQty == 0)
                            {
                                //直接点亮背后的标签
                                RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(dy2.tag_back_ip, dy2.tag_back_id, 1, 1, 5);
                                foreach (DataModelDAS.DASLayer3 dy301 in dy2.DAS3)
                                {
                                    RemoveListFromDataGridViewXDetailId(dy301.detail_id);
                                }
                                //状态更改
                                string sdy2 = string.Format("update t_das_line set flag_line = 2 where line_id = {0}", dy2.line_id);
                                string sMsg = string.Empty;
                                myShd.ExecutCmd(sdy2, out sMsg);
                            }
                            else
                            {
                                dy1.DAS2.Add(dy2);
                            }
                        }
                        
                    }
                    //放入到集合中
                    listDataModel.Add(dy1);
                    //把订单显示器、巷道灯点亮
                    LightTagObj(dy1);
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
            }

        }
        /// <summary>
        /// 说明:把添加的新集合，把订单显示器显示波次号、巷道灯点亮
        /// </summary>
        /// <param name="dsplayer1"></param>
        private void LightTagObj(DataModelDAS.DASLayer1 dsplayer1)
        {
            lock (v5)
            {
                try
                {
                    //根据Flag_Line的状态，如果=2，代表背后的标签已经点亮过，需要再次亮起
                    List<DataModelDAS.DASLayer2> dy2s = dsplayer1.DAS2.FindAll(x => x.flag_line == 2);
                    if (dy2s != null)
                    {
                        foreach (DataModelDAS.DASLayer2 dy2 in dy2s)
                        {
                            //点亮背后的标签
                            RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(dy2.tag_back_ip, dy2.tag_back_id, 1, 1, 5);
                            dsplayer1.DAS2.Remove(dy2);
                        }
                    }
                    if (dsplayer1.DAS2.Count == 0)
                    {
                        listDataModel.Remove(dsplayer1);
                    }
                    string sOrderIp = dsplayer1.order_id_ip;
                    string sOrderId = dsplayer1.order_id;
                    string sAisleIp = dsplayer1.aisle_lamp_id_ip;
                    string sAisleId = dsplayer1.aisle_lamp_id;
                    string sWaveNo = dsplayer1.wave_no;
                    //点亮巷道灯
                    RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(sAisleIp, sAisleId, 7, 1);
                    //汇总标签显示待播种的数量
                    RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(dsplayer1.sTagIpTotal, dsplayer1.sTagIdTotal, dsplayer1.TotalNum, 1, 4);
                    //把波次信息显示在订单显示器上
                    sWaveNo = sWaveNo.PadLeft(11, '0').Substring(1, 10);
                    if (dsplayer1.TotalNum == 0)
                    {
                        sWaveNo = sWaveNo + "\r\n 已经完成!";
                    }
                    RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sOrderIp, sOrderId, "波次号:" + sWaveNo, 0);
                    RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(sOrderIp, sOrderId, "波次号:" + sWaveNo, 1);
                }
                catch (Exception ex)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
                }
            }
        }

        /// <summary>
        /// 说明:处理从标签返回后拍灭值(包括了感应灭灯)
        /// </summary>
        /// <param name="sIp"></param>
        /// <param name="sTagId"></param>
        /// <param name="sKeyCode"></param>
        /// <param name="sLocator"></param>
        /// <param name="sValue"></param>
        private void DoKeyValueFromDASTagGreen(string sIp, string sTagId, string sKeyCode, string sLocator, string sValue)
        {
            try
            {
                foreach (DataModelDAS.DASLayer1 dy1 in listDataModel)
                {
                    DataModelDAS.DASLayer2 dy2 = dy1.DAS2.Find(x => x.tag_id_ip == sIp && x.tag_id == sTagId && x.isLight2 == 1);
                    if (dy2 == null)
                    {
                        continue;
                    }
                    //更新对应编码的数量
                    DataModelDAS.DASLayer3 dy3 = dy2.DAS3.Find(x => x.isLight3 == 1);
                    if (dy3 != null)
                    {
                        if (dy1.ListlarmTag.Count > 0)
                        {
                            if (dy1.ListlarmTag.Contains(sTagId))
                            {
                                dy1.ListlarmTag.Remove(sTagId);
                            }
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("IP:{0},Id:{1}，不能触发此标签.需要先处理异常的标签.", sIp, sTagId));
                            //重新点亮此标签
                            //RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(sIp, sTagId, (dy3.require_qty-dy3.actual_qty).ToString(), Convert.ToInt32(sValue), 1, 2);
                            return;
                        }
                        dy3.actual_qty = dy3.actual_qty + Convert.ToInt32(sValue);//变更泛型集合中的数量
                        //更新后台表中的数量
                        string sMsg = string.Empty;
                        string sSql = string.Format("update t_das_detail set actual_qty = actual_qty + {0},last_update_date = getdate() where detail_id = {1}", sValue, dy3.detail_id);
                        bool bExecut = myShd.ExecutCmd(sSql, out sMsg);
                        if (bExecut)
                        {
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("IP:{0},Id:{1} 灭灯后，提交数据成功.", sIp, sTagId));
                            //汇总标签的数量递减，并显示在标签上
                            dy1.TotalNum = dy1.TotalNum - Convert.ToInt32(sValue);
                            RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(dy1.sTagIpTotal, dy1.sTagIdTotal, dy1.TotalNum, 1, 4);
                            dy3.isLight3 = 0;//把物料编码重置为未点亮状态
                            dy2.isLight2 = 0;//把对应的订单标签标识为已拍灭状态
                            if (dy3.require_qty == dy3.actual_qty) 
                            {
                                RemoveListFromDataGridViewXDetailId(dy3.detail_id);//从列表中移除记录行
                                dy2.DAS3.Remove(dy3);//从集成中移除第3层
                                if (dy2.DAS3.Count == 0) //对应此订单的物料已经播种完成，亮起背后的标签
                                {
                                    //dy1.DAS2.Remove(dy2);//把当前已经做过的记录行，比如同一个订单的一个箱子记录行，先移除掉。
                                    //这个地方还需要再判断一下2016-11-09 因为同一个波次、同一个客户、可能会对应多个箱子在行表中存在多行。
                                    List<DataModelDAS.DASLayer2> dy2Others = dy1.DAS2.FindAll(x => x.tag_id_ip == sIp && x.tag_id == sTagId);
                                    if (dy2Others != null && dy2Others.Count >=1)
                                    {
                                       //判断一下对应的第2层下的第3层，是否还有东西要分播
                                        bool bExsits = false;
                                        foreach (DataModelDAS.DASLayer2 ddd3 in dy2Others)
                                        {
                                            if (ddd3.DAS3.Count > 0)
                                            {
                                                bExsits = true;
                                                break;
                                            }
                                        }
                                        if (bExsits)
                                        {
                                            LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("标签：{0}  对应的订单行表有还有！", sTagId)); ;
                                            return;
                                        }
                                    }
                                    RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(dy2.tag_back_ip, dy2.tag_back_id, 1, 1, 5);
                                    //更新订单对应的状态为1
                                    string sSql1 = string.Format("update t_das_line set flag_line = 2 where line_id = {0}", dy2.line_id);
                                    string sMsg1 = string.Empty;
                                    bool bExecut1 = myShd.ExecutCmd(sSql1, out sMsg1);
                                    if (bExecut1)
                                    {
                                        LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("订单:{0},Line_Id:{1}更新状态为1，提交数据成功.",dy2.order_no,dy2.line_id));
                                    }
                                    //前后不同步，也就是所背后拍灯不影响前面的操作
                                    if (!bFrontAndBackSync)
                                    {
                                        dy1.DAS2.Remove(dy2);
                                        if (dy1.DAS2.Count == 0) //当前分区的波次订单已经全部分播完成
                                        {
                                            //灭巷道灯
                                            RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(dy1.aisle_lamp_id_ip, dy1.aisle_lamp_id, 7, 0);
                                            //灭订单显示器
                                            RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(dy1.order_id_ip, dy1.order_id, "此波次已分播完成！End", 0);
                                            RYB_PTL_API.RYB_PTL.RYB_PTL_DspOrderLED(dy1.order_id_ip, dy1.order_id, "此波次已分播完成！End", 1);

                                            //更新波次的状态
                                            string sSql2 = string.Format("update t_das_head set flag_head = 2,last_update_date = getdate() where head_id = {0}", dy1.head_id);
                                            string sMsg2 = string.Empty;
                                            bool bExecut2 = myShd.ExecutCmd(sSql2, out sMsg2);
                                            if (bExecut2)
                                            {
                                                //从大集合中移除第一层
                                                listDataModel.Remove(dy1);
                                                LogRichBox.LogRichBox.DoLogRichBox(Color.Green, FontStyle.Bold, 10, string.Format("波次号:{0},Head_Id:{1}更新状态为1，提交数据成功.", dy1.wave_no, dy1.head_id));
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                updateDataGridView(dy3.detail_id, Convert.ToInt32(sValue));//把感应标签或手工拍灭的标签的数量更新到列表中
                            }
                        }
                        else
                        {
                            LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("IP:{0},Id:{1} 灭灯后，提交数据失败.{0}", sIp, sTagId, sMsg));
                        }
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}6][{1}]", sErrCode, ex.Message));
            }
        }
        private delegate void delegateupdateDataGridView(int idetail_id, int iQty);
        private void updateDataGridView(int idetail_id,int iQty)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new delegateupdateDataGridView(updateDataGridView), new object[] { idetail_id, iQty });
            }
            else
            {
                try
                {
                    if (!checkBox1.Checked)
                    {
                        return;
                    }
                    for (int i = 0; i < dataGridViewX1.Rows.Count; i++)
                    {
                        int ld = Convert.ToInt32(dataGridViewX1.Rows[i].Cells["detail_id"].Value);
                        if (ld == idetail_id)
                        {
                            int iOldAutualQty = Convert.ToInt32(dataGridViewX1.Rows[i].Cells["actual_qty"].Value);
                            int iReqQty = Convert.ToInt32(dataGridViewX1.Rows[i].Cells["actual_qty"].Value);
                            dataGridViewX1.Rows[i].Cells["actual_qty"].Value = iOldAutualQty + iQty;
                            if (iReqQty == iOldAutualQty + iQty)
                            {
                                dataGridViewX1.Rows.RemoveAt(i);
                                dataGridViewX1.Refresh();
                                lblCount.Text = dataGridViewX1.Rows.Count.ToString();
                            }
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
                }
            }
        }

         /// <summary>
        ///  防止跨线程操作，把数据赋值给DataGridView
       /// </summary>
       /// <param name="db"></param>
        private delegate void delegateDisplayDataGridView(DataTable db);
        object vDisplayDataGridView = new object();
        private void DisplayDataGridView(DataTable db)
        {
            lock (vDisplayDataGridView)
            {
                try
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new delegateDisplayDataGridView(DisplayDataGridView), new object[] { db });
                    }
                    else
                    {
                        if (!checkBox1.Checked)
                        {
                            return;
                        }
                        this.dataGridViewX1.DataSource = null;
                        this.dataGridViewX1.DataSource = db;
                        this.lblCount.Text = this.dataGridViewX1.Rows.Count.ToString();
                    }
                }
                catch
                {

                }
            }
        }
        private void btnReturn_Click(object sender, EventArgs e)
        {
            RYB_PTL_API.RYB_PTL.UserResultAvailable -= new RYB_PTL_API.RYB_PTL.UserResultAvailableEventHandler(ptl_UserResultAvailable);
            System.Threading.Thread.Sleep(50);
            this.Close();
            this.Dispose();
        }
        private void dataGridViewX1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //int rownum = (e.RowIndex + 1);
            //System.Drawing.Rectangle rct = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y + 4, ((DataGridView)sender).RowHeadersWidth - 4, e.RowBounds.Height);
            //TextRenderer.DrawText(e.Graphics, rownum.ToString(), ((DataGridView)sender).RowHeadersDefaultCellStyle.Font, rct, ((DataGridView)sender).RowHeadersDefaultCellStyle.ForeColor, System.Drawing.Color.Transparent, TextFormatFlags.HorizontalCenter);
        }

        private void FrmDPS_Load(object sender, EventArgs e)
        {
            //加载已配置的文件
            LoadSetting();

            //加载SQL语句
            if (cfgsql["sql"].Trim().Length == 0)
            {
                cfgsql["sql"] = base64.StringToBase64string(sQueryDataByPickOrder);
            }
            else
            {
                sQueryDataByPickOrder = base64.Base64StringTostring(cfgsql["sql"]);
            }
        }
        private void LoadSetting()
        {
            //作业模式
            if (mycfg["DASOperateModel"] == "One")
            {
                bOperateModel = true;//逐件分播
                lblTaskType.Text = "逐件分播(B2C)";

            }
            else
            {
                bOperateModel = false;//批量分播
                lblTaskType.Text = "批量分播(B2B)";

            }

            //灭灯模式
            if (mycfg["DASLight"] == "Sensor")
            {
                bLightModel = true;
                lblLightType.Text = "感应式灭灯";
                OpenOrShutDownTagSensor(1);
            }
            else
            {
                bLightModel = false;
                lblLightType.Text = "手工拍灭";
                OpenOrShutDownTagSensor(0);
            }
            //挂起任务
            if (mycfg["DASPending"].Trim().Length == 0)
            {
                sPendingBarcode = string.Empty;
            }
            else
            {
                sPendingBarcode = mycfg["DASPending"];
             
            }
            sWaveCodeHead = mycfg["DASWaveHead"];
            if (mycfg["DASWait"] == "Y")
            {
                bFrontAndBackSync = true;
                lblFrontBack.Text = "是";
            }
            else
            {
                bFrontAndBackSync = false;
                lblFrontBack.Text = "否";
            }
            if (mycfg["Download"].Trim().Length == 0)
            {
                iDownload = 10;//单位秒
                mycfg["Download"] = "10";
            }
            else
            {
                iDownload = Convert.ToInt32(mycfg["Download"]);//单位秒
            }
            if (mycfg["iUpload"].Trim().Length == 0)
            {
                iUpload = 40;//单位秒
                mycfg["iUpload"] = "40";
            }
            else
            {
                iUpload = Convert.ToInt32(mycfg["iUpload"]);//单位秒
            }
        }
        /// <summary>
        /// 说明：是否开光栅
        /// </summary>
        /// <param name="iFlag">0是关闭 1是开启</param>
        private void OpenOrShutDownTagSensor(int iFlag)
        {
            //try
            //{
            //    string sSql = @"select distinct r.tag_id_ip, r.tag_id from t_tag_locator r";
            //    string sMsg = string.Empty;
            //    DataSet dsCtrl = myShd.ExecuteSql(sSql, out sMsg);
            //    if (dsCtrl == null || dsCtrl.Tables.Count == 0 || dsCtrl.Tables[0].Rows.Count == 0)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        foreach (DataRow dr in dsCtrl.Tables[0].Rows)
            //        {
            //            string ctrIp = dr["tag_id_ip"].ToString();
            //            string TagId = dr["tag_id"].ToString();

            //            RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5Sensor(ctrIp, TagId, iFlag);
            //        }
            //        string sg = "";
            //        if (iFlag == 0)
            //        {
            //            sg = "关闭光栅成功！";
            //        }
            //        else
            //        {
            //            sg = "开启光栅成功！";
            //        }
            //        LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, sg);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("错误代码:[{0}{1}][{2}]", sErrCode, new StackTrace(new StackFrame(true)).GetFrame(0).GetFileLineNumber(), ex.Message));
            //}
        }
        string sQueryDataByPickOrder = @"select tr.region_no,
                                           t1.wave_no,
                                           t1.order_no,
                                           tr.tag_id,
                                           tr.tag_id_ip,
                                           t1.item_code,
                                           t1.require_qty,
                                           t1.actual_qty,
                                           t1.item_barcode,
                                           t1.head_id,
                                           t1.line_id,
                                           t1.detail_id,
                                           t1.flag_head,
                                           t1.locator,
                                           t1.back_locator,
                                           t1.flag_line,
                                           tr.com_id_ip,
                                           tr.com_id,
                                           tr.order_id_ip,
                                           tr.order_id,
                                           tr.aisle_lamp_id_ip,
                                           tr.aisle_lamp_id
                                      from t_tag_locator tr,
                                           (select h.head_id,
                                                   l.line_id,
                                                   d.detail_id,
                                                   h.wave_no,
                                                   h.flag_head,
                                                   l.order_no,
                                                   l.locator,
                                                   l.back_locator,
                                                   l.flag_line,
                                                   d.item_code,
                                                   d.item_barcode,
                                                   d.require_qty,
                                                   d.actual_qty
                                              from t_das_head h, t_das_line l, t_das_detail d
                                             where h.head_id = l.head_id
                                               and l.line_id = d.line_id
                                               and h.wave_no = '{0}') t1
                                     where tr.locator = t1.locator order by locator";
        private void dataGridViewX1_RowPostPaint_1(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //int rownum = (e.RowIndex + 1);
            //System.Drawing.Rectangle rct = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y + 4, ((DataGridView)sender).RowHeadersWidth - 4, e.RowBounds.Height);
            //TextRenderer.DrawText(e.Graphics, rownum.ToString(), ((DataGridView)sender).RowHeadersDefaultCellStyle.Font, rct, ((DataGridView)sender).RowHeadersDefaultCellStyle.ForeColor, System.Drawing.Color.Transparent, TextFormatFlags.HorizontalCenter);
        }



        private void DFrmDAS_FormClosing(object sender, FormClosingEventArgs e)
        {
            RYB_PTL_API.RYB_PTL.UserResultAvailable -= new RYB_PTL_API.RYB_PTL.UserResultAvailableEventHandler(ptl_UserResultAvailable);
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[FrmDAS.dll][已经关闭!]");
        }
    }
}
