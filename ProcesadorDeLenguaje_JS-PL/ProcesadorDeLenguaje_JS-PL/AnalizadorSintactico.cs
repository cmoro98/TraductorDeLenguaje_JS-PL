using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication.ExtendedProtection;


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

        private List<string[]> tablaAccion;
        private Dictionary<string, int> columnaAccion; // Devuelve la posicion j donde esta el token en la tablaAccion ej tablaAccion[numero estado,id] transforma el id al indice de turno.
        private List<string[]> tablaGoto;
        private Dictionary<string, int> columnaGoto;
        private List<(string,int)> reglas = new List<(string Antecedente,int numDeConsecuentes)>();
        private AnalisisLexico alex;
        private string ficheroTokens = "";
        private GestorDeErrores erroresSintactico;

        public AnalizadorSintactico(AnalisisLexico alex,string pathTablaAccion,string pathTablaGoto,string pathNumeroConsecuentes)
        {
            this.alex = alex; // recibimos el analizador lexico. 
            tablaAccion = new List<string[]>();
            columnaAccion = new Dictionary<string, int>();
            tablaGoto = new List<string[]>();
            columnaGoto= new Dictionary<string, int>();
            tablaAccion = CrearTabla(pathTablaAccion,columnaAccion);
            tablaGoto = CrearTabla(pathTablaGoto,columnaGoto);
            CalcularNumeroDeConscuentesPorRegla(pathNumeroConsecuentes,reglas);
            erroresSintactico = new GestorDeErrores();
        }

      
        
        /// <summary>
        /// 
        /// </summary>
        /// <para>Cadena de entrada con delimiador $ por la derecha</para>
        /// <para>Tabla accion</para>
        /// <para>tabla goto</para>
        /// <returns>El Parse, un analisis ascendente de la cadena de entrada. En caso de error una explicación del mismo</returns>
        public string GetParse()
        {
            AnalisisLexico.Token tokenDeEntrada=alex.GetToken();
            if (tokenDeEntrada != null)
            {
                ficheroTokens = tokenDeEntrada.Imprimir() + "\n";
                string parse = "Ascendente ";
                List<string> pila = new List<string>();
                pila.Add("0");
                while (true) // mientras no se acabe el fichero. Que peligro tiene ese while true.
                {
                    // El ultimo elemento de la pila deberá ser un numero. Si eso no es así  peta.
                    string casilla = Accion(Int32.Parse(pila[pila.Count - 1]), tokenDeEntrada.Codigo); //   Codigo no?

                    if (casilla.Substring(0, 1) == "s")
                    {
                       
                        /* DESPLAZAR
                     * 1 Meter tokenDeEntrada en la pila
                     * 2 Meter el estado al que se desplaza  en la pila
                     * 3 Leer sig token.
                     */
                        pila.Add(tokenDeEntrada.Codigo);
                        pila.Add(casilla.Substring(1));
                        tokenDeEntrada = alex.GetToken();
                        ficheroTokens += tokenDeEntrada.Imprimir() + "\n";
                        
                    }
                    else if (casilla.Substring(0, 1) == "r")
                    { 
                        
                     /*REDUCCION A->B                A es el antecedente y B el consecuente
                     * 1 Sacar (2*Nº de consecuentes de la regla) de la pila
                     * 2 s' = pila.cima()
                     * 2 meter A en la pila
                     * 3 Obtener Goto[s',A]
                     * Generar el parse correspondiente a la regla
                     */

                     
                        int sacar = 2 * reglas[Int32.Parse(casilla.Substring(1))].Item2;
                        pila.RemoveRange(pila.Count - sacar, sacar); //revisar si saca lo esperado.
                        //s' = pila.cima()
                        int estadoPila = Convert.ToInt32(pila[pila.Count - 1]);
                        // Meter A en la pila:
                        int numRegla = Int32.Parse(casilla.Substring(1));
                        string antecedente = reglas[numRegla].Item1; // reglas[Nº Regla].antecedente
                        pila.Add(antecedente); // se añade a la pila. 
                        //tablaGoto[s',A]
                        string a=Goto(estadoPila, antecedente);
                        pila.Add(a);
                        parse += (Convert.ToInt32(casilla.Substring(1))+1)+" "; // solucion to cutre sumamos un 1 a la regla y ya esta listo para vast
                        
                    }
                    else if (casilla == "acc")
                    {
                        return parse+=1;// añadimos la regla axioma
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
            }
            return "";
        }
        

        public List<string[]> CrearTabla(string path,Dictionary<string,int>Columna)
        {// creamos la estructura tabla. Ya sea la tabla goto o la tabla acción.
            using (var reader = new StreamReader(path))
            {
                List<string[]> tabla = new List<string[]>();
                var firstline = reader.ReadLine()?.Split(','); // csv separado por hashtags.
                if (firstline != null)
                    for (int i = 0; i < firstline.Length; i++)
                    {
                        Columna.Add(firstline[i], i);
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

        public void CalcularNumeroDeConscuentesPorRegla(string path, List<(string,int)> reglas) {
            using (var reader = new StreamReader(path)) {
                int i = 1;
                reglas.Add(("",0));
                while (!reader.EndOfStream) {
                    var line = reader.ReadLine()?.Split(new[]{"->"},StringSplitOptions.None);
                    if (line != null)
                    {
                        string[] numeroDeConsecuentes = line[1].Split(' ');
                        reglas.Add( (line[0],numeroDeConsecuentes[0].Equals("")?0:numeroDeConsecuentes.Length) );//metemos en la tupla de posicion Nº de regla: antecedente,Nº de Consecuentes ej A->B R   mete (A,2)
                        // Recuerda que si hubiese una regla A->    es decir un lambda pues tendría length =1 pero debería tener 0 de ahí el ternario de arriba.
                    }
                    i++;         
                }
            }
        }

        /// <summary>
        /// Recibe el estado de la cima de la pila y un token. Te dice si es un desplazamiento reducción aceptación o error.
        /// </summary>
        public string Accion(int estadoPila,string token)
        {
            return tablaAccion[estadoPila][columnaAccion[token]]; // si estadoPila =0 coincide con la fila 0 de la tabla. Ya que la fila extra con los nombres nos la hemos quitado y guardado en columnaAccionSumamos 
        }
        
        public string Goto(int estadoPila, string antecedente)
        {
            return tablaGoto[estadoPila][columnaGoto[antecedente]];
        }
        
        public string GetFichTokens()
        {
            return ficheroTokens;
        }
    }
}