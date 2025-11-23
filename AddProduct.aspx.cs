using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace agri.Vendor
{
	public partial class AddProduct : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}
     
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var BusinessID = Session["UserID"].ToString();
                //var BusinessID = "1";

                string folderPath = Server.MapPath("~/Uploads/");
                string imgPath1 = SaveFile(img1, folderPath);
                string imgPath2 = SaveFile(img2, folderPath);
                string imgPath3 = SaveFile(img3, folderPath);

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                {
                    string query = @"INSERT INTO Product (img1, img2, img3, ptype, pname, descp, amt, BusinessID,Category,quantity) 
                         VALUES (@img1, @img2, @img3, @ptype, @pname, @descp, @amt, @BusinessID,@Category,@quantity)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@img1", "Uploads/" + imgPath1);
                        cmd.Parameters.AddWithValue("@img2", "Uploads/" + imgPath2);
                        cmd.Parameters.AddWithValue("@img3", "Uploads/" + imgPath3);
                        cmd.Parameters.AddWithValue("@ptype", txtType.Text.Trim());
                        cmd.Parameters.AddWithValue("@pname", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@descp", txtDesc.Text.Trim());
                        cmd.Parameters.AddWithValue("@amt", txtAmt.Text.Trim());
                        //cmd.Parameters.AddWithValue("@quantity", txtQty.Text.Trim());
                        //cmd.Parameters.AddWithValue("@pstatus", ddlStatus.SelectedValue);
                        cmd.Parameters.AddWithValue("@BusinessID", BusinessID); // Replace with actual logged-in vendor ID
                    
                        cmd.Parameters.AddWithValue("@Category", Category.Text); // Replace with actual logged-in vendor ID
                        cmd.Parameters.AddWithValue("@quantity", quantity.Text); // Replace with actual logged-in vendor ID

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        Response.Write("<script>alert('Product added successfully!'); window.location='ManageProduct.aspx';</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }

        }


        private string SaveFile(FileUpload fileUpload, string folderPath)
        {
            if (fileUpload.HasFile)
            {
                // Ensure folderPath ends with a slash
                if (!folderPath.EndsWith("\\"))
                {
                    folderPath += "\\";
                }

                // Get original file name and extension
                string originalFileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                string extension = Path.GetExtension(fileUpload.FileName);

                // Generate a unique name with timestamp
                string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
                string newFileName = originalFileName + "_" + timeStamp + extension;

                // Build the full path to save
                string savePath = Path.Combine(folderPath, newFileName);

                // Save the file
                fileUpload.SaveAs(savePath);

                // Return just the file name
                return newFileName;
            }
            return string.Empty;
        }
    }
}