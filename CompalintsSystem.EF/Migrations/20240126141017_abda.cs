using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompalintsSystem.EF.Migrations
{
    public partial class abda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.CreateTable(
                name: "Collegess",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collegess", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComplatinClassfactions",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplatinClassfactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proposals",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitileProposal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescProposal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateSubmeted = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "تحديد وقت ادخال الصف في قاعدية البيانات"),
                    BeneficiarieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Societys",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Societys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StagesComplaints",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StagesComplaints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusCompalints",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusCompalints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeCommunications",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "varchar(150)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsersNameAddType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeCommunications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeComplaints",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "varchar(150)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsersNameAddType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeComplaints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departmentss",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollegesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departmentss", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departmentss_Collegess_CollegesId",
                        column: x => x.CollegesId,
                        principalSchema: "Identity",
                        principalTable: "Collegess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubDepartmentss",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDepartmentss", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubDepartmentss_Departmentss_DepartmentsId",
                        column: x => x.DepartmentsId,
                        principalSchema: "Identity",
                        principalTable: "Departmentss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollegesId = table.Column<int>(type: "int", nullable: false),
                    DepartmentsId = table.Column<int>(type: "int", nullable: false),
                    SubDepartmentsId = table.Column<int>(type: "int", nullable: false),
                    SocietyId = table.Column<int>(type: "int", nullable: true),
                    ProfilePicture = table.Column<string>(type: "varchar(250)", nullable: true),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    originatorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserRoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Collegess_CollegesId",
                        column: x => x.CollegesId,
                        principalSchema: "Identity",
                        principalTable: "Collegess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Departmentss_DepartmentsId",
                        column: x => x.DepartmentsId,
                        principalSchema: "Identity",
                        principalTable: "Departmentss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Societys_SocietyId",
                        column: x => x.SocietyId,
                        principalSchema: "Identity",
                        principalTable: "Societys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_SubDepartmentss_SubDepartmentsId",
                        column: x => x.SubDepartmentsId,
                        principalSchema: "Identity",
                        principalTable: "SubDepartmentss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_User_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "Identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UploadsComplaintes",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleComplaint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeComplaintId = table.Column<int>(type: "int", nullable: false),
                    DescComplaint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SocietyId = table.Column<int>(type: "int", nullable: true),
                    StatusCompalintId = table.Column<int>(type: "int", nullable: false),
                    StagesComplaintId = table.Column<int>(type: "int", nullable: false),
                    PropBeneficiarie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollegesId = table.Column<int>(type: "int", nullable: false),
                    DepartmentsId = table.Column<int>(type: "int", nullable: false),
                    SubDepartmentsId = table.Column<int>(type: "int", nullable: false),
                    ComplatinClassfactionId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserRoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Today = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReturnedTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadsComplaintes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadsComplaintes_Collegess_CollegesId",
                        column: x => x.CollegesId,
                        principalSchema: "Identity",
                        principalTable: "Collegess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UploadsComplaintes_ComplatinClassfactions_ComplatinClassfactionId",
                        column: x => x.ComplatinClassfactionId,
                        principalSchema: "Identity",
                        principalTable: "ComplatinClassfactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UploadsComplaintes_Departmentss_DepartmentsId",
                        column: x => x.DepartmentsId,
                        principalSchema: "Identity",
                        principalTable: "Departmentss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UploadsComplaintes_Societys_SocietyId",
                        column: x => x.SocietyId,
                        principalSchema: "Identity",
                        principalTable: "Societys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UploadsComplaintes_StagesComplaints_StagesComplaintId",
                        column: x => x.StagesComplaintId,
                        principalSchema: "Identity",
                        principalTable: "StagesComplaints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UploadsComplaintes_StatusCompalints_StatusCompalintId",
                        column: x => x.StatusCompalintId,
                        principalSchema: "Identity",
                        principalTable: "StatusCompalints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UploadsComplaintes_SubDepartmentss_SubDepartmentsId",
                        column: x => x.SubDepartmentsId,
                        principalSchema: "Identity",
                        principalTable: "SubDepartmentss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UploadsComplaintes_TypeComplaints_TypeComplaintId",
                        column: x => x.TypeComplaintId,
                        principalSchema: "Identity",
                        principalTable: "TypeComplaints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UploadsComplaintes_User_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersCommunications",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reportSubmitterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    reportSubmitterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reporteeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BenfPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollegesId = table.Column<int>(type: "int", nullable: false),
                    DepartmentsId = table.Column<int>(type: "int", nullable: false),
                    SubDepartmentsId = table.Column<int>(type: "int", nullable: false),
                    TypeCommuncationId = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Titile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersCommunications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersCommunications_Collegess_CollegesId",
                        column: x => x.CollegesId,
                        principalSchema: "Identity",
                        principalTable: "Collegess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersCommunications_Departmentss_DepartmentsId",
                        column: x => x.DepartmentsId,
                        principalSchema: "Identity",
                        principalTable: "Departmentss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersCommunications_SubDepartmentss_SubDepartmentsId",
                        column: x => x.SubDepartmentsId,
                        principalSchema: "Identity",
                        principalTable: "SubDepartmentss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersCommunications_TypeCommunications_TypeCommuncationId",
                        column: x => x.TypeCommuncationId,
                        principalSchema: "Identity",
                        principalTable: "TypeCommunications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersCommunications_User_reportSubmitterId",
                        column: x => x.reportSubmitterId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Compalints_Solutions",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UploadsComplainteId = table.Column<int>(type: "int", nullable: false),
                    SolutionProvName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolutionProvIdentity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAccept = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonWhySolutionRejected = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentSolution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateSolution = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compalints_Solutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compalints_Solutions_UploadsComplaintes_UploadsComplainteId",
                        column: x => x.UploadsComplainteId,
                        principalSchema: "Identity",
                        principalTable: "UploadsComplaintes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Compalints_Solutions_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComplaintsRejecteds",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UploadsComplainteId = table.Column<int>(type: "int", nullable: false),
                    RejectedProvName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectedUserProvIdentity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateSolution = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintsRejecteds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplaintsRejecteds_UploadsComplaintes_UploadsComplainteId",
                        column: x => x.UploadsComplainteId,
                        principalSchema: "Identity",
                        principalTable: "UploadsComplaintes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComplaintsRejecteds_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UpComplaintCauses",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UploadsComplainteId = table.Column<int>(type: "int", nullable: false),
                    UpProvName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpUserProvIdentity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cause = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateUp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpComplaintCauses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpComplaintCauses_UploadsComplaintes_UploadsComplainteId",
                        column: x => x.UploadsComplainteId,
                        principalSchema: "Identity",
                        principalTable: "UploadsComplaintes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UpComplaintCauses_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "AspNetRoles",
                columns: new[] { "Id", "ApplicationUserId", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8faa31eb-ded0-4711-8e0b-0f509c4b332f", null, "93efadaa-2a40-4a8a-a737-64ef3168a10e", "Beneficiarie", "BENEFICIARIE" },
                    { "81b7a93d-4221-4d50-884c-08d98676a9c8", null, "973b7325-e112-4d48-81f5-80f19cee9792", "AdminDepartments", "ADMINDEPARTMENTS" },
                    { "7f95d3fd-8840-466e-9da6-d7dcf06298de", null, "103efdba-ff1b-4bb9-8b4d-8f6a26946f02", "AdminColleges", "ADMINCOLLEGES" },
                    { "2e803883-c018-4f7c-90e3-c3c1db0d8f00", null, "ecae0872-d9db-4ead-96bb-a691de047b0e", "AdminGeneralFederation", "ADMINGENERALFEDERATION" },
                    { "64a8ff4a-f4a2-405c-9c1a-85f0f9f46145", null, "27fd1341-dba0-450a-ae71-7e602c28d9b6", "AdminSubDepartments", "ADMINSUBDEPARTMENTS" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Collegess",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "كلية الطب العلوم الصحية " },
                    { 2, " كلية الهندسة وتقنية المعلومات" },
                    { 3, " كلية التجارة والاقتصاد" },
                    { 4, "كلية العلوم التطبيقية والانسانية " },
                    { 5, "كلية الاعمال باللغة الانجليزية" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "ComplatinClassfactions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "متوسطة" },
                    { 3, "منخفضة" },
                    { 1, "عالية" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Societys",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "الملتقى" },
                    { 2, " موظف الشكوى" },
                    { 3, "ادارة عامة" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "StagesComplaints",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "موظف الشكوى" },
                    { 2, "رئيس القسم" },
                    { 3, "عميد الكلية" },
                    { 4, " الادارة العامة" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "StatusCompalints",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 4, "مغلقة" },
                    { 5, "مرفوعة" },
                    { 1, "جديدة" },
                    { 2, "محلولة" },
                    { 3, "مرفوضة" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "TypeCommunications",
                columns: new[] { "Id", "CreatedDate", "Type", "UserId", "UsersNameAddType" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 26, 17, 10, 15, 929, DateTimeKind.Local).AddTicks(1008), "بلاغ أكاديمي", null, "قيمة افتراضية من النضام" },
                    { 2, new DateTime(2024, 1, 26, 17, 10, 15, 929, DateTimeKind.Local).AddTicks(1023), "بلاغ إداري", null, "قيمة افتراضية من النضام" },
                    { 7, new DateTime(2024, 1, 26, 17, 10, 15, 929, DateTimeKind.Local).AddTicks(1043), "بلاغ تحرش", null, "قيمة افتراضية من النضام" },
                    { 5, new DateTime(2024, 1, 26, 17, 10, 15, 929, DateTimeKind.Local).AddTicks(1039), "بلاغ أمان وصحة", null, "قيمة افتراضية من النضام" },
                    { 4, new DateTime(2024, 1, 26, 17, 10, 15, 929, DateTimeKind.Local).AddTicks(1034), "بلاغ تمييز", null, "قيمة افتراضية من النضام" },
                    { 3, new DateTime(2024, 1, 26, 17, 10, 15, 929, DateTimeKind.Local).AddTicks(1029), "بلاغ تأديبي", null, "قيمة افتراضية من النضام" },
                    { 6, new DateTime(2024, 1, 26, 17, 10, 15, 929, DateTimeKind.Local).AddTicks(1048), "تلاعب بالحلول", null, "قيمة افتراضية من النضام" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "TypeComplaints",
                columns: new[] { "Id", "CreatedDate", "Type", "UserId", "UsersNameAddType" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 26, 17, 10, 15, 930, DateTimeKind.Local).AddTicks(280), "شكاوى أكاديمية", null, "قيمة افتراضية من النضام" },
                    { 2, new DateTime(2024, 1, 26, 17, 10, 15, 930, DateTimeKind.Local).AddTicks(1043), "شكاوى إدارية", null, "قيمة افتراضية من النضام" },
                    { 4, new DateTime(2024, 1, 26, 17, 10, 15, 930, DateTimeKind.Local).AddTicks(1054), "شكاوى تمييز وتحيز", null, "قيمة افتراضية من النضام" },
                    { 5, new DateTime(2024, 1, 26, 17, 10, 15, 930, DateTimeKind.Local).AddTicks(1060), "شكاوى سلوك غير لائق", null, "قيمة افتراضية من النضام" },
                    { 6, new DateTime(2024, 1, 26, 17, 10, 15, 930, DateTimeKind.Local).AddTicks(1065), "شكاوى الأمان والسلامة", null, "قيمة افتراضية من النضام" },
                    { 3, new DateTime(2024, 1, 26, 17, 10, 15, 930, DateTimeKind.Local).AddTicks(1049), "شكاوى تقنية", null, "قيمة افتراضية من النضام" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Departmentss",
                columns: new[] { "Id", "CollegesId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "طب بشري" },
                    { 22, 4, "ترجمة" },
                    { 21, 4, "جغرافيا_تاريخ" },
                    { 20, 4, "رياضيات" },
                    { 19, 4, "فيزياء" },
                    { 18, 4, "كيمياء" },
                    { 25, 4, "دراسات عربية" },
                    { 17, 4, "لغة انجليزي" },
                    { 16, 4, "قران كريم وعلومة _علوم الحياة" },
                    { 15, 4, "علوم زراعية" },
                    { 14, 4, "الشرعة والقانون" },
                    { 23, 5, "محاسبة وتمويل" },
                    { 13, 3, "ادارة اعمال دولية" },
                    { 11, 3, "ادارة اعمال " },
                    { 10, 3, "محاسبة" },
                    { 9, 2, "الامن السيبراني والشبكات" },
                    { 8, 2, " نظم معلومات حاسوبية" },
                    { 7, 2, "علوم حاسوب" },
                    { 6, 2, "هندسة معماري" },
                    { 5, 2, "هندسة مدني " },
                    { 4, 1, "تمريض" },
                    { 3, 1, "مختبرات" },
                    { 2, 1, "صيدلة" },
                    { 12, 3, "علوم مالية ومصرفية" },
                    { 24, 5, "ادارة اعمال دولية" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "SubDepartmentss",
                columns: new[] { "Id", "DepartmentsId", "Name" },
                values: new object[,]
                {
                    { 1, 1, " الاول" },
                    { 103, 25, " الثالث" },
                    { 102, 25, " الثاني" },
                    { 101, 25, " الاول" },
                    { 72, 17, " الرابع" },
                    { 71, 17, " الثالث" },
                    { 70, 17, " الثاني" },
                    { 69, 17, " الاول" },
                    { 68, 16, " الرابع" },
                    { 67, 16, " الثالث" },
                    { 66, 16, " الثاني" },
                    { 104, 25, " الرابع" },
                    { 65, 16, " الاول" },
                    { 63, 15, " الثالث" },
                    { 62, 15, " الثاني" },
                    { 61, 15, " الاول" },
                    { 60, 14, " الرابع" },
                    { 59, 14, " الثالث" },
                    { 58, 14, " الثاني" },
                    { 57, 14, " الاول" },
                    { 56, 13, " الرابع" },
                    { 55, 13, " الثالث" },
                    { 54, 13, " الثاني" },
                    { 64, 15, " الرابع" },
                    { 73, 18, " الاول" },
                    { 74, 18, " الثاني" },
                    { 75, 18, " الثالث" },
                    { 98, 24, " الثاني" },
                    { 97, 24, " الاول" },
                    { 96, 23, " الرابع" },
                    { 95, 23, " الثالث" },
                    { 94, 23, " الثاني" },
                    { 93, 23, " الاول" },
                    { 92, 22, " الرابع" },
                    { 91, 22, " الثالث" },
                    { 90, 22, " الثاني" },
                    { 89, 22, " الاول" },
                    { 88, 21, " الرابع" },
                    { 87, 21, " الثالث" },
                    { 86, 21, " الثاني" },
                    { 85, 21, " الاول" },
                    { 84, 20, " الرابع" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "SubDepartmentss",
                columns: new[] { "Id", "DepartmentsId", "Name" },
                values: new object[,]
                {
                    { 83, 20, " الثالث" },
                    { 82, 20, " الثاني" },
                    { 81, 20, " الاول" },
                    { 80, 19, " الرابع" },
                    { 79, 19, " الثالث" },
                    { 78, 19, " الثاني" },
                    { 77, 19, " الاول" },
                    { 76, 18, " الرابع" },
                    { 53, 13, " الاول" },
                    { 52, 12, " الرابع" },
                    { 51, 12, " الثالث" },
                    { 50, 12, " الثاني" },
                    { 24, 5, " الرابع" },
                    { 23, 5, " الثالث" },
                    { 22, 5, " الثاني" },
                    { 21, 5, " الاول" },
                    { 20, 4, " الرابع" },
                    { 19, 4, " الثالث" },
                    { 18, 4, " الثاني" },
                    { 17, 4, " الاول" },
                    { 16, 3, " الرابع" },
                    { 15, 3, " الثالث" },
                    { 14, 3, " الثاني" },
                    { 13, 3, " الاول" },
                    { 12, 2, " الخامس" },
                    { 11, 2, " الرابع" },
                    { 10, 2, " الثالث" },
                    { 9, 2, " الثاني" },
                    { 8, 2, " الاول" },
                    { 7, 1, " السابع" },
                    { 6, 1, " السادس" },
                    { 5, 1, " الخامس" },
                    { 4, 1, " الرابع" },
                    { 3, 1, " الثالث" },
                    { 2, 1, " الثاني" },
                    { 105, 5, " الخامس" },
                    { 99, 24, " الثالث" },
                    { 25, 6, " الاول" },
                    { 27, 6, " الثالث" },
                    { 49, 12, " الاول" },
                    { 48, 11, " الرابع" },
                    { 47, 11, " الثالث" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "SubDepartmentss",
                columns: new[] { "Id", "DepartmentsId", "Name" },
                values: new object[,]
                {
                    { 46, 11, " الثاني" },
                    { 45, 11, " الاول" },
                    { 44, 10, " الرابع" },
                    { 43, 10, " الثالث" },
                    { 42, 10, " الثاني" },
                    { 41, 10, " الاول" },
                    { 40, 9, " الرابع" },
                    { 39, 9, " الثالث" },
                    { 38, 9, " الثاني" },
                    { 37, 9, " الاول" },
                    { 36, 8, " الرابع" },
                    { 35, 8, " الثالث" },
                    { 34, 8, " الثاني" },
                    { 33, 8, " الاول" },
                    { 32, 7, " الرابع" },
                    { 31, 7, " الثالث" },
                    { 30, 7, " الثاني" },
                    { 29, 7, " الاول" },
                    { 106, 6, " الخامس" },
                    { 28, 6, " الرابع" },
                    { 26, 6, " الثاني" },
                    { 100, 24, " الرابع" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "Identity",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_ApplicationUserId",
                schema: "Identity",
                table: "AspNetRoles",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Identity",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "Identity",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "Identity",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "Identity",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Compalints_Solutions_UploadsComplainteId",
                schema: "Identity",
                table: "Compalints_Solutions",
                column: "UploadsComplainteId");

            migrationBuilder.CreateIndex(
                name: "IX_Compalints_Solutions_UserId",
                schema: "Identity",
                table: "Compalints_Solutions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintsRejecteds_UploadsComplainteId",
                schema: "Identity",
                table: "ComplaintsRejecteds",
                column: "UploadsComplainteId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintsRejecteds_UserId",
                schema: "Identity",
                table: "ComplaintsRejecteds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Departmentss_CollegesId",
                schema: "Identity",
                table: "Departmentss",
                column: "CollegesId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDepartmentss_DepartmentsId",
                schema: "Identity",
                table: "SubDepartmentss",
                column: "DepartmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_UpComplaintCauses_UploadsComplainteId",
                schema: "Identity",
                table: "UpComplaintCauses",
                column: "UploadsComplainteId");

            migrationBuilder.CreateIndex(
                name: "IX_UpComplaintCauses_UserId",
                schema: "Identity",
                table: "UpComplaintCauses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadsComplaintes_ApplicationUserId",
                schema: "Identity",
                table: "UploadsComplaintes",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadsComplaintes_CollegesId",
                schema: "Identity",
                table: "UploadsComplaintes",
                column: "CollegesId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadsComplaintes_ComplatinClassfactionId",
                schema: "Identity",
                table: "UploadsComplaintes",
                column: "ComplatinClassfactionId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadsComplaintes_DepartmentsId",
                schema: "Identity",
                table: "UploadsComplaintes",
                column: "DepartmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadsComplaintes_SocietyId",
                schema: "Identity",
                table: "UploadsComplaintes",
                column: "SocietyId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadsComplaintes_StagesComplaintId",
                schema: "Identity",
                table: "UploadsComplaintes",
                column: "StagesComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadsComplaintes_StatusCompalintId",
                schema: "Identity",
                table: "UploadsComplaintes",
                column: "StatusCompalintId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadsComplaintes_SubDepartmentsId",
                schema: "Identity",
                table: "UploadsComplaintes",
                column: "SubDepartmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadsComplaintes_TypeComplaintId",
                schema: "Identity",
                table: "UploadsComplaintes",
                column: "TypeComplaintId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Identity",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_User_CollegesId",
                schema: "Identity",
                table: "User",
                column: "CollegesId");

            migrationBuilder.CreateIndex(
                name: "IX_User_DepartmentsId",
                schema: "Identity",
                table: "User",
                column: "DepartmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_User_SocietyId",
                schema: "Identity",
                table: "User",
                column: "SocietyId");

            migrationBuilder.CreateIndex(
                name: "IX_User_SubDepartmentsId",
                schema: "Identity",
                table: "User",
                column: "SubDepartmentsId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Identity",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UsersCommunications_CollegesId",
                schema: "Identity",
                table: "UsersCommunications",
                column: "CollegesId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersCommunications_DepartmentsId",
                schema: "Identity",
                table: "UsersCommunications",
                column: "DepartmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersCommunications_reportSubmitterId",
                schema: "Identity",
                table: "UsersCommunications",
                column: "reportSubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersCommunications_SubDepartmentsId",
                schema: "Identity",
                table: "UsersCommunications",
                column: "SubDepartmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersCommunications_TypeCommuncationId",
                schema: "Identity",
                table: "UsersCommunications",
                column: "TypeCommuncationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Compalints_Solutions",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ComplaintsRejecteds",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Proposals",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UpComplaintCauses",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UsersCommunications",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UploadsComplaintes",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "TypeCommunications",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ComplatinClassfactions",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "StagesComplaints",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "StatusCompalints",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "TypeComplaints",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Societys",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "SubDepartmentss",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Departmentss",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Collegess",
                schema: "Identity");
        }
    }
}
