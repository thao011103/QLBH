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

namespace STOCK
{
    public partial class uc_BaoCao : UserControl
    {
        public string EmployeeName { get; set; }


        private readonly string robotoRegularPath = @"C:\Users\ADMIN\Downloads\Roboto\Roboto-Regular.ttf";
        private readonly string robotoBoldPath = @"C:\Users\ADMIN\Downloads\Roboto\Roboto-Bold.ttf";

        public uc_BaoCao(string employeeName)
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
            DateTime startDate = dtTuNgay.Value;
            DateTime endDate = dtDenNgay.Value;
            string sqlQuery = "EXEC GetDoanhThuTheoHangHoa @StartDate, @EndDate";

            try
            {
                using (SqlConnection connection = Connection.GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        gcBaoCao.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBaoCao_Click_1(object sender, EventArgs e)
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
                FileName = "BaoCaoDoanhThu.pdf"
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
                        iTextParagraph title = new iTextParagraph("BÁO CÁO DOANH THU", titleFont) { Alignment = iTextElement.ALIGN_CENTER };
                        pdfDoc.Add(title);

                        // Khoảng trắng và thông tin thời gian

                        pdfDoc.Add(new iTextParagraph($"Từ ngày: {dtTuNgay.Value:dd/MM/yyyy} - Đến ngày: {dtDenNgay.Value:dd/MM/yyyy}", normalFont) { Alignment = iTextElement.ALIGN_CENTER });
                        pdfDoc.Add(new iTextParagraph("\n"));


                        pdfDoc.Add(new iTextParagraph($"Người tạo: {EmployeeName}", normalFont) { Alignment = iTextElement.ALIGN_LEFT });
                        pdfDoc.Add(new iTextParagraph($"Ngày tạo: {DateTime.Now:dd/MM/yyyy}", normalFont) { Alignment = iTextElement.ALIGN_LEFT });  // Thêm ngày tạo
                        pdfDoc.Add(new iTextParagraph("\n"));


                        PdfPTable unitTable = new PdfPTable(1);
                        unitTable.WidthPercentage = 100;
                        PdfPCell unitCell = new PdfPCell(new Phrase("Đơn vị: Đồng", normalFont))
                        {
                            HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                            Border = PdfPCell.NO_BORDER,
                            Padding = 8
                        };
                        unitTable.AddCell(unitCell);
                        pdfDoc.Add(unitTable);

                        pdfDoc.Add(new iTextParagraph("\n"));

                        PdfPTable pdfTable = new PdfPTable(dataTable.Columns.Count);
                        pdfTable.WidthPercentage = 100;

                        float[] columnWidths = { 2f, 4f, 2f, 1.5f, 1.5f, 2f }; // Thiết lập tỷ lệ độ rộng các cột
                        pdfTable.SetWidths(columnWidths);

                        // Tiêu đề cột
                        // Tiêu đề cột
                        // Tiêu đề cột
                        string[] columnTitles = { "Mã hàng hóa", "Tên hàng hóa", "Đơn vị tính", "Giá bán", "Số lượng", "Doanh thu" };
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
                        foreach (DataRow row in dataTable.Rows)
                        {
                            for (int i = 0; i < row.ItemArray.Length; i++)
                            {
                                object cellData = row.ItemArray[i];
                                string cellValue = cellData.ToString();

                                // Check if the value is numeric and format it
                                if (decimal.TryParse(cellValue, out decimal numericValue))
                                {
                                    // Format the number with commas for thousand separators
                                    cellValue = numericValue.ToString("N0");
                                }

                                PdfPCell dataCell = new PdfPCell(new Phrase(cellValue, dataFont))
                                {
                                    Padding = 8 // Tăng độ rộng hàng
                                };

                                // Align the "Giá bán" and "Doanh thu" columns to the right
                                if (i == 3 || i == 5)  // Columns 4 (Giá bán) and 6 (Doanh thu)
                                {
                                    dataCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                }
                                // Align the "Đơn vị tính" and "Số lượng" columns to the center
                                else if (i == 2 || i == 4)  // Columns 3 (Đơn vị tính) and 5 (Số lượng)
                                {
                                    dataCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                }
                                else
                                {
                                    dataCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
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
