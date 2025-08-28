using System;
using System.Collections.Generic;
using System.Text;

//
// DEBER SEMANA 11 – Traductor básico (Diccionarios)
// - Traduce frases ES⇄EN usando SOLO las palabras registradas en el diccionario.
// - Permite agregar nuevas palabras al diccionario (en ambos sentidos).
// - Preserva espacios y signos; respeta la capitalización del original.
// Autor: (tu nombre)
//

internal class DS11_Diccionario
{
    // Diccionarios base: Inglés -> Español y Español -> Inglés
    // Se usan con comparador "OrdinalIgnoreCase" para no diferenciar mayúsculas/minúsculas
    private static readonly Dictionary<string, string> engToEs =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"time","tiempo"},
            {"person","persona"},
            {"year","año"},
            {"way","camino"},     // (también "forma")
            {"day","día"},
            {"thing","cosa"},
            {"man","hombre"},
            {"world","mundo"},
            {"life","vida"},
            {"hand","mano"},
            {"part","parte"},
            {"child","niño"},
            {"eye","ojo"},
            {"woman","mujer"},
            {"place","lugar"},
            {"work","trabajo"},
            {"week","semana"},
            {"case","caso"},
            {"point","punto"},    // (también "tema")
            {"government","gobierno"},
            {"company","compañía"} // o "empresa"
        };

    private static readonly Dictionary<string, string> esToEng =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    private static void InicializarInverso()
    {
        // Construye el diccionario inverso ES->EN a partir de EN->ES
        foreach (var kv in engToEs)
        {
            // Si varias palabras en inglés apuntaran al mismo español,
            // aquí quedaría la última; para este deber es suficiente.
            esToEng[kv.Value] = kv.Key;
        }
    }

    // --------------------- PUNTO DE ENTRADA ---------------------
    private static void Main()
    {
        InicializarInverso();

        int opcion;
        do
        {
            Console.WriteLine("==================== MENÚ ====================");
            Console.WriteLine("1. Traducir una frase");
            Console.WriteLine("2. Agregar palabras al diccionario");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opción: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                Console.WriteLine("Ingrese una opción válida.\n");
                continue;
            }

            Console.WriteLine(); // separación visual

            switch (opcion)
            {
                case 1:
                    OpcionTraducir();
                    break;

                case 2:
                    OpcionAgregarPalabra();
                    break;

                case 0:
                    Console.WriteLine("Saliendo...");
                    break;

                default:
                    Console.WriteLine("Opción no válida.\n");
                    break;
            }
        } while (opcion != 0);
    }

    // --------------------- OPCIÓN 1: TRADUCIR ---------------------
    private static void OpcionTraducir()
    {
        // El usuario elige el sentido de la traducción
        Console.WriteLine("Seleccione el sentido de traducción:");
        Console.WriteLine("1) Español  → Inglés");
        Console.WriteLine("2) Inglés   → Español");
        Console.Write("Opción: ");

        var modo = Console.ReadLine();
        Dictionary<string, string> diccionario;

        if (modo == "1")
            diccionario = esToEng;
        else if (modo == "2")
            diccionario = engToEs;
        else
        {
            Console.WriteLine("Opción de traducción inválida.\n");
            return;
        }

        Console.WriteLine("\nIngrese la frase a traducir:");
        string frase = Console.ReadLine() ?? string.Empty;

        string traducida = TraducirFrasePreservandoSignos(frase, diccionario);

        Console.WriteLine("\nFrase traducida (solo palabras registradas):");
        Console.WriteLine(traducida + "\n");
    }

    // ------------------ OPCIÓN 2: AGREGAR PALABRA ----------------
    private static void OpcionAgregarPalabra()
    {
        // Pedimos ambas formas para mantener el traductor bidireccional
        Console.WriteLine("Agregar palabra al diccionario (se guardará en ambos sentidos).");
        Console.Write("Palabra en inglés: ");
        string en = (Console.ReadLine() ?? "").Trim();

        if (string.IsNullOrWhiteSpace(en))
        {
            Console.WriteLine("La palabra en inglés no puede estar vacía.\n");
            return;
        }

        Console.Write("Traducción en español: ");
        string es = (Console.ReadLine() ?? "").Trim();

        if (string.IsNullOrWhiteSpace(es))
        {
            Console.WriteLine("La traducción en español no puede estar vacía.\n");
            return;
        }

        engToEs[en] = es; // inserta o actualiza
        esToEng[es] = en; // inserta o actualiza

        Console.WriteLine("✅ Palabra registrada/actualizada en ambos diccionarios.\n");
    }

    // ---------------- NÚCLEO DE TRADUCCIÓN / UTILIDADES ----------------

    // Traduce SOLO tokens que existan como clave en el diccionario.
    // Se preservan los separadores (espacios, signos, números, etc.)
    private static string TraducirFrasePreservandoSignos(string frase, Dictionary<string, string> diccionario)
    {
        var tokens = Tokenizar(frase);

        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].EsPalabra && diccionario.TryGetValue(tokens[i].Texto, out string traduccion))
            {
                tokens[i].Texto = AplicarCapitalizacion(tokens[i].Texto, traduccion);
            }
        }

        return Reconstruir(tokens);
    }

    // Representa un fragmento de texto: palabra (letras) o separador (lo demás)
    private class Token
    {
        public string Texto;
        public bool EsPalabra;
        public Token(string texto, bool esPalabra) { Texto = texto; EsPalabra = esPalabra; }
    }

    // Divide el texto en tokens preservando absolutamente todo lo que el usuario escribió.
    private static List<Token> Tokenizar(string texto)
    {
        var res = new List<Token>();
        if (string.IsNullOrEmpty(texto)) return res;

        int i = 0;
        while (i < texto.Length)
        {
            if (char.IsLetter(texto, i)) // incluye letras con tilde; funciona en Unicode
            {
                int ini = i;
                while (i < texto.Length && char.IsLetter(texto, i)) i++;
                res.Add(new Token(texto[ini..i], true));
            }
            else
            {
                int ini = i;
                while (i < texto.Length && !char.IsLetter(texto, i)) i++;
                res.Add(new Token(texto[ini..i], false));
            }
        }
        return res;
    }

    // Reconstruye la cadena a partir de los tokens traducidos
    private static string Reconstruir(List<Token> tokens)
    {
        var sb = new StringBuilder(tokens.Count * 6);
        foreach (var t in tokens) sb.Append(t.Texto);
        return sb.ToString();
    }

    // Copia la "forma" de capitalización del original a la traducción:
    // - todo minúsculas → minúsculas
    // - Título (Primera Mayúscula) → Título
    // - TODO MAYÚSCULAS → TODO MAYÚSCULAS
    private static string AplicarCapitalizacion(string original, string traduccion)
    {
        if (EsTodoMayus(original)) return traduccion.ToUpper();
        if (EsTitulo(original))    return CapitalizarPrimera(traduccion);
        return traduccion.ToLower();
    }

    private static bool EsTodoMayus(string s)
    {
        bool hayLetra = false;
        foreach (var c in s)
        {
            if (char.IsLetter(c))
            {
                hayLetra = true;
                if (!char.IsUpper(c)) return false;
            }
        }
        return hayLetra;
    }

    private static bool EsTitulo(string s)
    {
        if (s.Length == 0) return false;
        // Primera en mayúscula y el resto minúsculas
        return char.IsUpper(s[0]) && s.Substring(1).ToLower() == s.Substring(1);
    }

    private static string CapitalizarPrimera(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        if (s.Length == 1) return s.ToUpper();
        return char.ToUpper(s[0]) + s.Substring(1).ToLower();
    }
}
