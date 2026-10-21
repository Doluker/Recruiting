using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class Application
{
    public int Id { get; set; }

    public int VacancyId { get; set; }

    public int CandidateId { get; set; }

    public decimal? Score { get; set; }

    public decimal? SkillsScore { get; set; }

    public decimal? ExperienceScore { get; set; }

    public decimal? EducationScore { get; set; }

    public int ApplicationStatusId { get; set; }

    public string? RejectReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual FunnelStatus ApplicationStatus { get; set; } = null!;

    public virtual Candidate Candidate { get; set; } = null!;

    public virtual ICollection<Letter> Letters { get; set; } = new List<Letter>();

    public virtual ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();

    public virtual Vacancy Vacancy { get; set; } = null!;
}
