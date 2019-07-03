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

namespace AnalizadorLexico {
    /* Analizdador Lexico: 
     * Recibe:Un codigo JavasScrpt-PL
     * Devuelve:Un token por llamada 
     * Resumen:Abre el fichero. Lo mete en un String
     */

    class AnalisisLexico {
        //Int16 a = -446;
        /*Tokens:
             * <MAS,->
             <&&,->
             <IGUAL,->
             <IGUALIGUAL,->
             <CierraParent,->
             <AbreParent,->
             <cadena,lex>
             <ENT,valor>
             <ID,posTS>           Variables int numerito=0;
             <Coma,->
             //<ComillasAbre,->
             //<ComillasCierra,->
             <PuntoComa>
             <AsignaConResto,->
             …………….Palabras Reservadas Van en la TS-----------
             <function,->
             <int,->
             <bool,->
             <string,->
             <true,->
             <false,->
             <prompt,->
             <print,->
             <var,->
             <for,->
         *Gramatica:
         *  S-> +|& A|= B|(|)|l C|d D|' E|% G|/ H|del S
            A-> &
            B-> =|LAMBDA
            C-> l C|d C|_ C|LAMBDA 
            D-> d D|LAMBDA
            E-> c1 E|null F
            F-> '
            G-> =
            H-> / I
            I-> c2 I|saltolinea S
            
            buena
            S-> +|& A|= B|(|)|l C|d D|' E|% F|/ G|del S
            A-> &
            B-> =|LAMBDA
            C-> l C|d C|_ C|LAMBDA 
            D-> d D|LAMBDA
            E-> c1 E|'
            F-> =
            G-> / H
            H-> c2 I|saltolinea S
            
            Leyenda:
                c1=caracteres-{'}
                c2=caracteres-{salto de linea. Retorno de carro}
         */


        /*       Estructura
         *  LeeLetra()
         *  estado=estado(actual,valor entrada)
         *  accion=estado(actual,valor entrada)
         * 
         */
        
        private string texto;//Nuestro archivo en texto. formato lista.
        private int pos;//posicion del cursor de la lista texto.        
        private string linea;
        private Boolean eof;
        private string[] textoPorLineas; 
        TablaDeSimbolos tablaSimbolos = new TablaDeSimbolos();
        Hashtable palabrasR; //Palabras reservadas
        private Regex numerosEnteros;
        private Regex letrasyNumeros;
        private int numLineaCodigo;
        private GestorDeErrores ALexErrores;
        

        
        public AnalisisLexico(string ruta ) {  /*Constructor.*/ //tabla de simbolos?
            abreArchivo(ruta);
            numerosEnteros = new Regex(@"[0-9]+$");
            letrasyNumeros = new Regex(@"^[A-Za-z_0-9]");
            palabrasR = new Hashtable();
            eof = false;
            pos = 0;
            numLineaCodigo = 0;
            ALexErrores= new GestorDeErrores();
        }

        /*Mete el archivo en un string llamado texto.*/
        private void abreArchivo(string ruta) {         
            try {
                StreamReader file = new StreamReader(ruta);//abrimos el archivo.
                string linea;
                while ((linea = file.ReadLine()) != null) {
                    texto = texto + linea + "\n";
                }
                texto.Remove(texto.Length - 1);
                file.Close();
                texto.ToCharArray();
            }
            catch (System.Exception e) {
                System.Console.WriteLine(e.Message);
            }      
        }

        //Crea la tabla de palabras reservadas. Mal van en la Tabla de Simbolos.seguro?
      
        private Hashtable rellenaPR() {
            palabrasR.Add("true", "true");
            palabrasR.Add("false", "false");
            palabrasR.Add("prompt", "prompt");
            palabrasR.Add("print", "print");
            palabrasR.Add("var", "var");
            palabrasR.Add("int", "int");
            palabrasR.Add("string", "string");
            palabrasR.Add("bool", "bool");
            palabrasR.Add("for", "for");
            palabrasR.Add("function", "function");
            return palabrasR;
        }
        
        



