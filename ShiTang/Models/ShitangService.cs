using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ShiTang.Models
{
    public static class ShitangService
    {
        static string str = "Data Source = 172.18.50.156;Initial Catalog = icsystem;User Id =sa;Password =*******;";
        static SqlConnection connection = new SqlConnection(str);
        public static string GetRemains(string username)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            SqlCommand cmdBOM = new SqlCommand();
            cmdBOM.Connection = connection;
            cmdBOM.CommandText =
                @" SELECT top 1 [dq_value]
  FROM [icsystem].[dbo].[DQ_WHITELIST_INIT] a, dq_r_emp_cards b
where a.dq_cardid = b.dq_cardid
and b.dq_empid = @user";
            cmdBOM.Parameters.Add("@user", System.Data.SqlDbType.NVarChar);
            cmdBOM.Parameters["@user"].Value = username;
            cmdBOM.Parameters["@user"].Direction = System.Data.ParameterDirection.Input;

            var res = cmdBOM.ExecuteScalar();
            connection.Close();
            if (res != null) return res.ToString();
            return "无此胸卡号[" + username+"]";
        }

        public static List<XiaoFeiJiLu> GetDetails(string username,string YearMonth)
        {
            List<XiaoFeiJiLu> jl = new List<XiaoFeiJiLu>();
            connection.Open();
            SqlCommand cmdBOM = new SqlCommand();
            cmdBOM.Connection = connection;
            cmdBOM.CommandText =
                @"SELECT a.[dq_ip]
      ,a.[dq_cardid]
      ,a.[dq_time]
      ,a.[dq_value]
      ,a.[dq_flag]
  FROM [icsystem].[dbo].DQ_CONSUME_RECORDS_"+YearMonth+@" a, dq_r_emp_cards b
where a.dq_cardid = b.dq_cardid
and b.dq_empid = @user order by a.dq_time";
            cmdBOM.Parameters.Add("@user", System.Data.SqlDbType.NVarChar);
            cmdBOM.Parameters["@user"].Value = username;
            cmdBOM.Parameters["@user"].Direction = System.Data.ParameterDirection.Input;

            var reader = cmdBOM.ExecuteReader();
            while (reader.Read())
            {
                jl.Add(new XiaoFeiJiLu()
                {
                    dq_ip = reader.GetSqlString(0).Value,
                    dq_cardid = reader.GetSqlString(1).Value,
                    dq_time = reader.GetSqlDateTime(2).Value,
                    dq_value = reader.GetSqlDecimal(3).Value,
                    dq_flag = reader.GetSqlInt32(4).Value,
                });
            }
            connection.Close();
            return jl;
        }
    }
}