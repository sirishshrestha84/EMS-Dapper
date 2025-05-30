﻿using Dapper;
using EMS_Dapper.Filter;
using EMS_Dapper.Models;
using EMS_Dapper.Repository.IRepository;
using EMS_Dapper.Unit_Of_Work;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Dapper.Controllers
{
    //    public class DepartmentController : Controller
    //    {
    //        private readonly DapperApplicationDbContext _db;
    //        public DepartmentController(DapperApplicationDbContext db)
    //        {
    //            _db = db;
    //        }
    //        public async Task<IActionResult> Index()
    //        {

    //            //Using dapper
    //            //using (var connection = _db.CreateConnection())
    //            //{
    //            //    string query = "SELECT * FROM Departments";
    //            //    var departments = await connection.QueryAsync<Department>(query);
    //            //    return View(departments.ToList());
    //            //}

    //            //Using function
    //                using (var connection = _db.CreateConnection())
    //            {
    //                var departments = await connection.QueryAsync<Department>(
    //                    "SELECT * FROM dbo.GetAllDepartments()");

    //                return View(departments.ToList());
    //            }
    //        }
    //        //GET : //Department/Create
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]
    //        public IActionResult CreateDepartment()
    //        {
    //            return View();
    //        }

    //        //POST : /Department/Create
    //        [HttpPost]
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]
    //        public async Task<IActionResult> CreateDepartment(Department department)
    //        {
    //            //Using dapper
    //            //if (ModelState.IsValid)
    //            //{
    //            //    using (var connection = _db.CreateConnection())
    //            //    {
    //            //        string query = @"INSERT INTO Departments (Name)
    //            //                        VALUES (@Name)";
    //            //        await connection.ExecuteAsync(query, department);
    //            //        return RedirectToAction("Index");
    //            //    }
    //            //}
    //            //return View(department);

    //            //Using store procedure in dapper
    //            if (ModelState.IsValid)
    //            {
    //                using (var connection = _db.CreateConnection())
    //                {
    //                    await connection.ExecuteAsync("CreateDepartment", new
    //                    {
    //                        Name = department.Name
    //                    }, commandType: System.Data.CommandType.StoredProcedure);

    //                    return RedirectToAction("Index");
    //                }
    //            }
    //            return View(department);
    //        }

    //        //GET: /Department/Edit/{id}
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]
    //        public async Task<IActionResult> EditDepartment(int id)
    //        {
    //            using (var connection = _db.CreateConnection())
    //            {
    //                string query = "SELECT * FROM Departments WHERE DepartmentId = @Id";
    //                var department = await connection.QuerySingleOrDefaultAsync<Department>(query, new { Id = id });

    //                if (department == null)
    //                {
    //                    return NotFound();
    //                }

    //                return View(department);
    //            }
    //        }

    //        //POST : /Department/Edit
    //        [HttpPost]
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]

    //        public async Task<IActionResult> EditDepartment(Department department)
    //        {
    //            //Using dapper
    //            //if (ModelState.IsValid)
    //            //{
    //            //    using (var connection = _db.CreateConnection())
    //            //    {
    //            //        string query = @"UPDATE Departments 
    //            //                        SET Name = @Name
    //            //                        WHERE DepartmentId = @DepartmentId";
    //            //        await connection.ExecuteAsync(query, department);
    //            //        return RedirectToAction("Index");
    //            //    }
    //            //}
    //            //return View(department);

    //            //Using store procedure in dapper
    //            if(ModelState.IsValid)
    //            {
    //                using (var connection = _db.CreateConnection())
    //                {
    //                    await connection.ExecuteAsync("UpdateDepartment", new
    //                    {
    //                        DepartmentId = department.DepartmentId,
    //                        Name = department.Name
    //                    },commandType: System.Data.CommandType.StoredProcedure);
    //                    return RedirectToAction("Index");
    //                }
    //            }
    //            return View(department);
    //        }

    //        ////GET : /Department/Delete/{id}
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]
    //        public async Task<IActionResult> DeleteDepartment(int id)
    //        {
    //            using (var connection = _db.CreateConnection())
    //            {
    //                string query = "SELECT * FROM Departments WHERE Departmentid = @Id";
    //                var department = await connection.QuerySingleOrDefaultAsync<Department>(query, new { Id = id });

    //                if (department == null)
    //                {
    //                    return NotFound();
    //                }
    //                return View(department);
    //            }
    //        }

    //        ////Post: //Designation/Delete/{id}}
    //        [HttpPost, ActionName("Delete")]
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]
    //        public async Task<IActionResult> DeleteConfirmed(int id)
    //        {
    //            //Using dapper
    //            //using (var connection = _db.CreateConnection())
    //            //{
    //            //    string query = "DELETE FROM Departments WHERE Departmentid= @Id";
    //            //    await connection.ExecuteAsync(query, new { Id = id });
    //            //    return RedirectToAction("Index");
    //            //}

    //            //Using store procedure in dapper
    //            using (var connection = _db.CreateConnection())
    //            {
    //                await connection.ExecuteAsync("DeleteDepartment", new
    //                {
    //                    Departmentid = id
    //                }, commandType: System.Data.CommandType.StoredProcedure);
    //                return RedirectToAction("Index");
    //            }
    //        }
    //    }
    //}
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.Department.GetAllAsync();
            return View(departments.ToList());
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateDepartment() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Department.CreateAsync(department);
                return RedirectToAction("Index");
            }
            return View(department);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditDepartment(int id)
        {
            var department = await _unitOfWork.Department.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditDepartment(Department department)
        {
            if(ModelState.IsValid)
            {
                await _unitOfWork.Department.UpdateAsync(department);
                return RedirectToAction("Index");
            }
            return View(department);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _unitOfWork.Department.GetByIdAsync(id);
            if(department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Department.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartmentAjax(int id)
        {
            var department = await _unitOfWork.Department.GetByIdAsync(id);
            if(department == null)
            {
                return Json(new { sucess = false, message = "Department no found." });
            }
            await _unitOfWork.Department.DeleteAsync(id);
            return Json(new { success = true });
        }
    }
    }