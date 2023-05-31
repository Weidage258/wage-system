using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wagemanagement.Filer;
using Wagemanagement.Models;

namespace Wagemanagement.Controllers
{
    public class HomeController : Controller
    {
        //主页面
        [yanzhen]
        public ActionResult Index()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                ViewBag.dianpu = db.Store.ToList().Count();
                ViewBag.Staff = db.Staff.ToList().Count();
                ViewBag.sum=  db.Wages_Records.Where(p => p.pay_of == "未发").Sum(p=>p.WR_Pay);
                return View();
            }

        }
        //店铺登陆界面
        [yanzhen]
        public ActionResult IndexDian()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var staff_id = (int)Session["Staff_id"];
                var store_id= db.Staff.FirstOrDefault(p => p.Staff_id == staff_id).Store_Id;
                ViewBag.staff = db.Staff.Where(p => p.Store_Id == store_id).Count();
                var store = db.Store.FirstOrDefault(p => p.Store_Id == store_id).Store_Name;
                var yue = DateTime.Now.ToString("yyyy-MM");
                ViewBag.store = db.Cwa_Rd_View.Where(p => p.Store_Name == store&&p.CR_date.ToString().Contains(yue)).Count();
                ViewBag.Bonus=db.Bonus_Rd_View.Where(p => p.Store_Name == store && p.CR_date.ToString().Contains(yue)).Count();
                return View();
            }

        }
        //员工登陆界面
        [yanzhen]
        public ActionResult IndexStaff()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                //公司获得工资总数
                var yue = DateTime.Now.ToString("yyyy-MM");
                var staff_id = (int)Session["Staff_id"];
                var yue1 = DateTime.Now.Month - 1;
                var yue2 = DateTime.Now.Year;
                string yuee = yue2 + "-0" + yue1;
                 var sum =db.Wages_View.Where(p => p.Staff_id == staff_id && p.pay_of == "已发").Sum(p => p.WR_Pay);
                var b = db.Wages_View.FirstOrDefault(p => p.Staff_id == staff_id && p.pay_of == "已发" && p.WR_remarks.Contains(yue));
                var c =  db.Wages_View.FirstOrDefault(p => p.Staff_id == staff_id && p.pay_of == "已发" && p.WR_remarks.Contains(yuee));
                if (sum!=null)
                {
                    ViewBag.sum = sum;
                }else {
                    ViewBag.sum = "0";
                }
                if (b != null)
                {
                    ViewBag.wage = b.WR_Pay;
                }
                else if (c != null)
                {
                    ViewBag.wage = c.WR_Pay;
                }
                else {
                    ViewBag.wage = "近两个月没有";
                }
                var d= db.Wages_View.FirstOrDefault(p => p.Staff_id == staff_id && p.pay_of == "已发" && p.WR_remarks.Contains(yue));
                var e = db.Wages_View.FirstOrDefault(p => p.Staff_id == staff_id && p.pay_of == "已发" && p.WR_remarks.Contains(yuee));
                if (d != null)
                {
                    ViewBag.duction = d.Deduction;
                }
                else if (e != null)
                {
                    ViewBag.duction = e.Deduction;
                }
                else
                {
                    ViewBag.duction = "近两个月没有";
                }
                return View();
            }

        }
        //修改密码
        public ActionResult modifyPwd()
        {
            return View();
        }
        //修改密码
        [HttpPost]
        public ActionResult ModifyPwd(string password1, string password2,string password3, int id)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                {

                    var data = db.Staff.FirstOrDefault(p => p.Staff_Pwd == password1);
                    if (data == null)
                    {
                        return Json(new
                        {
                            message = "原密码错误",
                            sadawd = false
                        });
                    }
                    else if (password1 == password2)
                    {
                        return Json(new
                        {
                            message = "原密码与修改密码一致",
                            sadawd = false
                        });
                    }
                    else if (password2 != password3)
                    {
                        return Json(new
                        {
                            message = "两次密码不一致",
                            sadawd = false
                        });
                    }
                  

                    else
                    {
                        var userObject = db.Staff.FirstOrDefault(p => p.Staff_id == id);
                        if (userObject != null)
                        {
                            userObject.Staff_Pwd = password2;

                        }
                        db.SaveChanges();


                        return Json(new
                        {
                            message = "正确",
                            sadawd = true
                        });
                    }
                  
                  
                }

            }

        }
        //奖金
        public ActionResult Bonus()
        {
            using (WagemanagementEntities db = new WagemanagementEntities()) {

                var bouns = db.Bonus.ToList();
                return View(bouns);
            }
                
        }
        public ActionResult Bounus(int id)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {

                var bouns = db.Bonus.FirstOrDefault(p=>p.Bonus_Id==id);
                if (bouns!=null)
                {
                    db.Bonus.Remove(bouns);
                    db.SaveChanges();
                    return RedirectToAction("Home", "Index");
                }

                return View(bouns);
            }

        }

        public ActionResult GetCharts()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {

                var Grade = db.Grade.Select(p => new { p.GradeName, p.Grade_price }).ToList();
                return Json(Grade, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetChartss()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var staff_id = (int)Session["Staff_id"];
                var store_id = db.Staff.FirstOrDefault(p => p.Staff_id == staff_id).Store_Id;
                var Store_Name = db.Store.FirstOrDefault(p => p.Store_Id == store_id).Store_Name;
                //var yue01 = DateTime.Now.ToString("yyyy-MM(-1)");
                var yue1 = DateTime.Now.Month-1;
                var yue2 = DateTime.Now.Year;
                string yue = yue2  +"-0"+ yue1;
                var Store =  db.Wages_View.Where(p => p.Store_Name == Store_Name && p.WR_remarks.Contains(yue.ToString())).Select(p => new { p.Staff_Name,p.Grade_price,p.SubsidyAmount,p.WR_Bonus,p.Deduction,p.WR_Pay,p.Real_Wage}).ToList();   
                return Json(Store, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetChartsss()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var staff_id = (int)Session["Staff_id"];
             
                var Staff_Name = db.Staff.FirstOrDefault(p => p.Staff_id == staff_id).Staff_Name;
                //var yue01 = DateTime.Now.ToString("yyyy-MM(-1)");
                var yue1 = DateTime.Now.Month - 1;
                var yue2 = DateTime.Now.Year;
                string yue = yue2 + "-0" + yue1;
                var Store = db.Wages_View.Where(p => p.Staff_Name == Staff_Name&&p.pay_of=="已发").Select(p => new { p.WR_remarks, p.Grade_price, p.SubsidyAmount, p.WR_Bonus, p.Deduction, p.WR_Pay, p.Real_Wage }).ToList();
                return Json(Store, JsonRequestBehavior.AllowGet);
            }
        }
    }
}