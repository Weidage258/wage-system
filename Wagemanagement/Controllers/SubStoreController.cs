using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wagemanagement.Models;

namespace Wagemanagement.Controllers
{
    public class SubStoreController : Controller
    {
        // GET: SubStore
       
        public ActionResult SubStoreIndex()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var Staff_id = Session["Staff_id"];
                var Staff_Name = db.Staff.Find(Staff_id).Staff_Name;
                var Store_Id = db.Store.FirstOrDefault(p => p.StoreLeader == Staff_Name).Store_Id;
                var cwa = db.Staff.Where(p => p.Store_Id == Store_Id && p.Sraff_State == "在职").ToList();
                return View(cwa);
            }

        }
        //查询表数据
        public string SubStoreSet(int page, int limit)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var Staff_id = Session["Staff_id"];
                var Staff_Name = db.Staff.Find(Staff_id).Staff_Name;
                var Store_Id = db.Store.FirstOrDefault(p => p.StoreLeader == Staff_Name).Store_Id;


                var data = db.Staff.Where(p=>p.Store_Id==Store_Id&&p.Sraff_State=="在职").ToList();

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
        //添加考勤
        public ActionResult EditSubStore(int? id)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                ViewBag.Staff_id = id;
                ViewBag.info1 = db.Cwa.ToList();
                return View();
            }
        }
        [HttpPost]
        public string SubStoreSelect(int page,int limit,string Staff_id,string Staff_Name)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {

                var Staff_id1 = Session["Staff_id"];
                var Staff_Name1 = db.Staff.Find(Staff_id1).Staff_Name;
                var Store_Id = db.Store.FirstOrDefault(p => p.StoreLeader == Staff_Name1).Store_Id;
                var data = db.Staff.Where(p => p.Store_Id == Store_Id && p.Sraff_State == "在职"&&p.Staff_id.ToString().Contains(Staff_id) && p.Staff_Name.Contains(Staff_Name)).ToList();
                var data2 = data.Skip((page - 1) * limit).Take(limit).ToList();
                var d = new { code = 0, msg = "", count = data.Count, data = data2 };
                return JsonConvert.SerializeObject(d);
            }
        }
        [HttpPost]
        //添加表数据
        public bool UpdateSubStore(Cwa_Rd_View cwa_Rd_View)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var chunzai = db.Cwa_Records.FirstOrDefault(p => p.Staff_id == cwa_Rd_View.Staff_id);
                if (chunzai != null)
                {
                    var date = DateTime.Now.ToString("yyyy-MM");
                    var Cwaid = db.Cwa.FirstOrDefault(p => p.Cwa_Name == cwa_Rd_View.Cwa_Name).Cwa_id;

                    var cun = db.Cwa_Records.Where(p => p.CR_date.ToString().Contains(date)).FirstOrDefault(p => p.Staff_id == cwa_Rd_View.Staff_id && p.Cwa_id == Cwaid);
                    if (cun != null)
                    {

                        cun.CR_Frequency += 1;


                        if (db.SaveChanges() > 0)
                        {
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        var jiang = db.Cwa.FirstOrDefault(p => p.Cwa_Name == cwa_Rd_View.Cwa_Name);
                        Cwa_Records b = new Cwa_Records
                        {
                            Staff_id = cwa_Rd_View.Staff_id,
                            Cwa_id = jiang.Cwa_id,
                            CR_Frequency = 1,
                            CR_date = DateTime.Today
                        };
                        db.Cwa_Records.Add(b);

                        if (db.SaveChanges() > 0)
                        {
                            return true;
                        }
                        return false;
                    }

                }
                else
                {
                    var jiang = db.Cwa.FirstOrDefault(p => p.Cwa_Name == cwa_Rd_View.Cwa_Name);

                    Cwa_Records b = new Cwa_Records
                    {
                        Staff_id = cwa_Rd_View.Staff_id,
                        Cwa_id = jiang.Cwa_id,
                        CR_Frequency = 1,
                        CR_date = DateTime.Today
                    };
                    db.Cwa_Records.Add(b);

                    if (db.SaveChanges() > 0)
                    {
                        return true;
                    }
                    return false;

                }

            }


        }
        [HttpPost]
        public JsonResult Wage(string[] id)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
               
                foreach (var item in id)
                {
                    int Staff_id = int.Parse(item);
                    var yue = DateTime.Now.ToString("yyyy-MM");
                    var jilu = db.Wages_Records.Where(p => p.WR_remarks.Contains(yue)).FirstOrDefault(p => p.Staff_id == Staff_id);
                    if (jilu!=null)
                    {
                        return Json(new { state = 100030,msg=Staff_id });
                    }

                  
                    var Staff = db.Staff.FirstOrDefault(p => p.Staff_id == Staff_id);
                    //月份
                  
                    //拿到符合条件的补贴记录
                    var Subsidy = db.Subsidy_Records.Where(p => p.Staff_id== Staff_id&&p.SR_date.ToString().Contains(yue)).ToList();
                    decimal? SubsidyAmount = 0;
                    foreach (var item1 in Subsidy)
                    {
                        var SR_id = item1.SR_Id;

                         var Subsidy_id = db.Subsidy_Records.FirstOrDefault(p => p.SR_Id == SR_id).Subsidy_id;
                        var Subsidy_pirce = db.Subsidy.FirstOrDefault(p => p.Subsidy_id == Subsidy_id).Subsidy_pirce;
                        SubsidyAmount += Subsidy_pirce;
                    }
                    //拿到全部关于这id考勤扣款
                    var Grade_id = db.Staff.FirstOrDefault(p => p.Staff_id == Staff_id).Grade_Id;
                    var price = db.Grade.FirstOrDefault(p => p.Grade_Id == Grade_id).Grade_price;
                    var Cwa = db.Cwa_Records.Where(p => p.Staff_id == Staff_id && p.CR_date.ToString().Contains(yue)).ToList();
                    decimal? Deduction = 0;
                    foreach (var item1 in Cwa)
                    {
                        var CR_id = item1.CR_id;
                        var Cwa_id = db.Cwa_Records.FirstOrDefault(p => p.CR_id == CR_id).Cwa_id;
                        var CR_Frequency = db.Cwa_Records.FirstOrDefault(p => p.CR_id == CR_id).CR_Frequency;
                        var Subsidy_pirce = db.Cwa.FirstOrDefault(p => p.Cwa_id == Cwa_id).Cwa_pirce;
                        Deduction += CR_Frequency*(price/30*Subsidy_pirce);
                    }
                    //拿到全部关于这id的奖金
                    var Bonus = db.Bonus_Records.Where(p => p.Staff_id == Staff_id && p.CR_date.ToString().Contains(yue)).ToList();
                    decimal? WR_Bonus = 0;
                    foreach (var item1 in Bonus)
                    {
                        var BR_id = item1.BR_id;

                        var Bonus_Id = db.Bonus_Records.FirstOrDefault(p => p.BR_id == BR_id).Bonus_Id;
                        var Bonus_pirce = db.Bonus.FirstOrDefault(p => p.Bonus_Id == Bonus_Id).Bonus_pirce;
                        WR_Bonus += Bonus_pirce;
                    }
                    //应发工资
                    var WR_Pay = price - Deduction + WR_Bonus + SubsidyAmount;
                    //实发工资
                    var Real_Wage = price + WR_Bonus + SubsidyAmount;
                    Wages_Records wages_Records = new Wages_Records
                    {
                        Staff_id = Staff_id,
                        Grade_Id = Staff.Grade_Id,
                        Store_Id = Staff.Store_Id,
                        SubsidyAmount = SubsidyAmount,
                        Deduction = Deduction,
                        WR_Bonus = WR_Bonus,
                        WR_Pay = WR_Pay,
                        Real_Wage = Real_Wage,
                        WR_remarks = yue+"份工资",
                        pay_of="未发"
                    };
                    db.Wages_Records.Add(wages_Records);
                }
             
             
                if (db.SaveChanges() > 0)
                {
                    
                    return Json(new { state = 10000 });
                }
                
                return Json(new { state = 100020 });
            }
        }
    }
}