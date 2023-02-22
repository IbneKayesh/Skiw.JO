using Newtonsoft.Json;
using Skiw.JO.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skiw.JO.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Class2Json()
        {
            Employee emp = new Employee { Id = 1, Name = "Kayesh", Email = "example@exam.ple", Designation = "Manager", Department = "IT", Salary = 120 };
            string json = Services.Db.JCHelper.ConvertToJson<Employee>(emp);
            ViewBag.MyString = json;
            return View();
        }
        public ActionResult Json2Class()
        {
            string json = "{\"Id\":1,\"Name\":\"Kayesh\",\"Email\":\"example@exam.ple\",\"Designation\":\"Manager\",\"Salary\":120,\"Department\":\"IT\"}";
            Employee emp = Services.Db.JCHelper.ConvertFromJson<Employee>(json);
            return View(emp);
        }
        public ActionResult Json2UnknownClass()
        {
            string json = "{\"Id\":1,\"Name\":\"Kayesh\",\"Email\":\"example@exam.ple\",\"Designation\":\"Manager\",\"Salary\":120,\"Department\":\"IT\"}";
            DataTable emp = Services.Db.JCHelper.ToDataTable(json);
            return View(emp);
        }

        public ActionResult DatatableInput()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Gender");
            dt.Columns.Add("Contact");
            dt.Columns.Add("Age");
            dt.Rows.Add();
            return View(dt);
        }
        [HttpPost]
        public ActionResult DatatableInput(FormCollection form)
        {
            // Get the submitted data from the Request.Form collection
            var formData = new DataTable();
            foreach (var columnName in Request.Form.AllKeys)
            {
                formData.Columns.Add(columnName);
            }
            var dataRow = formData.NewRow();
            foreach (var columnName in Request.Form.AllKeys)
            {
                dataRow[columnName] = Request.Form[columnName];
            }
            formData.Rows.Add(dataRow);
            return RedirectToAction("DatatableInput");
        }
    }
}