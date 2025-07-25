// código_ejercicio1 – Elementos únicos de una lista

using System;
using System.Collections.Generic;
using System.Linq;

class codigo_ejercicio1
{
    static void Main(string[] args)
    {
        // Lista con elementos duplicados
        List<int> numeros = new List<int> { 1, 2, 2, 3, 4, 4, 5 };

        // Usamos Distinct() para obtener elementos únicos
        List<int> unicos = numeros.Distinct().ToList();

        // Mostramos resultados
        Console.WriteLine("Lista original: " + string.Join(", ", numeros));
        Console.WriteLine("Elementos únicos: " + string.Join(", ", unicos));
    }
}