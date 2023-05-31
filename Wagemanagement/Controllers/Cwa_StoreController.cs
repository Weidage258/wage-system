using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wagemanagement.Models;

namespace Wagemanagement.Controllers
{
    public class Cwa_StoreController : Controller
    {
        // GET: Cwa_Store
        //店铺考勤管理
        public ActionResult Cwa_StoreIndex()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                return View();
            }

        }
        //查询本店铺的考勤情况
        public string Cwa_StoreSet(int page, int limit)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var Staff_id = Session["Staff_id"];
                var Staff_Name = db.Staff.Find(Staff_id).Staff_Name;
                var Store_Name = db.Store.FirstOrDefault(p => p.StoreLeader == Staff_Name).Store_Name;
                var data = db.Cwa_Rd_View.Where(p=>p.Store_Name== Store_Name).ToList();
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

        //批量删除
        [HttpPost]
        public JsonResult RemoveS(string[] id)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {

                foreach (var item in id)
                {
                    int CR_id = int.Parse(item);
                    var da = db.Cwa_Records.FirstOrDefault(c => c.CR_id == CR_id);

                    db.Cwa_Records.Remove(da);
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
        public string Cwa_StoreSelectS(int page, int limit, string Cwa_Name, string Staff_id, string CR_date, string CR_Frequency)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var Staff_id1 = Session["Staff_id"];
                var Staff_Name = db.Staff.Find(Staff_id1).Staff_Name;
                var Store_Name = db.Store.FirstOrDefault(p => p.StoreLeader == Staff_Name).Store_Name;
                var data = db.Cwa_Rd_View.Where(p => p.Store_Name == Store_Name&& p.Cwa_Name.Contains(Cwa_Name) && p.Staff_id.ToString().Contains(Staff_id) && p.CR_date.ToString().Contains(CR_date) && p.CR_Frequency.ToString().Contains(CR_Frequency)).ToList();
                
                var data2 = data.Skip((page - 1) * limit).Take(limit).ToList();
                var d = new { code = 0, msg = "", count = data.Count, data = data2 };
                return JsonConvert.SerializeObject(d);
            }
        }

    }
}