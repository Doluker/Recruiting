using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Recruiting.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // "ParseJobStatuses"."Id" uses a hand-made sequence (with a typo in its name) instead of
            // an identity column, so the sequence has to be created before the table that references it.
            migrationBuilder.Sql("""CREATE SEQUENCE IF NOT EXISTS "ParceJobStatuses_Id_seq" AS integer START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1;""");

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Cities_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Currencies_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Educations_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FunnelStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FunnelStatuses_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LetterKinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("LetterKinds_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LetterStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("LetterStatuses_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParseJobStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"ParceJobStatuses_Id_seq\"'::regclass)"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ParseJobStatuses_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PriceMonthly = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    MaxUploadsPerMonth = table.Column<int>(type: "integer", nullable: true),
                    MaxActiveVacancies = table.Column<int>(type: "integer", nullable: true),
                    CanParse = table.Column<bool>(type: "boolean", nullable: false),
                    CanExportExcel = table.Column<bool>(type: "boolean", nullable: false),
                    HasFullDashboard = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Plans_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Positions_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResumeContentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ResumeContentTypes_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Roles_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Skills_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sources_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VacancyStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("VacancyStatuses_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Users_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "Users_RoleId_fkey",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    Action = table.Column<string>(type: "text", nullable: false),
                    Entity = table.Column<string>(type: "text", nullable: true),
                    EntityId = table.Column<int>(type: "integer", nullable: true),
                    Details = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ActivityLogs_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "ActivityLogs_UserId_fkey",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    Surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Patronymic = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CityId = table.Column<int>(type: "integer", nullable: true),
                    PositionId = table.Column<int>(type: "integer", nullable: true),
                    ExperienceYears = table.Column<int>(type: "integer", nullable: true),
                    EducationId = table.Column<int>(type: "integer", nullable: true),
                    SourceId = table.Column<int>(type: "integer", nullable: false),
                    ExternalId = table.Column<string>(type: "text", nullable: true),
                    SourceUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Candidates_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "Candidates_CityId_fkey",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Candidates_EducationId_fkey",
                        column: x => x.EducationId,
                        principalTable: "Educations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Candidates_OwnerId_fkey",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Candidates_PositionId_fkey",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    PlanId = table.Column<int>(type: "integer", nullable: false),
                    ProviderPaymentId = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    CurrencyId = table.Column<int>(type: "integer", nullable: false),
                    PaymentStatusId = table.Column<int>(type: "integer", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Payments_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "Payments_CurrencyId_fkey",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Payments_PlanId_fkey",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Payments_UserId_fkey",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    PlanId = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Subscriptions_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "Subscriptions_PlanId_fkey",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Subscriptions_UserId_fkey",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    HhTokenEncrypted = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("UserSettings_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "UserSettings_UserId_fkey",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vacancies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SourceId = table.Column<int>(type: "integer", nullable: false),
                    ExternalId = table.Column<string>(type: "text", nullable: true),
                    SourceUrl = table.Column<string>(type: "text", nullable: true),
                    CityId = table.Column<int>(type: "integer", nullable: true),
                    SalaryFrom = table.Column<int>(type: "integer", nullable: true),
                    SalaryTo = table.Column<int>(type: "integer", nullable: true),
                    ExperienceYears = table.Column<int>(type: "integer", nullable: true),
                    EducationId = table.Column<int>(type: "integer", nullable: true),
                    VacancyStatusId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Vacancies_pkey", x => x.Id);
                    table.CheckConstraint("CK_Vacancies_Salary", "(\"SalaryFrom\" IS NULL) OR (\"SalaryTo\" IS NULL) OR (\"SalaryFrom\" <= \"SalaryTo\")");
                    table.ForeignKey(
                        name: "Vacancies_CityId_fkey",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Vacancies_EducationId_fkey",
                        column: x => x.EducationId,
                        principalTable: "Educations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Vacancies_OwnerId_fkey",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Vacancies_SourceId_fkey",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Vacancies_VacancyStatusId_fkey",
                        column: x => x.VacancyStatusId,
                        principalTable: "VacancyStatuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CandidateSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CandidateId = table.Column<int>(type: "integer", nullable: false),
                    SkillId = table.Column<int>(type: "integer", nullable: false),
                    Years = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CandidateSkills_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "CandidateSkills_CandidateId_fkey",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "CandidateSkills_SkillId_fkey",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    CandidateId = table.Column<int>(type: "integer", nullable: true),
                    ObjectKey = table.Column<string>(type: "text", nullable: false),
                    OriginalFileName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ContentTypeId = table.Column<int>(type: "integer", nullable: false),
                    FileHash = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    ExtractedText = table.Column<string>(type: "text", nullable: true),
                    ParsedJsonKey = table.Column<string>(type: "text", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ResumeFiles_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "ResumeFiles_CandidateId_fkey",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "ResumeFiles_ContentTypeId_fkey",
                        column: x => x.ContentTypeId,
                        principalTable: "ResumeContentTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "ResumeFiles_OwnerId_fkey",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VacancyId = table.Column<int>(type: "integer", nullable: false),
                    CandidateId = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    SkillsScore = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    ExperienceScore = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    EducationScore = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    ApplicationStatusId = table.Column<int>(type: "integer", nullable: false),
                    RejectReason = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Applications_pkey", x => x.Id);
                    table.CheckConstraint("CK_Applications_Score", "(\"Score\" IS NULL) OR ((\"Score\" >= 0) AND (\"Score\" <= 100))");
                    table.ForeignKey(
                        name: "Applications_ApplicationStatusId_fkey",
                        column: x => x.ApplicationStatusId,
                        principalTable: "FunnelStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Applications_CandidateId_fkey",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Applications_VacancyId_fkey",
                        column: x => x.VacancyId,
                        principalTable: "Vacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacancySkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VacancyId = table.Column<int>(type: "integer", nullable: false),
                    SkillId = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("VacancySkills_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "VacancySkills_SkillId_fkey",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "VacancySkills_VacancyId_fkey",
                        column: x => x.VacancyId,
                        principalTable: "Vacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParseJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResumeFileId = table.Column<int>(type: "integer", nullable: false),
                    ParseJobStatusId = table.Column<int>(type: "integer", nullable: false),
                    Attempts = table.Column<int>(type: "integer", nullable: false),
                    Error = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LockedUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ParseJobs_pkey", x => x.Id);
                    table.CheckConstraint("CK_ParseJobs_Attempts", "\"Attempts\" >= 0");
                    table.ForeignKey(
                        name: "ParseJobs_ParseJobStatusId_fkey",
                        column: x => x.ParseJobStatusId,
                        principalTable: "ParseJobStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "ParseJobs_ResumeFileId_fkey",
                        column: x => x.ResumeFileId,
                        principalTable: "ResumeFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Letters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    KindId = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Letters_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "Letters_ApplicationId_fkey",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Letters_KindId_fkey",
                        column: x => x.KindId,
                        principalTable: "LetterKinds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Letters_StatusId_fkey",
                        column: x => x.StatusId,
                        principalTable: "LetterStatuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StatusHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    FromStatusId = table.Column<int>(type: "integer", nullable: true),
                    ToStatusId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("StatusHistory_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "StatusHistory_ApplicationId_fkey",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "StatusHistory_FromStatusId_fkey",
                        column: x => x.FromStatusId,
                        principalTable: "FunnelStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "StatusHistory_ToStatusId_fkey",
                        column: x => x.ToStatusId,
                        principalTable: "FunnelStatuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_UserId",
                table: "ActivityLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "Applications_VacancyId_CandidateId_key",
                table: "Applications",
                columns: new[] { "VacancyId", "CandidateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicationStatusId",
                table: "Applications",
                column: "ApplicationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Candidate",
                table: "Applications",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Vacancy",
                table: "Applications",
                columns: new[] { "VacancyId", "ApplicationStatusId" });

            migrationBuilder.CreateIndex(
                name: "Candidates_OwnerId_SourceId_ExternalId_key",
                table: "Candidates",
                columns: new[] { "OwnerId", "SourceId", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_CityId",
                table: "Candidates",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_EducationId",
                table: "Candidates",
                column: "EducationId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_Owner",
                table: "Candidates",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_PositionId",
                table: "Candidates",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "CandidateSkills_CandidateId_SkillId_key",
                table: "CandidateSkills",
                columns: new[] { "CandidateId", "SkillId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateSkills_Skill",
                table: "CandidateSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Letters_Application",
                table: "Letters",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Letters_KindId",
                table: "Letters",
                column: "KindId");

            migrationBuilder.CreateIndex(
                name: "IX_Letters_StatusId",
                table: "Letters",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ParseJobs_Queue",
                table: "ParseJobs",
                columns: new[] { "ParseJobStatusId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "UQ_ParseJobs_Active",
                table: "ParseJobs",
                column: "ResumeFileId",
                unique: true,
                filter: "(\"FinishedAt\" IS NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CurrencyId",
                table: "Payments",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PlanId",
                table: "Payments",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "Payments_ProviderPaymentId_key",
                table: "Payments",
                column: "ProviderPaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeFiles_CandidateId",
                table: "ResumeFiles",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeFiles_ContentTypeId",
                table: "ResumeFiles",
                column: "ContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeFiles_Owner",
                table: "ResumeFiles",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "ResumeFiles_OwnerId_FileHash_key",
                table: "ResumeFiles",
                columns: new[] { "OwnerId", "FileHash" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatusHistory_App",
                table: "StatusHistory",
                columns: new[] { "ApplicationId", "ChangedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_StatusHistory_FromStatusId",
                table: "StatusHistory",
                column: "FromStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusHistory_ToStatusId",
                table: "StatusHistory",
                column: "ToStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PlanId",
                table: "Subscriptions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "UQ_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_CityId",
                table: "Vacancies",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_EducationId",
                table: "Vacancies",
                column: "EducationId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_Owner",
                table: "Vacancies",
                columns: new[] { "OwnerId", "VacancyStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_SourceId",
                table: "Vacancies",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_VacancyStatusId",
                table: "Vacancies",
                column: "VacancyStatusId");

            migrationBuilder.CreateIndex(
                name: "Vacancies_OwnerId_SourceId_ExternalId_key",
                table: "Vacancies",
                columns: new[] { "OwnerId", "SourceId", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VacancySkills_Skill",
                table: "VacancySkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "VacancySkills_VacancyId_SkillId_key",
                table: "VacancySkills",
                columns: new[] { "VacancyId", "SkillId" },
                unique: true);

            CreateExpressionIndexes(migrationBuilder);

            migrationBuilder.Sql("""ALTER SEQUENCE "ParceJobStatuses_Id_seq" OWNED BY "ParseJobStatuses"."Id";""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            DropExpressionIndexes(migrationBuilder);

            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "CandidateSkills");

            migrationBuilder.DropTable(
                name: "Letters");

            migrationBuilder.DropTable(
                name: "ParseJobs");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "StatusHistory");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "VacancySkills");

            migrationBuilder.DropTable(
                name: "LetterKinds");

            migrationBuilder.DropTable(
                name: "LetterStatuses");

            migrationBuilder.DropTable(
                name: "ParseJobStatuses");

            migrationBuilder.DropTable(
                name: "ResumeFiles");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "ResumeContentTypes");

            migrationBuilder.DropTable(
                name: "FunnelStatuses");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Vacancies");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "VacancyStatuses");

            migrationBuilder.DropTable(
                name: "Roles");
        }

        /// <summary>
        /// Expression (functional) unique indexes are not supported by the EF Core model,
        /// so they are created with raw SQL. lower()/upper() make the uniqueness case-insensitive.
        /// EF Core doesn't know about them, so it never tries to recreate or drop them on its own.
        /// </summary>
        private static void CreateExpressionIndexes(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_Cities_Name" ON "Cities" (lower("Name"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_Currencies_Code" ON "Currencies" (upper("Code"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_Educations_Name" ON "Educations" (lower("Name"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_FunnelStatuses_Name" ON "FunnelStatuses" (lower("Name"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_LetterKinds_Name" ON "LetterKinds" (lower("Name"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_LetterStatuses_Name" ON "LetterStatuses" (lower("Name"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_ParseJobStatuses_Name" ON "ParseJobStatuses" (lower("Name"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_Plans_Name" ON "Plans" (lower("Name"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_Positions_Name" ON "Positions" (lower("Name"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_ResumeContentTypes_Name" ON "ResumeContentTypes" (lower("Name"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_Roles_Name" ON "Roles" (lower("Name"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_Skills_Name" ON "Skills" (lower("Name"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_Sources_Name" ON "Sources" (lower("Name"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_Users_Email" ON "Users" (lower("Email"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_Users_UserName" ON "Users" (lower("UserName"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_VacancyStatuses_Name" ON "VacancyStatuses" (lower("Name"));""");
        }

        /// <summary>
        /// Drops the expression indexes created by <see cref="CreateExpressionIndexes"/>.
        /// </summary>
        private static void DropExpressionIndexes(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_Cities_Name";""");
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_Currencies_Code";""");
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_Educations_Name";""");
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_FunnelStatuses_Name";""");
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_LetterKinds_Name";""");
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_LetterStatuses_Name";""");
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_ParseJobStatuses_Name";""");
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_Plans_Name";""");
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_Positions_Name";""");
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_ResumeContentTypes_Name";""");
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_Roles_Name";""");
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_Skills_Name";""");
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_Sources_Name";""");
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_Users_Email";""");
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_Users_UserName";""");
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_VacancyStatuses_Name";""");
        }
    }
}
