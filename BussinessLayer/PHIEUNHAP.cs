using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class PHIEUNHAP
    {
        Entities db;
        public PHIEUNHAP()
        {
            db = Entities.CreateEntities();
        }
        public tb_PHIEUNHAP getItem(string mapn)
        {
            return db.tb_PHIEUNHAP.FirstOrDefault(x => x.MAPN == mapn);
        }
        public List<tb_PHIEUNHAP> getAll()
        {
            return db.tb_PHIEUNHAP.ToList();
        }
        public void add(tb_PHIEUNHAP pn)
        {
            try
            {
                db.tb_PHIEUNHAP.Add(pn);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }
        public void update(tb_PHIEUNHAP pn)
        {
            tb_PHIEUNHAP _pn = db.tb_PHIEUNHAP.FirstOrDefault(x => x.MAPN == pn.MAPN);
            _pn.NGAYNHAP = pn.NGAYNHAP;
            _pn.SOLUONG = pn.SOLUONG;
            _pn.TONGTIEN = pn.TONGTIEN;
            _pn.GHICHU = pn.GHICHU;
            _pn.MANV = pn.MANV;
            _pn.MANCC = pn.MANCC;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }
        public void delete(string mapn)
        {
            tb_PHIEUNHAP pn = db.tb_PHIEUNHAP.FirstOrDefault(x => x.MAPN == mapn);
            db.tb_PHIEUNHAP.Remove(pn);
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