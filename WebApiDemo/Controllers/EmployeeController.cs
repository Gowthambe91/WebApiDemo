using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using WebApiDemo.Models.BusinessLogic;
using WebApiDemo.Models.DomainModel;
using WebApiDemo.Filters;
using System.Security.Claims;
using System.Web.Http.Cors;

namespace WebApiDemo.Controllers
{
    [BasicAuthentication]
    [EnableCors("*","*","*")]
    public class EmployeeController : ApiController
    {
        [MyAuthorize(Roles = "Admin,Super Admin")]
        [Route("api/AllEmployees")]
        public IHttpActionResult GetAllEmployees()
        {
            var identity = User.Identity;
            var username = identity.Name;
            return Ok(new EmployeeBL().GetEmployees());
        }

        [MyAuthorize(Roles = "Super Admin")]
        [Route("api/AllFemaleEmployees")]
        public IHttpActionResult GetAllFemaleEmployees()
        {
            //(ClaimsIdentity)
            var identity = User.Identity;
            var username = identity.Name;
            var employees = new EmployeeBL().GetEmployees().Where(e => e.Gender.ToLower() == "female").ToList();

            return Ok(employees);
        }

        [MyAuthorize(Roles = "Admin")]
        [Route("api/AllMaleEmployees")]
        public IHttpActionResult GetAllMaleEmployees()
        {
            //(ClaimsIdentity)
            var identity = User.Identity;
            var username = identity.Name;
            var employees = new EmployeeBL().GetEmployees().Where(e => e.Gender.ToLower() == "male").ToList();

            return Ok(employees);

        }
    }
}
