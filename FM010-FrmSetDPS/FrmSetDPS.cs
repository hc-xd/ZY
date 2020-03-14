using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Threading;

namespace NFrmSetDPS
{
    public partial class DFrmSetDPS : Form
    {
        private const string sErrCode = "错误代码:FM010";
        //用户名
        string mysUser;
        //访问通用配置文件
        dllConfigApp.ConfigApp mycfg;
        //数据查询对象
        DAL.SQLHelpDataBase myShd;
        //数据库访问对象
        BLL.CreatBaseTable cltBase = new BLL.CreatBaseTable();
        //操作日志的保存
        BLL.CreatLogTable cltlog = new BLL.CreatLogTable();
        dllConfigApp.ConfigApp cfgsql = new dllConfigApp.ConfigApp("SQL_DPS_GetByOrderNo.xml");
        dllConvertBase64.ConvertBase64 base64 = new dllConvertBase64.ConvertBase64();
        string MysqlConnectStr = string.Empty;
        List<layer1> listLayer1 = new List<layer1>();
        const string sKeyValueFromTag_GREEN = "12";
        object v1 = new object();
        object v2 = new object();
        object v3 = new object();
        object v4 = new object();
        object v5 = new object();
        object v6 = new object();
        public DFrmSetDPS(string sUser, dllConfigApp.ConfigApp cfg, DAL.SQLHelpDataBase shd)
        {
            InitializeComponent();
            mysUser = sUser;
            mycfg = cfg;
            myShd = shd;
            RYB_PTL_API.RYB_PTL.UserResultAvailable += new RYB_PTL_API.RYB_PTL.UserResultAvailableEventHandler(ptl_UserResultAvailable);
        }
        private void ptl_UserResultAvailable(RYB_PTL_API.RYB_PTL.RtnValueStruct rs)
        {
            string sIp = rs.Ip;
            string sTag = rs.Tagid;
            string sKeyCode = rs.KeyCode.ToUpper();
            string sLocator = rs.Locator;
            string sValue = rs.Number;
            HandleDataFromScanner(sIp, sTag, sKeyCode, sLocator, sValue);
        }
        private void HandleDataFromScanner(string sIp, string sTagId, string sKeyCode, string sLocator, string sNumber)
        {
            switch (sKeyCode.ToUpper())
            {               
                case sKeyValueFromTag_GREEN:
                    {
                        lock (v1)
                        {
                            DosKeyValueFromTag(sIp, sTagId, sKeyCode, sLocator, sNumber);
                        }
                        break;
                    }                
                default:
                    break;
            }
        }
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
               
