using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablaSimbolos
{
    public class TablaDeSimbolos
    {
        /*Resumen:   
        * Tenemos una pila de Tablas de simbolos. La pila sera un List de hash map.
        * El hashmap contendra ObjetoTS
        * Cuando se busque un valor. Se nos dara el lexema e iremos buscando en el hashmap y recorriendo 
        * el arraylist.
        */
        //Coleccion elementos clave,valor.
        //la clave no se puede repetir. 
        
        private List<Hashtable> pilaTS;
        private Hashtable TSG;
        private bool encontrado;
        private int numeroDeTabla;


        public TablaDeSimbolos()
        {
            this.numeroDeTabla = 1;
            this.pilaTS = new List<Hashtable>();
            this.TSG = new Hashtable();
            string lexema;
            this.encontrado = false;
            pilaTS.Add(TSG);
        }
        //

        //Metodo insertar  
        public void insertaTS(string lexema)
        {
            // Atributos atri = new Atributos(tipo);
            pilaTS.First().Add(lexema,new ObjetoTS(lexema));
            //tiene que devolver una posicion
        }

        //recorre la TS y te dice si lo encontro.
        public bool busca(string lexema)
        {
            return encontrado;
        }
        //Por hacer
        public void insertaTSfunc(string lexema, string tipo)
        {

        }
        //Imprime en el txt
        public void imprimirTS()
        {
            using (System.IO.StreamWriter tablaSimbolos = new System.IO.StreamWriter(@"TablaSimbolos.txt", true))
            {

                foreach (Hashtable busc in pilaTS)
                {
                    tablaSimbolos.WriteLine("Contenido de la tabla# " + numeroDeTabla);
                    //recorrer hasMap en orden.
                    int i = 0;
                    foreach (DictionaryEntry entrada in busc)
                    {
                        //Imprimir con formato en el txt
                        tablaSimbolos.WriteLine("* LEXEMA : '" + busc.Keys + "'");
                        tablaSimbolos.WriteLine(busc.Values);
                        tablaSimbolos.WriteLine("---------------");

                    }

                }
            }
        }
    }

    public class ObjetoTS
    {
        private string lexema;
        private string tipo;
        private string dir;
        private int nParametros;
        private string tipoDevuelto;
        private string etiqueta;
        private string[] tiposParametros;

        public ObjetoTS(string lexema) {
            this.lexema = lexema;
        }

        public string Lexema { get => lexema; set => lexema = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public string Dir { get => dir; set => dir = value; }
        public int NParametros { get => nParametros; set => nParametros = value; }
        public string TipoDevuelto { get => tipoDevuelto; set => tipoDevuelto = value; }
        public string Etiqueta { get => etiqueta; set => etiqueta = value; }
        public string[] TiposParametros { get => tiposParametros; set => tiposParametros = value; }
    }
}
