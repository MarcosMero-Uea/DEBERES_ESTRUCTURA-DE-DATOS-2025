// codigo_ejercicio_2.cs
// Título: Registro de estudiantes - Lista Enlazada con inserción diferenciada (modo interactivo)

using System;

// Clase que representa un nodo (estudiante) de la lista enlazada
class Estudiante
{
    public string Cedula { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Correo { get; set; }
    public double NotaDefinitiva { get; set; }
    public Estudiante Siguiente { get; set; }

    public Estudiante(string cedula, string nombre, string apellido, string correo, double nota)
    {
        Cedula = cedula;
        Nombre = nombre;
        Apellido = apellido;
        Correo = correo;
        NotaDefinitiva = nota;
        Siguiente = null;
    }
}

// Clase que representa la lista enlazada de estudiantes
class ListaEstudiantes
{
    private Estudiante head;

    public ListaEstudiantes()
    {
        head = null;
    }

    // Agrega estudiante: si aprueba (>=7) al inicio, si reprueba al final
    public void AgregarEstudiante(string cedula, string nombre, string apellido, string correo, double nota)
    {
        Estudiante nuevo = new Estudiante(cedula, nombre, apellido, correo, nota);

        if (nota >= 7)
        {
            nuevo.Siguiente = head;
            head = nuevo;
        }
        else
        {
            if (head == null)
            {
                head = nuevo;
            }
            else
            {
                Estudiante actual = head;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevo;
            }
        }
    }

    // Buscar estudiante por cédula
    public Estudiante BuscarPorCedula(string cedula)
    {
        Estudiante actual = head;
        while (actual != null)
        {
            if (actual.Cedula == cedula)
                return actual;
            actual = actual.Siguiente;
        }
        return null;
    }

    // Eliminar estudiante por cédula
    public void EliminarEstudiante(string cedula)
    {
        if (head == null) return;

        if (head.Cedula == cedula)
        {
            head = head.Siguiente;
            return;
        }

        Estudiante actual = head;
        while (actual.Siguiente != null)
        {
            if (actual.Siguiente.Cedula == cedula)
            {
                actual.Siguiente = actual.Siguiente.Siguiente;
                return;
            }
            actual = actual.Siguiente;
        }
    }

    // Total estudiantes aprobados (nota >= 7)
    public int TotalAprobados()
    {
        int contador = 0;
        Estudiante actual = head;
        while (actual != null)
        {
            if (actual.NotaDefinitiva >= 7)
                contador++;
            actual = actual.Siguiente;
        }
        return contador;
    }

    // Total estudiantes reprobados (nota < 7)
    public int TotalReprobados()
    {
        int contador = 0;
        Estudiante actual = head;
        while (actual != null)
        {
            if (actual.NotaDefinitiva < 7)
                contador++;
            actual = actual.Siguiente;
        }
        return contador;
    }

    // Muestra todos los estudiantes
    public void MostrarEstudiantes()
    {
        Estudiante actual = head;
        Console.WriteLine("\nLista de estudiantes registrados:");
        while (actual != null)
        {
            Console.WriteLine("Cédula: {0}, Nombre: {1} {2}, Correo: {3}, Nota: {4}",
                actual.Cedula, actual.Nombre, actual.Apellido, actual.Correo, actual.NotaDefinitiva);
            actual = actual.Siguiente;
        }
    }
}

// Clase principal con menú interactivo
class Program
{
    static void Main()
    {
        ListaEstudiantes lista = new ListaEstudiantes();
        string opcion;

        do
        {
            Console.WriteLine("\n--- MENÚ DE OPCIONES ---");
            Console.WriteLine("1. Agregar estudiante");
            Console.WriteLine("2. Buscar estudiante por cédula");
            Console.WriteLine("3. Eliminar estudiante por cédula");
            Console.WriteLine("4. Mostrar todos los estudiantes");
            Console.WriteLine("5. Total de aprobados y reprobados");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opción: ");
            opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.Write("Cédula: ");
                    string cedula = Console.ReadLine();

                    Console.Write("Nombre: ");
                    string nombre = Console.ReadLine();

                    Console.Write("Apellido: ");
                    string apellido = Console.ReadLine();

                    Console.Write("Correo: ");
                    string correo = Console.ReadLine();

                    Console.Write("Nota definitiva (1-10): ");
                    double nota = Convert.ToDouble(Console.ReadLine());

                    lista.AgregarEstudiante(cedula, nombre, apellido, correo, nota);
                    Console.WriteLine("✅ Estudiante agregado correctamente.");
                    break;

                case "2":
                    Console.Write("Ingrese la cédula a buscar: ");
                    string cedBuscar = Console.ReadLine();
                    Estudiante encontrado = lista.BuscarPorCedula(cedBuscar);
                    if (encontrado != null)
                    {
                        Console.WriteLine("Estudiante encontrado:");
                        Console.WriteLine("Cédula: {0}, Nombre: {1} {2}, Correo: {3}, Nota: {4}",
                            encontrado.Cedula, encontrado.Nombre, encontrado.Apellido, encontrado.Correo, encontrado.NotaDefinitiva);
                    }
                    else
                    {
                        Console.WriteLine("⚠️ Estudiante no encontrado.");
                    }
                    break;

                case "3":
                    Console.Write("Ingrese la cédula a eliminar: ");
                    string cedEliminar = Console.ReadLine();
                    lista.EliminarEstudiante(cedEliminar);
                    Console.WriteLine("🗑️ Estudiante eliminado (si existía).");
                    break;

                case "4":
                    lista.MostrarEstudiantes();
                    break;

                case "5":
                    Console.WriteLine("✅ Total Aprobados: {0}", lista.TotalAprobados());
                    Console.WriteLine("❌ Total Reprobados: {0}", lista.TotalReprobados());
                    break;

                case "0":
                    Console.WriteLine("👋 Saliendo del programa...");
                    break;

                default:
                    Console.WriteLine("⚠️ Opción no válida.");
                    break;
            }

        } while (opcion != "0");
    }
}
