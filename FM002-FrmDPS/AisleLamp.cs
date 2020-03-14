using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFrmDPS
{
    public class AisleLamp
    {
        public string region { get; set; }
        public int count { get; set; }//默认为0 没任务，小于等于3，每发送一次巷道灯指令+1，
                                      //防止发送一次没有亮，3次后不再发送
        public int lightCount { get; set; }
    }
}
