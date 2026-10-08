using System.ComponentModel.DataAnnotations;

namespace HoulailYassine.Models;

public class CreationClientViewModel
{
    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "First Name")]
    [MaxLength(300, ErrorMessage = "First name cannot exceed 300 characters")]
    public string Nom { get; set; } = string.Empty;

    [Display(Name = "Last Name")]
    [MaxLength(300, ErrorMessage = "Last name cannot exceed 300 characters")]
    public string Prenom { get; set; } = string.Empty;

    [Display(Name = "Phone Number")]
    [MaxLength(12, ErrorMessage = "Phone number cannot exceed 12 characters")]
    public string NumdeTelephone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    public string EMail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string MotPasse { get; set; } = string.Empty;
}