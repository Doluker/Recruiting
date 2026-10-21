using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recruiting.Migrations
{
    /// <inheritdoc />
    public partial class ShiftDictionaryIdsToOneBased : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── снимаем FK, ссылающиеся на справочники ───────────────────
            migrationBuilder.Sql("""ALTER TABLE "Users" DROP CONSTRAINT "Users_RoleId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Vacancies" DROP CONSTRAINT "Vacancies_VacancyStatusId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Vacancies" DROP CONSTRAINT "Vacancies_SourceId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Candidates" DROP CONSTRAINT "Candidates_SourceId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Applications" DROP CONSTRAINT "Applications_ApplicationStatusId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "StatusHistory" DROP CONSTRAINT "StatusHistory_FromStatusId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "StatusHistory" DROP CONSTRAINT "StatusHistory_ToStatusId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Letters" DROP CONSTRAINT "Letters_KindId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Letters" DROP CONSTRAINT "Letters_StatusId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "ParseJobs" DROP CONSTRAINT "ParseJobs_ParseJobStatusId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Payments" DROP CONSTRAINT "Payments_PlanId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Subscriptions" DROP CONSTRAINT "Subscriptions_PlanId_fkey";""");

            // ── снимаем PK: иначе UPDATE "+1" упадёт на уникальном индексе (23505) ──
            migrationBuilder.Sql("""ALTER TABLE "Roles" DROP CONSTRAINT "Roles_pkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Sources" DROP CONSTRAINT "Sources_pkey";""");
            migrationBuilder.Sql("""ALTER TABLE "VacancyStatuses" DROP CONSTRAINT "VacancyStatuses_pkey";""");
            migrationBuilder.Sql("""ALTER TABLE "FunnelStatuses" DROP CONSTRAINT "FunnelStatuses_pkey";""");
            migrationBuilder.Sql("""ALTER TABLE "LetterKinds" DROP CONSTRAINT "LetterKinds_pkey";""");
            migrationBuilder.Sql("""ALTER TABLE "LetterStatuses" DROP CONSTRAINT "LetterStatuses_pkey";""");
            migrationBuilder.Sql("""ALTER TABLE "ParseJobStatuses" DROP CONSTRAINT "ParseJobStatuses_pkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Plans" DROP CONSTRAINT "Plans_pkey";""");

            // ── сдвигаем справочники +1 ────────────────────────────────────
            migrationBuilder.Sql("""UPDATE "Roles" SET "Id" = "Id" + 1;""");
            migrationBuilder.Sql("""UPDATE "Sources" SET "Id" = "Id" + 1;""");
            migrationBuilder.Sql("""UPDATE "VacancyStatuses" SET "Id" = "Id" + 1;""");
            migrationBuilder.Sql("""UPDATE "FunnelStatuses" SET "Id" = "Id" + 1;""");
            migrationBuilder.Sql("""UPDATE "LetterKinds" SET "Id" = "Id" + 1;""");
            migrationBuilder.Sql("""UPDATE "LetterStatuses" SET "Id" = "Id" + 1;""");
            migrationBuilder.Sql("""UPDATE "ParseJobStatuses" SET "Id" = "Id" + 1;""");
            migrationBuilder.Sql("""UPDATE "Plans" SET "Id" = "Id" + 1;""");

            // ── сдвигаем ссылки +1 ──────────────────────────────────────────
            migrationBuilder.Sql("""UPDATE "Users" SET "RoleId" = "RoleId" + 1;""");
            migrationBuilder.Sql("""UPDATE "Vacancies" SET "VacancyStatusId" = "VacancyStatusId" + 1;""");
            migrationBuilder.Sql("""UPDATE "Vacancies" SET "SourceId" = "SourceId" + 1;""");
            migrationBuilder.Sql("""UPDATE "Candidates" SET "SourceId" = "SourceId" + 1;""");
            migrationBuilder.Sql("""UPDATE "Applications" SET "ApplicationStatusId" = "ApplicationStatusId" + 1;""");
            migrationBuilder.Sql("""UPDATE "StatusHistory" SET "FromStatusId" = "FromStatusId" + 1 WHERE "FromStatusId" IS NOT NULL;""");
            migrationBuilder.Sql("""UPDATE "StatusHistory" SET "ToStatusId" = "ToStatusId" + 1;""");
            migrationBuilder.Sql("""UPDATE "Letters" SET "KindId" = "KindId" + 1;""");
            migrationBuilder.Sql("""UPDATE "Letters" SET "StatusId" = "StatusId" + 1;""");
            migrationBuilder.Sql("""UPDATE "ParseJobs" SET "ParseJobStatusId" = "ParseJobStatusId" + 1;""");
            migrationBuilder.Sql("""UPDATE "Payments" SET "PlanId" = "PlanId" + 1;""");
            migrationBuilder.Sql("""UPDATE "Subscriptions" SET "PlanId" = "PlanId" + 1;""");

            // ── возвращаем PK ───────────────────────────────────────────────
            migrationBuilder.Sql("""ALTER TABLE "Roles" ADD CONSTRAINT "Roles_pkey" PRIMARY KEY ("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Sources" ADD CONSTRAINT "Sources_pkey" PRIMARY KEY ("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "VacancyStatuses" ADD CONSTRAINT "VacancyStatuses_pkey" PRIMARY KEY ("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "FunnelStatuses" ADD CONSTRAINT "FunnelStatuses_pkey" PRIMARY KEY ("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "LetterKinds" ADD CONSTRAINT "LetterKinds_pkey" PRIMARY KEY ("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "LetterStatuses" ADD CONSTRAINT "LetterStatuses_pkey" PRIMARY KEY ("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "ParseJobStatuses" ADD CONSTRAINT "ParseJobStatuses_pkey" PRIMARY KEY ("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Plans" ADD CONSTRAINT "Plans_pkey" PRIMARY KEY ("Id");""");

            // ── возвращаем FK ───────────────────────────────────────────────
            migrationBuilder.Sql("""ALTER TABLE "Users" ADD CONSTRAINT "Users_RoleId_fkey" FOREIGN KEY ("RoleId") REFERENCES "Roles"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Vacancies" ADD CONSTRAINT "Vacancies_VacancyStatusId_fkey" FOREIGN KEY ("VacancyStatusId") REFERENCES "VacancyStatuses"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Vacancies" ADD CONSTRAINT "Vacancies_SourceId_fkey" FOREIGN KEY ("SourceId") REFERENCES "Sources"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Candidates" ADD CONSTRAINT "Candidates_SourceId_fkey" FOREIGN KEY ("SourceId") REFERENCES "Sources"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Applications" ADD CONSTRAINT "Applications_ApplicationStatusId_fkey" FOREIGN KEY ("ApplicationStatusId") REFERENCES "FunnelStatuses"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "StatusHistory" ADD CONSTRAINT "StatusHistory_FromStatusId_fkey" FOREIGN KEY ("FromStatusId") REFERENCES "FunnelStatuses"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "StatusHistory" ADD CONSTRAINT "StatusHistory_ToStatusId_fkey" FOREIGN KEY ("ToStatusId") REFERENCES "FunnelStatuses"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Letters" ADD CONSTRAINT "Letters_KindId_fkey" FOREIGN KEY ("KindId") REFERENCES "LetterKinds"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Letters" ADD CONSTRAINT "Letters_StatusId_fkey" FOREIGN KEY ("StatusId") REFERENCES "LetterStatuses"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "ParseJobs" ADD CONSTRAINT "ParseJobs_ParseJobStatusId_fkey" FOREIGN KEY ("ParseJobStatusId") REFERENCES "ParseJobStatuses"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Payments" ADD CONSTRAINT "Payments_PlanId_fkey" FOREIGN KEY ("PlanId") REFERENCES "Plans"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Subscriptions" ADD CONSTRAINT "Subscriptions_PlanId_fkey" FOREIGN KEY ("PlanId") REFERENCES "Plans"("Id");""");

            // ── sequence'ы за новыми max(id) ────────────────────────────────
            migrationBuilder.Sql("""SELECT setval('"Roles_Id_seq"', (SELECT MAX("Id") FROM "Roles"));""");
            migrationBuilder.Sql("""SELECT setval('"Sources_Id_seq"', (SELECT MAX("Id") FROM "Sources"));""");
            migrationBuilder.Sql("""SELECT setval('"VacancyStatuses_Id_seq"', (SELECT MAX("Id") FROM "VacancyStatuses"));""");
            migrationBuilder.Sql("""SELECT setval('"FunnelStatuses_Id_seq"', (SELECT MAX("Id") FROM "FunnelStatuses"));""");
            migrationBuilder.Sql("""SELECT setval('"LetterKinds_Id_seq"', (SELECT MAX("Id") FROM "LetterKinds"));""");
            migrationBuilder.Sql("""SELECT setval('"ParceJobStatuses_Id_seq"', (SELECT MAX("Id") FROM "ParseJobStatuses"));""");
            migrationBuilder.Sql("""SELECT setval('"Plans_Id_seq"', (SELECT MAX("Id") FROM "Plans"));""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // ── снимаем FK ─────────────────────────────────────────────────
            migrationBuilder.Sql("""ALTER TABLE "Users" DROP CONSTRAINT "Users_RoleId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Vacancies" DROP CONSTRAINT "Vacancies_VacancyStatusId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Vacancies" DROP CONSTRAINT "Vacancies_SourceId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Candidates" DROP CONSTRAINT "Candidates_SourceId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Applications" DROP CONSTRAINT "Applications_ApplicationStatusId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "StatusHistory" DROP CONSTRAINT "StatusHistory_FromStatusId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "StatusHistory" DROP CONSTRAINT "StatusHistory_ToStatusId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Letters" DROP CONSTRAINT "Letters_KindId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Letters" DROP CONSTRAINT "Letters_StatusId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "ParseJobs" DROP CONSTRAINT "ParseJobs_ParseJobStatusId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Payments" DROP CONSTRAINT "Payments_PlanId_fkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Subscriptions" DROP CONSTRAINT "Subscriptions_PlanId_fkey";""");

            // ── снимаем PK ─────────────────────────────────────────────────
            migrationBuilder.Sql("""ALTER TABLE "Roles" DROP CONSTRAINT "Roles_pkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Sources" DROP CONSTRAINT "Sources_pkey";""");
            migrationBuilder.Sql("""ALTER TABLE "VacancyStatuses" DROP CONSTRAINT "VacancyStatuses_pkey";""");
            migrationBuilder.Sql("""ALTER TABLE "FunnelStatuses" DROP CONSTRAINT "FunnelStatuses_pkey";""");
            migrationBuilder.Sql("""ALTER TABLE "LetterKinds" DROP CONSTRAINT "LetterKinds_pkey";""");
            migrationBuilder.Sql("""ALTER TABLE "LetterStatuses" DROP CONSTRAINT "LetterStatuses_pkey";""");
            migrationBuilder.Sql("""ALTER TABLE "ParseJobStatuses" DROP CONSTRAINT "ParseJobStatuses_pkey";""");
            migrationBuilder.Sql("""ALTER TABLE "Plans" DROP CONSTRAINT "Plans_pkey";""");

            // ── сдвигаем справочники -1 ────────────────────────────────────
            migrationBuilder.Sql("""UPDATE "Roles" SET "Id" = "Id" - 1;""");
            migrationBuilder.Sql("""UPDATE "Sources" SET "Id" = "Id" - 1;""");
            migrationBuilder.Sql("""UPDATE "VacancyStatuses" SET "Id" = "Id" - 1;""");
            migrationBuilder.Sql("""UPDATE "FunnelStatuses" SET "Id" = "Id" - 1;""");
            migrationBuilder.Sql("""UPDATE "LetterKinds" SET "Id" = "Id" - 1;""");
            migrationBuilder.Sql("""UPDATE "LetterStatuses" SET "Id" = "Id" - 1;""");
            migrationBuilder.Sql("""UPDATE "ParseJobStatuses" SET "Id" = "Id" - 1;""");
            migrationBuilder.Sql("""UPDATE "Plans" SET "Id" = "Id" - 1;""");

            // ── сдвигаем ссылки -1 ──────────────────────────────────────────
            migrationBuilder.Sql("""UPDATE "Users" SET "RoleId" = "RoleId" - 1;""");
            migrationBuilder.Sql("""UPDATE "Vacancies" SET "VacancyStatusId" = "VacancyStatusId" - 1;""");
            migrationBuilder.Sql("""UPDATE "Vacancies" SET "SourceId" = "SourceId" - 1;""");
            migrationBuilder.Sql("""UPDATE "Candidates" SET "SourceId" = "SourceId" - 1;""");
            migrationBuilder.Sql("""UPDATE "Applications" SET "ApplicationStatusId" = "ApplicationStatusId" - 1;""");
            migrationBuilder.Sql("""UPDATE "StatusHistory" SET "FromStatusId" = "FromStatusId" - 1 WHERE "FromStatusId" IS NOT NULL;""");
            migrationBuilder.Sql("""UPDATE "StatusHistory" SET "ToStatusId" = "ToStatusId" - 1;""");
            migrationBuilder.Sql("""UPDATE "Letters" SET "KindId" = "KindId" - 1;""");
            migrationBuilder.Sql("""UPDATE "Letters" SET "StatusId" = "StatusId" - 1;""");
            migrationBuilder.Sql("""UPDATE "ParseJobs" SET "ParseJobStatusId" = "ParseJobStatusId" - 1;""");
            migrationBuilder.Sql("""UPDATE "Payments" SET "PlanId" = "PlanId" - 1;""");
            migrationBuilder.Sql("""UPDATE "Subscriptions" SET "PlanId" = "PlanId" - 1;""");

            // ── возвращаем PK ───────────────────────────────────────────────
            migrationBuilder.Sql("""ALTER TABLE "Roles" ADD CONSTRAINT "Roles_pkey" PRIMARY KEY ("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Sources" ADD CONSTRAINT "Sources_pkey" PRIMARY KEY ("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "VacancyStatuses" ADD CONSTRAINT "VacancyStatuses_pkey" PRIMARY KEY ("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "FunnelStatuses" ADD CONSTRAINT "FunnelStatuses_pkey" PRIMARY KEY ("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "LetterKinds" ADD CONSTRAINT "LetterKinds_pkey" PRIMARY KEY ("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "LetterStatuses" ADD CONSTRAINT "LetterStatuses_pkey" PRIMARY KEY ("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "ParseJobStatuses" ADD CONSTRAINT "ParseJobStatuses_pkey" PRIMARY KEY ("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Plans" ADD CONSTRAINT "Plans_pkey" PRIMARY KEY ("Id");""");

            // ── возвращаем FK ───────────────────────────────────────────────
            migrationBuilder.Sql("""ALTER TABLE "Users" ADD CONSTRAINT "Users_RoleId_fkey" FOREIGN KEY ("RoleId") REFERENCES "Roles"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Vacancies" ADD CONSTRAINT "Vacancies_VacancyStatusId_fkey" FOREIGN KEY ("VacancyStatusId") REFERENCES "VacancyStatuses"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Vacancies" ADD CONSTRAINT "Vacancies_SourceId_fkey" FOREIGN KEY ("SourceId") REFERENCES "Sources"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Candidates" ADD CONSTRAINT "Candidates_SourceId_fkey" FOREIGN KEY ("SourceId") REFERENCES "Sources"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Applications" ADD CONSTRAINT "Applications_ApplicationStatusId_fkey" FOREIGN KEY ("ApplicationStatusId") REFERENCES "FunnelStatuses"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "StatusHistory" ADD CONSTRAINT "StatusHistory_FromStatusId_fkey" FOREIGN KEY ("FromStatusId") REFERENCES "FunnelStatuses"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "StatusHistory" ADD CONSTRAINT "StatusHistory_ToStatusId_fkey" FOREIGN KEY ("ToStatusId") REFERENCES "FunnelStatuses"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Letters" ADD CONSTRAINT "Letters_KindId_fkey" FOREIGN KEY ("KindId") REFERENCES "LetterKinds"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Letters" ADD CONSTRAINT "Letters_StatusId_fkey" FOREIGN KEY ("StatusId") REFERENCES "LetterStatuses"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "ParseJobs" ADD CONSTRAINT "ParseJobs_ParseJobStatusId_fkey" FOREIGN KEY ("ParseJobStatusId") REFERENCES "ParseJobStatuses"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Payments" ADD CONSTRAINT "Payments_PlanId_fkey" FOREIGN KEY ("PlanId") REFERENCES "Plans"("Id");""");
            migrationBuilder.Sql("""ALTER TABLE "Subscriptions" ADD CONSTRAINT "Subscriptions_PlanId_fkey" FOREIGN KEY ("PlanId") REFERENCES "Plans"("Id");""");

            // ── sequence'ы обратно ─────────────────────────────────────────
            migrationBuilder.Sql("""SELECT setval('"Roles_Id_seq"', (SELECT MAX("Id") FROM "Roles"));""");
            migrationBuilder.Sql("""SELECT setval('"Sources_Id_seq"', (SELECT MAX("Id") FROM "Sources"));""");
            migrationBuilder.Sql("""SELECT setval('"VacancyStatuses_Id_seq"', (SELECT MAX("Id") FROM "VacancyStatuses"));""");
            migrationBuilder.Sql("""SELECT setval('"FunnelStatuses_Id_seq"', (SELECT MAX("Id") FROM "FunnelStatuses"));""");
            migrationBuilder.Sql("""SELECT setval('"LetterKinds_Id_seq"', (SELECT MAX("Id") FROM "LetterKinds"));""");
            migrationBuilder.Sql("""SELECT setval('"ParceJobStatuses_Id_seq"', (SELECT MAX("Id") FROM "ParseJobStatuses"));""");
            migrationBuilder.Sql("""SELECT setval('"Plans_Id_seq"', (SELECT MAX("Id") FROM "Plans"));""");
        }
    }
}