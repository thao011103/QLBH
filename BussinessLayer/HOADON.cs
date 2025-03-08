using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class HOADON
    {
        Entities db;
        public HOADON()
        {
            db = Entities.CreateEntities();
        }
        public tb_HOADON getItem(string mahd)
        {
            return db.tb_HOADON.FirstOrDefault(x => x.MAHD == mahd);
        }
        public List<tb_HOADON> getAll()
        {
            return db.tb_HOADON.ToList();
        }
        public void add(tb_HOADON hd)
        {
            try
            {
                db.tb_HOADON.Add(hd);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }
        public void update(tb_HOADON hd)
        {
            tb_HOADON _hd = db.tb_HOADON.FirstOrDefault(x => x.MAHD == hd.MAHD);
            _hd.NGAYHD = hd.NGAYHD;
            _hd.SOLUONG = hd.SOLUONG;
            _hd.TONGTIEN = hd.TONGTIEN;
            _hd.GHICHU = hd.GHICHU;
            _hd.MANV = hd.MANV;
            _hd.MAKH = hd.MAKH;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }
        public void delete(string mahd)
        {
            tb_HOADON hd = db.tb_HOADON.FirstOrDefault(x => x.MAHD == mahd);
            db.tb_HOADON.Remove(hd);
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