        //devuelve un caracter de texto y avanza el puntero.
        public char LeeCaracter(int pos) {
            //Recorremos un string texto caracter a caracter
            if (pos < texto.Length) {
                // pos++;
                if (texto[pos] == '\n') {
                    numLineaCodigo++;
                    return texto[pos];
                }
                return texto[pos];
            }
            else {
                eof = true;
                return '\0';
            }
        }


        /*lexico()
         * Realiza la funcion de automata del analisis lexico.
         * Devuelve un token.Por llamada. Null si no hay mas tokens.
         */
        public Token GetToken() {
            Boolean fin = false;
            Token token = null; 
            int contador = 0;
            short valor = 0;
            String cadena = ""; // sirve  para los tokens de tipo cadena ej 'hola' y tokens tipo identificador ej id3
            int estado = 0;
            if (eof == true) { return null; }
            while (!fin) {
                char leido;
                leido = LeeCaracter(pos);
                if (eof == true) { return null; }

                switch (estado) {
                    case 0://Estado 0
                        switch (leido) {
                            case '+':                               
                                token = new Token("MAS");
                                pos++;
                                fin = true;
                                break;
                            case '&':
                                estado = 1;
                                pos++;
                                break;
                            case '=':
                                estado = 2;
                                pos++;
                                break;
                            case '(':
                                token = new Token("ABPAR");
                                pos++;
                                fin = true;
                                break;
                            case ')':
                                token = new Token("CRPAR");
                                pos++;
                                fin = true;
                                break;
                            case ',':
                                token = new Token("COMA");
                                pos++;
                                fin = true;
                                break;
                            case ';':
                                 token = new Token("PUNTCOMA");
                                pos++;
                                fin = true;
                                break;
                            case '\'': // ' 
                                estado = 5;
                                pos++;
                                break;
                            case '%':
                                estado = 7;
                                pos++;
                                break;
                            case '/':
                                estado = 8;
                                pos++;
                                break;
                            case '$'://Esto no se si existe
                                token = new Token("FINFICH");
                                pos++;
                                fin = true;
                                break;
                   
                            case ' ':
                                pos++;
                                estado = 0;
                                break;
                            
                            case '\n':
                                pos++;
                                break;
                            default:
                                //letras
                                if ((leido >= 'a' && leido <= 'z') || (leido >= 'A' && leido <= 'Z')) {
                                    pos++;
                                    estado = 3;
                                    cadena = cadena + leido;
                                    break;
                                }
                                //numeros
                                if (numerosEnteros.IsMatch(leido + "")) {
                                    valor = (short) (leido-'0'); // leido lo convertimos a int.
                                    estado = 4;
                                }
                                // Algo inesperado -> Error
                                else {
                                    ALexErrores.Error(" El símbolo "+leido+ "no está permitido");
                                    fin = true;
                                }
                                break;
                        }
                        break;

                    case 1://Estado 1
                        if (leido == '&') {
                            token = new Token("&&");
                            //pos++;
                        }
                        else {
                          //error
                          ALexErrores.Error("ERROR: Símbolo no permitido./n NO existe el componente léxico & " +
                                            "pruebe con &&");
                        }
                        fin=true;
                        break;

                    case 2://Estado 2
                        if (leido == '=') {
                            token = new Token("IGUALIGUAL");
                            pos++;
                        }
                        else {
                            token = new Token("IGUAL");
                        }
                        fin = true;                       
                        break;
                    case 3: // Estado 3
                        //Regex letras = new Regex(@"^[A-Za-z_0-9]"); a-z A-Z  0-9 y _ 
                        if (letrasyNumeros.IsMatch(leido + "")) {
                            estado = 3;
                            cadena = cadena + leido;
                            pos++;
                        }
                        else {  // si Other Character
                            if (tablaSimbolos.isPalabraReservada(cadena)) {//Es una palabra reservada
                                token = new Token(cadena);
                            }
                            else {
                                // REVISAR 
                                Console.WriteLine("lin de turno:" + linea);
                                //la buscamos en la TS
                                //si esta la imprimimos
                                //si no esta la añadimos.
                                if (tablaSimbolos.busca(cadena) == true) {
                                    token = new Token(cadena);
                                }
                                else {
                                    tablaSimbolos.insertaTS(cadena);
                                    token = new Token(cadena);
                                }
                                //se coje el token de la tabla de palabras reservadas
                            }
                            fin = true;
                        }
                        break;
                    case 4: // Estado 4
                        if (numerosEnteros.IsMatch(leido + "")) {
                            pos++;
                            valor = (short) (valor * 10 + (leido - '0'));
                            estado = 4;
                        }
                        else {
                            token = new Token("ENT");
                            token.setValor(valor);
                            fin = true;
                        }
                        break;

                    case 6://Estado 5 Cadenas
                        if (leido == (char)0) {//si es nulo --> fin de cadena
                            estado = 6;
                            pos++;
                        }
                        else {
                            cadena = cadena + leido;
                        }
                        break;
                    case 5://Estado 5 Cadenas
                        if (leido == '\'') {//si ' --> fin de cadena
                            token = new Token("cadena");//generar laa cadena
                            fin = true;
                            pos++;
                        }
                        else
                        {
                            cadena = cadena + leido;
                            pos++;
                        }

                        break;

                    case 7://Estado 7
                        if (leido == '=') 
                            token = new Token("AsigRest");
                        else 
                            ALexErrores.Error("El componente lexico % no existe. pruebe con %=");
                        
                        pos++;
                        fin = true;
                        break;

                    case 8://Estado 9 Comentarios
                        if (leido == '/') {
                            estado = 9;
                            pos++;
                        }
                        else
                            ALexErrores.Error("El componente léxico / no existe pruebe con //");
                        break;

                    case 9:
                        if (leido == '\n') 
                            estado = 0;
                        else
                        {
                            pos++;
                            estado = 9;
                        }
                        break;
                }
            }
            //llamado o dir escribir el token pos 1
            if (token != null) {
                Console.WriteLine("<" + token.getToken() + "," + ">");
                Console.WriteLine(token.toString());
                Console.ReadKey();

            }
            return token;
        }


