using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    public class _TestController : Controller
    {
        [Authorize(Roles = WebUserRoles.Accountant)]
        public ActionResult CheckAuth()
        {
            return Json(User.GetUserData(), JsonRequestBehavior.AllowGet);
        }
        // GET: _Test
        public ActionResult Index()
        {
            return View();
        }

        // GET: _Test/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: _Test/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: _Test/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: _Test/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: _Test/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: _Test/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: _Test/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
