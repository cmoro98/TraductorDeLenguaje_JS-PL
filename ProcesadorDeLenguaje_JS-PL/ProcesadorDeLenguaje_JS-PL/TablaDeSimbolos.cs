using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablaSimbolos
{
    public class TablaDeSimbolos : ITablaDeSimbolos
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
        private Hashtable tablaSimbolos;
       

        //private bool encontrado;
        //private int numeroDeTabla;
        private int dirDeMemoria;
        private int desplazamiento;
        private int posEnLaTablaDeSimbolos;
        private int numeroTS;

        // Constructor: Recibe un booleano para indicar si la tabla de simbolos es global -> true o es local -> false
        public TablaDeSimbolos(bool global)
        {
            this.tablaSimbolos = new Hashtable();
            if (global)
            {
                /*tablaSimbolos.Add("if", new ObjetoTS("if", true, 0));
                tablaSimbolos.Add("function", new ObjetoTS("function", true, 1));
                tablaSimbolos.Add("int", new ObjetoTS("int", true, 2));
                tablaSimbolos.Add("boolean", new ObjetoTS("boolean", true, 3));
                tablaSimbolos.Add("string", new ObjetoTS("string", true, 4));
                tablaSimbolos.Add("true", new ObjetoTS("true", true, 5));
                tablaSimbolos.Add("false", new ObjetoTS("false", true, 6));
                tablaSimbolos.Add("input", new ObjetoTS("input", true, 7));
                tablaSimbolos.Add("print", new ObjetoTS("print", true, 8));
                tablaSimbolos.Add("var", new ObjetoTS("var", true, 9));
                tablaSimbolos.Add("do", new ObjetoTS("do", true, 10));
                tablaSimbolos.Add("while", new ObjetoTS("while", true, 11));
                tablaSimbolos.Add("return", new ObjetoTS("return", true, 12));*/
                
            }
            dirDeMemoria = 0;
            posEnLaTablaDeSimbolos = 0;
            

        }

        public int? buscarTS(string lexema)
        {
            ObjetoTS ret = (ObjetoTS) tablaSimbolos[lexema];
            if (ret == null) { return null;}

            return ret.PosEnLaTablaDeSimbolos;
        }
        
        public ObjetoTS buscarObjTS(string lexema)
        {
            ObjetoTS ret = (ObjetoTS) tablaSimbolos[lexema];
            return  ret;
        }
        
        

        public int insertarTS(string lexema)
        {
            posEnLaTablaDeSimbolos++;
            tablaSimbolos.Add(lexema, new ObjetoTS(lexema));
            return posEnLaTablaDeSimbolos;
        }
        
        public int insertarTS(string lexema,int desplazamiento,string tipo)
        {
            //dirDeMemoria += desplazamiento;
            posEnLaTablaDeSimbolos++;
            tablaSimbolos.Add(lexema, new ObjetoTS(lexema,desplazamiento,tipo));
            return posEnLaTablaDeSimbolos;
        }

        public void insertarDespl(string lexema, int despl)
        {
          // tablaSimbolos.Add(lexema,);
          ObjetoTS aux = (ObjetoTS) tablaSimbolos[lexema];
          aux.DirDeMemoria = despl;
          tablaSimbolos[lexema] = aux;
        }

        // Imprime la tabla de simbolos de mayor prioridad.
        public string ImprimirTS()
        {
            string ret="Contenido de la tabla # "+numeroTS+":\n";
            foreach (DictionaryEntry elemento in tablaSimbolos)
            {
               
                ObjetoTS rt = (ObjetoTS)elemento.Value;
               // Console.WriteLine("({0},{1})", elemento.Key,rt.Lexema);
                ret = ret +  rt.ImprimirObjetoTS();
            }

            using (System.IO.StreamWriter fichTS = new System.IO.StreamWriter(@"../../Resultados/TS.txt", true))
            {
                fichTS.WriteLine(ret); 
            }

            return ret;
        }

        public int NumeroTs
        {
            get => numeroTS;
            set => numeroTS = value;
        }
        

        public class ObjetoTS
        {
            private string lexema;
            private string tipo;
            private string despl;
            private int nParametros;
            private string tipoDevuelto;
            private string etiqueta;
            private string[] tiposParametros;
            private int dirDeMemoria; // posición por orden de llegada.
            private int posEnLaTablaDeSimbolos; // solo numeros, indica el orden de entrada. Sirve para ir a la derecha del identificador en el fich token. Req de la practica nada mas.

            public int PosEnLaTablaDeSimbolos
            {
                get => posEnLaTablaDeSimbolos;
                set => posEnLaTablaDeSimbolos = value;
            }


            public ObjetoTS(string lexema)
            {
                this.lexema = lexema;
            }

            public ObjetoTS(string lexema, int dirDeMemoria)
            {
                this.lexema = lexema;
                this.dirDeMemoria = dirDeMemoria;
            }
            public ObjetoTS(string lexema, int dirDeMemoria , string tipo)
            {
                this.lexema = lexema;
                this.dirDeMemoria = dirDeMemoria;
                this.Tipo = tipo;
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
                get => despl;
                set => despl = value;
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

            public int DirDeMemoria // El desplazamiento.
            {
                get => dirDeMemoria;
                set => dirDeMemoria = value;
            }
            

            public string ImprimirObjetoTS()
            {
                string ret = "* LEXEMA: "+"\'"+lexema+"\' \n"+ "ATRIBUTOS: \n"
                             + "+ tipo: "+ "\'"+tipo+"\' \n"
                             + "+ despl: "+ "\'"+dirDeMemoria+"\' \n"
                    ;
                return ret;
            }
            
            
            
        }
    }
}
