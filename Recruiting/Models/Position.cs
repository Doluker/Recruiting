using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class Position
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();
}
