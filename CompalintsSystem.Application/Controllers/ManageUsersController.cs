using AutoMapper;
using CompalintsSystem.Core.Interfaces;
using CompalintsSystem.Core.Models;
using CompalintsSystem.Core.ViewModels;
using CompalintsSystem.Core.ViewModels.Data;
using CompalintsSystem.EF.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CompalintsSystem.Application.Controllers
{
    [Authorize(Roles = "AdminGeneralFederation,AdminColleges,AdminDepartments,AdminSubDepartments")]
    public class ManageUsersController : Controller
    {
        //protected abstract void Validate();
        private readonly List<Colleges> _Collegess = new List<Colleges>();
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICompalintRepository _compalintRepository;
        private readonly AppCompalintsContextDB _context;
        private readonly IMapper mapper;

        public ManageUsersController(
            IUserService userService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ICompalintRepository compalintService,
            AppCompalintsContextDB context,
            IMapper mapper)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _compalintRepository = compalintService;
            _context = context;
            this.mapper = mapper;
            _compalintRepository = compalintService;
        }


        private string UserId
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }

        // GET: Users/Create
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentName = currentUser.FullName;
            var model = new AddUserViewModel()
            {

                CollegessList = await _context.Collegess.ToListAsync()
            };
            ViewBag.ViewGover = model.CollegessList.ToArray();
            return View(model);
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            model.CollegessList = await _context.Collegess.ToListAsync();
            var currentUser = await _userManager.GetUserAsync(User);
            var currentName = currentUser.FullName;
            var currentId = currentUser.IdentityNumber;

            if (ModelState.IsValid)
            {

                if (_userService.returntype == 1)
                {
                    TempData["Error"] = _userService.Error;
                    return View(model);
                }
                else if (_userService.returntype == 2)
                {
                    TempData["Error"] = _userService.Error;
                    return View(model);
                }

                await _userService.AddUserAsync(model, currentName, currentId);

                return RedirectToAction("ViewUsers", "GeneralFederation");


            }
            return View(model);
        }


        public async Task<IActionResult> CreateUserDep()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentName = currentUser.FullName;
            var model = new AddUserViewModel()
            {

                CollegessList = await _context.Collegess.ToListAsync()
            };
            ViewBag.ViewGover = model.CollegessList.ToArray();
            return View(model);
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserDep(AddUserViewModel model)
        {
            model.CollegessList = await _context.Collegess.ToListAsync();
            var currentUser = await _userManager.GetUserAsync(User);
            var currentName = currentUser.FullName;
            var currentId = currentUser.IdentityNumber;

            if (ModelState.IsValid)
            {

                if (_userService.returntype == 1)
                {
                    TempData["Error"] = _userService.Error;
                    return View(model);
                }
                else if (_userService.returntype == 2)
                {
                    TempData["Error"] = _userService.Error;
                    return View(model);
                }

                await _userService.AddUserDepAsync(model, currentName, currentId);

                return RedirectToAction("ViewUsers", "GeneralFederation");


            }
            return View(model);
        }



        public async Task<IActionResult> CreateUserComp()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentName = currentUser.FullName;
            var model = new AddUserViewModel()
            {

                CollegessList = await _context.Collegess.ToListAsync()
            };
            ViewBag.ViewGover = model.CollegessList.ToArray();
            return View(model);
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserComp(AddUserViewModel model)
        {
            model.CollegessList = await _context.Collegess.ToListAsync();
            var currentUser = await _userManager.GetUserAsync(User);
            var currentName = currentUser.FullName;
            var currentId = currentUser.IdentityNumber;

            if (ModelState.IsValid)
            {

                if (_userService.returntype == 1)
                {
                    TempData["Error"] = _userService.Error;
                    return View(model);
                }
                else if (_userService.returntype == 2)
                {
                    TempData["Error"] = _userService.Error;
                    return View(model);
                }

                await _userService.AddUserCompAsync(model, currentName, currentId);

                return RedirectToAction("ViewUsers", "GeneralFederation");


            }
            return View(model);
        }



        
        public async Task<IActionResult> CreateColageComp()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentName = currentUser.FullName;
            var model = new AddUserViewModel()
            {

                CollegessList = await _context.Collegess.ToListAsync()
            };
            ViewBag.ViewGover = model.CollegessList.ToArray();
            return View(model);
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateColageComp(AddUserViewModel model)
        {
            model.CollegessList = await _context.Collegess.ToListAsync();
            var currentUser = await _userManager.GetUserAsync(User);
            var currentName = currentUser.FullName;
            var currentId = currentUser.IdentityNumber;

            if (ModelState.IsValid)
            {

                if (_userService.returntype == 1)
                {
                    TempData["Error"] = _userService.Error;
                    return View(model);
                }
                else if (_userService.returntype == 2)
                {
                    TempData["Error"] = _userService.Error;
                    return View(model);
                }

                await _userService.AddUserColgeAsync(model, currentName, currentId);

                return RedirectToAction("ViewUsers", "GeneralFederation");


            }
            return View(model);
        }


        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var users = await _userService.GetAllAsync();
            ViewBag.UserCount = users.Count();

            var user = await _userService.GetByIdAsync((string)id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            //_Collegess.Clear();
            if (user == null)
            {
                return NotFound();
            }

            var newUser = new EditUserViewModel
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                IdentityNumber = user.IdentityNumber,
                IsBlocked = user.IsBlocked,
                DateOfBirth = user.DateOfBirth,
                CollegesId = user.CollegesId,
                DepartmentsId = user.DepartmentsId,
                DepartmentsName = user.Departments.Name,
                SubDepartmentsId = user.SubDepartmentsId,
                SubDepartmentsName = user.SubDepartments.Name,
                RoleId = user.RoleId,

            };
            ViewBag.ViewGover = newUser.CollegessList.ToArray();
            return View(newUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserViewModel user)
        {
            //var users = await _userService.GetAllAsync();
            //ViewBag.UserCount = users.Count();
            user.CollegessList = await _context.Collegess.ToListAsync();
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.UpdateAsync(id, user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ViewUsers");
            }
            return View();
        }

        [AllowAnonymous]

        //public async Task<IActionResult> CheckingIdentityNumber(AddUserViewModel model)
        //{
        //    var user = _userManager.FindByEmailAsync(model.IdentityNumber);

        //    var userr = _context.Users.Where(a => a.IdentityNumber == model.IdentityNumber).FirstOrDefault();
        //    if (userr == null)
        //    {
        //        return Json(true);
        //    }
        //    else
        //    {
        //        return Json($"يوجد رقم بطاقة   {model.IdentityNumber} من قبل بهذا الرقم ");
        //    }


        //}

        //[AllowAnonymous]
        //public Task<IActionResult> CheckingPhoneNumber(AddUserViewModel model)
        //{


        //    if (model.PhoneNumber.Length == 9)
        //    {
        //        return Task.FromResult<IActionResult>(Json(true));
        //    }

        //    else
        //    {
        //        return Task.FromResult<IActionResult>(Json($"   موجود من قبل {model.PhoneNumber}رقم الهاتف هذا"));
        //    }


        //}

        [HttpGet]
        public async Task<IActionResult> GetCollegess()
        {
            var Collegess = await _context.Collegess.ToListAsync();

            var result = Collegess.Select(d => new { id = d.Id, name = d.Name }).ToList();

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartmentssByCollegesId(int CollegesId)
        {
            var Departmentss = await _context.Departmentss.Where(d => d.CollegesId == CollegesId).ToListAsync();

            var result = Departmentss.Select(d => new { id = d.Id, name = d.Name }).ToList();

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubDepartmentssByDepartmentsId(int DepartmentsId)
        {
            var subDepartmentss = await _context.SubDepartmentss.Where(s => s.DepartmentsId == DepartmentsId).ToListAsync();

            var result = subDepartmentss.Select(s => new { id = s.Id, name = s.Name }).ToList();

            return Json(result);
        }


        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user != null)
            {
                var result = await _userService.DeleteAsync(id);
                if (result == null)
                {
                    return NotFound();
                }
                else if (result.StartsWith("حدث خطأ"))
                {

                    return View("Error", result);
                }
                else if (result.StartsWith("لا يمكن حذف هذا المستخدم"))
                {

                    return View("Error", result);
                }
                else
                {

                    return RedirectToAction(null);
                }
            }


            return RedirectToAction(null);
        }

        public async Task<IActionResult> Block(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var result = await _userService.TogelBlockUserAsync(id);
                if (result.Success)
                {
                    TempData["Succes"] = result.Message;
                }
                else
                {
                    TempData["Error"] = result.Message;
                }
                return RedirectToAction("ViewUsers", "GeneralFederation");
            }
            TempData["Error"] = "لم يتم العثور على رقم المستخدم";
            return RedirectToAction("ViewUsers", "GeneralFederation");

        }



        [HttpPost]
        public IActionResult Search(InputSearch input)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.Search(input.Term);
                return View(result);
            }
            return View();
        }
        public async Task<IActionResult> AccountRestriction()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentIdUser = currentUser.IdentityNumber;
            var result = _userService.GetAllUserBlockedAsync(currentIdUser);



            return View(result.ToList());

        }



        public async Task<IActionResult> UsersCounts()
        {
            var totalUsersCount = await _userService.UserRegistrationCountAsync();
            var month = DateTime.Today.Month;
            var monthUsersCount = await _userService.UserRegistrationCountAsync(month);

            return Json(new { tota = totalUsersCount, month = monthUsersCount });

        }





        private bool UserExists(string id)
        {
            return string.IsNullOrEmpty(_userService.GetByIdAsync(id).ToString());
        }


        //public async Task<IActionResult> ChaingeStatusAsync(int id, bool IsBlocked)
        //{
        //    await _userService.EnableAndDisbleUser(id);
        //    return RedirectToAction("ViewUsers");
        //}



        public async Task<IActionResult> UserReport(string userId)
        {
            var comSolution = _context.Compalints_Solutions.Where(u => u.UserId == userId)
                             .GroupBy(c => c.UploadsComplainteId);
            var ComplaintsRejecteds = _context.Compalints_Solutions.Where(u => u.UserId == userId)
                             .GroupBy(c => c.UploadsComplainteId);
            var user = await _userService.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var result = new UserReportVM
            {
                UserId = user.Id,
                TotlSolutionComp = comSolution.Count(),
                TotlRejectComp = ComplaintsRejecteds.Count(),
                //Orders = userGroup,
                FullName = user.FullName,
                Gov = user.Colleges.Name,
                Dir = user.Departments.Name,
                Role = user.RoleName,

            };


            return View(result);
        }



    }
}
