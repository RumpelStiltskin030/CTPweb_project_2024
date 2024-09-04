using System;
using System.Collections.Generic;

namespace HomeWorks.Models;

public partial class Permission
{
    public string PreId { get; set; } = null!;

    public string PreName { get; set; } = null!;

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
