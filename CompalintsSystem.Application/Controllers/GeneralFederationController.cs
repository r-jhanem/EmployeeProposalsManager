using CompalintsSystem.Core;
using CompalintsSystem.Core.Constants;
using CompalintsSystem.Core.Hubs;
using CompalintsSystem.Core.Interfaces;
using CompalintsSystem.Core.Models;
using CompalintsSystem.Core.Statistics;
using CompalintsSystem.Core.ViewModels;
using CompalintsSystem.Core.ViewModels.Data;
using CompalintsSystem.EF.DataBase;
using ComplantSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NToastNotify;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static CompalintsSystem.Core.HelperModal;

namespace CompalintsSystem.Application.Controllers
{

    [Authorize(Policy = "AdminPolicy")]
    public class GeneralFederationController : Controller
    {

        //private readonly ICompalintRepository _unitOfWork.CompalinteRepo;
        private readonly IUnitOfWork _unitOfWork;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotefcationHub> notificationHub;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _env;
        private readonly ICategoryService _service;
        private readonly AppCompalintsContextDB _context;
        private readonly ICompalintRepository __service;
        private readonly IUserService _userService;
        private readonly ICompalintRepository _compReop;
        public GeneralFederationController(
               ICompalintRepository __service,
            ICategoryService service,
            IUnitOfWork unitOfWork,
            IUserService userService,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotefcationHub> notificationHub,
            IWebHostEnvironment env,
            IToastNotification toastNotification,
               ICompalintRepository compReop,
            AppCompalintsContextDB context)
        {
            this.__service = __service;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            this.notificationHub = notificationHub;
            _service = service;
            _context = context;
            _compReop = compReop;
            _env = env;
            _toastNotification = toastNotification;
        }


        private string UserId
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }



        public async Task<IActionResult> Index()
        {
            var allComp = await _unitOfWork.Compalinte.GetAllAsync(g => g.Colleges);
            var result = await _unitOfWork.User.GetAllAsync(h => h.Colleges);
            var compSolv = allComp.Where(r => r.StatusCompalintId == 2);

            int totalcompSolv = compSolv.Count();
            int totalUsers = result.Count();
            int totalComp = allComp.Count();

            ViewBag.totalcompSolv = totalcompSolv;
            ViewBag.totalUsers = totalUsers;
            ViewBag.totalComp = totalComp;

            //------------- أحصائيات بالمستخدمين في كل كلية --------------------//


            List<UsersInStatistic> usersIn = new List<UsersInStatistic>();
            usersIn = ViewBag.totalGovermentuser;

            List<ApplicationUser> applicationUsers = await _context.Users

                .Include(su => su.Colleges).ToListAsync();

            //Totalcountuser
            int TotalUsers = applicationUsers.Count();

            ViewBag.Users = TotalUsers;

            ViewBag.totalGovermentuser = applicationUsers.GroupBy(j => j.CollegesId).Select(g => new UsersInStatistic
            {
                Name = g.First().Colleges.Name,
                totalUsers = g.Count().ToString(),
                Users = (g.Count() * 100) / TotalUsers


            }).ToList();



            //------------- نـــــهاية أحصائيات بالمستخدمين --------------------//


            //-------------أحصائيات انواع الشكاوى --------------------//

            // استرداد جميع شكاوى الرفع من قاعدة البيانات مع تضمين نوع الشكوى
            List<UploadsComplainte> compalints = await _context.UploadsComplaintes
                .Include(su => su.TypeComplaint).ToListAsync();

            // إنشاء قائمة للاحصائيات التي سيتم عرضها في المخطط البياني
            List<TypeCompalintStatistic> typeCompalints = new List<TypeCompalintStatistic>();

            // استرداد القائمة من ViewBag
            typeCompalints = ViewBag.GrapComplanrType;

            // حساب إجمالي عدد شكاوى الرفع
            int totalcomplant = compalints.Count();

            // إضافة إجمالي عدد شكاوى الرفع إلى ViewBag
            ViewBag.Totalcomplant = totalcomplant;

            // تجميع الشكاوى الرفع حسب نوعها وإنشاء قائمة من الإحصائيات
            ViewBag.GrapComplanrType = compalints.GroupBy(x => x.TypeComplaintId)
                .Select(g => new TypeCompalintStatistic
                {
                    Name = g.First().TypeComplaint.Type,
                    TotalCount = g.Count().ToString(),
                    TypeComp = (g.Count() * 100) / totalcomplant
                }).ToList();




            //------------- نهاية أحصائيات انواع الشكاوى --------------------//


            //-------------أحصائيات حالات الشكاوى --------------------//


            // استرداد جميع شكاوى الرفع من قاعدة البيانات مع تضمين حالة الشكوى
            List<UploadsComplainte> stutuscompalints = await _context.UploadsComplaintes
                .Include(su => su.StatusCompalint).ToListAsync();

            // إنشاء قائمة للاحصائيات التي سيتم عرضها في المخطط البياني
            List<StutusCompalintStatistic> stutusCompalints = new List<StutusCompalintStatistic>();

            // استرداد القائمة من ViewBag
            stutusCompalints = ViewBag.GrapComplanrStutus;

            // حساب إجمالي عدد شكاوى الرفع
            int totalStutuscomplant = stutuscompalints.Count();

            // إضافة إجمالي عدد شكاوى الرفع إلى ViewBag
            ViewBag.TotalStutusComplant = totalStutuscomplant;

            // تجميع الشكاوى الرفع حسب حالتها وإنشاء قائمة من الإحصائيات
            ViewBag.GrapComplanrStutus = stutuscompalints.GroupBy(s => s.StatusCompalintId)
                .Select(g => new StutusCompalintStatistic
                {
                    //id = 
                    Name = g.First().StatusCompalint.Name,
                    TotalCountStutus = g.Count().ToString(),
                    stutus = (g.Count() * 100) / totalStutuscomplant
                }).ToList();
            //------------- نهاية أحصائيات حالات الشكاوى --------------------//



            //-------------  أحصائيات عدد المستخدمين حسب الصلاحيات--------------------//

            List<ApplicationUser> usersRoles = await _context.Users.Include(x => x.UserRoles).ToListAsync();


            //Totalcountuser
            int totalCountByRole = applicationUsers.Count();

            ViewBag.TotalCountByRoles = totalCountByRole;

            // show Name Role Rether Than Id
            var Roles = _context.Roles.ToList();
            var x = from r in Globals.RolesLists
                    join u in usersRoles
                    on r.Id equals u.RoleId
                    select new ApplicationUser
                    {
                        RoleName = r.Name,
                        UserRoles = u.UserRoles
                    };

            List<UserByRolesStatistic> gtus = new List<UserByRolesStatistic>();
            gtus = ViewBag.UserByRoles;

            //total Users By Role
            ViewBag.UserByRoles = x.GroupBy(j => j.RoleName).Select(g => new UserByRolesStatistic
            {
                RoleName = g.First().RoleName,
                TotalCount = g.Count().ToString(),
                RolsTot = (g.Count() * 100) / totalCountByRole
            }).ToList();




            //------------------ نهاية أحصائيات عدد المستخدمين حسب الصلاحيات--------------------//


            //-------------  أحصائيات انواع اليلاغات    --------------------//



            List<UsersCommunication> communcations = await _context.UsersCommunications
                .Include(su => su.TypeCommunication).ToListAsync();
            List<TypeCommunicationStatistic> TotalTypeCommuncations = new List<TypeCommunicationStatistic>();

            int totalCommunication = communcations.Count();

            TotalTypeCommuncations = ViewBag.typeCommun;

            ViewBag.TypeCommuncations = communcations.GroupBy(x => x.TypeCommunication).Select(g => new TypeCommunicationStatistic
            {
                Name = g.First().TypeCommunication.Type,
                TotalCount = g.Count().ToString(),
                TypeComp = (g.Count() * 100) / totalCommunication
            }).ToList();

            //------------- نهاية أحصائيات انواع اليلاغات --------------------//


            //-------------  أحصائيات عدد اليلاغات    --------------------//


            List<TotalCommunicationStatistic> communicationsIn = new List<TotalCommunicationStatistic>();
            communicationsIn = ViewBag.totalcommunications;

            List<UsersCommunication> communications = await _context.UsersCommunications

                .Include(su => su.Colleges).ToListAsync();

            //Totalcountuser
            int TotalCommun = communications.Count();

            ViewBag.TotalCommun = TotalCommun;

            //total Govermentuser
            ViewBag.totalcommunications = communications.GroupBy(j => j.CollegesId).Select(g => new TotalCommunicationStatistic
            {
                Name = g.First().Colleges.Name,
                TotalCount = g.Count().ToString(),
                TypeComp = (g.Count() * 100) / TotalUsers

            }).ToList();

            //------------- نهاية أحصائيات عدد اليلاغات --------------------//

            return View();
        }

