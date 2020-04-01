using System;
using System.Collections.Generic;
using System.IO;



namespace ProcesadorDeLenguaje_JS_PL
{
    public class AnalizadorSintactico
    {
        /*Este es un analizador sintáctico LR(1)
         Contiene 
            Una Pila
            Dos Tablas: 
                    Tabla Accion
                    Tabla GOTO
        */

        private readonly List<string[]> tablaAccion;
        private readonly Dictionary<string, int> columnaAccion; // Devuelve la posicion j donde esta el token en la tablaAccion ej tablaAccion[numero estado,id] transforma el id al indice de turno.
        private readonly List<string[]> tablaGoto;
        private readonly Dictionary<string, int> columnaGoto;
        private readonly List<(string,int)> reglas = new List<(string Antecedente,int numDeConsecuentes)>();
        private readonly AnalisisLexico alex;
        private string ficheroTokens = "";
        private GestorDeErrores erroresSintactico;
        private GestorTS gestorTs;

        public AnalizadorSintactico(AnalisisLexico alex, GestorTS gsTs,string pathTablaAccion,string pathTablaGoto,string pathNumeroConsecuentes)
        {
            this.alex = alex; // recibimos el analizador lexico. 
            tablaAccion = new List<string[]>();
            columnaAccion = new Dictionary<string, int>();
            tablaGoto = new List<string[]>();
            columnaGoto= new Dictionary<string, int>();
            tablaAccion = CrearTabla(pathTablaAccion,columnaAccion);
            tablaGoto = CrearTabla(pathTablaGoto,columnaGoto);
            CalcularNumeroDeConscuentesPorRegla(pathNumeroConsecuentes);
            erroresSintactico = new GestorDeErrores();
            gestorTs = gsTs;
        }

      
        
        /// <summary>
        /// Metodo principal del analizador sintactico. Devuelve el parse a partir del analizador lexico y los fich de configuración(tabla Accion y tabla GOTO).
        /// Utiliza la Tabla de Simbolos y el Gestor de errores.
        /// </summary>
        /// <para>Cadena de entrada con delimiador $ por la derecha</para>
        /// <para>Tabla accion</para>
        /// <para>tabla goto</para>
        /// <returns>El Parse, un analisis ascendente de la cadena de entrada. En caso de error una explicación del mismo</returns>
        public string GetParse()
        {
            AnalisisLexico.Token tokenDeEntrada=alex.GetToken();
            if (tokenDeEntrada == null) return "";
            ficheroTokens = tokenDeEntrada.Imprimir() + "\n";
            string parse = "Ascendente ";
            List<string> pilaSt = new List<string> {"0"};
            Stack<Atributo> pilaSem = new Stack<Atributo>();
            AnalizadorSemantico Asem = new AnalizadorSemantico(gestorTs);
            while (true) // mientras no se acabe el fichero. Que peligro tiene ese while true.
            {
                // El ultimo elemento de la pila deberá ser un numero. Si eso no es así  peta.
                string casilla = Accion(int.Parse(pilaSt[pilaSt.Count - 1]), tokenDeEntrada.Codigo); 

                if (casilla.Substring(0, 1) == "s")// la s se debe tomar como una "d" de desplazar.
                {
                    /*
                    // ############################### INICIO SEMANTICO  P1 ########################################################
                     if(tokenDeEntrada.Codigo.Equals("ID"))
                     {
                         // Obtenemos el lexema y el tipo
                         // Lo metemos en la pila semantica como atributo
                         // Meter el atributo en la pila semantica
                         pilaSem.Push(new Atributo(tokenDeEntrada.Codigo,tokenDeEntrada.Cadena));
                     }
                     else
                     {
                         pilaSem.Push(new Atributo(tokenDeEntrada.Codigo,tokenDeEntrada.Cadena));
                         // meter el atributo en la pila (Atributo: token.entrada,tipo.)
                     }
                     // ############################### FIN SEMANTICO  P1 ###########################################################
                     */
                    /* DESPLAZAR
                     * 1 Meter tokenDeEntrada en la pila
                     * 2 Meter el estado al que se desplaza  en la pila
                     * 3 Leer sig token.
                     */
                    pilaSt.Add(tokenDeEntrada.Codigo);
                    pilaSt.Add(casilla.Substring(1));
                    tokenDeEntrada = alex.GetToken();
                    if (tokenDeEntrada != null)// si es null signifa que hemos acabado todos los tokens
                    { ficheroTokens += tokenDeEntrada.Imprimir() + "\n"; }
                    else { tokenDeEntrada = new AnalisisLexico.Token("$"); }// Este es siempre el ultimo token.
                }
                else if (casilla.Substring(0, 1) == "r")
                { 
                        
                    /*REDUCCION A->B   A es el antecedente y B el consecuente
                     * 1 Sacar (2*Nº de consecuentes de la regla) de la pila
                     * 2 s' = pila.cima()
                     * 2 meter A en la pila
                     * 3 Obtener estado=Goto[s',A] y meter enla pila el estado
                     * Generar el parse correspondiente a la regla
                     */

                     
                    int sacar = 2 * reglas[int.Parse(casilla.Substring(1))].Item2;
                    pilaSt.RemoveRange(pilaSt.Count - sacar, sacar); //revisar si saca lo esperado.
                    //s' = pila.cima()
                    int estadoPila = Convert.ToInt32(pilaSt[pilaSt.Count - 1]);
                    // Meter A en la pila:
                    int numRegla = int.Parse(casilla.Substring(1));
                    string antecedente = reglas[numRegla].Item1; // reglas[Nº Regla].antecedente
                    pilaSt.Add(antecedente); // se añade a la pila el antecedente. 
                    //tablaGoto[s',A]  un (estado,antecedente)
                    string estado=Goto(estadoPila, antecedente);
                    
                    /*
                    // ############################### INICIO SEMANTICO  P2 ########################################################
                    Asem.ejecAccSemantica(Convert.ToInt32(casilla.Substring(1)) + 1, pilaSem);
                    // ############################### FIN SEMANTICO  P2 ###########################################################
                    */
                    //Console.WriteLine("tipo"+pilaSem[0]);
                    pilaSt.Add(estado);
                    Console.WriteLine("regla:"+antecedente+" Tam pila: "+pilaSt.Count+" Pila: "+pilaSt[0] );
                    // meter en el parse la regla por la q se reduce. casilla.Substring(1)+1
                    parse += numRegla+1+" "; // solucion to cutre sumamos un 1 a la regla y ya esta listo para vast
                    //El semantico debe Ejecutar segun la regla q sea.
                        
                }
                else if (casilla == "acc")
                {
                    return parse + 1;// añadimos la regla axioma
                }
                else
                {
                    //error
                    Console.WriteLine("Error: casilla= " + casilla);
                    erroresSintactico.ErrSintactico(1,"error: sintactico encontrado. en Linea " + alex.NumLineaCodigo+
                                                      " . Tiene que estar cerca del simbolo  : "+ tokenDeEntrada.Codigo);
                    break;
                }
            }
            return ""; // se ejecuta solo en caso de error.
        }
        
