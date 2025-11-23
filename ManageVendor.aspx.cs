using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace agri.Vendor
{
	public partial class ManageVendor : System.Web.UI.Page
	{
        SqlCommand cmd = new SqlCommand();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["Username"] == null)
            //{

            //    Response.Redirect("../index.aspx");
            //}
            loaddata();
            //string ComId = Session["cid"].ToString();
            if (!Page.IsPostBack)
            {
                loaddata();
            }
        }
        private void loaddata()
        {
            string businessID = Session["UserId"].ToString();
            //string ComId = Session["cid"].ToString();
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["Constr"]);
            cmd.Connection = con;
            cmd.CommandText = "select * from Users where utype='Vendor'";
            con.Open();
            RepeatInformation.DataSource = cmd.ExecuteReader();
            RepeatInformation.DataBind();
            con.Close();
        }
        [System.Web.Services.WebMethod()]
       
        public static int deleteClaim(string ID)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["Constr"]);
            try
            {
                con.Open();

                // Delete related data from Product table based on BusinessID (assuming BusinessID is related to the user)
                string deleteProductSql = "DELETE FROM Product WHERE BusinessID = (SELECT BusinessID FROM Users WHERE UserID = @UserID)";
                SqlCommand cmdDeleteProduct = new SqlCommand(deleteProductSql, con);
                cmdDeleteProduct.Parameters.AddWithValue("@UserID", ID);

                // Execute deletion from Product table
                int productRowsAffected = cmdDeleteProduct.ExecuteNonQuery();

                // Now, delete the user from the Users table
                string deleteUserSql = "DELETE FROM Users WHERE UserID = @UserID";
                SqlCommand cmdDeleteUser = new SqlCommand(deleteUserSql, con);
                cmdDeleteUser.Parameters.AddWithValue("@UserID", ID);

                // Execute deletion from Users table
                int userRowsAffected = cmdDeleteUser.ExecuteNonQuery();

                // If both deletions were successful, return 1
                if (productRowsAffected > 0 && userRowsAffected > 0)
                {
                    return 1;  // Successful deletion
                }

                return 0;  // If there was an issue deleting
            }
            catch (Exception ex)
            {
                // Log the error if needed
                return 0;  // Return 0 in case of any error
            }
            finally
            {
                con.Close();
            }
        }


    }
}