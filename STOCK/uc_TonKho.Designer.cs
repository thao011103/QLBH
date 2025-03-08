
namespace STOCK
{
    partial class Uc_TonKho
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gcTonKho = new DevExpress.XtraGrid.GridControl();
            this.gvTonKho = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.DVT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MAHH = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TENHH = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TRIGIA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LG_NHAP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.THANHTIEN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LG_TON = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LG_BAN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LG_ANTOAN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnXuatBaoCao = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gcTonKho)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTonKho)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gcTonKho
            // 
            this.gcTonKho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcTonKho.Location = new System.Drawing.Point(3, 39);
            this.gcTonKho.MainView = this.gvTonKho;
            this.gcTonKho.Name = "gcTonKho";
            this.gcTonKho.Size = new System.Drawing.Size(1465, 697);
            this.gcTonKho.TabIndex = 4;
            this.gcTonKho.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTonKho});
            // 
            // gvTonKho
            // 
            this.gvTonKho.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.DVT,
            this.MAHH,
            this.TENHH,
            this.TRIGIA,
            this.LG_NHAP,
            this.THANHTIEN,
            this.LG_TON,
            this.LG_BAN,
            this.LG_ANTOAN});
            this.gvTonKho.GridControl = this.gcTonKho;
            this.gvTonKho.Name = "gvTonKho";
            this.gvTonKho.OptionsFind.AlwaysVisible = true;
            this.gvTonKho.RowHeight = 40;
            // 
            // DVT
            // 
            this.DVT.AppearanceCell.Options.UseTextOptions = true;
            this.DVT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DVT.AppearanceHeader.Options.UseTextOptions = true;
            this.DVT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DVT.Caption = "ĐVT";
            this.DVT.FieldName = "DVT";
            this.DVT.MaxWidth = 120;
            this.DVT.MinWidth = 120;
            this.DVT.Name = "DVT";
            this.DVT.Visible = true;
            this.DVT.VisibleIndex = 2;
            this.DVT.Width = 120;
            // 
            // MAHH
            // 
            this.MAHH.AppearanceHeader.Options.UseTextOptions = true;
            this.MAHH.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.MAHH.Caption = "Mã hàng";
            this.MAHH.FieldName = "MAHH";
            this.MAHH.MaxWidth = 120;
            this.MAHH.MinWidth = 120;
            this.MAHH.Name = "MAHH";
            this.MAHH.Visible = true;
            this.MAHH.VisibleIndex = 0;
            this.MAHH.Width = 120;
            // 
            // TENHH
            // 
            this.TENHH.AppearanceHeader.Options.UseTextOptions = true;
            this.TENHH.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TENHH.Caption = "Tên hàng hóa";
            this.TENHH.FieldName = "TENHH";
            this.TENHH.MaxWidth = 350;
            this.TENHH.MinWidth = 350;
            this.TENHH.Name = "TENHH";
            this.TENHH.Visible = true;
            this.TENHH.VisibleIndex = 1;
            this.TENHH.Width = 350;
            // 
            // TRIGIA
            // 
            this.TRIGIA.AppearanceCell.Options.UseTextOptions = true;
            this.TRIGIA.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.TRIGIA.AppearanceHeader.Options.UseTextOptions = true;
            this.TRIGIA.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TRIGIA.Caption = "Trị giá";
            this.TRIGIA.DisplayFormat.FormatString = "N0";
            this.TRIGIA.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.TRIGIA.FieldName = "TRIGIA";
            this.TRIGIA.MaxWidth = 200;
            this.TRIGIA.MinWidth = 200;
            this.TRIGIA.Name = "TRIGIA";
            this.TRIGIA.Visible = true;
            this.TRIGIA.VisibleIndex = 7;
            this.TRIGIA.Width = 200;
            // 
            // LG_NHAP
            // 
            this.LG_NHAP.AppearanceCell.Options.UseTextOptions = true;
            this.LG_NHAP.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LG_NHAP.AppearanceHeader.Options.UseTextOptions = true;
            this.LG_NHAP.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LG_NHAP.Caption = "Nhập mua";
            this.LG_NHAP.FieldName = "LG_NHAP";
            this.LG_NHAP.MaxWidth = 150;
            this.LG_NHAP.MinWidth = 150;
            this.LG_NHAP.Name = "LG_NHAP";
            this.LG_NHAP.Visible = true;
            this.LG_NHAP.VisibleIndex = 3;
            this.LG_NHAP.Width = 150;
            // 
            // THANHTIEN
            // 
            this.THANHTIEN.AppearanceCell.Options.UseTextOptions = true;
            this.THANHTIEN.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.THANHTIEN.AppearanceHeader.Options.UseTextOptions = true;
            this.THANHTIEN.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.THANHTIEN.Caption = "Thành tiền";
            this.THANHTIEN.DisplayFormat.FormatString = "n0";
            this.THANHTIEN.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.THANHTIEN.FieldName = "THANHTIEN";
            this.THANHTIEN.MaxWidth = 200;
            this.THANHTIEN.MinWidth = 200;
            this.THANHTIEN.Name = "THANHTIEN";
            this.THANHTIEN.Visible = true;
            this.THANHTIEN.VisibleIndex = 8;
            this.THANHTIEN.Width = 200;
            // 
            // LG_TON
            // 
            this.LG_TON.AppearanceCell.Options.UseTextOptions = true;
            this.LG_TON.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LG_TON.AppearanceHeader.Options.UseTextOptions = true;
            this.LG_TON.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LG_TON.Caption = "Lượng tồn";
            this.LG_TON.FieldName = "LG_TON";
            this.LG_TON.MaxWidth = 150;
            this.LG_TON.MinWidth = 150;
            this.LG_TON.Name = "LG_TON";
            this.LG_TON.Visible = true;
            this.LG_TON.VisibleIndex = 5;
            this.LG_TON.Width = 150;
            // 
            // LG_BAN
            // 
            this.LG_BAN.AppearanceCell.Options.UseTextOptions = true;
            this.LG_BAN.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LG_BAN.AppearanceHeader.Options.UseTextOptions = true;
            this.LG_BAN.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LG_BAN.Caption = "Lượng bán";
            this.LG_BAN.FieldName = "LG_BAN";
            this.LG_BAN.MaxWidth = 150;
            this.LG_BAN.MinWidth = 150;
            this.LG_BAN.Name = "LG_BAN";
            this.LG_BAN.Visible = true;
            this.LG_BAN.VisibleIndex = 4;
            this.LG_BAN.Width = 150;
            // 
            // LG_ANTOAN
            // 
            this.LG_ANTOAN.AppearanceCell.Options.UseTextOptions = true;
            this.LG_ANTOAN.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LG_ANTOAN.AppearanceHeader.Options.UseTextOptions = true;
            this.LG_ANTOAN.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LG_ANTOAN.Caption = "Lượng an toàn";
            this.LG_ANTOAN.FieldName = "LG_ANTOAN";
            this.LG_ANTOAN.MaxWidth = 150;
            this.LG_ANTOAN.MinWidth = 150;
            this.LG_ANTOAN.Name = "LG_ANTOAN";
            this.LG_ANTOAN.Visible = true;
            this.LG_ANTOAN.VisibleIndex = 6;
            this.LG_ANTOAN.Width = 150;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.gcTonKho, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1471, 739);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.btnXuatBaoCao);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1465, 30);
            this.panel1.TabIndex = 5;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(516, 10);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(268, 34);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "BÁO CÁO TỒN KHO";
            // 
            // btnXuatBaoCao
            // 
            this.btnXuatBaoCao.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnXuatBaoCao.Location = new System.Drawing.Point(1243, 15);
            this.btnXuatBaoCao.Name = "btnXuatBaoCao";
            this.btnXuatBaoCao.Size = new System.Drawing.Size(160, 29);
            this.btnXuatBaoCao.TabIndex = 0;
            this.btnXuatBaoCao.Text = "Xuất báo cáo";
            this.btnXuatBaoCao.UseVisualStyleBackColor = true;
            this.btnXuatBaoCao.Click += new System.EventHandler(this.btnXuatBaoCao_Click);
            // 
            // Uc_TonKho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Uc_TonKho";
            this.Size = new System.Drawing.Size(1471, 739);
            ((System.ComponentModel.ISupportInitialize)(this.gcTonKho)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTonKho)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraGrid.GridControl gcTonKho;
        private DevExpress.XtraGrid.Views.Grid.GridView gvTonKho;
        private DevExpress.XtraGrid.Columns.GridColumn MAHH;
        private DevExpress.XtraGrid.Columns.GridColumn TENHH;
        private DevExpress.XtraGrid.Columns.GridColumn TRIGIA;
        private DevExpress.XtraGrid.Columns.GridColumn LG_NHAP;
        private DevExpress.XtraGrid.Columns.GridColumn THANHTIEN;
        private DevExpress.XtraGrid.Columns.GridColumn LG_TON;
        private DevExpress.XtraGrid.Columns.GridColumn LG_BAN;
        private DevExpress.XtraGrid.Columns.GridColumn LG_ANTOAN;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnXuatBaoCao;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn DVT;
    }
}
