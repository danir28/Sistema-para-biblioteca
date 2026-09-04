namespace AccesoDatos.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public int AnioPublicacion { get; set; }
        public string Titulo { get; set; }
        public int AutorId { get; set; }
        public Autor Autor { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        public bool Activo { get; set; } = true;
    }
}