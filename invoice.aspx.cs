using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace agri.shop
{
	public partial class invoice : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                int orderId;
                if (int.TryParse(Request.QueryString["orderId"], out orderId))
                {
                    GenerateInvoicePdf(orderId);
                }
                else
                {
                    Response.Write("Invalid Order ID.");
                }
            }
        }

        private void GenerateInvoicePdf(int orderId)
        {
            string constr = ConfigurationManager.AppSettings["Constr"];
            string firstName = "", lastName = "", email = "";
            DateTime orderDate = DateTime.Now;
            decimal totalAmount = 0;
            DataTable dtProducts = new DataTable();

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                // Order header info
                SqlCommand cmdOrder = new SqlCommand("SELECT FirstName, LastName, Email, OrderDate, TotalAmount FROM [Order] WHERE OrderID = @OrderID", con);
                cmdOrder.Parameters.AddWithValue("@OrderID", orderId);
                using (SqlDataReader reader = cmdOrder.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        firstName = reader["FirstName"].ToString();
                        lastName = reader["LastName"].ToString();
                        email = reader["Email"].ToString();
                        orderDate = Convert.ToDateTime(reader["OrderDate"]);
                        totalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                    }
                }

                // Order details
                SqlCommand cmdDetails = new SqlCommand("SELECT ProductName, Quantity, Price FROM OrderDetails WHERE OrderID = @OrderID", con);
                cmdDetails.Parameters.AddWithValue("@OrderID", orderId);
                SqlDataAdapter da = new SqlDataAdapter(cmdDetails);
                da.Fill(dtProducts);
              
            }

            // Generate PDF
            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var normal = FontFactory.GetFont(FontFactory.HELVETICA, 11);

                doc.Add(new Paragraph("Invoice", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)));
                doc.Add(new Paragraph("Order ID: " + orderId));
                doc.Add(new Paragraph("Customer: " + firstName + " " + lastName));
                doc.Add(new Paragraph("Email: " + email));
                doc.Add(new Paragraph("Order Date: " + orderDate.ToString("dd MMM yyyy")));
                doc.Add(new Paragraph("\n"));

                // Product Table
                PdfPTable table = new PdfPTable(4) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 40, 20, 20, 20 });

                table.AddCell(new Phrase("Product", bold));
                table.AddCell(new Phrase("Quantity", bold));
                table.AddCell(new Phrase("Price", bold));
                table.AddCell(new Phrase("Subtotal", bold));

                foreach (DataRow row in dtProducts.Rows)
                {
                    decimal qty = Convert.ToDecimal(row["Quantity"]);
                    decimal price = Convert.ToDecimal(row["Price"]);
                    decimal subtotal = qty * price;

                    table.AddCell(new Phrase(row["ProductName"].ToString(), normal));
                    table.AddCell(new Phrase(qty.ToString(), normal));
                    table.AddCell(new Phrase(price.ToString("C"), normal));
                    table.AddCell(new Phrase(subtotal.ToString("C"), normal));
                }

                // Total row
                PdfPCell empty = new PdfPCell(new Phrase("")) { Colspan = 3, Border = 0 };
                table.AddCell(empty);
                table.AddCell(new Phrase("Total: " + totalAmount.ToString("C"), bold));

                doc.Add(table);
                doc.Close();

                // Stream PDF to browser
                byte[] pdfBytes = ms.ToArray();
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", $"attachment; filename=Invoice_{orderId}.pdf");
                Response.OutputStream.Write(pdfBytes, 0, pdfBytes.Length);
                Response.Flush();
                Response.End();
            }
           
        }       
    }
}