namespace tp5.ViewModels;
public class PedidosClienteViewModel {  
    public string nombreCliente;
    public List<PedidoViewModel> pedidosCliente;
    public PedidosClienteViewModel(string nombreCliente, List<PedidoViewModel> pedidosCliente)
    {
        this.nombreCliente = nombreCliente;
        this.pedidosCliente = pedidosCliente;
    }

    public PedidosClienteViewModel(){
        this.nombreCliente = "";
        this.pedidosCliente = new List<PedidoViewModel>();
    }
}

