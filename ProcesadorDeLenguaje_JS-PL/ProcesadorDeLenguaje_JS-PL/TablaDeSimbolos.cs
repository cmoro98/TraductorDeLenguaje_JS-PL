using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablaSimbolos
{

    public class GestorTablaSimbolos
    {
        //crea tabla
        //maneja las tablas 
    }

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

        //private List<Hashtable> pilaTS;
        private Hashtable tablaSimbolosGlobal;

        //private bool encontrado;
        //private int numeroDeTabla;
        private short posTablaDeSimbolos;


        public TablaDeSimbolos()
        {
            this.tablaSimbolosGlobal = new Hashtable();
            object ObjetoTS;
            tablaSimbolosGlobal.Add("if", new ObjetoTS("if", true, 0));
            tablaSimbolosGlobal.Add("function", new ObjetoTS("function", true, 1));
            tablaSimbolosGlobal.Add("int", new ObjetoTS("int", true, 2));
            tablaSimbolosGlobal.Add("boolean", new ObjetoTS("boolean", true, 3));
            tablaSimbolosGlobal.Add("string", new ObjetoTS("string", true, 4));
            tablaSimbolosGlobal.Add("true", new ObjetoTS("true", true, 5));
            tablaSimbolosGlobal.Add("false", new ObjetoTS("false", true, 6));
            tablaSimbolosGlobal.Add("print", new ObjetoTS("print", true, 7));
            tablaSimbolosGlobal.Add("var", new ObjetoTS("var", true, 8));
            tablaSimbolosGlobal.Add("do", new ObjetoTS("do", true, 9));
            tablaSimbolosGlobal.Add("while", new ObjetoTS("while", true, 10));
            tablaSimbolosGlobal.Add("return", new ObjetoTS("return", true, 11));
            posTablaDeSimbolos = 11;
        }

        public short? buscarPR(string lexema)
        {
            ObjetoTS ret = (ObjetoTS) tablaSimbolosGlobal[lexema];
            if (ret == null) { return null;}
            
            if (ret.EsPalabraReservada)
            {
                return (short?) ret.PosicionTablaDeSimbolos;
            }

            return null;
        }

        public short? buscarTS(string lexema)
        {
            ObjetoTS ret = (ObjetoTS) tablaSimbolosGlobal[lexema];
            if (ret == null) { return null;}
            return (short?) ret.PosicionTablaDeSimbolos;
        }

        public short insertarTS(string lexema)
        {
            posTablaDeSimbolos++;
            tablaSimbolosGlobal.Add(lexema, new ObjetoTS(lexema, esPalabraReservada: false, posTablaDeSimbolos));
            return posTablaDeSimbolos;
        }

/*
        public TablaDeSimbolos()
        {
            //this.numeroDeTabla = 1;
          //  this.pilaTS = new List<Hashtable>();
            this.TSG = new Hashtable<>();
            string lexema;
           // this.encontrado = false;
           // pilaTS.Add(TSG);
        }
        
        
        //
        public bool isPalabraReservada(string pr)
        {
            return false;
        }

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
*/
        public class ObjetoTS
        {
            private string lexema;
            private string tipo;
            private string dir;
            private int nParametros;
            private string tipoDevuelto;
            private string etiqueta;
            private string[] tiposParametros;
            private bool esPalabraReservada;
            private int posicionTablaDeSimbolos; // posición por orden de llegada.



            public ObjetoTS(string lexema)
            {
                this.lexema = lexema;
            }

            public ObjetoTS(string lexema, bool esPalabraReservada, int posicionTablaDeSimbolos)
            {
                this.lexema = lexema;
                this.esPalabraReservada = esPalabraReservada;
                this.posicionTablaDeSimbolos = posicionTablaDeSimbolos;
            }

            public string Lexema
            {
                get => lexema;
                set => lexema = value;
            }

            public string Tipo
            {
                get => tipo;
                set => tipo = value;
            }

            public string Dir
            {
                get => dir;
                set => dir = value;
            }

            public int NParametros
            {
                get => nParametros;
                set => nParametros = value;
            }

            public string TipoDevuelto
            {
                get => tipoDevuelto;
                set => tipoDevuelto = value;
            }

            public string Etiqueta
            {
                get => etiqueta;
                set => etiqueta = value;
            }

            public string[] TiposParametros
            {
                get => tiposParametros;
                set => tiposParametros = value;
            }

            public bool EsPalabraReservada
            {
                get => esPalabraReservada;
                set => esPalabraReservada = value;
            }

            public int PosicionTablaDeSimbolos
            {
                get => posicionTablaDeSimbolos;
                set => posicionTablaDeSimbolos = value;
            }
        }
    }
}
