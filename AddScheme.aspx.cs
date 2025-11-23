using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace agri.Vendor
{
	public partial class AddScheme : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {


                // Save the uploaded image     string imgPath1 = SaveFile(img1, folderPath);
                string imagePath = SaveFile(fuImage, Server.MapPath("~/uploads/"));

                // Save relative path to DB
                string dbImagePath = imagePath != string.Empty ? "/uploads/" + Path.GetFileName(imagePath) : null;


                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                {
                    string query = @"INSERT INTO Scheme (Name,url,Other,Description,pfile) 
                         VALUES (@Name,@url,@Other,@Description,@pfile)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        
                        cmd.Parameters.AddWithValue("@Name", Txtname.Text.Trim());
                        cmd.Parameters.AddWithValue("@url", Txturl.Text.Trim());
                    
                        cmd.Parameters.AddWithValue("@Other", Txtother.Text.Trim());
                        cmd.Parameters.AddWithValue("@Description", txtDesc.Text.Trim());
                    
                        cmd.Parameters.AddWithValue("@pfile", (object)dbImagePath ?? DBNull.Value);
                        //cmd.Parameters.AddWithValue("@quantity", txtQty.Text.Trim());
                        //cmd.Parameters.AddWithValue("@pstatus", ddlStatus.SelectedValue);


                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        Response.Write("<script>alert('Scheme Added Succesfully'); window.location='ManageScheme.aspx';</script>");
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
                string fileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                string extension = Path.GetExtension(fileUpload.FileName);
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string uniqueFileName = $"{fileName}_{timestamp}{extension}";

                string fullPath = Path.Combine(folderPath, uniqueFileName);
                fileUpload.SaveAs(fullPath);

                // Return only file name with timestamp
                return uniqueFileName;
            }
            return string.Empty;
        }
    }
}
