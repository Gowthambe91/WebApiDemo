using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiDemo.Models.DomainModel;

namespace WebApiDemo.Models.BusinessLogic
{
    public class EmployeeBL
    {
        public List<Employee> GetEmployees()
        {
            List<Employee> employeesList = new List<Employee>();

            for (int i = 0; i < 10; i++)
            {
                if (i < 5)
                {
                    employeesList.Add(new Employee()
                    {
                        ID = i,
                        Dept = "Technology",
                        Gender = "Male",
                        Name = "Name" + i,
                        Salary = 1000 + i
                    });
                }
                else
                {
                    employeesList.Add(new Employee()
                    {
                        ID = i,
                        Dept = "Technology",
                        Gender = "Female",
                        Name = "Name" + i,
                        Salary = 1000 + i
                    });
                }
            }

            return employeesList;
        }
    }
}