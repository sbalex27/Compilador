using System;

namespace SintacticoNamespace
{
    public class Sintactico
    {
        private Lexico lexico; // Objeto léxico miembro de la clase

        public Sintactico(string fuente, int traza)
        {
            lexico = new Lexico(fuente, traza); // Se inicializa el constructor de la clase léxico

            if (lexico.ExisteTraza())
                Console.WriteLine("INICIO DE ANALISIS SINTACTICO");

            Programa();
        }

        ~Sintactico()
        {
            if (lexico.ExisteTraza())
            {
                Console.WriteLine("FIN DE ANALISIS SINTACTICO");
                Console.WriteLine("FIN DE COMPILACION");
            }
        }

        private void Programa()
        {
            char token;
            if (lexico.ExisteTraza())
                Console.WriteLine("ANALISIS SINTACTICO: <PROGRAMA>");

            token = lexico.SiguienteToken();
            if (token == 'M')
                Console.Write("M");
            else
                Errores(8);

            token = lexico.SiguienteToken();
            if (token != '{')
                Errores(9);

            Bloque();

            token = lexico.SiguienteToken();
            if (token == '}')
            {
                Console.Write("}");
            }
            else
                Errores(2);
        }

        private void Bloque()
        {
            char token = ' ';
            if (lexico.ExisteTraza())
                Console.WriteLine("ANALISIS SINTACTICO: <BLOQUE>");

            Sentencia();
            OtraSentencia();
        }

        private void OtraSentencia()
        {
            char token;
            if (lexico.ExisteTraza())
                Console.WriteLine("ANALISIS SINTACTICO: <OTRA_SENTENCIA>");

            token = lexico.SiguienteToken();
            if (token == ';')
            {
                Sentencia();
                OtraSentencia();
            }
            else
                lexico.DevuelveToken(token); // Vacio
        }

        private void Sentencia()
        {
            char token;
            if (lexico.ExisteTraza())
                Console.WriteLine("ANALISIS SINTACTICO: <SENTENCIA>");

            token = lexico.SiguienteToken();
            if ((token >= 'a') && (token <= 'z'))
            {
                lexico.DevuelveToken(token);
                Asignacion();
            }
            else if (token == 'R')
                Lectura();
            else if (token == 'W')
                Escritura();
            else
                Errores(6);
        }

        private void Asignacion()
        {
            char token;
            if (lexico.ExisteTraza())
                Console.WriteLine("ANALISIS SINTACTICO: <ASIGNACION>");

            Variable();
            token = lexico.SiguienteToken();
            if (token != '=')
                Errores(3);

            Expresion();
        }

        private void Variable()
        {
            char token;
            if (lexico.ExisteTraza())
                Console.WriteLine("ANALISIS SINTACTICO: <VARIABLE>");

            token = lexico.SiguienteToken();
            if ((token >= 'a') && (token <= 'z'))
                Console.Write(token);
            else
                Errores(5);
        }

        private void Expresion()
        {
            if (lexico.ExisteTraza())
                Console.WriteLine("ANALISIS SINTACTICO: <EXPRESION>");

            Termino();
            MasTerminos();
        }

        private void Termino()
        {
            if (lexico.ExisteTraza())
                Console.WriteLine("ANALISIS SINTACTICO: <TERMINO>");

            Factor();
            MasFactores();
        }

        private void MasTerminos()
        {
            char token;
            if (lexico.ExisteTraza())
                Console.WriteLine("ANALISIS SINTACTICO: <MAS_TERMINOS>");

            token = lexico.SiguienteToken();
            if (token == '+')
            {
                Termino();
                MasTerminos();
            }
            else if (token == '-')
            {
                Termino();
                MasTerminos();
            }
            else
                lexico.DevuelveToken(token); // <vacio>
        }

        private void Factor()
        {
            char token;
            if (lexico.ExisteTraza())
                Console.WriteLine("ANALISIS SINTACTICO: <FACTOR>");

            token = lexico.SiguienteToken();
            if ((token >= '0') && (token <= '9'))
            {
                lexico.DevuelveToken(token);
                Constante();
            }
            else if (token == '(')
            {
                Expresion();
                token = lexico.SiguienteToken();
                if (token != ')')
                    Errores(4);
            }
            else
            {
                lexico.DevuelveToken(token);
                Variable();
            }
        }

        private void MasFactores()
        {
            char token;
            if (lexico.ExisteTraza())
                Console.WriteLine("ANALISIS SINTACTICO: <MAS_FACTORES>");

            token = lexico.SiguienteToken();
            switch (token)
            {
                case '*':
                    Factor();
                    MasFactores();
                    break;
                case '/':
                    Factor();
                    MasFactores();
                    break;
                case '%':
                    Factor();
                    MasFactores();
                    break;
                default:
                    lexico.DevuelveToken(token);
                    break;
            }
        }

