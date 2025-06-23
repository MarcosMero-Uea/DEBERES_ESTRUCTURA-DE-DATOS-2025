using System;
using System.Collections.Generic;

namespace ClinicaTurnos
{
    class Program
    {
        struct PacienteTurno
        {
            public string nombre;
            public string cedula;
            public DateTime fechaHora;
            public string medico;
        }

        static List<PacienteTurno> listaTurnos = new List<PacienteTurno>();

        static void Main(string[] args)
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("=== SISTEMA DE AGENDAMIENTO DE TURNOS ===");
                Console.WriteLine("1. Registrar turno");
                Console.WriteLine("2. Mostrar turnos registrados");
                Console.WriteLine("3. Buscar turno por cédula");
                Console.WriteLine("4. Eliminar turno por cédula");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");
                opcion = int.Parse(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        RegistrarTurno();
                        break;
                    case 2:
                        MostrarTurnos();
                        break;
                    case 3:
                        BuscarTurno();
                        break;
                    case 4:
                        EliminarTurno();
                        break;
                    case 5:
                        Console.WriteLine("Saliendo del sistema...");
                        break;
                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }

                Console.WriteLine("\nPresione una tecla para continuar...");
                Console.ReadKey();

            } while (opcion != 5);
        }

        static void RegistrarTurno()
        {
            PacienteTurno nuevoTurno;

            Console.Write("Nombre del paciente: ");
            nuevoTurno.nombre = Console.ReadLine();

            Console.Write("Cédula: ");
            nuevoTurno.cedula = Console.ReadLine();

            Console.Write("Fecha del turno (dd/mm/aaaa): ");
            string fecha = Console.ReadLine();

            Console.Write("Hora del turno (hh:mm): ");
            string hora = Console.ReadLine();

            Console.Write("Médico asignado: ");
            nuevoTurno.medico = Console.ReadLine();

            DateTime fechaHora;
            if (!DateTime.TryParse($"{fecha} {hora}", out fechaHora))
            {
                Console.WriteLine("Formato de fecha y hora inválido.");
                return;
            }

            // Validación avanzada de disponibilidad para el médico
            bool conflicto;
            do
            {
                conflicto = false;
                foreach (var turno in listaTurnos)
                {
                    if (turno.medico == nuevoTurno.medico)
                    {
                        TimeSpan diferencia = (turno.fechaHora - fechaHora).Duration();

                        if (diferencia.TotalMinutes < 40)
                        {
                            conflicto = true;
                            Console.WriteLine($"El Dr. {nuevoTurno.medico} tiene otra cita cerca de esta hora.");
                            fechaHora = fechaHora.AddMinutes(40);
                            Console.WriteLine($"Nuevo horario asignado automáticamente: {fechaHora:dd/MM/yyyy HH:mm}");
                            break;
                        }
                    }
                }
            } while (conflicto);

            nuevoTurno.fechaHora = fechaHora;
            listaTurnos.Add(nuevoTurno);

            Console.WriteLine("Turno registrado correctamente.");
        }

        static void MostrarTurnos()
        {
            if (listaTurnos.Count == 0)
            {
                Console.WriteLine("No hay turnos registrados.");
                return;
            }

            Console.WriteLine("\n=== LISTA DE TURNOS ===");
            foreach (var turno in listaTurnos)
            {
                Console.WriteLine($"Nombre: {turno.nombre} | Cédula: {turno.cedula} | Fecha y hora: {turno.fechaHora:dd/MM/yyyy HH:mm} | Médico: {turno.medico}");
            }
        }

        static void BuscarTurno()
        {
            Console.Write("Ingrese la cédula del paciente: ");
            string cedula = Console.ReadLine();
            var turnos = listaTurnos.FindAll(t => t.cedula == cedula);

            if (turnos.Count > 0)
            {
                foreach (var turno in turnos)
                {
                    Console.WriteLine($"Nombre: {turno.nombre} | Fecha y hora: {turno.fechaHora:dd/MM/yyyy HH:mm} | Médico: {turno.medico}");
                }
            }
            else
            {
                Console.WriteLine("Turno no encontrado.");
            }
        }

        static void EliminarTurno()
        {
            Console.Write("Ingrese la cédula del paciente a eliminar: ");
            string cedula = Console.ReadLine();
            int index = listaTurnos.FindIndex(t => t.cedula == cedula);

            if (index != -1)
            {
                listaTurnos.RemoveAt(index);
                Console.WriteLine("Turno eliminado correctamente.");
            }
            else
            {
                Console.WriteLine("No se encontró un turno con esa cédula.");
            }
        }
    }
}
