using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProcesadorDeLenguaje_JS_PL;
using TablaSimbolos;

namespace ProcesadorDeLenguaje_JS_PL
{
    /* Analizdador Lexico: 
     * Recibe:Un codigo JavasScrpt-PL
     * Devuelve:Un token por llamada 
     * Resumen:Abre el fichero. Lo mete en un String
     */

    public class AnalisisLexico
    {
        //Int16 a = -446;
        /*Tokens
	Gramatica:
			S-> &A|=B|(|)||,|;|{|}|lC|dD|"E|+F|/G|del S
		            A-> &
		            B-> =|LAMBDA
		            C-> lC|dC|_C|LAMBDA
		            D-> dD|LAMBDA
		            E-> c1E|"
		            F-> +|LAMBDA
		            G-> /H
		            H-> c2 H|saltolinea S
            
            Leyenda:
                c1=caracteres-{"} Solo caracteres imprimibles.
                c2=caracteres-{salto de linea. Retorno de carro}
         */


        /*       Estructura
         *  LeeLetra()
         *  estado=estado(actual,valor entrada)
         *  accion=estado(actual,valor entrada)
         * 
         */

        private string texto; //Nuestro archivo en texto. formato lista.
        private int pos; //posicion del cursor de la lista texto.        
        private string linea;
        private Boolean eof;
        private string[] textoPorLineas;
        TablaDeSimbolos tablaSimbolos = new TablaDeSimbolos();
        Hashtable palabrasR; //Palabras reservadas
        private Regex numerosEnteros;
        private Regex letrasyNumeros;
        private int numLineaCodigo;
        private GestorDeErrores ALexErrores;



        public AnalisisLexico(string ruta)
        {
            /*Constructor.*/ //tabla de simbolos?
            abreArchivo(ruta);
            numerosEnteros = new Regex(@"[0-9]+$");
            letrasyNumeros = new Regex(@"^[A-Za-z_0-9]");
            palabrasR = new Hashtable();
            eof = false;
            pos = 0;
            numLineaCodigo = 0;
            ALexErrores = new GestorDeErrores();
        }

