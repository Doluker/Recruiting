using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class ResumeFile
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public int? CandidateId { get; set; }

    public string ObjectKey { get; set; } = null!;

    public string OriginalFileName { get; set; } = null!;

    public int ContentTypeId { get; set; }

    public string FileHash { get; set; } = null!;

    public long FileSize { get; set; }

    public string? ExtractedText { get; set; }

    public string? ParsedJsonKey { get; set; }

    public DateTime UploadedAt { get; set; }

    public DateTime? ProcessedAt { get; set; }

    public virtual Candidate? Candidate { get; set; }

    public virtual ResumeContentType ContentType { get; set; } = null!;

    public virtual User Owner { get; set; } = null!;

    public virtual ParseJob? ParseJob { get; set; }
}
