using Microsoft.AspNetCore.Mvc;
using SmartMedical.BLL;
using SmartMedical.DAL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static SmartMedical.BLL.SmartMedicalBLL;

namespace SmartMedical.Controllers
{
    public class PatientController : Controller
    {
        SmartMedicalBLL _bll;
        LoginTel _logintel;
        DBHelper _db;
        public PatientController(SmartMedicalBLL bll, LoginTel logintel, DBHelper db)
        {
            _bll = bll;
            _logintel = logintel;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login(string phone,string password) 
        {
            string sql = $"select * from patient where patientphone='{phone}' and patientpassword='{password}'";
            DataSet ds = _db.GetDateSet(sql);
            return Ok(new { msg =ds.Tables[0].Rows.Count>0?"登录成功!":"账号或密码错误!",state=ds.Tables[0].Rows.Count > 0 ?true:false });
        }
        public IActionResult ZhuCe(string phone) 
        {
            int h = 0;
            string sql = $"select * from patient where patientphone='{phone}'";
            DataSet ds = _db.GetDateSet(sql);
            if (ds.Tables[0].Rows.Count>0)
            {
                h = 0;
            }
            else
            {
                sql = $"insert into patient(patientphone) values ('{phone}') ";
                h = _db.ExecuteNonQuery(sql);
            }
            return Ok(new { msg=h>0?"注册成功！":"手机号已存在!",state=h>0?true:false});
        }
        public IActionResult PhoneYZM(string phone) 
        {
            Random r = new Random();
            string code = r.Next(1000,9999).ToString();
            var str = _logintel.sendSmsCode(phone,code);
            return Ok(new { data=str,code=code});
        }
    }
}
