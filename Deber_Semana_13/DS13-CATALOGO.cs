using System;
using System.Collections.Generic;

namespace CatalogoRevistas
{
    class Program
    {
        // Usamos HashSet para búsquedas O(1) e ignorar mayúsculas/minúsculas
        static HashSet<string> catalogo = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Revista Ciencia Hoy",
            "Tecnología al Día",
            "Salud y Bienestar",
            "Economía Global",
            "Arte y Cultura",
            "Revista de Matemáticas",
            "Literatura Contemporánea",
            "Viajes y Aventuras",
            "Historia Universal",
            "Innovación Educativa"
        };

        static void Main(string[] args)
        {
            int opcion;
            do
            {
                Console.WriteLine("=== Catálogo de Revistas ===");
                Console.WriteLine("1. Buscar título");
                Console.WriteLine("2. Salir");
                Console.Write("Ingrese opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("Opción inválida. Intente de nuevo.\n");
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        BuscarTitulo();
                        break;
                    case 2:
                        Console.WriteLine("Saliendo de la aplicación.");
                        break;
                    default:
                        Console.WriteLine("Opción inválida. Intente de nuevo.");
                        break;
                }

                Console.WriteLine(); // Línea en blanco
            } while (opcion != 2);
        }

        /// <summary>
        /// Solicita el título a buscar y muestra el resultado
        /// </summary>
        static void BuscarTitulo()
        {
            Console.Write("Ingrese el título a buscar: ");
            string? tituloBuscado = Console.ReadLine();

            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(tituloBuscado))
            {
                Console.WriteLine("Entrada vacía. Intente nuevamente.");
                return;
            }

            tituloBuscado = tituloBuscado.Trim();

            bool encontrado = catalogo.Contains(tituloBuscado);

            Console.WriteLine(encontrado ? "Encontrado" : "No encontrado");
        }
    }
}
