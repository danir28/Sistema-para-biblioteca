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
        public List<T> obtenerTodos()
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }
    }
}