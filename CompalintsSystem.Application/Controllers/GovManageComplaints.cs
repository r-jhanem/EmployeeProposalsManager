using CompalintsSystem.Core.Interfaces;
using CompalintsSystem.Core.Models;
using CompalintsSystem.Core.ViewModels;
using CompalintsSystem.Core.ViewModels.Data;
using CompalintsSystem.EF.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CompalintsSystem.Application.Controllers
{

    [Authorize(Roles = "AdminColleges")]
    public class GovManageComplaintsController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService _userService;
        private readonly ICompalintRepository _compReop;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly ICategoryService _service;
        private readonly AppCompalintsContextDB _context;
        private readonly ICompalintRepository __service;
        private readonly ICompalintRepository compalintRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICategoryService categoryService;
        public GovManageComplaintsController(
                        IUnitOfWork unitOfWork,

           ICategoryService service,
            ICompalintRepository compReop,
            IUserService userService,
            UserManager<ApplicationUser> userManager,

            IWebHostEnvironment env,
            ICompalintRepository compalintRepository,

            ICategoryService categoryService,
                 ICompalintRepository __service,
            AppCompalintsContextDB context)
        {

            _compReop = compReop;
            _unitOfWork = unitOfWork;

            _userService = userService;
            _userManager = userManager;
            _service = service;
            _context = context;
            _env = env;
            this.__service = __service;
            this.compalintRepository = compalintRepository;
            this.userManager = userManager;
            this.categoryService = categoryService;

        }

        private string UserId
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Create1()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var currentName = currentUser.FullName;
            var compalintDropdownsData = await categoryService.GetNewCompalintsDropdownsValues();
            ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");
            ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create1(InputCompmallintVM model)
        {

            var currentUser = await userManager.GetUserAsync(User);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var role = claimsIdentity.FindFirst(ClaimTypes.Role); // استخدام الكائن claimsIdentity بدلاً من ClaimsIdentity
            string userRole = role.Value;

            if (!ModelState.IsValid)
            {
                var Identity = currentUser.IdentityNumber;

                var compalintDropdownsData = await categoryService.GetNewCompalintsDropdownsValues();
                ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");
                ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");

                var newName = Guid.NewGuid().ToString(); //rre-rewrwerwer-gwgrg-grgr
                var extension = Path.GetExtension(model.File?.FileName);
                var fileName = string.Concat(newName, extension); // newName + extension
                var root = _env.WebRootPath;
                var path = Path.Combine(root, "Uploads", fileName);

                using (var fs = System.IO.File.Create(path))
                {
                    await model.File?.CopyToAsync(fs);
                }


                //await _service.CreateAsync(data);
                await __service.CreateAsync(new InputCompmallintVM
                {
                    TitleComplaint = model.TitleComplaint,
                    TypeComplaintId = model.TypeComplaintId,
                    DescComplaint = model.DescComplaint,
                    PropBeneficiarie = model.PropBeneficiarie,
                    CollegesId = currentUser.CollegesId,
                    DepartmentsId = currentUser.DepartmentsId,
                    SubDepartmentsId = currentUser.SubDepartmentsId,
                    UserId = Identity,
                    UserRoleName = userRole,
                    StagesComplaintId = model.StagesComplaintId = 4,// هذه المرحلة الخاصة بالشكوى كل رقم يبين مرحلة معينة عام ووو
                    OriginalFileName = model.File.FileName,
                    FileName = fileName,
                    ContentType = model.File.ContentType,
                    Size = model.File.Length,

                });

                return RedirectToAction(nameof(Index1));
            }
            return View(model);
        }

        public async Task<IActionResult> ViewResolvedComplaints()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var Identity = currentUser.IdentityNumber;
            var compalintDropdownsData = await categoryService.GetNewCompalintsDropdownsValues();
            ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
            ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

            ViewBag.status = ViewBag.StatusCompalints;

            var AllRejectedComplaints = __service.GetAllResolvedComplaints(Identity);
            return View(AllRejectedComplaints.ToList());

        }

        public async Task<IActionResult> ViewAllRejectedComplaints1()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var Identity = currentUser.IdentityNumber;
            var compalintDropdownsData = await categoryService.GetNewCompalintsDropdownsValues();
            ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
            ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

            ViewBag.status = ViewBag.StatusCompalints;

            var AllRejectedComplaints1 = __service.GetAllRejectedComplaints(Identity);

            return View(AllRejectedComplaints1.ToList());

        }

        public async Task<IActionResult> Yes(int id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index1));
            }
            else
            {
                var so = await _context.Compalints_Solutions.Where(a => a.Id == id).FirstOrDefaultAsync();

                //if(cc.StagesComplaintId==1)
                //  {
                //      cc.StagesComplaintId = 2;
                //  }
                //elseIf(cc.StagesComplaintId=2)
                //      {
                //      cc.StagesComplaintId = 3;
                //  }
                so.IsAccept = true;
                _context.Compalints_Solutions.Update(so);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index1));

            }
        }

        public async Task<IActionResult> No(int id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index1));
            }
            else
            {
                var solution = await _context.Compalints_Solutions
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (solution == null)
                {
                    return RedirectToAction(nameof(Index1));
                }

                solution.IsAccept = false;

                _context.Compalints_Solutions.Update(solution);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index1));
            }
        }
        public async Task<IActionResult> Index1()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var Identity = currentUser.IdentityNumber;
            var compalintDropdownsData = await categoryService.GetNewCompalintsDropdownsValues();

            ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
            ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");


            var result = __service.GetBenfeficarieCompalintBy(Identity);

            int totalCompalints = result.Count();

            ViewBag.totalCompalints = totalCompalints;

            return View(result.ToList());

        }

        public async Task<IActionResult> ViewCompalintDetails1(int id)
        {
            var currentUser = await GetCurrentUser();
            var ComplantList = await __service.FindAsync(id);
            var userDropdownsData = await GetUserDropdownsData(currentUser);

            ViewBag.UsersName = new SelectList(userDropdownsData.ApplicationUsers, "Id", "FullName");

            TransferComplaintToAnotherUser toAnotherUser = new TransferComplaintToAnotherUser()
            {
                ConplaintId = id,

            };
            AddSolutionVM addsoiationView = new AddSolutionVM()
            {
                UploadsComplainteId = id,

            };
            ProvideSolutionsVM MV = new ProvideSolutionsVM
            {
                compalint = ComplantList,
                Compalints_SolutionList = await _context.Compalints_Solutions.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                AddSolution = addsoiationView,
                users = userDropdownsData.ApplicationUsers,
                ToAnotherUser = toAnotherUser,
            };

            return View(MV);
        }



        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);



            //-------------أحصائيات بالمستخدمين التابعين لنفس قسم الموضف--------------------//


            List<UsersIn> usersIn = new List<UsersIn>();
            usersIn = ViewBag.totalUserSubDir;

            List<ApplicationUser> applicationUsers = await _context.Users
                .Where(s => s.CollegesId == currentUser.CollegesId && s.RoleId == 3)
                .Include(su => su.Departments).ToListAsync();

            //Totalcountuser
            int TotalUsers = applicationUsers.Count();

            ViewBag.Users = TotalUsers;

            //total Govermentuser
            ViewBag.totalUserSubDir = applicationUsers.GroupBy(j => j.DepartmentsId).Select(g => new UsersIn
            {
                Name = g.First().Departments.Name,
                totalUsers = g.Count().ToString(),
                Users = g.Count() * 100 / TotalUsers


            }).ToList();



            //------------- نـــــهاية أحصائيات بالمستخدمين التابعين لنفس قسم الموضف--------------------//



            //-------------أحصائيات بالطالبين التابعين لنفس قسم الموضف--------------------//


            List<UsersIn> usersBenfIn = new List<UsersIn>();
            usersBenfIn = ViewBag.totaBenflUserSubDir;

            List<ApplicationUser> applicationBenfUsers = await _context.Users
                .Where(s => s.CollegesId == currentUser.CollegesId && s.RoleId == 5)
                .Include(su => su.Departments).ToListAsync();

            //Totalcountuser
            int TotalBenfUsers = applicationBenfUsers.Count();

            ViewBag.BenfUsers = TotalBenfUsers;

            //total Govermentuser
            ViewBag.totalBenfUserSubDir = applicationBenfUsers.GroupBy(j => j.DepartmentsId).Select(g => new UsersIn
            {
                Name = g.First().Departments.Name,
                totalUsers = g.Count().ToString(),
                Users = g.Count() * 100 / TotalBenfUsers


            }).ToList();



            //------------- نـــــهاية أحصائيات بالطالبين التابعين لنفس قسم الموضف--------------------//


            //-------------أحصائيات انواع الشكاوى المقدمة لنفس قسم الموضف--------------------//



            List<UploadsComplainte> compalints = await _context.UploadsComplaintes
                .Where(s => s.DepartmentsId == currentUser.DepartmentsId)
                .Include(su => su.TypeComplaint).ToListAsync();
            List<TypeCompalints> typeCompalints = new List<TypeCompalints>();
            typeCompalints = ViewBag.GrapComplanrType;

            int totalcomplant = compalints.Count();
            ViewBag.Totalcomplant = totalcomplant;

            ViewBag.GrapComplanrType = compalints.GroupBy(x => x.TypeComplaintId).Select(g => new TypeCompalints
            {
                Name = g.First().TypeComplaint.Type,
                TotalCount = g.Count().ToString(),
                TypeComp = g.Count() * 100 / totalcomplant
            }).ToList();




            //------------- نهاية أحصائيات انواع الشكاوى المقدمة لنفس قسم الموضف--------------------//


            //-------------أحصائيات حالات الشكاوى المقدمة لنفس قسم الموضف--------------------//


            List<UploadsComplainte> stutuscompalints = await _context.UploadsComplaintes
                .Where(s => s.CollegesId == currentUser.CollegesId)
                .Include(su => su.StatusCompalint).ToListAsync();
            List<StutusCompalints> stutusCompalints = new List<StutusCompalints>();
            stutusCompalints = ViewBag.GrapComplanrStutus;

            int totalStutuscomplant = stutuscompalints.Count();
            ViewBag.TotalStutusComplant = totalStutuscomplant;

            ViewBag.GrapComplanrStutus = stutuscompalints.GroupBy(s => s.StatusCompalintId).Select(g => new StutusCompalints
            {
                Name = g.First().StatusCompalint.Name,
                TotalCountStutus = g.Count().ToString(),
                stutus = g.Count() * 100 / totalStutuscomplant
            }).ToList();






            //------------- نهاية أحصائيات حالات الشكاوى المقدمة لنفس قسم الموضف--------------------//
            return View();
        }


        public class TypeCompalints
        {
            public string Name { get; set; }

            public string TotalCount { get; set; }
            public double TypeComp { get; set; }

        }

        public class StutusCompalints
        {
            public string Name { get; set; }

            public string TotalCountStutus { get; set; }
            public double stutus { get; set; }

        }

        public class UsersIn
        {

            public string Name { get; set; }

            public string totalUsers { get; set; }
            public double Users { get; set; }

        }




        public async Task<IActionResult> NewCompalints(int? page)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var allCompalintsVewi = await _compReop.GetAllAsync(
                                    g => g.Colleges,
                                    d => d.Departments,
                                    b => b.SubDepartments,
                                    s => s.StatusCompalint,
                                    t => t.TypeComplaint,
                                    s => s.StagesComplaint
                                );



            var compBy = allCompalintsVewi
                    .Where(g =>
                        g.CollegesId == currentUser.CollegesId ||
                        (g.StatusCompalintId == 5)
                    );

            if (!compBy.Any())
            {
                // تحقق من الشرط الثاني إذا لم يتحقق الشرط الأول
                compBy = allCompalintsVewi
                    .Where(g =>
                        g.CollegesId == currentUser.CollegesId ||
                        g.ReturnedTo == currentUser.UserId
                    );
            }


            int totalCompalints = compBy.Count();
            //ViewBag.TotalCompalints = Convert.ToInt32(page != 0 && totalCompalints);
            ViewBag.TotalNewCompalints = totalCompalints;

            return View(compBy.ToList());
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
                var userIdentity = await _userManager.FindByEmailAsync(model.IdentityNumber);
                if (userIdentity != null)
                {
                    ModelState.AddModelError("Email", "email aoset");
                    model.CollegessList = await _context.Collegess.ToListAsync();
                    ViewBag.ViewGover = model.CollegessList.ToArray();
                    return View(model);
                }
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

                return RedirectToAction(nameof(ViewUsers));
            }
            return View(model);
        }


        public async Task<IActionResult> ViewUsers()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            //var result = await _userService.GetAllAsync(currentIdUser, gov, dir, sub);
            var result = await _context.Users.Where(d => d.UserId == currentUser.IdentityNumber && d.EmailConfirmed != false && d.RoleId != 5)
                .OrderByDescending(d => d.CreatedDate)
                .Include(g => g.Colleges)
                .Include(g => g.Departments)
                .Include(g => g.SubDepartments)
                .ToListAsync();

            int totalUsers = result.Count();

            ViewBag.totalUsers = totalUsers;
            //return View(await PaginatedList<ApplicationUser>.CreateAsync(result.AsNoTracking(), pageNumber ?? 1, pageSize));
            return View(result.ToList());

        }

        //public async Task<IActionResult> BeneficiariesAccount()
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    var result = await _context.Users.Where(r => r.RoleId == 6 && r.CollegesId == currentUser.CollegesId)
        //        .Include(g => g.Colleges)
        //        .Include(g => g.Departments)
        //        .Include(g => g.SubDepartments)
        //        .ToListAsync();

        //    int totalUsers = result.Count();

        //    ViewBag.totalUsers = totalUsers;


        //    return View(result.ToList());

        //}
        public async Task<IActionResult> BeneficiariesAccount()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var result = await _context.Users.Where(r => r.RoleId == 5 && r.CollegesId == currentUser.CollegesId)
                .Include(g => g.Colleges)
                .Include(g => g.Departments)
                .Include(g => g.SubDepartments)
                .ToListAsync();

            int totalUsers = result.Count();
            ViewBag.totalUsers = totalUsers;
            return View(result.ToList());
        }


        //public async Task<IActionResult> ViewUsers()
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    var result = await _context.Users.Where(d => d.UserId == currentUser.IdentityNumber && d.EmailConfirmed != false)
        //         .OrderByDescending(d => d.CreatedDate)
        //         .Include(g => g.Colleges)
        //         .Include(g => g.Departments)
        //         .Include(g => g.SubDepartments)
        //         .ToListAsync();


        //    int totalUsers = result.Count();

        //    ViewBag.totalUsers = totalUsers;





        //    //return View(await PaginatedList<ApplicationUser>.CreateAsync(result.AsNoTracking(), pageNumber ?? 1, pageSize));
        //    return View(result.ToList());

        //}


        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var users = await _userService.GetAllAsync();
            ViewBag.UserCount = users.Count();

            var user = await _userService.GetByIdAsync(id);
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

            if (user == null)
            {
                return NotFound();
            }
            List<Colleges> CollegesList = new List<Colleges>();
            CollegesList = (from d in _context.Collegess select d).ToList();
            CollegesList.Insert(0, new Colleges { Id = 0, Name = "حدد الكلية" });
            ViewBag.ViewGover = CollegesList;
            var newUser = new EditUserViewModel
            {
                //CollegessList = await _context.Collegess.ToListAsync(),

                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                IdentityNumber = user.IdentityNumber,
                IsBlocked = user.IsBlocked,
                DateOfBirth = user.DateOfBirth,
                CollegesId = user.Colleges.Id,
                DepartmentsId = user.Departments.Id,
                SubDepartmentsId = user.SubDepartments.Id,
                RoleId = user.RoleId,

            };

            //ViewBag.ViewGover = newUser.CollegessList.ToArray();
            return View(newUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserViewModel user)
        {
            //var users = await _userService.GetAllAsync();
            //ViewBag.UserCount = users.Count();

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
                return RedirectToAction(nameof(ViewUsers));
            }
            return View();
        }

        private bool UserExists(string id)
        {
            return string.IsNullOrEmpty(_userService.GetByIdAsync(id).ToString());
        }

        public async Task<IActionResult> AccountRestriction()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentIdUser = currentUser.IdentityNumber;
            var result = _userService.GetAllUserBlockedAsync(currentIdUser);



            return View(result.ToList());

        }

        //public async Task<IActionResult> DisbleOrEnableUser(int id)
        //{
        //    await _userService.TogelBlockUserAsync(id);
        //    return RedirectToAction(nameof(ViewUsers));


        //}
        public async Task<IActionResult> ViewCompalintSolutionDetails(int id)
        {
            var ComplantList = await _compReop.FindAsync(id);
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

        public async Task<IActionResult> AllComplaintHasUp(int? page)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var allCompalintsVewi = await _compReop.GetAllAsync(
                                    g => g.Colleges,
                                    d => d.Departments,
                                    b => b.SubDepartments,
                                    s => s.StagesComplaint
                                );

            var compBy = allCompalintsVewi
                .Where(g =>
                     g.CollegesId == currentUser.CollegesId && g.StatusCompalintId == 5
                //g.StagesComplaintId == 2 ||
                //g.StatusCompalintId == 5
                // g.CollegesId == currentUser.CollegesId ||
                //g.DepartmentsId == currentUser.DepartmentsId ||
                //g.SubDepartmentsId == currentUser.SubDepartmentsId
                );

            int totalCompalints = compBy.Count();
            //ViewBag.TotalCompalints = Convert.ToInt32(page != 0 && totalCompalints);
            ViewBag.TotalNewCompalints = totalCompalints;
            var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();

            ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
            ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

            ViewBag.status = ViewBag.StatusCompalints;

            ViewBag.totalCompalints = totalCompalints;

            return View(compBy.ToList());
        }
        public async Task<IActionResult> ViewCompalintRejectedDetails(int id)
        {
            var ComplantList = await _compReop.FindAsync(id);
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

        public async Task<IActionResult> ViewAllRejectedComplaints(int? page)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser.Id;
            var AllRejectedComplaints = await _compReop.GetAllAsync(g => g.Colleges, n => n.StatusCompalint);
            var Getrejected = AllRejectedComplaints.Where(
                   g => g.ComplaintsRejecteds != null && g.ComplaintsRejecteds.Any(rh => rh.UserId == userId)
                    && g.Colleges != null && g.Colleges.Id == currentUser.CollegesId

            );

            int totalCompalints = Getrejected.Count();
            ViewBag.TotalCompalints = Convert.ToInt32(page == 0 ? "false" : totalCompalints);
            ViewBag.totalCompalints = totalCompalints;

            return View(Getrejected);

        }


        //public async Task<IActionResult> SolutionComplaints()
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    var userId = currentUser.Id;
        //    var AllSolutionComplaints = await _compReop.GetAllAsync(
        //        g => g.Colleges,
        //        d => d.Departments,
        //        s => s.SubDepartments,
        //        n => n.StatusCompalint,
        //        st => st.StagesComplaint);
        //    var Getrejected = AllSolutionComplaints.Where(
        //        g => g.Compalints_Solutions != null && g.Compalints_Solutions.Any(rh => rh.UserId == userId)
        //            && g.Colleges != null && g.Colleges.Id == currentUser.CollegesId

        //        );
        //    ViewBag.status = ViewBag.StatusCompalints;
        //    int totalCompalints = Getrejected.Count();
        //    ViewBag.totalCompalints = totalCompalints;

        //    return View(Getrejected);
        //}

        public async Task<IActionResult> SolutionComplaints()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser.Id;

            var AllSolutionComplaints = await _compReop.GetAllAsync(
                g => g.Colleges,
                d => d.Departments,
                s => s.SubDepartments,
                n => n.StatusCompalint,
                st => st.StagesComplaint,
                sc => sc.Compalints_Solutions);

            if (AllSolutionComplaints != null)
            {
                var Getrejected = AllSolutionComplaints.Where(
                    g => g.Compalints_Solutions != null && g.Compalints_Solutions.Any(rh => rh.UserId == userId)
                         && g.Colleges != null && g.Colleges.Id == currentUser.CollegesId
                    //&& g.SubDepartments != null && g.SubDepartments.Id == currentUser.SubDepartmentsId
                    //&& g.StatusCompalint != null && g.StatusCompalint.Id == 2
                    );

                int totalCompalintsSolution = Getrejected.Count();
                ViewBag.TotalCompalintsSolution = totalCompalintsSolution;
                var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();

                ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
                ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

                ViewBag.status = ViewBag.StatusCompalints;
                return View(Getrejected);
            }
            var emptyList = Enumerable.Empty<UploadsComplainte>(); // إنشاء قائمة فارغة من الشكاوى
            return View(emptyList); // إرجاع قائمة فارغة في حالة عدم وجود شكاوى مرفوضة
        }

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


                int totalCompalintsUp = Getrejected.Count();
                ViewBag.totalCompalintsUp = totalCompalintsUp;
                var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();

                ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
                ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

                ViewBag.status = ViewBag.StatusCompalints;
                return View(Getrejected);
            }
            var emptyList = Enumerable.Empty<UpComplaintCause>(); // إنشاء قائمة فارغة من الشكاوى
            return View(emptyList); // إرجاع قائمة فارغة في حالة عدم وجود شكاوى مرفوضة
        }



        public async Task<IActionResult> ViewCompalintDetails(int id)
        {
            var ComplantList = await _compReop.FindAsync(id);
            var currentUser = await GetCurrentUser();
            var userDropdownsData = await GetUserDropdownsData(currentUser);
            ViewBag.UsersName = new SelectList(userDropdownsData.ApplicationUsers, "Id", "FullName");

            TransferComplaintToAnotherUser toAnotherUser = new TransferComplaintToAnotherUser()
            {
                ConplaintId = id,

            };
            AddSolutionVM addsoiationView = new AddSolutionVM()
            {
                UploadsComplainteId = id,

            };
            ComplaintsRejectedVM rejectView = new ComplaintsRejectedVM()
            {
                UploadsComplainteId = id,

            };
            UpComplaintVM up = new UpComplaintVM()
            {
                UploadsComplainteId = id,
            };
            ProvideSolutionsVM VM = new ProvideSolutionsVM
            {
                compalint = ComplantList,
                Compalints_SolutionList = await _context.Compalints_Solutions.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                ComplaintsRejectedList = await _context.ComplaintsRejecteds.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                RejectedComplaintVM = rejectView,
                UpComplaint = up,
                UpComplaintCauseList = await _context.UpComplaintCauses.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                AddSolution = addsoiationView,
                ToAnotherUser = toAnotherUser,
            };
            return View(VM);
        }

        //public async Task<IActionResult> UpCompalint(int id, UploadsComplainte complainte)
        //{

        //    var upComp = await _compReop.FindAsync(id);
        //    var dbComp = await _context.UploadsComplaintes.FirstOrDefaultAsync(n => n.Id == upComp.Id);
        //    if (dbComp != null)
        //    {

        //        dbComp.Id = complainte.Id;
        //        dbComp.StatusCompalintId = 5;
        //        dbComp.StagesComplaintId = dbComp.StagesComplaintId + 1;


        //        await _context.SaveChangesAsync();
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("AllUpComplaints");

        //}
        // رفع الشكوى مع سبب الرفع للإدارة العلياء 

        //public async Task<IActionResult> RejectedThisComplaint(int id, UploadsComplainte complainte)
        //{

        //    var upComp = await _compReop.FindAsync(id);
        //    var dbComp = await _context.UploadsComplaintes.FirstOrDefaultAsync(n => n.Id == upComp.Id);
        //    if (dbComp != null)
        //    {

        //        dbComp.Id = complainte.Id;
        //        dbComp.StatusCompalintId = 3;


        //        await _context.SaveChangesAsync();
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(AllRejectedComplaints));

        //}


        //public async Task<IActionResult> AllRejectedComplaints()
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    var userId = currentUser.Id;
        //    var AllRejectedComplaints = await _compReop.GetAllAsync(
        //        g => g.Colleges,
        //        d => d.Departments,
        //        s => s.SubDepartments,
        //        n => n.StatusCompalint,
        //        st => st.StagesComplaint);
        //    var Getrejected = AllRejectedComplaints.Where(
        //        g => g.ComplaintsRejecteds != null && g.ComplaintsRejecteds.Any(rh => rh.UserId == userId)
        //            && g.Colleges != null && g.Colleges.Id == currentUser.CollegesId

        //    );
        //    var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();

        //    ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
        //    ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

        //    ViewBag.status = ViewBag.StatusCompalints;
        //    int totalCompalints = Getrejected.Count();
        //    //ViewBag.TotalCompalints = Convert.ToInt32(page == 0 ? "false" : totalCompalints);
        //    ViewBag.totalCompalints = totalCompalints;

        //    return View(Getrejected);

        //}
        public async Task<IActionResult> AllRejectedComplaints()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser.Id;

            var AllRejectedComplaints = await _compReop.GetAllAsync(
                g => g.Colleges,
                d => d.Departments,
                s => s.SubDepartments,
                n => n.StatusCompalint,
                st => st.StagesComplaint,
                rc => rc.ComplaintsRejecteds);
            if (AllRejectedComplaints != null)
            {
                var Getrejected = AllRejectedComplaints.Where(
                    g => g.ComplaintsRejecteds != null
                    && g.ComplaintsRejecteds != null && g.ComplaintsRejecteds.Any(rh => rh.UserId == userId)
                      && g.Colleges != null && g.Colleges.Id == currentUser.CollegesId
                    //&& g.StatusCompalint != null && g.StatusCompalint.Id == 4
                    //&& g.StagesComplaint != null && g.StagesComplaint.Id == 3
                    );

                var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();

                ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
                ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

                ViewBag.status = ViewBag.StatusCompalints;
                int totalCompalintsRejected = Getrejected.Count();
                ViewBag.TotalCompalintsRejected = totalCompalintsRejected;

                return View(Getrejected);
            }
            var emptyList = Enumerable.Empty<UploadsComplainte>(); // إنشاء قائمة فارغة من الشكاوى
            return View(emptyList); // إرجاع قائمة فارغة في حالة عدم وجود شكاوى مرفوضة

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSolutions(ProvideSolutionsVM model, int id)
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


                var upComp = await _compReop.FindAsync(idComp);
                var dbComp = await _context.UploadsComplaintes.FirstOrDefaultAsync(n => n.Id == upComp.Id);
                if (dbComp != null)
                {
                    dbComp.StatusCompalintId = 2;
                    dbComp.StagesComplaintId = 4;
                    await _context.SaveChangesAsync();
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("SolutionComplaints");

            }

            return NotFound();


        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> RejectedThisComplaint(ProvideSolutionsVM model, int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return NotFound();
        //    }

        //    var currentUser = await _userManager.GetUserAsync(User);
        //    var claimsIdentity = (ClaimsIdentity)User.Identity;
        //    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        //    var role = claimsIdentity.FindFirst(ClaimTypes.Role);
        //    string userRole = role.Value;
        //    string UserId = claim.Value;
        //    var subuser = await _context.Users.FindAsync(UserId);
        //    var idComp = model.RejectedComplaintVM.UploadsComplainteId;

        //    var complaintsRejected = new ComplaintsRejected()
        //    {
        //        UserId = subuser.Id,
        //        RejectedProvName = subuser.FullName,
        //        UploadsComplainteId = model.RejectedComplaintVM.UploadsComplainteId,
        //        RejectedUserProvIdentity = subuser.IdentityNumber,
        //        reume = model.RejectedComplaintVM.reume,
        //        DateSolution = DateTime.Now,
        //        Role = userRole,
        //    };

        //    _context.ComplaintsRejecteds.Add(complaintsRejected);
        //    await _context.SaveChangesAsync();

        //    var upComp = await _compReop.FindAsync(idComp);
        //    var dbComp = await _context.UploadsComplaintes.FirstOrDefaultAsync(n => n.Id == upComp.Id);
        //    if (dbComp != null)
        //    {
        //        dbComp.StatusCompalintId = 3;
        //        dbComp.StagesComplaintId = 4;
        //        await _context.SaveChangesAsync();
        //    }

        //    return RedirectToAction("AllRejectedComplaints");
        //}
        public async Task<IActionResult> RejectedThisComplaint(int id, UploadsComplainte complainte)
        {
            var upComp = await _compReop.FindAsync(id);
            var dbComp = await _context.UploadsComplaintes.FirstOrDefaultAsync(n => n.Id == upComp.Id);
            if (dbComp != null)
            {
                dbComp.Id = complainte.Id;

                dbComp.StatusCompalintId = 3;

                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AllRejectedComplaints));

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

                var complaintsRejected = new ComplaintsRejected()
                {
                    UserId = subuser.Id,
                    RejectedProvName = subuser.FullName,
                    UploadsComplainteId = model.RejectedComplaintVM.UploadsComplainteId,
                    RejectedUserProvIdentity = subuser.IdentityNumber,
                    reume = model.RejectedComplaintVM.reume,
                    DateSolution = DateTime.Now,
                    Role = userRole,

                };

                _context.ComplaintsRejecteds.Add(complaintsRejected);
                await _context.SaveChangesAsync();

                var upComp = await _compReop.FindAsync(idComp);
                var dbComp = await _context.UploadsComplaintes.FirstOrDefaultAsync(n => n.Id == upComp.Id);
                if (dbComp != null)
                {
                    dbComp.StatusCompalintId = 3;
                    dbComp.StagesComplaintId = 4;
                    await _context.SaveChangesAsync();
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("AllRejectedComplaints");

            }

            return NotFound();

        }
        //public async Task<IActionResult> AllUpComplaints()
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    var userId = currentUser.Id;
        //    var allComp = _compReop.GetAll().Where(g => g.ComplaintsUp != null
        //            && g.ComplaintsUp.Any(rh => rh.UserId == userId)
        //             && g.Colleges != null && g.Colleges.Id == currentUser.CollegesId);
        //    var totaleComp = allComp.Count();
        //    ViewBag.totaleComp = totaleComp;
        //    return View(allComp);
        //}
        public async Task<IActionResult> AllUpComplaints()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser.Id;

            var AllUpComplaints = await _compReop.GetAllAsync(
                g => g.Colleges,
                d => d.Departments,
                s => s.SubDepartments,
                n => n.StatusCompalint,
                st => st.StagesComplaint,
                up => up.ComplaintsUp);

            if (AllUpComplaints != null)
            {
                var Getrejected = AllUpComplaints.Where(
                    g => g.ComplaintsUp != null
                    && g.ComplaintsUp != null && g.ComplaintsUp.Any(rh => rh.UserId == userId)
                      || g.Colleges != null && g.Colleges.Id == currentUser.CollegesId
                    //&& g.StatusCompalint != null && g.StatusCompalint.Id == 5
                    //&& g.StagesComplaint != null && g.StagesComplaint.Id == 3
                    );


                int totalCompalints = Getrejected.Count();
                ViewBag.TotalCompalintsUp = totalCompalints;
                var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();

                ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
                ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

                ViewBag.status = ViewBag.StatusCompalints;
                return View(Getrejected);
            }
            var emptyList = Enumerable.Empty<UploadsComplainte>(); // إنشاء قائمة فارغة من الشكاوى
            return View(emptyList); // إرجاع قائمة فارغة في حالة عدم وجود شكاوى مرفوضة
        }
        public async Task<IActionResult> ViewCompalintUpDetails(int id)
        {

            var ComplantList = await _compReop.FindAsync(id);
            var currentUser = await GetCurrentUser();
            var userDropdownsData = await GetUserDropdownsData(currentUser);

            ViewBag.UsersName = new SelectList(userDropdownsData.ApplicationUsers, "Id", "FullName");

            TransferComplaintToAnotherUser toAnotherUser = new TransferComplaintToAnotherUser()
            {
                ConplaintId = id,

            };
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
                AddSolution = addsoiationView,
                ToAnotherUser = toAnotherUser

            };
            return View(VM);
        }
        public async Task<IActionResult> UpCompalint(int id, ProvideSolutionsVM model)
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

                var idComp = model.UpComplaint.UploadsComplainteId;
                var upComplaint = new UpComplaintCause()
                {
                    UserId = subuser.Id,
                    UpProvName = subuser.FullName,
                    UploadsComplainteId = model.UpComplaint.UploadsComplainteId,
                    UpUserProvIdentity = subuser.IdentityNumber,
                    Cause = model.UpComplaint.Cause,
                    DateUp = DateTime.Now,
                    Role = userRole,


                };

                _context.UpComplaintCauses.Add(upComplaint);
                await _context.SaveChangesAsync();


                var upComp = await _compReop.FindAsync(idComp);
                var dbComp = await _context.UploadsComplaintes.FirstOrDefaultAsync(n => n.Id == upComp.Id);
                if (dbComp != null)
                {
                    dbComp.StatusCompalintId = 5;
                    dbComp.StagesComplaintId = upComp.StagesComplaintId + 1;
                    await _context.SaveChangesAsync();
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AllUpComplaints));



            }
            return NotFound();
        }


        // كل البلاغات المقدمة
        public async Task<IActionResult> AllCommunication()
        {
            var currentUser = await GetCurrentUser();
            var communicationDropdownsData = await GetCommunicationDropdownsData(currentUser);
            ViewBag.TypeCommunication = new SelectList(communicationDropdownsData.TypeCommunications, "Id", "Name");

            CommunicationVM communication = new CommunicationVM
            {
                CommunicationList = await _context.UsersCommunications.Where(u => u.reportSubmitterId == UserId).OrderByDescending(t => t.CreateDate).ToListAsync(),

            };


            int totalCompalints = communication.CommunicationList.Count();
            ViewBag.totalCompalints = totalCompalints;


            return View(communication);
        }



        private async Task<SelectDataCommuncationDropdownsVM> GetCommunicationDropdownsData2()
        {
            return await __service.GetAddCommunicationDropdownsValues2();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AddCommunication()
        {

            var currentUser = await GetCurrentUser();
            var communicationDropdownsData = await GetCommunicationDropdownsData(currentUser);

            ViewBag.typeCommun = new SelectList(communicationDropdownsData.TypeCommunications, "Id", "Type");
            ViewBag.UsersName = new SelectList(communicationDropdownsData.ApplicationUsers, "Id", "FullName");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCommunication(AddCommunicationVM communication)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await GetCurrentUser();
                var communicationDropdownsData = await GetCommunicationDropdownsData2();

                var currentName = currentUser.FullName;
                var reporteeId = communication.reporteeName;

                var currentPhone = currentUser.PhoneNumber;
                var currentGov = currentUser.CollegesId;
                var currentDir = currentUser.DepartmentsId;
                var currentSub = currentUser.SubDepartmentsId;
                var getReporteeName = await _context.Users.Where(x => x.Id == reporteeId).FirstOrDefaultAsync();
                var reporteeName = getReporteeName.FullName;
                await __service.CreateCommuncationAsync(new AddCommunicationVM
                {
                    Titile = communication.Titile,
                    reporteeName = reporteeName,
                    reason = communication.reason,
                    CreateDate = communication.CreateDate,
                    TypeCommuncationId = communication.TypeCommuncationId,
                    reportSubmitterId = currentUser.Id,
                    reportSubmitterName = currentName,
                    BenfPhoneNumber = currentPhone,
                    CollegesId = currentGov,
                    DepartmentsId = currentDir,
                    SubDepartmentsId = currentSub,

                });


                return RedirectToAction(nameof(AllCommunication));
            }
            return View(communication);
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

        private async Task<ApplicationUser> GetCurrentUser()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var userId = currentUser.Id.ToString();
            var user = await userManager.FindByIdAsync(userId);
            return user;
        }

        private async Task<SelectDataCommuncationDropdownsVM> GetCommunicationDropdownsData(ApplicationUser currentUser)
        {
            var CollegesId = currentUser.CollegesId;
            var DepartmentsId = currentUser.DepartmentsId;
            var subDepartmentsId = currentUser.SubDepartmentsId;

            var roles = await userManager.GetRolesAsync(currentUser);
            var rolesString = string.Join(",", roles);
            var roleId = _context.Roles.FirstOrDefault(role => role.Name == roles.FirstOrDefault())?.Id;

            return await __service.GetAddCommunicationDropdownsValues(subDepartmentsId, DepartmentsId, CollegesId, rolesString, roleId);
        }
        public async Task<IActionResult> UserReportAsPDF(string Id)
        {
            var comSolution = _context.Compalints_Solutions.Where(u => u.UserId == Id)
                             .GroupBy(c => c.UploadsComplainteId);

            var AcceptSolution = _context.Compalints_Solutions.Where(u => u.UserId == Id)
                             .GroupBy(c => c.UploadsComplainteId, a => a.IsAccept);
            var ComplaintsRejecteds = _context.ComplaintsRejecteds.Where(u => u.UserId == Id)
                             .GroupBy(c => c.UploadsComplainteId);
            var user = await _userService.GetByIdAsync(Id);


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
        private async Task<SelectDataCommuncationDropdownsVM> GetUserDropdownsData(ApplicationUser currentUser)
        {
            var CollegesId = currentUser.CollegesId;
            var DepartmentsId = currentUser.DepartmentsId;
            var subDepartmentsId = currentUser.SubDepartmentsId;
            var userId = currentUser.Id;

            var roles = await userManager.GetRolesAsync(currentUser);
            var rolesString = string.Join(",", roles);
            var roleId = _context.Roles.FirstOrDefault(role => role.Name == roles.FirstOrDefault())?.Id;

            return await __service.GetUserDropdownsValues(userId, subDepartmentsId, DepartmentsId, CollegesId, rolesString, roleId);
        }


    }
}
