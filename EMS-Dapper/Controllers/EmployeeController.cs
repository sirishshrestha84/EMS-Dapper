using Dapper;
using EMS_Dapper.Filter;
using EMS_Dapper.Models;
using EMS_Dapper.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Data;

namespace EMS_Dapper.Controllers
{
    //    public class EmployeeController : Controller
    //    {
    //        private readonly DapperApplicationDbContext _db;
    //        public EmployeeController(DapperApplicationDbContext db)
    //        {
    //            _db = db;
    //        }

    //        public IActionResult Index( int page =1, int pageSize =5)
    //        {

    //            using (var connection = _db.CreateConnection())
    //            {
    //                var employees = connection.Query<Employee>(
    //           "SELECT * FROM GetPagedEmployees1(@PageNumber, @PageSize)",
    //           new { PageNumber = page, PageSize = pageSize }).ToList();

    //                var totalCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Employees");
    //                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

    //                var model = new Pager
    //                {
    //                    Employees = employees,
    //                    CurrentPage = page,
    //                    TotalPages = totalPages,
    //                    PageSize = pageSize
    //                };

    //                return View(model);
    //            }
    //        }
    //        //public IActionResult Index()
    //        //{

    //        //    using (var connection = _db.CreateConnection())
    //        //    {
    //        //        //Call the user-defined function to get all employees
    //        //        string query = "SELECT * FROM dbo.Dp_GetAllEmployees()";

    //        //        //Fetch the employee data using Dapper
    //        //        IEnumerable<Employee> employeeList = connection.Query<Employee>(query);

    //        //        //Return the data to the view
    //        //        return View(employeeList);
    //        //    }
    //        //    //Using dapper
    //        //    //using (var connection = _db.CreateConnection())
    //        //    //{
    //        //    //    string query = "Select a.Id,a.Name as EmployeeName, a.Email, a.DepartmentId,a.DesignationId, b.Name as DepartmentName, c.DesignationName from Employees as a inner join Departments as b on b.DepartmentId = a.DepartmentId \r\n   " +
    //        //    //        "                 inner join Designations as c on c.DesignationId = a.DesignationId ";
    //        //    //    connection.Open();
    //        //    //    IEnumerable<Employee> emplists = connection.Query<Employee>(query);
    //        //    //    return View(emplists);
    //        //    //}

    //        //    //Using function


    //        //}

    //        //GET : CREATE
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        //[JwtAuthorize("Admin")]

    //        [Authorize(Roles="Admin")]
    //        public async Task<IActionResult> CreateEmployee()
    //        {
    //            //Dapper default code
    //            //using (var connection = _db.CreateConnection())
    //            //{
    //            //    string departmentQuery = "SELECT * FROM Departments";
    //            //    string designationQuery = "SELECT * FROM Designations";

    //            //    IEnumerable<Department> departments = await connection.QueryAsync<Department>(departmentQuery);
    //            //    IEnumerable<Designation> designations = await connection.QueryAsync<Designation>(designationQuery);

    //            //    ViewBag.DepartmentDropdownLists = departments;
    //            //    ViewBag.DesignationDropdownLists = designations;

    //            //    return View();
    //            //}


    //            //Using store procedure in dapper
    //            using (var connection = _db.CreateConnection())
    //            {
    //                var departments = await connection.QueryAsync<Department>("SELECT * FROM Departments");
    //                var designations = await connection.QueryAsync<Designation>("SELECT * FROM Designations");

    //                ViewBag.DepartmentDropdownLists = departments.ToList();
    //                ViewBag.DesignationDropdownLists = designations.ToList();

    //                return View();
    //            }

    //            }

