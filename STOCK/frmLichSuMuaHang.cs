using BussinessLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace STOCK
{
    public partial class frmLichSuMuaHang : Form
    {
        private string _customerID;

        public frmLichSuMuaHang(string customerID)
        {
            InitializeComponent();
            _customerID = customerID; // Lưu mã khách hàng
        }

        private void frmLichSuMuaHang_Load(object sender, EventArgs e)
        {
            LoadLichSuMuaHang(_customerID); // Tải lịch sử mua hàng khi form được tải
            LoadTenKhachHang(_customerID);
            LoadTongSoDonHang(_customerID);
        }

        private void LoadLichSuMuaHang(string customerID)
        {
            // Truy vấn để lấy lịch sử mua hàng của khách hàng theo mã khách hàng
            string query = @"
        SELECT 
            ROW_NUMBER() OVER (ORDER BY NGAYHD DESC) AS STT,  -- Sắp xếp theo ngày hóa đơn từ mới đến cũ
            MAHD, 
            NGAYHD, 
            SOLUONG, 
            TONGTIEN 
        FROM tb_HOADON 
        WHERE MAKH = @CustomerID 
        ORDER BY NGAYHD DESC";  // Đảm bảo rằng thứ tự là theo ngày gần nhất

            using (SqlConnection conn = Connection.GetConnection()) // Sử dụng phương thức GetConnection để lấy kết nối
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerID); // Truyền mã khách hàng vào truy vấn

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt); // Lấy dữ liệu vào DataTable

                gcLichSu.DataSource = dt; // Đổ dữ liệu vào gvLichSu
            }
        }
        private void LoadTenKhachHang(string customerID)
        {
            string query = "SELECT TENKH FROM tb_KHACHHANG WHERE MAKH = @CustomerID"; // Truy vấn để lấy tên khách hàng

            using (SqlConnection conn = Connection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);

                conn.Open();
                object result = cmd.ExecuteScalar(); // Thực hiện truy vấn và lấy kết quả đầu tiên

                if (result != null)
                {
                    label3.Text = result.ToString(); // Gán giá trị tên khách hàng vào label3
                }
                else
                {
                    label3.Text = "Tên khách hàng không tìm thấy"; // Hiển thị thông báo nếu không tìm thấy
                }
            }
        }
        private void LoadTongSoDonHang(string customerID)
        {
            string countQuery = "SELECT COUNT(MAHD) FROM tb_HOADON WHERE MAKH = @CustomerID";
            string totalQuery = "SELECT SUM(TONGTIEN) FROM tb_HOADON WHERE MAKH = @CustomerID";

            using (SqlConnection conn = Connection.GetConnection())
            {
                conn.Open();

                // Tính tổng số đơn hàng
                SqlCommand countCmd = new SqlCommand(countQuery, conn);
                countCmd.Parameters.AddWithValue("@CustomerID", customerID);
                object countResult = countCmd.ExecuteScalar();
                int totalOrders = countResult != null && countResult != DBNull.Value ? Convert.ToInt32(countResult) : 0;
                label5.Text = totalOrders.ToString(); // Hiển thị số đơn hàng vào label5

                // Tính tổng số tiền thanh toán
                SqlCommand totalCmd = new SqlCommand(totalQuery, conn);
                totalCmd.Parameters.AddWithValue("@CustomerID", customerID);
                object totalResult = totalCmd.ExecuteScalar();
                decimal totalAmount = totalResult != null && totalResult != DBNull.Value ? Convert.ToDecimal(totalResult) : 0;
                label7.Text = totalAmount.ToString("N0"); // Hiển thị tổng tiền vào label7 với định dạng số
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}

