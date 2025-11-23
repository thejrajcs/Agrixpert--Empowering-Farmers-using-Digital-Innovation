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
	public partial class AddEvent : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            DateTime date = DateTime.Parse(txtDate.Text);
            TimeSpan startTime = TimeSpan.Parse(txtStartTime.Text);
            TimeSpan endTime = TimeSpan.Parse(txtEndTime.Text);
            string venue = txtVenue.Text.Trim();
            string eventURL = txtURL.Text.Trim();
            string status = ddlStatus.SelectedValue;

            // Save the uploaded image     string imgPath1 = SaveFile(img1, folderPath);
            string imagePath = SaveFile(fuImage, Server.MapPath("~/uploads/"));

            // Save relative path to DB
            string dbImagePath = imagePath != string.Empty ? "/uploads/" + Path.GetFileName(imagePath) : null;

            string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Events 
                        (Title, Date, StartTime, EndTime, Venue, ImagePath, EventURL, Status)
                        VALUES 
                        (@Title, @Date, @StartTime, @EndTime, @Venue, @ImagePath, @EventURL, @Status)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@StartTime", startTime);
                    cmd.Parameters.AddWithValue("@EndTime", endTime);
                    cmd.Parameters.AddWithValue("@Venue", venue);
                    cmd.Parameters.AddWithValue("@ImagePath", (object)dbImagePath ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EventURL", eventURL);
                    cmd.Parameters.AddWithValue("@Status", status);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            // Optional: Clear form and notify user
          
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Event added successfully!');", true);
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