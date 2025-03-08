using BussinessLayer;
using DevExpress.XtraCharts;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace STOCK
{
    public partial class uc_Home : UserControl
    {

        private Timer refreshTimer;
        public uc_Home()
        {
            InitializeComponent();
            InitializeTimer();
        }
        private void uc_Home_Load(object sender, EventArgs e)
        {
            LoadChartDT(); // Gọi phương thức để tải dữ liệu biểu đồ khi UserControl được tải
            LoadDoanhThuThangHienTai();
            LoadChartHH();
            LoadTongSoDonHang();
            LoadChartNhomHH();
            LoadLoiNhuanThangHienTai();
        }
        private void InitializeTimer()
        {
            refreshTimer = new Timer();
            refreshTimer.Interval = 5000; // Đặt khoảng thời gian là 5 giây (5000 ms)
            refreshTimer.Tick += new EventHandler(RefreshChart); // Gán sự kiện Tick
            refreshTimer.Start(); // Bắt đầu Timer
        }

        private void RefreshChart(object sender, EventArgs e)
        {
            LoadChartDT(); // Gọi phương thức cập nhật dữ liệu biểu đồ
            LoadDoanhThuThangHienTai();
            LoadChartHH();
            LoadTongSoDonHang();
            LoadChartNhomHH();
            LoadLoiNhuanThangHienTai();
        }
        public void RefreshData()
        {
            LoadChartDT();  // Gọi lại hàm hiển thị dữ liệu
            LoadDoanhThuThangHienTai();
            LoadChartHH();
            LoadTongSoDonHang();
            LoadChartNhomHH();
            LoadLoiNhuanThangHienTai();
        }
        private void LoadLoiNhuanThangHienTai()
        {
            // Sử dụng phương thức Connection.GetConnection() để lấy kết nối cơ sở dữ liệu
            using (SqlConnection connection = Connection.GetConnection())
            {
                string query = "SELECT LoiNhuan FROM dbo.TinhDoanhThuVaLoiNhuanTheoThang() WHERE Thang = MONTH(GETDATE());";
                // Truy vấn để lấy lợi nhuận tháng hiện tại
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Sử dụng Convert.ToDecimal để tránh lỗi cast
                        decimal loiNhuanThangHienTai = Convert.ToDecimal(reader[0]); // Lấy giá trị lợi nhuận từ cột đầu tiên
                        label7.Text = loiNhuanThangHienTai.ToString("N0"); // Gán giá trị cho Label7 và định dạng thành số nguyên
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }



        private void LoadChartDT()
        {
            // Sử dụng phương thức Connection.GetConnection() để lấy kết nối cơ sở dữ liệu
            using (SqlConnection connection = Connection.GetConnection())
            {
                try
                {
                    connection.Open();
                     string query = "SELECT * FROM dbo.TinhDoanhThuVaLoiNhuanTheoThang();";  // Truy vấn SQL để lấy doanh thu và lợi nhuận theo tháng

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt); // Đổ dữ liệu vào DataTable

                        // Xóa các series cũ trong biểu đồ
                        chartDT.Series.Clear();

                        // Tạo series cho Doanh Thu
                        Series seriesDoanhThu = new Series("Doanh Thu", ViewType.Bar);
                        // Tạo series cho Lợi Nhuận
                        Series seriesLoiNhuan = new Series("Lợi Nhuận", ViewType.Line); // Sử dụng loại Line cho Lợi Nhuận

                        foreach (DataRow row in dt.Rows)
                        {
                            int thang = Convert.ToInt32(row["Thang"]); // Lấy tháng
                            decimal doanhThu = Convert.ToDecimal(row["DoanhThu"]); // Lấy doanh thu
                            decimal loiNhuan = Convert.ToDecimal(row["LoiNhuan"]); // Lấy lợi nhuận

                            // Thêm điểm vào series doanh thu
                            seriesDoanhThu.Points.Add(new SeriesPoint(thang, doanhThu));
                            // Thêm điểm vào series lợi nhuận
                            seriesLoiNhuan.Points.Add(new SeriesPoint(thang, loiNhuan));
                        }

                        // Thêm series vào biểu đồ
                        chartDT.Series.Add(seriesDoanhThu);
                        chartDT.Series.Add(seriesLoiNhuan);

                        // Thiết lập chú thích cho các series
                        seriesDoanhThu.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                        seriesLoiNhuan.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

                        // Thiết lập tiêu đề cho biểu đồ
                        //chartDT.Titles.Clear();
                        //ChartTitle title = new ChartTitle
                        //{
                        //    Text = "DOANH THU VÀ LỢI NHUẬN THEO THÁNG", // Full chữ in
                        //    Font = new Font("Arial", 14, FontStyle.Bold), // Kiểu chữ, kích thước và độ dày
                  
                        //};
                        //chartDT.Titles.Add(title);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}");
                }
            }
        }

        public void LoadTongSoDonHang()
        {
            // Sử dụng Connection.GetConnection() để lấy kết nối
            using (SqlConnection connection = Connection.GetConnection())
            {
                string query = "SELECT * FROM dbo.TinhTongDonHangThangHienTai();";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int tongSoDonHang = reader.GetInt32(0); // Lấy giá trị tổng số đơn hàng
                        label4.Text = tongSoDonHang.ToString(); // Gán giá trị cho Label4
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void LoadDoanhThuThangHienTai()
        {
            using (SqlConnection connection = Connection.GetConnection())
            {
                string query = "SELECT DoanhThu FROM dbo.TinhDoanhThuVaLoiNhuanTheoThang() WHERE Thang = MONTH(GETDATE());"; // Truy vấn để lấy doanh thu tháng hiện tại
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Kiểm tra xem giá trị có phải NULL không
                        if (!reader.IsDBNull(0)) // Nếu không phải NULL
                        {
                            // Sử dụng Convert để chuyển đổi giá trị, tùy thuộc vào kiểu dữ liệu của bạn
                            decimal doanhThuThangHienTai = Convert.ToDecimal(reader[0]); // Lấy giá trị doanh thu
                            label3.Text = doanhThuThangHienTai.ToString("N0"); // Gán giá trị cho Label3 và định dạng thành số nguyên
                        }
                        else // Nếu là NULL
                        {
                            label3.Text = "0"; // Hoặc bạn có thể hiển thị một thông báo khác
                        }
                    }
                    else
                    {
                        label3.Text = "0"; // Nếu không có kết quả nào, gán giá trị mặc định
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }



        private void LoadChartHH()
        {
            // Sử dụng phương thức Connection.GetConnection() để lấy kết nối cơ sở dữ liệu
            using (SqlConnection connection = Connection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM dbo.fn_Top5SanPhamBanChay();"; // Truy vấn SQL để lấy dữ liệu sản phẩm bán chạy nhất

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt); // Đổ dữ liệu vào DataTable

                        // Xóa các series cũ trong biểu đồ
                        chartHH.Series.Clear();

                        // Tạo và thêm series vào biểu đồ cột
                        Series series = new Series("Sản Phẩm Bán Chạy", ViewType.Bar); // Đặt loại là Bar cho biểu đồ cột ngang

                        foreach (DataRow row in dt.Rows)
                        {
                            string tenHH = row["TENHH"].ToString(); // Lấy tên hàng hóa
                            int soLuong = Convert.ToInt32(row["SOLUONG"]); // Lấy số lượng bán được

                            // Thêm dữ liệu vào series
                            series.Points.Add(new SeriesPoint(tenHH, soLuong));
                        }


                        // Thêm series vào biểu đồ
                        chartHH.Series.Add(series);

                        //chartHH.Titles.Clear(); // Xóa các tiêu đề cũ
                        //chartHH.Titles.Add(new ChartTitle { Text = "Top 5 Sản Phẩm Bán Chạy" }); // Tiêu đề biểu đồ
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}");
                }
            }
        }
        private void LoadChartNhomHH()
        {
            // Sử dụng phương thức Connection.GetConnection() để lấy kết nối cơ sở dữ liệu
            using (SqlConnection connection = Connection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM dbo.TinhDoanhThuTheoNhomHangHienTai();"; // Truy vấn SQL lấy dữ liệu doanh thu theo nhóm hàng hóa

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt); // Đổ dữ liệu vào DataTable

                        // Xóa các series cũ trong biểu đồ
                        chartNHOMHH.Series.Clear();

                        // Tạo và thêm series vào biểu đồ tròn (Pie)
                        Series series = new Series("Doanh Thu Nhóm Hàng", ViewType.Pie);

                        foreach (DataRow row in dt.Rows)
                        {
                            string tenNhom = row["TENNHOM"].ToString(); // Lấy tên nhóm hàng hóa
                            decimal doanhThu = Convert.ToDecimal(row["DOANHTHU"]); // Lấy doanh thu

                            // Thêm dữ liệu vào series (Pie chart)
                            series.Points.Add(new SeriesPoint(tenNhom, doanhThu));
                        }

                        // Thêm series vào biểu đồ
                        chartNHOMHH.Series.Add(series);

                        // Ẩn chú thích mặc định
                        series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

                        // Định dạng nhãn hiển thị phần trăm
                        PieSeriesLabel label = (PieSeriesLabel)series.Label;
                        label.Position = PieSeriesLabelPosition.Outside;
                        label.TextPattern = "{A}: {VP:p0}"; // Hiển thị tên nhóm và phần trăm doanh thu

                        // Tùy chỉnh sự xuất hiện của biểu đồ
                        PieSeriesView pieView = (PieSeriesView)series.View;
                        //pieView.Titles.Add(new SeriesTitle());
                        //pieView.Titles[0].Text = "Doanh Thu Theo Nhóm Hàng"; // Tiêu đề của biểu đồ
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}");
                }
            }
        }

        private void chartNV_Click(object sender, EventArgs e)
        {


        }

        private void chartHH_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
