using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wagemanagement.Models;

namespace Wagemanagement.Controllers
{
    public class StaffWagesController : Controller
    {
        // GET: StaffWages
        
        public ActionResult StaffWagesIndex()
        {
            return View();
        }
        public ActionResult StaffBounsIndex()
        {
            return View();
        }
         public ActionResult StaffSubsidyIndex()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var sub = db.Subsidy.ToList();
                return View(sub);
            }

        }

        public ActionResult StaffCwaIndex()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {

                var cwa = db.Cwa.ToList();
                return View(cwa);
            }

        }
        public string CwaRSet(int page, int limit)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {

                int Staff_id = (int)Session["Staff_id"];
                var data = db.Cwa_Rd_View.Where(p => p.Staff_id == Staff_id).ToList();

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
        [HttpPost]
        //模糊查询奖金名数据
        public string CwaSelectS(int page, int limit, string CR_date)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                int Staff_id = (int)Session["Staff_id"];
                var data = db.Cwa_Rd_View.Where(p => p.Staff_id == Staff_id && p.CR_date.ToString().Contains(CR_date)).ToList();
                var data2 = data.Skip((page - 1) * limit).Take(limit).ToList();
                var d = new { code = 0, msg = "", count = data.Count, data = data2 };
                return JsonConvert.SerializeObject(d);
            }
        }
        public string SubRSet(int page, int limit)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {

                int Staff_id = (int)Session["Staff_id"];
                var data = db.Subsidy_View.Where(p => p.Staff_id == Staff_id).ToList();
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
        public string WagesSet(int page, int limit)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                int Staff_id = (int)Session["Staff_id"];
                var data = db.Wages_View.Where(p=>p.Staff_id == Staff_id&&p.pay_of=="已发").ToList();
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
        public string BounsRSet(int page, int limit)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                int Staff_id = (int)Session["Staff_id"];
                var data = db.Bonus_Rd_View.Where(p => p.Staff_id == Staff_id).ToList();
              
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
        [HttpPost]
        //模糊查询奖金名数据
        public string WagesSelect(int page, int limit,string WR_remarks)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                int Staff_id = (int)Session["Staff_id"];
                var data = db.Wages_View.Where(p => p.Staff_id == Staff_id && p.pay_of == "已发"&& p.WR_remarks.ToString().Contains(WR_remarks)).ToList();
                var data2 = data.Skip((page - 1) * limit).Take(limit).ToList();
                var d = new { code = 0, msg = "", count = data.Count, data = data2 };
                return JsonConvert.SerializeObject(d);
            }
        }
        [HttpPost]
        //模糊查询奖金名数据
        public string BounsSelectS(int page, int limit, string CR_date)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                int Staff_id = (int)Session["Staff_id"];
                var data = db.Bonus_Rd_View.Where(p => p.Staff_id == Staff_id&&p.CR_date.ToString().Contains(CR_date)).ToList();
                var data2 = data.Skip((page - 1) * limit).Take(limit).ToList();
                var d = new { code = 0, msg = "", count = data.Count, data = data2 };
                return JsonConvert.SerializeObject(d);
            }
        }
        public string SubSelectS(int page, int limit, string SR_date)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var data = db.Subsidy_View.Where(p => p.SR_date.ToString().Contains(SR_date)).ToList();
                var data2 = data.Skip((page - 1) * limit).Take(limit).ToList();
                var d = new { code = 0, msg = "", count = data.Count, data = data2 };
                return JsonConvert.SerializeObject(d);
            }
        }
    }
}