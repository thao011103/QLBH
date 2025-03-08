using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BussinessLayer
{
    public class TAIKHOAN
    {
        Entities db;

        public TAIKHOAN()
        {
            db = Entities.CreateEntities();
        }

        // Lấy thông tin tài khoản theo MATK
        public tb_TAIKHOAN getItem(string matk)
        {
            return db.tb_TAIKHOAN.FirstOrDefault(x => x.MATK == matk);
        }

        // Lấy tất cả tài khoản
        public List<tb_TAIKHOAN> getAll()
        {
            return db.tb_TAIKHOAN.ToList();
        }

        // Thêm tài khoản mới
        public void add(tb_TAIKHOAN tk)
        {
            try
            {
                db.tb_TAIKHOAN.Add(tk);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }

        // Cập nhật thông tin tài khoản
        public void update(tb_TAIKHOAN tk)
        {
            tb_TAIKHOAN _tk = db.tb_TAIKHOAN.FirstOrDefault(x => x.MATK == tk.MATK);
            _tk.MANV = tk.MANV;
            _tk.TENDANGNHAP = tk.TENDANGNHAP;
            _tk.MATKHAU = tk.MATKHAU;
            _tk.VAITRO = tk.VAITRO;

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }

        // Xóa tài khoản theo MATK
        public void delete(string matk)
        {
            tb_TAIKHOAN tk = db.tb_TAIKHOAN.FirstOrDefault(x => x.MATK == matk);
            db.tb_TAIKHOAN.Remove(tk);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }
    }
}