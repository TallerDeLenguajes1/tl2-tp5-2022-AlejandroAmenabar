namespace tp5.ViewModels;

public class UsuarioModificadoViewModel
{
    [Required] public int Id { get; set; }

    [StringLength(100)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; }

    [Display(Name = "Dirreción")]
    public string Direccion { get; set; }

    [Display(Name = "Teléfono")]
    public string Telefono { get; set; }
}