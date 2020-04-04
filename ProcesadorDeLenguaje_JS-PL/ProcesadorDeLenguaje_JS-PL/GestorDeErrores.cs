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
            Console.WriteLine("Teclee una letra para continuar...    no se asegura que los siguientes errores sean correctos.");
            Console.ReadKey();
        }

        public void ErrSintactico(int nErr,string error)
        {
            Console.WriteLine(error);
            Console.WriteLine("Teclee una letra para continuar...");
            Console.ReadKey();
        }

        public void ErrSemantico(int nErr,string error)
        {
            Console.WriteLine("Error semantico: " + error);
            Console.WriteLine("Teclee una letra para continuar...");
        }
        
        // TODO Guardar error en fichero
        public void GuardarError()
        {
            
        }

    }
}