using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShiTang.Models
{
    public class XiaoFeiJiLu
    {
        public string dq_ip { set; get; }
        public string dq_cardid { set; get; }
        public decimal dq_value { set; get; }
        public DateTime dq_time { set; get; }
        public int dq_flag { set; get; }
    }
    public class XiaoFeiYue
    {
        public string remains { set; get; }
        public DateTime lastupdate { set; get; }
    }
}