using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace agri.shop
{
	public partial class cart : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        [WebMethod]
        public static List<Product> GetCartProducts()
        {
            //int userId = 1; // Use Session or actual logged-in user
            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            List<Product> cartProducts = new List<Product>();

            string conStr = ConfigurationManager.AppSettings["Constr"];
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT p.id, p.pname, p.amt, p.img1, c.Quantity FROM Cart c JOIN Product p ON c.ProductID = p.id WHERE c.UserID = @UserID and c.status!='Ordered'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserID", userId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product p = new Product();
                    p.Id = reader.GetInt32(0);
                    p.Name = reader.GetString(1);
                    p.Price = Convert.ToDecimal(reader[2]);
                    p.ImageUrl = reader.GetString(3);
                    p.Quantity = Convert.ToInt32(reader[4]);
                    cartProducts.Add(p);
                }
            }
            return cartProducts;
        }

        [WebMethod]
        public static void UpdateCartQuantity(int productId, int quantity)
        {
            //int userId = 1; // Replace with actual session user ID
            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string conStr = ConfigurationManager.AppSettings["Constr"];
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "UPDATE Cart SET Quantity = @Quantity WHERE UserID = @UserID AND ProductID = @ProductID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public static void RemoveCartItem(int productId)
        {
            //int userId = 1; // Replace with actual session user ID
            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string conStr = ConfigurationManager.AppSettings["Constr"];
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "DELETE FROM Cart WHERE UserID = @UserID AND ProductID = @ProductID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // =============== Product Class (Model) =================
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string ImageUrl { get; set; }
            public int Quantity { get; set; }
        }

    }
}