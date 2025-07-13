using CompalintsSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompalintsSystem.EF.Configuration
{
    public class SubConfigration : IEntityTypeConfiguration<SubDepartments>
    {
        public void Configure(EntityTypeBuilder<SubDepartments> builder)
        {
            builder.HasData(
                    new SubDepartments
                    {
                        Id = 1,
                        Name = " الاول",
                        DepartmentsId = 1,
                    },
                      new SubDepartments
                      {
                          Id = 2,
                          Name = " الثاني",
                          DepartmentsId = 1,
                      },
                        new SubDepartments
                        {
                            Id = 3,
                            Name = " الثالث",
                            DepartmentsId = 1,
                        },
                          new SubDepartments
                          {
                              Id = 4,
                              Name = " الرابع",
                              DepartmentsId = 1,
                          },
                            new SubDepartments
                            {
                                Id = 5,
                                Name = " الخامس",
                                DepartmentsId = 1,
                            },
                              new SubDepartments
                              {
                                  Id = 6,
                                  Name = " السادس",
                                  DepartmentsId = 1,
                              },
                                new SubDepartments
                                {
                                    Id = 7,
                                    Name = " السابع",
                                    DepartmentsId = 1,
                                },
                                  new SubDepartments
                                  {
                                      Id = 8,
                                      Name = " الاول",
                                      DepartmentsId = 2,
                                  },
                      new SubDepartments
                      {
                          Id = 9,
                          Name = " الثاني",
                          DepartmentsId = 2,
                      },
                        new SubDepartments
                        {
                            Id = 10,
                            Name = " الثالث",
                            DepartmentsId = 2,
                        },
                          new SubDepartments
                          {
                              Id = 11,
                              Name = " الرابع",
                              DepartmentsId =2,
                          },
                            new SubDepartments
                            {
                                Id = 12,
                                Name = " الخامس",
                                DepartmentsId = 2,
                            },
      new SubDepartments
      {
          Id = 13,
          Name = " الاول",
          DepartmentsId = 3,
      },
                      new SubDepartments
                      {
                          Id = 14,
                          Name = " الثاني",
                          DepartmentsId = 3,
                      },
                        new SubDepartments
                        {
                            Id = 15,
                            Name = " الثالث",
                            DepartmentsId = 3,
                        },
                          new SubDepartments
                          {
                              Id = 16,
                              Name = " الرابع",
                              DepartmentsId = 3,
                          },

      new SubDepartments
      {
          Id = 17,
          Name = " الاول",
          DepartmentsId = 4,
      },
                      new SubDepartments
                      {
                          Id = 18,
                          Name = " الثاني",
                          DepartmentsId = 4,
                      },
                        new SubDepartments
                        {
                            Id = 19,
                            Name = " الثالث",
                            DepartmentsId = 4,
                        },
                          new SubDepartments
                          {
                              Id = 20,
                              Name = " الرابع",
                              DepartmentsId = 4,
                          },
                                new SubDepartments
                                {
                                    Id = 21,
                                    Name = " الاول",
                                    DepartmentsId = 5,
                                },
                      new SubDepartments
                      {
                          Id = 22,
                          Name = " الثاني",
                          DepartmentsId = 5,
                      },
                        new SubDepartments
                        {
                            Id = 23,
                            Name = " الثالث",
                            DepartmentsId = 5,
                        },
                          new SubDepartments
                          {
                              Id = 24,
                              Name = " الرابع",
                              DepartmentsId = 5,
                          },
                                new SubDepartments
                                {
                                    Id = 25,
                                    Name = " الاول",
                                    DepartmentsId = 6,
                                },
                      new SubDepartments
                      {
                          Id = 26,
                          Name = " الثاني",
                          DepartmentsId = 6,
                      },
                        new SubDepartments
                        {
                            Id = 27,
                            Name = " الثالث",
                            DepartmentsId = 6,
                        },
                          new SubDepartments
                          {
                              Id = 28,
                              Name = " الرابع",
                              DepartmentsId = 6,
                          },
                                new SubDepartments
                                {
                                    Id = 29,
                                    Name = " الاول",
                                    DepartmentsId = 7,
                                },
                      new SubDepartments
                      {
                          Id = 30,
                          Name = " الثاني",
                          DepartmentsId = 7,
                      },
                        new SubDepartments
                        {
                            Id = 31,
                            Name = " الثالث",
                            DepartmentsId = 7,
                        },
                          new SubDepartments
                          {
                              Id = 32,
                              Name = " الرابع",
                              DepartmentsId = 7,
                          },
                                new SubDepartments
                                {
                                    Id = 33,
                                    Name = " الاول",
                                    DepartmentsId = 8,
                                },
                      new SubDepartments
                      {
                          Id = 34,
                          Name = " الثاني",
                          DepartmentsId = 8,
                      },
                        new SubDepartments
                        {
                            Id = 35,
                            Name = " الثالث",
                            DepartmentsId = 8,
                        },
                          new SubDepartments
                          {
                              Id = 36,
                              Name = " الرابع",
                              DepartmentsId = 8,
                          },
                                new SubDepartments
                                {
                                    Id = 37,
                                    Name = " الاول",
                                    DepartmentsId = 9,
                                },
                      new SubDepartments
                      {
                          Id = 38,
                          Name = " الثاني",
                          DepartmentsId = 9,
                      },
                        new SubDepartments
                        {
                            Id = 39,
                            Name = " الثالث",
                            DepartmentsId = 9,
                        },
                          new SubDepartments
                          {
                              Id = 40,
                              Name = " الرابع",
                              DepartmentsId = 9,
                          },
                                new SubDepartments
                                {
                                    Id = 41,
                                    Name = " الاول",
                                    DepartmentsId = 10,
                                },
                      new SubDepartments
                      {
                          Id = 42,
                          Name = " الثاني",
                          DepartmentsId = 10,
                      },
                        new SubDepartments
                        {
                            Id = 43,
                            Name = " الثالث",
                            DepartmentsId = 10,
                        },
                          new SubDepartments
                          {
                              Id = 44,
                              Name = " الرابع",
                              DepartmentsId = 10,
                          },
                                new SubDepartments
                                {
                                    Id = 45,
                                    Name = " الاول",
                                    DepartmentsId = 11,
                                },
                      new SubDepartments
                      {
                          Id = 46,
                          Name = " الثاني",
                          DepartmentsId = 11,
                      },
                        new SubDepartments
                        {
                            Id = 47,
                            Name = " الثالث",
                            DepartmentsId = 11,
                        },
                          new SubDepartments
                          {
                              Id = 48,
                              Name = " الرابع",
                              DepartmentsId = 11,
                          },
                                new SubDepartments
                                {
                                    Id = 49,
                                    Name = " الاول",
                                    DepartmentsId = 12,
                                },
                      new SubDepartments
                      {
                          Id = 50,
                          Name = " الثاني",
                          DepartmentsId = 12,
                      },
                        new SubDepartments
                        {
                            Id = 51,
                            Name = " الثالث",
                            DepartmentsId = 12,
                        },
                          new SubDepartments
                          {
                              Id = 52,
                              Name = " الرابع",
                              DepartmentsId = 12,
                          },
                                new SubDepartments
                                {
                                    Id = 53,
                                    Name = " الاول",
                                    DepartmentsId = 13,
                                },
                      new SubDepartments
                      {
                          Id = 54,
                          Name = " الثاني",
                          DepartmentsId = 13,
                      },
                        new SubDepartments
                        {
                            Id = 55,
                            Name = " الثالث",
                            DepartmentsId = 13,
                        },
                          new SubDepartments
                          {
                              Id = 56,
                              Name = " الرابع",
                              DepartmentsId = 13,
                          },
                                new SubDepartments
                                {
                                    Id = 57,
                                    Name = " الاول",
                                    DepartmentsId = 14,
                                },
                      new SubDepartments
                      {
                          Id = 58,
                          Name = " الثاني",
                          DepartmentsId = 14,
                      },
                        new SubDepartments
                        {
                            Id = 59,
                            Name = " الثالث",
                            DepartmentsId = 14,
                        },
                          new SubDepartments
                          {
                              Id = 60,
                              Name = " الرابع",
                              DepartmentsId = 14,
                          },
                                new SubDepartments
                                {
                                    Id = 61,
                                    Name = " الاول",
                                    DepartmentsId = 15,
                                },
                      new SubDepartments
                      {
                          Id = 62,
                          Name = " الثاني",
                          DepartmentsId = 15,
                      },
                        new SubDepartments
                        {
                            Id = 63,
                            Name = " الثالث",
                            DepartmentsId = 15,
                        },
                          new SubDepartments
                          {
                              Id = 64,
                              Name = " الرابع",
                              DepartmentsId = 15,
                          },
                                new SubDepartments
                                {
                                    Id = 65,
                                    Name = " الاول",
                                    DepartmentsId = 16,
                                },
                      new SubDepartments
                      {
                          Id = 66,
                          Name = " الثاني",
                          DepartmentsId = 16,
                      },
                        new SubDepartments
                        {
                            Id = 67,
                            Name = " الثالث",
                            DepartmentsId = 16,
                        },
                          new SubDepartments
                          {
                              Id = 68,
                              Name = " الرابع",
                              DepartmentsId = 16,
                          },
                                new SubDepartments
                                {
                                    Id = 69,
                                    Name = " الاول",
                                    DepartmentsId = 17,
                                },
                      new SubDepartments
                      {
                          Id = 70,
                          Name = " الثاني",
                          DepartmentsId = 17,
                      },
                        new SubDepartments
                        {
                            Id = 71,
                            Name = " الثالث",
                            DepartmentsId = 17,
                        },
                          new SubDepartments
                          {
                              Id = 72,
                              Name = " الرابع",
                              DepartmentsId = 17,
                          },
                                new SubDepartments
                                {
                                    Id = 73,
                                    Name = " الاول",
                                    DepartmentsId = 18,
                                },
                      new SubDepartments
                      {
                          Id = 74,
                          Name = " الثاني",
                          DepartmentsId = 18,
                      },
                        new SubDepartments
                        {
                            Id = 75,
                            Name = " الثالث",
                            DepartmentsId = 18,
                        },
                          new SubDepartments
                          {
                              Id = 76,
                              Name = " الرابع",
                              DepartmentsId = 18,
                          },
                                new SubDepartments
                                {
                                    Id = 77,
                                    Name = " الاول",
                                    DepartmentsId = 19,
                                },
                      new SubDepartments
                      {
                          Id = 78,
                          Name = " الثاني",
                          DepartmentsId = 19,
                      },
                        new SubDepartments
                        {
                            Id = 79,
                            Name = " الثالث",
                            DepartmentsId = 19,
                        },
                          new SubDepartments
                          {
                              Id = 80,
                              Name = " الرابع",
                              DepartmentsId = 19,
                          },
                                new SubDepartments
                                {
                                    Id = 81,
                                    Name = " الاول",
                                    DepartmentsId = 20,
                                },
                      new SubDepartments
                      {
                          Id = 82,
                          Name = " الثاني",
                          DepartmentsId = 20,
                      },
                        new SubDepartments
                        {
                            Id = 83,
                            Name = " الثالث",
                            DepartmentsId = 20,
                        },
                          new SubDepartments
                          {
                              Id = 84,
                              Name = " الرابع",
                              DepartmentsId = 20,
                          },
                          
                                new SubDepartments
                                {
                                    Id = 85,
                                    Name = " الاول",
                                    DepartmentsId = 21,
                                },
                      new SubDepartments
                      {
                          Id = 86,
                          Name = " الثاني",
                          DepartmentsId = 21,
                      },
                        new SubDepartments
                        {
                            Id = 87,
                            Name = " الثالث",
                            DepartmentsId = 21,
                        },
                          new SubDepartments
                          {
                              Id = 88,
                              Name = " الرابع",
                              DepartmentsId = 21,
                          },
                          
                                new SubDepartments
                                {
                                    Id = 89,
                                    Name = " الاول",
                                    DepartmentsId = 22,
                                },
                      new SubDepartments
                      {
                          Id = 90,
                          Name = " الثاني",
                          DepartmentsId = 22,
                      },
                        new SubDepartments
                        {
                            Id = 91,
                            Name = " الثالث",
                            DepartmentsId = 22,
                        },
                          new SubDepartments
                          {
                              Id = 92,
                              Name = " الرابع",
                              DepartmentsId = 22,
                          },
                                new SubDepartments
                                {
                                    Id = 93,
                                    Name = " الاول",
                                    DepartmentsId = 23,
                                },
                      new SubDepartments
                      {
                          Id = 94,
                          Name = " الثاني",
                          DepartmentsId = 23,
                      },
                        new SubDepartments
                        {
                            Id = 95,
                            Name = " الثالث",
                            DepartmentsId = 23,
                        },
                          new SubDepartments
                          {
                              Id = 96,
                              Name = " الرابع",
                              DepartmentsId = 23,
                          },
                                new SubDepartments
                                {
                                    Id = 97,
                                    Name = " الاول",
                                    DepartmentsId = 24,
                                },
                      new SubDepartments
                      {
                          Id = 98,
                          Name = " الثاني",
                          DepartmentsId = 24,
                      },
                        new SubDepartments
                        {
                            Id = 99,
                            Name = " الثالث",
                            DepartmentsId = 24,
                        },
                          new SubDepartments
                          {
                              Id = 100,
                              Name = " الرابع",
                              DepartmentsId = 24,
                          },

                                new SubDepartments
                                {
                                    Id = 101,
                                    Name = " الاول",
                                    DepartmentsId = 25,
                                },
                      new SubDepartments
                      {
                          Id = 102,
                          Name = " الثاني",
                          DepartmentsId = 25,
                      },
                        new SubDepartments
                        {
                            Id = 103,
                            Name = " الثالث",
                            DepartmentsId = 25,
                        },
                          new SubDepartments
                          {
                              Id = 104,
                              Name = " الرابع",
                              DepartmentsId = 25,
                          },
                           new SubDepartments
                           {
                               Id = 105,
                               Name = " الخامس",
                               DepartmentsId = 5,
                           },
                        new SubDepartments
                        {
                            Id = 106,
                            Name = " الخامس",
                            DepartmentsId = 6,
                        }






                );
        }
    }
}
