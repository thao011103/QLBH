using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

using iTextParagraph = iTextSharp.text.Paragraph;
using iTextFont = iTextSharp.text.Font;
using iTextElement = iTextSharp.text.Element;
using iTextBaseColor = iTextSharp.text.BaseColor;
using BussinessLayer;
using DevExpress.XtraGrid.Views.Grid;

namespace STOCK
{
    public partial class uc_BaoCao2 : UserControl
    {
        public string EmployeeName { get; set; }


        private readonly string robotoRegularPath = @"C:\Users\ADMIN\Downloads\Roboto\Roboto-Regular.ttf";
        private readonly string robotoBoldPath = @"C:\Users\ADMIN\Downloads\Roboto\Roboto-Bold.ttf";
        public uc_BaoCao2(string employeeName)
        {
            InitializeComponent();
            EmployeeName = employeeName;
            SetDefaultDateRange();
            LoadData();
        }

        private void SetDefaultDateRange()
        {
            DateTime now = DateTime.Now;
            dtTuNgay.Value = new DateTime(now.Year, now.Month, 1);
            dtDenNgay.Value = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        }
      
        private void LoadData()
        {
            if (cboLoaiBC.SelectedItem == null)
            {
                // Nếu không có giá trị đã chọn, chọn mặc định (ví dụ: "Khách hàng")
                cboLoaiBC.SelectedIndex = 0; // Hoặc chọn mục phù hợp nếu có
            }

            DateTime startDate = dtTuNgay.Value;
            DateTime endDate = dtDenNgay.Value;

            // Lấy giá trị loại báo cáo từ cboLoaiBC
            string selectedReportType = cboLoaiBC.SelectedItem.ToString();  // Lấy giá trị từ ComboBox
            gcBaoCao.Refresh();

            string groupBy = selectedReportType;  // Ví dụ: "Khách hàng", "Nhân viên", v.v.
            LoadDataForThang(startDate, endDate, groupBy);  // Gọi phương thức load cho Tháng


        }


