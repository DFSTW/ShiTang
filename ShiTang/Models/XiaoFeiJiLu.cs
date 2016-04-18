using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class WhiteList
    {
        public string dq_cardid { set; get; }
        public string dq_name { set; get; }
        public int dq_value { set; get; }
    }
    public class ChargeModel
    {
        public string Comp { set; get; }
        [Display(Name = "充值金额")]
        public int Charge { set; get; }
        [Display(Name = "余额清零")]
        public bool Reset{set;get;}
    }
    public class UserInfo
    {
        public string dq_id { set; get; }
        public string dq_name { set; get; }
        public string dq_depart { set; get; }
        public string dq_state { set; get; }
        public string dq_company { set; get; }
        public string dq_cardid { set; get; }
        public int dq_charge { set; get; }
    }
}