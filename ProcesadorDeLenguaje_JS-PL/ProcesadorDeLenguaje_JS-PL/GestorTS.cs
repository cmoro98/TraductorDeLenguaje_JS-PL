using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TablaSimbolos;

namespace ProcesadorDeLenguaje_JS_PL
{
    public class GestorTS
    {
        // esta clase gestiona las TS. Las crea busca etc.
        // Solo es util cuando existen dos tablas de simbolos al mismo tiempo.
        private bool tablaLocalActiva = false; 
        private TablaDeSimbolos tsg;
        private TablaDeSimbolos tsl;
        private Hashtable tablaPalabrasReservadas;
        private int numeroTS;// la global sera 0 la local sera 1,2,3...(Solo existen 2 al mismo tiempo , las locales se van
                             // creando y destruyendo)
                             
       // private int numElementos; // TODO: REVISAR SI ES NECESARIO
        private string fichTSLocal;
        private string fichTSGlobal;
        

        public GestorTS()
        {
            tablaPalabrasReservadas = new Hashtable();
            numeroTS = 0;
            tablaPalabrasReservadas.Add("if","if");
            tablaPalabrasReservadas.Add("function", "function");
            tablaPalabrasReservadas.Add("int","int");
            tablaPalabrasReservadas.Add("boolean", "boolean");
            tablaPalabrasReservadas.Add("string","string");
            tablaPalabrasReservadas.Add("true", "true");
            tablaPalabrasReservadas.Add("false","false");
            tablaPalabrasReservadas.Add("input","input");
            tablaPalabrasReservadas.Add("print","print");
            tablaPalabrasReservadas.Add("var", "var");
            tablaPalabrasReservadas.Add("do", "do");
            tablaPalabrasReservadas.Add("while","while");
            tablaPalabrasReservadas.Add("return","return");
            //numElementos = 13;
            fichTSLocal = "";
            fichTSGlobal = "";

        }

        public TablaDeSimbolos crearTS(bool global)
        {
            numeroTS++;
            if (global)
            {
                tsg = new TablaDeSimbolos(true);
                tsg.NumeroTs = numeroTS;
                return tsg;
            }
            tsl=new TablaDeSimbolos(false);
            tablaLocalActiva = true;
            tsl.NumeroTs = numeroTS;
            return tsl;
        }

        public void destruirTS()
        {
            if (tablaLocalActiva)
            {
                tsl = null;
                tablaLocalActiva = false;
            }
            else
            {
                tsg.ImprimirTS();
                tsg = null;
            }
        }

        public short? buscarPR(string lexema)
        {
            short? aux = 0;
            string ret =  (string) tablaPalabrasReservadas[lexema];
            return ret == null ? null : aux;
        }

        public int? buscarTS(string lexema)
        {
            if (tablaLocalActiva)
            {
                int? resLocal= tsl.buscarTS(lexema);
                if (resLocal != null)
                {
                    return resLocal;
                }
            }
            return tsg.buscarTS(lexema);
        }
        
        // Busqueda en la tabla de simbolos con la intencion de declarar.
        public int? buscarTSDeclarar(string lexema)
        {
            if (tablaLocalActiva)
            {
                return tsl.buscarTS(lexema);
            }
            return tsg.buscarTS(lexema);
        }
        public int? buscarTSGlobal(string lexema)
        {
            return tsg.buscarTS(lexema);
        }
        public int? buscarTSLocal(string lexema)
        {
            if (tablaLocalActiva)
            {
                int? resLocal= tsl.buscarTS(lexema);
                if (resLocal != null)
                {
                    return resLocal;
                }
            }
            return null;
        }

        public string buscaTipoTS(string lexema)
        {
            if (tablaLocalActiva)
            {
                if (tsl.buscarObjTS(lexema) != null)
                {
                    return tsl.buscarObjTS(lexema).Tipo;
                }
            }
            return tsg.buscarObjTS(lexema).Tipo;
        }
        public Tuple<TipoOperando, string> buscarDesplazamientoTS(string lexema)
        {
            if (tablaLocalActiva)
            {
                if (tsl.buscarObjTS(lexema) != null)
                {
                    return new Tuple<TipoOperando, string>(TipoOperando.Local, tsl.buscarObjTS(lexema).DirDeMemoria.ToString()); //;
                }
            }
            return  new Tuple<TipoOperando, string>(TipoOperando.Global, tsg.buscarObjTS(lexema).DirDeMemoria.ToString());
        }
        
        public List<Tipo> buscaTipoParametrosTS(string lexema)
        {
            return tsg.buscarObjTS(lexema).TiposParametros;
        }
        public string buscaTipoRetornoTS(string lexema)
        {
            return tsg.buscarObjTS(lexema).TipoDevuelto;
        }
        
        

        public int insertarTS(string lexema)
        {
            if (tablaLocalActiva)
            {
                return  tsl.insertarTS(lexema);
             
            }
            return tsg.insertarTS(lexema);
        }
        public int insertarTS(string lexema,int desplazamiento,string tipo)
        {
            if (tablaLocalActiva)
            {
                return  tsl.insertarTS(lexema,desplazamiento,tipo);
             
            }
            return tsg.insertarTS(lexema, desplazamiento, tipo);
        }

        public int crearNuevaTemporal( int desplazamiento, string tipo)
        {
            if (tablaLocalActiva)
            {
                return  tsl.crearTemporalTS(desplazamiento,tipo);
             
            }
            return tsg.crearTemporalTS(desplazamiento,tipo);
            
        }
        
        public int? insertarNumParametrosTS(string lexema,int numParametros)
        {
            if (tablaLocalActiva)
            {
                return tsl.insertarNumParametrosTS(lexema, numParametros);
            }
            return tsg.insertarNumParametrosTS(lexema, numParametros);
        }
        public int? insertarTipoParametrosTS(string lexema, List<Tipo> tipoParametros)
        {
/*            if (tablaLocalActiva)
            {
                return tsl.insertarTipoParametrosTS(lexema,  tipoParametros);
            }*/
            return tsg.insertarTipoParametrosTS(lexema,  tipoParametros);
        }
        public int? insertarEtiquetaTS(string lexema, string etiqueta)
        {
/*            if (tablaLocalActiva)
            {
                return tsl.insertarTipoParametrosTS(lexema,  tipoParametros);
            }*/
            return tsg.insertarEtiquetaTS(lexema,  etiqueta);
        }
        public int? insertarTipoRetornoTS(string lexema, string  tipoRetorno)
        {
            if (tablaLocalActiva)
            {
                return tsl.insertarTipoRetornoTS(lexema,  tipoRetorno);
            }
            return tsg.insertarTipoRetornoTS( lexema, tipoRetorno);
        }
        
        public void imprimirTS(string idLexema)
        {
            if (tablaLocalActiva)
            {
                fichTSLocal = fichTSLocal + " TABLA Local de la FUNCION "+idLexema +"  "+ tsl.ImprimirTS();
               
                return;
            }

            fichTSGlobal= "Tabla Global "+ tsg.ImprimirTS();
        }

        public string getFichTS()
        {
            return fichTSGlobal + fichTSLocal;
        }

        public bool TablaLocalActiva
        {
            get => tablaLocalActiva;
            set => tablaLocalActiva = value;
        }


        //Insertar_TS(TS,id.lexema,T.tipo,Despl)
       // public void insertar_TS(id.lexema, T.tipo, Despl)
    
        ///Busca_TS(TS,id.lexema)
    }
}