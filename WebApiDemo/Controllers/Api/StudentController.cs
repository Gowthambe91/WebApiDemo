using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    /*SEE THE THIRD ACTION METHOD TO UNDERSTAND WHY THE FIRST TWO ARE COMMENTED OUT*/
    public class StudentController : ApiController
    {
        public IHttpActionResult GetAllStudents() /*Un commenting this, because the third was not working as expected due to Table relationship*/
        {
            IList<StudentViewModel> students = null;

            using (var ctx = new MyTestDBEntities())
            {
                students = ctx.Students
                    .Select(s => new StudentViewModel()
                    {
                        Id = s.StudentID,
                        FirstName = s.FirstName,
                        LastName = s.LastName
                    }).ToList<StudentViewModel>();
            }

            if (students.Count == 0)
            {
                return NotFound();
            }

            return Ok(students);
        }

        public IHttpActionResult GetStudentByName(string studentName)
        {
            StudentViewModel student = null;
            using (var ctx = new MyTestDBEntities())
            {
                student = ctx.Students.Include("Standard").Where(s => s.FirstName == studentName).Select(s => new StudentViewModel()
                {
                    Id = s.StudentID,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Standard = new StandardViewModel()
                    {
                        Name = s.Standard.StandardName,
                        StandardId = s.Standard.StandardID
                    }
                }).FirstOrDefault<StudentViewModel>();
            }

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        public IHttpActionResult GetStudentById(int studentId)
        {
            StudentViewModel student = null;
            using (var ctx = new MyTestDBEntities())
            {
                student = ctx.Students.Include("Standard").Where(s => s.StudentID == studentId).Select(s => new StudentViewModel()
                {
                    Id = s.StudentID,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Standard = new StandardViewModel()
                    {
                        Name = s.Standard.StandardName,
                        StandardId = s.Standard.StandardID
                    }
                }).FirstOrDefault<StudentViewModel>();
            }

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        public IHttpActionResult GetStudentsByStandard(int StandardId)
        {
            List<StudentViewModel> students = null;

            using (var ctx = new MyTestDBEntities())
            {
                students = ctx.Students.Include("Standard").Where(std => std.StandardID == StandardId).
                    Select(s => new StudentViewModel()
                    {
                        Id = s.StudentID,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Standard = new StandardViewModel()
                        {
                            Name = s.Standard.StandardName,
                            StandardId = s.Standard.StandardID
                        }
                    }).ToList<StudentViewModel>();
            }

            if (students == null)
            {
                return NotFound();
            }

            return Ok(students);
        }
        /*POST, PUT METHODS BELOW*/

        /* http://localhost:58710/api/student - This will throw because API does not know which GET to be called, to handle it , 
         * make some changes 
        public IHttpActionResult GetAllStudentswithAddress()
        {
            IList<StudentViewModel> students = null;

            using (var ctx = new MyTestDBEntities())
            {
                students = ctx.Students.Include("StudentAddress").Select(s => new StudentViewModel()
                {
                    Id = s.StudentID,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Address = s.StudentAddresses == null ? null : (AddressViewModel)s.StudentAddresses.Where(a => a.StudentID == s.StudentID)
                    .Select(ad => new AddressViewModel()
                    {
                        Address1 = ad.Address1,
                        Address2 = ad.Address2,
                        City = ad.City,
                        State = ad.State
                    })
                }).ToList<StudentViewModel>();
            }

            if (students == null)
            {
                return NotFound();
            }

            return Ok(students);
        } */



        //public IHttpActionResult GetAllStudents(bool includeAddress = false) //To overcome the above scenario of multiple get methods calls
        //{
        //    IList<StudentViewModel> students = null;

        //    using (var ctx = new MyTestDBEntities())
        //    {
        //        students = ctx.Students.Include("StudentAddress").Select(s => new StudentViewModel()
        //        {
        //            Id = s.StudentID,
        //            FirstName = s.FirstName,
        //            LastName = s.LastName,
        //            Address = s.StudentAddresses == null || includeAddress == false ? null : new AddressViewModel()
        //            {
        //                StudentId = s.StudentAddresses.StudentID,
        //                Address1 = s.StudentAddresses.Address1,
        //                Address2 = s.StudentAddresses.Address2,
        //                City = s.StudentAddresses.City,
        //                State = s.StudentAddresses.State
        //            }
        //        }).ToList<StudentViewModel>();
        //    }

        //    if (students == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(students);
        //}

        public IHttpActionResult PostNewStudent(StandardViewModel standard)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new MyTestDBEntities())
                {
                    ctx.Standards.Add(new Standard()
                    {
                        StandardID = standard.StandardId,
                        StandardName = standard.Name,
                        Description = standard.Name
                        //StandardID = student.Standard.StandardId
                    });

                    ctx.SaveChanges();
                }
                return Created(Request.RequestUri + "/" + standard.StandardId.ToString(), standard);
            }
            else
                return BadRequest("Invalid data.");
        }

        public IHttpActionResult Put(int studentId, StudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new MyTestDBEntities())
                {
                    var existingStudent = ctx.Students.FirstOrDefault(s => s.StudentID == studentId);
                    if (existingStudent == null)
                    {
                        return NotFound();
                    }

                    existingStudent.FirstName = student.FirstName;
                    existingStudent.LastName = student.LastName;

                    ctx.SaveChanges();
                    return StatusCode(HttpStatusCode.OK);
                }
            }
            else
            {
                return BadRequest("Invalid Data.");
            }
        }

        public IHttpActionResult Delete(int studentId)
        {
            if (studentId > 0)
            {
                using (var ctx = new MyTestDBEntities())
                {
                    var existingStudent = ctx.Students.FirstOrDefault(s => s.StudentID == studentId);
                    if (existingStudent == null)
                    {
                        return NotFound();
                    }

                    ctx.Entry(existingStudent).State = System.Data.Entity.EntityState.Deleted;                    
                    ctx.SaveChanges();
                    return StatusCode(HttpStatusCode.OK);
                }
            }
            else
            {
                return BadRequest("Invalid Student ID.");
            }
        }
    }
}
