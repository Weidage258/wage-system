using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wagemanagement.Models;

namespace Wagemanagement.Controllers
{
    public class Staff_RoleController : Controller
    {
        // GET: Staff_Role
        public ActionResult Staff_RoleIndex()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {

                var cwa = db.Staff_View.ToList();
                return View(cwa);
            }

        }
        //查询表数据
        public string Staff_RoleSet(int page, int limit)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {

                var data = db.Staff_View.ToList();

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
        
        public string Staff_RoleSelect(int page, int limit, string Staff_id, string Staff_Name)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var data = db.Staff_View.Where(p => p.Staff_id.ToString().Contains(Staff_id) && p.Staff_Name.Contains(Staff_Name)).ToList();
                var data2 = data.Skip((page - 1) * limit).Take(limit).ToList();
                var d = new { code = 0, msg = "", count = data.Count, data = data2 };
                return JsonConvert.SerializeObject(d);
            }
        }
        public ActionResult EditStaff_Role(int? id)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
             
               
                var b = db.Staff.Find(id);
                var Role_id = b.Role_id;
                ViewBag.Role = db.Role.ToList();
                ViewBag.Role_Name = db.Role.FirstOrDefault(c => c.Role_id == Role_id).Role_Name;
                return View(b);
            }
        }
        //更新修改表数据
        public bool UpdateStaff_Role(Staff_View staff_View)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var Role_id = db.Role.FirstOrDefault(p => p.Role_Name == staff_View.Role_Name).Role_id;


                var staff = db.Staff.Find(staff_View.Staff_id);

                staff.Role_id = Role_id;

                if (db.SaveChanges() > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}