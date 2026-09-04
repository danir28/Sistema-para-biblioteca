using AccesoDatos.Data;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AplicacionDbContext _context;
        public Repository(AplicacionDbContext context)
        {
            _context = context;
        }
        public void agregar(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
        public void modificar(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }
        public List<T> ObtenerTodosCon(params string[] propiedadesRelacionadas)
        {
            IQueryable<T> consulta = _context.Set<T>().AsNoTracking();

            foreach (var propiedad in propiedadesRelacionadas)
            {
                consulta = consulta.Include(propiedad);
            }

            return consulta.ToList();
        }

        // Busca una entidad puntual por su Id (para modificar o eliminar lógicamente).
        // Find() no usa AsNoTracking: la entidad queda trackeada por el contexto,
        // así que alcanza con cambiarle propiedades y guardar.
        public T? ObtenerPorId(int id)
        {
            return _context.Set<T>().Find(id);
        }
    }
}