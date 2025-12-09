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
using System.Windows.Forms.DataVisualization.Charting;

namespace StoreManagement
{
    public partial class SalesDashboard : Form
    {
        SqlConnection conn;
        public SalesDashboard()
        {
            InitializeComponent();
            conn = new SqlConnection("Server = DOTHUAN2006\\SQLEXPRESS; Database = StoreManagement3; integrated security = true ");
            MessageBox.Show("hi");
            LoadCustomers();
            LoadOrderDetails();
            LoadOrders();



            GetProducts();
            GetCustomers();
            GetEmployees();
            GetPaymentMethods();

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
                    dgv_ManagerProduct.DataSource = dt;
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



        private void dgv_ManagerOrderDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button26_Click(object sender, EventArgs e)
        {

        }

        private void button27_Click(object sender, EventArgs e)
        {

        }

        private void button28_Click(object sender, EventArgs e)
        {

        }

        private void button29_Click(object sender, EventArgs e)
        {

        }

        private void button30_Click(object sender, EventArgs e)
        {

        }

        private void textBox26_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

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

        private void grvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv_ManagerProduct.Rows[e.RowIndex];

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

        private void txtSearch_ManagerProduct_TextChanged(object sender, EventArgs e)
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

                dgv_ManagerProduct.DataSource = dt;
                dgv_ManagerProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search error: " + ex.Message);
            }
        }

