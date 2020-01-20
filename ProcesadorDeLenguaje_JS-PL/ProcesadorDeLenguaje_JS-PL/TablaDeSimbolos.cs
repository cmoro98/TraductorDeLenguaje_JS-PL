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
        *
        * El hashmap contendra ObjetoTS CORRECCION tenemos 2 tablas. un global y la local que se van creando y destruyendo. pero solo 2 al mismo tiempo.
        * Cuando se busque un valor. Se nos dara el lexema e iremos buscando en el hashmap y recorriendo 
        * el arraylist.
        */
        //Coleccion elementos clave,valor.
        //la clave no se puede repetir. 

       //Uso:
       /* Hay al menos dos casos claramente diferenciados:
        * 1-> No se permiten funciones dentro de funciones-> Existe una TSG y una TSL al mismo tiempo. Las TSL iran creandose y destruyendose pero solo existira una unica TSL al mismo tiempo.
        *
        * 2-> Se permiten funciones dentro de funciones-> Existe una TSG y N TSL
        *     Recomendacion: Crea un objeto gestor_TS que contiene una pila de Tablas de simbolos y otra pila de desplazamientos.
        *     si usas una pila ya tienes hecha la busqueda de variables(el ord de acc a la pila) mirando primero di la variables
        *     es local y en caso de no encontrarla ya si buscara en la siguiente TS
        * 
        */
       
       
        // Esto es un objeto tabla de simbolos. Si tu lenguaje no contiene funciones dentro de funciones (lo que provocaria
        //la necesidad de tener varias tablas de simbolos locales) Tendras normalmente dos tablas una TSG y una TSL que serán instancias de este objeto. 
        // En caso de necesitar numerosas TSL sería interesante crear un objeto gestor de TS que contuviese una pila de TS y una pila de Desplazamientos.
        
        
        /*ADVERTENCIA: Esta tabla de simbolos ha sido dieñada para un lenguaje que No admite funciones
        dentro de funciones o lo que es lo mismo, Existe una Tabla de simbolos global y una tabla de
        simbolos local como máximo Es decir solo existe una TSLocal al mismo tiempo. EN caso */

        
        /* METODOS Que debería haber
         * Crear_TS();  Constructor.
         * Insertar_TS(TS,id.lexema,T.tipo,Despl)
         * Busca_TS(TS,id.lexema)
         * 
         */
        //private List<Hashtable> pilaTS;
        private Hashtable tablaSimbolosGlobal;
       

        //private bool encontrado;
        //private int numeroDeTabla;
        private short posTablaDeSimbolos;
        private int desplazamiento;


        public TablaDeSimbolos()
        {
            this.tablaSimbolosGlobal = new Hashtable();
            tablaSimbolosGlobal.Add("if", new ObjetoTS("if", true, 0));
            tablaSimbolosGlobal.Add("function", new ObjetoTS("function", true, 1));
            tablaSimbolosGlobal.Add("int", new ObjetoTS("int", true, 2));
            tablaSimbolosGlobal.Add("boolean", new ObjetoTS("boolean", true, 3));
            tablaSimbolosGlobal.Add("string", new ObjetoTS("string", true, 4));
            tablaSimbolosGlobal.Add("true", new ObjetoTS("true", true, 5));
            tablaSimbolosGlobal.Add("false", new ObjetoTS("false", true, 6));
            tablaSimbolosGlobal.Add("input", new ObjetoTS("input", true, 7));
            tablaSimbolosGlobal.Add("print", new ObjetoTS("print", true, 8));
            tablaSimbolosGlobal.Add("var", new ObjetoTS("var", true, 9));
            tablaSimbolosGlobal.Add("do", new ObjetoTS("do", true, 10));
            tablaSimbolosGlobal.Add("while", new ObjetoTS("while", true, 11));
            tablaSimbolosGlobal.Add("return", new ObjetoTS("return", true, 12));
            posTablaDeSimbolos = 13;
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
            tablaSimbolosGlobal.Add(lexema, new ObjetoTS(lexema,false,posTablaDeSimbolos));
            return posTablaDeSimbolos;
        }

        // Imprime la tabla de simbolos de mayor prioridad.
        public string ImprimirTS()
        {
            string ret="Contenido de la tabla # "+0+":\n";
            foreach (DictionaryEntry elemento in tablaSimbolosGlobal)
            {
                Console.WriteLine("({0},{1})", elemento.Key,elemento.Value);
                ObjetoTS rt = (ObjetoTS)elemento.Value;
                ret = ret +  rt.ImprimirObjetoTS();
            }

            using (System.IO.StreamWriter fichTS = new System.IO.StreamWriter(@"TS.txt", true))
            {
                fichTS.WriteLine(ret); 
            }

            return ret;
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
            

            public string ImprimirObjetoTS()
            {
                string ret = "* LEXEMA: "+"\'"+lexema+"\' \n"+ "ATRIBUTOS: \n"
                             + "+ tipo: "+ "\'"+tipo+"\' \n"
                             + "+ despl: "+ "\'"+dir+"\' \n"
                    ;
                return ret;
            }
            
        }
    }
}
