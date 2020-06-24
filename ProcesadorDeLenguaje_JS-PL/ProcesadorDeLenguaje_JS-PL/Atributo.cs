using System;
using System.Collections.Generic;

namespace ProcesadorDeLenguaje_JS_PL
{
    public enum AmbitoVariable
    {
        Global,Local,NoLocal
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
        
        // Atributos GCI
        private Tuple<AmbitoVariable,int> lugar;

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

        public Tuple<AmbitoVariable, int> Lugar
        {
            get => lugar;
            set => lugar = value;
        }

        public int NumLineaCodigo => numLineaCodigo;
    }

    public enum Tipo { UNDEFF,TIPO_OK,TIPO_ERROR,@int,@string,boolean, vacio,function };
}