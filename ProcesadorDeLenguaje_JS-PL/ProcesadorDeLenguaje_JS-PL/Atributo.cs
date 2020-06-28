using System;
using System.Collections.Generic;

namespace ProcesadorDeLenguaje_JS_PL
{
    public enum TipoOperando
    {
        Inmediato,Global,Local,Etiqueta
        // parametro por valor -> = Local
        // temporal            -> = Local
        
    }

    public class Atributo
    {
        
        
        /*Un atributo sirve tanto para un terminal commo para un no terminal.*/
        //Atributos normales
        private string simbolo;  // simbolo sea terminal: ID,PuntoComa... o NO terminal T,P..
        private string lexema; // propiedad de un Simb terminal ej ID.lexema
        //private string tipo;
        private int ancho;
        //Atributos extras.
        private Tipo tipoRet;
        private List<Tipo>  listaVar ;
        private int numLineaCodigo;
        // estos realmente es un def. Lo suyo hubiese sido reutilizar token y añadirle los atributos. Mucho más facil y limpio, pero ya no lo voy a cambiar.
        // Para cadena
        private string cadena;
        // para digitos
        private short digito; 
        
        // Atributos GCI
       // private Tuple<TipoOperando,string> lugar;
        private TipoOperando tipoOperando;
        private string operando;

        private List<Cuarteto> codigo;

        public Atributo()
        {
            listaVar = new List<Tipo>();
        }

        public Atributo(string simbolo)
        {
            this.simbolo = simbolo;
        }
        
        public Atributo(string simbolo,int numLineaCodigo)
        {
            this.simbolo = simbolo;
            this.numLineaCodigo = numLineaCodigo;
        }
        
        public Atributo(string simbolo,string lexema,int numLineaCodigo)
        {
            this.simbolo = simbolo;
            this.lexema = lexema;
            this.numLineaCodigo = numLineaCodigo;
        }

        public Atributo(string simbolo, int numLineaCodigo, short? valor)
        {
            this.simbolo = simbolo;
            this.numLineaCodigo = numLineaCodigo;
            this.digito =  (short) valor;
        }

        public Atributo(string  simbolo, int numLineaCodigo, string valor)
        {
            this.simbolo = simbolo;
            this.numLineaCodigo = numLineaCodigo;
            this.cadena =  valor;
        }


        public string Lexema
        {
            get => lexema;
            set => lexema = value;
        }

        public Tipo Tipo { get; set; }
        
        public Tipo TipoRet
        {
            get => tipoRet;
            set => tipoRet = value;
        }

        public List<Tipo> ListaVar
        {
            get => listaVar;
            set => listaVar = value;
        }
        
        public int Ancho
        {
            get => ancho;
            set => ancho = value;
        }

        public string Simbolo
        {
            get => simbolo;
            set => simbolo = value;
        }

        /*public Tuple<TipoOperando, string> Lugar
        {
            get => lugar;
            set => lugar = value;
        }*/

        public TipoOperando TipoOperando
        {
            get => tipoOperando;
            set => tipoOperando = value;
        }

        public string Operando
        {
            get => operando;
            set => operando = value;
        }


        public int NumLineaCodigo => numLineaCodigo;

        public string Cadena
        {
            get => cadena;
            set => cadena = value;
        }

        public short Digito
        {
            get => digito;
            set => digito = value;
        }

        public List<Cuarteto> Codigo
        {
            get => codigo;
            set => codigo = value;
        }
    }

    public enum Tipo { UNDEFF,TIPO_OK,TIPO_ERROR,@int,@string,boolean, vacio,function };
}