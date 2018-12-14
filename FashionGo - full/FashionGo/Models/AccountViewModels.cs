using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionGo.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Ghi nhớ trên trình duyệt")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Địa chỉ Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Địa chỉ Email")]
        [System.Web.Mvc.Remote("IsUserExists", "ValidatingController", ErrorMessage = "{0} đã tồn tại!")]
        public string Email { get; set; }

        [EmailAddress]
        [Display(Name = "Xác nhận Email")]
        [Compare("Email", ErrorMessage = "Email xác nhận không khớp.")]
        public string ConfirmEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Độ dài {0} yêu cầu lớn hơn {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không đúng.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Độ dài {0} yêu cầu lớn hơn {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không đúng.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class UserViewModel
    {
        
        [Display(Name = "Id")]
        public string Id { get; set; }
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "họ và tên")]
        public string FullName { get; set; }

        public UserViewModel(ApplicationUser u)
        {
            this.Address = u.Address;
            this.Email = u.Email;
            this.Id = u.Id;
            this.PhoneNumber = u.PhoneNumber;
            this.UserName = u.UserName;
            this.FullName = u.FullName;
        }
        public UserViewModel()
        {
        }

    }
    public class RoleViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }
        [EmailAddress]
        [Display(Name = "Tên")]
        public string Name { get; set; }
        

    }
}
