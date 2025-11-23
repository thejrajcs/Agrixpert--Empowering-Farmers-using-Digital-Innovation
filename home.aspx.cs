using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace agri.Vendor
{
	public partial class home : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            // Session validation
            if (Session["UserName"] == null || string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                // Session expired or not set, redirect to register (login) page
                Response.Redirect("register.aspx");
                return;
            }
            if (!IsPostBack)
            {
                BindCounts();
            }
        }

        private void BindCounts()
        {
            string conStr = ConfigurationManager.AppSettings["Constr"];
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                // Get Total Users
                string queryUser = "SELECT COUNT(*) FROM Users where utype='User'";  // Change Users to your actual Users table name
                SqlCommand cmdUser = new SqlCommand(queryUser, con);
                int totalUsers = (int)cmdUser.ExecuteScalar();
                Label1.Text = totalUsers.ToString();

                // Get Total Vendors
                string queryVendor = "SELECT COUNT(*) FROM Users where utype='Vendor'";  // Change Vendors to your actual Vendors table name
                SqlCommand cmdVendor = new SqlCommand(queryVendor, con);
                int totalVendors = (int)cmdVendor.ExecuteScalar();
                Label2.Text = totalVendors.ToString();

                // Get Total Events
                string queryEvent = "SELECT COUNT(*) FROM Events";  // Change Events to your actual Events table name
                SqlCommand cmdEvent = new SqlCommand(queryEvent, con);
                int totalEvents = (int)cmdEvent.ExecuteScalar();
                Label3.Text = totalEvents.ToString();

                // Get Total Schemes
                string queryScheme = "SELECT COUNT(*) FROM Scheme";  // Change Schemes to your actual Schemes table name
                SqlCommand cmdScheme = new SqlCommand(queryScheme, con);
                int totalSchemes = (int)cmdScheme.ExecuteScalar();
                Label4.Text = totalSchemes.ToString();

                con.Close();
            }
        }
    }
}