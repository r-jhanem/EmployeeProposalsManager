            using CompalintsSystem.Core.Interfaces;
            using CompalintsSystem.Core.Models;
            using CompalintsSystem.Core.ViewModels;
            using CompalintsSystem.Core.ViewModels.Data;
            using CompalintsSystem.EF.DataBase;
            using Microsoft.AspNetCore.Authorization;
            using Microsoft.AspNetCore.Hosting;
            using Microsoft.AspNetCore.Http;
            using Microsoft.AspNetCore.Identity;
            using Microsoft.AspNetCore.Mvc;
            using Microsoft.AspNetCore.Mvc.Rendering;
            using Microsoft.EntityFrameworkCore;
            using Rotativa.AspNetCore;
            using Rotativa.AspNetCore.Options;
            using System;
            using System.Collections.Generic;
            using System.Data;
            using System.IO;
            using System.Linq;
            using System.Security.Claims;
            using System.Threading.Tasks;

            namespace CompalintsSystem.Application.Controllers
            {
                [Authorize(Roles = "AdminSubDepartments")]
                public class SubManageComplaintsController : Controller
                {
                    private readonly ICompalintRepository __service;
                    private readonly IUnitOfWork _unitOfWork;
                    private readonly ICompalintRepository _compReop;
                    private readonly IUserService _userService;
                    private readonly UserManager<ApplicationUser> _userManager;
                    private readonly IWebHostEnvironment _env;
                    private readonly ICategoryService _service;
                    private readonly AppCompalintsContextDB _context;
                    private readonly ICompalintRepository compalintRepository;
                    private readonly UserManager<ApplicationUser> userManager;
                    private readonly ICategoryService categoryService;
        private static int ID_Person = 0, id_styit = 0, st_login_empleyy_id = 0, IDesi =0, IDesi2 = 0;

        public SubManageComplaintsController(
                                    IUnitOfWork unitOfWork,
                                     ICompalintRepository __service,
                        ICategoryService service,
                        ICompalintRepository compReop,
                        IUserService userService,
                        UserManager<ApplicationUser> userManager,
                         ICompalintRepository compalintRepository,

                        ICategoryService categoryService,
                        IWebHostEnvironment env,

                        AppCompalintsContextDB context)
                    {
                        _unitOfWork = unitOfWork;
                        _compReop = compReop;
                        _userService = userService;
                        _userManager = userManager;
                        this.__service = __service;
                        _service = service;

                        _context = context;
                        _env = env;
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

                    //------------- عرض الشكاوى المحلولة والمرفوضه والمرفوعه--------------------
                    public async Task<IActionResult> Index()
                    {

                        var currentUser = await _userManager.GetUserAsync(User);
                        //var Gov = currentUser?.Collegess.Id;
                        var college = currentUser.CollegesId;
                        var department = currentUser.DepartmentsId;


                        //var Gov = currentUser?.Collegess.Id;


                        var allCompalintsVewi = _compReop.GetAll().Where(x => x.CollegesId == college && x.StatusCompalintId ==1).ToList();


                        var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();
                        ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
                        ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");
                        ViewBag.Status = ViewBag.StatusCompalints;
                        int totalCompalints = allCompalintsVewi.Count();

                        ViewBag.totalCompalints = totalCompalints;

                        return View(allCompalintsVewi);
                    }
        //public async Task<IActionResult> Index()
        //{

        //    var currentUser = await _userManager.GetUserAsync(User);
        //    //var Gov = currentUser?.Collegess.Id;


        //    var allCompalintsVewi = _compReop.GetAll().Where(g => g.SubDepartmentsId == currentUser.SubDepartmentsId && g.StagesComplaintId == 1).ToList();
        //    var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();
        //    ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
        //    ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");
        //    ViewBag.Status = ViewBag.StatusCompalints;
        //    int totalCompalints = allCompalintsVewi.Count();

        //    ViewBag.totalCompalints = totalCompalints;

        //    return View(allCompalintsVewi);
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

        //    if (AllRejectedComplaints != null)
        //    {
        //        var Getrejected = AllRejectedComplaints.Where(
        //            g => g.ComplaintsRejecteds != null && g.ComplaintsRejecteds.Any(rh => rh.UserId == userId)
        //            && g.SubDepartments != null && g.SubDepartments.Id == currentUser.SubDepartmentsId
        //            //&& g.StatusCompalint != null && g.StatusCompalint.Id == 4
        //            //&& g.StagesComplaint != null && g.StagesComplaint.Id == 3
        //            );


        //        int totalCompalintsRejected = Getrejected.Count();
        //        ViewBag.TotalCompalintsRejected = totalCompalintsRejected;

        //        return View(Getrejected);
        //    }
        //    var emptyList = Enumerable.Empty<UploadsComplainte>(); // إنشاء قائمة فارغة من الشكاوى
        //    return View(emptyList); // إرجاع قائمة فارغة في حالة عدم وجود شكاوى مرفوضة

        //}
        [HttpPost]
        public async Task<IActionResult> GetDirectorateies(int userRoles)
        {
            List<ApplicationUser> directorate = new List<ApplicationUser>();
            if(userRoles==3)
            {
                directorate = await _context.Users.Where(m => m.RoleId == userRoles && m.CollegesId == IDesi&&m.DepartmentsId== IDesi2).ToListAsync();
                // directorate = await _context.Users.Where(m => m.RoleId == userRoles).ToListAsync();

                return Json(new SelectList(directorate, "Id", "FullName"));

            }

            // directorate = await _context.Users.Where(m => m.GovernorateId == 1 && m.RoleId== userRoles).ToListAsync();
            directorate = await _context.Users.Where(m => m.RoleId == userRoles && m.CollegesId == IDesi).ToListAsync();
            // directorate = await _context.Users.Where(m => m.RoleId == userRoles).ToListAsync();

            return Json(new SelectList(directorate, "Id", "FullName"));


        }

        public async Task<IActionResult> AllRejectedComplaints()
                    {
                        var currentUser = await _userManager.GetUserAsync(User);

                        var AllRejectedComplaints = await _compReop.GetAllAsync(
                            g => g.Colleges,
                            d => d.Departments,
                            s => s.SubDepartments,
                            n => n.StatusCompalint,
                            st => st.StagesComplaint);
                        if (AllRejectedComplaints != null)
                        {
                            var Getrejected = AllRejectedComplaints.Where(g => g.StatusCompalint.Id == 3 && g.StagesComplaint.Id == 4);
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
                        UpComplaintVM UpView = new UpComplaintVM()
                        {
                            UploadsComplainteId = id,

                        };
                        ProvideSolutionsVM VM = new ProvideSolutionsVM
                        {
                            compalint = ComplantList,
                            Compalints_SolutionList = await _context.Compalints_Solutions.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                            ComplaintsRejectedList = await _context.ComplaintsRejecteds.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                            RejectedComplaintVM = rejectView,
                            AddSolution = addsoiationView,
                            UpComplaintCauseList = await _context.UpComplaintCauses.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                            UpComplaint = UpView,
                            ToAnotherUser = toAnotherUser,
                        };
                        return View(VM);
                    }

                    [HttpGet]
                    [AllowAnonymous]
                    public async Task<IActionResult> Create()
                    {
                        var currentUser = await userManager.GetUserAsync(User);
                        var currentName = currentUser.FullName;
                        var compalintDropdownsData = await categoryService.GetNewCompalintsDropdownsValues();
                        ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");
                        ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");

                        return View();
                    }

        //[HttpPost]
        //public async Task<IActionResult> Create(InputCompmallintVM model)
        //{

        //    var currentUser = await userManager.GetUserAsync(User);
        //    var claimsIdentity = (ClaimsIdentity)User.Identity;
        //    var role = claimsIdentity.FindFirst(ClaimTypes.Role); // استخدام الكائن claimsIdentity بدلاً من ClaimsIdentity
        //    string userRole = role.Value;

        //    if (!ModelState.IsValid)
        //    {
        //        var Identity = currentUser.IdentityNumber;

        //        var compalintDropdownsData = await categoryService.GetNewCompalintsDropdownsValues();
        //        ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");
        //        ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");

        //        var newName = Guid.NewGuid().ToString(); //rre-rewrwerwer-gwgrg-grgr
        //        var extension = Path.GetExtension(model.File?.FileName);
        //        var fileName = string.Concat(newName, extension); // newName + extension
        //        var root = _env.WebRootPath;
        //        var path = Path.Combine(root, "Uploads", fileName);

        //        using (var fs = System.IO.File.Create(path))
        //        {
        //            await model.File?.CopyToAsync(fs);
        //        }


        //        //await _service.CreateAsync(data);
        //        await __service.CreateAsync(new InputCompmallintVM
        //        {
        //            TitleComplaint = model.TitleComplaint,
        //            TypeComplaintId = model.TypeComplaintId,
        //            DescComplaint = model.DescComplaint,
        //            PropBeneficiarie = model.PropBeneficiarie,
        //            CollegesId = currentUser.CollegesId,
        //            DepartmentsId = currentUser.DepartmentsId,
        //            SubDepartmentsId = currentUser.SubDepartmentsId,
        //            UserId = Identity,
        //            UserRoleName = userRole,
        //            StagesComplaintId = model.StagesComplaintId = 4,// هذه المرحلة الخاصة بالشكوى كل رقم يبين مرحلة معينة عام ووو
        //            OriginalFileName = model.File.FileName,
        //            FileName = fileName,
        //            ContentType = model.File.ContentType,
        //            Size = model.File.Length,

        //        });

        //        return RedirectToAction(nameof(Index1));
        //    }
        //    return View(model);
        //}

        [HttpPost]
        public async Task<IActionResult> Create(InputCompmallintVM model)
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

                if (model.File != null)
                {
                    var newName = Guid.NewGuid().ToString(); //rre-rewrwerwer-gwgrg-grgr
                    var extension = Path.GetExtension(model.File.FileName);
                    var fileName = string.Concat(newName, extension); // newName + extension
                    var root = _env.WebRootPath;
                    var path = Path.Combine(root, "Uploads", fileName);

                    using (var fs = System.IO.File.Create(path))
                    {
                        await model.File.CopyToAsync(fs);
                    }

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
                }
                else
                {
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
                        StagesComplaintId = 4, // هذه المرحلة الخاصة بالشكوى كل رقم يبين مرحلة معينة عام ووو
                    });
                }

                return RedirectToAction(nameof(Index1));
            }

            return View(model);
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

                    public async Task<IActionResult> ViewAllRejectedComplaints()
                    {
                        var currentUser = await userManager.GetUserAsync(User);
                        var Identity = currentUser.IdentityNumber;
                        var compalintDropdownsData = await categoryService.GetNewCompalintsDropdownsValues();
                        ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
                        ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

                        ViewBag.status = ViewBag.StatusCompalints;

                        var AllRejectedComplaints = __service.GetAllRejectedComplaints(Identity);

                        return View(AllRejectedComplaints.ToList());

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
                        UpComplaintVM UpView = new UpComplaintVM()
                        {
                            UploadsComplainteId = id,

                        };
                        ProvideSolutionsVM VM = new ProvideSolutionsVM
                        {
                            compalint = ComplantList,
                            Compalints_SolutionList = await _context.Compalints_Solutions.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                            ComplaintsRejectedList = await _context.ComplaintsRejecteds.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                            RejectedComplaintVM = rejectView,
                            AddSolution = addsoiationView,
                            UpComplaintCauseList = await _context.UpComplaintCauses.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                            UpComplaint = UpView,
                            ToAnotherUser = toAnotherUser,
                        };
                        return View(VM);
                    }


                    public async Task<IActionResult> ViewCompalintRejectedDetails(int id)
                    {
                        var ComplantList = await _compReop.FindAsync(id);
                        var currentUser = await GetCurrentUser();
                        var userDropdownsData = await GetUserDropdownsData(currentUser);
                        ViewBag.UsersName = new SelectList(userDropdownsData.ApplicationUsers, "Id", "FullName");
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
                            Compalints_SolutionList = await _context.Compalints_Solutions.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                            ComplaintsRejectedList = await _context.ComplaintsRejecteds.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                            RejectedComplaintVM = rejectView,
                            AddSolution = addsoiationView,
                            ToAnotherUser = toAnotherUser,
                        };
                        return View(VM);
                    }

                    //public async Task<IActionResult> AllUpComplaints()
                    //{
                    //    var currentUser = await _userManager.GetUserAsync(User);

                    //    var AllComplaintsUp = await _compReop.GetAllAsync(
                    //        g => g.Colleges,
                    //        d => d.Departments,
                    //        s => s.SubDepartments,
                    //        n => n.StatusCompalint,
                    //        st => st.StagesComplaint);
                    //    if (AllComplaintsUp != null)
                    //    {
                    //        var Getrejected = AllComplaintsUp.Where(g => g.StatusCompalint.Id == 4 && g.StagesComplaint.Id == 4);
                    //        var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();

                    //        ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
                    //        ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

                    //        ViewBag.status = ViewBag.StatusCompalints;
                    //        int totalCompalints = Getrejected.Count();

                    //        ViewBag.totalCompalints = totalCompalints;

                    //        return View(Getrejected);

                    //    }

                    //    var emptyList = Enumerable.Empty<UploadsComplainte>(); // إنشاء قائمة فارغة من الشكاوى
                    //    return View(emptyList); // إرجاع قائمة فارغة في حالة عدم وجود شكاوى مرفوضة

                    //}



                    public async Task<IActionResult> AllUpComplaints()
                    {
                        var currentUser = await _userManager.GetUserAsync(User);
                        var userId = currentUser.UserId;

                        var AllComplaintsUp = await _compReop.GetAllAsync(
                            g => g.Colleges,
                            d => d.Departments,
                            s => s.SubDepartments,
                            n => n.StatusCompalint,
                            st => st.StagesComplaint,
                            up => up.ComplaintsUp);

                        if (AllComplaintsUp != null)
                        {
                            var Getrejected = AllComplaintsUp.Where(g => g.CollegesId == currentUser.CollegesId 
                            && g.StatusCompalintId ==5 /*== 5 && g.UserId == userId*/
                
                            );// في القسم لانة عند الرفع يتم نقلها الى القسم
                            var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();
                            ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
                            ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

                            ViewBag.status = ViewBag.StatusCompalints;
                
                            int totalCompalintsUp = Getrejected.Count();
                            ViewBag.totalCompalintsUp = totalCompalintsUp;

                            return View(Getrejected);
                        }
                        var emptyList = Enumerable.Empty<UpComplaintCause>(); // إنشاء قائمة فارغة من الشكاوى
                        return View(emptyList); // إرجاع قائمة فارغة في حالة عدم وجود شكاوى 
                    }





                    public async Task<IActionResult> ViewCompalintUpDetails(int id)
                    {
                        var ComplantList = await _compReop.FindAsync(id);

                        var addsoiationView = new AddSolutionVM
                        {
                            UploadsComplainteId = id
                        };
                        TransferComplaintToAnotherUser toAnotherUser = new TransferComplaintToAnotherUser()
                        {
                            ConplaintId = id,

                        };
                        var rejectView = new ComplaintsRejectedVM
                        {
                            UploadsComplainteId = id
                        };

                        var compalints_SolutionList = await _context.Compalints_Solutions
                            .Where(a => a.UploadsComplainteId == id)
                            .ToListAsync();

                        var complaintsRejectedList = await _context.ComplaintsRejecteds
                            .Where(a => a.UploadsComplainteId == id)
                            .ToListAsync();

                        var upComplaintCauseList = await _context.UpComplaintCauses
                            .Where(a => a.UploadsComplainteId == id)
                            .ToListAsync();

                        var VM = new ProvideSolutionsVM
                        {
                            compalint = ComplantList,
                            Compalints_SolutionList = compalints_SolutionList,
                            ComplaintsRejectedList = complaintsRejectedList,
                            UpComplaintCauseList = upComplaintCauseList,
                            RejectedComplaintVM = rejectView,
                            AddSolution = addsoiationView,
                            ToAnotherUser = toAnotherUser,
                        };

                        return View(VM);
                    }
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
                                 && g.SubDepartments != null && g.SubDepartments.Id == currentUser.SubDepartmentsId
                                //&& g.SubDepartments != null && g.SubDepartments.Id == currentUser.SubDepartmentsId
                                //&& g.StatusCompalint != null && g.StatusCompalint.Id == 2
                                );
                            var compalintDropdownsData = await _service.GetNewCompalintsDropdownsValues();

                            ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
                            ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

                            ViewBag.status = ViewBag.StatusCompalints;
               

               
                            int totalCompalintsSolution = Getrejected.Count();
                            ViewBag.TotalCompalintsSolution = totalCompalintsSolution;

                            return View(Getrejected);
                        }
                        var emptyList = Enumerable.Empty<UploadsComplainte>(); // إنشاء قائمة فارغة من الشكاوى
                        return View(emptyList); // إرجاع قائمة فارغة في حالة عدم وجود شكاوى مرفوضة
                    }

                    public async Task<IActionResult> RejectedThisComplaint(int id, UploadsComplainte complainte)
                    {

                        var upComp = await _compReop.FindAsync(id);
                        var dbComp = await _context.UploadsComplaintes.FirstOrDefaultAsync(n => n.Id == upComp.Id);
                        if (dbComp != null)
                        {
                            dbComp.Id = complainte.Id;
                            dbComp.StatusCompalintId = 3;
                            dbComp.StagesComplaintId = 4;

                            await _context.SaveChangesAsync();
                        }

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(AllRejectedComplaints));

                    }
                    //-------------نــــهـــــــايـــة عرض الشكاوى المحلولة والمرفوضه والمرفوعه --------------------

                    // -----------------عرض المستخدمين --------------------------------------------------------------

                    //public async Task<IActionResult> ViewUsers()
                    //{
                    //    var currentUser = await _userManager.GetUserAsync(User);
                    //    var currentIdUser = currentUser.IdentityNumber;

                    //    var sub = currentUser.SubDepartmentsId;


                    //    var result = await _userService.GetAllAsync(curre,sub);


                    //    int totalUsers = result.Count();

                    //    ViewBag.totalUsers = totalUsers;


                    //    //return View(await PaginatedList<ApplicationUser>.CreateAsync(result.AsNoTracking(), pageNumber ?? 1, pageSize));
                    //    return View(result.ToList());

                    //}
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
                    //    var result = await _context.Users.Where(r => r.RoleId == 5 && r.SubDepartmentsId == currentUser.SubDepartmentsId)
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





        // GET: Users/Create
        public async Task<IActionResult> Create2()
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
                    public async Task<IActionResult> Create2(AddUserViewModel model)
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

                            return RedirectToAction("ViewUsers");
                        }
                        return View(model);
                    }
                    // GET: Users/Details/5
                    public async Task<IActionResult> Details1(string id)
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

        

        

       







                    private void UploadImage(AddUserViewModel model)
                    {

                        var file = HttpContext.Request.Form.Files;
                        if (file.Count() > 0)
                        {  //@"wwwroot/"
                            string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                            var filestream = new FileStream(Path.Combine(_env.WebRootPath, "Images", ImageName), FileMode.Create);
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
                    private void UploadImage2(EditUserViewModel model)
                    {

                        var file = HttpContext.Request.Form.Files;
                        if (file.Count() > 0)
                        {  //@"wwwroot/"
                            string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                            var filestream = new FileStream(Path.Combine(_env.WebRootPath, "Images", ImageName), FileMode.Create);
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


                    public async Task<IActionResult> Edit(string id)
                    {
                        if (id == null)
                        {
                            return NotFound();
                            //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        var user = await _unitOfWork.User.GetByIdAsync(id);

                        if (user == null)
                        {
                            return NotFound();
                        }

                        List<Colleges> CollegesList = new List<Colleges>();
                        CollegesList = (from d in _context.Collegess select d).ToList();
                        //CollegesList.Insert(0, new Colleges { Id = 0, Name = "حدد الكلية" });
                        ViewBag.ViewGover = CollegesList;
                        var newUser = new EditUserViewModel
                        {
                            //CollegessList = await _context.Collegess.ToListAsync(),

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

                        //ViewBag.ViewGover = newUser.CollegessList.ToArray();
                        return View(newUser);
                    }
                    private bool UserExists(string id)
                    {
                        return string.IsNullOrEmpty(_unitOfWork.User.GetByIdAsync(id).ToString());
                    }


                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult> Edit(string id, EditUserViewModel user)
                    {
                        //var users = await _unitOfWork.User.GetAllAsync();
                        //ViewBag.UserCount = users.Count();
                        UploadImage2(user);
                        if (ModelState.IsValid)
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
                                    throw;
                                }
                            }
                        }
                        return View(user);
                    }

                    public async Task<IActionResult> AccountRestriction()
                    {
                        var currentUser = await _userManager.GetUserAsync(User);
                        var currentIdUser = currentUser.IdentityNumber;
                        var result = _userService.GetAllUserBlockedAsync(currentIdUser);



                        return View(result.ToList());

                    }

                    // ----------------- نــــهـــــــايـــة عرض المستخدمين ---------------------------------------



                    // ----------------------- إدارة الشكوى---------------------------------------------------------
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    // ----------------------تقديم حل للشكوى--------------------------------------------------------
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
                            return RedirectToAction(nameof(SolutionComplaints));

                        }

                        return NotFound();


                    }

                    // رفع الشكوى مع سبب الرفع للإدارة العلياء 
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
                            return RedirectToAction(nameof(AllRejectedComplaints));

                        }

                        return NotFound();


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



        //private async Task<SelectDataCommuncationDropdownsVM> GetCommunicationDropdownsData2()
        //{
        //    return await __service.GetAddCommunicationDropdownsValues2();
        //}

        //public async Task<IActionResult> ShowCommunication(int id)
        //{
        //    if (id == 0)
        //        return View(new CommunicationVM());
        //    else
        //    {
        //        var usersCommunicationsModel = await _context.UsersCommunications.Where(i => i.Id == id).FirstOrDefaultAsync();
        //        if (usersCommunicationsModel == null)
        //        {
        //            return NotFound();
        //        }

        //        return PartialView("_ShowCommunication", usersCommunicationsModel);
        //    }

        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> AddCommunication()
        //{
        //    var currentUser = await GetCurrentUser();
        //    var communicationDropdownsData = await GetCommunicationDropdownsData(currentUser);

        //    ViewBag.typeCommun = new SelectList(communicationDropdownsData.TypeCommunications, "Id", "Type");
        //    ViewBag.UsersName = new SelectList(communicationDropdownsData.ApplicationUsers, "Id", "FullName");

        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddCommunication(AddCommunicationVM communication)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var currentUser = await GetCurrentUser();
        //        var communicationDropdownsData = await GetCommunicationDropdownsData(currentUser);
        //        ViewBag.typeCommun = new SelectList(communicationDropdownsData.TypeCommunications, "Id", "Type");
        //        ViewBag.UsersName = new SelectList(communicationDropdownsData.ApplicationUsers, "Id", "Name");

        //        var currentName = currentUser.FullName; //مقدم البلاغ
        //        var currentPhone = currentUser.PhoneNumber;
        //        var currentGov = currentUser.CollegesId;
        //        var currentDir = currentUser.DepartmentsId;
        //        var currentSub = currentUser.SubDepartmentsId;

        //        await __service.CreateCommuncationAsync(new AddCommunicationVM
        //        {
        //            Titile = communication.Titile,
        //            reason = communication.reason,
        //            CreateDate = communication.CreateDate,
        //            TypeCommuncationId = communication.TypeCommuncationId,
        //            reportSubmitterId = currentUser.Id,
        //            reportSubmitterName = currentName,
        //            BenfPhoneNumber = currentPhone,
        //            CollegesId = currentGov,
        //            DepartmentsId = currentDir,
        //            SubDepartmentsId = currentSub,
        //        });

        //        return RedirectToAction("AllCommunication");
        //    }
        //    return View(communication);
        //}

        //private async Task<ApplicationUser> GetCurrentUser()
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    var userId = currentUser.Id.ToString();
        //    var user = await _userManager.FindByIdAsync(userId);
        //    return user;
        //}

        //private async Task<SelectDataCommuncationDropdownsVM> GetCommunicationDropdownsData(ApplicationUser currentUser)
        //{
        //    var CollegesId = currentUser.CollegesId;
        //    var DepartmentsId = currentUser.DepartmentsId;
        //    var subDepartmentsId = currentUser.SubDepartmentsId;
        //    //var roles = await _userManager.GetRolesAsync(currentUser);

        //    var roles = await _userManager.GetRolesAsync(currentUser);
        //    var rolesString = string.Join(",", roles);
        //    var roleId = _context.Roles.FirstOrDefault(role => role.Name == roles.FirstOrDefault())?.Id;

        //    return await _compReop.GetAddCommunicationDropdownsValues(subDepartmentsId, DepartmentsId, CollegesId, rolesString, roleId);
        //}

        // كل البلاغات المقدمة
        public IActionResult ClsComModleView(int? Id)
        {


            ViewData["eaaad_id"] = st_login_empleyy_id;

            ViewData["ClsComplaintId"] = Id.ToString();


            var clo = _context.UploadsComplaintes.Where(x => x.Id == Id).FirstOrDefault();

            int g = clo.CollegesId;
            int g_2 = clo.DepartmentsId;

            IDesi = g;
            IDesi2 = g_2;
            var u = _context.Users.Where(x => x.CollegesId == g);






            ViewData["ClsCompltintClssictionId"] = new SelectList(_context.ComplatinClassfactions, "Id", "Name");
            // IDesi = g_2;


            // var bookstoreDbContext = _context.clsComplaints.Include(c => c.ClsStudent);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClsComModleView(int? Id, ClsComModelView clsCom)

        {

            //UploadsComplainte clsResponse = new UploadsComplainte
            //{
            //    //  ClsComplaint =ff,
            //    //ClsEmplloyOfficer =f,

            //   UploadDate = DateTime.Now,
            //    UserId = clsCom.ClsEmplloyOfficerId,
            //    ComplatinClassfactionId = clsCom.ClsCompltintClssictionId,

            //};

            //_context.UploadsComplaintes.Update(clsResponse);
            //_context.SaveChanges();



            var complaint = _context.UploadsComplaintes.Find(clsCom.ClsComplaintId);

            if (clsCom.ClsTypeEmplloyId == 2)

            {
                complaint.StagesComplaintId = 3;

            }
            else
            {
                complaint.StagesComplaintId = 2;

            }


            complaint.StatusCompalintId = 5;
            complaint.ComplatinClassfactionId = clsCom.ClsCompltintClssictionId;

            _context.SaveChanges();

            return RedirectToAction(nameof(AllUpComplaints));



            return View();
        }




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




        public async Task<IActionResult> TransferComplaintToAnotherUser(ProvideSolutionsVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // استرجاع الشكوى التي تحتاج إلى النقل
                    var transfer = await _context.UploadsComplaintes.FirstOrDefaultAsync(n => n.Id == model.ToAnotherUser.ConplaintId && n.ReturnedTo == model.ToAnotherUser.UserId);

                    if (transfer != null)
                    {
                        // تحديد المستخدم الجديد الذي سيتم نقل الشكوى إليه
                        transfer.ReturnedTo = model.ToAnotherUser.UserId;

                        // تحديث الشكوى في قاعدة البيانات
                        _context.UploadsComplaintes.Update(transfer);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("ComplaintsForMe", "CompalintsMang");
                    }
                    else
                    {
                        // رسالة خطأ إذا لم يتم العثور على الشكوى
                        ModelState.AddModelError("", "لم يتم العثور على الشكوى.");
                    }
                }
                catch (Exception ex)
                {
                    // يمكنك إضافة إجراءات إضافية هنا إذا كنت تحتاج إلى معالجة الأخطاء
                    ModelState.AddModelError("", "حدث خطأ أثناء معالجة الطلب.");
                }
            }

            // إذا كان هناك خطأ في النموذج أو لم يتم العثور على الشكوى، يتم إعادة توجيه المستخدم إلى صفحة ComplaintsForMe
            return RedirectToAction("ComplaintsForMe", "CompalintsMang");
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

                            return View(Getrejected);
                        }
                        var emptyList = Enumerable.Empty<UpComplaintCause>(); // إنشاء قائمة فارغة من الشكاوى
                        return View(emptyList); // إرجاع قائمة فارغة في حالة عدم وجود شكاوى 
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


                    public IActionResult AllCirculars()
                    {
                        return View();
                    }

                    [HttpGet]
                    public async Task<IActionResult> Download(int id)
                    {
                        var selectedFile = await _compReop.FindAsync(id);
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

                    //public async Task<IActionResult> DisbleOrEnableUser(int id)
                    //{
                    //    await _userService.TogelBlockUserAsync(id);
                    //    return RedirectToAction("ViewUsers");


                    //}


                }
            }


