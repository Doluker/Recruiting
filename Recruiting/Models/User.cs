using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<ResumeFile> ResumeFiles { get; set; } = new List<ResumeFile>();

    public virtual Role Role { get; set; } = null!;

    public virtual Subscription? Subscription { get; set; }

    public virtual UserSetting? UserSetting { get; set; }

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
