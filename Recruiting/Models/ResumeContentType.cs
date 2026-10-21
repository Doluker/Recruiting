using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class ResumeContentType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ResumeFile> ResumeFiles { get; set; } = new List<ResumeFile>();
}
