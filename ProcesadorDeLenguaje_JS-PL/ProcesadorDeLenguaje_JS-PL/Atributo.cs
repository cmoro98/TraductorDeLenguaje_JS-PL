namespace ProcesadorDeLenguaje_JS_PL
{
    public class Atributo
    {
        //Atributos normales
            private string lexema;
            private string tipo;
            private int ancho;
            //Atributos extras.
            private string tipoRet;
            private string listaVar;


            public Atributo()
            {
            }

            public Atributo(string lexema, string tipo)
            {
                this.lexema = lexema;
                this.tipo = tipo;
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