using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class NHOMHH
    {
        Entities db;
        public NHOMHH()
        {
            db = Entities.CreateEntities();
        }
        public tb_NHOMHH getItem(string manhom)
        {
            return db.tb_NHOMHH.FirstOrDefault(x => x.MANHOM == manhom);
        }
        public List<tb_NHOMHH> getAll()
        {
            return db.tb_NHOMHH.ToList();
        }
        public void add(tb_NHOMHH nhomhh)
        {
            try
            {
                db.tb_NHOMHH.Add(nhomhh);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }
        public void update(tb_NHOMHH nhomhh)
        {
            tb_NHOMHH _nhomhh = db.tb_NHOMHH.FirstOrDefault(x => x.MANHOM == nhomhh.MANHOM);
            _nhomhh.TENNHOM = nhomhh.TENNHOM;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu." + ex.Message);
            }
        }
        public void delete(string manhom)
        {
            tb_NHOMHH nhomhh = db.tb_NHOMHH.FirstOrDefault(x => x.MANHOM == manhom);
            db.tb_NHOMHH.Remove(nhomhh);
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
