namespace AccesoDatos.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; }
    }
}