        private void LoadDataForThang(DateTime startDate, DateTime endDate, string groupBy)
        {
            string sqlQuery = "EXEC [dbo].[GetDoanhThuTheoThang] @StartDate, @EndDate, @GroupBy";

            try
            {
                SqlConnection connection = Connection.GetConnection();
                if (connection == null)
                {
                    throw new NullReferenceException("Không thể tạo kết nối đến cơ sở dữ liệu.");
                }

                using (connection)
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);
                        command.Parameters.AddWithValue("@GroupBy", groupBy);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable == null || dataTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Không có dữ liệu để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        gcBaoCao.DataSource = dataTable;
                        gcBaoCao.Refresh();

                        GridView gvBaoCao = gcBaoCao.MainView as GridView;
                        if (gvBaoCao != null)
                        {
                            gvBaoCao.OptionsView.ColumnAutoWidth = false; // Tắt tự động điều chỉnh cột

                            int totalWidth = gcBaoCao.Width - 20; // Chiều rộng của GridControl (trừ đi padding)
                            int columnCount = gvBaoCao.Columns.Count; // Tổng số cột

               
                            float[] columnRatios = { 0.2f, 0.3f, 0.2f, 0.2f }; // Tỷ lệ phần trăm theo cột

                            for (int i = 0; i < gvBaoCao.Columns.Count; i++)
                            {
                                // Thiết lập độ rộng cột dựa trên tỷ lệ
                                if (i < columnRatios.Length)
                                {
                                    gvBaoCao.Columns[i].Width = (int)(totalWidth * columnRatios[i]);
                                }
                                else
                                {
                                    gvBaoCao.Columns[i].Width = totalWidth / columnCount; // Cột dư được chia đều
                                }

                                // Căn giữa tên cột
                                gvBaoCao.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                gvBaoCao.Columns[i].AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9, System.Drawing.FontStyle.Bold);
                            }

                            // Tùy chọn hiển thị GridView
                            gvBaoCao.OptionsView.ShowFooter = true; // Hiển thị footer nếu cần
                            gvBaoCao.OptionsBehavior.Editable = false; // Khóa chỉnh sửa cột nếu chỉ xem
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            DataTable dataTable = (DataTable)gcBaoCao.DataSource;
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất báo cáo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = "BaoCaoDoanhThuTheoThang.pdf"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        BaseFont robotoRegular = BaseFont.CreateFont(robotoRegularPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        BaseFont robotoBold = BaseFont.CreateFont(robotoBoldPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                        iTextFont shopFont = new iTextFont(robotoBold, 14, iTextFont.BOLD);
                        iTextFont titleFont = new iTextFont(robotoBold, 18, iTextFont.BOLD);
                        iTextFont headerFont = new iTextFont(robotoBold, 12, iTextFont.BOLD, iTextBaseColor.WHITE);
                        iTextFont dataFont = new iTextFont(robotoRegular, 10);
                        iTextFont normalFont = new iTextFont(robotoRegular, 10);

                        // Thông tin cửa hàng
                        pdfDoc.Add(new iTextParagraph("Tip Xinh Cosmetic", shopFont) { Alignment = iTextElement.ALIGN_LEFT });
                        pdfDoc.Add(new iTextParagraph("Địa chỉ: Số 01, Ngõ 102, Trần Phú, Mỗ Lao, Hà Đông, Hà Nội", normalFont) { Alignment = iTextElement.ALIGN_LEFT });
                        pdfDoc.Add(new iTextParagraph("\n"));

                        // Tiêu đề báo cáo
                        string selectedReportType = cboLoaiBC.SelectedItem.ToString().ToUpper();  // Lấy giá trị và chuyển sang chữ in hoa
                        iTextParagraph title = new iTextParagraph($"BÁO CÁO DOANH THU THEO {selectedReportType}", titleFont) { Alignment = iTextElement.ALIGN_CENTER };
                        pdfDoc.Add(title);



                        // Khoảng trắng và thông tin thời gian
                        pdfDoc.Add(new iTextParagraph($"Từ ngày: {dtTuNgay.Value:dd/MM/yyyy} - Đến ngày: {dtDenNgay.Value:dd/MM/yyyy}", normalFont) { Alignment = iTextElement.ALIGN_CENTER });
                        pdfDoc.Add(new iTextParagraph("\n"));

                        pdfDoc.Add(new iTextParagraph($"Người tạo: {EmployeeName}", normalFont) { Alignment = iTextElement.ALIGN_LEFT });
                        pdfDoc.Add(new iTextParagraph($"Ngày tạo: {DateTime.Now:dd/MM/yyyy}", normalFont) { Alignment = iTextElement.ALIGN_LEFT });
                        pdfDoc.Add(new iTextParagraph("\n"));

                        PdfPTable pdfTable = new PdfPTable(dataTable.Columns.Count);
                        pdfTable.WidthPercentage = 100;

                        float[] columnWidths = { 2f, 4f, 1.5f, 2f }; // Thiết lập tỷ lệ độ rộng các cột
                        pdfTable.SetWidths(columnWidths);

                        // Tiêu đề cột
                        string[] columnTitles;
                        if (cboLoaiBC.SelectedItem.ToString() == "Nhân viên")
                        {
                            columnTitles = new string[] { "Mã nhân viên", "Tên hàng hóa", "Số lượng đơn hàng", "Doanh thu" };
                        }
                        else if (cboLoaiBC.SelectedItem.ToString() == "Khách hàng")
                        {
                            columnTitles = new string[] { "Mã khách hàng", "Tên khách hàng", "Số lượng đơn hàng", "Doanh thu" };
                        }
                        else
                        {
                            columnTitles = new string[] { "Mã nhóm", "Tên nhóm", "Số lượng hàng hóa","Doanh thu" };
                        }
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(columnTitles[i], headerFont))
                            {
                                BackgroundColor = iTextBaseColor.GRAY,
                                HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                                Padding = 8 // Tăng độ rộng hàng
                            };
                            pdfTable.AddCell(cell);
                        }

                        // Dữ liệu bảng
                        // Dữ liệu bảng
                        foreach (DataRow row in dataTable.Rows) // Lặp qua từng hàng
                        {
                            foreach (DataColumn column in dataTable.Columns) // Lặp qua từng cột trong hàng
                            {
                                string cellValue = row[column].ToString(); // Truy cập giá trị của từng ô
                                PdfPCell dataCell;

                                // Kiểm tra tên cột và định dạng dữ liệu
                                if (column.ColumnName == "Số lượng đơn hàng" || column.ColumnName == "Số lượng hàng hóa")
                                {
                                    // Định dạng số ngăn cách 3 chữ số
                                    cellValue = int.TryParse(cellValue, out int quantity) ? quantity.ToString("n0") : "0";
                                    dataCell = new PdfPCell(new Phrase(cellValue, dataFont))
                                    {
                                        HorizontalAlignment = PdfPCell.ALIGN_CENTER, // Căn giữa
                                        Padding = 8
                                    };
                                }
                                else if (column.ColumnName == "Doanh thu")
                                {
                                    // Định dạng số ngăn cách 3 chữ số
                                    cellValue = decimal.TryParse(cellValue, out decimal revenue) ? revenue.ToString("n0") : "0";
                                    dataCell = new PdfPCell(new Phrase(cellValue, dataFont))
                                    {
                                        HorizontalAlignment = PdfPCell.ALIGN_RIGHT, // Căn phải
                                        Padding = 8
                                    };
                                }
                                else
                                {
                                    // Các cột khác căn trái
                                    dataCell = new PdfPCell(new Phrase(cellValue, dataFont))
                                    {
                                        HorizontalAlignment = PdfPCell.ALIGN_LEFT,
                                        Padding = 8
                                    };
                                }

                                pdfTable.AddCell(dataCell);
                            }
                        }




                        pdfDoc.Add(pdfTable);
                        pdfDoc.Close();
                    }

                    // Mở file PDF sau khi xuất
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);

                    MessageBox.Show("Xuất báo cáo thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi xuất báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void uc_BaoCao2_Load(object sender, EventArgs e)
        {
            if (cboLoaiBC.Items.Count > 0)
            {
                // Chọn mặc định "Khách hàng" nếu có ít nhất một mục trong cboLoaiBC
                cboLoaiBC.SelectedIndex = cboLoaiBC.Items.IndexOf("Khách hàng");
            }
        }

        private void cboLoaiBC_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

