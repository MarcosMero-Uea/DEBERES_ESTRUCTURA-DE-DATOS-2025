// código_ejercicio5 – Elementos comunes entre dos listas

using System;
using System.Collections.Generic;
using System.Linq;

class codigo_ejercicio5
{
    static void Main(string[] args)
    {
        // Dos listas de números
        List<int> lista1 = new List<int> { 1, 2, 3, 4, 5 };
        List<int> lista2 = new List<int> { 4, 5, 6, 7, 8 };

        // Obtenemos los elementos comunes
        List<int> comunes = lista1.Intersect(lista2).ToList();

        // Mostramos resultados
        Console.WriteLine("Lista 1: " + string.Join(", ", lista1));
        Console.WriteLine("Lista 2: " + string.Join(", ", lista2));
        Console.WriteLine("Elementos comunes: " + string.Join(", ", comunes));
    }
}
