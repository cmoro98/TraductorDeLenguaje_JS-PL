using System.Linq.Expressions;

namespace TablaSimbolos
{
    public interface ITablaDeSimbolos
    {
        //short? buscarPR(string lexema);
        int? buscarTS(string lexema);
        TablaDeSimbolos.ObjetoTS buscarObjTS(string lexema);
        int insertarTS(string lexema);
        int insertarTS(string lexema,int dirDeMemoria,string tipo);
        string ImprimirTS();
        //void insertarDespl();
        int NumeroTs { get; set; }
    }
}