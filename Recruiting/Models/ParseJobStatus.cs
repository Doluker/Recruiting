using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class ParseJobStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ParseJob> ParseJobs { get; set; } = new List<ParseJob>();
}