        public async Task<IActionResult> PrintReports()
        {



            //------------- أحصائيات بالمستخدمين في كل كلية --------------------//


            List<UsersInStatistic> usersIn = new List<UsersInStatistic>();
            usersIn = ViewBag.totalGovermentuser;

            List<ApplicationUser> applicationUsers = await _context.Users

                .Include(su => su.Colleges).ToListAsync();

            //Totalcountuser
            int TotalUsers = applicationUsers.Count();

            ViewBag.Users = TotalUsers;

            //total Govermentuser
            ViewBag.totalGovermentuser = applicationUsers.GroupBy(j => j.CollegesId).Select(g => new UsersInStatistic
            {
                Name = g.First().Colleges.Name,
                totalUsers = g.Count().ToString(),
                Users = (g.Count() * 100) / TotalUsers


            }).ToList();



            //------------- نـــــهاية أحصائيات بالمستخدمين --------------------//


            //-------------أحصائيات انواع الشكاوى --------------------//



            List<UploadsComplainte> compalints = await _context.UploadsComplaintes
                .Include(su => su.TypeComplaint).ToListAsync();
            List<TypeCompalintStatistic> typeCompalints = new List<TypeCompalintStatistic>();
            typeCompalints = ViewBag.GrapComplanrType;

            int totalcomplant = compalints.Count();
            ViewBag.Totalcomplant = totalcomplant;

            ViewBag.GrapComplanrType = compalints.GroupBy(x => x.TypeComplaintId).Select(g => new TypeCompalintStatistic
            {
                Name = g.First().TypeComplaint.Type,
                TotalCount = g.Count().ToString(),
                TypeComp = (g.Count() * 100) / totalcomplant
            }).ToList();




            //------------- نهاية أحصائيات انواع الشكاوى --------------------//


            //-------------أحصائيات حالات الشكاوى --------------------//


            List<UploadsComplainte> stutuscompalints = await _context.UploadsComplaintes
                .Include(su => su.StatusCompalint).ToListAsync();
            List<StutusCompalintStatistic> stutusCompalints = new List<StutusCompalintStatistic>();
            stutusCompalints = ViewBag.GrapComplanrStutus;

            int totalStutuscomplant = stutuscompalints.Count();
            ViewBag.TotalStutusComplant = totalStutuscomplant;

            ViewBag.GrapComplanrStutus = stutuscompalints.GroupBy(s => s.StatusCompalintId).Select(g => new StutusCompalintStatistic
            {
                //id = 
                Name = g.First().StatusCompalint.Name,
                TotalCountStutus = g.Count().ToString(),
                stutus = (g.Count() * 100) / totalStutuscomplant
            }).ToList();


            //------------- نهاية أحصائيات حالات الشكاوى --------------------//



            //-------------  أحصائيات عدد المستخدمين حسب الصلاحيات--------------------//

            List<ApplicationUser> UsersRoles = await _context.Users.Include(x => x.UserRoles).ToListAsync();

            //Totalcountuser
            int totalCountByRole = applicationUsers.Count();

            ViewBag.TotalCountByRoles = totalCountByRole;

            // show Name Role Rether Than Id
            var Roles = _context.Roles.ToList();
            var x = from r in Globals.RolesLists
                    join u in UsersRoles
                    on r.Id equals u.RoleId
                    select new ApplicationUser
                    {
                        RoleName = r.Name,
                        UserRoles = u.UserRoles
                    };

            //total Users By Role
            ViewBag.totalUserByRoles = x.GroupBy(j => j.RoleName).Select(g => new UserByRolesStatistic
            {
                RoleName = g.First().RoleName,
                TotalCount = g.Count().ToString(),
                RolsTot = (g.Count() * 100) / totalCountByRole

            }).ToList();


            List<UserByRolesStatistic> gtus = new List<UserByRolesStatistic>();
            gtus = ViewBag.totalUserByRoles;


            //------------------ نهاية أحصائيات عدد المستخدمين حسب الصلاحيات--------------------//


            //-------------  أحصائيات انواع اليلاغات    --------------------//

            // استرداد جميع سجلات الاتصال الموجودة مع بيانات نوع الاتصال المرتبطة بها
            List<UsersCommunication> communcations = await _context.UsersCommunications
                .Include(su => su.TypeCommunication).ToListAsync();

            // إنشاء قائمة جديدة لتخزين نتائج الإحصائيات
            List<TypeCommunicationStatistic> TotalTypeCommuncations = new List<TypeCommunicationStatistic>();

            // حساب إجمالي عدد الاتصالات
            int totalCommunication = communcations.Count();

            // تعيين القيمة الإجمالية لعدد الاتصالات
            TotalTypeCommuncations = ViewBag.typeCommun;

            // تحديد الإحصائيات لكل نوع اتصال وإضافتها إلى قائمة الإحصائيات
            ViewBag.TypeCommuncations = communcations.GroupBy(x => x.TypeCommunication).Select(g => new TypeCommunicationStatistic
            {
                Name = g.First().TypeCommunication.Type, // اسم نوع الاتصال
                TotalCount = g.Count().ToString(), // عدد الاتصالات المماثلة
                TypeComp = (g.Count() * 100) / totalCommunication // نسبة الاتصالات المماثلة
            }).ToList();




            //------------- نهاية أحصائيات انواع اليلاغات --------------------//


            //-------------  أحصائيات عدد اليلاغات    --------------------//


            List<TotalCommunicationStatistic> communicationsIn = new List<TotalCommunicationStatistic>();
            communicationsIn = ViewBag.totalcommunications;

            List<UsersCommunication> communications = await _context.UsersCommunications

                .Include(su => su.Colleges).ToListAsync();

            //Totalcountuser
            int TotalCommun = communications.Count();

            ViewBag.TotalCommun = TotalCommun;

            //total Govermentuser
            ViewBag.totalcommunications = communications.GroupBy(j => j.CollegesId).Select(g => new TotalCommunicationStatistic
            {
                Name = g.First().Colleges.Name,
                TotalCount = g.Count().ToString(),
                TypeComp = (g.Count() * 100) / TotalUsers

            }).ToList();

            //------------- نهاية أحصائيات عدد اليلاغات --------------------//
            return View();
        }

