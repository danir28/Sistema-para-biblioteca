using AccesoDatos.Models;
using AccesoDatos.Repositories;
using AccesoDatos.Data;

var contexto = new AplicacionDbContext();
IRepository<Autor> autorRepository = new Repository<Autor>(contexto);
IRepository<Libro> libroRepository = new Repository<Libro>(contexto);

bool continuar = true;
while (continuar)
{
    Console.WriteLine("=================================================");
    Console.WriteLine("Bienvenido al sistema de gestión de autores y libros");
    Console.WriteLine("=================================================");
    Console.WriteLine("1. Agregar un autor");
    Console.WriteLine("2. Agregar un libro");
    Console.WriteLine("3. Listar Libros");
    Console.WriteLine("4. Salir");

    Console.WriteLine("Seleccione una opción:");
    string opcion = Console.ReadLine();
    Console.Clear();

    switch (opcion)
    {
        case "1":
            AltaAutor();
            break;
        case "2":
            AltaLibro();
            break;
        case "3":
            ListarLibros();
            break;
        case "4":
            Console.WriteLine("Saliendo del programa");
            continuar = false;
            break;
        default:
            Console.WriteLine("Opción no válida. Intente nuevamente.");
            break;
    }
}

void AltaAutor()
{
    Console.WriteLine("Ingrese el nombre del autor: ");
    string NombreAutor = Console.ReadLine();

    var nuevoAutor = new Autor
    {
        Nombre = NombreAutor
    };

    autorRepository.agregar(nuevoAutor);
    Console.WriteLine("Usuario agregado exitosamente");
}

void AltaLibro()
{
    Console.WriteLine("Ingrese el Titulo del libro: ");
    string TituloLibro = Console.ReadLine();
    Console.WriteLine("Ingrese el anio de publicacion: ");
    int AnioPublicacion = int.Parse(Console.ReadLine());

    var autores = autorRepository.obtenerTodos();

    if (!autores.Any())
    {
        Console.WriteLine("No hay autores cargados todavía. Agregá un autor primero (opción 1).");
        return;
    }

    Console.WriteLine(" Autores disponibles ");
    foreach (var autor in autores)
    {
        Console.WriteLine($"{autor.Id} - {autor.Nombre}");
    }

    Console.WriteLine("Ingrese el ID del autor: ");
    int autorId = int.Parse(Console.ReadLine());

    var nuevoLibro = new Libro
    {
        Titulo = TituloLibro,
        AnioPublicacion = AnioPublicacion,
        AutorId = autorId
    };

    libroRepository.agregar(nuevoLibro);
    Console.WriteLine("Libro agregado exitosamente");
}

void ListarLibros()
{
    var libros = libroRepository.obtenerTodos();
    if (!libros.Any())
    {
        Console.WriteLine("No hay libros cargados todavía.");
        return;
    }

    var autores = autorRepository.obtenerTodos();

    foreach (var libro in libros)
    {
        var autor = autores.FirstOrDefault(a => a.Id == libro.AutorId);
        string nombreAutor = autor != null ? autor.Nombre : "Desconocido";
        Console.WriteLine($"{libro.Titulo} ({libro.AnioPublicacion}) - Autor: {nombreAutor}");
    }
}