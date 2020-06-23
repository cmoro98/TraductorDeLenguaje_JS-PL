using System.Collections.Generic;
using System.Linq.Expressions;
using ProcesadorDeLenguaje_JS_PL;

namespace TablaSimbolos
{
    public interface ITablaDeSimbolos
    {
        //short? buscarPR(string lexema);
        int? buscarTS(string lexema);
        TablaDeSimbolos.ObjetoTS buscarObjTS(string lexema);
        int insertarTS(string lexema);
        int insertarTS(string lexema,int dirDeMemoria,string tipo);
        int? insertarNumParametrosTS(string lexema, int numParametros);
        int? insertarTipoParametrosTS(string lexema, List<Tipo> tipoParametros);
        int? insertarTipoRetornoTS(string lexema, string  tipoRetorno);
        
        string ImprimirTS();
        //void insertarDespl();
        int NumeroTs { get; set; }
    }
}