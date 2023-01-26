namespace tp5.ViewModels;

public class PedidoViewModel
{
    [Required] public int Id { get; set; }

    [Required]
    [StringLength(200)]
    [Display(Name = "Observacion")]
    public string Observacion { get; set; }

    [Required]
    [StringLength(30)]
    [Display(Name = "Estado")]
    public string Estado { get; set; }

    [Required] public string Cliente { get; set; }

    [Required] public string Cadete { get; set; }
}