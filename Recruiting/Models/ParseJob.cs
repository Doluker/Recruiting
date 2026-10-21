using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class ParseJob
{
    public int Id { get; set; }

    public int ResumeFileId { get; set; }

    public int ParseJobStatusId { get; set; }

    public int Attempts { get; set; }

    public string? Error { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? LockedUntil { get; set; }

    public DateTime? FinishedAt { get; set; }

    public virtual ParseJobStatus ParseJobStatus { get; set; } = null!;

    public virtual ResumeFile ResumeFile { get; set; } = null!;
}
