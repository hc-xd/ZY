using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NFrmDAS
{
    public class FM013_Interface
    {
        //数据查询对象
        DAL.SQLHelpDataBase myShd;

        public FM013_Interface(DAL.SQLHelpDataBase shd)
        {
            myShd = shd;
        }
        /// <summary>
        ///  说明：根据波次号获取头表的Id
        /// </summary>
        /// <param name="sWave_No">波次号</param>
        /// <param name="sRegion_No">播种墙区域号</param>
        /// <returns></returns>
        public int GetHeadId(string sWave_No)
        {
            string sSql = string.Format("select head_id from  t_das_head h where h.WAVE_NO = '{0}'", sWave_No);
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
        ///  说明：把数据保存到DAS的头表中
        /// </summary>
        /// <param name="sWAVE_NO">波次号</param>
        /// <param name="sREGION_NO">播种墙的编号</param>
        /// <param name="sg">返回的信息</param>
        /// <returns></returns>
        public bool InsertDASHead(string sWAVE_NO, string sREGION_NO, out string sg)
        {
            //同一个波次，状态为0时，可能要重新下载(订单更改的情况)
            string sSqlDelete = string.Format("delete t_das_head where wave_no = '{0}' and flag_head = 0", sWAVE_NO);
            myShd.ExecutCmd(sSqlDelete);
            //执行插入
            string sSql = string.Format("insert into t_das_head (wave_no,flag_head, region_no) values ('{0}',0, {1})", sWAVE_NO, sREGION_NO);
            return myShd.ExecutCmd(sSql, out sg);
        }
        /// <summary>
        /// 说明：获取任务单行表的ID
        /// </summary>
        /// <param name="sORDER_NO">订单号</param>
        /// <param name="sLOCATOR">货位</param>
        /// <param name="iHeadId">头表的ID</param>
        /// <param name="sBoxNo">箱号</param>
        /// <returns>是否成功</returns>
        public int GetLineId(string sORDER_NO, string sLOCATOR, int iHeadId, string sBoxNo)
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
        /// <summary>
        /// 说明：插入记录到行表中
        /// </summary>
        /// <param name="ihead_id">传入的头表ID号</param>
        /// <param name="sOrder_no">订单号</param>
        /// <param name="sLocator">货位号</param>
        /// <param name="sback_locator">此货位对应的背后的货位</param>
        /// <param name="sg">返回的信息</param>
        /// <returns></returns>
        public bool InsertDASLine(int ihead_id, string sOrder_no, string sLocator, string sback_locator, string sBoxNo, out string sg)
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
        /// <param name="sSourceId">从WMS下载的记录行Id</param>
        /// <param name="sg"></param>
        /// <returns></returns>
        public bool InsertDASDetail(int iLine, string sitem_code, string sitem_barcode, int irequire_qty, int actual_qty, string sitem_unit, string sSourceId, out string sg)
        {
            string sSql = string.Format(@"insert into t_das_detail(line_id, item_code, item_barcode, require_qty, actual_qty, item_unit,create_by) values ({0}, '{1}', '{2}', {3}, 0, '{4}','{5}')", iLine, sitem_code, sitem_barcode, irequire_qty, sitem_unit, sSourceId);
            return myShd.ExecutCmd(sSql, out sg);
        }

    }
}
