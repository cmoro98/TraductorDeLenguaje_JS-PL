using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProcesadorDeLenguaje_JS_PL
{
    public class AnalizadorSintactico
    {
        /*Este es un analizador sintáctico LR(1)
         Contiene 
            Una Pila sintactico
            Una pila semantico
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
        private bool detectarFuncionFlag;
        //private bool detectaAhoraLaFuncion;
        private string codigoEnsamblador;

        public AnalizadorSintactico(AnalisisLexico alex, GestorTS gsTs, string pathTablaAccion, string pathTablaGoto,
            string pathNumeroConsecuentes, GestorDeErrores gestorDeErrores)
        {
            this.alex = alex; // recibimos el analizador lexico. 
            tablaAccion = new List<string[]>();
            columnaAccion = new Dictionary<string, int>();
            tablaGoto = new List<string[]>();
            columnaGoto= new Dictionary<string, int>();
            tablaAccion = CrearTabla(pathTablaAccion,columnaAccion);
            tablaGoto = CrearTabla(pathTablaGoto,columnaGoto);
            CalcularNumeroDeConscuentesPorRegla(pathNumeroConsecuentes);
            erroresSintactico = gestorDeErrores;
            gestorTs = gsTs;
            detectarFuncionFlag = false;
            //detectaAhoraLaFuncion = false; 
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
            AnalizadorSemantico aSemantico = new AnalizadorSemantico(gestorTs,alex,erroresSintactico);
            List<Atributo> detectarFuncionUltimosTresElementos; // sirve para detectar una funcion y así declararla cuando hay que hacerlo.. El patron : function H ID Abreparent A CierraParent
            while (true) // mientras no se acabe el fichero. Que peligro tiene ese while true.
            {
                // El ultimo elemento de la pila deberá ser un numero. Si eso no es así  peta.
                string casilla = Accion(int.Parse(pilaSt[pilaSt.Count - 1]), tokenDeEntrada.Codigo); 

                if (casilla.Substring(0, 1) == "s")// Desplazar :  la s se debe tomar como una "d" de desplazar.
                {
                    // ############################### INICIO SEMANTICO  P1 ########################################################
                   // Console.WriteLine("token"+ tokenDeEntrada.Imprimir());
                   if (detectarFuncionFlag && pilaSem.Count >= 3)
                   {
                       detectarFuncionUltimosTresElementos = pilaSem.ToList().GetRange(0, 3);
                       if (DetectarFunction(detectarFuncionUltimosTresElementos))
                       {
                           //Console.WriteLine("Declarando funcion.");
                           // Ejecutamo la accion semantica de turno.
                           aSemantico.declararFuncion(detectarFuncionUltimosTresElementos);
                           detectarFuncionFlag = false;
                       }
                   }
                   if (tokenDeEntrada.Codigo.Equals("ID")) 
                   {
                        pilaSem.Push(new Atributo(tokenDeEntrada.Codigo,tokenDeEntrada.NombreIdentificador,tokenDeEntrada.NumLinea));
                   }
                   
                   else if (tokenDeEntrada.Codigo.Equals("digito")) 
                   {
                       pilaSem.Push(new Atributo(tokenDeEntrada.Codigo,tokenDeEntrada.NumLinea,tokenDeEntrada.Valor));
                   }
                   else if (tokenDeEntrada.Codigo.Equals("cadena")) 
                   {
                       pilaSem.Push(new Atributo(tokenDeEntrada.Codigo,tokenDeEntrada.NumLinea,tokenDeEntrada.Cadena));
                   }
                    else if (tokenDeEntrada.Codigo.Equals("function")) 
                    {
                        /*En principio con la regla semantica F -> function H ID (A) cuando llegamos hasta un punto creamos la tabla de simbolos y eso.
                         pero ,nosotros solo llamamos a las acc semanticas cuando se reduce una regla no cuando esta a medias. Realmente estamos haciendo un DDS.
                         La implementación así es mucho más faci.
                         Esto queda sucio porque aquí tengo que semi ejecutar una acción semántica para ver si puedo crear la nueva tabla de símbolos y ttodo eso. */
                        
                        pilaSem.Push(new Atributo(tokenDeEntrada.Codigo,tokenDeEntrada.NumLinea));
                        //Console.WriteLine("FUNCTION:");
                        detectarFuncionFlag = true;
                    }
                    else
                    {
                        pilaSem.Push(new Atributo(tokenDeEntrada.Codigo,tokenDeEntrada.NumLinea));
                    }
                    
                    // ############################### FIN SEMANTICO  P1 ###########################################################
                    
                    /* DESPLAZAR
                      "Si no se puede completar una parte derecha, se acumula a lo que se esta construyendo. 
                       Esta accion es el desplazamiento."
                     * 1 Meter tokenDeEntrada en la pila
                     * 2 Meter el estado al que se desplaza  en la pila
                     * 3 Leer sig token.
                     */
                    pilaSt.Add(tokenDeEntrada.Codigo);
                    pilaSt.Add(casilla.Substring(1));
                    tokenDeEntrada = alex.GetToken();
                    
                    /* Lo de abajo es un apaño para distinguir la llegada de tokens y el ultimo token.
                     Que debería ser $ pero como lo que nos llega es un null pues lo detectamos y simulamos que es $.*/
                    if (tokenDeEntrada != null)// si es null signifa que hemos acabado todos los tokens
                    { ficheroTokens += tokenDeEntrada.Imprimir() + "\n"; }
                    else { tokenDeEntrada = new AnalisisLexico.Token("$"); }// Este es siempre el ultimo token.
                }
                else if (casilla.Substring(0, 1) == "r")
                { 
                        
                    /*REDUCCION
                     "Si el token es una parte derecha derecha de una regla o completa una parte derecha en contruccion, entonces 
                      se puede deshacer la regla. Esta accion es la reduccion."
                     * A->B   A es el antecedente y B el consecuente
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
                    string estado=Goto(estadoPila, antecedente);  //tablaGoto[s',A]  un (estado,antecedente)
                    
                    
                    // ############################### INICIO SEMANTICO  P2 ########################################################
                    //Asem.ejecAccSemantica(Convert.ToInt32(casilla.Substring(1)) + 1, pilaSem);
                    pilaSem.Push(new Atributo(antecedente,alex.NumLineaCodigo));
                    aSemantico.ejecAccSemantica(numRegla+1, pilaSem);

                    // ############################### FIN SEMANTICO  P2 ###########################################################
                  
                    //Console.WriteLine("tipo"+pilaSem[0]);
                    pilaSt.Add(estado);
                    
                    // Console.WriteLine("Num regla: "+ (numRegla+1) +" regla:"+antecedente+" Tam pila: "+pilaSt.Count +" Pila: "+pilaSt[0] );
                    
                    // meter en el parse la regla por la q se reduce. casilla.Substring(1)+1
                    parse += numRegla+1+" "; // solucion to cutre sumamos un 1 a la regla y ya esta listo para vast
                    //El semantico debe Ejecutar segun la regla q sea.
                        
                }
                else if (casilla == "acc") // ACEPTACION "La cadena pertenece al lenguaje generado. Cadena sintacticamente correcta"
                {
                    // ############################### INICIO SEMANTICO  P3 ########################################################
                    codigoEnsamblador = aSemantico.ejecAccSemantica(1, pilaSem);
                    // ############################### FIN SEMANTICO  P3 ###########################################################
                    return parse + 1;// añadimos la regla axioma
                }
                else
                {
                    //error
                    //Console.WriteLine("Error: casilla= " + casilla);
                    string valor = "";
                    valor = tokenDeEntrada.Valor?.ToString();
                    valor = tokenDeEntrada.Cadena?.ToString();
                    erroresSintactico.ErrSintactico(1,
                                                      "Tiene que estar cerca del simbolo: "+ tokenDeEntrada.Codigo + ": "+ valor );
                    //Console.WriteLine("Num de Regla:");
                    
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

        public bool DetectarFunction(List<Atributo> ultmosTresElementos)
        {
            List<string> patronFuncion = new List<string> {"ID","H","function"}; //{"CierraParent","A","AbreParent","ID","H","function"};
            int i = 0;
            foreach (var atributo in ultmosTresElementos)
            {
                if (atributo.Simbolo != patronFuncion[i])
                {
                    return false;
                }
                i++;
            }
            return true;
        }

        public string CodigoEnsamblador
        {
            get => codigoEnsamblador;
            set => codigoEnsamblador = value;
        }
    }
}