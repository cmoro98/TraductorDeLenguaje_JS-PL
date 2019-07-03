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
        
        // TODO Guardar error en fichero
        public void GuardarError()
        {
            
        }

    }
}