        //public async Task<IActionResult> AllComplaints()
        //{
        //    var allComp = await _unitOfWork.Compalinte.GetAllByOrderAsync(
        //        d => d.UploadDate,
        //         OrderBy.Descending,
        //        g => g.Colleges,
        //        g => g.StatusCompalint,
        //        g => g.TypeComplaint,
        //         g => g.ComplatinClassfaction

        //        );
        //    var totaleComp = allComp.Count(); ;
        //    ViewBag.totaleComp = totaleComp;
        //    return View(allComp);
        //}
        public async Task<IActionResult> AllComplaints()
        {
            var allComp = await _unitOfWork.Compalinte.GetAllByOrderAsync(
                d => d.UploadDate,
                 OrderBy.Descending,
                g => g.Colleges,
                g => g.StatusCompalint,
                g => g.TypeComplaint,
                g => g.ComplatinClassfaction
                );
            var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();

            ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
            ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

            ViewBag.status = ViewBag.StatusCompalints;
            var totaleComp = allComp.Count(); ;
            ViewBag.totaleComp = totaleComp;
            return View(allComp);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AllRejectedComplaints()
        {
            var allComp = await _unitOfWork.Compalinte.GetAllAsync(
                s => s.StatusCompalintId == 3,//جلب كافة الشكاوى بهذا الشرط
                g => g.Colleges);
            var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();

            ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
            ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

            ViewBag.status = ViewBag.StatusCompalints;
            var totaleComp = allComp.Count();
            ViewBag.totaleComp = totaleComp;
            return View(allComp);

        }
        public async Task<IActionResult> AllUsersUpComplaints()
        {
            var allComp = await _unitOfWork.Compalinte.GetByCondationAndOrderAsync(
                 s => s.UserRoleName == "Beneficiarie",
                 //s => s.StatusCompalintId == 3 && s.UserRoleName == "Beneficiarie",
                 d => d.UploadDate,
                 OrderBy.Descending,
                 g => g.Colleges // تضمين جدول الكليات

                );
            var totaleComp = allComp.Count(s => s.StatusCompalintId.Equals(3));
            ViewBag.totaleComp = totaleComp;
            return View(allComp);
        }
        public async Task<IActionResult> AllUpComplaints()
        {
            var allComp = await _unitOfWork.Compalinte.GetByCondationAndOrderAsync(
                 s => s.StatusCompalintId == 3,//جلب كافة الشكاوى بهذا الشرط
                 d => d.UploadDate,
                 OrderBy.Descending,
                 g => g.Colleges // تضمين جدول الكليات

                );
            var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();

            ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
            ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

            ViewBag.status = ViewBag.StatusCompalints;
            var totaleComp = allComp.Count(s => s.StatusCompalintId.Equals(3));
            ViewBag.totaleComp = totaleComp;
            return View(allComp);
        }

        public async Task<IActionResult> ViewCompalintUpDetailsDetails(int id)
        {
            // استرداد بيانات الشكوى المرتبطة بالمعرف المحدد
            var ComplantList = await _unitOfWork.Compalinte.FaindAsync(
                i => i.Id.Equals(id),
                g => g.Colleges,
                s => s.StatusCompalint,
                f => f.TypeComplaint,
                d => d.Departments,
                su => su.SubDepartments,
                st => st.StagesComplaint
            );

            // إنشاء عنصر عرض لإضافة الحلول
            AddSolutionVM addsoiationView = new AddSolutionVM()
            {
                UploadsComplainteId = id,
            };

            // إنشاء عنصر عرض للشكاوى المرفوضة
            ComplaintsRejectedVM rejectView = new ComplaintsRejectedVM()
            {
                UploadsComplainteId = id,
            };

            // إنشاء عنصر عرض لبيانات الشكوى والحلول المرتبطة بها
            ProvideSolutionsVM VM = new ProvideSolutionsVM
            {
                compalint = ComplantList,
                Compalints_SolutionList = await _context.Compalints_Solutions.Where(a => a.UploadsComplainteId == id).ToListAsync(),

                ComplaintsRejectedList = await _context.ComplaintsRejecteds.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                RejectedComplaintVM = rejectView,
                AddSolution = addsoiationView
            };

            // عرض العنصر النموذجي في الواجهة الأمامية للمستخدم
            return View(VM);
        }
        public async Task<IActionResult> SolutionComplaints()
        {
            var allComp = await _unitOfWork.Compalinte.GetByCondationAndOrderAsync(
                 s => s.StatusCompalintId == 2,// condation 
                 d => d.UploadDate,
                 OrderBy.Descending,
                 g => g.Colleges,
                 s => s.StatusCompalint,
                 f => f.TypeComplaint
                  );
            var totaleComp = allComp.Count(s => s.StatusCompalintId.Equals(2)); ;
            ViewBag.totaleComp = totaleComp;
            var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();

            ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
            ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

            ViewBag.status = ViewBag.StatusCompalints;
            return View(allComp);
        }

        // عرض المستخدمين من غير الطالبين
        public async Task<IActionResult> ViewUsers()
        {

            var result = await _context.Users.Where(r => r.RoleId != 5)
                .OrderByDescending(d => d.CreatedDate)
                .Include(s => s.Colleges)
                .Include(g => g.Departments)
                .Include(d => d.SubDepartments)
                .ToListAsync();

            int totalUsers = result.Count();

            ViewBag.totalUsers = totalUsers;


            //return View(await PaginatedList<ApplicationUser>.CreateAsync(result.AsNoTracking(), pageNumber ?? 1, pageSize));
            return View(result.ToList());

        }

        public async Task<IActionResult> ViewUsers2()
        {
            var result = await _context.Users.Where(r => r.RoleId != 5)
                .OrderByDescending(d => d.CreatedDate)
                .Include(s => s.Colleges)
                .Include(g => g.Departments)
                .Include(d => d.SubDepartments)
                .ToListAsync();
            int totalUsers = result.Count();

            ViewBag.totalUsers = totalUsers;


            //return View(await PaginatedList<ApplicationUser>.CreateAsync(result.AsNoTracking(), pageNumber ?? 1, pageSize));
            return View(result.ToList());

        }


        // GET: Users/Create
        public async Task<IActionResult> Create()
        {

            var currentUser = await _userManager.GetUserAsync(User);
            var model = new AddUserViewModel()
            {
                CollegessList = await _context.Collegess.ToListAsync(),
            };
            ViewBag.ViewGover = model.CollegessList.ToArray();

            return View(model);
        }

        // تأكد من أن النموذج يتم الإرسال به بطريقة POST وأنه صالح
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            // استرداد قائمة الكليات من قاعدة البيانات
            model.CollegessList = await _context.Collegess.ToListAsync();

            // استرداد المستخدم الحالي
            var currentUser = await _userManager.GetUserAsync(User);

            // الحصول على اسم المستخدم ورقم الهوية الحاليين
            var currentName = currentUser.FullName;
            var currentId = currentUser.IdentityNumber;


            try
            {
                // تحميل الصورة المرفقة مع النموذج
                UploadImage(model);

                // التحقق من أن النموذج صالح
                //if (ModelState.IsValid)
                //{
                //    if (_unitOfWork.User.returntype == 1)
                //    {
                //        TempData["Error"] = _unitOfWork.User.Error;
                //        return View(model);
                //    }
                //    else if (_unitOfWork.User.returntype == 2)
                //    {
                //        TempData["Error"] = _unitOfWork.User.Error;
                //        return View(model);
                //    }
                bool user = _userService.IdentityNamberDoesNotHaveToBeDuplicate(model.IdentityNumber);
                if (user == true)
                {
                    ModelState.AddModelError(string.Empty, "ان رقم هذا الحساب موجود بالفعل");
                    return View(model);
                }
                else if (ModelState.IsValid)
                {

                    if (_unitOfWork.User.returntype == 1)
                    {
                        TempData["Error"] = _unitOfWork.User.Error;
                        return View(model);
                    }
                    else if (_unitOfWork.User.returntype == 2)
                    {
                        TempData["Error"] = _unitOfWork.User.Error;
                        return View(model);
                    }
                    // إضافة المستخدم الجديد إلى قاعدة البيانات
                    await _unitOfWork.User.AddUserAsync(model, currentName, currentId);

                    // حفظ التغييرات في قاعدة البيانات
                    await _unitOfWork.Complete();

                    // عرض رسالة النجاح
                    _toastNotification.AddSuccessToastMessage(Constant.Messages.AddUser);

                    // إعادة توجيه المستخدم إلى صفحة عرض المستخدمين بعد إضافة المستخدم بنجاح
                    return RedirectToAction("Create", "GeneralFederation");
                    //return RedirectToAction(nameof(ViewUsers));
                }


            }
            catch (RetryLimitExceededException /* dex */)
            {
                // إذا حدث خطأ في قاعدة البيانات، عرض رسالة الخطأ
                ModelState.AddModelError("", "غير قادر على حفظ التغييرات. حاول مرة أخرى، وإذا استمرت المشكلة، فاستشر مدير نظامك.");
            }

            return View(model);
        }


