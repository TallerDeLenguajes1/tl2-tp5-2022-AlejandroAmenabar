namespace tp5.Repositories;

public interface IRepositorioUsuario : IRepositorio<Usuario>
{
    Usuario? Verificar(Usuario usuario);

    IEnumerable<Usuario> BuscarTodosPorRol(Rol rol);
}