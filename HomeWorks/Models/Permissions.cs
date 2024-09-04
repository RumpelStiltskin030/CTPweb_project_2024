using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeWorks.Models;

public partial class Permissions
{
    [Display(Name = "權限編號")]
    public string PreId { get; set; } = null!;
    [Display(Name = "權限名稱")]
    public string PreName { get; set; } = null!;
}
