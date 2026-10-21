using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class VacancyStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
