// código_ejercicio4 – Calcular el promedio de una lista de números

using System;
using System.Collections.Generic;
using System.Linq;

class codigo_ejercicio4
{
    static void Main(string[] args)
    {
        // Lista de valores decimales
        List<double> numeros = new List<double> { 10.5, 20.3, 15.2, 9.5 };

        // Calculamos el promedio
        double promedio = numeros.Average();

        // Mostramos resultados
        Console.WriteLine("Números: " + string.Join(", ", numeros));
        Console.WriteLine("Promedio: " + promedio.ToString("F2"));
    }
}