        static int pan(string[] args) {
            //era Main no pan
            if (args.Length == 0) {
                System.Console.WriteLine("Ingrese un argumento");
                Console.ReadKey();
                return -1;
            }
            string fichero = args[0];
            AnalisisLexico prueba = new AnalisisLexico(fichero);
           // prueba.abreArchivo(fichero);
            prueba.rellenaPR();
            Token tokenDevuelto = null;
            do {
                tokenDevuelto = prueba.GetToken();
                using (System.IO.StreamWriter fichTokens = new System.IO.StreamWriter(@"tokens.txt", true)) {
                    if (tokenDevuelto != null) {
                        if (tokenDevuelto.getToken().Equals("ENT")) { fichTokens.WriteLine("<" + tokenDevuelto.getToken() + "," + tokenDevuelto.getValor() + "" + ">"); }
                        else { fichTokens.WriteLine("<" + tokenDevuelto.getToken() + "," + "" + ">"); }
                    }

                }

            }
            while (tokenDevuelto != null);
            prueba.tablaSimbolos.imprimirTS();
            Console.WriteLine(fichero);
            Console.ReadKey();
            return 0;
        }

        public int[,] a;
        //Acciones Semánticas

    }
    
    
    
    
    class Token {
        private String token;
        private int valor;
        private String cadena;

        public Token(String token) {
            this.token = token;
        }
        public void setValor(int valor) {
            this.valor = valor;
        }
        public void setCadena(String cadena) {
            this.cadena = cadena;
        }
        public int getValor() {
            return valor;
        }
        public String getCadena() {
            return cadena;
        }
        public String getToken() {
            return token;
        }
        public String toString() {
            
            //String res = "<"+ token+","+"-"+">";
           /* if ((valor + cadena + "") == "") {
                return "<" + token + "," + "-" + ">";
            }*/
            //token con formato.
            return ((valor + cadena + "") == "")?"<" + token + "," + "-" + ">":"<" + token + "," + valor + cadena + ">";

        }
    }
}
