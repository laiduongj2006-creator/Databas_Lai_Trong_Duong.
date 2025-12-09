using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace StoreManagement
{
    public partial class WarehouseDashboard : Form
    {
        SqlConnection conn;
        public WarehouseDashboard()
        {
            InitializeComponent();
            conn = new SqlConnection("Server = DOTHUAN2006\\SQLEXPRESS; Database = StoreManagement3; integrated security = true ");
            MessageBox.Show("hi");


        }
        public void LoadProducts()
        {
            string sql =
        @"SELECT p.ProductID, p.ProductName, p.CategoryID, p.SupplierID,
                 c.CategoryName, s.SupplierName,
                 p.CostPrice, p.SellingPrice
          FROM Product p
          INNER JOIN Category c ON p.CategoryID = c.CategoryID
          INNER JOIN Supplier s ON p.SupplierID = s.SupplierID";

            using (SqlConnection conn =
                new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvProducts.DataSource = dt;
                }
            }
        }

        public void GetCategories()
        {
            string sql = "SELECT CategoryID, CategoryName FROM dbo.Category";
            DataTable table = new DataTable();
            new SqlDataAdapter(sql, conn).Fill(table);

            DataRow row = table.NewRow();
            row["CategoryID"] = 0;
            row["CategoryName"] = "-- Select Category --";
            table.Rows.InsertAt(row, 0);

            cbCategory_ManagerProduct.DataSource = table;
            cbCategory_ManagerProduct.DisplayMember = "CategoryName";
            cbCategory_ManagerProduct.ValueMember = "CategoryID";
        }
        public void GetSuppliers()
        {
            string sql = "SELECT SupplierID, SupplierName FROM dbo.Supplier";
            DataTable table = new DataTable();
            new SqlDataAdapter(sql, conn).Fill(table);

            DataRow row = table.NewRow();
            row["SupplierID"] = 0;
            row["SupplierName"] = "-- Select Supplier --";
            table.Rows.InsertAt(row, 0);
            cbSupplier_ManagerProduct.DataSource = table;
            cbSupplier_ManagerProduct.DisplayMember = "SupplierName";
            cbSupplier_ManagerProduct.ValueMember = "SupplierID";
        }
        public void ClearData()
        {
            txtID_ManagerProduct.Clear();
            txtName_ManagerProduct.Clear();
            txtCostPrice_ManagerProduct.Clear();
            txtSellingPrice_ManagerProduct.Clear();
            cbCategory_ManagerProduct.SelectedIndex = 0;
            cbSupplier_ManagerProduct.SelectedIndex = 0;
        }



        private void WarehouseDashboard_Load(object sender, EventArgs e)
        {
            LoadProducts();
            GetCategories();
            GetSuppliers();
        }

        private void btnAdd_Product_Click(object sender, EventArgs e)
        {
            if ((int)cbCategory_ManagerProduct.SelectedValue == 0 || (int)cbSupplier_ManagerProduct.SelectedValue == 0)
            {
                MessageBox.Show("Please select Category and Supplier");
                return;
            }

            // parse decimal
            if (!decimal.TryParse(txtCostPrice_ManagerProduct.Text, out decimal cost))
            {
                MessageBox.Show("Invalid Cost Price");
                return;
            }
            if (!decimal.TryParse(txtSellingPrice_ManagerProduct.Text, out decimal sell))
            {
                MessageBox.Show("Invalid Selling Price");
                return;
            }

            string insert = @"
        INSERT INTO Product (ProductName, CategoryID, SupplierID, CostPrice, SellingPrice)
        VALUES (@name, @cateid, @supid, @cost, @sell)";

            try
            {
                using (SqlConnection conn = new SqlConnection("Server = DOTHUAN2006\\SQLEXPRESS; Database = StoreManagement3; integrated security = true "))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(insert, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", txtName_ManagerProduct.Text);
                        cmd.Parameters.AddWithValue("@cateid", cbCategory_ManagerProduct.SelectedValue);
                        cmd.Parameters.AddWithValue("@supid", cbSupplier_ManagerProduct.SelectedValue);
                        cmd.Parameters.AddWithValue("@cost", cost);
                        cmd.Parameters.AddWithValue("@sell", sell);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Inserted successfully");
                LoadProducts();
                ClearData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dgv_ManagerProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];

                txtID_ManagerProduct.Text = row.Cells["ProductID"].Value.ToString();
                txtName_ManagerProduct.Text = row.Cells["ProductName"].Value.ToString();
                txtCostPrice_ManagerProduct.Text = row.Cells["CostPrice"].Value.ToString();
                txtSellingPrice_ManagerProduct.Text = row.Cells["SellingPrice"].Value.ToString();

                // Gán ID cho ComboBox (ValueMember = CategoryID / SupplierID)
                cbCategory_ManagerProduct.SelectedValue = Convert.ToInt32(row.Cells["CategoryID"].Value);
                cbSupplier_ManagerProduct.SelectedValue = Convert.ToInt32(row.Cells["SupplierID"].Value);
            }
        }

        private void btnUpdate_Product_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to edit?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (!int.TryParse(txtID_ManagerProduct.Text, out int id))
            {
                MessageBox.Show("Please select a valid product to update.");
                return;
            }
            if ((int)cbCategory_ManagerProduct.SelectedValue == 0 || (int)cbSupplier_ManagerProduct.SelectedValue == 0)
            {
                MessageBox.Show("Please select Category and Supplier");
                return;
            }
            if (!decimal.TryParse(txtCostPrice_ManagerProduct.Text, out decimal cost))
            {
                MessageBox.Show("Invalid Cost Price");
                return;
            }
            if (!decimal.TryParse(txtSellingPrice_ManagerProduct.Text, out decimal sell))
            {
                MessageBox.Show("Invalid Selling Price");
                return;
            }

            string update = @"
        UPDATE Product
        SET ProductName = @name,
            CategoryID = @cateid,
            SupplierID = @supid,
            CostPrice = @cost,
            SellingPrice = @sell
        WHERE ProductID = @id";

            try
            {
                using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True;"))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(update, cn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@name", txtName_ManagerProduct.Text);
                        cmd.Parameters.AddWithValue("@cateid", cbCategory_ManagerProduct.SelectedValue);
                        cmd.Parameters.AddWithValue("@supid", cbSupplier_ManagerProduct.SelectedValue);
                        cmd.Parameters.AddWithValue("@cost", cost);
                        cmd.Parameters.AddWithValue("@sell", sell);

                        try
                        {
                            int rows = cmd.ExecuteNonQuery();
                            if (rows == 0)
                                MessageBox.Show("No product updated (ID not found).");
                            else
                                MessageBox.Show("Updated successfully");
                        }
                        catch (SqlException sqlEx)
                        {
                            MessageBox.Show($"SQL Error #{sqlEx.Number}: {sqlEx.Message}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                LoadProducts();
                ClearData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btndelete_Product_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (!int.TryParse(txtID_ManagerProduct.Text, out int id))
            {
                MessageBox.Show("Please select a valid product to delete.");
                return;
            }

            string delete = "DELETE FROM Product WHERE ProductID = @id";

            try
            {
                using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True;"))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(delete, cn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        try
                        {
                            int rows = cmd.ExecuteNonQuery();
                            if (rows == 0)
                                MessageBox.Show("No product deleted (ID not found).");
                            else
                                MessageBox.Show("Deleted successfully");
                        }
                        catch (SqlException sqlEx)
                        {
                            MessageBox.Show($"SQL Error #{sqlEx.Number}: {sqlEx.Message}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                LoadProducts();
                ClearData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnrefresh_Product_Click(object sender, EventArgs e)
        {
            ClearData();
        }
        private void DemoProduct_Load(object sender, EventArgs e)
        {



            LoadProducts();
            GetSuppliers();
            GetCategories();
        }



        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void dgv_ManagerProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];

                txtID_ManagerProduct.Text = row.Cells["ProductID"].Value.ToString();
                txtName_ManagerProduct.Text = row.Cells["ProductName"].Value.ToString();
                txtCostPrice_ManagerProduct.Text = row.Cells["CostPrice"].Value.ToString();
                txtSellingPrice_ManagerProduct.Text = row.Cells["SellingPrice"].Value.ToString();

            }
        }

        private void txtSearch_ManagerProduct_TextChanged(object sender, EventArgs e)
        {
            {
                string kw = txtSearch_ManagerProduct.Text.Trim();
                DataTable dt = new DataTable();

                try
                {
                    string sql = @"
       SELECT p.ProductID, p.ProductName, p.CostPrice, p.SellingPrice, 
              c.CategoryName, s.SupplierName, p.CategoryID, p.SupplierID
       FROM Product p
       LEFT JOIN Category c ON p.CategoryID = c.CategoryID
       LEFT JOIN Supplier s ON p.SupplierID = s.SupplierID";

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;

                        if (!string.IsNullOrEmpty(kw))
                        {
                            if (int.TryParse(kw, out int id))
                            {
                                sql += " WHERE p.ProductID = @id";
                                cmd.Parameters.AddWithValue("@id", id);
                            }
                            else
                            {
                                sql += " WHERE p.ProductName LIKE @k OR ISNULL(c.CategoryName,'') LIKE @k OR ISNULL(s.SupplierName,'') LIKE @k";
                                cmd.Parameters.AddWithValue("@k", "%" + kw + "%");
                            }
                        }

                        cmd.CommandText = sql;
                        new SqlDataAdapter(cmd).Fill(dt);
                    }

                    dgvProducts.DataSource = dt;
                    dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search error: " + ex.Message);
                }
            }
        }

        private void btnLogout_ManagerProduct_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to logout?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide(); // Hide current form
                Form1 login = new Form1();
                login.ShowDialog(); // Display login form
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProducts.Rows.Count == 0)
                {
                    MessageBox.Show("No data available to export.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Save Products Data",
                    FileName = "Products.xlsx"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Products");
                        for (int i = 0; i < dgvProducts.Columns.Count; i++)
                        {
                            worksheet.Cell(1, i + 1).Value = dgvProducts.Columns[i].HeaderText;
                        }
                        for (int i = 0; i < dgvProducts.Rows.Count; i++)
                        {
                            for (int j = 0; j < dgvProducts.Columns.Count; j++)
                            {
                                worksheet.Cell(i + 2, j + 1).Value = dgvProducts.Rows[i].Cells[j].Value?.ToString();
                            }
                        }
                        workbook.SaveAs(saveFileDialog.FileName);
                    }
                    MessageBox.Show("Exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
}
