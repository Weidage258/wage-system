using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wagemanagement.Models;

namespace Wagemanagement.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult NewsIndex()
        {
            return View();
        }
        //页面拿数据
        public string Shuju()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                
                var data = db.News.OrderByDescending(p => p.New_id).First(p=> p.New_Show ==1);
                return JsonConvert.SerializeObject(data);
            }
        }
        //公告效果拿数据
        public string Shuju1()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {

                var data = db.News.OrderByDescending(p => p.New_id).First(p => p.New_Show == 0);
                return JsonConvert.SerializeObject(data);
            }
        }
        [HttpPost]
        public bool NewsFu(string nai)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {

                News news = new News();
                news.New_content = nai;
                news.New_Show = 0;
                db.News.Add(news);
                if (db.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        [HttpPost]
        public bool NewsFus(string nai)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
            var New_id = db.News.FirstOrDefault(p => p.New_content == nai).New_id;
                var News = db.News.FirstOrDefault(p => p.New_id == New_id);
                News.New_Show = 1;
                if (db.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        [HttpPost]
        public JsonResult Upload()
        {

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase f = Request.Files[0];//最简单的获取方法
                f.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "Imgs//" + f.FileName);//保存图片

                //这下面是返回json给前端 
                var data1 = new
                {
                    src = "/Imgs/" + f.FileName//服务器储存路径
                };
                var Person = new
                {
                    code = 0,//0表示成功
                    msg = "",//这个是失败返回的错误
                    data = data1
                };
                return Json(Person);//格式化为json
            }
            else
            {
                return null;
            }


        }


    }
}