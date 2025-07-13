using AutoMapper;
using CompalintsSystem.Core.Interfaces;
using CompalintsSystem.Core.Models;
using CompalintsSystem.Core.ViewModels;
using CompalintsSystem.EF.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CompalintsSystem.Application.Controllers
{
    public class ManageProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;


        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICompalintRepository _compalintService;
        private readonly AppCompalintsContextDB context;
        private readonly IMapper mapper;

        public ManageProfileController(
            IUserService userService,
                        IUnitOfWork unitOfWork,

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
            _compalintService = compalintService;
            this.context = context;
            _unitOfWork = unitOfWork;

            this.mapper = mapper;
            _compalintService = compalintService;
        }



        public async Task<IActionResult> Profile(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var model = mapper.Map<UserViewModels>(currentUser);
                model.Collegess = context.Collegess.FirstOrDefault(g => g.Id == model.CollegesId);
                model.Departmentss = context.Departmentss.FirstOrDefault(g => g.Id == model.DepartmentsId);
                model.SubDepartments = context.SubDepartmentss.FirstOrDefault(g => g.Id == model.SubDepartmentsId);
                //var user = await _userService.GetByIdAsync((string)id);
                return View(model);
            }
            return NotFound();


        }
        [HttpPost]
        public async Task<IActionResult> Profile(UserViewModels model)
        {

            var currentUser = await _userManager.GetUserAsync(User);
            string userId = currentUser.Id;
            var user = await _userService.GetByIdAsync((string)userId);
            if (user == null)
            {
                return NotFound();
            }


            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditMyProfile()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var model = mapper.Map<UserProfileEditVM>(currentUser);

                return View(model);
            }
            return NotFound();
        }



        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMyProfile(UserProfileEditVM model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    currentUser.IdentityNumber = model.IdentityNumber;
                    currentUser.FullName = model.FullName;
                    currentUser.PhoneNumber = model.PhoneNumber;


                    var result = await _userManager.UpdateAsync(currentUser);
                    if (result.Succeeded)
                    {

                        return RedirectToAction("Profile");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    return View("Empty");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var users = await _unitOfWork.User.GetAllAsync();
            ViewBag.UserCount = users.Count();

            var user = await _unitOfWork.User.GetByIdAsync((string)id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }



        //private bool UserExists(int id)
        //{
        //    return string.IsNullOrEmpty(_userService.GetByIdAsync(id).ToString());
        //}

        public IActionResult ChangePassword(string user, string token)
        {
            //if (user == null || token == null)
            //{
            //    ModelState.AddModelError("", "إعادة تعيين رمز كلمة المرور غير صالح");
            //}
            //ChangePasswordViewModel result = new ChangePasswordViewModel
            //{
            //    CurrentPassword = token,
            //};
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePassword)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser != null)
            {
                if (ModelState.IsValid)
                {
                    var result = await _userManager.ChangePasswordAsync(currentUser, changePassword.CurrentPassword, changePassword.NewPassword);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("Login", "Account");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(result);
                }
                ModelState.AddModelError("", "  إعادة تعيين رمز كلمة المرور غير صالح قم بالتأكد من إدخال البيانات بشكل سليم");
            }
            else
            {
                return NotFound();
            }
            return View("/Views/ManageUsers/ChangePassword.cshtml", mapper.Map<ChangePasswordViewModel>(currentUser));


        }







    }
}
