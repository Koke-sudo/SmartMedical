using Microsoft.AspNetCore.Mvc;
using SmartMedical.BLL;
using SmartMedical.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMedical.Controllers
{
    public class SmartApiController : Controller
    {
        SmartMedicalBLL _bll;
        public SmartApiController(SmartMedicalBLL bll)
        {
            _bll = bll;
        }
        public IActionResult Index()
        {
            return View();
        }
        //管理员登录
        public IActionResult Login(Admin m) 
        {
            int h = _bll.Login(m);
            return Ok(new { msg=h>0?"登陆成功!":"用户名或密码不正确!",state=h>0?true:false});
        }
    }
}