        /*Mete el archivo en un string llamado texto.*/
        private void abreArchivo(string ruta)
        {
            try
            {
                StreamReader file = new StreamReader(ruta); //abrimos el archivo.
                string linea;
                while ((linea = file.ReadLine()) != null)
                {
                    texto = texto + linea + "\n";
                }

                texto.Remove(texto.Length - 1);
                file.Close();
                texto.ToCharArray();
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        // Devuelve un caracter de texto y avanza el puntero.
        public char LeeCaracter(int pos)
        {
            // Recorremos un string texto caracter a caracter
            if (pos < texto.Length)
            {
                // pos++;
                if (texto[pos] == '\n')
                {
                    numLineaCodigo++;
                    return texto[pos];
                }

                return texto[pos];
            }
            else
            {
                eof = true;
                return '\0';
            }
        }
        // Acciones Semánticas:
        public void L()
        {
            pos++;
        }
        public string C(char c, string cadena)
        {
            return cadena + c;
        }


        /*lexico()
         * Realiza la funcion de automata del analisis lexico.
         * Devuelve un token.Por llamada. Null si no hay mas tokens.
         */
        public Token GetToken()
        {
            Boolean fin = false;
            Token token = null;
            int contador = 0;
            short valor = 0;
            String cadena = ""; // sirve  para los tokens de tipo cadena ej 'hola' y tokens tipo identificador ej id3
            int estado = 0;
            if (eof == true)
            {
                return null;
            }

            while (!fin)
            {
                char leido;
                leido = LeeCaracter(pos);
                if (eof == true)
                {
                    tablaSimbolos.ImprimirTS();
                    return null;
                }

                switch (estado)
                {
                    case 0: //Estado 0
                        switch (leido)
                        {
                            case '&':
                                estado = 1;
                                pos++;
                                break;
                            case '=':
                                estado = 2;
                                pos++;
                                break;
                            case '(':
                                token = new Token("AbreParent");
                                pos++;
                                fin = true;
                                break;
                            case ')':
                                token = new Token("CierraParent");
                                pos++;
                                fin = true;
                                break;
                            case ',':
                                token = new Token("Coma");
                                pos++;
                                fin = true;
                                break;
                            case ';':
                                token = new Token("PuntoComa");
                                pos++;
                                fin = true;
                                break;
                            case '{':
                                token = new Token("AbreCorchetes");
                                pos++;
                                fin = true;
                                break;
                            case '}':
                                token = new Token("CierraCorchetes");
                                pos++;
                                fin = true;
                                break;

                            case '\"': // " 
                                estado = 5;
                                pos++;
                                break;
                            case '+':
                                estado = 6;
                                pos++;
                                break;
                            case '/':
                                estado = 7;
                                pos++;
                                break;
                            case ' ':
                                pos++;
                                estado = 0;
                                break;
                            case '\n':
                                pos++;
                                break;
                            case '\t':
                                pos++;
                                break;
                            default:
                                //letras
                                if ((leido >= 'a' && leido <= 'z') || (leido >= 'A' && leido <= 'Z'))
                                {
                                    pos++;
                                    estado = 3;
                                    cadena = cadena + leido;
                                    break;
                                }

                                //numeros
                                if (numerosEnteros.IsMatch(leido + ""))
                                {
                                    valor = (short) (leido - '0'); // leido lo convertimos a int.
                                    estado = 4;
                                    pos++;
                                    break;
                                }
                                // Algo inesperado -> Error
                                else
                                {
                                    ALexErrores.Error(" El símbolo " + leido + "no está permitido");
                                    fin = true;
                                }

                                break;
                        }

                        break;

                    case 1: //Estado 1
                        if (leido == '&')
                        {
                            token = new Token("&&");
                            pos++;
                        }
                        else
                        {
                            //error
                            ALexErrores.Error("ERROR: Símbolo no permitido./n NO existe el componente léxico & " +
                                              "pruebe con &&");
                        }

                        fin = true;
                        break;

                    case 2: //Estado 2
                        if (leido == '=')
                        {
                            token = new Token("IGUALIGUAL");
                            pos++;
                        }
                        else
                        {
                            token = new Token("IGUAL");
                        }

                        fin = true;
                        break;
                    case 3: // Estado 3
                        //Regex letras = new Regex(@"^[A-Za-z_0-9]"); a-z A-Z  0-9 y _ 
                        if (letrasyNumeros.IsMatch(leido + ""))
                        {
                            estado = 3;
                            cadena = cadena + leido;
                            pos++;
                        }
                        else
                        {
                            // si Other Character
                            Int16? p;
                            p = tablaSimbolos.buscarPR(cadena);
                            if (p != null)
                            {
                                //Es una palabra reservada
                                token = new Token(cadena);
                                fin = true;
                                break;
                            }

                            p = tablaSimbolos.buscarTS(cadena);
                            if (p != null)
                            {

                                token = new Token("ID", (short) p);
                                fin = true;
                                break;
                            }
                            else
                            {
                                p = tablaSimbolos.insertarTS(cadena);
                                token = new Token("ID", (short) p);
                                fin = true;
                                break;
                            }
                        }
                        break;
                    case 4: // Estado 4
                        if (numerosEnteros.IsMatch(leido + ""))
                        {
                            pos++;
                            // TODO Numeros negativos no funcionan
                            short aux=valor;
                            valor = (short) (valor * 10 + (leido - '0'));
                            if (valor < aux)
                            {
                                ALexErrores.Error("El rango de numeros es (-32768,32767)");
                            }

                            estado = 4;
                        }
                        else
                        {
                            token = new Token("digito", valor);
                            fin = true;
                        }

                        break;

                    case 6: // Estado 6 +
                        if (leido == '+')
                        {
                            //si es nulo --> fin de cadena
                            token = new Token("MASMAS");
                            fin = true;
                            pos++;
                        }
                        else
                        {
                            token = new Token("Suma");
                            fin = true;
                        }

                        break;
                    case 5: //Estado 5 Cadenas
                        if (leido == '\"')
                        {
                            //si " --> fin de cadena
                            token = new Token("cadena", cadena); //generar laa cadena
                            fin = true;
                            pos++;
                        }
                        else // TODO Nadie comprueba si es un salto de linea. 
                        {
                            cadena = cadena + leido;
                            pos++;
                        }

                        break;
                    case 7: //Estado 7 Comentarios
                        if (leido == '/')
                        {
                            estado = 8;
                            pos++;
                        }
                        else
                            ALexErrores.Error("El componente léxico / no existe pruebe con //");

                        break;

                    case 8:
                        if (leido == '\n')
                            estado = 0;
                        else
                        {
                            pos++;
                            estado = 8;
                        }

                        break;
                }
            }

            //llamado o dir escribir el token pos 1
            if (token != null)
            {
                Console.WriteLine(" desde Alex <" + token.Codigo + "," + ">");
                //Console.WriteLine(token.ToString());
                //Console.ReadKey();
               

            }

            if (token == null)
            {
                tablaSimbolos.ImprimirTS();
                token= new Token("$");
            }
            

            return token;
        }


        public class Token
        {
            // Token: <codigo,atributo> Un atributo puede ser un valor o una cadena.
            private string codigo;
            private string cadena;
            private Int16? valor;

            public Token(string codigo)
            {
                this.codigo = codigo;
                cadena = null;
                valor = null;
            }

            public Token(string codigo, Int16 valor)
            {
                this.codigo = codigo;
                this.valor = valor;
                this.cadena = null;
            }

            public Token(string codigo, string cadena)
            {
                this.codigo = codigo;
                this.cadena = cadena;
                valor = null;
            }

            // Geters y seters
            public string Codigo
            {
                get => codigo;
                set => codigo = value;
            }

            public string Cadena
            {
                get => cadena;
                set => cadena = value;
            }

            public short? Valor
            {
                get => valor;
                set => valor = value;
            }


            //
            public string Imprimir()
            {
                // token con formato.
               if (valor==null&&cadena==null)
                {
                    return "<" + codigo + "," + "" + ">";
                }
                else if (string.IsNullOrEmpty(cadena))
                {
                    return "<" + codigo + "," + valor + ">";
                }
                else
                {
                    return "<" + codigo + "," + "\"" + cadena + "\"" + ">"; // <codigo,"cadena">
                }
            }

        }
    }
}
