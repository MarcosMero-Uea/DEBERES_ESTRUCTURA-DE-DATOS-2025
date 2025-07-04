// código_ejercicio3 – Filtrar cadenas mayores a 5 caracteres

using System;
using System.Collections.Generic;
using System.Linq;

class codigo_ejercicio3
{
    static void Main(string[] args)
    {
        // Lista de palabras
        List<string> cadenas = new List<string> { "sol", "estrella", "cielo", "universo", "luz" };

        // Filtramos las que tienen más de 5 caracteres
        List<string> largas = cadenas.Where(c => c.Length > 5).ToList();

        // Mostramos resultados
        Console.WriteLine("Cadenas originales: " + string.Join(", ", cadenas));
        Console.WriteLine("Mayores a 5 caracteres: " + string.Join(", ", largas));
    }
}
