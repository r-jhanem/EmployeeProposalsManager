
using CompalintsSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompalintsSystem.EF.Configuration
{
    public class ComplatinClassfactionConfiguration : IEntityTypeConfiguration<ComplatinClassfaction>
    {
        public void Configure(EntityTypeBuilder<ComplatinClassfaction> builder)
        {
            builder.HasData(
                    new ComplatinClassfaction
                    {
                        Id = 1,
                        Name = "عالية",

                    },
                    new ComplatinClassfaction
                    {
                        Id = 2,
                        Name = "متوسطة",
                    },

                      new ComplatinClassfaction
                      {
                          Id = 3,
                          Name = "منخفضة",
                      }
                       
                );
        }


    }

}