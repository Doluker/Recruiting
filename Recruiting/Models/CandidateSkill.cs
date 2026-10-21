using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class CandidateSkill
{
    public int Id { get; set; }

    public int CandidateId { get; set; }

    public int SkillId { get; set; }

    public int? Years { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}
