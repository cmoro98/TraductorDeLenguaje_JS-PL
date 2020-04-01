namespace TablaSimbolos
{
    public interface ITablaDeSimbolos
    {
        short? buscarPR(string lexema);
        short? buscarTS(string lexema);
        short insertarTS(string lexema);
        string ImprimirTS();
        int NumeroTs { get; set; }
    }
}