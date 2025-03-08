using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class HANGHOA
    {
        Entities db;
        public HANGHOA()
        {
            db = Entities.CreateEntities();
        }
        public tb_HANGHOA getItem(string mahh)
        {
            return db.tb_HANGHOA.FirstOrDefault(x => x.MAHH == mahh);
        }
        public List<tb_HANGHOA> getAll()
        {
            return db.tb_HANGHOA.ToList();
        }
        public List<tb_HANGHOA> getListByKeyword(string keyword)
        {
            return db.tb_HANGHOA.Where(ts => ts.TENHH.Contains(keyword)).ToList();
        }
        public List<tb_HANGHOA> getListByNhom(string manhom)
        {
            return db.tb_HANGHOA.Where(x => x.MANHOM == manhom).OrderBy(o => o).ToList();
        }
        public bool checkExist(string mahh)
        {
            return db.tb_HANGHOA.Any(x => x.MAHH == mahh);
        }
        public void add(tb_HANGHOA hh)
        {
            try
            {
                // Kiểm tra các trường bắt buộc
                if (string.IsNullOrEmpty(hh.TENHH) || string.IsNullOrEmpty(hh.MANHOM) || string.IsNullOrEmpty(hh.DVT))
                {
                    throw new Exception("Các trường bắt buộc không được để trống.");
                }

                // Kiểm tra mã trùng
                if (checkExist(hh.MAHH))
                {
                    throw new Exception($"Mã hàng hóa {hh.MAHH} đã tồn tại. Vui lòng kiểm tra lại.");
                }

                // Thêm mới hàng hóa
                db.tb_HANGHOA.Add(hh);
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Thuộc tính: {validationError.PropertyName} Lỗi: {validationError.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi bằng MessageBox nếu là ứng dụng Windows Forms
                System.Windows.Forms.MessageBox.Show(
                    "Có lỗi xảy ra: " + ex.Message,
                    "Lỗi",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        public void update(tb_HANGHOA hh)
        {
            tb_HANGHOA _hh = db.tb_HANGHOA.FirstOrDefault(x => x.MAHH == hh.MAHH);
            _hh.TENHH = hh.TENHH;
            _hh.MOTA = hh.MOTA;
            _hh.MANHOM = hh.MANHOM;
            _hh.DVT = hh.DVT;
            _hh.GIANHAP = hh.GIANHAP;
            _hh.GIABAN = hh.GIABAN;
            _hh.NUOCSX = hh.NUOCSX;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }
        public void delete(string mahh)
        {
            tb_HANGHOA hh = db.tb_HANGHOA.FirstOrDefault(x => x.MAHH == mahh);
            db.tb_HANGHOA.Remove(hh);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }
        public List<view_HANGHOA_WITH_LG_TON> getAllWithLG_TON()
        {
            return db.view_HANGHOA_WITH_LG_TON.ToList();
        }

        // Lấy danh sách hàng hóa theo từ khóa kèm theo số lượng tồn
        public List<view_HANGHOA_WITH_LG_TON> getListByKeywordWithLG_TON(string keyword)
        {
            return db.view_HANGHOA_WITH_LG_TON
                     .Where(h => h.TENHH.Contains(keyword))
                     .ToList();
        }
        public view_HANGHOA_WITH_LG_TON getItemWithLG_TON(string mahh)
        {
            return db.view_HANGHOA_WITH_LG_TON.FirstOrDefault(x => x.MAHH == mahh);
        }

    }
}