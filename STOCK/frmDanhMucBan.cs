using BussinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace STOCK
{
    public partial class frmDanhMucBan : Form
    {
        public frmDanhMucBan()
        {
            InitializeComponent();
        }

        public frmDanhMucBan(DevExpress.XtraGrid.Views.Grid.GridView gvChiTiet, string chuoi)
        {
            InitializeComponent();
            this._chuoi = chuoi;
            this._gvChiTiet = gvChiTiet;
        }

        string _chuoi;
        DevExpress.XtraGrid.Views.Grid.GridView _gvChiTiet;
        CT_PHIEUNHAP _phieunhapct;
        HANGHOA _hanghoa;

        private void frmDanhMucBan_Load(object sender, EventArgs e)
        {
            _phieunhapct = new CT_PHIEUNHAP();
            _hanghoa = new HANGHOA();

            if (_chuoi.Trim().Length == 1)
                gcDanhSach.DataSource = _hanghoa.getAllWithLG_TON();
            else
                gcDanhSach.DataSource = _hanghoa.getListByKeywordWithLG_TON(_chuoi.Substring(1).Trim());

            gvDanhSach.OptionsBehavior.Editable = false;
        }

        void Insert()
        {
            int[] _selectRow = gvDanhSach.GetSelectedRows();
            List<string> _selected = new List<string>();
            foreach (int item in _selectRow)
            {
                _selected.Add(gvDanhSach.GetRowCellValue(item, "MAHH").ToString());
            }

            List<errExport> _err = new List<errExport>();
            List<string> _valid = new List<string>();
            List<string> _exist = new List<string>();

            // Kiểm tra các mặt hàng đã tồn tại trong grid chi tiết
            if (_gvChiTiet.RowCount > 1)
            {
                if (_gvChiTiet.GetRowCellValue(_gvChiTiet.RowCount - 1, "TENHH") != null)
                {
                    for (int i = 0; i < _gvChiTiet.RowCount - 1; i++)
                        _exist.Add(_gvChiTiet.GetRowCellValue(i, "MAHH").ToString());
                }
                else
                {
                    for (int i = 0; i < _gvChiTiet.RowCount - 1; i++)
                        _exist.Add(_gvChiTiet.GetRowCellValue(i, "MAHH").ToString());
                }
            }

            foreach (string _item in _selected)
            {
                tb_HANGHOA hh = _hanghoa.getItem(_item);
                if (_exist.Contains(_item))
                {
                    errExport err = new errExport();
                    err._mahh = _item;
                    err._soluong = 1;
                    err._errcode = "Mã đã tồn tại trên lưới dữ liệu";
                    _err.Add(err);
                    continue;
                }
                else
                {
                    // Kiểm tra số lượng tồn
                    var itemWithLG_TON = _hanghoa.getItemWithLG_TON(_item);
                    if (itemWithLG_TON != null && itemWithLG_TON.LG_TON <= 0)
                    {
                        MessageBox.Show($"Mặt hàng {itemWithLG_TON.TENHH} đã hết hàng không thể thêm vào");
                        continue;
                    }

                    _valid.Add(_item);
                }
            }

            // Thêm các mặt hàng đã chọn vào grid chi tiết
            foreach (string _item in _valid)
            {
                tb_HANGHOA hh = _hanghoa.getItem(_item);
                if (_gvChiTiet.RowCount > 1)
                {
                    int mautin = _gvChiTiet.RowCount;
                    _gvChiTiet.SelectRow(mautin - 1);
                    if (_gvChiTiet.GetRowCellValue(_gvChiTiet.FocusedRowHandle, "TENHH") == null)
                    {
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "STT", mautin);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "MAHH", hh.MAHH);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "TENHH", hh.TENHH);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "DVT", hh.DVT);

                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "SOLUONGCT", 1);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "GIABAN", hh.GIABAN);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "THANHTIEN", hh.GIABAN);
                    }
                    else
                    {
                        _gvChiTiet.AddNewRow();
                        _gvChiTiet.SelectRow(mautin);
                        mautin++;
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "STT", mautin);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "MAHH", hh.MAHH);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "TENHH", hh.TENHH);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "DVT", hh.DVT);
                        // Thêm DVT ở đây
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "SOLUONGCT", 1);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "GIABAN", hh.GIABAN);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "THANHTIEN", hh.GIABAN);
                    }
                }
                else
                {
                    if (_gvChiTiet.RowCount == 0)
                        _gvChiTiet.AddNewRow();
                    int mautin = _gvChiTiet.RowCount;
                    _gvChiTiet.SelectRow(mautin - 1);
                    if (_gvChiTiet.GetRowCellValue(_gvChiTiet.FocusedRowHandle, "TENHH") == null)
                    {
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "STT", mautin);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "MAHH", hh.MAHH);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "TENHH", hh.TENHH);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "DVT", hh.DVT);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "SOLUONGCT", 1);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "GIABAN", hh.GIABAN);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "THANHTIEN", hh.GIABAN);
                    }
                    else
                    {
                        _gvChiTiet.AddNewRow();
                        _gvChiTiet.SelectRow(mautin);
                        mautin++;
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "STT", mautin);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "MAHH", hh.MAHH);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "TENHH", hh.TENHH);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "DVT", hh.DVT);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "SOLUONGCT", 1);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "GIABAN", hh.GIABAN);
                        _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "THANHTIEN", hh.GIABAN);
                    }
                }
            }

            _gvChiTiet.AddNewRow();
            _gvChiTiet.SelectRow(_gvChiTiet.RowCount - 1);
            _gvChiTiet.DeleteSelectedRows();
            _gvChiTiet.RefreshData();

            if (_err.Count > 0)
            {
                MessageBox.Show("Có lỗi xảy ra");
                this.Close();
            }
            else
            {
                this.Close();
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            Insert();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}