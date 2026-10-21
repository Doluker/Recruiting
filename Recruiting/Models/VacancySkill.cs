using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class VacancySkill
{
    public int Id { get; set; }

    public int VacancyId { get; set; }

    public int SkillId { get; set; }

    public bool IsRequired { get; set; }

    public virtual Skill Skill { get; set; } = null!;

    public virtual Vacancy Vacancy { get; set; } = null!;
}
