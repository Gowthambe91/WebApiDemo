using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    public class StudentsController : Controller
    {
        //
        // GET: /Student/
        public ActionResult Index()
        {
            StudentViewModel students = null;
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder("http://localhost:58710/api/student");
                builder.Query = "studentId=1";
                //client.BaseAddress = builder.Uri;
                //client

                var responseTask = client.GetAsync(builder.Uri);

                responseTask.Wait();

                var responseResult = responseTask.Result;

                if (responseResult.IsSuccessStatusCode)
                {
                    var readTask = responseResult.Content.ReadAsAsync<StudentViewModel>();
                    readTask.Wait();

                    students = readTask.Result;
                }
                else
                {
                    students = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return View(students);
        }
	}
}