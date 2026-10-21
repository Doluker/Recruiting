using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class Skill
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();

    public virtual ICollection<VacancySkill> VacancySkills { get; set; } = new List<VacancySkill>();
}
