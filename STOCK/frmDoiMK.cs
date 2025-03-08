using BussinessLayer;
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

namespace STOCK
{
    public partial class frmDoiMK : Form
    {
        private string employeeName; // Tên tài khoản
        private string username;
        public frmDoiMK(string employeeName, string username)
        {
            InitializeComponent();
            this.employeeName = employeeName;
            this.username = username;
        }

        private void frmDoiMK_Load(object sender, EventArgs e)
        {
            lbTaiKhoan.Text = $"{employeeName}";
            lbTenDN.Text = $"{username}";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string currentPassword = txtMK.Text;
            string newPassword = txtMKmoi.Text;
            string confirmPassword = txtXacNhanMK.Text;

            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Mật khẩu mới và xác nhận mật khẩu không khớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra mật khẩu cũ
            using (SqlConnection conn = Connection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MATKHAU FROM tb_TAIKHOAN WHERE TENDANGNHAP = @username";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);

                    object result = cmd.ExecuteScalar();
                    if (result == null || result.ToString() != currentPassword)
                    {
                        MessageBox.Show("Mật khẩu hiện tại không đúng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Cập nhật mật khẩu mới
                    string updateQuery = "UPDATE tb_TAIKHOAN SET MATKHAU = @newPassword WHERE TENDANGNHAP = @username";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@newPassword", newPassword);
                    updateCmd.Parameters.AddWithValue("@username", username);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Đổi mật khẩu thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
