using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace agri.shop
{
	public partial class checkout : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}



        [WebMethod]
        public static List<CartProduct> GetCartItems()
        {
            //int userId = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            //int userId = 1;
            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            List<CartProduct> cartItems = new List<CartProduct>();

            string constr = ConfigurationManager.AppSettings["Constr"];
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string query = @"SELECT p.pname,c.ProductID, p.amt, c.Quantity 
                         FROM Cart c 
                         JOIN Product p ON c.ProductID = p.id 
                         WHERE c.UserID = @userid AND c.Status = 'Pending'";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userid", userId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cartItems.Add(new CartProduct
                    {
                        Name = reader["pname"].ToString(),
                        pid = reader["ProductID"].ToString(),
                        Price = Convert.ToDecimal(reader["amt"]),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }
                conn.Close();
            }

            return cartItems;
        }

    
        public class CartProduct
        {
            public string Name { get; set; }
            public string pid { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }

[WebMethod]
public static string PlaceOrder(OrderDetailsModel details)
{
    int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
    string constr = ConfigurationManager.AppSettings["Constr"];
    decimal totalCartAmount = 0;
    int orderId = 0; // Declare here to make it accessible at the end

    List<CartProduct> cartItems = new List<CartProduct>();

    using (SqlConnection conn = new SqlConnection(constr))
    {
        conn.Open();

        // 1. Get Cart Items
        string cartQuery = @"SELECT p.pname,c.Productid, p.amt, c.Quantity 
                             FROM Cart c 
                             JOIN Product p ON c.ProductID = p.id 
                             WHERE c.UserID = @userid AND c.Status = 'Pending'";
        SqlCommand cmdCart = new SqlCommand(cartQuery, conn);
        cmdCart.Parameters.AddWithValue("@userid", userId);
        SqlDataReader reader = cmdCart.ExecuteReader();
        while (reader.Read())
        {
            var item = new CartProduct
            {
                Name = reader["pname"].ToString(),
                pid = reader["Productid"].ToString(),
                Price = Convert.ToDecimal(reader["amt"]),
                Quantity = Convert.ToInt32(reader["Quantity"])
            };
            totalCartAmount += item.Price * item.Quantity;
            cartItems.Add(item);
        }
        reader.Close();

        // 2. Check Wallet Balance
        SqlCommand cmdWallet = new SqlCommand("SELECT Amount FROM Wallet WHERE UserID = @userid", conn);
        cmdWallet.Parameters.AddWithValue("@userid", userId);
        decimal walletAmount = Convert.ToDecimal(cmdWallet.ExecuteScalar());

        if (walletAmount < totalCartAmount)
            return "Insufficient balance in wallet.";

        // 3. Insert into Order table
        string insertOrder = @"INSERT INTO [Order] 
                               (UserID, OrderDate, TotalAmount, FirstName, LastName, CompanyName, Country, AddressLine1, AddressLine2, City, State, ZipCode, Phone, Email, OrderNotes)
                               OUTPUT INSERTED.OrderID
                               VALUES 
                               (@UserID, GETDATE(), @TotalAmount, @FirstName, @LastName, @CompanyName, @Country, @Address1, @Address2, @City, @State, @ZipCode, @Phone, @Email, @Notes)";
        SqlCommand cmdOrder = new SqlCommand(insertOrder, conn);
        cmdOrder.Parameters.AddWithValue("@UserID", userId);
        cmdOrder.Parameters.AddWithValue("@TotalAmount", totalCartAmount);
        cmdOrder.Parameters.AddWithValue("@FirstName", details.FirstName);
        cmdOrder.Parameters.AddWithValue("@LastName", details.LastName);
        cmdOrder.Parameters.AddWithValue("@CompanyName", details.CompanyName ?? "");
        cmdOrder.Parameters.AddWithValue("@Country", details.Country);
        cmdOrder.Parameters.AddWithValue("@Address1", details.Address1);
        cmdOrder.Parameters.AddWithValue("@Address2", details.Address2);
        cmdOrder.Parameters.AddWithValue("@City", details.City);
        cmdOrder.Parameters.AddWithValue("@State", details.State);
        cmdOrder.Parameters.AddWithValue("@ZipCode", details.ZipCode);
        cmdOrder.Parameters.AddWithValue("@Phone", details.Phone);
        cmdOrder.Parameters.AddWithValue("@Email", details.Email);
        cmdOrder.Parameters.AddWithValue("@Notes", details.Notes ?? "");

        // Store the generated order ID
        orderId = (int)cmdOrder.ExecuteScalar();

        // 4. Insert each item into OrderDetails
        foreach (var item in cartItems)
        {
            SqlCommand cmdDetails = new SqlCommand(@"INSERT INTO OrderDetails (OrderID,pid, ProductName, Quantity, Price, userid) 
                                                     VALUES (@OrderID,@pid, @ProductName, @Quantity, @Price, @userid)", conn);
            cmdDetails.Parameters.AddWithValue("@OrderID", orderId);
            cmdDetails.Parameters.AddWithValue("@pid", item.pid);
            cmdDetails.Parameters.AddWithValue("@ProductName", item.Name);
            cmdDetails.Parameters.AddWithValue("@Quantity", item.Quantity);
            cmdDetails.Parameters.AddWithValue("@Price", item.Price);
            cmdDetails.Parameters.AddWithValue("@userid", userId);
            cmdDetails.ExecuteNonQuery();
        }

        // 5. Deduct from wallet
        SqlCommand cmdUpdateWallet = new SqlCommand("UPDATE Wallet SET Amount = Amount - @deduct WHERE UserID = @userid", conn);
        cmdUpdateWallet.Parameters.AddWithValue("@deduct", totalCartAmount);
        cmdUpdateWallet.Parameters.AddWithValue("@userid", userId);
        cmdUpdateWallet.ExecuteNonQuery();

        // 6. Update cart status
        SqlCommand cmdUpdateCart = new SqlCommand("UPDATE Cart SET Status = 'Ordered' WHERE UserID = @userid AND Status = 'Pending'", conn);
        cmdUpdateCart.Parameters.AddWithValue("@userid", userId);
        cmdUpdateCart.ExecuteNonQuery();

        conn.Close();
    }

    // Return success message along with the generated OrderID
    return "success|" + orderId;
}

        public class OrderDetailsModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string CompanyName { get; set; }
            public string Country { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Notes { get; set; }
        }


    }
}