using SmartMedical.DAL;
using SmartMedical.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SmartMedical.BLL
{
    public class SmartMedicalBLL
    {
        DBHelper _db;
        public SmartMedicalBLL(DBHelper db)
        {
            _db = db;
        }
        public int Login(Admin m) 
        {
            string sql = $"select * from admin where adminname='{m.AdminName}' and adminpassword='{m.AdminPassWord}'";
            DataSet ds = _db.GetDateSet(sql);
            int h = ds.Tables[0].Rows.Count;
            return h;
        }
    }
}
