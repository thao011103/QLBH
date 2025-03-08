using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class CT_HOADON
    {
        Entities db;
        public CT_HOADON()
        {
            db = Entities.CreateEntities();
        }

        public tb_CT_HOADON getItem(string macthd)
        {
            return db.tb_CT_HOADON.FirstOrDefault(x => x.MA_CT_HD == macthd);
        }

        public List<tb_CT_HOADON> getList(string mahd)
        {
            return db.tb_CT_HOADON.Where(x => x.MAHD == mahd).ToList();
        }

        public List<obj_CT_HOADON> getListByKhoaFull(string mahd)
        {
            var lst = db.tb_CT_HOADON.Where(x => x.MAHD == mahd).ToList();
            List<obj_CT_HOADON> lsCT = new List<obj_CT_HOADON>();
            obj_CT_HOADON obj;
            foreach (var item in lst)
            {
                obj = new obj_CT_HOADON();
                obj.MA_CT_HD = item.MA_CT_HD;
                obj.MAHD = item.MAHD;
                obj.MAHH = item.MAHH;

                // Lấy thông tin hàng hóa
                var h = db.tb_HANGHOA.FirstOrDefault(x => x.MAHH == item.MAHH);
                obj.TENHH = h?.TENHH;

                // Lấy thông tin đơn vị tính từ tb_HANGHOA
                if (h != null)
                {
                    obj.DVT = h.DVT; // Đơn vị tính giờ lấy từ tb_HANGHOA
                }
                else
                {
                    obj.DVT = "Không có đơn vị tính"; // Nếu không tìm thấy hàng hóa
                }

                // Thêm các thông tin khác vào obj
                obj.SOLUONGCT = item.SOLUONGCT;
                obj.GIABAN = item.GIABAN;
                obj.THANHTIEN = item.THANHTIEN;

                // Thêm đối tượng vào danh sách
                lsCT.Add(obj);
            }

            return lsCT;
        }

        public void add(tb_CT_HOADON cthd)
        {
            try
            {
                db.tb_CT_HOADON.Add(cthd);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Lấy chi tiết từ InnerException để biết nguyên nhân cụ thể
                var innerMessage = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? "Không có thông tin chi tiết";
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu: " + ex.Message + " - Chi tiết: " + innerMessage);
            }
        }

        public void update(tb_CT_HOADON cthd)
        {
            tb_CT_HOADON _cthd = db.tb_CT_HOADON.FirstOrDefault(x => x.MA_CT_HD == cthd.MA_CT_HD);
            _cthd.MAHD = cthd.MAHD;
            _cthd.TENHH = cthd.TENHH;
            _cthd.MAHH = cthd.MAHH;
            _cthd.SOLUONGCT = cthd.SOLUONGCT;
            _cthd.GIABAN = cthd.GIABAN;
            _cthd.THANHTIEN = cthd.THANHTIEN;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }

        public void delete(string macthd)
        {
            tb_CT_HOADON cthd = db.tb_CT_HOADON.FirstOrDefault(x => x.MA_CT_HD == macthd);
            db.tb_CT_HOADON.Remove(cthd);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }

        public void deleteAll(string maHD)
        {
            var itemsToDelete = db.tb_CT_HOADON.Where(x => x.MAHD == maHD).ToList();
            db.tb_CT_HOADON.RemoveRange(itemsToDelete);
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
