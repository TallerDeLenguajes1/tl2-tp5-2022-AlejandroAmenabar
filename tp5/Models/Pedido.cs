namespace tp5.Models;

public class Pedido{

    public int Id { get; set; }

    public string Observacion { get; set; }

    public string Estado { get; set; }
    
    public Cliente Cliente { get; set; }

    public Pedido(int id, string observacion, string estado)
    {
        Id = id;
        Observacion = observacion;
        Estado = estado;
        Cliente = new Cliente("", "",1);
    }

    public override string? ToString()
    {
        return "Número: " + Id + " Cliente: " + Cliente.Nombre + " Estado: " + (Estado);
    }
}