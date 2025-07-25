// código_ejercicio2 – Filtrar números pares de una lista

using System;
using System.Collections.Generic;
using System.Linq;

class codigo_ejercicio2
{
    static void Main(string[] args)
    {
        // Lista con varios números
        List<int> numeros = new List<int> { 1, 4, 5, 8, 9, 10 };

        // Filtramos solo los números pares
        List<int> pares = numeros.Where(n => n % 2 == 0).ToList();

        // Mostramos resultados
        Console.WriteLine("Números originales: " + string.Join(", ", numeros));
        Console.WriteLine("Números pares: " + string.Join(", ", pares));
    }
}
