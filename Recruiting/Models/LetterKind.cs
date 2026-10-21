using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class LetterKind
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Letter> Letters { get; set; } = new List<Letter>();
}
