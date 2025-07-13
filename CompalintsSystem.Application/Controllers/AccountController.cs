using AutoMapper;
using CompalintsSystem.Core;
using CompalintsSystem.Core.Interfaces;
using CompalintsSystem.Core.Models;
using CompalintsSystem.Core.ViewModels;
using CompalintsSystem.EF.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CompalintsSystem.Application.Controllers
{

    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppCompalintsContextDB _context;


        public AccountController(
            IUserService userService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            AppCompalintsContextDB contex,
            IMapper mapper)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = contex;

        }


        //====(بداية دالة استيراد الطالب من ملف)=============================================ImportStudents

        [HttpGet]
        public IActionResult ImportStudents()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> ImportStudents(IFormFile excelFile)
        //{
        //    if (excelFile != null && excelFile.Length > 0)
        //    {
        //        if (excelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        //        {
        //            using (var stream = new MemoryStream())
        //            {
        //                excelFile.CopyTo(stream);
        //                stream.Position = 0;

        //                using (var package = new ExcelPackage(stream))
        //                {
        //                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
        //                    if (worksheet != null)
        //                    {
        //                        int rowCount = worksheet.Dimension.Rows;

        //                        for (int row = 2; row <= rowCount; row++)
        //                        {
        //                            string name = worksheet.Cells[row, 1].Value?.ToString();

        //                            if (!string.IsNullOrEmpty(name))
        //                            {
        //                                string email = worksheet.Cells[row, 2].Value?.ToString();
        //                                string phone = worksheet.Cells[row, 3].Value?.ToString();
        //                                string collegeId = worksheet.Cells[row, 4].Value?.ToString();
        //                                string departmentId = worksheet.Cells[row, 5].Value?.ToString();
        //                                string levelId = worksheet.Cells[row, 6].Value?.ToString();

        //                                Colleges college = _context.Collegess.FirstOrDefault(c => c.Name == collegeId);
        //                                Departments department = _context.Departmentss.FirstOrDefault(c => c.Name == departmentId);
        //                                SubDepartments subDepartment = _context.SubDepartmentss.FirstOrDefault(g => g.Name == levelId);


        //                                if (email != null)
        //                                {

        //                                    // تحقق من وجود الطالب في قاعدة البيانات
        //                                    string existingStudentMessage = CheckExistingStudent(name, email);
        //                                    if (existingStudentMessage != null)
        //                                    {
        //                                        ViewBag.Message = existingStudentMessage;
        //                                        return View();
        //                                    }
        //                                    else
        //                                    {

        //                                        AddUserViewModel student = new AddUserViewModel
        //                                        {
        //                                            FullName = name,
        //                                            IdentityNumber = email,
        //                                            PhoneNumber = phone,
        //                                            DateOfBirth = DateTime.Now,
        //                                            CollegesId = college.Id,
        //                                            DepartmentsId = department.Id,
        //                                            SubDepartmentsId = subDepartment.Id,
        //                                            Password = "Coding@1234?",

        //                                            PasswordConfirm = "Coding@1234?"
        //                                        };
        //                                        await _userService.RegisterAsync(student);
        //                                    }

        //                                }


        //                                else if (name != null)
        //                                {
        //                                    ViewBag.Message = $"الاسم غير موجود للطالب: {name}";
        //                                    return View();
        //                                }
        //                                else if (phone != null)
        //                                {
        //                                    ViewBag.Message = $"رقم التلفون غير موجود للطالب: {name}";
        //                                    return View();
        //                                }
        //                                else if (college != null)
        //                                {
        //                                    ViewBag.Message = $"الكلية غير موجود للطالب: {name}";
        //                                    return View();
        //                                }
        //                                else if (department != null)
        //                                {
        //                                    ViewBag.Message = $"القسم غير موجود للطالب: {name}";
        //                                    return View();
        //                                }
        //                                else if (subDepartment != null)
        //                                {
        //                                    ViewBag.Message = $"المستوى غير موجود للطالب: {name}";
        //                                    return View();
        //                                }
        //                                else
        //                                {
        //                                    ViewBag.Message = "خطأ في استيراد بيانات الطلاب.";
        //                                    return View();
        //                                }

        //                            }

        //                        }


        //                        ViewBag.Message = "تم استيراد بيانات الطلاب بنجاح.";
        //                        _context.SaveChanges();
        //                        return RedirectToAction("BeneficiariesAccount", "GeneralFederation");
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Message = "خطأ في قراءة ورقة العمل.";
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.Message = "يرجى اختيار ملف Excel للاستيراد.";
        //        }
        //    }

        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> ImportStudents(IFormFile excelFile)
        {
            try
            {
                if (excelFile != null && excelFile.Length > 0)
                {
                    if (excelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        using (var stream = new MemoryStream())
                        {
                            excelFile.CopyTo(stream);
                            stream.Position = 0;

                            using (var package = new ExcelPackage(stream))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                                if (worksheet != null)
                                {
                                    int rowCount = worksheet.Dimension.Rows;

                                    for (int row = 2; row <= rowCount; row++)
                                    {
                                        string name = worksheet.Cells[row, 1].Value?.ToString();

                                        if (!string.IsNullOrEmpty(name))
                                        {
                                            string email = worksheet.Cells[row, 2].Value?.ToString();
                                            string phone = worksheet.Cells[row, 3].Value?.ToString();
                                            string collegeId = worksheet.Cells[row, 4].Value?.ToString();
                                            string departmentId = worksheet.Cells[row, 5].Value?.ToString();
                                            string levelId = worksheet.Cells[row, 6].Value?.ToString();

                                            Colleges college = _context.Collegess.FirstOrDefault(c => c.Name == collegeId);
                                            Departments department = _context.Departmentss.FirstOrDefault(c => c.Name == departmentId);
                                            SubDepartments subDepartment = _context.SubDepartmentss.FirstOrDefault(g => g.Name == levelId);

                                            if (college == null)
                                            {
                                                ViewBag.Message = $"الكلية غير موجودة للطالب: {name}";
                                                return View();
                                            }

                                            if (department == null)
                                            {
                                                ViewBag.Message = $"القسم غير موجود للطالب: {name}";
                                                return View();
                                            }

                                            if (subDepartment == null)
                                            {
                                                ViewBag.Message = $"المستوى غير موجود للطالب: {name}";
                                                return View();
                                            }

                                            if (email != null)
                                            {
                                                string existingStudentMessage = CheckExistingStudent(name, email);
                                                if (existingStudentMessage != null)
                                                {
                                                    ViewBag.Message = existingStudentMessage;
                                                    return View();
                                                }
                                                else
                                                {
                                                    AddUserViewModel student = new AddUserViewModel
                                                    {
                                                        FullName = name,
                                                        IdentityNumber = email,
                                                        PhoneNumber = phone,
                                                        DateOfBirth = DateTime.Now,
                                                        CollegesId = college.Id,
                                                        DepartmentsId = department.Id,
                                                        SubDepartmentsId = subDepartment.Id,
                                                        Password = "Coding@1234?",
                                                        PasswordConfirm = "Coding@1234?"
                                                    };
                                                    await _userService.RegisterAsync(student);
                                                }
                                            }
                                            else
                                            {
                                                ViewBag.Message = $"البريد الإلكتروني غير موجود للطالب: {name}";
                                                return View();
                                            }
                                        }
                                    }

                                    ViewBag.Message = "تم استيراد بيانات الطلاب بنجاح.";
                                    _context.SaveChanges();
                                    return RedirectToAction("BeneficiariesAccount", "GeneralFederation");
                                }
                                else
                                {
                                    ViewBag.Message = "خطأ في قراءة ورقة العمل.";
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "يرجى اختيار ملف Excel للاستيراد.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"خطأ في استيراد بيانات الطلاب: {ex.Message}";
            }

            return View();
        }

        //==============================================(نهاية دالة استيراد الطالب من ملف)===


        //===============(دالة للتحقق من وجود الطالب)=====================================CheckExistingStudent
        //دالة للتحقق من وجود الطالب 
        private string CheckExistingStudent(string name, string email)
        {
            ApplicationUser existingStudent = _context.Users.FirstOrDefault(s => s.FullName == name || s.IdentityNumber == email);
            if (existingStudent != null)
            {
                if (existingStudent.FullName == name)
                {
                    return $"الاسم موجود بالفعل: {existingStudent.FullName}";
                }
                else if (existingStudent.IdentityNumber == email)
                {
                    return $"الايميل موجود: {existingStudent.IdentityNumber}";
                }
            }
            return null;
        }
        //==============================================(نهاية الدالة التي تتحقق من الطالب)===



        [HttpPost]
        public IActionResult ExportStudents()
        {
            var students = _context.Users.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Students");

                // العناوين
                worksheet.Cells[1, 1].Value = "الاسم";
                worksheet.Cells[1, 2].Value = "البريد الإلكتروني";
                worksheet.Cells[1, 3].Value = "رقم الهاتف";
                worksheet.Cells[1, 4].Value = " الكلية";
                worksheet.Cells[1, 5].Value = " القسم";
                worksheet.Cells[1, 6].Value = " المستوى";

                // البيانات
                for (int i = 0; i < students.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = students[i].FullName;
                    worksheet.Cells[i + 2, 2].Value = students[i].Email;
                    worksheet.Cells[i + 2, 3].Value = students[i].PhoneNumber;
                    Colleges college = _context.Collegess.FirstOrDefault(c => c.Id == students[i].CollegesId);
                    worksheet.Cells[i + 2, 4].Value = college.Name;
                    Departments department = _context.Departmentss.FirstOrDefault(c => c.Id == students[i].DepartmentsId);
                    worksheet.Cells[i + 2, 5].Value = department.Name;
                    SubDepartments subDepartment = _context.SubDepartmentss.FirstOrDefault(g => g.Id == students[i].SubDepartmentsId);
                    worksheet.Cells[i + 2, 6].Value = subDepartment.Name;
                }

                // تنسيق الجدول
                worksheet.Cells.AutoFitColumns();

                // توليد الملف وتنزيله
                var stream = new MemoryStream(package.GetAsByteArray());
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = "students.xlsx";

                return File(stream, contentType, fileName);
            }
        }











        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new AddUserViewModel()
            {

                CollegessList = await _context.Collegess.ToListAsync()
            };
            ViewBag.ViewGover = model.CollegessList.ToArray();
            return View(model);
        }


        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            //model.CollegessList = await _context.Collegess.ToListAsync();

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

                await _userService.RegisterAsync(model);

                return RedirectToAction("Login", "Account");


            }
            ViewBag.ViewGover = await _context.Collegess.ToListAsync();

            return View(model);

        }




        //[HttpGet]
        //public IActionResult Login()
        //{
        //    if (_signInManager.IsSignedIn(User))
        //        return RedirectToAction("");


        //    return View();
        //}




        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    TempData["Error"] = null;
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, true);
        //        if (result.Succeeded)
        //        {
        //            _ = model.Email;
        //            var user = await _userManager.FindByEmailAsync(model.Email);
        //            if (user.IsBlocked == false)
        //            {
        //                if (User.IsInRole("AdminGeneralFederation"))
        //                {
        //                    return RedirectToAction("Index", "GeneralFederation");

        //                }
        //                else if (User.IsInRole(UserRoles.Beneficiarie))
        //                {
        //                    return RedirectToAction("Index", "Beneficiarie");

        //                }
        //                else if (User.IsInRole(UserRoles.AdminColleges))
        //                {
        //                    return RedirectToAction("Index", "GovManageComplaints");

        //                }
        //                else if (User.IsInRole(UserRoles.AdminDepartments))
        //                {
        //                    return RedirectToAction("Report", "DirManageComplaints");

        //                }
        //                else if (User.IsInRole(UserRoles.AdminSubDepartments))
        //                {
        //                    return RedirectToAction("Index", "SubManageComplaints");

        //                }
        //                return RedirectToAction("AccessDenied");
        //            }
        //            else
        //            {
        //                // TempData["Error"] = " حسابك موقف!  الرجاء تنشيط الحساب من قبل المسؤول";
        //                return RedirectToAction("AccessDenied");

        //            }
        //        }

        //        TempData["Error"] = " تاكد من صحة كتابة رقم البطاقة او كلمة المرور";
        //        return View(model);
        //    }
        //    return View(model);
        //}


        [HttpGet]
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            TempData["Error"] = null;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    if (user.IsBlocked)
                    {
                        return await GetRedirectActionResultForUserRole(user);
                    }
                    else
                    {
                        return await GetRedirectActionResultForUserRole(user);
                    }
                }

                TempData["Error"] = "تاكد من صحة كتابة رقم البطاقة او كلمة المرور";
            }

            return View(model);
        }
        private async Task<IActionResult> GetRedirectActionResultForUserRole(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Contains(UserRoles.AdminGeneralFederation))
            {
                return Redirect(Url.Action("Index", "GeneralFederation"));
            }
            else if (userRoles.Contains(UserRoles.Beneficiarie))
            {
                return Redirect(Url.Action("Index", "Beneficiarie"));
            }
            else if (userRoles.Contains(UserRoles.AdminColleges))
            {
                return Redirect(Url.Action("Index", "GovManageComplaints"));
            }
            else if (userRoles.Contains(UserRoles.AdminDepartments))
            {
                return Redirect(Url.Action("Report", "DirManageComplaints"));
            }
            else if (userRoles.Contains(UserRoles.AdminSubDepartments))
            {
                return Redirect(Url.Action("Index", "SubManageComplaints"));
            }

            return RedirectToAction("Account", "Login");
        }



        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");


        }

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


        //منع الوصول 

        public IActionResult AccessDenied(string returnUrl)
        {
            // التحقق من أن المستخدم غير مسجل في النظام
            if (!_signInManager.IsSignedIn(User))
            {
                // إعادة توجيه المستخدم إلى صفحة تسجيل الدخول عندما يحاول المستخدم الوصول إلى صفحة محظورة بدون تسجيل الدخول
                return RedirectToAction("Account", "Login");
            }

            // إرجاع عرض AccessDenied مع إرجاع العنوان الخاص بالصفحة المحظورة
            ViewData["returnUrl"] = returnUrl;
            return View();

        }




    }

}

