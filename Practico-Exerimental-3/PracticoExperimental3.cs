using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

/*
========================================================================
PRACTICO-EXERIMENTAL-3
Tema: Teoría de conjuntos y mapas aplicada a un torneo de fútbol
Estructuras: Dictionary<string, HashSet<Jugador>>
Reportería + Stopwatch para rendimiento
Correcciones clave (tu error):
  - Se nombran explícitamente las tuplas como (Equipo: ..., Jugador: ...)
    y (Equipo: ..., Dorsales: ...) para evitar que queden (Key, j) o (Key, Value).
  - OrderBy/ThenBy usan esos nombres.
  - Sin MaxBy (compatibilidad .NET anterior).
========================================================================
*/

namespace PracticoExperimental3App
{
    public class Jugador
    {
        public string NombreCompleto { get; }
        public int Dorsal { get; }

        public Jugador(string nombreCompleto, int dorsal)
        {
            NombreCompleto = (nombreCompleto ?? string.Empty).Trim();
            Dorsal = dorsal;
        }

        public override bool Equals(object obj)
        {
            if (obj is Jugador other)
            {
                return string.Equals(NombreCompleto, other.NombreCompleto, StringComparison.OrdinalIgnoreCase)
                       && Dorsal == other.Dorsal;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreCompleto.ToLowerInvariant(), Dorsal);
        }

        public override string ToString() => $"{NombreCompleto} (#{Dorsal})";
    }

    public class TorneoManager
    {
        private readonly Dictionary<string, HashSet<Jugador>> _equipos =
            new(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, HashSet<int>> _dorsalesPorEquipo =
            new(StringComparer.OrdinalIgnoreCase);

        public bool RegistrarEquipo(string nombreEquipo, out string mensaje)
        {
            mensaje = "";
            nombreEquipo = (nombreEquipo ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nombreEquipo))
            {
                mensaje = "El nombre del equipo no puede estar vacío.";
                return false;
            }
            if (_equipos.ContainsKey(nombreEquipo))
            {
                mensaje = "El equipo ya existe.";
                return false;
            }

            _equipos[nombreEquipo] = new HashSet<Jugador>();
            _dorsalesPorEquipo[nombreEquipo] = new HashSet<int>();
            mensaje = "Equipo registrado correctamente.";
            return true;
        }

