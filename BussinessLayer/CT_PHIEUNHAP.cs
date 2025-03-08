using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BussinessLayer
{
    public class CT_PHIEUNHAP
    {
        Entities db;
        public CT_PHIEUNHAP()
        {
            db = Entities.CreateEntities();
        }

        public tb_CT_PHIEUNHAP getItem(string mactpn)
        {
            return db.tb_CT_PHIEUNHAP.FirstOrDefault(x => x.MA_CT_PN == mactpn);
        }

        public List<tb_CT_PHIEUNHAP> getList(string mapn)
        {
            return db.tb_CT_PHIEUNHAP.Where(x => x.MAPN == mapn).ToList();
        }

        public List<obj_CT_PHIEUNHAP> getListByKhoaFull(string mapn)
        {
            var lst = db.tb_CT_PHIEUNHAP.Where(x => x.MAPN == mapn).ToList();
            List<obj_CT_PHIEUNHAP> lsCT = new List<obj_CT_PHIEUNHAP>();
            obj_CT_PHIEUNHAP obj;
            foreach (var item in lst)
            {
                obj = new obj_CT_PHIEUNHAP();
                obj.MA_CT_PN = item.MA_CT_PN;
                obj.MAPN = item.MAPN;
                obj.MAHH = item.MAHH;

                // Lấy thông tin hàng hóa
                var h = db.tb_HANGHOA.FirstOrDefault(x => x.MAHH == item.MAHH);
                if (h != null)
                {
                    obj.TENHH = h.TENHH;
                    obj.DVT = h.DVT;  // Truyền trường DVT từ bảng HANGHOA
                }

                // Thêm các thông tin khác vào obj
                obj.SOLUONGCT = item.SOLUONGCT;
                obj.GIANHAP = item.GIANHAP;
                obj.THANHTIEN = item.THANHTIEN;

                // Thêm đối tượng vào danh sách
                lsCT.Add(obj);
            }

            return lsCT;
        }

        public void add(tb_CT_PHIEUNHAP ctpn)
        {
            try
            {
                db.tb_CT_PHIEUNHAP.Add(ctpn);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Lấy chi tiết từ InnerException để biết nguyên nhân cụ thể
                var innerMessage = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? "Không có thông tin chi tiết";
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu: " + ex.Message + " - Chi tiết: " + innerMessage);
            }
        }

        public void update(tb_CT_PHIEUNHAP ctpn)
        {
            tb_CT_PHIEUNHAP _ctpn = db.tb_CT_PHIEUNHAP.FirstOrDefault(x => x.MA_CT_PN == ctpn.MA_CT_PN);
            _ctpn.MAPN = ctpn.MAPN;
            _ctpn.MAHH = ctpn.MAHH;
            _ctpn.SOLUONGCT = ctpn.SOLUONGCT;
            _ctpn.DVT = ctpn.DVT; // Trường DVT lấy từ đối tượng ctpn
            _ctpn.GIANHAP = ctpn.GIANHAP;
            _ctpn.THANHTIEN = ctpn.THANHTIEN;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }

        public void delete(string mactpn)
        {
            tb_CT_PHIEUNHAP ctpn = db.tb_CT_PHIEUNHAP.FirstOrDefault(x => x.MA_CT_PN == mactpn);
            db.tb_CT_PHIEUNHAP.Remove(ctpn);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }

        public void deleteAll(string maPN)
        {
            var itemsToDelete = db.tb_CT_PHIEUNHAP.Where(x => x.MAPN == maPN).ToList();
            db.tb_CT_PHIEUNHAP.RemoveRange(itemsToDelete);
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
