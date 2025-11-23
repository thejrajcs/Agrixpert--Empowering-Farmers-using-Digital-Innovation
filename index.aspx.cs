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
	public partial class index : System.Web.UI.Page
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
                BindTotalProductCount();
            }
        }

        private void BindTotalProductCount()
        {
            string businessID = Session["UserId"].ToString();
            string conStr = ConfigurationManager.AppSettings["Constr"];
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT COUNT(*) FROM Product where businessID='"+ businessID + "'";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    int totalProducts = (int)cmd.ExecuteScalar();
                    Label1.Text = totalProducts.ToString();
                }
            }
        }
    }
}