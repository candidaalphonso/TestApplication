using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using TestApplication.Models;

namespace TestApplication
{
    public class DAL//data access layer to sql
    {
        public static string Register(Authentication model)
        {
            SqlConnection cn = new SqlConnection();
            string connectionstring = @"Server=(LocalDb)\MSSQLLocalDB;Database=TestApp;";
            string strSql = " exec usp_register   @username = '" + model.Username + "', @password = '" + model.Password + "'";
            System.Data.DataSet response = new System.Data.DataSet();

            SqlDataAdapter sqlAdp;
            System.Data.DataSet ds = new System.Data.DataSet();
            string result;
            cn.ConnectionString = connectionstring;

            cn.Open();


            try
            {

                sqlAdp = new SqlDataAdapter(strSql, cn);

                sqlAdp.Fill(ds);
                result = ds.Tables[0].Rows[0]["result"].ToString();
                //result = "success";
                
                cn.Close();

            }
            catch (Exception ex)
            {
                DataTable table1 = new DataTable("Error");

                table1.Columns.Add("ErrorMessage");
                table1.Rows.Add(ex.Message);

                result = "fail";
            }
            return result;
        }

        public static string Login(Authentication model)
        {
            SqlConnection cn = new SqlConnection();
            string connectionstring = @"Server=(LocalDb)\MSSQLLocalDB;Database=TestApp;";
            string strSql = " exec usp_login   @username = '" + model.Username + "', @password = '" + model.Password + "'";
            System.Data.DataSet response = new System.Data.DataSet();

            SqlDataAdapter sqlAdp;
            System.Data.DataSet ds = new System.Data.DataSet();
            string result;
            cn.ConnectionString = connectionstring;

            cn.Open();


            try
            {

                sqlAdp = new SqlDataAdapter(strSql, cn);

                sqlAdp.Fill(ds);
                result =  ds.Tables[0].Rows[0]["result"].ToString();

                cn.Close();

            }
            catch (Exception ex)
            {
                DataTable table1 = new DataTable("Error");

                table1.Columns.Add("ErrorMessage");
                table1.Rows.Add(ex.Message);

                result = "fail";
            }
            return result;
        }
    }
}
