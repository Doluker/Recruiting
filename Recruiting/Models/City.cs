using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class City
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
