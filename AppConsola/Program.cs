using AccesoDatos.Models;
using AccesoDatos.Repositories;
using AccesoDatos.Data;
using Microsoft.EntityFrameworkCore;

var contexto = new AplicacionDbContext();
contexto.Database.Migrate();
IRepository<Autor> autorRepository = new Repository<Autor>(contexto);
IRepository<Libro> libroRepository = new Repository<Libro>(contexto);
IRepository<Categoria> categoriaRepository = new Repository<Categoria>(contexto);

bool continuar = true;
while (continuar)
{
    Console.WriteLine("=================================================");
    Console.WriteLine("Bienvenido al sistema de gestión de autores y libros");
    Console.WriteLine("=================================================");
    Console.WriteLine("1. Alta Autor");
    Console.WriteLine("2. Alta Categoría");
    Console.WriteLine("3. Alta Libro");
    Console.WriteLine("4. Ver Autores");
    Console.WriteLine("5. Ver Categorías");
    Console.WriteLine("6. Ver Libros");
    Console.WriteLine("7. Modificar Libro");
    Console.WriteLine("8. Eliminar Libro");
    Console.WriteLine("9. Modificar Autor");
    Console.WriteLine("0. Salir");

    Console.WriteLine("Seleccione una opción:");
    string opcion = Console.ReadLine();
    Console.Clear();

    switch (opcion)
    {
        case "0":
            Console.WriteLine("Saliendo del programa");
            continuar = false;
            break;
        case "1":
            AltaAutor();
            break;
        case "2":
            AltaCategoria();
            break;
        case "3":
            AltaLibro();
            break;
        case "4":
            VerAutores();
            break;
        case "5":
            VerCategorias();
            break;
        case "6":
            ListarLibros();
            break;
        case "7":
            ModificarLibro();
            break;
        case "8":
            EliminarLibro();
            break;
        case "9":
            ModificarAutor();
            break;
        default:
            Console.WriteLine("Opción no válida. Intente nuevamente.");
            break;
    }
}

// Da de alta un nuevo autor a partir del nombre ingresado por consola.
void AltaAutor()
{
    Console.WriteLine("Ingrese el nombre del autor: ");
    string NombreAutor = Console.ReadLine();

    var nuevoAutor = new Autor
    {
        Nombre = NombreAutor
    };

    autorRepository.agregar(nuevoAutor);
    Console.WriteLine("Autor agregado exitosamente");
}

// Da de alta una nueva categoría a partir del nombre ingresado por consola.
void AltaCategoria()
{
    Console.WriteLine("Ingrese el nombre de la categoría: ");
    string NombreCategoria = Console.ReadLine();

    var nuevaCategoria = new Categoria
    {
        Nombre = NombreCategoria
    };

    categoriaRepository.agregar(nuevaCategoria);
    Console.WriteLine("Categoría agregada exitosamente");
}

// Da de alta un libro nuevo, pidiendo título, año, autor (por Id) y categoría (por Id).
void AltaLibro()
{
    Console.WriteLine("Ingrese el Titulo del libro: ");
    string TituloLibro = Console.ReadLine();
    Console.WriteLine("Ingrese el anio de publicacion: ");
    int AnioPublicacion = int.Parse(Console.ReadLine());

    var autores = autorRepository.ObtenerTodosCon("Libros");

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

    var categorias = categoriaRepository.ObtenerTodosCon("Libros");

    if (!categorias.Any())
    {
        Console.WriteLine("No hay categorías cargadas todavía. Agregá una categoría primero (opción 2).");
        return;
    }

    Console.WriteLine(" Categorías disponibles ");
    foreach (var categoria in categorias)
    {
        Console.WriteLine($"{categoria.Id} - {categoria.Nombre}");
    }

    Console.WriteLine("Ingrese el ID de la categoría: ");
    int categoriaId = int.Parse(Console.ReadLine());

    var nuevoLibro = new Libro
    {
        Titulo = TituloLibro,
        AnioPublicacion = AnioPublicacion,
        AutorId = autorId,
        CategoriaId = categoriaId
    };

    libroRepository.agregar(nuevoLibro);
    Console.WriteLine("Libro agregado exitosamente");
}

// Lista todos los autores existentes junto con la cantidad de libros que tiene cada uno.
void VerAutores()
{
    var autores = autorRepository.ObtenerTodosCon("Libros");
    if (!autores.Any())
    {
        Console.WriteLine("No hay autores cargados todavía.");
        return;
    }

    foreach (var autor in autores)
    {
        Console.WriteLine($"{autor.Id} - {autor.Nombre} ({autor.Libros.Count} libro/s)");
    }
}