    //        //POST : Create
    //        [HttpPost]
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]

    //        public async Task<IActionResult> CreateEmployee(Employee emp)
    //        {
    //            //Code for default dapper code format
    //            //using (var connection = _db.CreateConnection())
    //            //{
    //            //    string query = @"INSERT INTO Employees(Name, Email, DepartmentId, DesignationId)
    //            //                 VALUES(@EmployeeName, @Email, @DepartmentId, @DesignationId)";
    //            //    connection.Execute(query, emp);
    //            //    return RedirectToAction("Index");
    //            //}

    //            //Code using store procedure in dapper
    //            if(ModelState.IsValid)
    //            {
    //                using(var connection = _db.CreateConnection())
    //                {
    //                    await connection.ExecuteAsync("CreateEmployee", new
    //                    {
    //                        Name = emp.EmployeeName,
    //                        Email = emp.Email,
    //                        DepartmentId = emp.DepartmentId,
    //                        DesignationId = emp.DesignationId
    //                    }, commandType: CommandType.StoredProcedure);
    //                    return RedirectToAction("Index");
    //                }
    //            }
    //            using (var connection = _db.CreateConnection())
    //            {
    //                ViewBag.DepartmentDropdownLists = (await connection.QueryAsync<Department>("SELECT * FROM Departments")).ToList();
    //                ViewBag.DesignationDropdownLists = (await connection.QueryAsync<Designation>("SELECT * FROM Designations")).ToList();
    //            }
    //            return View(emp);
    //        }

    //        //Get :/Employee/Edit/{id}
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]
    //        public async Task<IActionResult> EditEmployee(int id)
    //        {
    //            //Code for dapper default format
    //            using (var connection = _db.CreateConnection())
    //            {
    //                var emp = await connection.QuerySingleOrDefaultAsync<Employee>(
    //                    "SELECT Name as EmployeeName, Id, Email, DepartmentId, DesignationId FROM Employees WHERE id = @Id", new { Id = id });
    //                if (emp == null)
    //                {
    //                    return NotFound();

    //                }
    //                ViewBag.DepartmentDropdownLists = (await connection.QueryAsync<Department>("SELECT * FROM Departments")).ToList();
    //                ViewBag.DesignationDropdownLists = (await connection.QueryAsync<Designation>("SELECT * FROM Designations")).ToList();

    //                return View(emp);
    //            }


    //        }


    //        // POST: /Employee/Edit
    //        [HttpPost]
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]
    //        public async Task<IActionResult> EditEmployee(Employee emp)
    //        {
    //            //Using dapper 
    //            //if (ModelState.IsValid)
    //            //{
    //            //    using (var connection = _db.CreateConnection())
    //            //    {
    //            //        string query = @"UPDATE Employees 
    //            //                         SET Name = @EmployeeName, Email = @Email, DepartmentId = @DepartmentId, DesignationId = @DesignationId 
    //            //                         WHERE Id = @Id";

    //            //        await connection.ExecuteAsync(query, emp);
    //            //        return RedirectToAction("Index");
    //            //    }
    //            //}

    //            //using (var connection = _db.CreateConnection())
    //            //{
    //            //    ViewBag.DepartmentDropdownLists = (await connection.QueryAsync<Department>("SELECT * FROM Departments")).ToList();
    //            //    ViewBag.DesignationDropdownLists = (await connection.QueryAsync<Designation>("SELECT * FROM Designations")).ToList();
    //            //    return View(emp);
    //            //}

    //            //Using store procedure in dapper
    //            if (ModelState.IsValid)
    //            {
    //                using (var connection = _db.CreateConnection())
    //                {
    //                    await connection.ExecuteAsync("UpdateEmployee", new
    //                    {
    //                        Id = emp.Id,
    //                        Name = emp.EmployeeName,
    //                        Email = emp.Email,
    //                        DepartmentId = emp.DepartmentId,
    //                        DesignationId = emp.DesignationId,
    //                    }, commandType: CommandType.StoredProcedure);

    //                    return RedirectToAction("Index");
    //                }
    //            }
    //            using (var connection = _db.CreateConnection())
    //            {
    //                ViewBag.DepartmentDropdownLists = (await connection.QueryAsync<Department>("SELECT * FROM Departments")).ToList();
    //                ViewBag.DesignationDropdownLists = (await connection.QueryAsync<Designation>("SELECT * FROM Designations")).ToList();
    //            }
    //            return View(emp);
    //        }

    //        // Get : /Employee/Delete/{id}
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]
    //        public async Task<IActionResult> DeleteEmployee(int id)
    //        {
    //            using (var connection = _db.CreateConnection())
    //            {
    //                var emp = await connection.QuerySingleOrDefaultAsync<Employee>(
    //                    "SELECT a.Name as EmployeeName, Id, Email, a.DepartmentId,b.Name as DepartmentName, a.DesignationId,c.DesignationName FROM Employees as a inner join Departments as b on a.DepartmentId = b.DepartmentId inner join Designations as c on a.DesignationId = c.DesignationId  WHERE id = @Id", new { Id = id });
    //                if (emp== null)
    //                {
    //                    return NotFound();
    //                }
    //                return View(emp);
    //            }
    //        }

    //        //POST : /Employee/Delete/{id}
    //        [HttpPost, ActionName("Delete")]
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]
    //        public async Task<IActionResult> DeleteConfirmed(int id)
    //        {
    //            // Using dapper
    //            //using (var connection = _db.CreateConnection())
    //            //{
    //            //    await connection.ExecuteAsync("DELETE FROM Employees WHERE Id = @Id", new { Id = id });
    //            //    return RedirectToAction("Index");
    //            //}


    //            //Using Store procedure
    //            using (var connection = _db.CreateConnection())
    //            {
    //                await connection.ExecuteAsync("DeleteEmployee", new { Id = id }, commandType: CommandType.StoredProcedure);
    //                return RedirectToAction("Index");
    //            }
    //        }
    //    }
    //}
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            var employees = await _repository.GetPagedEmployeesAsync(page, pageSize);
            var totalCount = await _repository.GetTotalEmployeeCountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var model = new Pager
            {
                Employees = employees.ToList(),
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };
            return View(model);
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CreateEmployee()
        {
            ViewBag.DepartmentDropdownLists = (await _repository.GetDepartmentsAsync()).ToList();
            ViewBag.DesignationDropdownLists = (await _repository.GetDesignationsAsync()).ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CreateEmployee(Employee emp)
        {
            if (ModelState.IsValid)
            {
                await _repository.CreateEmployeeAsync(emp);
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentDropDownLists = (await _repository.GetDepartmentsAsync()).ToList();
            ViewBag.DesignationDropDownLists = (await _repository.GetDesignationsAsync()).ToList();
            return View(emp);
        }

        //Get : Employee/EditEmployee/{Id}
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditEmployee(int id)
        {
            var emp = await _repository.GetEmployeeByIdAsync(id);
            if (emp == null)
            { 
                return NotFound();
            }
            ViewBag.DepartmentDropdownLists = (await _repository.GetDepartmentsAsync()).ToList();
            ViewBag.DesignationDropdownLists = (await _repository.GetDesignationsAsync()).ToList();

            return View(emp);
        }

        //POST: /Employee/EditEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditEmployee(Employee emp)
        {
            if (ModelState.IsValid)
            { 
                await _repository.UpdateEmployeeAsync(emp);
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentDropdownLists = (await _repository.GetDepartmentsAsync()).ToList();
            ViewBag.DesignationDropdownLists = (await _repository.GetDesignationsAsync()).ToList();

            return View(emp);

        }

        //GET : /Employee/DeleteEmployee/ {id}
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var emp = await _repository.GetEmployeeByIdAsync(id);
            if(emp == null)
            {
                return NotFound();
            }
            return View(emp);
        }

        //POST: /Employee/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteEmployeeAsync(id);
            return RedirectToAction("Index");
        }

    }

}