        private void UploadImage(AddUserViewModel model)
        {

            var file = HttpContext.Request.Form.Files;
            if (file.Count() > 0)
            {  //@"wwwroot/"
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                var filestream = new FileStream(Path.Combine(_env.WebRootPath, "~/Images/", ImageName), FileMode.Create);
                file[0].CopyTo(filestream);
                model.ProfilePicture = ImageName;
            }
            else if (model.ProfilePicture == null)
            {
                model.ProfilePicture = "DefultImage.jpg";
            }
            else
            {
                model.ProfilePicture = model.ProfilePicture;
            }
        }
        //private void UploadImage2(EditUserViewModel model)
        //{

        //    var file = HttpContext.Request.Form.Files;
        //    if (file.Count() > 0)
        //    {  //@"wwwroot/"
        //        string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
        //        var filestream = new FileStream(Path.Combine(_env.WebRootPath, "~/Images/", ImageName), FileMode.Create);
        //        file[0].CopyTo(filestream);
        //        model.ProfilePicture = ImageName;
        //    }
        //    else if (model.ProfilePicture == null)
        //    {
        //        model.ProfilePicture = "DefultImage.jpg";
        //    }
        //    else
        //    {
        //        model.ProfilePicture = model.ProfilePicture;
        //    }
        //}

        //private void UploadImage2(EditUserViewModel model)
        //{
        //    try
        //    {
        //        var files = HttpContext.Request.Form.Files;

        //        if (files.Count > 0)
        //        {
        //            string uploadsFolder = Path.Combine(_env.WebRootPath, "Images");
        //            string uniqueFileName = Guid.NewGuid().ToString() + "_" + files[0].FileName;
        //            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                files[0].CopyTo(fileStream);
        //            }

        //            model.ProfilePicture = uniqueFileName;
        //        }
        //        else if (model.ProfilePicture == null)
        //        {
        //            model.ProfilePicture = "DefaultImage.jpg";
        //        }
        //        // No need for the else statement here, as it duplicates the assignment

        //        // Additional logic or validation can be added as needed
        //    }
        //    catch (Exception ex)
        //    {
        //        // يمكنك تسجيل الخطأ هنا، وطباعة رسالة الخطأ في تسجيل أخطاء النظام أو ملف السجلات
        //        Console.WriteLine($"Error during image upload: {ex.Message}");
        //        // يمكنك أيضا إلقاء الاستثناء مرة أخرى إذا كنت ترغب في تمريره إلى المستوى العلوي للتعامل معه هناك
        //        // throw;
        //    }
        //}


        // GET: Users/Details/5
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



        // GET: Users/Edit/5

        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //        //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var user = await _unitOfWork.User.GetByIdAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    List<Colleges> CollegesList = new List<Colleges>();
        //    CollegesList = (from d in _context.Collegess select d).ToList();
        //    //CollegesList.Insert(0, new Colleges { Id = 0, Name = "حدد الكلية" });
        //    ViewBag.ViewGover = CollegesList;
        //    var newUser = new EditUserViewModel
        //    {
        //        //CollegessList = await _context.Collegess.ToListAsync(),

        //        FullName = user.FullName,
        //        PhoneNumber = user.PhoneNumber,
        //        ProfilePicture = user.ProfilePicture,
        //        IdentityNumber = user.IdentityNumber,
        //        IsBlocked = user.IsBlocked,
        //        DateOfBirth = user.DateOfBirth,
        //        CollegesId = user.CollegesId,
        //        DepartmentsId = user.DepartmentsId,
        //        DepartmentsName = user.Departments.Name,
        //        SubDepartmentsId = user.SubDepartmentsId,
        //        SubDepartmentsName = user.SubDepartments.Name,
        //        RoleId = user.RoleId,

