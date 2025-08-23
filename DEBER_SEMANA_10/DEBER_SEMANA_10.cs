using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DEBER_SEMANA_10
{
    internal class Program
    {
        const int TOTAL_CIUDADANOS = 500;
        const int PFIZER_COUNT = 75;
        const int ASTRA_COUNT = 75;

        static void Main(string[] args)
        {
            var rngPfizer = new Random(20250823 + 1);
            var rngAstra  = new Random(20250823 + 2);

            string[] ciudadanos = CrearCiudadanos(TOTAL_CIUDADANOS);

            var vacunadosPfizer = TomarMuestra(ciudadanos, PFIZER_COUNT, rngPfizer);
            var vacunadosAstra  = TomarMuestra(ciudadanos, ASTRA_COUNT, rngAstra);

            var alMenosUnaDosis = new HashSet<string>(vacunadosPfizer);
            alMenosUnaDosis.UnionWith(vacunadosAstra);

            var noVacunados = new HashSet<string>(ciudadanos);
            noVacunados.ExceptWith(alMenosUnaDosis);

            var ambasDosis = new HashSet<string>(vacunadosPfizer);
            ambasDosis.IntersectWith(vacunadosAstra);

            var soloPfizer = new HashSet<string>(vacunadosPfizer);
            soloPfizer.ExceptWith(vacunadosAstra);

            var soloAstra = new HashSet<string>(vacunadosAstra);
            soloAstra.ExceptWith(vacunadosPfizer);

            ImprimirResumen(
                TOTAL_CIUDADANOS,
                vacunadosPfizer.Count,
                vacunadosAstra.Count,
                alMenosUnaDosis.Count,
                ambasDosis.Count,
                soloPfizer.Count,
                soloAstra.Count,
                noVacunados.Count
            );

            Directory.CreateDirectory("resultados");
            EscribirArchivo("resultados/NoVacunados.txt", noVacunados);
            EscribirArchivo("resultados/AmbasDosis.txt", ambasDosis);
            EscribirArchivo("resultados/SoloPfizer.txt", soloPfizer);
            EscribirArchivo("resultados/SoloAstraZeneca.txt", soloAstra);

            Console.WriteLine("\nArchivos generados en la carpeta 'resultados/'.");
        }

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