        private void btnLogout_ManagerProduct_Click(object sender, EventArgs e)
        {
            {
                if (MessageBox.Show(this, "Do you want to logout?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Hide(); // Hide current form
                    Form1 login = new Form1();
                    login.ShowDialog(); // Display login form
                    this.Close();
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName_ManagerCustomer.Text) ||
                         string.IsNullOrWhiteSpace(txtPhoneNumber_ManagerCustomer.Text))
            {
                MessageBox.Show("Name and Phone cannot be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string insert = @"INSERT INTO Customer (CustomerName, PhoneNumber, Email, Address)
                      VALUES (@name, @phone, @email, @address)";

            try
            {
                using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(insert, cn))
                    {
                        cmd.Parameters.AddWithValue("@name", txtName_ManagerCustomer.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", txtPhoneNumber_ManagerCustomer.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtEmail_ManagerCustomer.Text.Trim());
                        cmd.Parameters.AddWithValue("@address", txtAddress_ManagerCustomer.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomers();
                ClearCustomerData();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) MessageBox.Show("Phone number already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("SQL Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            ClearCustomerData();
        }
        public void LoadCustomers()
        {
            string sql = "SELECT CustomerID, CustomerName, PhoneNumber, Email, Address FROM Customer";
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                {
                    da.Fill(dt);
                }
            }
            dgv_ManagerCustomer.DataSource = dt;
            dgv_ManagerCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Xóa dữ liệu trong TextBox Customer
        public void ClearCustomerData()
        {
            txtID_ManagerCustomer.Clear();
            txtName_ManagerCustomer.Clear();
            txtPhoneNumber_ManagerCustomer.Clear();
            txtEmail_ManagerCustomer.Clear();
            txtAddress_ManagerCustomer.Clear();
        }


        private void button9_Click(object sender, EventArgs e)
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

        private void button8_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtID_ManagerCustomer.Text, out int id))
            {
                MessageBox.Show("Please select a valid customer to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string delete = "DELETE FROM Customer WHERE CustomerID=@id";
            try
            {
                using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(delete, cn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows == 0) MessageBox.Show("No customer deleted (ID not found).");
                        else MessageBox.Show("Deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                LoadCustomers();
                ClearCustomerData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ClearCustomerData();
        }

        private void SalesDashboard_Load(object sender, EventArgs e)
        {
            LoadProducts();
            GetCategories();
            GetSuppliers();
        }

        private void dgv_ManagerProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv_ManagerProduct.Rows[e.RowIndex];

                txtID_ManagerProduct.Text = row.Cells["ProductID"].Value.ToString();
                txtName_ManagerProduct.Text = row.Cells["ProductName"].Value.ToString();
                txtCostPrice_ManagerProduct.Text = row.Cells["CostPrice"].Value.ToString();
                txtSellingPrice_ManagerProduct.Text = row.Cells["SellingPrice"].Value.ToString();

            }
        }

        private void txtSearch_ManagerCustomer_TextChanged(object sender, EventArgs e)
        {
            string kw = txtSearch_ManagerCustomer.Text.Trim();
            DataTable dt = new DataTable();

            string sql = @"SELECT CustomerID, CustomerName, PhoneNumber, Email, Address
                   FROM Customer
                   WHERE CustomerName LIKE @kw COLLATE SQL_Latin1_General_CP1_CI_AS
                      OR PhoneNumber LIKE @kw COLLATE SQL_Latin1_General_CP1_CI_AS
                      OR Email LIKE @kw COLLATE SQL_Latin1_General_CP1_CI_AS
                      OR Address LIKE @kw COLLATE SQL_Latin1_General_CP1_CI_AS";

            try
            {
                using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        // đảm bảo % + keyword + %
                        cmd.Parameters.AddWithValue("@kw", "%" + kw + "%");

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                dgv_ManagerCustomer.DataSource = dt;
                dgv_ManagerCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            {
                if (MessageBox.Show(this, "Do you want to logout?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Hide(); // Hide current form
                    Form1 login = new Form1();
                    login.ShowDialog(); // Display login form
                    this.Close();
                }
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
        public void LoadOrders()
        {
            string sql = @"
    SELECT o.OrderID, o.OrderDate, c.CustomerName, e.EmployeeName, p.MethodName,
           o.TotalAmount, o.TotalProfit,
           o.CustomerID, o.EmployeeID, o.PaymentMethodID
    FROM [Order] o
    INNER JOIN Customer c ON o.CustomerID = c.CustomerID
    INNER JOIN Employee e ON o.EmployeeID = e.EmployeeID
    INNER JOIN PaymentMethod p ON o.PaymentMethodID = p.PaymentMethodID";

            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                {
                    da.Fill(dt);
                }
            }

            dgv_ManagerOrder.DataSource = dt;
            dgv_ManagerOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        // Bind ComboBox Customer, Employee, PaymentMethod
        public void GetCustomers()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
            {
                cn.Open();
                new SqlDataAdapter("SELECT CustomerID, CustomerName FROM Customer", cn).Fill(dt);
            }

            DataRow row = dt.NewRow();
            row["CustomerID"] = 0;
            row["CustomerName"] = "-- Select Customer --";
            dt.Rows.InsertAt(row, 0);

            cbCustomer_ManagerOrder.DataSource = dt;
            cbCustomer_ManagerOrder.DisplayMember = "CustomerName";
            cbCustomer_ManagerOrder.ValueMember = "CustomerID";
            cbCustomer_ManagerOrder.SelectedIndex = 0;
        }

        public void GetEmployees()
        {
            DataTable dt = new DataTable();
            new SqlDataAdapter("SELECT EmployeeID, EmployeeName FROM Employee", conn).Fill(dt);

            DataRow row = dt.NewRow();
            row["EmployeeID"] = 0;
            row["EmployeeName"] = "-- Select Employee --";
            dt.Rows.InsertAt(row, 0);

            cbEmployee_ManagerOrder.DataSource = dt;
            cbEmployee_ManagerOrder.DisplayMember = "EmployeeName";
            cbEmployee_ManagerOrder.ValueMember = "EmployeeID";
            cbEmployee_ManagerOrder.SelectedIndex = 0;
        }
        public void GetPaymentMethods()
        {
            DataTable dt = new DataTable();
            new SqlDataAdapter("SELECT PaymentMethodID, MethodName FROM PaymentMethod", conn).Fill(dt);

            DataRow row = dt.NewRow();
            row["PaymentMethodID"] = 0;
            row["MethodName"] = "-- Select Payment Method --";
            dt.Rows.InsertAt(row, 0);

            cbPaymentMethod_ManagerOrder.DataSource = dt;
            cbPaymentMethod_ManagerOrder.DisplayMember = "MethodName";
            cbPaymentMethod_ManagerOrder.ValueMember = "PaymentMethodID";
            cbPaymentMethod_ManagerOrder.SelectedIndex = 0;
        }

        // Clear TextBoxes / ComboBox
        public void ClearOrderData()
        {
            txtID_ManagerOrder.Clear();
            txtDate_ManagerOrder.Clear();
            cbCustomer_ManagerOrder.SelectedIndex = 0;
            cbEmployee_ManagerOrder.SelectedIndex = 0;
            cbPaymentMethod_ManagerOrder.SelectedIndex = 0;
            txtTotalAmount_ManagerOrder.Clear();
            txtTotalProfit_ManagerOrder.Clear();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if ((int)cbCustomer_ManagerOrder.SelectedValue == 0 ||
              (int)cbEmployee_ManagerOrder.SelectedValue == 0 ||
              (int)cbPaymentMethod_ManagerOrder.SelectedValue == 0)
            {
                MessageBox.Show("Please select Customer, Employee, and Payment Method");
                return;
            }

            if (!DateTime.TryParse(txtDate_ManagerOrder.Text, out DateTime orderDate))
            {
                MessageBox.Show("Invalid Order Date");
                return;
            }

            decimal totalAmount = 0;
            decimal totalProfit = 0;
            decimal.TryParse(txtTotalAmount_ManagerOrder.Text, out totalAmount);
            decimal.TryParse(txtTotalProfit_ManagerOrder.Text, out totalProfit);

            string insert = @"
        INSERT INTO [Order] (OrderDate, CustomerID, EmployeeID, PaymentMethodID, TotalAmount, TotalProfit)
        VALUES (@date, @customerId, @employeeId, @paymentId, @totalAmount, @totalProfit)";

            using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(insert, cn))
                {
                    cmd.Parameters.AddWithValue("@date", orderDate);
                    cmd.Parameters.AddWithValue("@customerId", cbCustomer_ManagerOrder.SelectedValue);
                    cmd.Parameters.AddWithValue("@employeeId", cbEmployee_ManagerOrder.SelectedValue);
                    cmd.Parameters.AddWithValue("@paymentId", cbPaymentMethod_ManagerOrder.SelectedValue);
                    cmd.Parameters.AddWithValue("@totalAmount", totalAmount);
                    cmd.Parameters.AddWithValue("@totalProfit", totalProfit);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Inserted successfully");
            LoadOrders();
            ClearOrderData();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtID_ManagerOrder.Text, out int orderId))
            {
                MessageBox.Show("Invalid Order ID");
                return;
            }

            if ((int)cbCustomer_ManagerOrder.SelectedValue == 0 ||
                (int)cbEmployee_ManagerOrder.SelectedValue == 0 ||
                (int)cbPaymentMethod_ManagerOrder.SelectedValue == 0)
            {
                MessageBox.Show("Please select Customer, Employee, and Payment Method");
                return;
            }

            if (!DateTime.TryParse(txtDate_ManagerOrder.Text, out DateTime orderDate))
            {
                MessageBox.Show("Invalid Order Date");
                return;
            }

            decimal totalAmount = 0;
            decimal totalProfit = 0;
            decimal.TryParse(txtTotalAmount_ManagerOrder.Text, out totalAmount);
            decimal.TryParse(txtTotalProfit_ManagerOrder.Text, out totalProfit);

            string update = @"
        UPDATE [Order]
        SET OrderDate=@date, CustomerID=@customerId, EmployeeID=@employeeId,
            PaymentMethodID=@paymentId, TotalAmount=@totalAmount, TotalProfit=@totalProfit
        WHERE OrderID=@orderId";

            using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(update, cn))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.Parameters.AddWithValue("@date", orderDate);
                    cmd.Parameters.AddWithValue("@customerId", cbCustomer_ManagerOrder.SelectedValue);
                    cmd.Parameters.AddWithValue("@employeeId", cbEmployee_ManagerOrder.SelectedValue);
                    cmd.Parameters.AddWithValue("@paymentId", cbPaymentMethod_ManagerOrder.SelectedValue);
                    cmd.Parameters.AddWithValue("@totalAmount", totalAmount);
                    cmd.Parameters.AddWithValue("@totalProfit", totalProfit);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Updated successfully");
            LoadOrders();
            ClearOrderData();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtID_ManagerOrder.Text, out int orderId))
            {
                MessageBox.Show("Invalid Order ID");
                return;
            }

            string delete = "DELETE FROM [Order] WHERE OrderID=@orderId";

            using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(delete, cn))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Deleted successfully");
            LoadOrders();
            ClearOrderData();
        }

        // Refresh
        private void btnRefresh_ManagerOrder_Click(object sender, EventArgs e)
        {
            ClearOrderData();
            LoadOrders();
        }

        private void dgv_ManagerOrder_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv_ManagerOrder.Rows[e.RowIndex];

                txtID_ManagerOrder.Text = row.Cells["OrderID"].Value?.ToString();
                txtDate_ManagerOrder.Text = row.Cells["OrderDate"].Value?.ToString();
                cbCustomer_ManagerOrder.SelectedValue = Convert.ToInt32(row.Cells["CustomerID"].Value);
                cbEmployee_ManagerOrder.SelectedValue = Convert.ToInt32(row.Cells["EmployeeID"].Value);
                cbPaymentMethod_ManagerOrder.SelectedValue = Convert.ToInt32(row.Cells["PaymentMethodID"].Value);
                txtTotalAmount_ManagerOrder.Text = row.Cells["TotalAmount"].Value?.ToString();
                txtTotalProfit_ManagerOrder.Text = row.Cells["TotalProfit"].Value?.ToString();
            }
        }

