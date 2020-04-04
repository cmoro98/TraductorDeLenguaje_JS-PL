namespace ProcesadorDeLenguaje_JS_PL
{
    public class Atributo
    {
        
        
        /*Un atributo sirve tanto para un terminal commo para un no terminal.*/
        //Atributos normales
        private string simbolo;  // simbolo sea terminal: ID,PuntoComa... o NO terminal T,P..
        private string lexema; // propiedad de un Simb terminal ej ID.lexema
        private string tipo;
        private int ancho;
        //Atributos extras.
        private string tipoRet;
        private string listaVar;


        public Atributo()
        {
        }

        public Atributo(string simbolo)
        {
            this.lexema = lexema;
        }
        public Atributo(string simbolo,string lexema)
        {
            this.lexema = lexema;
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

        public string TipoRet
        {
            get => tipoRet;
            set => tipoRet = value;
        }

        public string ListaVar
        {
            get => listaVar;
            set => listaVar = value;
        }
        
        public int Ancho
        {
            get => ancho;
            set => ancho = value;
        }
    }
}