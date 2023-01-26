namespace tp5.ViewModels;

public class PedidoModificadoViewModel
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

    public int Cadete { get; set; }
    public int Cliente { get; set; }

    public List<UsuarioViewModel> Cadetes { get; set; }
    public List<UsuarioViewModel> Clientes { get; set; }
}