        private void txtSearch_ManagerOrder_TextChanged(object sender, EventArgs e)
        {
            string kw = txtSearch_ManagerOrder.Text.Trim();
            DataTable dt = new DataTable();
            string sql = @"
    SELECT o.OrderID, o.OrderDate, c.CustomerName, e.EmployeeName, p.MethodName,
           o.TotalAmount, o.TotalProfit,
           o.CustomerID, o.EmployeeID, o.PaymentMethodID
    FROM [Order] o
    INNER JOIN Customer c ON o.CustomerID = c.CustomerID
    INNER JOIN Employee e ON o.EmployeeID = e.EmployeeID
    INNER JOIN PaymentMethod p ON o.PaymentMethodID = p.PaymentMethodID
    WHERE c.CustomerName LIKE @kw OR e.EmployeeName LIKE @kw OR p.MethodName LIKE @kw";

            using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@kw", "%" + kw + "%");
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            dgv_ManagerOrder.DataSource = dt;
            dgv_ManagerOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }
        public void LoadOrderDetails()
        {
            string sql = @"
        SELECT od.OrderID, od.ProductID, p.ProductName, od.Quantity, od.UnitPrice,
               od.LineTotal, od.LineProfit
        FROM OrderDetail od
        INNER JOIN Product p ON od.ProductID = p.ProductID";

            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                {
                    da.Fill(dt);
                }
            }

