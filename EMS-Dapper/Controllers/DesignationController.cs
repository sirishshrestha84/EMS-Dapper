using Dapper;
using EMS_Dapper.Filter;
using EMS_Dapper.Models;
using EMS_Dapper.Repository.IRepository;
using EMS_Dapper.Unit_Of_Work;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EMS_Dapper.Controllers
{
    //    public class DesignationController : Controller
    //    {
    //            private readonly DapperApplicationDbContext _db;
    //            public DesignationController(DapperApplicationDbContext db)
    //            {
    //                _db = db;
    //            }

    //        public async Task<IActionResult> Index() 
    //        {
    //            //Using dapper
    //            //using (var connection = _db.CreateConnection())
    //            //{
    //            //    string query = "SELECT * FROM Designations";
    //            //    var designations = await connection.QueryAsync<Designation>(query);
    //            //    return View(designations.ToList());
    //            //}

    //            //Using function
    //            using (var connection = _db.CreateConnection()) 
    //            {
    //                //Use the user_defined function to fetch all the designation
    //                var designations = await connection.QueryAsync<Designation>(
    //                    "SELECT * FROM dbo.GetAllDesignations()");
    //                return View(designations.ToList());
    //            }    
    //        }
    //        //GET : //Designation/Create
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]
    //        public IActionResult CreateDesignation()
    //        {
    //            return View();
    //        }

    //        //POST : /Designation/Create
    //        [HttpPost]
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        [CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        public async Task<IActionResult> CreateDesignation(Designation designation)
    //        {
    //            if(ModelState.IsValid)
    //            {
    //                using (var connection = _db.CreateConnection())
    //                {
    //                    string query = @"INSERT INTO Designations (DesignationName)
    //                                    VALUES (@DesignationName)";
    //                    await connection.ExecuteAsync(query, designation);
    //                    return RedirectToAction("Index");
    //                }
    //            }
    //            return View(designation);
    //        }

    //        //GET: /Designation/Edit/{id}
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]
    //        public async Task<IActionResult> EditDesignation(int id)
    //        {
    //            using (var connection = _db.CreateConnection())
    //            {
    //                string query = "SELECT * FROM Designations WHERE DesignationId = @Id";
    //                var designation = await connection.QuerySingleOrDefaultAsync<Designation>(query, new { Id = id });

    //                if (designation == null)
    //                {
    //                    return NotFound();
    //                }

    //                return View(designation);
    //            }
    //        }

    //        //POST : /Designation/Edit
    //        [HttpPost]
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]
    //        public async Task<IActionResult> EditDesignation(Designation designation)
    //        {

    //            //Using dapper
    //            //if (ModelState.IsValid)
    //            //{
    //            //    using (var connection = _db.CreateConnection())
    //            //    {
    //            //        string query = @"UPDATE Designations 
    //            //                        SET DesignationName = @DesignationName
    //            //                        WHERE DesignationId = @DesignationId";
    //            //        await connection.ExecuteAsync(query, designation);
    //            //        return RedirectToAction("Index");
    //            //    }
    //            //}
    //            //return View(designation);

    //            //Using store procedure in dapper
    //            if(ModelState.IsValid)
    //            {
    //                using (var connection = _db.CreateConnection())
    //                {
    //                    await connection.ExecuteAsync("UpdateDesignation", new
    //                    {
    //                        DesignationId = designation.DesignationId,
    //                        DesignationName = designation.DesignationName
    //                    }, commandType: System.Data.CommandType.StoredProcedure);
    //                    return RedirectToAction("Index");
    //                }
    //            }
    //            return View(designation);
    //        }

    //        //GET : /Designation/Delete/{id}
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]
    //        public async Task<IActionResult> DeleteDesignation(int id)
    //        {
    //            using (var connection = _db.CreateConnection())
    //            {
    //                string query = "SELECT * FROM Designations WHERE DesignationId = @Id";
    //                var designation = await connection.QuerySingleOrDefaultAsync<Designation>(query, new {Id = id });

    //                if (designation == null)
    //                {
    //                    return NotFound();
    //                }
    //                return View(designation);
    //            }
    //        }

    //        //Post: //Designation/Delete/{id}}
    //        [HttpPost,ActionName("Delete")]
    //        //[CustomAuthorize("Admin")] //Custom Session based authorization
    //        //[Authorize(Roles = "Admin")] //Cookies based authorization
    //        //[CookiesCustomAuthorize("Admin")] //Custom cookies based authorization
    //        [Authorize(Roles = "Admin")]
    //        public async Task<IActionResult> DeleteConfirmed(int id)
    //        {
    //            //Using dapper
    //            //using (var connection = _db.CreateConnection())
    //            //{
    //            //    string query = "DELETE FROM Designations WHERE DesignationId= @Id";
    //            //    await connection.ExecuteAsync(query, new { Id = id });
    //            //    return RedirectToAction("Index");
    //            //}

    //            //Using store procedure in dapper
    //            using (var connection = _db.CreateConnection())
    //            {
    //                //call the store procedure to delete the designation
    //                await connection.ExecuteAsync("DeleteDesignation", new
    //                {
    //                    DesignationId = id
    //                }, commandType: System.Data.CommandType.StoredProcedure);

    //                //Redirect bacl to the Index view after deletion
    //                return RedirectToAction("Index");
    //            }
    //        }


    //    }
    //}
    public class DesignationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DesignationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var designations = await _unitOfWork.Designation.GetAllAsync();
            return View(designations.ToList());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateDesignation() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDesignation(Designation designation)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Designation.CreateAsync(designation);
                return RedirectToAction("Index");
            }
            return View(designation);
        }       

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditDesignation(int id)
        {
            var designation = await _unitOfWork.Designation.GetByIdAsync(id);
            if (designation == null) return NotFound();
            return View(designation);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditDesignation(Designation designation)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Designation.UpdateAsync(designation);
                return RedirectToAction("Index");
            }
            return View(designation);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            var designation = await _unitOfWork.Designation.GetByIdAsync(id);
            if (designation == null) return NotFound();
            return View(designation);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Designation.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
