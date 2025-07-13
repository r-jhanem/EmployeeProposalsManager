using CompalintsSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CompalintsSystem.EF.Configuration
{
    public class TypeComplaintConfiguration : IEntityTypeConfiguration<TypeComplaint>
    {
        public void Configure(EntityTypeBuilder<TypeComplaint> builder)
        {
            builder.HasData(
                    new TypeComplaint
                    {
                        //Id = Guid.NewGuid().ToString(),
                        Id = 1,
                        Type = "شكاوى أكاديمية",
                        UsersNameAddType = "قيمة افتراضية من النضام",
                        CreatedDate = DateTime.Now,
                    },
                      new TypeComplaint
                      {
                          //Id = Guid.NewGuid().ToString(),
                          Id = 2,
                          Type = "شكاوى إدارية",
                          UsersNameAddType = "قيمة افتراضية من النضام",
                          CreatedDate = DateTime.Now,
                      },
                        new TypeComplaint
                        {
                            //Id = Guid.NewGuid().ToString(),
                            Id = 3,
                            Type = "شكاوى تقنية",
                            UsersNameAddType = "قيمة افتراضية من النضام",
                            CreatedDate = DateTime.Now,
                        },
                          new TypeComplaint
                          {
                              //Id = Guid.NewGuid().ToString(),
                              Id = 4,
                              Type = "شكاوى تمييز وتحيز",
                              UsersNameAddType = "قيمة افتراضية من النضام",
                              CreatedDate = DateTime.Now,
                          },
                            new TypeComplaint
                            {
                                //Id = Guid.NewGuid().ToString(),
                                Id = 5,
                                Type = "شكاوى سلوك غير لائق",
                                UsersNameAddType = "قيمة افتراضية من النضام",
                                CreatedDate = DateTime.Now,
                            },
                    new TypeComplaint
                    {
                        Id = 6,
                        Type = "شكاوى الأمان والسلامة",
                        UsersNameAddType = "قيمة افتراضية من النضام",
                        CreatedDate = DateTime.Now,

                    }
                );
        }


    }

}
