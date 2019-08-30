using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApiDemo.Models;
using System.Configuration;

namespace WebApiDemo.Controllers
{
    public class StudentsController : Controller
    {
        //
        // GET: /Student/
        string ApiUri = ConfigurationManager.AppSettings["ApiBaseUri"].ToString();
        public ActionResult Index()
        {
            List<StudentViewModel> students = null;
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder("http://localhost:58710/api/student");
                //builder.Query = "studentId=1"; //This is to add parameter to the Uri -- commenting this will call GetStudents method and fetch all students
                //client.BaseAddress = builder.Uri;
                //else Client
                //client

                var responseTask = client.GetAsync(builder.Uri);

                responseTask.Wait();

                var responseResult = responseTask.Result;

                if (responseResult.IsSuccessStatusCode)
                {
                    var readTask = responseResult.Content.ReadAsAsync<List<StudentViewModel>>();
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

        public ActionResult GetStudentById(int studentId)
        {
            StudentViewModel students = null;
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder("http://localhost:58710/api/student");
                builder.Query = "studentId=" + studentId; //This is to add parameter to the Uri -- commenting this will call GetStudents method and fetch all students
                //client.BaseAddress = builder.Uri;
                //else Client
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentViewModel student)
        {
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder(ApiUri + "/student");
                var postTask = client.PostAsJsonAsync(builder.Uri, student);
                postTask.Wait();

                if (postTask.Result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator..");

            return View(student);
        }

        public ActionResult Edit(int id)
        {
            StudentViewModel student = null;

            using (var cdx = new MyTestDBEntities())
            {
                student = cdx.Students.Select(s => new StudentViewModel()
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Id = s.StudentID
                }).FirstOrDefault();
            }

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StudentViewModel student)
        {
            UriBuilder builder = new UriBuilder(ApiUri + "/student?studentId=" + student.Id);
            using (var client = new HttpClient())
            {
                var putTask = client.PutAsJsonAsync(builder.Uri, student);
                putTask.Wait();

                if (putTask.Result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "Server error.. Please contact administrator.");
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            UriBuilder builder = new UriBuilder(ApiUri + "/student?studentId=" + id);
            using (var client = new HttpClient())
            {
                var deleteTask = client.DeleteAsync(builder.Uri);
                deleteTask.Wait();

                if (deleteTask.Result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "Server error.. Please contact administrator..");

                return Content(string.Empty);
            }
        }
    }
}