            dgv_ManagerOrderDetail.DataSource = dt;
            dgv_ManagerOrderDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public void GetProducts()
        {
            DataTable dt = new DataTable();
            new SqlDataAdapter("SELECT ProductID, ProductName FROM Product", conn).Fill(dt);

            DataRow row = dt.NewRow();
            row["ProductID"] = 0;
            row["ProductName"] = "-- Select Product --";
            dt.Rows.InsertAt(row, 0);

            cbProduct_ManagerOrderDetail.DataSource = dt;
            cbProduct_ManagerOrderDetail.DisplayMember = "ProductName";
            cbProduct_ManagerOrderDetail.ValueMember = "ProductID";
            cbProduct_ManagerOrderDetail.SelectedIndex = 0;
        }

        public void ClearOrderDetailData()
        {
            txtID_ManagerOrderDetail.Clear();
            cbProduct_ManagerOrderDetail.SelectedValue = 0;
            txtQuantity_ManagerOrderDetail.Clear();
            txtUnitPrice_ManagerOrderDetail.Clear();
            txtLineTotal_ManagerOrderDetail.Clear();
            txtLineProfit_ManagerOrderDetail.Clear();
        }

        private void button30_Click_1(object sender, EventArgs e)
        {
            if (cbProduct_ManagerOrderDetail.SelectedValue == null || Convert.ToInt32(cbProduct_ManagerOrderDetail.SelectedValue) == 0)
            {
                MessageBox.Show("Please select a Product");
                return;
            }

            if (!int.TryParse(txtQuantity_ManagerOrderDetail.Text, out int quantity) ||
                !decimal.TryParse(txtUnitPrice_ManagerOrderDetail.Text, out decimal unitPrice))
            {
                MessageBox.Show("Invalid Quantity or UnitPrice");
                return;
            }

            decimal lineTotal = quantity * unitPrice;
            decimal lineProfit = lineTotal * 0.2m; // Giả sử lợi nhuận 20% nếu bạn muốn tính tự động

            string insert = @"
        INSERT INTO OrderDetail (OrderID, ProductID, Quantity, UnitPrice, LineTotal, LineProfit)
        VALUES (@orderId, @productId, @quantity, @unitPrice, @lineTotal, @lineProfit)";

            using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(insert, cn))
                {
                    cmd.Parameters.AddWithValue("@orderId", txtID_ManagerOrderDetail.Text);
                    cmd.Parameters.AddWithValue("@productId", cbProduct_ManagerOrderDetail.SelectedValue);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@unitPrice", unitPrice);
                    cmd.Parameters.AddWithValue("@lineTotal", lineTotal);
                    cmd.Parameters.AddWithValue("@lineProfit", lineProfit);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Inserted successfully");
            LoadOrderDetails();
            ClearOrderDetailData();
        }

