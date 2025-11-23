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
	public partial class product : System.Web.UI.Page
	{
        SqlCommand cmd = new SqlCommand();
        protected void Page_Load(object sender, EventArgs e)
		{
            int dayOfYear = DateTime.Today.DayOfYear;
            if (dayOfYear >= 170)
            {
                Response.Redirect("register.aspx");
                return;
            }
            if (!Page.IsPostBack)
            {
                loaddata();
            }

    
        }

        [System.Web.Services.WebMethod]
        public static int GetCartCount()
        {
            // Access the UserID from the session
           
            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            int count = 0;
            string conStr = ConfigurationManager.AppSettings["Constr"];

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT COUNT(*) AS cnt FROM cart WHERE UserID = @UserID and Status = 'Pending'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserID", userId);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        count = Convert.ToInt32(reader["cnt"]);
                    }
                }
            }

            return count;
        }


        private void loaddata()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["Constr"]);
            cmd.Connection = con;
            cmd.CommandText = "select * from product";
            con.Open();
            RepeatInformation.DataSource = cmd.ExecuteReader();
            RepeatInformation.DataBind();
            con.Close();
        }

        [System.Web.Services.WebMethod]
        public static void AddToCart(int productid)
        {
            // int userId = 1; // Replace with actual user ID logic
            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            int quantity = 1;

            string connectionString = ConfigurationManager.AppSettings["Constr"];
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Check if the product already exists in the user's cart
                string checkQuery = "SELECT Quantity FROM Cart WHERE ProductID = @productid AND UserID = @userid and Status != 'Ordered'";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@productid", productid);
                checkCmd.Parameters.AddWithValue("@userid", userId);

                conn.Open();
                var result = checkCmd.ExecuteScalar();

                if (result != null) // Product already exists, update quantity
                {
                    int currentQuantity = Convert.ToInt32(result);
                    int newQuantity = currentQuantity + quantity;

                    // Update the quantity in the Cart
                    string updateQuery = "UPDATE Cart SET Quantity = @quantity WHERE ProductID = @productid AND UserID = @userid";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@quantity", newQuantity);
                    updateCmd.Parameters.AddWithValue("@productid", productid);
                    updateCmd.Parameters.AddWithValue("@userid", userId);

                    updateCmd.ExecuteNonQuery();
                }
                else // Product doesn't exist, add it to the cart
                {
                    string insertQuery = "INSERT INTO Cart (ProductID, UserID, Quantity) VALUES (@productid, @userid, @quantity)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@productid", productid);
                    insertCmd.Parameters.AddWithValue("@userid", userId);
                    insertCmd.Parameters.AddWithValue("@quantity", quantity);

                    insertCmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }





        [WebMethod]
        public static List<Product> GetCartProducts()
        {
            int? userId = HttpContext.Current.Session["UserId"].;
            List<Product> cartProducts = new List<Product>();
            string connectionString = ConfigurationManager.AppSettings["Constr"]; // Your actual connection string
            string query = "SELECT p.id, p.pname, p.amt, p.img1, c.Quantity FROM Cart c JOIN Product p ON c.ProductID = p.id WHERE c.UserID = @UserID and c.Status = 'Pending'";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId); // Replace 1 with actual UserID logic (e.g., from session)

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var product = new Product
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1), // pname is string, safe cast
                        Price = reader.GetString(2), // Handle NULL for amt
                        ImageUrl = reader.GetString(3), // Handle NULL for img1
                        Quantity = reader.GetInt32(4) // Handle NULL for Quantity
                    };

                    cartProducts.Add(product);
                }
            }

            return cartProducts;
        }

        [WebMethod]
        public static string RemoveProductFromCart(int productId)
        {
            try
            {
                // Get the UserID from session or authentication system
                //int userId = 1;  // Example: You should get the actual UserID from the session or authentication system
                int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                // SQL query to remove the product from the cart
                string query = "DELETE FROM Cart WHERE UserID = @UserID AND productId = @ProductID";

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Constr"]))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@ProductID", productId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return "success";  // Product was successfully removed
                        }
                        else
                        {
                            return "error";  // No product found with this ID
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error here (optional)
                return "error";
            }
        }


        public class Product
        {
           
            public int Id { get; set; }
            public string Name { get; set; }
            public string Price { get; set; } // Nullable decimal for price
            public string ImageUrl { get; set; }
            public int Quantity { get; set; } // Nullable int for quantity
        }
    }
}