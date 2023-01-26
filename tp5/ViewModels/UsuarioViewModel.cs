namespace tp5.ViewModels;

public class UsuarioViewModel
{
    [Required] public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; }

    [Required]
    [StringLength(300)]
    [Display(Name = "Usuario")]
    public string NombreUsuario { get; set; }
    
    [Required]
    [Display(Name = "Dirreción")]
    public string Direccion { get; set; }

    [Required]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; }
    
    [Required] [Display(Name = "Clave")] public string Clave { get; set; }

    [Required] [Display(Name = "Rol")] public Rol Rol { get; set; }
}