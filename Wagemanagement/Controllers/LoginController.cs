using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wagemanagement.Models;
using System.Web.Helpers;

namespace Wagemanagement.Controllers
{
    public class LoginController : Controller
    {
        string bodyStr="okkkk";
            string SmtpServer= "smtp.qq.com";
        int SmtpPort=25; 
        bool EnableSsl=true;
        string UserName="914654258@qq.com";
        string Password= "xtbinogrxkhfbchd";
        string FromEmail="914654258@qq.com";
        string ToEmail="914654258@qq.com";
        string Subject ="对对对";

        public ActionResult Index()
        {

            return View();

        }
        // GET: Login
        //登录界面
        [HttpPost]
        public ActionResult Index(Staff staff)
        {

            if (staff.Staff_Remark.ToLower() != Session["sn"].ToString())
            {
                return Json(new
                {
                    message = "验证码错误!",
                    sadawd = false
                });
            }
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var yuangong = db.Staff.Where(s => s.Staff_id == staff.Staff_id && s.Staff_Pwd == staff.Staff_Pwd).FirstOrDefault();
                var lizhi = db.Staff.Where(s => s.Staff_id == staff.Staff_id && s.Staff_Pwd == staff.Staff_Pwd && s.Sraff_State == "离职").FirstOrDefault();
                if (yuangong == null)
                {
                    return Json(new
                    {
                        message = "账号或者密码错误！",
                        sadawd = false
                    });
                }
                else if (lizhi != null)
                {
                    return Json(new
                    {
                        message = "您已经离职！无法登录该系统",
                        sadawd = false
                    });
                }
                else
                {
                    Session["Staff_id"] = yuangong.Staff_id;
                    Session["id"] = yuangong.Role_id;
                    Session["Login"] = yuangong.Staff_Name;
                    if (yuangong.Role_id == 1)
                    {
                        WebMail.SmtpServer = SmtpServer;
                        WebMail.SmtpPort = SmtpPort;
                        WebMail.EnableSsl = EnableSsl;
                        WebMail.UserName = UserName;
                        WebMail.Password = Password;
                        WebMail.From = FromEmail;
                        WebMail.Send(ToEmail, Subject, bodyStr);


                        return Json(new
                        {
                            message = "账号或者密码错误！",
                            sadawd = "Index"
                        });
                        //return RedirectToAction("Index", "Home");
                    }
                    else if (yuangong.Role_id == 2)
                    {
                        return Json(new
                        {
                            message = "账号或者密码错误！",
                            sadawd = "IndexDian"
                        });
                        //return RedirectToAction("IndexDian","Home");
                    }
                    return Json(new
                    {
                        message = "账号或者密码错误！",
                        sadawd = "IndexStaff"
                    });
                    //return RedirectToAction("IndexStaff", "Home");
                }


            }

        }
        public ActionResult Niu(int id)
        {


            if (id == 1)
            {
                return Content("管理员");
            }
            else if (id == 2)
            {
                return Content("店部管理者");
            }

            return Content("普通员工");
        }


    }
}