        //    };

        //ViewBag.ViewGover = newUser.CollegessList.ToArray();
        //    return View(newUser);
        //}
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.User.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            List<Colleges> collegesList = _context.Collegess.ToList();
            ViewBag.ViewGover = collegesList;

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
                DepartmentsName = user.Departments?.Name, // التحقق من عدم الإشارة إلى كائن Null
                SubDepartmentsId = user.SubDepartmentsId,
                SubDepartmentsName = user.SubDepartments?.Name, // التحقق من عدم الإشارة إلى كائن Null
                RoleId = user.RoleId,
            };

            return View(newUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserViewModel user)
        {
            //var users = await _unitOfWork.User.GetAllAsync();
            //ViewBag.UserCount = users.Count();

            if (UploadImage2(user) && ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.User.UpdateAsync(id, user);
                    return RedirectToAction(nameof(ViewUsers));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        // التعامل مع الاستثناءات الأخرى إذا لزم الأمر
                        return HandleUpdateException(user);
                    }
                }
            }

            return View(user);
        }

        private bool UploadImage2(EditUserViewModel model)
        {
            try
            {
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "Images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + files[0].FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    model.ProfilePicture = uniqueFileName;
                    return true; // نجاح حفظ الصورة
                }
                else if (model.ProfilePicture == null)
                {
                    model.ProfilePicture = "DefaultImage.jpg";
                    return true; // لا توجد صورة وتم تعيين الصورة الافتراضية
                }

                // لم يتم تحميل أي صورة
                return false;
            }
            catch (Exception ex)
            {
                // يمكنك تسجيل الخطأ هنا أو إلقاء استثناء مرة أخرى
                Console.WriteLine($"Error during image upload: {ex.Message}");
                return false;
            }
        }

        private IActionResult HandleUpdateException(EditUserViewModel user)
        {
            // يمكنك إضافة تعامل إضافي للتعامل مع استثناء تحديث قاعدة البيانات هنا
            ViewBag.ErrorMessage = "فشل في تحديث المستخدم. يرجى المحاولة مرة أخرى.";
            return View(user);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, EditUserViewModel user)
        //{
        //    //var users = await _unitOfWork.User.GetAllAsync();
        //    //ViewBag.UserCount = users.Count();
        //    UploadImage2(user);
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //            await _unitOfWork.User.UpdateAsync(id, user);
        //            return RedirectToAction(nameof(ViewUsers));
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserExists(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //    }
        //    return View(user);
        //}

        private bool UserExists(string id)
        {
            return string.IsNullOrEmpty(_unitOfWork.User.GetByIdAsync(id).ToString());
        }


        public async Task<IActionResult> AccountRestriction()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentIdUser = currentUser.IdentityNumber;
            var result = _unitOfWork.User.GetAllUserBlockedAsync(currentIdUser);



            return View(result.ToList());

        }

        public async Task<IActionResult> ChaingStatusComp(int id)
        {

            var upComp = await _unitOfWork.CompalinteRepo.FindAsync(id);
            var dbComp = await _context.UploadsComplaintes.FirstOrDefaultAsync(n => n.Id == upComp.Id);
            if (dbComp != null)
            {
                dbComp.StatusCompalintId = 2;

                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AllComplaints));

        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEditSolutions(int id = 0)
        {
            if (id == 0)
                return View(new ProvideSolutionsVM());
            else
            {
                var SolutionModel = await _context.Compalints_Solutions.FindAsync(id);
                if (SolutionModel == null)
                {
                    return NotFound();
                }
                return View(SolutionModel);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditSolutions(int id, ProvideSolutionsVM model)
        {
            //[Bind("Id,UserId,UserAddSolution,UploadsComplainteId,SolutionProvName,SolutionProvIdentity,IsAccept,Role,ContentSolution,DateSolution")]
            //Compalints_Solution model

            var currentUser = await _userManager.GetUserAsync(User);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var role = claimsIdentity.FindFirst(ClaimTypes.Role);
            string userRole = role.Value;
            string UserId = claim.Value;


            if (!ModelState.IsValid)
            {
                ModelState.Remove("ContentSolution");
                if (id == 0)
                {
                    var subuser = await _context.Users.Where(a => a.Id == UserId).FirstOrDefaultAsync();
                    var idComp = model.AddSolution.UploadsComplainteId;
                    var solution = new Compalints_Solution()
                    {
                        UserId = subuser.Id,
                        SolutionProvName = subuser.FullName,
                        UploadsComplainteId = model.AddSolution.UploadsComplainteId,
                        SolutionProvIdentity = subuser.IdentityNumber,
                        ContentSolution = model.AddSolution.ContentSolution,
                        DateSolution = DateTime.Now,
                        Role = userRole,
                    };

                    _context.Compalints_Solutions.Add(solution);
                    await _context.SaveChangesAsync();
                    var upComp = await _unitOfWork.CompalinteRepo.FindAsync(idComp);
                    var dbComp = await _context.UploadsComplaintes.FirstOrDefaultAsync(n => n.Id == upComp.Id);
                    if (dbComp != null)
                    {
                        dbComp.StatusCompalintId = 2;
                        dbComp.StagesComplaintId = 4;
                        await _context.SaveChangesAsync();
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var subuser = await _context.Users.Where(a => a.Id == UserId).FirstOrDefaultAsync();
                    var solutionUpdate = await _context.Compalints_Solutions.Where(sid => sid.Id == id).FirstOrDefaultAsync();
                    if (solutionUpdate != null)
                    {
                        solutionUpdate.UserId = subuser.Id;
                        solutionUpdate.SolutionProvName = subuser.FullName;
                        solutionUpdate.UploadsComplainteId = model.AddSolution.UploadsComplainteId;
                        solutionUpdate.SolutionProvIdentity = subuser.IdentityNumber;
                        //ContentSolution = model.AddSolution.ContentSolution,
                        solutionUpdate.ContentSolution = model.AddSolution.ContentSolution;
                        solutionUpdate.DateSolution = DateTime.Now;
                        solutionUpdate.Role = userRole;
                    }
                    try
                    {
                        _context.Update(solutionUpdate);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(ViewCompalintDetails), new { Id = model.AddSolution.UploadsComplainteId });
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TransactionModelExists(model.CompalintsSolution.Id))
                        { return NotFound(); }
                        else
                        { throw; }
                    }

                }

                return RedirectToAction(nameof(ViewCompalintDetails), new { Id = model.AddSolution.UploadsComplainteId });

            }

            return View("AddOrEditSolutions", model);

        }

        public async Task<IActionResult> AllCategoriesCommunications()
        {
            var allCategoriesComplaints = await _service.GetAllGategoryCommAsync();

            return View(allCategoriesComplaints);
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEditCategoryComm(int id = 0)
        {
            if (id == 0)
                return View(new TypeCommunication());
            else
            {
                var typeCommunicationModel = await _context.TypeCommunications.FindAsync(id);
                if (typeCommunicationModel == null)
                {
                    return NotFound();
                }
                return View(typeCommunicationModel);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditCategoryComm(int id, [Bind("Id,Type,UsersNameAddType,CreatedDate")] TypeCommunication typeCommunicationModel)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentName = currentUser.FullName;

            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {

                    TypeCommunication type = new TypeCommunication
                    {

                        Type = typeCommunicationModel.Type,
                        UsersNameAddType = currentName,
                        CreatedDate = typeCommunicationModel.CreatedDate = DateTime.Now,
                        UserId = currentUser.Id,
                    };
                    _context.Add(type);
                    await _context.SaveChangesAsync();

                }
                //Update
                else
                {
                    try
                    {
                        _context.Update(typeCommunicationModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TransactionModelExists(typeCommunicationModel.Id))
                        { return NotFound(); }
                        else
                        { throw; }
                    }
                }
                return Json(new { isValid = true, html = HelperModal.RenderRazorViewToString(this, "_ViewAllCatyCoryComm", _context.TypeCommunications.ToList()) });
            }

            return Json(new { isValid = false, html = HelperModal.RenderRazorViewToString(this, "AddOrEditCategoryComm", typeCommunicationModel) });


        }










        [NoDirectAccess]
        public async Task<IActionResult> AddOrEditCategoryCompalint(int id = 0)
        {
            if (id == 0)
                return View(new TypeComplaint());
            else
            {
                var typeComplaintModel = await _context.TypeComplaints.FindAsync(id);
                if (typeComplaintModel == null)
                {
                    return NotFound();
                }
                return View(typeComplaintModel);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditCategoryCompalint(int id, [Bind("Id,Type,UsersNameAddType,CreatedDate")] TypeComplaint typeComplaintModel)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentName = currentUser.FullName;


            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {

                    TypeComplaint type = new TypeComplaint
                    {

                        Type = typeComplaintModel.Type,
                        UsersNameAddType = currentName,
                        CreatedDate = typeComplaintModel.CreatedDate = DateTime.Now,
                        UserId = currentUser.Id,
                    };
                    _context.Add(type);
                    await _context.SaveChangesAsync();

                }
                //Update
                else
                {
                    try
                    {
                        _context.Update(typeComplaintModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TransactionModelExists(typeComplaintModel.Id))
                        { return NotFound(); }
                        else
                        { throw; }
                    }
                }
                return Json(new { isValid = true, html = HelperModal.RenderRazorViewToString(this, "_ViewAllCatycoryCompalints", _context.TypeComplaints.ToList()) });
            }
            return Json(new { isValid = false, html = HelperModal.RenderRazorViewToString(this, "AddOrEditCategoryCompalint", typeComplaintModel) });


        }

        public string NumberToWords(double doubleNumber)
        {
            int beforeFloatingPoint = (int)Math.Floor(doubleNumber);
            string beforeFloatingPointWord = string.Format("{0} ريال", NumberToWords(beforeFloatingPoint, "ريال"));
            string afterFloatingPointWord = string.Format("{0} هللة فقط.", SmallNumberToWord((int)((doubleNumber - beforeFloatingPoint) * 100), ""));
            if ((int)((doubleNumber - beforeFloatingPoint) * 100) > 0)
            {
                return string.Format("{0} و {1}", beforeFloatingPointWord, afterFloatingPointWord);
            }
            else
            {
                return string.Format("{0} فقط", beforeFloatingPointWord);
            }
        }

        private string NumberToWords(int number, string unit)
        {
            if (number == 0)
                return "صفر";

            if (number < 0)
                return "ناقص " + NumberToWords(Math.Abs(number), unit);

            var words = "";

            if (number / 1000000000 > 0)
            {
                words += NumberToWords(number / 1000000000, unit) + " مليار ";
                number %= 1000000000;
            }

            if (number / 1000000 > 0)
            {
                words += NumberToWords(number / 1000000, unit) + " مليون ";
                number %= 1000000;
            }

            if (number / 1000 > 0)
            {
                words += NumberToWords(number / 1000, unit) + " ألف ";
                number %= 1000;
            }

            if (number / 100 > 0)
            {
                words += NumberToWords(number / 100, unit) + " مئة ";
                number %= 100;
            }

            words = SmallNumberToWord(number, words);

            return words;
        }

        private string SmallNumberToWord(int number, string words)
        {
            if (number <= 0) return words;
            if (words != "")
                words += " ";

            var unitsMap = new[] { "صفر", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة", "عشرة", "أحد عشر", "اثني عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
            var tensMap = new[] { "صفر", "عشرة", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
            if (number < 2)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
            return words;
        }



        public async Task<IActionResult> ShowSolution(int id)
        {
            if (id == 0)
                return View(new Compalints_Solution());
            else
            {
                var SolutionModel = await _context.Compalints_Solutions.Where(i => i.Id == id).FirstOrDefaultAsync();
                if (SolutionModel == null)
                {
                    return NotFound();
                }
                //return Json(new { isValid = false, html = HelperModal.RenderRazorViewToString(this, "_ShowSolution", SolutionModel) });
                return PartialView("_ShowSolution", SolutionModel);
            }

        }


        public async Task<IActionResult> AllCategoriesComplaints()
        {
            var allCategoriesComplaints = await _service.GetAllGategoryCompAsync();

            return View(allCategoriesComplaints);
        }


        public async Task<IActionResult> ViewCompalintDetails(int id)
        {
            var ComplantList = await _unitOfWork.CompalinteRepo.FindAsync(id);
            var CompalintsSolution = await _context.Compalints_Solutions.Where(a => a.UploadsComplainteId == id).ToListAsync();
            var getAllUsers = await _context.Users.ToListAsync();

            AddSolutionVM addsoiationView = new AddSolutionVM()
            {
                UploadsComplainteId = id,


            };
            TransferComplaintToAnotherUser toAnotherUser = new TransferComplaintToAnotherUser()
            {
                ConplaintId = id,
            };

            ComplaintsRejectedVM rejectView = new ComplaintsRejectedVM()
            {
                UploadsComplainteId = id,

            };
            ProvideSolutionsVM VM = new ProvideSolutionsVM
            {
                compalint = ComplantList,
                Compalints_SolutionList = await _context.Compalints_Solutions.Where(a => a.UploadsComplainteId == id).OrderByDescending(t => t.DateSolution).ToListAsync(),
                ComplaintsRejectedList = await _context.ComplaintsRejecteds.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                UpComplaintCauseList = await _context.UpComplaintCauses.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                RejectedComplaintVM = rejectView,
                AddSolution = addsoiationView,
                users = getAllUsers,
            };
            return View(VM);
        }
        public async Task<IActionResult> ViewCompalintSolutionDetails(int id)
        {
            var ComplantList = await _unitOfWork.CompalinteRepo.FindAsync(id);
            AddSolutionVM addsoiationView = new AddSolutionVM()
            {
                UploadsComplainteId = id,

            };
            ComplaintsRejectedVM rejectView = new ComplaintsRejectedVM()
            {
                UploadsComplainteId = id,

            };
            ProvideSolutionsVM VM = new ProvideSolutionsVM
            {
                compalint = ComplantList,
                Compalints_SolutionList = await _context.Compalints_Solutions.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                ComplaintsRejectedList = await _context.ComplaintsRejecteds.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                RejectedComplaintVM = rejectView,
                AddSolution = addsoiationView
            };
            return View(VM);
        }
        public async Task<IActionResult> ViewCompalintRejectedDetails(int id)
        {
            var ComplantList = await _unitOfWork.CompalinteRepo.FindAsync(id);
            AddSolutionVM addsoiationView = new AddSolutionVM()
            {
                UploadsComplainteId = id,

            };
            ComplaintsRejectedVM rejectView = new ComplaintsRejectedVM()
            {
                UploadsComplainteId = id,

            };
            ProvideSolutionsVM VM = new ProvideSolutionsVM
            {
                compalint = ComplantList,
                Compalints_SolutionList = await _context.Compalints_Solutions.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                ComplaintsRejectedList = await _context.ComplaintsRejecteds.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                RejectedComplaintVM = rejectView,
                AddSolution = addsoiationView
            };
            return View(VM);
        }

        public async Task<IActionResult> ViewCompalintUpDetails(int id)
        {

            var ComplantList = await _unitOfWork.CompalinteRepo.FindAsync(id);
            AddSolutionVM addsoiationView = new AddSolutionVM()
            {
                UploadsComplainteId = id,

            };
            ComplaintsRejectedVM rejectView = new ComplaintsRejectedVM()
            {
                UploadsComplainteId = id,

            };
            UpComplaintVM upComplaint = new UpComplaintVM()
            {
                UploadsComplainteId = id,
            };
            ProvideSolutionsVM VM = new ProvideSolutionsVM
            {
                compalint = ComplantList,
                Compalints_SolutionList = await _context.Compalints_Solutions.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                ComplaintsRejectedList = await _context.ComplaintsRejecteds.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                RejectedComplaintVM = rejectView,
                UpComplaintCauseList = await _context.UpComplaintCauses.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                AddSolution = addsoiationView
            };
            return View(VM);
        }
        //public async Task<IActionResult> AllCommunication()
        //{
        //    var currentUser = await GetCurrentUser();
        //    var communicationDropdownsData = await GetCommunicationDropdownsData(currentUser);
        //    ViewBag.TypeCommunication = new SelectList(communicationDropdownsData.TypeCommunications, "Id", "Name");

        //    CommunicationVM communication = new CommunicationVM
        //    {
        //        CommunicationList = await _context.UsersCommunications.Where(u => u.reportSubmitterId == UserId).OrderByDescending(t => t.CreateDate).ToListAsync(),

        //    };


        //    int totalCompalints = communication.CommunicationList.Count();
        //    ViewBag.totalCompalints = totalCompalints;


        //    return View(communication);
        //}

        public async Task<IActionResult> TransferComplaintToAnotherUser(ProvideSolutionsVM model)
        {
            if (ModelState.IsValid)
            {

                //var complainte = await _compReop.FindAsync(model.ToAnotherUser.ConplaintId);
                var transfer = await _context.UploadsComplaintes.FirstOrDefaultAsync(n => n.Id == model.ToAnotherUser.ConplaintId);
                if (transfer != null)
                {
                    transfer.ReturnedTo = model.ToAnotherUser.UserId;
                    _context.UploadsComplaintes.Update(transfer);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("ComplaintsForMe", "CompalintsMang");
            }
            return View("ComplaintsForMe");
        }

        public async Task<IActionResult> ComplaintsForMe()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser.Id;

            var AllComplaintsUp = await _compReop.GetAllAsync(
                g => g.Colleges,
                d => d.Departments,
                s => s.SubDepartments,
                n => n.StatusCompalint,
                st => st.StagesComplaint,
                up => up.ComplaintsUp);


            if (AllComplaintsUp != null)
            {
                var Getrejected = AllComplaintsUp.Where(g => g.ReturnedTo == currentUser.Id);

                var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();

                ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
                ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

                ViewBag.status = ViewBag.StatusCompalints;
                int totalCompalintsUp = Getrejected.Count();
                ViewBag.totalCompalintsUp = totalCompalintsUp;

                return View(Getrejected);
            }
            var emptyList = Enumerable.Empty<UpComplaintCause>(); // إنشاء قائمة فارغة من الشكاوى
            return View(emptyList); // إرجاع قائمة فارغة في حالة عدم وجود شكاوى مرفوضة
        }


        

       
        private async Task<SelectDataCommuncationDropdownsVM> GetCommunicationDropdownsData(ApplicationUser currentUser)
        {
            var CollegesId = currentUser.CollegesId;
            var DepartmentsId = currentUser.DepartmentsId;
            var subDepartmentsId = currentUser.SubDepartmentsId;

            var roles = await _userManager.GetRolesAsync(currentUser);
            var rolesString = string.Join(",", roles);
            var roleId = _context.Roles.FirstOrDefault(role => role.Name == roles.FirstOrDefault())?.Id;

            return await __service.GetAddCommunicationDropdownsValues(subDepartmentsId, DepartmentsId, CollegesId, rolesString, roleId);
        }





    



public async Task<IActionResult> AllCommunication()
        {
            var currentUser = await GetCurrentUser();
            var communicationDropdownsData = await GetCommunicationDropdownsData(currentUser);

            ViewBag.TypeCommunication = new SelectList(communicationDropdownsData.TypeCommunications, "Id", "Name");

            var result = _context.UsersCommunications
            .OrderByDescending(d => d.CreateDate)
            .Include(s => s.reportSubmitter)
            .Include(s => s.TypeCommunication)
            .Include(g => g.Colleges)
            .Include(d => d.Departments)
            .Include(su => su.SubDepartments);



            //List<ApplicationUser> meeting = _context.Users.Where(m => m.Id == ).ToList<ApplicationUser>();


            int totalCompalints = result.Count();

            ViewBag.totalCompalints = totalCompalints;

            return View(result.ToList());
        }
        private async Task<ApplicationUser> GetCurrentUser()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser.Id.ToString();
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        
        private async Task<SelectDataCommuncationDropdownsVM> GetCommunicationDropdownsData2()
        {
            return await __service.GetAddCommunicationDropdownsValues2();
        }

        public async Task<IActionResult> ShowCommunication(int id)
        {
            if (id == 0)
                return View(new CommunicationVM());
            else
            {
                var usersCommunicationsModel = await _context.UsersCommunications.Where(i => i.Id == id).FirstOrDefaultAsync();
                if (usersCommunicationsModel == null)
                {
                    return NotFound();
                }

                return PartialView("_ShowCommunication", usersCommunicationsModel);
            }

        }





        //Get: Category/Delete/1

        public async Task<IActionResult> DeleteCategoriesComm(int? id)
        {

            if (id == null)
            {
                return View("Empty");
            }

            var typeCommunicationsModel = await _context.TypeCommunications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeCommunicationsModel == null)
            {
                return NotFound();
            }

            return View(typeCommunicationsModel);
        }


        public async Task<IActionResult> DeleteCategoryComm(int? id)
        {

            var typeCommunicationsModel = await _context.TypeCommunications.FindAsync(id);
            _context.TypeCommunications.Remove(typeCommunicationsModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AllCategoriesCommunications));
            //return Json(new { html = HelperModal.RenderRazorViewToString(this, "_ViewAllCatyCoryComm", _context.TypeCommunications.ToList()) });
        }

        public async Task<IActionResult> DeleteCategoriesComplaints(int? id)
        {

            if (id == null)
            {
                return View("Empty");
            }

            var typeComplaintsModel = await _context.TypeComplaints
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeComplaintsModel == null)
            {
                return NotFound();
            }

            return View(typeComplaintsModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoriesComplaints(int id)
        {

            var typeComplaintsModel = await _context.TypeComplaints.FindAsync(id);
            _context.TypeComplaints.Remove(typeComplaintsModel);
            await _context.SaveChangesAsync();
            //return Json(new { html = HelperModal.RenderRazorViewToString(this, "_ViewAllCatycoryCompalints", _context.TypeComplaints.ToList()) });
            return RedirectToAction(nameof(AllCategoriesComplaints));
        }


        public async Task<IActionResult> AllCirculars()
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync(UserRoles.AdminSubDepartments);
            var adminIds = adminUsers.Select(user => user.Id);
            if (adminIds.Any())
            {
                await notificationHub.Clients.Users(adminIds).SendAsync("ReceiveNotification", "WOOOOOOOOOOOOO");
            }
            return RedirectToAction();
        }

        public IActionResult AddCirculars()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectedThisComplaint(ProvideSolutionsVM model, int id)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var role = claimsIdentity.FindFirst(ClaimTypes.Role);
                string userRole = role.Value;
                string UserId = claim.Value;
                var subuser = await _context.Users.Where(a => a.Id == UserId).FirstOrDefaultAsync();
                var idComp = model.RejectedComplaintVM.UploadsComplainteId;
                var rejected = new ComplaintsRejected()
                {
                    UserId = subuser.Id,
                    RejectedProvName = subuser.FullName,
                    UploadsComplainteId = model.RejectedComplaintVM.UploadsComplainteId,

                    reume = model.RejectedComplaintVM.reume,
                    DateSolution = DateTime.Now,
                    Role = userRole,


                };

                _context.ComplaintsRejecteds.Add(rejected);
                await _context.SaveChangesAsync();


                var upComp = await _unitOfWork.CompalinteRepo.FindAsync(idComp);
                var dbComp = await _context.UploadsComplaintes.FirstOrDefaultAsync(n => n.Id == upComp.Id);
                if (dbComp != null)
                {
                    dbComp.StatusCompalintId = 3;
                    dbComp.StagesComplaintId = 4;
                    await _context.SaveChangesAsync();
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AllRejectedComplaints));

            }

            return NotFound();

        }

        public async Task<IActionResult> UserReportAsPDF(string Id)
        {
            var comSolution = _context.Compalints_Solutions.Where(u => u.UserId == Id)
                             .GroupBy(c => c.UploadsComplainteId);

            var AcceptSolution = _context.Compalints_Solutions.Where(u => u.UserId == Id)
                             .GroupBy(c => c.UploadsComplainteId, a => a.IsAccept);
            var ComplaintsRejecteds = _context.ComplaintsRejecteds.Where(u => u.UserId == Id)
                             .GroupBy(c => c.UploadsComplainteId);
            var user = await _unitOfWork.User.GetByIdAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            var result = new UserReportVM
            {
                UserId = user.Id,
                TotlSolutionComp = comSolution.Count(),
                TotlRejectComp = ComplaintsRejecteds.Count(),
                TotlAcceptSolution = AcceptSolution.Count(),
                //Orders = userGroup,
                FullName = user.FullName,
                Gov = user.Colleges.Name,
                Dir = user.Departments.Name,
                Role = user.RoleName,
                PhonNumber = user.PhoneNumber,
                CreatedDate = user.CreatedDate
            };


            return new ViewAsPdf("UserReportAsPDF", result)
            {
                PageOrientation = Orientation.Portrait,
                MinimumFontSize = 25,
                PageSize = Size.A4,
                CustomSwitches = " --print-media-type --no-background --footer-line --header-line --page-offset 0 --footer-center [page] --footer-font-size 8 --footer-right \"page [page] from [topage]\"  "
            };
        }
        public async Task<IActionResult> BeneficiariesAccount()
        {

            var result = await _context.Users.Where(r => r.RoleId == 5)
                .Include(g => g.Colleges)
                .Include(g => g.Departments)
                .Include(g => g.SubDepartments)
                .ToListAsync();

            int totalUsers = result.Count();

            ViewBag.totalUsers = totalUsers;


            return View(result.ToList());

        }
        [HttpGet]
        public async Task<IActionResult> Download(int id)
        {
            var selectedFile = await _unitOfWork.CompalinteRepo.FindAsync(id);
            if (selectedFile == null)
            {
                return NotFound();
            }

            //await _compService.IncreamentDownloadCount(id);

            var path = "~/Uploads/" + selectedFile.FileName;
            Response.Headers.Add("Expires", DateTime.Now.AddDays(-3).ToLongDateString());
            Response.Headers.Add("Cache-Control", "no-cache");
            return File(path, selectedFile.ContentType, selectedFile.OriginalFileName);
        }


        public async Task<IActionResult> ReportManagement()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartmentsies(int id)
        {
            List<Departments> Departments = new List<Departments>();
            Departments = await _context.Departmentss.Where(m => m.CollegesId == id).ToListAsync();
            return Json(new SelectList(Departments, "Id", "Name"));
        }

        [HttpGet]
        public async Task<IActionResult> GetSubDepartments(int id)
        {
            List<SubDepartments> subDepartments = new List<SubDepartments>();
            subDepartments = await _context.SubDepartmentss.Where(m => m.DepartmentsId == id).ToListAsync();
            return Json(new SelectList(subDepartments, "Id", "Name"));
        }





        public async Task<IActionResult> DisbleOrEnableUser(string id)
        {
            await _unitOfWork.User.TogelBlockUserAsync(id);
            return RedirectToAction("ViewUsers");


        }
        private bool TransactionModelExists(int id)
        {
            return _context.TypeCommunications.Any(e => e.Id == id);
        }


    }
}
