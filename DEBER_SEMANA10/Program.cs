using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DEBER_SEMANA10
{
    internal class Program
    {
        // Parámetros del ejercicio
        const int TOTAL_CIUDADANOS = 500;
        const int PFIZER_COUNT = 75;
        const int ASTRA_COUNT = 75;

        static void Main(string[] args)
        {
            // Semillas fijas para reproducibilidad
            var rngPfizer = new Random(20250823 + 1);
            var rngAstra  = new Random(20250823 + 2);

            // UNIVERSO U: 500 ciudadanos ficticios
            string[] ciudadanos = CrearCiudadanos(TOTAL_CIUDADANOS);

            // P: vacunados con Pfizer (|P| = 75)
            // A: vacunados con AstraZeneca (|A| = 75)
            var vacunadosPfizer = TomarMuestra(ciudadanos, PFIZER_COUNT, rngPfizer);
            var vacunadosAstra  = TomarMuestra(ciudadanos, ASTRA_COUNT,  rngAstra);

            // Operaciones de teoría de conjuntos
            // Unión: P ∪ A (al menos una dosis)
            var alMenosUnaDosis = new HashSet<string>(vacunadosPfizer);
            alMenosUnaDosis.UnionWith(vacunadosAstra);

            // No vacunados: U \ (P ∪ A)
            var noVacunados = new HashSet<string>(ciudadanos);
            noVacunados.ExceptWith(alMenosUnaDosis);

            // Ambas dosis (interpretación: aparece en ambos conjuntos = P ∩ A)
            var ambasDosis = new HashSet<string>(vacunadosPfizer);
            ambasDosis.IntersectWith(vacunadosAstra);

            // Solo Pfizer: P \ A
            var soloPfizer = new HashSet<string>(vacunadosPfizer);
            soloPfizer.ExceptWith(vacunadosAstra);

            // Solo AstraZeneca: A \ P
            var soloAstra = new HashSet<string>(vacunadosAstra);
            soloAstra.ExceptWith(vacunadosPfizer);

            // Resumen en consola
            ImprimirResumen(
                total: TOTAL_CIUDADANOS,
                pfizer: vacunadosPfizer.Count,
                astra: vacunadosAstra.Count,
                union: alMenosUnaDosis.Count,
                interseccion: ambasDosis.Count,
                soloP: soloPfizer.Count,
                soloA: soloAstra.Count,
                noVac: noVacunados.Count
            );

            // Guardar archivos
            Directory.CreateDirectory("resultados");
            EscribirArchivo("resultados/NoVacunados.txt", noVacunados);
            EscribirArchivo("resultados/AmbasDosis.txt",  ambasDosis);
            EscribirArchivo("resultados/SoloPfizer.txt",  soloPfizer);
            EscribirArchivo("resultados/SoloAstraZeneca.txt", soloAstra);

            Console.WriteLine("\nListados generados en la carpeta 'resultados/'.");
            Console.WriteLine("Sube el proyecto a tu repositorio público y revisa que sea accesible.");
        }

        // ---------- Utilidades ----------
        static string[] CrearCiudadanos(int n) =>
            Enumerable.Range(1, n).Select(i => $"Ciudadano {i}").ToArray();

        static HashSet<string> TomarMuestra(string[] universo, int n, Random rng)
        {
            var muestra = new HashSet<string>();
            while (muestra.Count < n)
            {
                int idx = rng.Next(universo.Length);
                muestra.Add(universo[idx]);
            }
            return muestra;
        }

        static void EscribirArchivo(string ruta, IEnumerable<string> datos)
        {
            File.WriteAllLines(ruta, datos.OrderBy(x => Numero(x)));
        }

        static int Numero(string nombre)
        {
            // "Ciudadano X" -> X
            var partes = nombre.Split(' ');
            if (partes.Length >= 2 && int.TryParse(partes[^1], out int n))
                return n;
            return int.MaxValue;
        }

        static void ImprimirResumen(int total, int pfizer, int astra, int union, int interseccion, int soloP, int soloA, int noVac)
        {
            Console.WriteLine("===== Resumen de Vacunación (Teoría de Conjuntos) =====");
            Console.WriteLine($"Total de ciudadanos (U): {total}");
            Console.WriteLine($"Pfizer (P): {pfizer}");
            Console.WriteLine($"AstraZeneca (A): {astra}");
            Console.WriteLine($"Al menos una dosis (P ∪ A): {union}");
            Console.WriteLine($"Ambas dosis (P ∩ A): {interseccion}");
            Console.WriteLine($"Solo Pfizer (P \\ A): {soloP}");
            Console.WriteLine($"Solo AstraZeneca (A \\ P): {soloA}");
            Console.WriteLine($"No vacunados (U \\ (P ∪ A)): {noVac}");
        }
    }
}
