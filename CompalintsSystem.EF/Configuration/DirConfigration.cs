using CompalintsSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompalintsSystem.EF.Configuration
{
    public class DirConfigration : IEntityTypeConfiguration<Departments>
    {


        public void Configure(EntityTypeBuilder<Departments> builder)
        {
            builder.HasData(
                    new Departments
                    {
                        Id = 1,
                        Name = "طب بشري",
                        CollegesId = 1,
                    },
                    new Departments
                    {
                        Id = 2,
                        Name = "صيدلة",
                        CollegesId = 1,
                    },
                    new Departments
                    {
                        Id = 3,
                        Name = "مختبرات",
                        CollegesId = 1,
                    },
                    new Departments
                    {
                        Id = 4,
                        Name = "تمريض",
                        CollegesId = 1,

                    },
                      new Departments
                      {
                          Id = 5,
                          Name = "هندسة مدني ",
                          CollegesId = 2,

                      },
                        new Departments
                        {
                            Id = 6,
                            Name = "هندسة معماري",
                            CollegesId = 2,

                        },
                      new Departments
                      {
                          Id = 7,
                          Name = "علوم حاسوب",
                          CollegesId = 2,

                      },
                     new Departments
                     {
                         Id = 8,
                         Name = " نظم معلومات حاسوبية",
                         CollegesId = 2,

                     },
                     new Departments
                     {
                         Id = 9,
                         Name = "الامن السيبراني والشبكات",
                         CollegesId = 2,

                     },
                      new Departments
                      {
                          Id = 10,
                          Name = "محاسبة",
                          CollegesId = 3,

                      },
                       new Departments
                       {
                           Id = 11,
                           Name = "ادارة اعمال ",
                           CollegesId = 3,

                       },
                        new Departments
                        {
                            Id = 12,
                            Name = "علوم مالية ومصرفية",
                            CollegesId = 3,

                        },
                      new Departments
                      {
                          Id = 13,
                          Name = "ادارة اعمال دولية",
                          CollegesId = 3,

                      },
                      new Departments
                      {
                          Id = 14,
                          Name = "الشرعة والقانون",
                          CollegesId = 4,

                      },
                      new Departments
                      {
                          Id = 15,
                          Name = "علوم زراعية",
                          CollegesId = 4,

                      },
                       new Departments
                       {
                           Id = 16,
                           Name = "قران كريم وعلومة _علوم الحياة",
                           CollegesId = 4,

                       },
                        new Departments
                        {
                            Id = 17,
                            Name = "لغة انجليزي",
                            CollegesId = 4,

                        },
                         new Departments
                         {
                             Id = 25,
                             Name = "دراسات عربية",
                             CollegesId = 4,

                         },
                          new Departments
                          {
                              Id = 18,
                              Name = "كيمياء",
                              CollegesId = 4,

                          },
                           new Departments
                           {
                               Id = 19,
                               Name = "فيزياء",
                               CollegesId = 4,

                           },
                            new Departments
                            {
                                Id = 20,
                                Name = "رياضيات",
                                CollegesId = 4,

                            },
                             new Departments
                             {
                                 Id = 21,
                                 Name = "جغرافيا_تاريخ",
                                 CollegesId = 4,

                             },
                              new Departments
                              {
                                  Id = 22,
                                  Name = "ترجمة",
                                  CollegesId = 4,

                              },
                               new Departments
                               {
                                   Id = 23,
                                   Name = "محاسبة وتمويل",
                                   CollegesId = 5,

                               },
                               new Departments
                               {
                                   Id = 24,
                                   Name = "ادارة اعمال دولية",
                                   CollegesId = 5,

                               }

                       
                );
        }




    }
}
