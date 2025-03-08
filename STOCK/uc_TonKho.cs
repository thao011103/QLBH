using BussinessLayer;  // Đảm bảo tham chiếu đúng namespace 
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
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
using iTextSharp.text;

namespace STOCK
{
    public partial class Uc_TonKho : UserControl
    {
        public string EmployeeName { get; set; }
        private Timer dataRefreshTimer;
        private readonly string robotoRegularPath = @"C:\Users\ADMIN\Downloads\Roboto\Roboto-Regular.ttf";
        private readonly string robotoBoldPath = @"C:\Users\ADMIN\Downloads\Roboto\Roboto-Bold.ttf";
        public Uc_TonKho(string employeeName)
        {
            InitializeComponent();
            DisplayData();
            EmployeeName = employeeName;
            dataRefreshTimer = new Timer();
            dataRefreshTimer.Interval = 5000; // 5 giây
            dataRefreshTimer.Tick += DataRefreshTimer_Tick;
            dataRefreshTimer.Start();
        }

        private void DataRefreshTimer_Tick(object sender, EventArgs e)
        {
            // Gọi lại phương thức DisplayData để làm mới dữ liệu
            DisplayData();
        }

        private void DisplayData()
        {
            try
            {
                using (SqlConnection connection = Connection.GetConnection())  // Gọi phương thức GetConnection
                {
                    connection.Open();
                    string sql = @"SELECT * FROM view_TONKHO";
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    gcTonKho.DataSource = dataTable;

                    GridView view = gcTonKho.MainView as GridView;
                    if (view != null)
                    {
                        foreach (DevExpress.XtraGrid.Columns.GridColumn column in view.Columns)
                        {
                            column.OptionsColumn.ReadOnly = true;
                        }

                        // Đăng ký sự kiện CustomDrawCell
                        view.CustomDrawCell += View_CustomDrawCell;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void View_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.FieldName == "LG_TON") // Kiểm tra cột LG_TON
            {
                // Lấy giá trị LG_TON và LG_ANTOAN
                decimal lgTon = Convert.ToDecimal(view.GetRowCellValue(e.RowHandle, "LG_TON"));
                decimal lgAntoan = Convert.ToDecimal(view.GetRowCellValue(e.RowHandle, "LG_ANTOAN"));

                // Nếu LG_TON nhỏ hơn LG_ANTOAN, đổi màu văn bản thành đỏ
                if (lgTon < lgAntoan)
                {
                    e.Appearance.ForeColor = System.Drawing.Color.Red; // Đặt màu chữ thành đỏ
                }
            }
        }

        public void RefreshData()
        {
            DisplayData();  // Gọi lại hàm hiển thị dữ liệu
        }

        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            DataTable dataTable = (DataTable)gcTonKho.DataSource;
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất báo cáo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = "BaoCaoTonKho.pdf"
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
                        iTextParagraph title = new iTextParagraph("BÁO CÁO TỒN KHO", titleFont) { Alignment = iTextElement.ALIGN_CENTER };
                        pdfDoc.Add(title);
                        pdfDoc.Add(new iTextParagraph("\n"));

                        pdfDoc.Add(new iTextParagraph($"Người tạo: {EmployeeName}", normalFont) { Alignment = iTextElement.ALIGN_LEFT });
                        pdfDoc.Add(new iTextParagraph($"Ngày tạo: {DateTime.Now:dd/MM/yyyy}", normalFont) { Alignment = iTextElement.ALIGN_LEFT });  // Thêm ngày tạo
                        pdfDoc.Add(new iTextParagraph("\n"));

                        PdfPTable pdfTable = new PdfPTable(dataTable.Columns.Count);
                        pdfTable.WidthPercentage = 100;

                        float[] columnWidths = { 2f, 4f,2f, 2f, 2f, 2f , 3f, 3f, 2f}; // Thiết lập tỷ lệ độ rộng các cột
                        pdfTable.SetWidths(columnWidths);

                        // Tiêu đề cột
                        string[] columnTitles = { "Mã hàng hóa", "Tên hàng hóa","Đơn vị tính", "Lượng nhập", "Lượng bán", "Lượng tồn", "Trị giá", "Thành tiền", "Lượng an toàn" };
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
                        foreach (DataRow row in dataTable.Rows)
                        {
                            for (int i = 0; i < row.ItemArray.Length; i++)
                            {
                                object cellData = row.ItemArray[i];
                                string cellValue = cellData != DBNull.Value ? cellData.ToString() : string.Empty;

                                // Kiểm tra nếu giá trị là số và định dạng có dấu phẩy
                                if (decimal.TryParse(cellValue, out decimal numericValue))
                                {
                                    // Định dạng số có dấu phẩy (N0: không có phần thập phân)
                                    cellValue = numericValue.ToString("N0");
                                }

                                PdfPCell dataCell = new PdfPCell(new Phrase(cellValue, dataFont))
                                {
                                    Padding = 8 // Tăng độ rộng hàng
                                };

                                // Căn phải cho các cột "Trị giá" và "Thành tiền" (ví dụ cột 6 và 7)
                                if (i == 6 || i == 7)  // Cột 6 ("Trị giá") và Cột 7 ("Thành tiền")
                                {
                                    dataCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT; // Căn phải
                                }
                                // Căn giữa cho các cột "Lượng tồn" (cột 5) và "Lượng an toàn" (cột 8)
                                else if (i == 5 || i == 8)  // Cột 5 ("Lượng tồn") và Cột 8 ("Lượng an toàn")
                                {
                                    dataCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER; // Căn giữa
                                }
                                else
                                {
                                    // Các cột khác căn trái
                                    dataCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT; // Căn trái
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
    }
}
