using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class NHANVIEN
    {
        Entities db;
        public NHANVIEN()
        {
            db = Entities.CreateEntities();
        }
        public tb_NHANVIEN getItem(string manv)
        {
            return db.tb_NHANVIEN.FirstOrDefault(x => x.MANV == manv);
        }
        public List<tb_NHANVIEN> getAll()
        {
            return db.tb_NHANVIEN.ToList();
        }
        public void add(tb_NHANVIEN nv)
        {
            try
            {
                if (nv == null)
                {
                    throw new ArgumentNullException(nameof(nv), "Đối tượng nhân viên không được null.");
                }

                // Kiểm tra mã nhân viên đã tồn tại hay chưa
                var existingNV = db.tb_NHANVIEN.FirstOrDefault(x => x.MANV == nv.MANV);
                if (existingNV != null)
                {
                    throw new Exception($"Mã nhân viên {nv.MANV} đã tồn tại.");
                }

                db.tb_NHANVIEN.Add(nv);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu: " + ex.Message);
            }
        }
        public void update(tb_NHANVIEN nv)
        {
            tb_NHANVIEN _nv = db.tb_NHANVIEN.FirstOrDefault(x => x.MANV == nv.MANV);
            _nv.TENNV = nv.TENNV;
            _nv.DIACHI = nv.DIACHI;
            _nv.SDT = nv.SDT;
            _nv.EMAIL = nv.EMAIL;
            _nv.GIOITINH = nv.GIOITINH;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }
        public void delete(string manv)
        {
            tb_NHANVIEN nv = db.tb_NHANVIEN.FirstOrDefault(x => x.MANV == manv);
            db.tb_NHANVIEN.Remove(nv);
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
