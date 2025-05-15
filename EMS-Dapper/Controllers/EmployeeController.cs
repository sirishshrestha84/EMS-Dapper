//using Dapper;
//using EMS_Dapper.Filter;
//using EMS_Dapper.Models;
//using EMS_Dapper.Repository.IRepository;
//using EMS_Dapper.Unit_Of_Work;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http.Connections;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Identity.Client;
//using System.Collections.Generic;
//using System.Data;

//namespace EMS_Dapper.Controllers
//{
//    public class EmployeeController : Controller
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public EmployeeController(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
//        {
//            // Fetch employees with pagination
//            var employees = await _unitOfWork.Employee.GetPagedEmployeesAsync(page, pageSize);
//            var totalCount = await _unitOfWork.Employee.GetTotalEmployeeCountAsync();
//            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

//            var model = new Pager
//            {
//                Employees = employees.ToList(),
//                CurrentPage = page,
//                TotalPages = totalPages,
//                PageSize = pageSize
//            };

//            return View(model); // Pass the model to the view
//        }

//        // AJAX method to get employee data (returns JSON)
//        [HttpGet]
//        public async Task<IActionResult> GetEmployees(int page = 1, int pageSize = 5)
//        {
//            var employees = await _unitOfWork.Employee.GetPagedEmployeesAsync(page, pageSize);
//            var totalCount = await _unitOfWork.Employee.GetTotalEmployeeCountAsync();
//            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

//            var model = new
//            {
//                Employees = employees.Select(emp => new
//                {
//                    emp.Id,
//                    emp.EmployeeName,
//                    emp.Email,
//                    emp.DepartmentName,
//                    emp.DesignationName
//                }).ToList(),
//                Pagination = new
//                {
//                    currentPage = page,
//                    totalPages,
//                    pageSize
//                }
//            };

//            return Json(model); // Return employee data in JSON format
//        }
//        [Authorize(Roles = "Admin")]

//        public async Task<IActionResult> CreateEmployee()
//        {
//            ViewBag.DepartmentDropDownLists = (await _unitOfWork.Employee.GetDepartmentsAsync()).ToList();
//            ViewBag.DesignationDropDownLists = (await _unitOfWork.Employee.GetDesignationsAsync()).ToList();
//            return View();
//        }

//        [HttpPost]
//        [Authorize(Roles = "Admin")]

//        public async Task<IActionResult> CreateEmployee(Employee emp)
//        {
//            if (ModelState.IsValid)
//            {
//                await _unitOfWork.Employee.CreateEmployeeAsync(emp);
//                return RedirectToAction("Index");
//            }

//            ViewBag.DepartmentDropDownLists = (await _unitOfWork.Employee.GetDepartmentsAsync()).ToList();
//            ViewBag.DesignationDropDownLists = (await _unitOfWork.Employee.GetDesignationsAsync()).ToList();
//            return View(emp);
//        }

//        //Get : Employee/EditEmployee/{Id}
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> EditEmployee(int id)
//        {
//            var emp = await _unitOfWork.Employee.GetEmployeeByIdAsync(id);
//            if (emp == null)
//            {
//                return NotFound();
//            }
//            ViewBag.DepartmentDropdownLists = (await _unitOfWork.Employee.GetDepartmentsAsync()).ToList();
//            ViewBag.DesignationDropdownLists = (await _unitOfWork.Employee.GetDesignationsAsync()).ToList();

//            return View(emp);
//        }

//        //POST: /Employee/EditEmployee
//        [HttpPost]
//        //[ValidateAntiForgeryToken]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> EditEmployee(Employee emp)
//        {
//            if (ModelState.IsValid)
//            {
//                await _unitOfWork.Employee.UpdateEmployeeAsync(emp);
//                return RedirectToAction("Index");
//            }
//            ViewBag.DepartmentDropdownLists = (await _unitOfWork.Employee.GetDepartmentsAsync()).ToList();
//            ViewBag.DesignationDropdownLists = (await _unitOfWork.Employee.GetDesignationsAsync()).ToList();

//            return View(emp);

//        }

//        //GET : /Employee/DeleteEmployee/ {id}
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> DeleteEmployee(int id)
//        {
//            var emp = await _unitOfWork.Employee.GetEmployeeByIdAsync(id);
//            if (emp == null)
//            {
//                return NotFound();
//            }
//            return View(emp);
//        }

//        //POST: /Employee/Delete/{id}
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            await _unitOfWork.Employee.DeleteEmployeeAsync(id);
//            return Ok();
//        }

//    }

//}

////public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
////{
////    // Initial employee data is not loaded in view, just sending the page size to view
////    ViewBag.PageSize = pageSize;
////    return View();
////}

////// AJAX method to fetch employee data (returns JSON)
////[HttpGet]
////public async Task<IActionResult> GetEmployees(int page = 1, int pageSize = 5)
////{
////    var employees = await _unitOfWork.Employee.GetPagedEmployeesAsync(page, pageSize);
////    var totalCount = await _unitOfWork.Employee.GetTotalEmployeeCountAsync();
////    var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

////    var model = new
////    {
////        Employees = employees.Select(emp => new
////        {
////            emp.Id,
////            emp.EmployeeName,
////            emp.Email,
////            emp.DepartmentName,
////            emp.DesignationName
////        }).ToList(),
////        Pagination = new
////        {
////            currentPage = page,
////            totalPages,
////            pageSize
////        }
////    };

////    return Json(model);
////}


////public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
////{
////    var employees = await _unitOfWork.Employee.GetPagedEmployeesAsync(page, pageSize);
////    var totalCount = await _unitOfWork.Employee.GetTotalEmployeeCountAsync();
////    var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

////    var model = new Pager
////    {
////        Employees = employees.ToList(),
////        CurrentPage = page,
////        TotalPages = totalPages,
////        PageSize = pageSize
////    };
////    //return View(model);
////    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
////    {
////        return PartialView("_EmployeeTablePartial", model); // New partial view
////    }

////    return View(model);
////}



////    public class EmployeeController : Controller
////    {
////        private readonly DapperApplicationDbContext _db;
////        public EmployeeController(DapperApplicationDbContext db)
////        {
////            _db = db;
////        }

////        public IActionResult Index( int page =1, int pageSize =5)
////        {

////            using (var connection = _db.CreateConnection())
////            {
////                var employees = connection.Query<Employee>(
////           "SELECT * FROM GetPagedEmployees1(@PageNumber, @PageSize)",
////           new { PageNumber = page, PageSize = pageSize }).ToList();

////                var totalCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Employees");
////                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

////                var model = new Pager
////                {
////                    Employees = employees,
////                    CurrentPage = page,
////                    TotalPages = totalPages,
////                    PageSize = pageSize
////                };

////                return View(model);
////            }
////        }
////        //public IActionResult Index()
////        //{

////        //    using (var connection = _db.CreateConnection())
////        //    {
////        //        //Call the user-defined function to get all employees
////        //        string query = "SELECT * FROM dbo.Dp_GetAllEmployees()";

////        //        //Fetch the employee data using Dapper
////        //        IEnumerable<Employee> employeeList = connection.Query<Employee>(query);

////        //        //Return the data to the view
////        //        return View(employeeList);
////        //    }
////        //    //Using dapper
////        //    //using (var connection = _db.CreateConnection())
////        //    //{
////        //    //    string query = "Select a.Id,a.Name as EmployeeName, a.Email, a.DepartmentId,a.DesignationId, b.Name as DepartmentName, c.DesignationName from Employees as a inner join Departments as b on b.DepartmentId = a.DepartmentId \r\n   " +
////        //    //        "                 inner join Designations as c on c.DesignationId = a.DesignationId ";
////        //    //    connection.Open();
////        //    //    IEnumerable<Employee> emplists = connection.Query<Employee>(query);
////        //    //    return View(emplists);
////        //    //}

////        //    //Using function


////        //}

////        //GET : CREATE
////        //[CustomAuthorize("Admin")] //Custom Session based authorization
////        //[Authorize(Roles = "Admin")] //Cookies based authorization
////        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
////        //[JwtAuthorize("Admin")]

////        [Authorize(Roles="Admin")]
////        public async Task<IActionResult> CreateEmployee()
////        {
////            //Dapper default code
////            //using (var connection = _db.CreateConnection())
////            //{
////            //    string departmentQuery = "SELECT * FROM Departments";
////            //    string designationQuery = "SELECT * FROM Designations";

////            //    IEnumerable<Department> departments = await connection.QueryAsync<Department>(departmentQuery);
////            //    IEnumerable<Designation> designations = await connection.QueryAsync<Designation>(designationQuery);

////            //    ViewBag.DepartmentDropdownLists = departments;
////            //    ViewBag.DesignationDropdownLists = designations;

////            //    return View();
////            //}


////            //Using store procedure in dapper
////            using (var connection = _db.CreateConnection())
////            {
////                var departments = await connection.QueryAsync<Department>("SELECT * FROM Departments");
////                var designations = await connection.QueryAsync<Designation>("SELECT * FROM Designations");

////                ViewBag.DepartmentDropdownLists = departments.ToList();
////                ViewBag.DesignationDropdownLists = designations.ToList();

////                return View();
////            }

////            }

////        //POST : Create
////        [HttpPost]
////        //[CustomAuthorize("Admin")] //Custom Session based authorization
////        //[Authorize(Roles = "Admin")] //Cookies based authorization
////        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
////        [Authorize(Roles = "Admin")]

////        public async Task<IActionResult> CreateEmployee(Employee emp)
////        {
////            //Code for default dapper code format
////            //using (var connection = _db.CreateConnection())
////            //{
////            //    string query = @"INSERT INTO Employees(Name, Email, DepartmentId, DesignationId)
////            //                 VALUES(@EmployeeName, @Email, @DepartmentId, @DesignationId)";
////            //    connection.Execute(query, emp);
////            //    return RedirectToAction("Index");
////            //}

////            //Code using store procedure in dapper
////            if(ModelState.IsValid)
////            {
////                using(var connection = _db.CreateConnection())
////                {
////                    await connection.ExecuteAsync("CreateEmployee", new
////                    {
////                        Name = emp.EmployeeName,
////                        Email = emp.Email,
////                        DepartmentId = emp.DepartmentId,
////                        DesignationId = emp.DesignationId
////                    }, commandType: CommandType.StoredProcedure);
////                    return RedirectToAction("Index");
////                }
////            }
////            using (var connection = _db.CreateConnection())
////            {
////                ViewBag.DepartmentDropdownLists = (await connection.QueryAsync<Department>("SELECT * FROM Departments")).ToList();
////                ViewBag.DesignationDropdownLists = (await connection.QueryAsync<Designation>("SELECT * FROM Designations")).ToList();
////            }
////            return View(emp);
////        }

////        //Get :/Employee/Edit/{id}
////        //[CustomAuthorize("Admin")] //Custom Session based authorization
////        //[Authorize(Roles = "Admin")] //Cookies based authorization
////        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
////        [Authorize(Roles = "Admin")]
////        public async Task<IActionResult> EditEmployee(int id)
////        {
////            //Code for dapper default format
////            using (var connection = _db.CreateConnection())
////            {
////                var emp = await connection.QuerySingleOrDefaultAsync<Employee>(
////                    "SELECT Name as EmployeeName, Id, Email, DepartmentId, DesignationId FROM Employees WHERE id = @Id", new { Id = id });
////                if (emp == null)
////                {
////                    return NotFound();

////                }
////                ViewBag.DepartmentDropdownLists = (await connection.QueryAsync<Department>("SELECT * FROM Departments")).ToList();
////                ViewBag.DesignationDropdownLists = (await connection.QueryAsync<Designation>("SELECT * FROM Designations")).ToList();

////                return View(emp);
////            }


////        }


////        // POST: /Employee/Edit
////        [HttpPost]
////        //[CustomAuthorize("Admin")] //Custom Session based authorization
////        //[Authorize(Roles = "Admin")] //Cookies based authorization
////        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
////        [Authorize(Roles = "Admin")]
////        public async Task<IActionResult> EditEmployee(Employee emp)
////        {
////            //Using dapper 
////            //if (ModelState.IsValid)
////            //{
////            //    using (var connection = _db.CreateConnection())
////            //    {
////            //        string query = @"UPDATE Employees 
////            //                         SET Name = @EmployeeName, Email = @Email, DepartmentId = @DepartmentId, DesignationId = @DesignationId 
////            //                         WHERE Id = @Id";

////            //        await connection.ExecuteAsync(query, emp);
////            //        return RedirectToAction("Index");
////            //    }
////            //}

////            //using (var connection = _db.CreateConnection())
////            //{
////            //    ViewBag.DepartmentDropdownLists = (await connection.QueryAsync<Department>("SELECT * FROM Departments")).ToList();
////            //    ViewBag.DesignationDropdownLists = (await connection.QueryAsync<Designation>("SELECT * FROM Designations")).ToList();
////            //    return View(emp);
////            //}

////            //Using store procedure in dapper
////            if (ModelState.IsValid)
////            {
////                using (var connection = _db.CreateConnection())
////                {
////                    await connection.ExecuteAsync("UpdateEmployee", new
////                    {
////                        Id = emp.Id,
////                        Name = emp.EmployeeName,
////                        Email = emp.Email,
////                        DepartmentId = emp.DepartmentId,
////                        DesignationId = emp.DesignationId,
////                    }, commandType: CommandType.StoredProcedure);

////                    return RedirectToAction("Index");
////                }
////            }
////            using (var connection = _db.CreateConnection())
////            {
////                ViewBag.DepartmentDropdownLists = (await connection.QueryAsync<Department>("SELECT * FROM Departments")).ToList();
////                ViewBag.DesignationDropdownLists = (await connection.QueryAsync<Designation>("SELECT * FROM Designations")).ToList();
////            }
////            return View(emp);
////        }

////        // Get : /Employee/Delete/{id}
////        //[CustomAuthorize("Admin")] //Custom Session based authorization
////        //[Authorize(Roles = "Admin")] //Cookies based authorization
////        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
////        [Authorize(Roles = "Admin")]
////        public async Task<IActionResult> DeleteEmployee(int id)
////        {
////            using (var connection = _db.CreateConnection())
////            {
////                var emp = await connection.QuerySingleOrDefaultAsync<Employee>(
////                    "SELECT a.Name as EmployeeName, Id, Email, a.DepartmentId,b.Name as DepartmentName, a.DesignationId,c.DesignationName FROM Employees as a inner join Departments as b on a.DepartmentId = b.DepartmentId inner join Designations as c on a.DesignationId = c.DesignationId  WHERE id = @Id", new { Id = id });
////                if (emp== null)
////                {
////                    return NotFound();
////                }
////                return View(emp);
////            }
////        }

////        //POST : /Employee/Delete/{id}
////        [HttpPost, ActionName("Delete")]
////        //[CustomAuthorize("Admin")] //Custom Session based authorization
////        //[Authorize(Roles = "Admin")] //Cookies based authorization
////        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
////        [Authorize(Roles = "Admin")]
////        public async Task<IActionResult> DeleteConfirmed(int id)
////        {
////            // Using dapper
////            //using (var connection = _db.CreateConnection())
////            //{
////            //    await connection.ExecuteAsync("DELETE FROM Employees WHERE Id = @Id", new { Id = id });
////            //    return RedirectToAction("Index");
////            //}


////            //Using Store procedure
////            using (var connection = _db.CreateConnection())
////            {
////                await connection.ExecuteAsync("DeleteEmployee", new { Id = id }, commandType: CommandType.StoredProcedure);
////                return RedirectToAction("Index");
////            }
////        }
////    }
////}
///

using ClosedXML.Excel;
using Dapper;
using EMS_Dapper.Filter;
using EMS_Dapper.Models;
using EMS_Dapper.Repository.IRepository;
using EMS_Dapper.Unit_Of_Work;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Data;

namespace EMS_Dapper.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            var employees = await _unitOfWork.Employee.GetPagedEmployeesAsync(page, pageSize);
            var totalCount = await _unitOfWork.Employee.GetTotalEmployeeCountAsync();
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

        //For pagination ajax
        [HttpGet]
        public async Task<IActionResult> GetPageData(int page, int pageSize)
        {
            var employees = await _unitOfWork.Employee.GetPagedEmployeesAsync(page, pageSize);

            return PartialView("_EmployeeRowsPartial", employees);
        }

        //For ajax
        [HttpGet]
        public async Task<IActionResult> GetPagedEmployees(int page = 1, int pageSize = 5)
        {
            var employees = await _unitOfWork.Employee.GetPagedEmployeesAsync(page, pageSize);
            var totalCount = await _unitOfWork.Employee.GetTotalEmployeeCountAsync();
            var totalPages = (int)Math.Ceiling(((double)totalCount / pageSize) * pageSize);

            return Json(new
            {
                employees,
                totalPages,
                currentPage = page
            });
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CreateEmployee()
        {
            ViewBag.DepartmentDropDownLists = (await _unitOfWork.Employee.GetDepartmentsAsync()).ToList();
            ViewBag.DesignationDropDownLists = (await _unitOfWork.Employee.GetDesignationsAsync()).ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CreateEmployee(Employee emp)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Employee.CreateEmployeeAsync(emp);
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentDropDownLists = (await _unitOfWork.Employee.GetDepartmentsAsync()).ToList();
            ViewBag.DesignationDropDownLists = (await _unitOfWork.Employee.GetDesignationsAsync()).ToList();
            return View(emp);
        }

        //Get : Employee/EditEmployee/{Id}
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditEmployee(int id)
        {
            var emp = await _unitOfWork.Employee.GetEmployeeByIdAsync(id);
            if (emp == null)
            {
                return NotFound();
            }
            ViewBag.DepartmentDropdownLists = (await _unitOfWork.Employee.GetDepartmentsAsync()).ToList();
            ViewBag.DesignationDropdownLists = (await _unitOfWork.Employee.GetDesignationsAsync()).ToList();

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
                await _unitOfWork.Employee.UpdateEmployeeAsync(emp);
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentDropdownLists = (await _unitOfWork.Employee.GetDepartmentsAsync()).ToList();
            ViewBag.DesignationDropdownLists = (await _unitOfWork.Employee.GetDesignationsAsync()).ToList();

            return View(emp);

        }

        //GET : /Employee/DeleteEmployee/ {id}
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var emp = await _unitOfWork.Employee.GetEmployeeByIdAsync(id);
            if (emp == null)
            {
                return NotFound();
            }
            return View(emp);
        }

        //for ajax action
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployeeAjax(int id)
        {
            var emp = await _unitOfWork.Employee.GetEmployeeByIdAsync(id);
            if (emp == null)
                return Json(new { success = false, message = "Employee not found." });

            await _unitOfWork.Employee.DeleteEmployeeAsync(id);
            return Json(new { success = true });
        }

        //POST: /Employee/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Employee.DeleteEmployeeAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetDepartmentChartData()
        {
            var data = await _unitOfWork.Employee.GetEmployeeCountByDepartmentAsync();
            return Json(data.Select(d => new
            {
                departmentName = d.DepartmentName,
                employeeCount = d.EmployeeCount
            }));

        }

        public async Task<IActionResult> GetDesignationChartData()
        {
            // Fetch data grouped by designation
            var data = await _unitOfWork.Employee.GetEmployeeCountByDesignationAsync();

            // Return the data in the required format for the chart
            return Json(data.Select(d => new
            {
                designationName = d.DesignationName,  // Make sure your model has this property
                employeeCount = d.EmployeeCount
            }));
        }

        public async Task<IActionResult> DownloadEmployeeTable()
        {
            var employees = await _unitOfWork.Employee.GetAllEmployeesAsync();

            return new Rotativa.AspNetCore.ViewAsPdf("EmployeePdf", employees.ToList())
            {
                FileName = "Employee_List.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        [HttpGet]
        public async Task<IActionResult> ExportTOExcel()
        {
            var employees = await _unitOfWork.Employee.GetAllEmployeesAsync();

            // Create a new Excel Workbook
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Employees");

            // Add header
            worksheet.Cell(1, 1).Value = "Name";
            worksheet.Cell(1, 2).Value = "Email";
            worksheet.Cell(1, 3).Value = "Department";
            worksheet.Cell(1, 4).Value = "Designation";

            // Add employee data to worksheet
            int row = 2;
            foreach (var emp in employees)
            {
                worksheet.Cell(row, 1).Value = emp.EmployeeName;
                worksheet.Cell(row, 2).Value = emp.Email;
                worksheet.Cell(row, 3).Value = emp.DepartmentName;
                worksheet.Cell(row, 4).Value = emp.DesignationName;
                row++;
            }

            // Auto-adjust column width
            worksheet.Columns().AdjustToContents();

            // Save to memory stream
            var stream = new MemoryStream();
            workbook.SaveAs(stream);

            // Ensure the position of the stream is set back to 0 before returning
            stream.Position = 0;

            // Return the Excel file
            var filename = $"Employees_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }

    }
}


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






