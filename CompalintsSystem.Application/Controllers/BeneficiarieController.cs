using AutoMapper;
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
using Microsoft.CodeAnalysis;
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

    [Authorize(Policy = "BeneficiariePolicy")]
    public class BeneficiarieController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ICompalintRepository _compReop;
        private readonly ICompalintRepository _service;
        private readonly IUserService userService;
        private readonly ICompalintRepository compalintRepository;
        private readonly IWebHostEnvironment _env;
        private readonly ICategoryService categoryService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AppCompalintsContextDB _context;
        private readonly IMapper mapper;

        public BeneficiarieController(

            ICompalintRepository service,
             ICompalintRepository compReop,
            IUserService userService,
            ICompalintRepository compalintRepository,
            IWebHostEnvironment env,
            ICategoryService categoryService,
              UserManager<ApplicationUser> userManager,
              SignInManager<ApplicationUser> signInManager,
            AppCompalintsContextDB context,
             IMapper mapper
            )
        {

            _compReop = compReop;
            _service = service;
            this.userService = userService;
            this.compalintRepository = compalintRepository;
            _context = context;
            this.mapper = mapper;
            _env = env;
            this.categoryService = categoryService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        private string UserId
        {
            get
            {
                //var IdentityUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //IdentityUser.
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }




        public async Task<IActionResult> Index()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var Identity = currentUser.IdentityNumber;
            var compalintDropdownsData = await categoryService.GetNewCompalintsDropdownsValues();

            ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
            ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");


            var result = _service.GetBenfeficarieCompalintBy(Identity);

            int totalCompalints = result.Count();

            ViewBag.totalCompalints = totalCompalints;

            return View(result.ToList());

        }
        public async Task<IActionResult> ViewCompalintRejectedDetails(int id)
        {
            var ComplantList = await _service.FindAsync(id);
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

        public async Task<IActionResult> ViewResolvedComplaints()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var Identity = currentUser.IdentityNumber;
            var compalintDropdownsData = await categoryService.GetNewCompalintsDropdownsValues();
            ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
            ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

            ViewBag.status = ViewBag.StatusCompalints;
            var AllRejectedComplaints = _service.GetAllResolvedComplaints(Identity);
            var result = _service.GetBenfeficarieCompalintBy(Identity);

            int totalCompalints = AllRejectedComplaints.Count();

            ViewBag.totalCompalints = totalCompalints;
            return View(AllRejectedComplaints.ToList());

        }

        public async Task<IActionResult> ViewAllRejectedComplaints()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var Identity = currentUser.IdentityNumber;
            var compalintDropdownsData = await categoryService.GetNewCompalintsDropdownsValues();
            ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");
            ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");

            ViewBag.status = ViewBag.StatusCompalints;

            var AllRejectedComplaints = _service.GetAllRejectedComplaints(Identity);
            var result = _service.GetBenfeficarieCompalintBy(Identity);

            int totalCompalints = AllRejectedComplaints.Count();

            ViewBag.totalCompalints = totalCompalints;
            return View(AllRejectedComplaints.ToList());

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

        [HttpPost]
        public async Task<IActionResult> Create(InputCompmallintVM model)
        {
            var currentUser = await userManager.GetUserAsync(User);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var role = claimsIdentity.FindFirst(ClaimTypes.Role); // استخدام الكائن claimsIdentity بدلاً من ClaimsIdentity
            string userRole = role.Value;

            string Today = DateTime.Now.ToString("MM/dd/yyyy");

            bool canSubmitComplaint = _service.CanSubmitComplaintForUserToday(currentUser.IdentityNumber, Today);

            if (canSubmitComplaint)
            {
                ModelState.AddModelError(string.Empty, "لا يمكن تقديم أكثر من شكوى واحدة في اليوم.");
                return View(model);
            }
            else if (!ModelState.IsValid)
            {
                var Identity = currentUser.IdentityNumber;

                var compalintDropdownsData = await categoryService.GetNewCompalintsDropdownsValues();
                ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");
                ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");

                var newName = Guid.NewGuid().ToString(); //rre-rewrwerwer-gwgrg-grgr

                string fileName = null;
                string contentType = null;
                long size = 0;

                if (model.File != null)
                {
                    var extension = Path.GetExtension(model.File.FileName);
                    fileName = string.Concat(newName, extension); // newName + extension
                    var root = _env.WebRootPath;
                    var path = Path.Combine(root, "Uploads", fileName);

                    using (var fs = System.IO.File.Create(path))
                    {
                        await model.File.CopyToAsync(fs);
                    }

                    contentType = model.File.ContentType;
                    size = model.File.Length;
                }

                await _service.CreateAsync(new InputCompmallintVM
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
                    StagesComplaintId = 1, // هذه المرحلة الخاصة بالشكوى كل رقم يبين مرحلة معينة عام ووو
                    OriginalFileName = model.File?.FileName,
                    FileName = fileName,
                    ContentType = contentType,
                    Size = size,
                    Today = Today
                });

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(InputCompmallintVM model)
        //{

        //    var currentUser = await userManager.GetUserAsync(User);
        //    var claimsIdentity = (ClaimsIdentity)User.Identity;
        //    var role = claimsIdentity.FindFirst(ClaimTypes.Role); // استخدام الكائن claimsIdentity بدلاً من ClaimsIdentity
        //    string userRole = role.Value;

        //    string Today = DateTime.Now.ToString("MM/dd/yyyy");

        //    bool canSubmitComplaint = _service.CanSubmitComplaintForUserToday(currentUser.IdentityNumber, Today);

        //    if (canSubmitComplaint)
        //    {
        //        ModelState.AddModelError(string.Empty, "لا يمكن تقديم أكثر من شكوى واحدة في اليوم.");
        //        return View(model);
        //    }
        //    else if (!ModelState.IsValid)
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
        //        await _service.CreateAsync(new InputCompmallintVM
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
        //            StagesComplaintId = model.StagesComplaintId = 1,// هذه المرحلة الخاصة بالشكوى كل رقم يبين مرحلة معينة عام ووو
        //            OriginalFileName = model.File.FileName,
        //            FileName = fileName,
        //            ContentType = model.File.ContentType,
        //            Size = model.File.Length,
        //            Today = Today,

        //        });

        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(InputCompmallintVM model)
        //{
        //    var currentUser = await userManager.GetUserAsync(User);
        //    var claimsIdentity = (ClaimsIdentity)User.Identity;
        //    var role = claimsIdentity.FindFirst(ClaimTypes.Role); // استخدام الكائن claimsIdentity بدلاً من ClaimsIdentity
        //    string userRole = role.Value;

        //    string Today = DateTime.Now.ToString("MM/dd/yyyy");

        //    bool canSubmitComplaint = _service.CanSubmitComplaintForUserToday(currentUser.IdentityNumber, Today);

        //    if (canSubmitComplaint)
        //    {
        //        ModelState.AddModelError(string.Empty, "لا يمكن تقديم أكثر من شكوى واحدة في اليوم.");
        //        return View(model);
        //    }
        //    else if (!ModelState.IsValid)
        //    {
        //        var Identity = currentUser.IdentityNumber;

        //        var compalintDropdownsData = await categoryService.GetNewCompalintsDropdownsValues();
        //        ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");
        //        ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");

        //        //await _service.CreateAsync(data);
        //        await _service.CreateAsync(new InputCompmallintVM
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
        //            StagesComplaintId = model.StagesComplaintId = 1, // هذه المرحلة الخاصة بالشكوى كل رقم يبين مرحلة معينة عام ووو
        //            Today = Today,
        //        });

        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(model);
        //}
        //public async Task<IActionResult> Create(InputCompmallintVM model)
        //{
        //    var currentUser = await userManager.GetUserAsync(User);
        //    var claimsIdentity = (ClaimsIdentity)User.Identity;
        //    var role = claimsIdentity.FindFirst(ClaimTypes.Role);
        //    string userRole = role.Value;

        //    string Today = DateTime.Now.ToString("MM/dd/yyyy");

        //    bool canSubmitComplaint = _service.CanSubmitComplaintForUserToday(currentUser.IdentityNumber, Today);

        //    if (canSubmitComplaint)
        //    {
        //        ModelState.AddModelError(string.Empty, "لا يمكن تقديم أكثر من شكوى واحدة في اليوم.");
        //        return View(model);
        //    }
        //    else if (ModelState.IsValid)
        //    {
        //        var Identity = currentUser.IdentityNumber;

        //        var compalintDropdownsData = await categoryService.GetNewCompalintsDropdownsValues();
        //        ViewBag.TypeComplaints = new SelectList(compalintDropdownsData.TypeComplaints, "Id", "Type");
        //        ViewBag.StatusCompalints = new SelectList(compalintDropdownsData.StatusCompalints, "Id", "Name");

        //        if (model.File != null)
        //        {
        //            var newName = Guid.NewGuid().ToString();
        //            var extension = Path.GetExtension(model.File.FileName);
        //            var fileName = string.Concat(newName, extension);
        //            var root = _env.WebRootPath;
        //            var path = Path.Combine(root, "Uploads", fileName);

        //            using (var fs = System.IO.File.Create(path))
        //            {
        //                await model.File.CopyToAsync(fs);
        //            }

        //            await _service.CreateAsync(new InputCompmallintVM
        //            {
        //                TitleComplaint = model.TitleComplaint,
        //                TypeComplaintId = model.TypeComplaintId,
        //                DescComplaint = model.DescComplaint,
        //                PropBeneficiarie = model.PropBeneficiarie,
        //                CollegesId = currentUser.CollegesId,
        //                DepartmentsId = currentUser.DepartmentsId,
        //                SubDepartmentsId = currentUser.SubDepartmentsId,
        //                UserId = Identity,
        //                UserRoleName = userRole,
        //                StagesComplaintId = model.StagesComplaintId = 1,
        //                OriginalFileName = model.File.FileName,
        //                FileName = fileName,
        //                ContentType = model.File.ContentType,
        //                Size = model.File.Length,
        //                Today = Today,
        //            });
        //        }
        //        else
        //        {
        //            // If the file is not provided, create the complaint without file details
        //            await _service.CreateAsync(new InputCompmallintVM
        //            {
        //                TitleComplaint = model.TitleComplaint,
        //                TypeComplaintId = model.TypeComplaintId,
        //                DescComplaint = model.DescComplaint,
        //                PropBeneficiarie = model.PropBeneficiarie,
        //                CollegesId = currentUser.CollegesId,
        //                DepartmentsId = currentUser.DepartmentsId,
        //                SubDepartmentsId = currentUser.SubDepartmentsId,
        //                UserId = Identity,
        //                UserRoleName = userRole,
        //                StagesComplaintId = model.StagesComplaintId = 1,
        //                Today = Today,
        //            });
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(model);
        //}

        public async Task<IActionResult> BeneficiariesAccount()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var result = await _context.Users.Where(r => r.RoleId == 5 && r.DepartmentsId == currentUser.DepartmentsId)
                .Include(g => g.Colleges)
                .Include(g => g.Departments)
                .Include(g => g.SubDepartments)
                .ToListAsync();

            int totalUsers = result.Count();
            ViewBag.totalUsers = totalUsers;
            return View(result.ToList());
        }

        public async Task<IActionResult> AccountRestriction()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentIdUser = currentUser.IdentityNumber;
            var result = await _context.Users.Where(u => u.EmailConfirmed == false && u.UserId == currentUser.IdentityNumber)
                .OrderByDescending(d => d.CreatedDate)
                .Include(s => s.Colleges)
                .Include(g => g.Departments)
                .Include(d => d.SubDepartments)
                .ToListAsync();

            return View(result.ToList());

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
            return await _service.GetAddCommunicationDropdownsValues2();
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
                await _service.CreateCommuncationAsync(new AddCommunicationVM
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

            return await _service.GetAddCommunicationDropdownsValues(subDepartmentsId, DepartmentsId, CollegesId, rolesString, roleId);
        }





        public async Task<IActionResult> ViewCompalintDetails(int id)
        {
            //var compalintDetails = await _service.FindAsync(id);
            //var ComplantList = await _context.UploadsComplainte.Include(a => a.Collegess).Include(a => a.Departmentss).Include(a => a.SubDepartmentss).Include(a => a.Villages).Include(a => a.TypeComplaint).Where(m => m.Id == id).FirstOrDefaultAsync();
            var ComplantList = await _service.FindAsync(id);
            Compalints_Solution soiationView = new Compalints_Solution()
            {
                UploadsComplainteId = id,


            };
            ProvideSolutionsVM MV = new ProvideSolutionsVM
            {
                compalint = ComplantList,
                Compalints_SolutionList = await _context.Compalints_Solutions.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                CompalintsSolution = soiationView,

            };
            return View(MV);

        }

        public async Task<IActionResult> Yes(int id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
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

                return RedirectToAction(nameof(Index));

            }
        }
        //public async Task<IActionResult> No(int id, ProvideSolutionsVM model)
        //{
        //    if (id == null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    else
        //    {

        //        var solution = await _context.Compalints_Solutions
        //            .FirstOrDefaultAsync(a => a.Id == id);
        //        if (solution != null)
        //        {
        //            solution.IsAccept = false;
        //            solution.ReasonWhySolutionRejected = model.CompalintsSolution.ReasonWhySolutionRejected;
        //            _context.Compalints_Solutions.Update(solution);
        //            await _context.SaveChangesAsync();
        //        }
        //        else
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }




        //        var comp = await _context.UploadsComplaintes.FirstOrDefaultAsync(ids => ids.Id == model.AddSolution.Id);
        //        if (comp != null)
        //        {
        //            comp.StagesComplaintId = comp.StagesComplaintId + 1;
        //            comp.StagesComplaintId = 2;
        //            _context.UploadsComplaintes.Update(comp);
        //            await _context.SaveChangesAsync();

        //        }
        //        else
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //        if (solution == null)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }



        //        return RedirectToAction(nameof(Index));
        //    }
        //}


        //public async Task<IActionResult> No(ProvideSolutionsVM model)
        //{
        //    if (model.CompalintsSolution.Id < 0)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var solution = await _context.Compalints_Solutions.FirstOrDefaultAsync(a => a.Id == model.CompalintsSolution.Id);

        //    if (solution == null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    solution.IsAccept = false;
        //    solution.ReasonWhySolutionRejected = model.CompalintsSolution.ReasonWhySolutionRejected;
        //    _context.Compalints_Solutions.Update(solution);

        //    var comp = await _context.UploadsComplaintes.FirstOrDefaultAsync(ids => ids.Id == model.CompalintsSolution.UploadsComplainteId);

        //    if (comp != null)
        //    {
        //        comp.StagesComplaintId++; // زيادة القيمة بواحد باستخدام العملية المختصرة
        //        _context.UploadsComplaintes.Update(comp);
        //    }
        //    else
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}


        public async Task<IActionResult> No(ProvideSolutionsVM model)
        {
            if (model.CompalintsSolution == null || model.CompalintsSolution.Id < 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var solution = await _context.Compalints_Solutions.FirstOrDefaultAsync(a => a.Id == model.CompalintsSolution.Id);

            if (solution == null)
            {
                return RedirectToAction(nameof(Index));
            }

            solution.IsAccept = false;
            solution.ReasonWhySolutionRejected = model.CompalintsSolution.ReasonWhySolutionRejected;

            _context.Compalints_Solutions.Update(solution);

            var comp = await _context.UploadsComplaintes.FirstOrDefaultAsync(ids => ids.Id == model.CompalintsSolution.UploadsComplainteId);

            if (comp != null)
            {
                comp.StagesComplaintId++;
                _context.UploadsComplaintes.Update(comp);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            // تأكد من نجاح التحديث
            var saveChangesResult = await _context.SaveChangesAsync();

            if (saveChangesResult > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // يمكنك إتخاذ إجراءات إضافية هنا إذا لم يكن هناك تحديثات ناجحة
                return RedirectToAction(nameof(Index));
            }
        }




        [AllowAnonymous]
        public async Task<IActionResult> FilterCompalintsBySearch(string term)
        {

            var allCompalints = await _service.GetAllAsync();
            if (!string.IsNullOrEmpty(term))
            {

                var result = _context.UploadsComplaintes.Where(
                 u => u.TitleComplaint == term
                 || u.DescComplaint == term);
                return View("Index", result);
            }
            return View("Index", allCompalints);
            //return result;
        }


        [HttpGet]
        public async Task<IActionResult> Download(int id)
        {
            var selectedFile = await _service.FindAsync(id);
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


        public async Task<IEnumerable<Proposal>> GetAllAsync() => await _context.Proposals.ToListAsync();

        public async Task<IActionResult> AllProposals()
        {
            var allProposals = await GetAllAsync();

            return View(allProposals);
        }


        public IActionResult AddProposals()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProposals([Bind("TitileProposal,DescProposal")] Proposal proposal)
        {
            if (ModelState.IsValid)
            {
                await _context.Proposals.AddAsync(proposal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AllProposals));
            }
            return View(proposal);
        }
        public async Task<IEnumerable<Proposal>> GetAllProposalsAsync() => await _context.Proposals.ToListAsync();

        public async Task<IActionResult> DetailsProposal(int id)
        {
            var detailsProposal = await GetByProposalIdAsync(id);
            return View(detailsProposal);
        }

        public async Task<Proposal> GetByProposalIdAsync(int id)
        {
            var proposalDetails = await _context.Proposals

                .FirstOrDefaultAsync(p => p.Id == id);
            return proposalDetails;
        }


        [HttpPost]
        public async Task<IActionResult> Profile(UserViewModels model)
        {

            var currentUser = await userManager.GetUserAsync(User);
            var userId = currentUser.Id;
            var user = await userService.GetByIdAsync((string)userId);
            if (user == null)
            {
                return NotFound();
            }


            return View(model);
        }



        public async Task<IActionResult> EditMyProfile()
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var model = mapper.Map<UserProfileEditVM>(currentUser);

                return View(model);
            }
            return NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> EditMyProfile(UserProfileEditVM model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    currentUser.IdentityNumber = model.IdentityNumber;
                    currentUser.FullName = model.FullName;
                    currentUser.PhoneNumber = model.PhoneNumber;


                    var result = await userManager.UpdateAsync(currentUser);
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






        private bool UserExists(string id)
        {
            return string.IsNullOrEmpty(userService.GetByIdAsync(id).ToString());
        }



        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePassword)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                if (ModelState.IsValid)
                {
                    var result = await userManager.ChangePasswordAsync(currentUser, changePassword.CurrentPassword, changePassword.NewPassword);
                    if (result.Succeeded)
                    {
                        //TempData["Success"] = stringLocalizer["ChangePasswordMessage"]?.Value;
                        await signInManager.SignOutAsync();
                        return RedirectToAction("Login");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                return NotFound();
            }
            return View("Login", mapper.Map<ChangePasswordViewModel>(currentUser));


        }

        public async Task<IActionResult> UserReportAsPDF(int id)
        {
            //var comSolution = _context.Compalints_Solutions.Where(u => u.UserId == Id)
            //                 .GroupBy(c => c.UploadsComplainteId);

            //var AcceptSolution = _context.Compalints_Solutions.Where(u => u.UserId == Id)
            //                 .GroupBy(c => c.UploadsComplainteId, a => a.IsAccept);
            //var ComplaintsRejecteds = _context.ComplaintsRejecteds.Where(u => u.UserId == Id)
            //                 .GroupBy(c => c.UploadsComplainteId);
            //var user = await userManager.FindByIdAsync(Id);


            //if (user == null)
            //{
            //    return NotFound();
            //}
            var ComplantList = await _service.FindAsync(id);
            AddSolutionVM addsoiationView = new AddSolutionVM()
            {
                UploadsComplainteId = id,

            };
            ProvideSolutionsVM MV = new ProvideSolutionsVM
            {
                compalint = ComplantList,
                Compalints_SolutionList = await _context.Compalints_Solutions.Where(a => a.UploadsComplainteId == id).ToListAsync(),
                AddSolution = addsoiationView
            };




            return new ViewAsPdf("UserReportAsPDF", View(MV))
            {
                PageOrientation = Orientation.Portrait,
                MinimumFontSize = 25,
                PageSize = Size.A4,
                CustomSwitches = " --print-media-type --no-background --footer-line --header-line --page-offset 0 --footer-center [page] --footer-font-size 8 --footer-right \"page [page] from [topage]\"  "
            };
        }


        public async Task<IActionResult> ChangePassword()
        {
            return View();

        }

        public async Task<IActionResult> Delete(int id)
        {
            var proposalDetails = await _context.Proposals.FirstOrDefaultAsync(n => n.Id == id);
            if (proposalDetails != null)
            {
                _context.Proposals.Remove(proposalDetails);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AllProposals));
        }




    }



}

