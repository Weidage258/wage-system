using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wagemanagement.Models;

namespace Wagemanagement.Controllers
{
    public class Staff_RPController : Controller
    {
        // GET: Staff_RP 密码管理
        public ActionResult Staff_RPIndex()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {

                var cwa = db.Staff.ToList();
                return View(cwa);
            }

        }
        //查询表数据
        public string Staff_RPSet(int page, int limit)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {

                var data = db.Staff.ToList();

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

        }//模糊查询
        public string Staff_RPSelect(int page, int limit, string Staff_id, string Staff_Name)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var data = db.Staff.Where(p => p.Staff_id.ToString().Contains(Staff_id) && p.Staff_Name.Contains(Staff_Name)).ToList();
                var data2 = data.Skip((page - 1) * limit).Take(limit).ToList();
                var d = new { code = 0, msg = "", count = data.Count, data = data2 };
                return JsonConvert.SerializeObject(d);
            }
        }
        public ActionResult EditStaff_RP(int? id)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var b = db.Staff.Find(id);
                //var Role_id = b.Role_id;
                //var Grade_Id = b.Grade_Id;
                //var Store_Id = b.Store_Id;
                //var Department_Id = b.Department_Id;
                //ViewBag.Grade1 = db.Grade.ToList();
                //ViewBag.Store1 = db.Store.ToList();
                //ViewBag.Department1 = db.Department.ToList();
                //ViewBag.Role = db.Role.FirstOrDefault(c => c.Role_id == Role_id).Role_Name;
                //ViewBag.Grade = db.Grade.FirstOrDefault(c => c.Grade_Id == Grade_Id).GradeName;
                //ViewBag.Store = db.Store.FirstOrDefault(c => c.Store_Id == Store_Id).Store_Name;
                //ViewBag.Department = db.Department.FirstOrDefault(c => c.Department_Id == Department_Id).Dt_Name;
                //ViewBag.info1 = db.Staff.ToList();
                return View(b);
            }
        }
        //更新修改表数据
        public bool UpdateStaff_RP(Staff staff1)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var staff = db.Staff.Find(staff1.Staff_id);
                
                staff.Staff_Pwd = staff1.Staff_Pwd;
              
                if (db.SaveChanges() > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}