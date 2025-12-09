using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace StoreManagement
{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection("Server=DOTHUAN2006\\SQLEXPRESS;Database=StoreManagement3;Integrated Security=True");

            txtPassword.UseSystemPasswordChar = true;

            // Tab to move to the next line
            // Tab order
            txtUserName.TabIndex = 0;
            txtPassword.TabIndex = 1;
            btnLogin.TabIndex = 2;
            btnExit.TabIndex = 3;

            // Events
            txtUserName.KeyDown += TxtUserName_KeyDown;
            txtPassword.KeyDown += txtPassword_KeyDown;
            txtPassword.UseSystemPasswordChar = true;
            this.MouseDown += MyForm_MouseDown;

        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void MyForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0xA1, 0x2, 0); // Kích hoạt kéo form
            }
        }
        private void TxtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                txtPassword.Focus();
                e.Handled = true;
                e.SuppressKeyPress = true; // Block the "ding" sound
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                MessageBox.Show("Connect successful");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection failed: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                MessageBox.Show("Connect successful");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection failed: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                conn.Open();

                string query = "SELECT EmployeeID, EmployeeName, AuthorityLevel FROM Employee " +
                               "WHERE Username = @username AND PasswordHash = @password AND IsActive = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                    cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = password; // Nếu hash, cần hash trước khi so sánh

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int employeeID = Convert.ToInt32(reader["EmployeeID"]); // Lấy ID nhân viên
                        string employeeName = reader["EmployeeName"].ToString();
                        int authority = Convert.ToInt32(reader["AuthorityLevel"]);

                        // **Gán EmployeeID vào Session**
                        Session.CurrentEmployeeID = employeeID;

                        MessageBox.Show($"Welcome {employeeName}!", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Hide();

                        // Phân quyền
                        if (authority == 1)
                        {
                            Form2 adminDashboard = new Form2();
                            adminDashboard.ShowDialog();
                        }
                        else if (authority == 2)
                        {
                            SalesDashboard saleDashboard = new SalesDashboard();
                            saleDashboard.ShowDialog();
                        }
                        else if (authority == 3)
                        {
                            WarehouseDashboard warehouseDashboard = new WarehouseDashboard();
                            warehouseDashboard.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("This account has no valid role.", "Error",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public static class Session
        {
            public static int CurrentEmployeeID { get; set; }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to exit?", "Question",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Login_Load_1(object sender, EventArgs e)
        {

        }

        private void txtUserName_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                txtPassword.Focus();
                e.Handled = true;
                e.SuppressKeyPress = true; // Block the "ding" sound
            }
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to exit?", "Question",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0xA1, 0x2, 0); // Kích hoạt kéo form
            }
        }
    }
}

    

