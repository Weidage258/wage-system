using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wagemanagement.Models;

namespace Wagemanagement.Controllers
{
    public class WagesController : Controller
    {
        // GET: Wages
        public ActionResult WagesIndex()
        {
            return View();
        }
        public string WagesSet(int page, int limit)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {

                var data = db.Wages_View.ToList();

                var data2 = data.Skip((page - 1) * limit).Take(limit).ToList();
                var d = new
                {
                    code = "0",
                    msg = ""
                    ,
                    count = data.Count
                    ,
                    data = data2
                };

                return JsonConvert.SerializeObject(d);
            }

        }
        //BounsRSel
        //奖金删除表
        [HttpPost]
        public bool WagesSel(int id)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var data = db.Wages_Records.Find(id);
                db.Wages_Records.Remove(data);
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
        public bool WagesSell(int id)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var data = db.Wages_Records.FirstOrDefault(p=>p.Staff_id==id);
                db.Wages_Records.Remove(data);
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
        //修改表试图
        public bool Wagesgongzi(int? id)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var yue = DateTime.Now.ToString("yyyy-MM");
                var data = db.Wages_Records.FirstOrDefault(p => p.Staff_id == id&&p.WR_remarks.Contains(yue));
                data.pay_of = "已发";
                if (db.SaveChanges()>0)
                {
                    return true;
                }
                return false;
            }
        }
        //批量删除
        [HttpPost]
        public JsonResult RemoveW(string[] id)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var yue = DateTime.Now.ToString("yyyy-MM");
                foreach (var item in id)
                {
                    int Staff_id = int.Parse(item);
                    var da = db.Wages_Records.FirstOrDefault(c => c.Staff_id == Staff_id && c.WR_remarks.Contains(yue));
                    da.pay_of = "已发";    
                }
                if (db.SaveChanges() > 0)
                {
                    return Json(new { state = 10000 });
                }
                ;
                return Json(new { state = 100020 });
            }
        }
        [HttpPost]
        //模糊查询奖金名数据
        public string WagesSelect(int page, int limit, string Staff_Name, string Staff_id, string WR_remarks, string Store_Name,string pay_of)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var data = db.Wages_View.Where(p => p.Staff_Name.Contains(Staff_Name) && p.Staff_id.ToString().Contains(Staff_id) && p.WR_remarks.ToString().Contains(WR_remarks) && p.Store_Name.Contains(Store_Name) && p.pay_of.Contains(pay_of)).ToList();
                var data2 = data.Skip((page - 1) * limit).Take(limit).ToList();
                var d = new { code = 0, msg = "", count = data.Count, data = data2 };
                return JsonConvert.SerializeObject(d);
            }
        }
    }
}