using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wagemanagement.Models;

namespace Wagemanagement.Controllers
{
    public class StoreController : Controller
    {
        // GET: Shop
        public ActionResult StoreIndex()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
               
                var Store = db.Store.ToList();
                return View(Store);
            }
           
        }
        //StoreSet
        //查询
        public string StoreSet(int page, int limit)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {

                var data = db.Store.ToList();

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
        //奖金删除表
        [HttpPost]
        public bool StoreSel(int id)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var data = db.Store.Find(id);
                db.Store.Remove(data);
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
        public ActionResult EditStore(int? id)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                ViewBag.staff = db.Staff.ToList();
                var b = db.Store.Find(id);
                return View(b);
            }
        }
        //UpdateStore
        //更新修改表数据
        public bool UpdateStore(Store Store)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var data = db.Store.FirstOrDefault(p => p.Store_Id == Store.Store_Id);
                var data1 = db.Store.Where(p => p.Store_Name == Store.Store_Name||p.Store_Address==Store.Store_Address||p.StoreLeader==Store.StoreLeader).ToList();//
                var data2 = db.Store.Where(p =>p.Store_Address == Store.Store_Address).ToList();
                var data3 = db.Store.Where(p => p.StoreLeader == Store.StoreLeader).ToList();
                var data4 = db.Store.Where(p => p.Store_Name == Store.Store_Name).ToList();
                if (data.Store_Name == Store.Store_Name&& data.StoreLeader==Store.StoreLeader&&data.Store_Address==Store.Store_Address)
                {
                    var b = db.Store.Find(Store.Store_Id);
                    b.Store_Remarks = Store.Store_Remarks;
                    b.Store_state = Store.Store_state;

                    if (db.SaveChanges() > 0)
                    {
                        return true;
                    }
                }
                else if (data.Store_Name == Store.Store_Name && data.StoreLeader == Store.StoreLeader && data.Store_Address != Store.Store_Address)
                {
                    if (data2.Count==0)
                    {
                        var b = db.Store.Find(Store.Store_Id);
                        b.Store_Remarks = Store.Store_Remarks;
                        b.Store_state = Store.Store_state;
                        b.Store_Address = Store.Store_Address;
                        if (db.SaveChanges() > 0)
                        {
                            return true;
                        }
                    
                    }
                    return false;
                }
                else if (data.Store_Name == Store.Store_Name && data.Store_Address== Store.Store_Address && data.StoreLeader != Store.StoreLeader )
                {
                    if (data3.Count == 0)
                    {
                        var c = db.Staff.FirstOrDefault(p => p.Staff_Name == data.StoreLeader);
                        c.Grade_Id = 2;
                        var d = db.Staff.FirstOrDefault(p => p.Staff_Name == Store.StoreLeader);
                        d.Grade_Id = 1;
                        var b = db.Store.Find(Store.Store_Id);
                        b.Store_Remarks = Store.Store_Remarks;
                        b.Store_state = Store.Store_state;
                        b.StoreLeader = Store.StoreLeader;
                      
                        if (db.SaveChanges() > 0)
                        {
                            return true;
                        }

                    }
                    return false;
                }
                else if (data.Store_Name != Store.Store_Name && data.StoreLeader == Store.StoreLeader && data.Store_Address == Store.Store_Address)
                {
                    if (data4.Count == 0)
                    {
                        var b = db.Store.Find(Store.Store_Id);
                        b.Store_Remarks = Store.Store_Remarks;
                        b.Store_state = Store.Store_state;
                        b.Store_Name = Store.Store_Name;
                       
                        if (db.SaveChanges() > 0)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                else
                {
                    if (data1.Count == 0)
                    {
                        var b = db.Store.Find(Store.Store_Id);
                        b.Store_Name = Store.Store_Name;
                       b.StoreLeader = Store.StoreLeader;
                        b.Store_Address = Store.Store_Address;
                        b.Store_Remarks = Store.Store_Remarks;
                        b.Store_state = Store.Store_state;
                        var c = db.Staff.FirstOrDefault(p => p.Staff_Name == data.StoreLeader && p.Store_Id == Store.Store_Id);
                        c.Grade_Id = 2;

                        if (db.SaveChanges() > 0)
                        {
                            return true;
                        }
                    }
                    return false;
                }

                return false;

                //return false;
                //var b = db.Store.Find(Store.Store_Id);
                //b.Store_Name = Store.Store_Name;
                //b.StoreLeader = Store.StoreLeader;
                //b.Store_Address = Store.Store_Address;
                //b.Store_Remarks = Store.Store_Remarks;
                //b.Store_state = Store.Store_state;


                //if (db.SaveChanges() > 0)
                //{
                //    return true;
                //}
                //return false;
            }
        }
        [HttpPost]
        //模糊查询奖金名数据
        public string StoreSelect(int page, int limit, string Store_Name,string Store_Address,string StoreLeader,string Store_state)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var data = db.Store.Where(p => p.Store_Name.Contains(Store_Name) && p.Store_Address.Contains(Store_Address) && p.StoreLeader.ToString().Contains(StoreLeader) && p.Store_state.ToString().Contains(Store_state)).ToList();

                var data2 = data.Skip((page - 1) * limit).Take(limit).ToList();
                var d = new { code = 0, msg = "", count = data.Count, data = data2 };

                return JsonConvert.SerializeObject(d);
            }

        }
        //添加表试图
        public ActionResult AddStore()
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                
                ViewBag.staff = db.Staff.Include("Store").Where(p=> p.Staff_Name!=p.Store.StoreLeader).ToList();
                return View();
            }
        }
        [HttpPost]
        //添加表数据
        public bool UpdateStoredate(Store store)
        {
            using (WagemanagementEntities db = new WagemanagementEntities())
            {
                var data = db.Store.FirstOrDefault(p => p.Store_Name == store.Store_Name||p.StoreLeader== store.StoreLeader||p.Store_Address==store.Store_Address);
                if (data == null)
                {
                    Store b = new Store
                    {
                        Store_Name = store.Store_Name,
                        Store_Address = store.Store_Address,
                        StoreLeader = store.StoreLeader,
                        Store_state = store.Store_state,
                        Store_Remarks = store.Store_Remarks

                    };
                    db.Store.Add(b);

                    if (db.SaveChanges() > 0)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
               
            }


        }
    }
}