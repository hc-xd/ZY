using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFrmDPS
{
    public class DataModelDPS
    {
        public DataModelDPS()
        {
        }

        /// <summary>
        /// 说明：数据模型的第一层（区域）
        /// </summary>
        public class DPSLayer1
        {
            public string date{ get; set; } //日期
            public string production { get; set; } //工艺
            public string productionName { get; set; }//零件
            public int id { get; set; }//唯一标识

            public  List<DPSLayer2> dps2 { get;set;}
        }

        /// <summary>
        /// 说明：数据模型的第二层（巷道）
        /// </summary>
        public class DPSLayer2
        {
            public string aisle_lamp_id { get; set; } //巷道灯地址
            public string aisle_lamp_id_ip { get; set; }//巷道灯对应的IP地址
            public string tag_id_End { get; set; } //记录最后一个标签地址
            public string tag_id_ip_End { get; set; } //记录最后一个标签IP地址
            public List<DPSLayer3> dps3 { get; set; }
        }

        /// <summary>
        /// 说明：数据模型的第三层（标签）
        /// </summary>
        public class DPSLayer3
        {
            public int ID { get; set; } //明细行唯一ID
            public string item_code { get; set; } //物料编码
            public string item_desc { get; set; } //物料名称
            public int flag { get; set; }//状态标识,0代表未拣 1拣料已完成
            public int require_quantity { get; set; } //拣料需求数量
            public int actual_quantity { get; set; } //实际拣货数量
            public string tag_id { get; set; } //标签地址
            public string tag_id_ip { get; set; } //标签IP地址
            public int isLight { get; set; } //设置标签是否已经成功被亮灯
            public string locator { get; set; }//标签货位
        }


    }
}
