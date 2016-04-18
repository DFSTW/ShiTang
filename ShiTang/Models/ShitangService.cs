using System;
using System.Collections.Generic;
using System.Data;
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
            return "无此胸卡号[" + username + "]";
        }

        public static List<XiaoFeiJiLu> GetDetails(string username, string YearMonth)
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
  FROM [icsystem].[dbo].DQ_CONSUME_RECORDS_" + YearMonth + @" a, dq_r_emp_cards b
where a.dq_cardid = b.dq_cardid
and a.dq_flag = 0
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
            reader.Close();
            connection.Close();
            return jl;
        }



        internal static ChargeModel GetCharge()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            SqlCommand cmdBOM = new SqlCommand();
            cmdBOM.Connection = connection;
            cmdBOM.CommandText =
                @" SELECT [dq_value],[dq_flag]
  FROM [icsystem].[dbo].[DQ_CHARGE]
where dq_comp = '东汽'";

            var reader = cmdBOM.ExecuteReader();
            ChargeModel cm = new ChargeModel();
            while (reader.Read())
            {
                cm.Charge = reader.GetSqlInt32(0).Value;
                cm.Reset = reader.GetSqlBoolean(1).Value;
            }
            reader.Close();
            connection.Close();

            return cm;
        }

        internal static void UpdateCharge(ChargeModel model)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            SqlCommand cmdBOM = new SqlCommand();
            cmdBOM.Connection = connection;
            cmdBOM.CommandText =
                @" UPDATE [icsystem].[dbo].[DQ_CHARGE]
                   SET [dq_value] = @dq_value
                      ,[dq_flag] = @dq_flag
                where [dq_comp] = @dq_comp";
            cmdBOM.Parameters.Add("@dq_value", SqlDbType.Int);
            cmdBOM.Parameters.Add("@dq_flag", SqlDbType.Bit);
            cmdBOM.Parameters.Add("@dq_comp", SqlDbType.NVarChar);
            cmdBOM.Parameters["@dq_value"].Value = model.Charge;
            cmdBOM.Parameters["@dq_flag"].Value = model.Reset;
            cmdBOM.Parameters["@dq_comp"].Value = model.Comp;

            var reader = cmdBOM.ExecuteNonQuery();
            connection.Close();
        }

        internal static WhiteList GetWhiteList(string id)
        {
            WhiteList w = new WhiteList();
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var cmd = new SqlCommand(
    @"select b.dq_cardid, c.dq_name, a.dq_charge from DQ_WHITELIST_201604 a, dq_r_emp_cards b, dq_employees c
where a.dq_empid = c.dq_id
and c.dq_id = b.dq_empid
and b.dq_isprimary = 'Y'
and c.dq_id = @id;");
                cmd.Connection = conn;
                cmd.Parameters.Add("@id", SqlDbType.NVarChar);
                cmd.Parameters["@id"].Value = id;

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    w.dq_cardid = reader.GetSqlString(0).Value;
                    w.dq_name = reader.GetSqlString(1).Value;
                    w.dq_value = reader.GetSqlInt32(2).Value;

                }
                conn.Close();
            }

            return w;

        }

        internal static bool CreateNewUser(string id, string name, string cardid, int charge)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand cmd;
                try
                {
                    cmd = new SqlCommand(
                   @"INSERT INTO [icsystem].[dbo].[DQ_EMPLOYEES]
                       ([dq_id]
                       ,[dq_name]
                       ,[dq_state]
		               ,[dq_type])
                 VALUES
                       (@dq_id
                       ,@dq_name
                       ,'正常'
                       ,'普通')");
                    cmd.Connection = conn;
                    cmd.Parameters.Add("@dq_id", SqlDbType.NVarChar);
                    cmd.Parameters["@dq_id"].Value = id;
                    cmd.Parameters.Add("@dq_name", SqlDbType.NVarChar);
                    cmd.Parameters["@dq_name"].Value = name;

                    cmd.ExecuteNonQuery();
                }
                catch (Exception e) { }
                try
                {
                    cmd = new SqlCommand(
                    @"INSERT INTO [icsystem].[dbo].[DQ_R_EMP_CARDS]
                       ([dq_empid]
                       ,[dq_cardid]
                       ,[dq_isprimary]
                       ,[dq_createdate]
                       )
                 VALUES
                       (@dq_empid
                       ,@dq_cardid
                       ,'Y'
                       ,GetDate())");
                    cmd.Connection = conn;
                    cmd.Parameters.Add("@dq_empid", SqlDbType.NVarChar);
                    cmd.Parameters["@dq_empid"].Value = id;
                    cmd.Parameters.Add("@dq_cardid", SqlDbType.NVarChar);
                    cmd.Parameters["@dq_cardid"].Value = cardid;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e) { }
                try
                {
                    cmd = new SqlCommand(
                    @"INSERT INTO [icsystem].[dbo].[DQ_WHITELIST_" + @DateTime.Now.ToString("yyyyMM") + @"]
                       ([dq_empid]
                       ,[dq_charge])
                 VALUES
                       (@dq_empid
                       ,@dq_charge)");
                    cmd.Connection = conn;
                    cmd.Parameters.Add("@dq_empid", SqlDbType.NVarChar);
                    cmd.Parameters["@dq_empid"].Value = id;
                    cmd.Parameters.Add("@dq_charge", SqlDbType.Int);
                    cmd.Parameters["@dq_charge"].Value = charge;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e) { }
                try
                {
                    cmd = new SqlCommand(
                    @"INSERT INTO [icsystem].[dbo].[DQ_WHITELIST_INIT]
                       ([dq_cardid]
                       ,[dq_name]
                       ,[dq_value])
                 VALUES
                       (@dq_cardid
                       ,@dq_name
                       ,@dq_value)");
                    cmd.Connection = conn;
                    cmd.Parameters.Add("@dq_cardid", SqlDbType.NVarChar);
                    cmd.Parameters["@dq_cardid"].Value = cardid;
                    cmd.Parameters.Add("@dq_name", SqlDbType.NVarChar);
                    cmd.Parameters["@dq_name"].Value = name;
                    cmd.Parameters.Add("@dq_value", SqlDbType.Int);
                    cmd.Parameters["@dq_value"].Value = charge;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e) { }
                try
                {
                    cmd = new SqlCommand(
                    @"[updateDepartment]");
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e) { }
                conn.Close();
            }
            return true;
        }

        internal static UserInfo GetUserInfo(string id)
        {
            var w = new UserInfo();
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var cmd = new SqlCommand(
                @"select x.*,  isnull(y.dq_charge, 0) dq_charge from (select a.dq_id, a.dq_name, a.dq_depart, a.dq_state, a.dq_company, b.dq_cardid from dq_employees a, dq_r_emp_cards b
            where a.dq_id = b.dq_empid
            and b.dq_isprimary = 'Y'
            and a.dq_id like '%' + @id + '%') x left join DQ_WHITELIST_" + DateTime.Now.ToString("yyyyMM") + @" y
            on x.dq_id = y.dq_empid");
                cmd.Connection = conn;
                cmd.Parameters.Add("@id", SqlDbType.NVarChar);
                cmd.Parameters["@id"].Value = id;

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    w.dq_id = reader.GetSqlString(0).Value;
                    w.dq_name = reader.GetSqlString(1).Value;
                    w.dq_depart = reader.GetSqlString(2).Value;
                    w.dq_state = reader.GetSqlString(3).Value;
                    w.dq_company = reader.GetSqlString(4).Value;
                    w.dq_cardid = reader.GetSqlString(5).Value;
                    w.dq_charge = reader.GetSqlInt32(6).Value;
                }
                conn.Close();
            }

            return w;
        }

        internal static void UpdateUserStatus(string id, string status)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var cmd = new SqlCommand(
                @"UPDATE [icsystem].[dbo].[DQ_EMPLOYEES]
                   SET [dq_state] = @state
                 WHERE dq_id = @id");
                cmd.Connection = conn;
                cmd.Parameters.Add("@state", SqlDbType.NVarChar);
                cmd.Parameters["@state"].Value = status;
                cmd.Parameters.Add("@id", SqlDbType.NVarChar);
                cmd.Parameters["@id"].Value = id;

                var reader = cmd.ExecuteNonQuery();
                conn.Close();
            }

        }

        internal static void UpdateCharge(string id, int charge)
        {
            if (UserChargeExists(id))
            {
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    var cmd = new SqlCommand(
                       @"update [icsystem].[dbo].[DQ_WHITELIST_" + @DateTime.Now.ToString("yyyyMM") + @"]
                       set [dq_charge] = @dq_charge
                       where dq_empid =  @dq_empid");
                    cmd.Connection = conn;
                    cmd.Parameters.Add("@dq_charge", SqlDbType.Int);
                    cmd.Parameters["@dq_charge"].Value = charge;
                    cmd.Parameters.Add("@dq_empid", SqlDbType.NVarChar);
                    cmd.Parameters["@dq_empid"].Value = id;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    var cmd = new SqlCommand(
                       @"INSERT INTO [icsystem].[dbo].[DQ_WHITELIST_" + @DateTime.Now.ToString("yyyyMM") + @"]
                       ([dq_empid]
                       ,[dq_charge])
                 VALUES
                       (@dq_empid
                       ,@dq_charge)");
                    cmd.Connection = conn;
                    cmd.Parameters.Add("@dq_empid", SqlDbType.NVarChar);
                    cmd.Parameters["@dq_empid"].Value = id;
                    cmd.Parameters.Add("@dq_charge", SqlDbType.Int);
                    cmd.Parameters["@dq_charge"].Value = charge;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        private static bool UserChargeExists(string id)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var cmd = new SqlCommand(
                   @"select count(*) from  [icsystem].[dbo].[DQ_WHITELIST_" + @DateTime.Now.ToString("yyyyMM") + @"]
                       where dq_empid =  @dq_empid");
                cmd.Connection = conn;
                cmd.Parameters.Add("@dq_empid", SqlDbType.NVarChar);
                cmd.Parameters["@dq_empid"].Value = id;

                var r = cmd.ExecuteScalar();
                conn.Close();
                if (r != null)
                {
                    return (int)r > 0;
                }
                return false;

            }
        }


    }
}