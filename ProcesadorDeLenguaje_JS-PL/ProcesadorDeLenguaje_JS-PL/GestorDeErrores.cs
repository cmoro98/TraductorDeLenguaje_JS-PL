using System;

namespace ProcesadorDeLenguaje_JS_PL
{
    public class GestorDeErrores
    {
        public GestorDeErrores()
        {
            //
        }

        public void Error(string error)
        {
            Console.WriteLine(error);
            Console.WriteLine("Teclee una letra para continuar...");
            Console.ReadKey();
        }

        public void ErrSintactico(int nErr,string error)
        {
            
        }
        
        // TODO Guardar error en fichero
        public void GuardarError()
        {
            
        }

    }
}