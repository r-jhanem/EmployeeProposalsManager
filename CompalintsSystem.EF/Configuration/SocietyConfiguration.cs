using CompalintsSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompalintsSystem.EF.Configuration
{
    public class SocietyConfiguration : IEntityTypeConfiguration<Society>
    {
        public void Configure(EntityTypeBuilder<Society> builder)
        {
            builder.HasData(
                    new Society
                    {
                        Id = 1,
                        Name = "الملتقى",
                    },
                    new Society
                    {
                        Id = 2,
                        Name = " موظف الشكوى",
                    },
                     new Society
                     {
                         Id = 3,
                         Name = "ادارة عامة",
                     }
                );
        }


    }

}