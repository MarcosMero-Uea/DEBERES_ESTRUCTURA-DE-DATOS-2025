// codigo_ejercicio_1.cs
// Título: Registro de vehículos - Lista Enlazada con interacción por menú

using System;

class Vehiculo
{
    public string Placa { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Anio { get; set; }
    public double Precio { get; set; }
    public Vehiculo Siguiente { get; set; }

    public Vehiculo(string placa, string marca, string modelo, int anio, double precio)
    {
        Placa = placa;
        Marca = marca;
        Modelo = modelo;
        Anio = anio;
        Precio = precio;
        Siguiente = null;
    }
}

class ListaVehiculos
{
    private Vehiculo head;

    public ListaVehiculos()
    {
        head = null;
    }

    public void AgregarVehiculo(string placa, string marca, string modelo, int anio, double precio)
    {
        Vehiculo nuevo = new Vehiculo(placa, marca, modelo, anio, precio);
        nuevo.Siguiente = head;
        head = nuevo;
    }

    public Vehiculo BuscarPorPlaca(string placa)
    {
        Vehiculo actual = head;
        while (actual != null)
        {
            if (actual.Placa == placa)
                return actual;
            actual = actual.Siguiente;
        }
        return null;
    }

    public void MostrarTodos()
    {
        Vehiculo actual = head;
        Console.WriteLine("\nVehículos registrados:");
        while (actual != null)
        {
            Console.WriteLine("Placa: {0}, Marca: {1}, Modelo: {2}, Año: {3}, Precio: {4}",
                actual.Placa, actual.Marca, actual.Modelo, actual.Anio, actual.Precio);
            actual = actual.Siguiente;
        }
    }

    public void MostrarPorAnio(int anio)
    {
        Vehiculo actual = head;
        Console.WriteLine("\nVehículos del año {0}:", anio);
        bool encontrado = false;
        while (actual != null)
        {
            if (actual.Anio == anio)
            {
                Console.WriteLine("Placa: {0}, Marca: {1}, Modelo: {2}, Precio: {3}",
                    actual.Placa, actual.Marca, actual.Modelo, actual.Precio);
                encontrado = true;
            }
            actual = actual.Siguiente;
        }
        if (!encontrado)
        {
            Console.WriteLine("No se encontraron vehículos de ese año.");
        }
    }

    public void EliminarVehiculo(string placa)
    {
        if (head == null) return;

        if (head.Placa == placa)
        {
            head = head.Siguiente;
            return;
        }

        Vehiculo actual = head;
        while (actual.Siguiente != null)
        {
            if (actual.Siguiente.Placa == placa)
            {
                actual.Siguiente = actual.Siguiente.Siguiente;
                return;
            }
            actual = actual.Siguiente;
        }
    }
}

class Program
{
    static void Main()
    {
        ListaVehiculos lista = new ListaVehiculos();
        string opcion;

        do
        {
            Console.WriteLine("\n--- MENÚ DE VEHÍCULOS ---");
            Console.WriteLine("1. Agregar vehículo");
            Console.WriteLine("2. Buscar vehículo por placa");
            Console.WriteLine("3. Eliminar vehículo por placa");
            Console.WriteLine("4. Mostrar todos los vehículos");
            Console.WriteLine("5. Mostrar vehículos por año");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opción: ");
            opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.Write("Placa: ");
                    string placa = Console.ReadLine();

                    Console.Write("Marca: ");
                    string marca = Console.ReadLine();

                    Console.Write("Modelo: ");
                    string modelo = Console.ReadLine();

                    Console.Write("Año: ");
                    int anio = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Precio: ");
                    double precio = Convert.ToDouble(Console.ReadLine());

                    lista.AgregarVehiculo(placa, marca, modelo, anio, precio);
                    Console.WriteLine("Vehículo agregado correctamente.");
                    break;

                case "2":
                    Console.Write("Ingrese la placa a buscar: ");
                    string buscar = Console.ReadLine();
                    Vehiculo encontrado = lista.BuscarPorPlaca(buscar);
                    if (encontrado != null)
                    {
                        Console.WriteLine("Vehículo encontrado:");
                        Console.WriteLine("Placa: {0}, Marca: {1}, Modelo: {2}, Año: {3}, Precio: {4}",
                            encontrado.Placa, encontrado.Marca, encontrado.Modelo, encontrado.Anio, encontrado.Precio);
                    }
                    else
                    {
                        Console.WriteLine("Vehículo no encontrado.");
                    }
                    break;

                case "3":
                    Console.Write("Ingrese la placa a eliminar: ");
                    string eliminar = Console.ReadLine();
                    lista.EliminarVehiculo(eliminar);
                    Console.WriteLine("Vehículo eliminado (si existía).");
                    break;

                case "4":
                    lista.MostrarTodos();
                    break;

                case "5":
                    Console.Write("Ingrese el año: ");
                    int anioConsulta = Convert.ToInt32(Console.ReadLine());
                    lista.MostrarPorAnio(anioConsulta);
                    break;

                case "0":
                    Console.WriteLine("Saliendo del sistema...");
                    break;

                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }

        } while (opcion != "0");
    }
}
