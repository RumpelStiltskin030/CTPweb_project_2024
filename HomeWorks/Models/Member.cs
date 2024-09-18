using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeWorks.Models;

public partial class Member
{
    [Display(Name = "帳號")]
    public string MeId { get; set; } = null!;

    [Required(ErrorMessage = "暱稱是必填的")]
    [Display(Name = "暱稱")]
    public string MeName { get; set; } = null!;

    [Required(ErrorMessage = "手機是必填的")]
    [RegularExpression(@"^09\d{8}$", ErrorMessage = "請填寫正確手機號碼")]
    [Display(Name = "手機")]
    public string MeTel { get; set; } = null!;

    [Required(ErrorMessage = "電子郵件是必填的")]
    [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
    [Display(Name = "電子郵件")]
    public string MeEmail { get; set; } = null!;

    [Required(ErrorMessage = "密碼是必填的")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8}$", ErrorMessage = "密碼必須為4位字母（大小寫均可）和4位數字")]
    [Display(Name = "密碼")]
    public string MePassword { get; set; } = null!;

    [Display(Name = "權限ID")]
    public string PreId { get; set; } = null!;

    [Display(Name = "照片")]
    public string? PhotoPath { get; set; }

    public virtual ICollection<Order> OrderMes { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderSeles { get; set; } = new List<Order>();

    public virtual Permission Pre { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
