using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Queue<string> filaEspera = new Queue<string>();
        int totalAsientos = 30;
        int totalIngresadas = 0;

        Console.WriteLine("=== SIMULADOR DE ASIGNACIÓN DE ASIENTOS ===");
        Console.WriteLine($"La atracción cuenta con {totalAsientos} asientos disponibles.\n");

        // Ingreso inicial
        Console.Write("¿Cuántas personas desea ingresar a la fila?: ");
        int cantidad = LeerCantidad();

        // Llenar automáticamente los nombres
        AgregarPersonas(filaEspera, ref totalIngresadas, cantidad);

        // Mientras falten personas para completar los 30
        while (totalIngresadas < totalAsientos)
        {
            Console.WriteLine($"\nAún faltan {totalAsientos - totalIngresadas} personas para completar los {totalAsientos} asientos.");
            Console.Write("Ingrese cuántas personas más desea agregar: ");
            int adicionales = LeerCantidad();

            AgregarPersonas(filaEspera, ref totalIngresadas, adicionales);
        }

        Console.WriteLine("\n=== ASIGNACIÓN DE ASIENTOS EN ORDEN DE LLEGADA ===");

        int asiento = 1;
        for (int i = 0; i < totalAsientos && filaEspera.Count > 0; i++)
        {
            string persona = filaEspera.Dequeue();
            Console.WriteLine($"{persona} ha sido asignada al asiento #{asiento}");
            asiento++;
        }

        if (filaEspera.Count > 0)
        {
            Console.WriteLine("\n=== PERSONAS EN ESPERA PARA EL SIGUIENTE TURNO ===");
            int turno = 1;
            foreach (string persona in filaEspera)
            {
                Console.WriteLine($"Turno de espera #{turno}: {persona}");
                turno++;
            }
        }
        else
        {
            Console.WriteLine("\nTodos los asientos fueron asignados sin personas en espera.");
        }
    }

    // Método para leer solo cantidades válidas (>= 1)
    static int LeerCantidad()
    {
        int cantidad;
        while (!int.TryParse(Console.ReadLine(), out cantidad) || cantidad < 1)
        {
            Console.Write("Entrada no válida. Ingrese un número mayor o igual a 1: ");
        }
        return cantidad;
    }

    // Agrega personas con nombre automático al Queue
    static void AgregarPersonas(Queue<string> fila, ref int contador, int cantidad)
    {
        for (int i = 1; i <= cantidad; i++)
        {
            contador++;
            string nombre = $"Persona {contador}";
            fila.Enqueue(nombre);
        }
    }
}