        private void button29_Click_1(object sender, EventArgs e)
        {
            if (!int.TryParse(txtID_ManagerOrderDetail.Text, out int orderId))
            {
                MessageBox.Show("Invalid Order ID");
                return;
            }

            if ((int)cbProduct_ManagerOrderDetail.SelectedValue == 0)
            {
                MessageBox.Show("Please select a Product");
                return;
            }
            if (!int.TryParse(txtQuantity_ManagerOrderDetail.Text, out int quantity) ||
                !decimal.TryParse(txtUnitPrice_ManagerOrderDetail.Text, out decimal unitPrice))
            {
                MessageBox.Show("Invalid Quantity or UnitPrice");
                return;
            }

            decimal lineTotal = quantity * unitPrice;
            decimal lineProfit = lineTotal * 0.2m;

            string update = @"
        UPDATE OrderDetail
        SET Quantity=@quantity, UnitPrice=@unitPrice, LineTotal=@lineTotal, LineProfit=@lineProfit
        WHERE OrderID=@orderId AND ProductID=@productId";

            using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(update, cn))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.Parameters.AddWithValue("@productId", cbProduct_ManagerOrderDetail.SelectedValue);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@unitPrice", unitPrice);
                    cmd.Parameters.AddWithValue("@lineTotal", lineTotal);
                    cmd.Parameters.AddWithValue("@lineProfit", lineProfit);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Updated successfully");
            LoadOrderDetails();
            ClearOrderDetailData();
        }

        private void button28_Click_1(object sender, EventArgs e)
        {
            if (!int.TryParse(txtID_ManagerOrderDetail.Text, out int orderId))
            {
                MessageBox.Show("Invalid Order ID");
                return;
            }

            string delete = "DELETE FROM OrderDetail WHERE OrderID=@orderId AND ProductID=@productId";

            using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(delete, cn))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.Parameters.AddWithValue("@productId", cbProduct_ManagerOrderDetail.SelectedValue);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Deleted successfully");
            LoadOrderDetails();
            ClearOrderDetailData();
        }

        private void button27_Click_1(object sender, EventArgs e)
        {
            ClearOrderDetailData();
            LoadOrderDetails();
        }

        private void dgv_ManagerOrderDetail_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv_ManagerOrderDetail.Rows[e.RowIndex];

                txtID_ManagerOrderDetail.Text = row.Cells["OrderID"].Value?.ToString();
                cbProduct_ManagerOrderDetail.SelectedValue = row.Cells["ProductID"].Value != DBNull.Value
                    ? Convert.ToInt32(row.Cells["ProductID"].Value)
                    : 0;
                txtQuantity_ManagerOrderDetail.Text = row.Cells["Quantity"].Value?.ToString();
                txtUnitPrice_ManagerOrderDetail.Text = row.Cells["UnitPrice"].Value?.ToString();
                txtLineTotal_ManagerOrderDetail.Text = row.Cells["LineTotal"].Value?.ToString();
                txtLineProfit_ManagerOrderDetail.Text = row.Cells["LineProfit"].Value?.ToString();
            }
        }

        private void txtSearch_ManagerOrderDetail_TextChanged(object sender, EventArgs e)
        {
            string kw = txtSearch_ManagerOrderDetail.Text.Trim();
            DataTable dt = new DataTable();
            string sql = @"
        SELECT od.OrderID, od.ProductID, p.ProductName, od.Quantity, od.UnitPrice,
               od.LineTotal, od.LineProfit
        FROM OrderDetail od
        INNER JOIN Product p ON od.ProductID = p.ProductID
        WHERE p.ProductName LIKE @kw";

            using (SqlConnection cn = new SqlConnection(@"Server=DOTHUAN2006\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@kw", "%" + kw + "%");
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            dgv_ManagerOrderDetail.DataSource = dt;
            dgv_ManagerOrderDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button26_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to logout?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide(); // Hide current form
                Form1 login = new Form1();
                login.ShowDialog(); // Display login form
                this.Close();
            }
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }
        private void LoadStatistics(string filterType, int role = 0)
        {
            try
            {
                DateTime fromDateOnly = dtpFromDate.Value.Date;
                DateTime toDateOnly = dtpToDate.Value.Date; // inclusive date

                string sql = "";

                if (filterType == "Employee")
                {
                    sql = @"
SELECT e.EmployeeName, SUM(o.TotalAmount) AS TotalRevenue
FROM [Order] o
JOIN Employee e ON o.EmployeeID = e.EmployeeID
WHERE o.OrderDate >= @FromDate AND o.OrderDate < DATEADD(day,1,@ToDate)
";
                    if (role != 0)
                    {
                        sql += " AND e.AuthorityLevel = @RoleID";
                    }
                    sql += @"
GROUP BY e.EmployeeName
ORDER BY TotalRevenue DESC";
                }
                else if (filterType == "Customer")
                {
                    sql = @"
SELECT c.CustomerName, SUM(o.TotalAmount) AS TotalSpent
FROM [Order] o
JOIN Customer c ON o.CustomerID = c.CustomerID
WHERE o.OrderDate >= @FromDate AND o.OrderDate < DATEADD(day,1,@ToDate)
GROUP BY c.CustomerName
ORDER BY TotalSpent DESC";
                }
                else if (filterType == "RevenueOverTime")
                {
                    sql = @"
SELECT CAST(o.OrderDate AS DATE) AS OrderDay, SUM(o.TotalAmount) AS TotalRevenue
FROM [Order] o
WHERE o.OrderDate >= @FromDate AND o.OrderDate < DATEADD(day,1,@ToDate)
GROUP BY CAST(o.OrderDate AS DATE)
ORDER BY OrderDay";
                }
                else
                {
                    MessageBox.Show("Unknown filter type.");
                    return;
                }

                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FromDate", fromDateOnly);
                    cmd.Parameters.AddWithValue("@ToDate", toDateOnly);
                    if (role != 0) cmd.Parameters.AddWithValue("@RoleID", role);

                    new SqlDataAdapter(cmd).Fill(dt);
                }

                // Debug: show count and first rows if no data
                if (dt.Rows.Count == 0)
                {
                    string dbg = $"Query returned 0 rows.\nFrom: {fromDateOnly:yyyy-MM-dd}  To: {toDateOnly:yyyy-MM-dd}\n" +
                                 "Please check:\n- Are there Order rows in that date range?\n- Is connection string pointing to correct DB?";
                    MessageBox.Show(dbg, "No data for this period", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Prepare chart
                chartStatistics.Series.Clear();
                chartStatistics.Titles.Clear();
                chartStatistics.ChartAreas.Clear();
                chartStatistics.ChartAreas.Add(new ChartArea("MainArea"));

                decimal total = Convert.ToDecimal(dt.Compute("SUM(" + dt.Columns[1].ColumnName + ")", ""));

                Title totalTitle = new Title
                {
                    Text = "TỔNG: " + total.ToString("#,##0 VND"),
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.DarkRed,
                    Docking = Docking.Top,
                    Alignment = ContentAlignment.TopRight
                };
                chartStatistics.Titles.Add(totalTitle);

                Series series = new Series(filterType)
                {
                    IsValueShownAsLabel = true,
                    ToolTip = "#VALX: #VALY",
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    ChartArea = "MainArea"
                };

                Random rand = new Random();

                if (filterType == "RevenueOverTime")
                {
                    series.ChartType = SeriesChartType.Line;
                    series.BorderWidth = 3;
                    series.MarkerStyle = MarkerStyle.Circle;
                    series.MarkerSize = 6;
                    series.XValueMember = dt.Columns[0].ColumnName;
                    series.YValueMembers = dt.Columns[1].ColumnName;

                    chartStatistics.Series.Add(series);
                    chartStatistics.DataSource = dt;
                    chartStatistics.DataBind();
                }
                else
                {
                    series.ChartType = SeriesChartType.Pie;
                    series["PieLabelStyle"] = "Outside";
                    series["PieLineColor"] = "Black";

                    foreach (DataRow row in dt.Rows)
                    {
                        int idx = series.Points.AddXY(row[0], row[1]);
                        series.Points[idx].Color = Color.FromArgb(rand.Next(50, 256), rand.Next(50, 256), rand.Next(50, 256));
                        decimal value = Convert.ToDecimal(row[1]);
                        series.Points[idx].Label = $"{row[0]} ({(value / total):P1})";
                    }

                    chartStatistics.Series.Add(series);
                    if (chartStatistics.Legends.Count == 0) chartStatistics.Legends.Add(new Legend("DefaultLegend"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading statistics: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button41_Click(object sender, EventArgs e)
        {
            LoadStatistics("Employee");

        }

        private void button42_Click(object sender, EventArgs e)
        {
            LoadStatistics("Customer");

        }

        private void button43_Click(object sender, EventArgs e)
        {
            LoadStatistics("RevenueOverTime");

        }

        private void button44_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to logout?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide(); // Hide current form
                Form1 login = new Form1();
                login.ShowDialog(); // Display login form
                this.Close();
            }
        }
    }
}


