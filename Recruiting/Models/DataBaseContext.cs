using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Recruiting.Models;

public partial class DataBaseContext : DbContext
{
    public DataBaseContext()
    {
    }

    public DataBaseContext(DbContextOptions<DataBaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<CandidateSkill> CandidateSkills { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Education> Educations { get; set; }

    public virtual DbSet<FunnelStatus> FunnelStatuses { get; set; }

    public virtual DbSet<Letter> Letters { get; set; }

    public virtual DbSet<LetterKind> LetterKinds { get; set; }

    public virtual DbSet<LetterStatus> LetterStatuses { get; set; }

    public virtual DbSet<ParseJob> ParseJobs { get; set; }

    public virtual DbSet<ParseJobStatus> ParseJobStatuses { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<ResumeContentType> ResumeContentTypes { get; set; }

    public virtual DbSet<ResumeFile> ResumeFiles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<Source> Sources { get; set; }

    public virtual DbSet<StatusHistory> StatusHistories { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSetting> UserSettings { get; set; }

    public virtual DbSet<Vacancy> Vacancies { get; set; }

    public virtual DbSet<VacancySkill> VacancySkills { get; set; }

    public virtual DbSet<VacancyStatus> VacancyStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // NOTE: the case-insensitive unique indexes (UX_*_Name, UX_Currencies_Code, UX_Users_Email,
        // UX_Users_UserName) are expression indexes - they can't be expressed in the EF Core model.
        // They are created with raw SQL, see Migrations/20261021072339_InitialCreate.cs -> CreateExpressionIndexes().

        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ActivityLogs_pkey");

            entity.HasOne(d => d.User).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ActivityLogs_UserId_fkey");
        });

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Applications_pkey");

            entity.HasIndex(e => new { e.VacancyId, e.CandidateId }, "Applications_VacancyId_CandidateId_key").IsUnique();

            entity.HasIndex(e => e.CandidateId, "IX_Applications_Candidate");

            entity.HasIndex(e => new { e.VacancyId, e.ApplicationStatusId }, "IX_Applications_Vacancy");

            entity.Property(e => e.EducationScore).HasPrecision(5, 2);
            entity.Property(e => e.ExperienceScore).HasPrecision(5, 2);
            entity.Property(e => e.Score).HasPrecision(5, 2);
            entity.Property(e => e.SkillsScore).HasPrecision(5, 2);

            entity.ToTable(t => t.HasCheckConstraint("CK_Applications_Score", "(\"Score\" IS NULL) OR ((\"Score\" >= 0) AND (\"Score\" <= 100))"));

            entity.HasOne(d => d.ApplicationStatus).WithMany(p => p.Applications)
                .HasForeignKey(d => d.ApplicationStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Applications_ApplicationStatusId_fkey");

            entity.HasOne(d => d.Candidate).WithMany(p => p.Applications)
                .HasForeignKey(d => d.CandidateId)
                .HasConstraintName("Applications_CandidateId_fkey");

            entity.HasOne(d => d.Vacancy).WithMany(p => p.Applications)
                .HasForeignKey(d => d.VacancyId)
                .HasConstraintName("Applications_VacancyId_fkey");
        });

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Candidates_pkey");

            entity.HasIndex(e => new { e.OwnerId, e.SourceId, e.ExternalId }, "Candidates_OwnerId_SourceId_ExternalId_key").IsUnique();

            entity.HasIndex(e => e.OwnerId, "IX_Candidates_Owner");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Patronymic).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(100);
            entity.Property(e => e.Surname).HasMaxLength(100);

            entity.HasOne(d => d.City).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Candidates_CityId_fkey");

            entity.HasOne(d => d.Education).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.EducationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Candidates_EducationId_fkey");

            entity.HasOne(d => d.Owner).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("Candidates_OwnerId_fkey");