        private void Lectura()
        {
            char token;
            token = lexico.SiguienteToken();
            if (lexico.ExisteTraza())
                Console.WriteLine("ANALISIS SINTACTICO: <LECTURA> " + token);

            if ((token < 'a') || (token > 'z'))
                Errores(5);
        }

        private void Escritura()
        {
            char token;
            token = lexico.SiguienteToken();
            if (lexico.ExisteTraza())
                Console.WriteLine("ANALISIS SINTACTICO: <ESCRITURA> " + token);

            if ((token < 'a') || (token > 'z'))
                Errores(5);
        }

        private void Constante()
        {
            char token;
            if (lexico.ExisteTraza())
                Console.WriteLine("ANALISIS SINTACTICO: <CONSTANTE>");

            token = lexico.SiguienteToken();
            if ((token >= '0') && (token <= '9'))
                Console.Write(token);
            else
                Errores(7);
        }

        private void Errores(int codigo)
        {
            int x;
            Console.WriteLine("LINEA " + lexico.LineaActual());
            Console.WriteLine(" ERROR SINTACTICO " + codigo);
            switch (codigo)
            {
                case 1:
                    Console.WriteLine(" :ESPERABA UN ;");
                    break;
                case 2:
                    Console.WriteLine(" :ESPERABA UNA }");
                    break;
                case 3:
                    Console.WriteLine(" :ESPERABA UN =");
                    break;
                case 4:
                    Console.WriteLine(" :ESPERABA UN )");
                    break;
                case 5:
                    Console.WriteLine(" :ESPERABA UN IDENTIFICADOR");
                    break;
                case 6:
                    Console.WriteLine(" :INSTRUCCION DESCONOCIDA");
                    break;
                case 7:
                    Console.WriteLine(" :ESPERABA UNA CONSTANTE");
                    break;
                case 8:
                    Console.WriteLine(" :ESPERABA UNA M DE MAIN");
                    break;
                case 9:
                    Console.WriteLine(" :ESPERABA UNA {");
                    break;
                default:
                    Console.WriteLine(" :NO DOCUMENTADO");
                    break;
            }
            x = Convert.ToInt32(Console.ReadLine());
            Environment.Exit(-(codigo + 100));
        }
    }

    public class Lexico
    {
        private string nombreFichero;
        private StreamReader entrada;
        private int n1;
        private int traza;
        private char[] buffer;
        private int pBuffer;

        public Lexico(string unNombreFichero, int unaTraza = 0)
        {
            entrada = new StreamReader(unNombreFichero);
            if (entrada == null)
            {
                Console.WriteLine("No se puede abrir el archivo");
                Console.ReadKey();
                Environment.Exit(-2);
            }

            traza = unaTraza;
            n1 = 1;
            buffer = new char[100];
            pBuffer = 0;
        }

        ~Lexico()
        {
            entrada.Close();
        }

        public char SiguienteToken()
        {
            char car = '\0';
            while ((car = (pBuffer > 0) ? buffer[--pBuffer] : (char)entrada.Read()) != -1)
            {
                if (car == ' ') continue;
                if (car == '\n') { ++n1; continue; }
                break;
            }
            if (traza == 1) Console.WriteLine("ANALIZADOR LEXICO: Lee el token " + car);
            switch (car)
            {
                case 'M':
                case 'R':
                case 'W':
                case '=':
                case '(':
                case ')':
                case ';':
                case '}':
                case '{':
                case '.':
                case '+':
                case '*':
                case '-':
                case '/':
                case '%':
                    return car;
            }
            if (char.IsLower(car)) return car;
            else if (char.IsDigit(car)) return car;
            else
            {
                Console.WriteLine("Error Lexico: Token Desconocido");
                Console.ReadKey();
                Environment.Exit(-4);
            }
            return car;
        }

        public void DevuelveToken(char token)
        {
            if (pBuffer >= buffer.Length)
            {
                Console.WriteLine("ERROR: Desbordamiento de buffer del analizador léxico");
                Console.ReadKey();
                Environment.Exit(-5);
            }
            else
            {
                buffer[pBuffer++] = token;
                if (ExisteTraza())
                    Console.WriteLine("ANALIZADOR LEXICO: Recibe en buffer el token " + token);
            }
        }

        public int LineaActual()
        {
            return n1;
        }

        public bool ExisteTraza()
        {
            return traza == 1;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Lexico lexico = new Lexico("fuente.txt", 1);
            Sintactico sintactico = new Sintactico("fuente.txt", 1);
        }
    }
}