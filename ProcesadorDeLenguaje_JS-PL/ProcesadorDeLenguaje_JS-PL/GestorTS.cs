using System;
using System.Collections;
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
                             
        private int numElementos;
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
            numElementos = 13;
            fichTSLocal = "";
            fichTSGlobal = "";

        }

        public TablaDeSimbolos crearTS(bool global)
        {
            if (global)
            {
                tsg = new TablaDeSimbolos(true);
                tsg.NumeroTs = numeroTS;
                numeroTS++;
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
                tsl.ImprimirTS();
                tsl = null;
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
            /*if (tablaLocalActiva)
            {
                short? resLocal= tsl.buscarPR(lexema);
                if (resLocal != null)
                {
                    return resLocal;
                }
            }*/
            //return tsg.buscarPR(lexema);
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

        public string buscaTipoTS(string lexema)
        {
            if (tablaLocalActiva)
            {
                return tsl.buscarObjTS(lexema).Tipo;
            }
            return tsg.buscarObjTS(lexema).Tipo;
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
/*        public short insertarDespl(string id,int despl)
        {
            if (tablaLocalActiva)
            {
                return  tsl.insertarTS(lexema);
             
            }
            return tsg.insertarTS(lexema);
        }*/

        public void imprimirTS()
        {
            if (tablaLocalActiva)
            {
                fichTSLocal+=tsl.ImprimirTS();
                return;
            }

            fichTSGlobal=tsg.ImprimirTS();
        }

        public string getFichTS()
        {
            return fichTSLocal+fichTSGlobal;
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