        public bool RegistrarJugador(string nombreEquipo, string nombreJugador, int dorsal, out string mensaje)
        {
            mensaje = "";
            nombreEquipo = (nombreEquipo ?? string.Empty).Trim();
            nombreJugador = (nombreJugador ?? string.Empty).Trim();

            if (!_equipos.ContainsKey(nombreEquipo))
            {
                mensaje = "El equipo no existe.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(nombreJugador))
            {
                mensaje = "El nombre del jugador no puede estar vacío.";
                return false;
            }
            if (dorsal <= 0)
            {
                mensaje = "El dorsal debe ser un entero positivo.";
                return false;
            }

            var setJugadores = _equipos[nombreEquipo];
            var setDorsales = _dorsalesPorEquipo[nombreEquipo];

            if (setDorsales.Contains(dorsal))
            {
                mensaje = $"El dorsal #{dorsal} ya está usado en el equipo {nombreEquipo}.";
                return false;
            }

            var jugador = new Jugador(nombreJugador, dorsal);
            if (!setJugadores.Add(jugador))
            {
                mensaje = "Ese jugador (nombre + dorsal) ya existe en el equipo.";
                return false;
            }

            setDorsales.Add(dorsal);
            mensaje = "Jugador registrado correctamente.";
            return true;
        }

        public IEnumerable<string> ListarEquipos() =>
            _equipos.Keys.OrderBy(k => k, StringComparer.OrdinalIgnoreCase);

        public IEnumerable<Jugador> ListarJugadoresDe(string nombreEquipo)
        {
            nombreEquipo = (nombreEquipo ?? string.Empty).Trim();
            if (!_equipos.ContainsKey(nombreEquipo)) return Enumerable.Empty<Jugador>();
            return _equipos[nombreEquipo]
                .OrderBy(j => j.Dorsal)
                .ThenBy(j => j.NombreCompleto, StringComparer.OrdinalIgnoreCase);
        }

        // >>>>> CORRECCIÓN: nombrar explícitamente las tuplas (Equipo, Jugador)
        public IEnumerable<(string Equipo, Jugador Jugador)> BuscarJugador(string filtro)
        {
            filtro = (filtro ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(filtro)) return Enumerable.Empty<(string, Jugador)>();

            var query = _equipos.SelectMany(kv =>
                kv.Value
                  .Where(j => j.NombreCompleto.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                  .Select(j => (Equipo: kv.Key, Jugador: j)) // nombres explícitos
            );

            return query
                .OrderBy(t => t.Equipo, StringComparer.OrdinalIgnoreCase)
                .ThenBy(t => t.Jugador.Dorsal);
        }

        public int TotalEquipos() => _equipos.Count;

        public int TotalJugadores() => _equipos.Values.Sum(set => set.Count);

        public (string Equipo, int Cantidad) EquipoConMasJugadores()
        {
            if (_equipos.Count == 0) return ("N/A", 0);
            var max = _equipos.OrderByDescending(kv => kv.Value.Count).First();
            return (max.Key, max.Value.Count);
        }

        // >>>>> CORRECCIÓN: nombrar explícitamente (Equipo, Dorsales)
        public IEnumerable<(string Equipo, IEnumerable<int> Dorsales)> ObtenerDorsalesPorEquipo()
        {
            var query = _dorsalesPorEquipo
                .Select(kv => (Equipo: kv.Key, Dorsales: kv.Value.OrderBy(d => d).AsEnumerable()));

            return query.OrderBy(t => t.Equipo, StringComparer.OrdinalIgnoreCase);
        }

        public (long InsertMs, long SearchMs, int N) MedirRendimiento(int inserciones = 30_000)
        {
            var sw = new Stopwatch();
            string equipoPerf = "__PERF__";

            if (!_equipos.ContainsKey(equipoPerf))
            {
                _equipos[equipoPerf] = new HashSet<Jugador>();
                _dorsalesPorEquipo[equipoPerf] = new HashSet<int>();
            }

            sw.Restart();
            for (int i = 1; i <= inserciones; i++)
            {
                var j = new Jugador($"JugadorPerf{i}", i);
                _equipos[equipoPerf].Add(j);
                _dorsalesPorEquipo[equipoPerf].Add(i);
            }
            sw.Stop();
            long insertMs = sw.ElapsedMilliseconds;

            sw.Restart();
            bool existe = _dorsalesPorEquipo[equipoPerf].Contains(inserciones);
            if (!existe) Console.Write("");
            sw.Stop();
            long searchMs = sw.ElapsedMilliseconds;

            return (insertMs, searchMs, inserciones);
        }
    }

    class PracticoExperimental3
    {
        static void Main(string[] args)
        {
            var gestor = new TorneoManager();

            while (true)
            {
                Console.WriteLine("\n==================== MENÚ ====================");
                Console.WriteLine("1. Registrar equipo");
                Console.WriteLine("2. Registrar jugador en equipo");
                Console.WriteLine("3. Listar equipos");
                Console.WriteLine("4. Listar jugadores de un equipo");
                Console.WriteLine("5. Buscar jugador por nombre");
                Console.WriteLine("6. Reportes");
                Console.WriteLine("7. Medir tiempos de ejecución");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opción: ");

                string opcion = (Console.ReadLine() ?? string.Empty).Trim();
                Console.WriteLine();

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            Console.Write("Nombre del equipo: ");
                            var nomEq = (Console.ReadLine() ?? string.Empty);
                            if (gestor.RegistrarEquipo(nomEq, out var msgEq))
                                Ok(msgEq);
                            else
                                Warn(msgEq);
                            break;

                        case "2":
                            Console.Write("Equipo: ");
                            var eq = (Console.ReadLine() ?? string.Empty);
                            Console.Write("Nombre del jugador: ");
                            var nj = (Console.ReadLine() ?? string.Empty);
                            Console.Write("Dorsal (entero positivo): ");
                            var dorsalInput = (Console.ReadLine() ?? string.Empty);
                            if (!int.TryParse(dorsalInput, out int dorsal) || dorsal <= 0)
                            {
                                Warn("Dorsal inválido.");
                                break;
                            }
                            if (gestor.RegistrarJugador(eq, nj, dorsal, out var msgJ))
                                Ok(msgJ);
                            else
                                Warn(msgJ);
                            break;

                        case "3":
                            Console.WriteLine("== Equipos ==");
                            var equipos = gestor.ListarEquipos().ToList();
                            if (!equipos.Any()) Info("(Sin equipos)");
                            foreach (var e in equipos) Console.WriteLine($"- {e}");
                            break;

                        case "4":
                            Console.Write("Equipo: ");
                            var eqCons = (Console.ReadLine() ?? string.Empty);
                            var jugadores = gestor.ListarJugadoresDe(eqCons).ToList();
                            Console.WriteLine($"== Jugadores de {eqCons} ==");
                            if (!jugadores.Any()) Info("(Sin jugadores o equipo no existe)");
                            foreach (var j in jugadores) Console.WriteLine($"- {j}");
                            break;

                        case "5":
                            Console.Write("Texto a buscar (nombre): ");
                            var filtro = (Console.ReadLine() ?? string.Empty);
                            var resultados = gestor.BuscarJugador(filtro).ToList();
                            Console.WriteLine("== Resultados de búsqueda ==");
                            if (!resultados.Any()) Info("(Sin coincidencias)");
                            foreach (var (equipo, jug) in resultados)
                                Console.WriteLine($"{jug} -> {equipo}");
                            break;

                        case "6":
                            Console.WriteLine("== Reportes ==");
                            Console.WriteLine($"Total equipos: {gestor.TotalEquipos()}");
                            Console.WriteLine($"Total jugadores: {gestor.TotalJugadores()}");
                            var t = gestor.EquipoConMasJugadores();
                            Console.WriteLine($"Equipo con más jugadores: {t.Equipo} ({t.Cantidad})");
                            Console.WriteLine("Dorsales por equipo:");
                            foreach (var par in gestor.ObtenerDorsalesPorEquipo())
                                Console.WriteLine($"- {par.Equipo}: [{string.Join(", ", par.Dorsales)}]");
                            break;

                        case "7":
                            Console.Write("Cantidad de inserciones de prueba (ENTER=30000): ");
                            var txt = (Console.ReadLine() ?? string.Empty).Trim();
                            int n = 30_000;
                            if (!string.IsNullOrWhiteSpace(txt) && int.TryParse(txt, out int nParsed) && nParsed > 0)
                                n = nParsed;

                            var (insMs, searchMs, N) = gestor.MedirRendimiento(n);
                            Console.WriteLine("== Rendimiento (aprox.) ==");
                            Console.WriteLine($"Inserción ({N} jugadores): {insMs} ms");
                            Console.WriteLine($"Búsqueda (Contains dorsal): {searchMs} ms");
                            Info("Nota: los tiempos varían según hardware/entorno.");
                            break;

                        case "0":
                            Console.WriteLine("¡Hasta luego!");
                            return;

                        default:
                            Warn("Opción inválida.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Error($"Se produjo un error: {ex.Message}");
                }
            }
        }

        static void Ok(string m)   => Console.WriteLine($"[OK] {m}");
        static void Warn(string m) => Console.WriteLine($"[AVISO] {m}");
        static void Info(string m) => Console.WriteLine($"[INFO] {m}");
        static void Error(string m)=> Console.WriteLine($"[ERROR] {m}");
    }
}