// Lista todas las categorías existentes junto con la cantidad de libros que tiene cada una.
void VerCategorias()
{
    var categorias = categoriaRepository.ObtenerTodosCon("Libros");
    if (!categorias.Any())
    {
        Console.WriteLine("No hay categorías cargadas todavía.");
        return;
    }

    foreach (var categoria in categorias)
    {
        Console.WriteLine($"{categoria.Id} - {categoria.Nombre} ({categoria.Libros.Count} libro/s)");
    }
}

// Lista únicamente los libros activos (no eliminados lógicamente), mostrando
// título, año y el nombre del autor y la categoría a través de las propiedades
// de navegación ya cargadas por el Include del repositorio.
void ListarLibros()
{
    var libros = libroRepository.ObtenerTodosCon("Autor", "Categoria")
        .Where(l => l.Activo)
        .ToList();

    if (!libros.Any())
    {
        Console.WriteLine("No hay libros activos cargados todavía.");
        return;
    }

    foreach (var libro in libros)
    {
        Console.WriteLine($"{libro.Titulo} ({libro.AnioPublicacion}) - Autor: {libro.Autor.Nombre} - Categoría: {libro.Categoria.Nombre}");
    }
}

// Permite cambiar el nombre de un autor existente, seleccionado por Id.
void ModificarAutor()
{
    var autores = autorRepository.ObtenerTodosCon("Libros");
    if (!autores.Any())
    {
        Console.WriteLine("No hay autores cargados todavía.");
        return;
    }

    Console.WriteLine(" Autores disponibles ");
    foreach (var autor in autores)
    {
        Console.WriteLine($"{autor.Id} - {autor.Nombre}");
    }

    Console.WriteLine("Ingrese el ID del autor a modificar: ");
    int autorId = int.Parse(Console.ReadLine());

    var autorAModificar = autorRepository.ObtenerPorId(autorId);
    if (autorAModificar == null)
    {
        Console.WriteLine("No existe un autor con ese ID.");
        return;
    }

    Console.WriteLine("Ingrese el nuevo nombre del autor: ");
    string nuevoNombre = Console.ReadLine();

    autorAModificar.Nombre = nuevoNombre;
    autorRepository.modificar(autorAModificar);
    Console.WriteLine("Autor modificado exitosamente");
}

// Permite cambiar el título de un libro activo existente, seleccionado por Id.
void ModificarLibro()
{
    var libros = libroRepository.ObtenerTodosCon("Autor").Where(l => l.Activo).ToList();
    if (!libros.Any())
    {
        Console.WriteLine("No hay libros activos cargados todavía.");
        return;
    }

    Console.WriteLine(" Libros disponibles ");
    foreach (var libro in libros)
    {
        Console.WriteLine($"{libro.Id} - {libro.Titulo} ({libro.AnioPublicacion}) - Autor: {libro.Autor.Nombre}");
    }

    Console.WriteLine("Ingrese el ID del libro a modificar: ");
    int libroId = int.Parse(Console.ReadLine());

    var libroAModificar = libroRepository.ObtenerPorId(libroId);
    if (libroAModificar == null || !libroAModificar.Activo)
    {
        Console.WriteLine("No existe un libro activo con ese ID.");
        return;
    }

    Console.WriteLine("Ingrese el nuevo título del libro: ");
    string nuevoTitulo = Console.ReadLine();

    libroAModificar.Titulo = nuevoTitulo;
    libroRepository.modificar(libroAModificar);
    Console.WriteLine("Libro modificado exitosamente");
}

// Elimina lógicamente un libro (marca Activo = false) en vez de borrar el registro,
// para conservar el historial y poder distinguir libros activos de eliminados.
void EliminarLibro()
{
    var libros = libroRepository.ObtenerTodosCon("Autor").Where(l => l.Activo).ToList();
    if (!libros.Any())
    {
        Console.WriteLine("No hay libros activos cargados todavía.");
        return;
    }

    Console.WriteLine(" Libros disponibles ");
    foreach (var libro in libros)
    {
        Console.WriteLine($"{libro.Id} - {libro.Titulo} ({libro.AnioPublicacion}) - Autor: {libro.Autor.Nombre}");
    }

    Console.WriteLine("Ingrese el ID del libro a eliminar: ");
    int libroId = int.Parse(Console.ReadLine());

    var libroAEliminar = libroRepository.ObtenerPorId(libroId);
    if (libroAEliminar == null || !libroAEliminar.Activo)
    {
        Console.WriteLine("No existe un libro activo con ese ID.");
        return;
    }

    libroAEliminar.Activo = false;
    libroRepository.modificar(libroAEliminar);
    Console.WriteLine("Libro eliminado exitosamente");
}
