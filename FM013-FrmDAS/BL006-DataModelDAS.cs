using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFrmDAS
{
   public class DataModelDAS
    {
       public DataModelDAS()
       {
       }
       /// <summary>
       /// 说明：数据模型的第一层(波次号）
       /// </summary>
       public class DASLayer1
       {
           public int head_id { get; set; } //头表的ID
           public string wave_no { get; set; } //波次号

           public int flag_head { get; set; }//状态

           public string Com_id { get; set; } //扫描枪的ID
           public string Com_Ip { get; set; }//扫描枪的IP地址

           public string aisle_lamp_id { get; set; } //巷道灯地址
           public string aisle_lamp_id_ip { get; set; }//巷道灯对应的IP地址

           public string order_id { get; set; }// 订单显示器地址
           public string order_id_ip { get; set; } //订单显示器IP地址

           public string sTagIdTotal { get; set; } //汇总标签的ID
           public string sTagIpTotal { get; set; } //汇总标签的IP

           public int TotalNum {get;set;} //记录的总条数

           public int iStatus { get; set; } //记录汇总标签是否被拍灭过

           public List<DASLayer2> DAS2 { get; set; }
           public List<string> ListlarmTag { get; set; } //记录误触发的标签
       }
       /// <summary>
       /// 说明：数据模型的第二层（订单）
       /// </summary>
       public class DASLayer2
       {
           public int line_id { get; set; } //行表的ID
           public string order_no { get; set; }//订单号
           public string locator { get; set; }//货位号
           public string back_locator { get; set; }//前面标签对应的背后标签
           public int flag_line { get; set; }//行表的状态
           
           //标签地址
           public string tag_id_ip{get;set;}
           public string tag_id{get;set;}

           //标签地址背后标签
           public string tag_back_ip { get; set; }
           public string tag_back_id { get; set; }

           //记录最后一个标签地址
           public string tag_id_End { get; set; } //记录最后一个标签地址
           public string tag_id_ip_End { get; set; } //记录最后一个标签IP地址

           //标签是否已成功被点亮过
           public int isLight2 { get; set; } //设置标签是否已经成功被亮灯
           public List<DASLayer3> DAS3 { get; set; }
       }

       /// <summary>
       /// 说明：数据模型的第三层（物料）
       /// </summary>
       public class DASLayer3
       {
           public int detail_id { get; set; } //分播物料行的ID
           public string item_code { get; set; } //物料编码
           public string item_barcode { get; set; } //物料条码
           public int require_qty { get; set; } //拣料需求数量
           public int actual_qty { get; set; } //实际拣货数量
           public int isLight3 { get; set; } //是否已经被点亮的记录行
       }
    }
}
