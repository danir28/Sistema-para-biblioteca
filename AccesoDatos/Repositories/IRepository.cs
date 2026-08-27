using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Repositories
{
    public interface IRepository<T> where T : class
    {
        void agregar(T entity);
        List<T> obtenerTodos();
    }
}