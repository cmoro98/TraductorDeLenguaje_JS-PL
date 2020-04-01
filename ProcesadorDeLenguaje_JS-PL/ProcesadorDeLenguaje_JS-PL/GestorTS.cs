using System;
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
        private int numeroTS;// la global sera 0 la local sera 1,2,3...(Solo existen 2 al mismo tiempo , las locales se van
                             // creando y destruyendo)
        public GestorTS()
        {
            numeroTS = 0;
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
            /*if (tablaLocalActiva)
            {
                short? resLocal= tsl.buscarPR(lexema);
                if (resLocal != null)
                {
                    return resLocal;
                }
            }*/
            return tsg.buscarPR(lexema);
        }

        public short? buscarTS(string lexema)
        {
            if (tablaLocalActiva)
            {
                short? resLocal= tsl.buscarTS(lexema);
                if (resLocal != null)
                {
                    return resLocal;
                }
            }
            return tsg.buscarTS(lexema);
        }

        public short insertarTS(string lexema)
        {
            if (tablaLocalActiva)
            {
                return  tsl.insertarTS(lexema);
             
            }
            return tsg.insertarTS(lexema);
        }

        public void imprimirTS()
        {
            if (tablaLocalActiva)
            {
                tsl.ImprimirTS();
                return;
            }

            tsg.ImprimirTS();
        }
        
        
        
        
        //Insertar_TS(TS,id.lexema,T.tipo,Despl)
       // public void insertar_TS(id.lexema, T.tipo, Despl)
    
        ///Busca_TS(TS,id.lexema)
    }
}