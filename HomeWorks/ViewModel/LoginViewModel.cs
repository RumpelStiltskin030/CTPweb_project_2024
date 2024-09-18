using System.ComponentModel.DataAnnotations;

namespace HomeWorks.ViewModel
{
    public class LoginViewModel
    {
        [Display(Name = "會員帳號")]
        [Required(ErrorMessage = "必填")]
        [RegularExpression("^[0-9]{8,12}$", ErrorMessage = "請輸入8到12位數字")]
        public string MeId { get; set; } = null!;
        [Display(Name = "會員密碼")]
        [Required(ErrorMessage = "必填")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).{8}$", ErrorMessage = "使用英文及數字組成八位密碼")]
        public string MePassword { get; set; } = null!;
    }
}
