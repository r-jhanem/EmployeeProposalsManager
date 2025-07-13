using CompalintsSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompalintsSystem.EF.Configuration
{
    public class GovConfigration : IEntityTypeConfiguration<Colleges>
    {


        public void Configure(EntityTypeBuilder<Colleges> builder)
        { 


            builder.HasData(
                    new Colleges
                    {
                        Id = 1,
                        Name = "كلية الطب العلوم الصحية ",
                    },
                    new Colleges
                    {
                        Id = 2,
                        Name = " كلية الهندسة وتقنية المعلومات",
                    },
                     new Colleges
                     {
                         Id = 3,
                         Name = " كلية التجارة والاقتصاد",
                     },
                      new Colleges
                      {
                          Id = 4,
                          Name = "كلية العلوم التطبيقية والانسانية ",
                      },
                     new Colleges
                     {
                         Id = 5,
                         Name = "كلية الاعمال باللغة الانجليزية",
                     }
                );
        }


    }

}