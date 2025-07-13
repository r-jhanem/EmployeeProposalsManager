using AutoMapper;
using CompalintsSystem.Core.Interfaces;
using CompalintsSystem.Core.Models;
using CompalintsSystem.Core.ViewModels;
using CompalintsSystem.EF.DataBase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CompalintsSystem.EF.Repositories
{
    public class CompalintRepository : EntityBaseRepository<UploadsComplainte>, ICompalintRepository
    {
        private readonly AppCompalintsContextDB _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public string Error { get; set; }
        public int returntype { get; set; }

        public CompalintRepository(
            AppCompalintsContextDB context,
            IMapper mapper,
            IWebHostEnvironment env,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager


            ) : base(context, mapper, env, userManager, signInManager)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
            _userManager = userManager;
            _signInManager = signInManager;

        }


        public IQueryable<UploadsComplainte> GetAllRejectedComplaints(string identity)
        {
            var result = _context.UploadsComplaintes.Where(j => j.StatusCompalintId == 3 && j.UserId == identity)
               .OrderByDescending(u => u.UploadDate)
               .Include(s => s.StatusCompalint)
               .Include(f => f.TypeComplaint)
               .Include(st => st.StagesComplaint)
               .Include(g => g.Colleges)
               .Include(d => d.Departments);
            return result;
        }


        public IQueryable<UploadsComplainte> GetAllResolvedComplaints(string identity)
        {
            var result = _context.UploadsComplaintes.Where(j => j.StatusCompalintId == 2 && j.UserId == identity)
               .OrderByDescending(u => u.UploadDate)
               .Include(s => s.StatusCompalint)
               .Include(st => st.StagesComplaint)
               .Include(f => f.TypeComplaint)
               .Include(g => g.Colleges)
               .Include(d => d.Departments);
            return result;
        }

        public Task GetAllGategoryCompAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdatedbCompAsync(UploadsComplainte data)
        {
            var dbComp = await _context.UploadsComplaintes.FirstOrDefaultAsync(n => n.Id == data.Id);
            if (dbComp != null)
            {

                dbComp.Id = data.Id;
                dbComp.StagesComplaintId = 2;


                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
        }

        public IQueryable<UploadsComplainte> Search(string term)
        {
            var result = _context.UploadsComplaintes.Where(
                u => u.TitleComplaint == term
                || u.DescComplaint == term);
            return result;
        }



        public IQueryable<UploadsComplainte> GetAll()
        {

            var result = _context.UploadsComplaintes
            .Include(s => s.StatusCompalint)
            .Include(f => f.TypeComplaint)
            .Include(st => st.StagesComplaint)
            .Include(g => g.Colleges)
            .Include(d => d.Departments)
            .Include(su => su.SubDepartments)
            .OrderByDescending(u => u.UploadDate);
            //Task.WhenAll((System.Collections.Generic.IEnumerable<Task>)result);
            return result;


        }

        public IQueryable<UsersCommunication> GetCommunicationBy(string UserId)
        {
            var result = _context.UsersCommunications.Where(u => u.reportSubmitterId == UserId)
            .OrderByDescending(u => u.CreateDate)
            .Include(s => s.TypeCommunication)
            .Include(f => f.reportSubmitter)
            .Include(g => g.Colleges)
            .Include(d => d.Departments)
            .Include(su => su.SubDepartments);

            return result;
        }

        // جلب الشكاوى المقدمة من الطالب الخاصة به وعرضها له
        public IQueryable<UploadsComplainte> GetBy(string Identity)
        {
            var result = _context.UploadsComplaintes.Where(u => u.UserId == Identity)
                .OrderByDescending(u => u.UploadDate)
                .Include(s => s.StatusCompalint)
                .Include(st => st.StagesComplaint)
                .Include(f => f.TypeComplaint)
                .Include(g => g.Colleges)
                .Include(d => d.Departments)
                .Include(su => su.SubDepartments);

            return result;
        }
        public IQueryable<UploadsComplainte> GetRejectComp(string userId)
        {
            var result = _context.UploadsComplaintes.Where(u => u.UserId == userId)
                .OrderByDescending(u => u.UploadDate)
                .Include(s => s.StatusCompalint)
                .Include(f => f.TypeComplaint)
                .Include(g => g.Colleges)
                .Include(d => d.Departments)
                .Include(su => su.SubDepartments);

            return result;
        }


        public async Task<UploadsComplainte> GetCompalintById(int id)
        {
            var compalintDetails = _context.UploadsComplaintes
                .Include(s => s.StatusCompalint)
                .Include(f => f.TypeComplaint)
                .Include(g => g.Colleges)
                .Include(d => d.Departments)
                .Include(su => su.SubDepartments)
                .Include(st => st.StagesComplaint)
                .FirstOrDefaultAsync(c => c.Id == id);
            //var compalintDetails = from m in _context.UploadsComplainte select m;
            return await compalintDetails;
        }
        // جلب الشكاوى المقدمة من الطالب الخاصة به وعرضها له
        public IQueryable<UploadsComplainte> GetBenfeficarieCompalintBy(string Identity)
        {
            var result = _context.UploadsComplaintes.Where(u => u.UserId == Identity)
                .OrderByDescending(u => u.UploadDate)
                .Include(s => s.StatusCompalint)
                .Include(st => st.StagesComplaint)
                .Include(f => f.TypeComplaint)
                .Include(g => g.Colleges)
                .Include(d => d.Departments)
                .Include(su => su.SubDepartments);

            return result;
        }
        public async Task<UploadsComplainte> FindAsync(int id)
        {
            var selectedUpload = await _context.UploadsComplaintes.FindAsync(id);
            if (selectedUpload != null)
            {
                //AutoMapper 
                var compalintDetails = _context.UploadsComplaintes
                .Include(s => s.StatusCompalint)
                .Include(f => f.TypeComplaint)
                .Include(g => g.Colleges)
                .Include(d => d.Departments)
                .Include(su => su.SubDepartments)
                .Include(st => st.StagesComplaint)
                .FirstOrDefaultAsync(c => c.Id == id);
                //var compalintDetails = from m in _context.UploadsComplainte select m;
                return await compalintDetails;
            }
            return null;
        }

        public async Task CreateAsync(InputCompmallintVM model)
        {
            var mappedObj = _mapper.Map<UploadsComplainte>(model);
            await _context.UploadsComplaintes.AddAsync(mappedObj);
            await _context.SaveChangesAsync();
        }

        public async Task CreateCommuncationAsync(AddCommunicationVM model)
        {
            var mappedObj = _mapper.Map<UsersCommunication>(model);
            await _context.UsersCommunications.AddAsync(mappedObj);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(string id)
        {
            var DeletedUser = await _userManager.FindByIdAsync(id);
            var roleId = await _userManager.GetRolesAsync(DeletedUser);
            if (DeletedUser == null)
            {
                return;
            }
            await _userManager.RemoveFromRolesAsync(DeletedUser, roleId);
            await _userManager.DeleteAsync(DeletedUser);
            await _context.SaveChangesAsync();
        }

        public async Task<SelectDataCommuncationDropdownsVM> GetAddCommunicationDropdownsValues(string subDirctoty, int DepartmentsId, int CollegesId, string role, string roleId)
        {
            var userManager = _userManager;// initialize your user manager here
            if (role == "AdminColleges")
            {
                var responseGov = new SelectDataCommuncationDropdownsVM
                {
                    ApplicationUsers = await _context.Users
                        .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { User = u, UserRole = ur })
                        .Join(_context.Roles, ur => ur.UserRole.RoleId, r => r.Id, (ur, r) => new { User = ur.User, UserRole = ur.UserRole, Role = r })
                        .Where(x =>
                            (x.UserRole.RoleId == roleId || x.UserRole.RoleId == "2e803883-c018-4f7c-90e3-c3c1db0d8f00") &&
                            x.User.CollegesId == CollegesId &&
                            x.User.DepartmentsId == DepartmentsId
                        )
                        .OrderBy(x => x.User.FullName)
                        .Select(x => new ApplicationUser
                        {
                            Id = x.User.Id,
                            FullName = x.User.FullName + " ( " + x.User.UserRoleName + "  )",
                        })
                        .ToListAsync(),


                    TypeCommunications = await _context.TypeCommunications
                        .OrderBy(tc => tc.Type)
                        .ToListAsync()
                };

                foreach (var user in responseGov.ApplicationUsers)
                {
                    var identityUser = await userManager.FindByIdAsync(user.Id);
                    var roles = await userManager.GetRolesAsync(identityUser);
                    user.UserRoles = roles.Select(role => new IdentityRole { Name = role }).ToList();
                }



                return responseGov;


            }
            else if (role == "AdminGeneralFederation")
            {
                var responseDir = new SelectDataCommuncationDropdownsVM
                {
                    ApplicationUsers = await _context.Users
                     .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { User = u, UserRole = ur })
                     .Where(x => x.UserRole.RoleId == roleId || x.UserRole.RoleId == "2e803883-c018-4f7c-90e3-c3c1db0d8f00"
                         && x.User.CollegesId == CollegesId
                         && x.User.DepartmentsId == DepartmentsId)
                     .OrderBy(x => x.User.FullName)
                        .Select(x => new ApplicationUser
                        {
                            Id = x.User.Id,
                            FullName = x.User.FullName + " ( " + x.User.UserRoleName + "  )",
                        })
                        .ToListAsync(),


                    TypeCommunications = await _context.TypeCommunications
                        .OrderBy(tc => tc.Type)
                        .ToListAsync()
                };

                foreach (var user in responseDir.ApplicationUsers)
                {
                    var identityUser = await userManager.FindByIdAsync(user.Id);
                    var roles = await userManager.GetRolesAsync(identityUser);
                    user.UserRoles = roles.Select(role => new IdentityRole { Name = role }).ToList();
                }

                return responseDir;
            }
            else if (role == "AdminDepartments")
            {
                var responseSub = new SelectDataCommuncationDropdownsVM
                {
                    ApplicationUsers = await _context.Users
                     .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { User = u, UserRole = ur })
                     .Where(x => x.UserRole.RoleId == roleId || x.UserRole.RoleId == "7f95d3fd-8840-466e-9da6-d7dcf06298de"
                         && x.User.CollegesId == CollegesId
                         && x.User.DepartmentsId == DepartmentsId)
                     .OrderBy(x => x.User.FullName)
                        .Select(x => new ApplicationUser
                        {
                            Id = x.User.Id,
                            FullName = x.User.FullName + " ( " + x.User.UserRoleName + "  )",
                        })
                        .ToListAsync(),


                    TypeCommunications = await _context.TypeCommunications
                        .OrderBy(tc => tc.Type)
                        .ToListAsync()
                };

                foreach (var user in responseSub.ApplicationUsers)
                {
                    var identityUser = await userManager.FindByIdAsync(user.Id);
                    var roles = await userManager.GetRolesAsync(identityUser);
                    user.UserRoles = roles.Select(role => new IdentityRole { Name = role }).ToList();
                }

                return responseSub;
            }
            else if (role == "AdminSubDepartments")
            {
                var responseSub = new SelectDataCommuncationDropdownsVM
                {
                    ApplicationUsers = await _context.Users
                     .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { User = u, UserRole = ur })
                     .Where(x => x.UserRole.RoleId == roleId || x.UserRole.RoleId == "81b7a93d-4221-4d50-884c-08d98676a9c8"
                         && x.User.CollegesId == CollegesId
                         && x.User.DepartmentsId == DepartmentsId)
                     .OrderBy(x => x.User.FullName)
                        .Select(x => new ApplicationUser
                        {
                            Id = x.User.Id,
                            FullName = x.User.FullName + " ( " + x.User.UserRoleName + "  )",
                        })
                        .ToListAsync(),


                    TypeCommunications = await _context.TypeCommunications
                        .OrderBy(tc => tc.Type)
                        .ToListAsync()
                };

                foreach (var user in responseSub.ApplicationUsers)
                {
                    var identityUser = await userManager.FindByIdAsync(user.Id);
                    var roles = await userManager.GetRolesAsync(identityUser);
                    user.UserRoles = roles.Select(role => new IdentityRole { Name = role }).ToList();
                }

                return responseSub;
            }
            else
            {
                var responseAdmin = new SelectDataCommuncationDropdownsVM
                {
                    ApplicationUsers = await _context.Users
                    .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { User = u, UserRole = ur })
                    .Where(x => x.UserRole.RoleId != roleId)
                      .OrderBy(x => x.User.FullName)
                        .Select(x => new ApplicationUser
                        {
                            Id = x.User.Id,
                            FullName = x.User.FullName + "   " + "(" + x.User.UserRoleName + ")",
                        }).ToListAsync(),

                    TypeCommunications = await _context.TypeCommunications
                      .OrderBy(tc => tc.Type)
                      .ToListAsync()
                };
                foreach (var user in responseAdmin.ApplicationUsers)
                {
                    var identityUser = await userManager.FindByIdAsync(user.Id);
                    var roles = await userManager.GetRolesAsync(identityUser);
                    user.UserRoles = roles.Select(role => new IdentityRole { Name = role }).ToList();
                }

                return responseAdmin;
            }
        }


        public async Task<SelectDataCommuncationDropdownsVM> GetUserDropdownsValues(string userId, int subDirctoty, int DepartmentsId, int CollegesId, string role, string roleId)
        {
            var userManager = _userManager;// initialize your user manager here
            var usersQuery = _context.Users
              .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { User = u, UserRole = ur })
              .Where(x => x.UserRole.RoleId == roleId && x.User.CollegesId == CollegesId &&
                           x.User.DepartmentsId == DepartmentsId ||
                           x.User.SubDepartmentsId == subDirctoty && x.User.Id != userId && x.UserRole.RoleId != "8faa31eb-ded0-4711-8e0b-0f509c4b332f"
                           )
              .OrderBy(x => x.User.FullName)
              .Select(x => new ApplicationUser
              {
                  Id = x.User.Id,
                  FullName = x.User.FullName + " ( " + x.User.UserRoleName + "  )",
              });

            var applicationUsers = await usersQuery.ToListAsync();

            var typeCommunications = await _context.TypeCommunications
                .OrderBy(tc => tc.Type)
                .ToListAsync();

            var response = new SelectDataCommuncationDropdownsVM
            {
                ApplicationUsers = applicationUsers,
                TypeCommunications = typeCommunications
            };


            foreach (var user in response.ApplicationUsers)
            {
                var identityUser = await userManager.FindByIdAsync(user.Id);
                var roles = await userManager.GetRolesAsync(identityUser);
                user.UserRoles = roles.Select(role => new IdentityRole { Name = role }).ToList();
            }

            return response;
        }


        public async Task<SelectDataCommuncationDropdownsVM> GetAddCommunicationDropdownsValues2()
        {

            var response = new SelectDataCommuncationDropdownsVM
            {
                ApplicationUsers = await _context.Users
                    .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { User = u, UserRole = ur })
                    .Join(_context.Roles, ur => ur.UserRole.RoleId, r => r.Id, (ur, r) => new { User = ur.User, UserRole = ur.UserRole, Role = r })
                    .Where(x =>
                        (x.UserRole.RoleId == "48e9b2f6-42e7-439f-afa2-03cf13342517")

                    )
                    .OrderBy(x => x.User.FullName)
                    .Select(x => new ApplicationUser
                    {
                        FullName = x.User.FullName + " ( " + x.User.UserRoleName + "  )",
                    })
                    .ToListAsync(),


                TypeCommunications = await _context.TypeCommunications
                    .OrderBy(tc => tc.Type)
                    .ToListAsync()
            };

            return response;

        }

        public bool CanSubmitComplaintForUserToday(string userId, string date)
        {
            bool hasComplaint = _context.UploadsComplaintes.Any(u => u.UserId == userId && u.Today == date);
            return hasComplaint;
        }


    }
}