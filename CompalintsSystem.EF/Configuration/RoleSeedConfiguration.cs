using CompalintsSystem.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CompalintsSystem.EF.Configuration
{
    public class RoleSeedConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                 new IdentityRole
                 {
                     Id = Guid.Parse("2e803883-c018-4f7c-90e3-c3c1db0d8f00").ToString(),
                     Name = UserRoles.AdminGeneralFederation,
                     NormalizedName = UserRoles.AdminGeneralFederation.ToUpper(),

                 },
                 new IdentityRole
                 {
                     Id = Guid.Parse("7f95d3fd-8840-466e-9da6-d7dcf06298de").ToString(),
                     Name = UserRoles.AdminColleges,
                     NormalizedName = UserRoles.AdminColleges.ToUpper(),

                 }
                 ,
                 new IdentityRole
                 {
                     Id = Guid.Parse("81b7a93d-4221-4d50-884c-08d98676a9c8").ToString(),
                     Name = UserRoles.AdminDepartments,
                     NormalizedName = UserRoles.AdminDepartments.ToUpper(),


                 },
                 new IdentityRole
                 {
                     Id = Guid.Parse("64a8ff4a-f4a2-405c-9c1a-85f0f9f46145").ToString(),
                     Name = UserRoles.AdminSubDepartments,
                     NormalizedName = UserRoles.AdminSubDepartments.ToUpper(),

                 },

                  new IdentityRole
                  {
                      Id = Guid.Parse("8faa31eb-ded0-4711-8e0b-0f509c4b332f").ToString(),
                      Name = UserRoles.Beneficiarie,
                      NormalizedName = UserRoles.Beneficiarie.ToUpper(),

                  }

            );
        }
    }
}