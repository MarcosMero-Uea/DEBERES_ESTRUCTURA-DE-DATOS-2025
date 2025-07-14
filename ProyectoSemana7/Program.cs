using System;
using System.Collections.Generic;

class TorresDeHanoi
{
    // Pilas para representar las tres torres
    static Stack<int> origen = new Stack<int>();
    static Stack<int> auxiliar = new Stack<int>();
    static Stack<int> destino = new Stack<int>();

    static void Main()
    {
        int numDiscos = 3; // Número de discos a mover

        // Apilamos los discos en la torre de origen en orden descendente (mayor abajo)
        for (int i = numDiscos; i >= 1; i--)
        {
            origen.Push(i);
        }

        Console.WriteLine("Movimientos para resolver Torres de Hanoi con " + numDiscos + " discos:");
        MoverDiscos(numDiscos, origen, destino, auxiliar, "Origen", "Destino", "Auxiliar");
    }

    // Función recursiva que realiza los movimientos necesarios
    static void MoverDiscos(int n, Stack<int> origen, Stack<int> destino, Stack<int> auxiliar, string nombreOrigen, string nombreDestino, string nombreAuxiliar)
    {
        if (n == 1)
        {
            // Caso base: mover un solo disco directamente
            destino.Push(origen.Pop());
            Console.WriteLine($"Mover disco 1 de {nombreOrigen} a {nombreDestino}");
            return;
        }

        // Paso 1: mover n-1 discos al auxiliar
        MoverDiscos(n - 1, origen, auxiliar, destino, nombreOrigen, nombreAuxiliar, nombreDestino);

        // Paso 2: mover el disco restante al destino
        destino.Push(origen.Pop());
        Console.WriteLine($"Mover disco {n} de {nombreOrigen} a {nombreDestino}");

        // Paso 3: mover los n-1 discos desde auxiliar al destino
        MoverDiscos(n - 1, auxiliar, destino, origen, nombreAuxiliar, nombreDestino, nombreOrigen);
    }
}
