using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class NHACC
    {
        Entities db;
        public NHACC()
        {
            db = Entities.CreateEntities();
        }
        public tb_NHACC getItem(string mancc)
        {
            return db.tb_NHACC.FirstOrDefault(x => x.MANCC == mancc);
        }
        public List<tb_NHACC> getAll()
        {
            return db.tb_NHACC.ToList();
        }
        public void add(tb_NHACC ncc)
        {
            try
            {
                db.tb_NHACC.Add(ncc);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }
        public void update(tb_NHACC ncc)
        {
            tb_NHACC _ncc = db.tb_NHACC.FirstOrDefault(x => x.MANCC == ncc.MANCC);
            _ncc.TENNCC = ncc.TENNCC;
            _ncc.DIACHI = ncc.DIACHI;
            _ncc.SDT = ncc.SDT;
            _ncc.EMAIL = ncc.EMAIL;
            _ncc.GHI_CHU = ncc.GHI_CHU;
            _ncc.MASOTHUE = ncc.MASOTHUE; 
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }
        public void delete(string mancc)
        {
            tb_NHACC ncc = db.tb_NHACC.FirstOrDefault(x => x.MANCC == mancc);
            db.tb_NHACC.Remove(ncc);
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