                LogRichBox.LogRichBox.DoLogRichBox(Color.Blue, FontStyle.Bold, 10, string.Format(string.Format("标签拍灭.:[IP:{0}][Id:{1}][数量:{2}]", sIp, sTagId, sValue)));
                foreach (layer1 dy1 in listLayer1.ToArray())
                {
                    layer2 dy2 = dy1.listLayer2.Find(x=>x.tagId==sTagId && x.tagIp==sIp);
                    if (dy2 != null)
                    {
                        dy1.listLayer2.Remove(dy2);
                        UpdateDataGridView(dy2.locator, dy2.itemCode);
                        if (dy1.listLayer2.Count == 0)
                        {
                            listLayer1.Remove(dy1);                            
                            RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(dy1.aisle_lamp_id_ip,dy1.aisle_lamp_id,6,0);
                            RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(dy1.aisle_lamp_id_ip, dy1.aisle_lamp_id, 5, 0);
                            RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(dy1.aisle_lamp_id_ip, dy1.aisle_lamp_id, 7, 0);
                        }
                    }                                     
                }
            }
            catch (Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Bold, 10, string.Format("异常:[{0}6][{1}]", sErrCode, ex.Message));
            }
        }
        private void btnOperationModel_Click(object sender, EventArgs e)
        {

        }

        private void btnLightModel_Click(object sender, EventArgs e)
        {

        }






        private void btnReturn_Click(object sender, EventArgs e)
        {
            RYB_PTL_API.RYB_PTL.UserResultAvailable -= new RYB_PTL_API.RYB_PTL.UserResultAvailableEventHandler(ptl_UserResultAvailable);
            LogRichBox.LogRichBox.DoLogRichBox(Color.Black, FontStyle.Bold, 10, "[FrmSetDPS.dll][已经关闭!]");
            this.Close();
            this.Dispose();
        }
        public class layer1
        {

            public string aisle_lamp_id { get; set; }
            public string aisle_lamp_id_ip { get; set; }
            public List<layer2> listLayer2 { get; set; }
        }
        public class layer2
        {
            public string itemCode { get; set; }
            public string locator { get; set; }
            public string tagId { get; set; }
            public string tagIp { get; set; }
            public int isLight { get; set; }
        }
        public void LightTag(layer2 layer2)
        {
            try
            {
                if (layer2 != null)
                {
                    RYB_PTL_API.RYB_PTL.RYB_PTL_DspDigit5(layer2.tagIp, layer2.tagId, 1, 1, 2);
                    layer2.isLight = 1;
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"异常："+ex.ToString());
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
            string sMsg = string.Empty;
            try
            {
                string str = string.Format("select distinct tag_id_ip from t_tag_locator");
                DataSet dt = ExcuteMySql(str,out sMsg);
                if (sMsg != "")
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, " 异常：" + sMsg);
                }
                else
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Tables[0].Rows)
                        {
                            string ip = dr["tag_id_ip"].ToString();
                            RYB_PTL_API.RYB_PTL.RYB_PTL_CloseDigit5(ip, "AAAA");//灭标签
                            RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(ip, "AAAB", 5, 0);
                            RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(ip, "AAAB", 6, 0);
                            RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(ip, "AAAB", 7, 0);
                        }
                    }
                }
             }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "OffTag 异常：" + ex.ToString());
            }
        }
        private void FrmSetDPS_Load(object sender, EventArgs e)
        {
            MysqlConnectStr = mycfg["ConnectionStr"];
            Thread threadOffTag = new Thread(OffTag);
            threadOffTag.IsBackground = true;
            threadOffTag.Name = "OffTag";
            threadOffTag.Start();
            //加载基本的设置参数
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
        public delegate void UpdateDataGridViewEventHandle1(layer2 dy2);
        private void InsertIntoDataGridView(layer2 dy2)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new UpdateDataGridViewEventHandle1(InsertIntoDataGridView), new object[] { dy2 });
                return;
            }
            try
            {
                dataGridView1.Rows.Add(1);
                int count = dataGridView1.Rows.Count;
                dataGridView1.Rows[count - 1].Cells[0].Value = dy2.itemCode;
                dataGridView1.Rows[count - 1].Cells[1].Value = dy2.locator;
                dataGridView1.Rows[count - 1].Cells[2].Value = dy2.tagId;
                dataGridView1.Rows[count - 1].Cells[3].Value = dy2.tagIp;
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, " InsertIntoDataGridView异常：" + ex.ToString());
            }
        }
        public delegate void UpdateDataGridViewEventHandle(string locator,string itemCode);
        private void UpdateDataGridView(string locator,string itemCode)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new UpdateDataGridViewEventHandle(UpdateDataGridView), new object[] { locator, itemCode });
                return;
            }
            try
            {
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    if (dr.Cells[0].Value.ToString() == itemCode && dr.Cells[1].Value.ToString() == locator)
                    {
                        dataGridView1.Rows.Remove(dr);
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "UpdateDataGridView 异常：" + ex.ToString());
            }
        }
        private void InsertIntoDatagridViewAndLightTag(DataTable dt)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string locator = dr["locator"].ToString();
                    string itemCode = dr["ItemCode"].ToString();
                    string tagId = dr["tag_id"].ToString();
                    string tagIp = dr["tag_id_ip"].ToString();
                    string aisle_lamp_id = dr["aisle_lamp_id"].ToString();
                    string aisle_lamp_id_ip = dr["aisle_lamp_id_ip"].ToString();
                    layer1 dy1 = listLayer1.Find(x=>x.aisle_lamp_id==aisle_lamp_id && x.aisle_lamp_id_ip==aisle_lamp_id_ip);
                    if (dy1 != null)
                    {
                        layer2 dy2 = dy1.listLayer2.Find(x=>x.locator==locator && x.tagId==tagId && x.tagIp==tagIp);
                        if (dy2 != null)
                        {
                            MessageBox.Show("请先处理上一个同位置的上架任务");
                            return;
                        }
                        else
                        {
                            layer2 dy2_1 = new layer2();
                            dy2_1.tagId = tagId;
                            dy2_1.tagIp = tagIp;
                            dy2_1.locator = locator;
                            dy2_1.itemCode = itemCode;
                            dy1.listLayer2.Add(dy2_1);
                            LightTag(dy2_1);
                            InsertIntoDataGridView(dy2_1);
                        }
                    }
                    else
                    {
                        layer1 dy1_1 = new layer1();
                        dy1_1.aisle_lamp_id = aisle_lamp_id;
                        dy1_1.aisle_lamp_id_ip = aisle_lamp_id_ip;
                        dy1_1.listLayer2 = new List<layer2>();
                        layer2 dy2 = new layer2();
                        dy2.tagId = tagId;
                        dy2.tagIp = tagIp;
                        dy2.locator = locator;
                        dy2.itemCode = itemCode;
                        dy1_1.listLayer2.Add(dy2);
                        listLayer1.Add(dy1_1);
                        LightTag(dy2);
                        InsertIntoDataGridView(dy2);                       
                        RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(dy1_1.aisle_lamp_id_ip, dy1_1.aisle_lamp_id, 7, 0);
                        RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(dy1_1.aisle_lamp_id_ip, dy1_1.aisle_lamp_id, 5, 0);
                        RYB_PTL_API.RYB_PTL.RYB_PTL_AisleLamp(dy1_1.aisle_lamp_id_ip, dy1_1.aisle_lamp_id, 6, 1);
                    }                                       
                }
            }
            catch(Exception ex)
            {
                LogRichBox.LogRichBox.DoLogRichBox(Color.Red, FontStyle.Regular, 12, "InsertIntoDatagridViewAndLightTag 异常：" + ex.ToString());
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            string errorMsg = string.Empty;
            if (textBox1.Text == "")
            {
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                string str = string.Format(@"select s.ItemCode,
                                                    s.ItemName,
                                                    l.aisle_lamp_id,
                                                    l.aisle_lamp_id_ip,
                                                    l.locator,
                                                    l.tag_id,
                                                    l.tag_id_ip 
                                            from t_stock s,t_tag_locator l
                                            where s.ItemCode='{0}' and l.locator=s.Locator and l.region_no='0'",textBox1.Text.Trim().ToString());
                DataSet dt = ExcuteMySql(str,out errorMsg);
                if (errorMsg != "")
                {
                    LogRichBox.LogRichBox.DoLogRichBox(Color.Red,FontStyle.Regular,12,"数据查询 异常："+errorMsg);
                }
                else
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        InsertIntoDatagridViewAndLightTag(dt.Tables[0]);
                    }
                }
            }
        }

        
    }
}

