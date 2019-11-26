using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AnalizadorLexico;


namespace ProcesadorDeLenguaje_JS_PL {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hola usuario que tal estas?      ");
          //  Console.ReadKey();
            string pathSourceCode="/home/krls/Programacion/C#/PDL/ProcesadorDeLenguaje_JS-PL/ProcesadorDeLenguaje_JS-PL/Codigos_de_Prueba/prueba0.txt";
            string pathAstTablaAccion = "/home/krls/Programacion/C#/PDL/ProcesadorDeLenguaje_JS-PL/ProcesadorDeLenguaje_JS-PL/Ficheros_Del_Traductor/Sintactico/TablaACCION.csv";
            string pathAstTablaGoto = "/home/krls/Programacion/C#/PDL/ProcesadorDeLenguaje_JS-PL/ProcesadorDeLenguaje_JS-PL/Ficheros_Del_Traductor/Sintactico/TablaGOTO.csv";
            //ruta=Console.ReadLine();
            AnalisisLexico alex= new AnalisisLexico(pathSourceCode);
            AnalisisLexico.Token tokenDevuelto;
            AnalizadorSintactico ast= new AnalizadorSintactico(alex, pathAstTablaAccion, pathAstTablaGoto);
            ast.GetParse();
            System.IO.File.Delete("tokens.txt");
            /*
            do {
                tokenDevuelto = alex.GetToken();
                using (System.IO.StreamWriter fichTokens = new System.IO.StreamWriter(@"tokens.txt", true)) {
                    if (tokenDevuelto != null) {
                        //fichTokens.WriteLine("hola");
                        fichTokens.WriteLine(tokenDevuelto.Imprimir()); 
                    }
                }
            }
            while (tokenDevuelto != null);
            */
            
        }
    }
}
