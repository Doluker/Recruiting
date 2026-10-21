using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class Candidate
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public string? Surname { get; set; }

    public string? Name { get; set; }

    public string? Patronymic { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int? CityId { get; set; }

    public int? PositionId { get; set; }

    public int? ExperienceYears { get; set; }

    public int? EducationId { get; set; }

    public int SourceId { get; set; }

    public string? ExternalId { get; set; }

    public string? SourceUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();

    public virtual City? City { get; set; }

    public virtual Education? Education { get; set; }

    public virtual User Owner { get; set; } = null!;

    public virtual Position? Position { get; set; }

    public virtual ICollection<ResumeFile> ResumeFiles { get; set; } = new List<ResumeFile>();
}
