namespace AccesoDatos.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public int AnioPublicacion { get; set; }
        public string Titulo { get; set; }
        public int AutorId { get; set; }
        public Autor Autor { get; set; }
    }
}