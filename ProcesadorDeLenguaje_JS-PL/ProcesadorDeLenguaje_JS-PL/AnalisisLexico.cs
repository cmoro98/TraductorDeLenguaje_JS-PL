using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
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
        private Boolean eof;
        private char posAnterior;
       
        private Regex numerosEnteros;
        private Regex letrasyNumeros;
        private int numLineaCodigo;
        private GestorDeErrores gestorDeErrores;
        // Tabla simbolos
        private GestorTS gestorTs = new GestorTS();
        private short numParaID = 0;
        char leido = ' ';
        int antPos = -1;
        
        TablaDeSimbolos tablaSimbolosG = new TablaDeSimbolos(true);



        public AnalisisLexico(string ruta, GestorTS gsTs, GestorDeErrores deErrores)
        {
            /*Constructor.*/ //tabla de simbolos?
            abreArchivo(ruta);
            numerosEnteros = new Regex(@"[0-9]+$");
            letrasyNumeros = new Regex(@"^[A-Za-z_0-9]");
            eof = false;
            pos = 0;
            numLineaCodigo = 1;
            //gestorDeErrores = new GestorDeErrores();
            gestorTs = gsTs;
            gestorTs.crearTS(true);  // Creamos la tabla de simbolos global.
            this.gestorDeErrores = deErrores;
            posAnterior = ' ';

        }

        public int NumLineaCodigo => numLineaCodigo;

        /*Mete el archivo en un string llamado texto.*/
        private void abreArchivo(string ruta)
        {
            if( new FileInfo( ruta ).Length == 0 )
            {
                Console.WriteLine("Fichero vacio.");
                Environment.Exit(-1);
            }
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
                texto.ToCharArray();  // TODO revisar
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // Devuelve un caracter de texto y avanza el puntero.
        
        public char LeeCaracter(int pos)
        {
            // Recorremos un string texto caracter a caracter
            if (pos < texto.Length)
            {
                /*char a = texto[pos];
                char b = texto[pos + 1];
                char bb = texto[pos + 2];
                char bbb = texto[pos + 3];
                char bbbb = texto[pos + 4];
                char bbbbb = texto[pos + 5];
                Console.WriteLine(a +" "+ b +" "+numLineaCodigo );*/
                // pos++;
                if (texto[pos] == '\n')
                {
                    numLineaCodigo++;
                    /*if (posAnterior != texto[pos] || posAnterior =='\n' )
                    {
                        numLineaCodigo++;
                    }*/

                    posAnterior = texto[pos];
                    
                    return texto[pos];
                }
                posAnterior = texto[pos];
                return texto[pos];
            }
            eof = true;
            return '\0';
            
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
            //int contador = 0;
            short valor = 0;
            String cadena = ""; // sirve  para los tokens de tipo cadena ej 'hola' y tokens tipo identificador ej id3
            int estado = 0;
            if (eof == true)
            {
                return null;
            }
            

            while (!fin)
            {
                
                if (pos != antPos)
                {
                    leido = LeeCaracter(pos);
                }

                antPos = pos;
                if (eof == true)
                {
                    //tablaSimbolosG.ImprimirTS();
                    //gestorTs.imprimirTS();
                    
                   // token = new Token("$");
                    return token;
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
                                token = new Token("AbreParent",numLineaCodigo);
                                pos++;
                                fin = true;
                                break;
                            case ')':
                                token = new Token("CierraParent",numLineaCodigo);
                                pos++;
                                fin = true;
                                break;
                            case ',':
                                token = new Token("COMA",numLineaCodigo);
                                pos++;
                                fin = true;
                                break;
                            case ';':
                                token = new Token("PuntoComa",numLineaCodigo);
                                pos++;
                                fin = true;
                                break;
                            case '{':
                                token = new Token("AbreCorchetes",numLineaCodigo);
                                pos++;
                                fin = true;
                                break;
                            case '}':
                                token = new Token("CierraCorchetes",numLineaCodigo);
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
                                    gestorDeErrores.ErrLexico("Error Lexico: Linea("+ numLineaCodigo + ") " + " El símbolo " + leido + " no está permitido");
                                    fin = true;
                                }

                                break;
                        }

                        break;

                    case 1: //Estado 1
                        if (leido == '&')
                        {
                            token = new Token("AND",numLineaCodigo);
                            pos++;
                        }
                        else
                        {
                            //error
                            
                            gestorDeErrores.ErrLexico("Error Lexico: Linea("+ numLineaCodigo + ") " +" Símbolo no permitido./n NO existe el componente léxico & " +
                                              "pruebe con &&");
                        }
                        fin = true;
                        break;

                    case 2: //Estado 2
                        if (leido == '=')
                        {
                            token = new Token("IGUALIGUAL",numLineaCodigo);
                            pos++;
                        }
                        else
                        {
                            token = new Token("IGUAL",numLineaCodigo);
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
                            //Int16? p;
                            Int32? p;
                            p = gestorTs.buscarPR(cadena);
                            if (cadena.Length > 64) {
                                //Error
                                gestorDeErrores.ErrLexico("Error Lexico: Linea("+ numLineaCodigo + ") " + " Una constante tiene como max 64 caracteres");
                            }
                            if (p != null) // si Palabra reservada
                            {
 
                                //Es una palabra reservada
                                token = new Token(cadena,numLineaCodigo);
                                fin = true;
                                break;
                            }
                        
                            // Si identificador normal.
                            /*
                            p = gestorTs.buscarTS(cadena);
                            if (p != null) // NO hay que meterla en la tabla de simbolos. Simplemente declaramos el token y fuera.
                            {
                                token = new Token("ID", (short) p);
                                token.NombreIdentificador = cadena;
                                fin = true;
                                break;
                            }
                            //Else
                            p = gestorTs.insertarTS(cadena);
                            //p = tablaSimbolosG.insertarTS(cadena);
                            token = new Token("ID", (short) p);
                            token.NombreIdentificador = cadena;
                            fin = true;
                            */

                            numParaID++;
                            token = new Token("ID", cadena,numLineaCodigo); // token = new Token("ID", cadena); CORRECTO:  // Para Draco poner numParaID
                            token.NombreIdentificador = cadena;
                            fin = true;
                            
                            
                            
                            
                        }
                        break;
                    case 4: // Estado 4
                        if (numerosEnteros.IsMatch(leido + ""))
                        {
                            pos++;
                            // TODO Numeros negativos no funcionan  Actualmente se notifica el error pero no se para la ejecucucion
                            short aux=valor;
                            valor = (short) (valor * 10 + (leido - '0'));
                            if (valor < aux)
                            {
                                gestorDeErrores.ErrLexico("Error Lexico: Linea("+ numLineaCodigo + ") " +"El rango de numeros es (-32768,32767)");
                                //return null; // probar si funciona con esta linea
                            }

                            estado = 4;
                        }
                        else
                        {
                            token = new Token("digito", valor,numLineaCodigo);
                            fin = true;
                        }

                        break;

                    case 6: // Estado 6 +
                        if (leido == '+')
                        {
                            //si es nulo --> fin de cadena
                            token = new Token("MASMAS",numLineaCodigo);
                            fin = true;
                            pos++;
                        }
                        else
                        {
                            token = new Token("Suma",numLineaCodigo);
                            fin = true;
                        }

                        break;
                    case 5: //Estado 5 Cadenas
                        if (cadena.Length > 64) {
                            //Error
                            gestorDeErrores.ErrLexico("Error Lexico: Linea("+ numLineaCodigo + ") " + "Una constante tiene como max 64 caracteres");
                        }
                        if (leido == '\"')
                        {

                            //si " --> fin de cadena
                            token = new Token("cadena", cadena,numLineaCodigo); //generar laa cadena
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
                            gestorDeErrores.ErrLexico("Error Lexico: Linea("+ numLineaCodigo + ") " + "El componente léxico / no existe pruebe con //");

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
               // Console.WriteLine(" desde A.lex <" + token.Codigo + "," + ">");
                //Console.WriteLine(token.ToString());
                //Console.ReadKey();
               

            }

            if (token == null)
            {
                //tablaSimbolos.ImprimirTS();
                //token = new Token("$");
            }
            

            return token;
        }


        public class Token
        {
            // Token: <codigo,atributo> Un atributo puede ser un valor o una cadena.   <- en este contexto(En el semantico con atributo nos referimos a otra cosa.)
            private string codigo;
            private string cadena;
            private Int16? valor;
            private string nombreIdentificador; // Usuado para guardar el nombre del identificador y poder localizarlo posteriormente en la Tabla de simbolos.
            private int numLinea;
            
            public string NombreIdentificador
            {
                get => nombreIdentificador;
                set => nombreIdentificador = value;
            }

            public Token(string codigo)
            {
                this.codigo = codigo;
                cadena = null;
                valor = null;
            }
            
            public Token(string codigo,int numLinea)
            {
                this.codigo = codigo;
                cadena = null;
                valor = null;
                this.numLinea = numLinea;
            }
            
            public Token(string codigo, Int16 valor,int numLinea)
            {
                this.codigo = codigo;
                this.valor = valor;
                this.cadena = null;
                this.numLinea = numLinea;
            }

            public Token(string codigo, string cadena,int numLinea)
            {
                this.codigo = codigo;
                this.cadena = cadena;
                this.numLinea = numLinea;
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

            public int NumLinea => numLinea;


            public string derecha()
            {
                if (cadena==null)
                {
                    return valor+"";
                }

                return cadena;
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

        } // Fin Token
    }     // Fin An Lexico
}
