namespace ProcesadorDeLenguaje_JS_PL
{
    public class Cuarteto
    {
        private Operador operador;
        private Atributo arg1;
        private Atributo arg2;
        private Atributo dest;

        public Cuarteto(Operador operador, Atributo arg1, Atributo arg2, Atributo dest)
        {
            this.operador = operador;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.dest = dest;
        }

        public Operador Operador
        {
            get => operador;
            set => operador = value;
        }

        public Atributo Arg1
        {
            get => arg1;
            set => arg1 = value;
        }

        public Atributo Arg2
        {
            get => arg2;
            set => arg2 = value;
        }

        public Atributo Dest
        {
            get => dest;
            set => dest = value;
        }
    }
}