using System.Text.RegularExpressions;

namespace ProcesadorDeLenguaje_JS_PL
{
    abstract class AbstractAutomata
    {
        protected int _s; // estado inicial
        protected int[] _f; // estados finales
        protected int[,] _stateTable; // Tabla de transicion.

        /*
         * Crea un objeto automata y lo inicializa con estado inicial S=0
         */
        public AbstractAutomata(int[] F, int[,] stateTable)
        {
            _s = 0;
            _f = F;
            _stateTable = stateTable;
        }
        /*Funcion de transicion
          s= estado actual, c= simbolo leido por el automata
         */
        public abstract int GetState(int s, Regex c);// regex o char?
        
        /*Evalua la cadena e indica si pertenece a una expresion regular de el aturomata
         * 
         */
        public abstract bool RecognizeToken(string inputString);
        
        
        /*Algoritmo de reconocimiento del token*/
        protected bool RecognizeBase(string inputString)
        {
            int n = 0;
            char c = inputString[n++];
            //Recorremos la cadena de entrada
            while (n<=inputString.Length)
            {
                // Obtenemos el estado siguiente para Q(k,sigma)
                _s = GetState(_s, c);
                
                // si no se ha terminado la cadena y Q(K,sigma) != -1
                if (_s != -1 && n < inputString.Length)
                {
                    c = inputString[n++];
                }
                else
                {   // Si Q(K,sigma) =-1 dejamos de analizar
                    break;
                }
            }
            
            //Buscamos S en F
            for (int i = 0; i < _f.Length; i++)
            {
                if (_f[i] == _s)
                    _s = 0;
                return true;
            }

            _s = 0;
            return false;
        }
    }
}