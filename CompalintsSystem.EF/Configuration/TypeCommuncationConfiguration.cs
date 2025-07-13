using CompalintsSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CompalintsSystem.EF.Configuration
{
    public class TypeCommuncationConfiguration : IEntityTypeConfiguration<TypeCommunication>
    {
        public void Configure(EntityTypeBuilder<TypeCommunication> builder)
        {
            builder.HasData(
                    new TypeCommunication
                    {
                        Id = 1,
                        Type = "بلاغ أكاديمي",
                        UsersNameAddType = "قيمة افتراضية من النضام",
                        CreatedDate = DateTime.Now,
                    },
                     new TypeCommunication
                     {
                         Id = 2,
                         Type = "بلاغ إداري",
                         UsersNameAddType = "قيمة افتراضية من النضام",
                         CreatedDate = DateTime.Now,
                     },
                      new TypeCommunication
                      {
                          Id = 3,
                          Type = "بلاغ تأديبي",
                          UsersNameAddType = "قيمة افتراضية من النضام",
                          CreatedDate = DateTime.Now,
                      },
                       new TypeCommunication
                       {
                           Id = 4,
                           Type = "بلاغ تمييز",
                           UsersNameAddType = "قيمة افتراضية من النضام",
                           CreatedDate = DateTime.Now,
                       },
                        new TypeCommunication
                        {
                            Id = 5,
                            Type = "بلاغ أمان وصحة",
                            UsersNameAddType = "قيمة افتراضية من النضام",
                            CreatedDate = DateTime.Now,
                        },
                        new TypeCommunication
                        {
                            Id = 7,
                            Type = "بلاغ تحرش",
                            UsersNameAddType = "قيمة افتراضية من النضام",
                            CreatedDate = DateTime.Now,
                        },
                    new TypeCommunication
                    {
                        //Id = Guid.NewGuid().ToString(),
                        Id = 6,
                        Type = "تلاعب بالحلول",
                        UsersNameAddType = "قيمة افتراضية من النضام",
                        CreatedDate = DateTime.Now,

                    }
                );
        }


    }

}