            entity.HasOne(d => d.Position).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Candidates_PositionId_fkey");
        });

        modelBuilder.Entity<CandidateSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CandidateSkills_pkey");

            entity.HasIndex(e => new { e.CandidateId, e.SkillId }, "CandidateSkills_CandidateId_SkillId_key").IsUnique();

            entity.HasIndex(e => e.SkillId, "IX_CandidateSkills_Skill");

            entity.HasOne(d => d.Candidate).WithMany(p => p.CandidateSkills)
                .HasForeignKey(d => d.CandidateId)
                .HasConstraintName("CandidateSkills_CandidateId_fkey");

            entity.HasOne(d => d.Skill).WithMany(p => p.CandidateSkills)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("CandidateSkills_SkillId_fkey");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Cities_pkey");

            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Currencies_pkey");

            entity.Property(e => e.Symbol).HasMaxLength(3);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Education>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Educations_pkey");

            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<FunnelStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FunnelStatuses_pkey");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Letter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Letters_pkey");

            entity.HasIndex(e => e.ApplicationId, "IX_Letters_Application");

            entity.HasOne(d => d.Application).WithMany(p => p.Letters)
                .HasForeignKey(d => d.ApplicationId)
                .HasConstraintName("Letters_ApplicationId_fkey");

            entity.HasOne(d => d.Kind).WithMany(p => p.Letters)
                .HasForeignKey(d => d.KindId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Letters_KindId_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Letters)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Letters_StatusId_fkey");
        });

        modelBuilder.Entity<LetterKind>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LetterKinds_pkey");

            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<LetterStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LetterStatuses_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ParseJob>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ParseJobs_pkey");

            entity.HasIndex(e => new { e.ParseJobStatusId, e.CreatedAt }, "IX_ParseJobs_Queue");

            entity.HasIndex(e => e.ResumeFileId, "UQ_ParseJobs_Active")
                .IsUnique()
                .HasFilter("(\"FinishedAt\" IS NULL)");

            entity.ToTable(t => t.HasCheckConstraint("CK_ParseJobs_Attempts", "\"Attempts\" >= 0"));

            entity.HasOne(d => d.ParseJobStatus).WithMany(p => p.ParseJobs)
                .HasForeignKey(d => d.ParseJobStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ParseJobs_ParseJobStatusId_fkey");

            entity.HasOne(d => d.ResumeFile).WithOne(p => p.ParseJob)
                .HasForeignKey<ParseJob>(d => d.ResumeFileId)
                .HasConstraintName("ParseJobs_ResumeFileId_fkey");
        });

        modelBuilder.Entity<ParseJobStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ParseJobStatuses_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"ParceJobStatuses_Id_seq\"'::regclass)");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Payments_pkey");

            entity.HasIndex(e => e.ProviderPaymentId, "Payments_ProviderPaymentId_key").IsUnique();

            entity.Property(e => e.Amount).HasPrecision(10, 2);

            entity.HasOne(d => d.Currency).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Payments_CurrencyId_fkey");

            entity.HasOne(d => d.Plan).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Payments_PlanId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Payments_UserId_fkey");

            entity.HasIndex(e => e.PaymentStatusId, "IX_Payments_PaymentStatus");

            entity.HasOne(d => d.PaymentStatus).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Payments_PaymentStatusId_fkey");
        });

        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PaymentStatuses_pkey");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasData(
                new PaymentStatus { Id = 1, Name = "Ожидает оплату" },
                new PaymentStatus { Id = 2, Name = "Оплачено" },
                new PaymentStatus { Id = 3, Name = "Ошибка оплаты" },
                new PaymentStatus { Id = 4, Name = "Возврат" });
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Plans_pkey");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PriceMonthly).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Positions_pkey");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ResumeContentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ResumeContentTypes_pkey");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ResumeFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ResumeFiles_pkey");

            entity.HasIndex(e => e.OwnerId, "IX_ResumeFiles_Owner");

            entity.HasIndex(e => new { e.OwnerId, e.FileHash }, "ResumeFiles_OwnerId_FileHash_key").IsUnique();

            entity.Property(e => e.OriginalFileName).HasMaxLength(200);

            entity.HasOne(d => d.Candidate).WithMany(p => p.ResumeFiles)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ResumeFiles_CandidateId_fkey");

            entity.HasOne(d => d.ContentType).WithMany(p => p.ResumeFiles)
                .HasForeignKey(d => d.ContentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ResumeFiles_ContentTypeId_fkey");

            entity.HasOne(d => d.Owner).WithMany(p => p.ResumeFiles)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("ResumeFiles_OwnerId_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Roles_pkey");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Skills_pkey");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Source>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Sources_pkey");

            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<StatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatusHistory_pkey");

            entity.ToTable("StatusHistory");

            entity.HasIndex(e => new { e.ApplicationId, e.ChangedAt }, "IX_StatusHistory_App");

            entity.HasOne(d => d.Application).WithMany(p => p.StatusHistories)
                .HasForeignKey(d => d.ApplicationId)
                .HasConstraintName("StatusHistory_ApplicationId_fkey");

            entity.HasOne(d => d.FromStatus).WithMany(p => p.StatusHistoryFromStatuses)
                .HasForeignKey(d => d.FromStatusId)
                .HasConstraintName("StatusHistory_FromStatusId_fkey");

            entity.HasOne(d => d.ToStatus).WithMany(p => p.StatusHistoryToStatuses)
                .HasForeignKey(d => d.ToStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("StatusHistory_ToStatusId_fkey");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Subscriptions_pkey");

            entity.HasIndex(e => e.UserId, "UQ_Subscriptions_UserId").IsUnique();

            entity.HasOne(d => d.Plan).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Subscriptions_PlanId_fkey");

            entity.HasOne(d => d.User).WithOne(p => p.Subscription)
                .HasForeignKey<Subscription>(d => d.UserId)
                .HasConstraintName("Subscriptions_UserId_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UserName).HasMaxLength(100);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_RoleId_fkey");
        });

        modelBuilder.Entity<UserSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserSettings_pkey");

            entity.HasIndex(e => e.UserId, "UQ_UserSettings_UserId").IsUnique();

            entity.HasOne(d => d.User).WithOne(p => p.UserSetting)
                .HasForeignKey<UserSetting>(d => d.UserId)
                .HasConstraintName("UserSettings_UserId_fkey");
        });

        modelBuilder.Entity<Vacancy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Vacancies_pkey");

            entity.HasIndex(e => new { e.OwnerId, e.VacancyStatusId }, "IX_Vacancies_Owner");

            entity.HasIndex(e => new { e.OwnerId, e.SourceId, e.ExternalId }, "Vacancies_OwnerId_SourceId_ExternalId_key").IsUnique();

            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.ToTable(t => t.HasCheckConstraint("CK_Vacancies_Salary", "(\"SalaryFrom\" IS NULL) OR (\"SalaryTo\" IS NULL) OR (\"SalaryFrom\" <= \"SalaryTo\")"));

            entity.HasOne(d => d.City).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Vacancies_CityId_fkey");

            entity.HasOne(d => d.Education).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.EducationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Vacancies_EducationId_fkey");

            entity.HasOne(d => d.Owner).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("Vacancies_OwnerId_fkey");

            entity.HasOne(d => d.Source).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.SourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vacancies_SourceId_fkey");

            entity.HasOne(d => d.VacancyStatus).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.VacancyStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vacancies_VacancyStatusId_fkey");
        });

        modelBuilder.Entity<VacancySkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("VacancySkills_pkey");

            entity.HasIndex(e => e.SkillId, "IX_VacancySkills_Skill");

            entity.HasIndex(e => new { e.VacancyId, e.SkillId }, "VacancySkills_VacancyId_SkillId_key").IsUnique();

            entity.HasOne(d => d.Skill).WithMany(p => p.VacancySkills)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("VacancySkills_SkillId_fkey");

            entity.HasOne(d => d.Vacancy).WithMany(p => p.VacancySkills)
                .HasForeignKey(d => d.VacancyId)
                .HasConstraintName("VacancySkills_VacancyId_fkey");
        });

        modelBuilder.Entity<VacancyStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("VacancyStatuses_pkey");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
