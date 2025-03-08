using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class KHACHHANG
    {
        Entities db;
        public KHACHHANG()
        {
            db = Entities.CreateEntities();
        }
        public tb_KHACHHANG getItem(string makh)
        {
            return db.tb_KHACHHANG.FirstOrDefault(x => x.MAKH == makh);
        }
        public List<tb_KHACHHANG> getAll()
        {
            return db.tb_KHACHHANG.ToList();
        }
        public void add(tb_KHACHHANG kh)
        {
            try
            {
                db.tb_KHACHHANG.Add(kh);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }
        public void update(tb_KHACHHANG kh)
        {
            tb_KHACHHANG _kh = db.tb_KHACHHANG.FirstOrDefault(x => x.MAKH == kh.MAKH);
            _kh.TENKH = kh.TENKH;
            _kh.DIACHI = kh.DIACHI;
            _kh.SDT = kh.SDT;
            _kh.EMAIL = kh.EMAIL;
            _kh.GIOITINH = kh.GIOITINH;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }
        public void delete(string makh)
        {
            tb_KHACHHANG kh = db.tb_KHACHHANG.FirstOrDefault(x => x.MAKH == makh);
            db.tb_KHACHHANG.Remove(kh);
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
