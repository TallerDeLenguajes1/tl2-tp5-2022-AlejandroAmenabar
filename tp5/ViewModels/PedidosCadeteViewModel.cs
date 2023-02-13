namespace tp5.ViewModels;
public class PedidosCadeteViewModel {  
    public string nombreCadete;
    public List<PedidoViewModel> pedidosCadete;
    public PedidosCadeteViewModel(string nombreCadete, List<PedidoViewModel> pedidosCadete)
    {
        this.nombreCadete = nombreCadete;
        this.pedidosCadete = pedidosCadete;
    }

    public PedidosCadeteViewModel(){
        this.nombreCadete = "";
        this.pedidosCadete = new List<PedidoViewModel>();
    }

}

