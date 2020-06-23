using System;

namespace ProcesadorDeLenguaje_JS_PL
{
    public class GestorDeErrores
    {
        // TODO: Contar errores. TIENE X erroes LOQ SEA o CORRECTO -> LEXICAMENTE / si..
        private int num_errores_lexico;
        private int num_errores_sintactico;
        private int num_errores_semantico;
        private AnalisisLexico alex;
        private string errores;
        private string warnings;
        private string errorActual;
        private string warningActual;
        private bool suprimirWarnings = false;
        public void  SetAnalizadorLexico(AnalisisLexico alex)// para poder acceder al num de linea. Tanto en el lex,sint, sem..
        {
            this.alex = alex;
            errores = "";
        }
        public GestorDeErrores()
        {
            num_errores_lexico = 0;
            num_errores_sintactico = 0;
            num_errores_semantico = 0;
        }

        public void ErrLexico(string error)
        {
            errores += "\n" + error;
            Console.WriteLine(error);
            Console.WriteLine("Teclee una letra para continuar...    no se asegura que los siguientes errores sean correctos.");
            Console.ReadKey();
        }

        public void ErrSintactico(int nErr,string error)
        {
            errorActual = "Error SINTACTICO: Linea(" + alex.NumLineaCodigo + ") " + error;
            errores += "\n" + errorActual;
            Console.WriteLine(errorActual);
            Console.WriteLine("Teclee una letra para continuar...");
            Console.ReadKey();
        }

        public void ErrSemantico(int nErr, string error, int idNumLineaCodigo)
        {

            num_errores_semantico++;
            errorActual = "Error SEMANTICO: Linea(" + idNumLineaCodigo + ") " + error;
            errores += "\n" + errorActual;
            Console.WriteLine(errorActual);
            
            if (nErr==1)
            {
                Environment.Exit(-1);
            }
        }

        public void WarningSemantico(string warning, int idNumLineaCodigo)
        {
            warningActual = "Warning Linea("+ idNumLineaCodigo+ ") " + warning;
            warnings += " \n " + warningActual;
            Console.WriteLine(warningActual);
            if (!suprimirWarnings)
            {
                errores+= " \n " + warningActual;
            }
            
        }

        public void getInformeErroes()
        {
            Console.WriteLine("Lexico: " + num_errores_lexico + " Errores");
            Console.WriteLine("Sintactico: " + num_errores_sintactico+ " Errores");
            Console.WriteLine("Semantico: " + num_errores_semantico+ " Errores");

        }
        
        // TODO Guardar error en fichero
        public void GuardarError()
        {
            
        }

    }
}