using BussinessLayer;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace STOCK
{
    public partial class frmLogin : Form
    {
        private string userRole;
        private string EmployeeName;
        private int failedLoginAttempts = 0; // Biến đếm số lần đăng nhập sai
        private const int MaxFailedAttempts = 5;

        public frmLogin()
        {
            InitializeComponent();

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
       
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Close the application when label2 is clicked
        }

        // Add properties to store role and employee name


        private void button3_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text; // Sử dụng username từ textbox
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin đăng nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Connect to the database using GetConnection method
            using (SqlConnection conn = Connection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT VAITRO, TENNV FROM tb_TAIKHOAN INNER JOIN tb_NHANVIEN ON tb_TAIKHOAN.MANV = tb_NHANVIEN.MANV WHERE TENDANGNHAP = @username AND MATKHAU = @password";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username); // Sử dụng biến username
                    cmd.Parameters.AddWithValue("@password", password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        string userRole = reader["VAITRO"].ToString();
                        string employeeName = reader["TENNV"].ToString();

                        // Reset failed attempts on successful login
                        failedLoginAttempts = 0;

                        // Mở form chính
                        FormMain formMain = new FormMain(userRole, employeeName, username);
                        formMain.Show();
                        this.Hide();
                    }
                    else
                    {
                        failedLoginAttempts++; // Tăng bộ đếm số lần đăng nhập sai

                        if (failedLoginAttempts >= MaxFailedAttempts)
                        {
                            MessageBox.Show($"Bạn đã nhập sai mật khẩu {MaxFailedAttempts} lần. Hệ thống sẽ tự động thoát.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit(); // Thoát ứng dụng
                        }
                        else
                        {
                            MessageBox.Show($"Sai tên đăng nhập hoặc mật khẩu. Bạn còn {MaxFailedAttempts - failedLoginAttempts} lần thử.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

