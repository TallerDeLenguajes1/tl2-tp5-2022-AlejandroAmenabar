namespace tp5.ViewModels;

public class LoginViewModel
{
    [Required]
    [StringLength(15, MinimumLength = 5)]
    [Display(Name = "Usuario")]
    public string NombreUsuario { get; set; }

    [Required]
    [StringLength(15, MinimumLength = 3)]
    [Display(Name = "Clave")]
    [DataType(DataType.Password)]
    public string Clave { get; set; }
}