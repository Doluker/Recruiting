using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class Letter
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }

    public int KindId { get; set; }

    public string Subject { get; set; } = null!;

    public string Body { get; set; } = null!;

    public int StatusId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? SentAt { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual LetterKind Kind { get; set; } = null!;

    public virtual LetterStatus Status { get; set; } = null!;
}
