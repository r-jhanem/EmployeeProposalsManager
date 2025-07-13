using CompalintsSystem.Core.Interfaces;
using CompalintsSystem.Core.Models;
using CompalintsSystem.Core.ViewModels;
using CompalintsSystem.EF.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CompalintsSystem.Application.Controllers
{
    [Authorize(Roles = "AdminColleges,AdminPolicy,AdminSubDepartments")]
    public class CompalintsMangController : Controller
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

        private readonly ICategoryService categoryService;
        public CompalintsMangController(

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

        public IActionResult Index()
        {
            return View();
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
                var Getrejected = AllComplaintsUp.Where(g => g.ReturnedTo == currentUser.Id && g.StatusCompalintId == 1).ToList(

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

        //public async Task<IActionResult> ComplaintsForMe()
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    var userId = currentUser.Id;

        //    var AllComplaintsUp = await _compReop.GetAllAsync(
        //        g => g.Colleges,
        //        d => d.Departments,
        //        t => t.TypeComplaint,
        //        s => s.SubDepartments,
        //        n => n.StatusCompalint,
        //        st => st.StagesComplaint,
        //        up => up.ComplaintsUp);


        //    if (AllComplaintsUp != null)
        //    {
        //        var Getrejected = AllComplaintsUp.Where(g => g.ReturnedTo == currentUser.Id);


        //        int totalCompalintsUp = Getrejected.Count();
        //        ViewBag.totalCompalintsUp = totalCompalintsUp;

        //        return View(Getrejected);
        //    }
        //    var emptyList = Enumerable.Empty<UpComplaintCause>(); // إنشاء قائمة فارغة من الشكاوى
        //    return View(emptyList); // إرجاع قائمة فارغة في حالة عدم وجود شكاوى مرفوضة
        //}


        public async Task<IActionResult> ViewCompalintDetails(int id)
        {
            var ComplantList = await _compReop.FindAsync(id);
            var currentUser = await GetCurrentUser();
            var userDropdownsData = await GetCommunicationDropdownsData(currentUser);
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





    }
}