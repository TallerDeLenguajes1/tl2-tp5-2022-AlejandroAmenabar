namespace tp5.Models;

public class Pedido{

    public int Id { get; set; }

    public string Observacion { get; set; }

    public string Estado { get; set; }
    
    public int? Cliente { get; set; }
    public int? Cadete { get; set; }
    public Pedido(){}
    public Pedido(int id, string observacion, string estado,int? cliente, int? cadete)
    {
        Id = id;
        Observacion = observacion;
        Estado = estado;
        Cliente = cliente;
        Cadete = cadete;
    }

    public override string? ToString()
    {
        return "Código Pedido: " + Id + " Código Cliente: " + Cliente + " Código Cadete: " + Cadete + " Estado: " + Estado;
    }
}