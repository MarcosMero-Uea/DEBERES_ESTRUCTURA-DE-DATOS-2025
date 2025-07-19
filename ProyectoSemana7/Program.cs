using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Queue<string> filaEspera = new Queue<string>();
        int totalAsientos = 30;

        Console.WriteLine("=== SIMULADOR DE ASIGNACIÓN DE ASIENTOS ===");
        Console.WriteLine($"La atracción cuenta con {totalAsientos} asientos disponibles.");

        Console.WriteLine("\n¿Cuántas personas desea ingresar a la fila?");
        Console.Write("Ingrese un número mayor o igual a 1: ");

        int cantidadTotal = 0;
        while (!int.TryParse(Console.ReadLine(), out cantidadTotal) || cantidadTotal < 1)
        {
            Console.Write("Entrada no válida. Por favor, ingrese un número mayor o igual a 1: ");
        }

        // Ingreso de todas las personas
        for (int i = 1; i <= cantidadTotal; i++)
        {
            Console.Write($"Ingrese el nombre de la persona #{i}: ");
            string nombre = Console.ReadLine();
            filaEspera.Enqueue(nombre);
        }

        // Asignación de los primeros 30 asientos
        Console.WriteLine("\n=== ASIGNACIÓN DE ASIENTOS EN ORDEN DE LLEGADA ===");

        int asiento = 1;
        for (int i = 0; i < totalAsientos && filaEspera.Count > 0; i++)
        {
            string persona = filaEspera.Dequeue();
            Console.WriteLine($"{persona} ha sido asignada al asiento #{asiento}");
            asiento++;
        }

        // Mostrar las personas que quedaron en espera
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
}