        // Metodo que crea la tabla goto y la tabla ACCION 
        // Recive un csv y una estructura y un hashmap 
        private static List<string[]> CrearTabla(string path,Dictionary<string,int>columna)
        {// creamos la estructura tabla. Ya sea la tabla goto o la tabla acción.
            using (var reader = new StreamReader(path))
            {
                List<string[]> tabla = new List<string[]>();
                var firstline = reader.ReadLine()?.Split(','); // csv separado por hashtags.
                if (firstline != null)
                    for (int i = 0; i < firstline.Length; i++)
                    {
                        columna.Add(firstline[i], i);
                    }
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine()?.Split(','); // csv separado por hashtags.
                    if (line == null)
                    {
                        // Lanzar error fichero no valido.
                    }
                    tabla.Add(line);
                    //Console.WriteLine(line);
                }
                return tabla;
            }
        }

        private void CalcularNumeroDeConscuentesPorRegla(string path) {
            using (var reader = new StreamReader(path)) {
                reglas.Add(("",0));
                while (!reader.EndOfStream) {
                    var line = reader.ReadLine()?.Split(new[]{"->"},StringSplitOptions.None);
                    if (line == null) continue;  // TODO si esto puede ocurrir? + Lanzar error.
                    string[] numeroDeConsecuentes = line[1].Split(' ');
                    reglas.Add( (line[0],numeroDeConsecuentes[0].Equals("")?0:numeroDeConsecuentes.Length) );//metemos en la tupla de posicion Nº de regla: antecedente,Nº de Consecuentes ej A->B R   mete (A,2)
                    // Recuerda que si hubiese una regla A->    es decir un lambda pues tendría length =1 pero debería tener 0 de ahí el ternario de arriba.
                }
            }
        }

        /// <summary>
        /// Recibe el estado de la cima de la pila y un token. Te dice si es un desplazamiento reducción aceptación o error.
        /// </summary>
        private string Accion(int estadoPila,string token)
        {
            return tablaAccion[estadoPila][columnaAccion[token]]; // si estadoPila =0 coincide con la fila 0 de la tabla. Ya que la fila extra con los nombres nos la hemos quitado y guardado en columnaAccionSumamos 
        }

        private string Goto(int estadoPila, string antecedente)
        {
            return tablaGoto[estadoPila][columnaGoto[antecedente]];
        }
        
        
        public string GetFichTokens()
        {
            return ficheroTokens;
        }
    }
}