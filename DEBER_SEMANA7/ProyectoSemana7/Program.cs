
using System;
using System.Collections.Generic;

class Programa
{
    static void Main()
    {
        Console.WriteLine("Ingrese una expresión matemática:");
        string expresion = Console.ReadLine(); // Entrada del usuario

        if (EstaBalanceada(expresion))
        {
            Console.WriteLine("Fórmula balanceada.");
        }
        else
        {
            Console.WriteLine("Fórmula NO balanceada.");
        }
    }

    // Función principal que evalúa si la expresión está balanceada
    static bool EstaBalanceada(string expresion)
    {
        Stack<char> pila = new Stack<char>(); // Pila para manejar los símbolos

        foreach (char c in expresion)
        {
            // Si el carácter es símbolo de apertura, se apila
            if (c == '(' || c == '{' || c == '[')
            {
                pila.Push(c);
            }
            // Si es símbolo de cierre, se compara con el tope de la pila
            else if (c == ')' || c == '}' || c == ']')
            {
                if (pila.Count == 0) return false; // No hay símbolo abierto para cerrar

                char tope = pila.Pop();
                if (!Coinciden(tope, c)) return false; // No coincide el tipo
            }
        }

        return pila.Count == 0; // Si la pila está vacía al final, está balanceada
    }

    // Verifica si el par de apertura y cierre coinciden
    static bool Coinciden(char apertura, char cierre)
    {
        return (apertura == '(' && cierre == ')') ||
               (apertura == '{' && cierre == '}') ||
               (apertura == '[' && cierre == ']');
    }
}