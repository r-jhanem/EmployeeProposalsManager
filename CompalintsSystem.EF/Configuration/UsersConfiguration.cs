using CompalintsSystem.Core;
using CompalintsSystem.Core.Helpers.Constants;
using CompalintsSystem.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompalintsSystem.EF.Configuration
{
    public class UsersConfiguration
    {


        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                // Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var usersToCreate = new List<(ApplicationUser User, string RoleName)>
        {
            (new ApplicationUser
            {
                FullName = "عبدالرحمن امين المصنف",
                IdentityNumber = "000243124111",
                Email = "000243124111",
                UserName = "000243124111",
                PhoneNumber = "775115810",
                CollegesId = 2,
                DepartmentsId = 7,
                SubDepartmentsId = 32,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                UserRoleName =UserRolesArbic.AdminGeneralFederation,
                RoleId = 1,
            }, UserRoles.AdminGeneralFederation),
            (new ApplicationUser
            {
                FullName = "عاصم خاوز",
                IdentityNumber = "023453253455",
                Email = "023453253455",
                UserName = "023453253455",
                PhoneNumber = "77883534",
                CollegesId = 2,
                DepartmentsId = 7,
                SubDepartmentsId = 29,
                EmailConfirmed = true,
                 UserRoleName =UserRolesArbic.AdminColleges,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                 RoleId = 2,
            }, UserRoles.AdminColleges),
            (new ApplicationUser
            {
                FullName = "يوسف الخالد",
                IdentityNumber = "023453253454",
                Email = "023453253454",
                UserName = "023453253454",
                PhoneNumber = "77883534",
                CollegesId = 1,
                DepartmentsId = 1,
                SubDepartmentsId = 5,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                UserRoleName =UserRolesArbic.AdminDepartments,
                RoleId = 3,
            }, UserRoles.AdminDepartments),
            (new ApplicationUser
            {
                Id ="e70aa106-c6d6-4037-bb94-eb241dd1ef60",
                FullName = "عاهد الشومي",
                IdentityNumber = "001024312444",
                Email = "001024312444",
                UserName = "001024312444",
                PhoneNumber = "77883534",
                CollegesId = 2,
                DepartmentsId = 7,
                SubDepartmentsId = 30,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                 UserRoleName =UserRolesArbic.AdminSubDepartments,
                  RoleId = 4,
            }, UserRoles.AdminSubDepartments),
            (new ApplicationUser
            {
                Id = "e70aa106-c6d6-4037-bb94-eb241dd1ef50",
                FullName = "ذا النون القحطاني",
                IdentityNumber = "000243124222",
                Email = "000243124222",
                UserName = "000243124222",
                PhoneNumber = "773453534",
                CollegesId = 2,
                DepartmentsId = 7,
                SubDepartmentsId = 31,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                UserRoleName =UserRolesArbic.Beneficiarie,
                RoleId = 5,
            }, UserRoles.Beneficiarie),// 



            //======================================
            (new ApplicationUser
            {
                FullName = " رضوان المصنف ",
                IdentityNumber = "000243124001",
                Email = "000243124001",
                UserName = "000243124001",
                PhoneNumber = "775115810",
                CollegesId = 2,
                DepartmentsId = 7,
                SubDepartmentsId = 32,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                 UserRoleName =UserRolesArbic.AdminColleges,
                  RoleId = 2,
            }, UserRoles.AdminColleges),
            (new ApplicationUser
            {
                FullName = " سامي الخمري",
                IdentityNumber = "023453253433",
                Email = "023453253433",
                UserName = "023453253433",
                PhoneNumber = "77883534",
                CollegesId = 2,
                DepartmentsId = 6,
                SubDepartmentsId = 27,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                 UserRoleName =UserRolesArbic.AdminColleges,
                  RoleId = 2,
            }, UserRoles.AdminColleges),
            (new ApplicationUser
            {
                FullName = "حمود الجحدبي",
                IdentityNumber = "023453253422",
                Email = "023453253422",
                UserName = "023453253422",
                PhoneNumber = "77883534",
                CollegesId = 2,
                DepartmentsId = 7,
                SubDepartmentsId = 30,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                 UserRoleName =UserRolesArbic.AdminDepartments,
                  RoleId = 3,
            }, UserRoles.AdminDepartments),
            (new ApplicationUser
            {
                FullName = "جواد الزبدي",
                IdentityNumber = "001024312455",
                Email = "001024312455",
                UserName = "001024312455",
                PhoneNumber = "77883534",
                CollegesId = 1,
                DepartmentsId = 1,
                SubDepartmentsId = 4,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                 UserRoleName =UserRolesArbic.AdminSubDepartments,
                  RoleId = 4,
            }, UserRoles.AdminSubDepartments),
            (new ApplicationUser
            {
                FullName = " عبده علي فاضل الخمري",
                IdentityNumber = "000243124121",
                Email = "000243124121",
                UserName = "000243124121",
                PhoneNumber = "773453534",
                CollegesId = 2,
                DepartmentsId = 7,
                SubDepartmentsId = 29,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                 UserRoleName =UserRolesArbic.AdminDepartments,
                  RoleId = 3,
            }, UserRoles.AdminDepartments),

            //==================================================
             (new ApplicationUser
            {
                FullName = "محمد على ",
                IdentityNumber = "000243124445",
                Email = "000243124445",
                UserName = "000243124445",
                PhoneNumber = "775115810",
                CollegesId = 1,
                DepartmentsId = 1,
                SubDepartmentsId = 3,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                 UserRoleName =UserRolesArbic.AdminGeneralFederation,
                  RoleId = 1,
            }, UserRoles.AdminGeneralFederation),
            (new ApplicationUser
            {
                FullName = "صقر العرشي",
                IdentityNumber = "023453253490",
                Email = "023453253490",
                UserName = "023453253490",
                PhoneNumber = "77883534",
                CollegesId = 1,
                DepartmentsId = 1,
                SubDepartmentsId = 1,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                 UserRoleName =UserRolesArbic.AdminColleges,
                  RoleId = 2,
            }, UserRoles.AdminColleges),
            (new ApplicationUser
            {
                FullName = "صلاح البدوي",
                IdentityNumber = "023453253499",
                Email = "023453253499",
                UserName = "023453253499",
                PhoneNumber = "77883534",
                CollegesId = 1,
                DepartmentsId = 1,
                SubDepartmentsId = 1,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                 UserRoleName =UserRolesArbic.AdminDepartments,
                  RoleId = 13,
            }, UserRoles.AdminDepartments),
            (new ApplicationUser
            {
                FullName = "نجم الدين صالح المصنف",
                IdentityNumber = "001024312555",
                Email = "001024312555",
                UserName = "001024312555",
                PhoneNumber = "77883534",
                CollegesId = 1,
                DepartmentsId = 1,
                SubDepartmentsId = 1,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                 UserRoleName =UserRolesArbic.AdminSubDepartments,
                  RoleId = 4,
            }, UserRoles.AdminSubDepartments),
            (new ApplicationUser
            {
                FullName = " محمد امين المصنف",
                IdentityNumber = "000243124666",
                Email = "000243124666",
                UserName = "000243124666",
                PhoneNumber = "773453534",
                CollegesId = 1,
                DepartmentsId = 1,
                SubDepartmentsId = 1,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                 UserRoleName =UserRolesArbic.AdminSubDepartments,
                  RoleId = 4,
            }, UserRoles.AdminSubDepartments),
            (new ApplicationUser
            {
                FullName = " سعيد علي احمد",
                IdentityNumber = "000243124222",
                Email = "000243124222",
                UserName = "000243124222",
                PhoneNumber = "773453534",
                CollegesId = 2,
                DepartmentsId = 7,
                SubDepartmentsId = 30,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedDate = DateTime.Now,
                UserRoleName = UserRolesArbic.Beneficiarie,
                RoleId = 4,
            }, UserRoles.Beneficiarie),



        };

                foreach (var userData in usersToCreate)
                {
                    var (user, roleName) = userData;

                    var existingUser = await userManager.FindByEmailAsync(user.Email);
                    if (existingUser == null)
                    {
                        await userManager.CreateAsync(user, "Coding@1234?");

                        if (!await roleManager.RoleExistsAsync(roleName))
                        {
                            await roleManager.CreateAsync(new IdentityRole(roleName));
                        }

                        await userManager.AddToRoleAsync(user, roleName);
                    }
                }
            }
        }

    }
}
