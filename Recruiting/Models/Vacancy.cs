using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class Vacancy
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int SourceId { get; set; }

    public string? ExternalId { get; set; }

    public string? SourceUrl { get; set; }

    public int? CityId { get; set; }

    public int? SalaryFrom { get; set; }

    public int? SalaryTo { get; set; }

    public int? ExperienceYears { get; set; }

    public int? EducationId { get; set; }

    public int VacancyStatusId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual City? City { get; set; }

    public virtual Education? Education { get; set; }

    public virtual User Owner { get; set; } = null!;

    public virtual Source Source { get; set; } = null!;

    public virtual ICollection<VacancySkill> VacancySkills { get; set; } = new List<VacancySkill>();

    public virtual VacancyStatus VacancyStatus { get; set; } = null!;
}
