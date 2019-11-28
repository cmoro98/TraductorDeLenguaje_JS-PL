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
            string pathReglas = "/home/krls/Programacion/C#/PDL/ProcesadorDeLenguaje_JS-PL/ProcesadorDeLenguaje_JS-PL/Ficheros_Del_Traductor/Sintactico/Reglas.txt";
            //ruta=Console.ReadLine();
            AnalisisLexico alex= new AnalisisLexico(pathSourceCode);
            //AnalisisLexico.Token tokenDevuelto;
            
            // LLamar An. SINTÁCTICO
            AnalizadorSintactico ast= new AnalizadorSintactico(alex, pathAstTablaAccion, pathAstTablaGoto,pathReglas);
            string parse = ast.GetParse();
            string listaTokens = ast.GetFichTokens();
            
            // Imprimir parse
            System.IO.File.Delete("parse.txt");
            using (System.IO.StreamWriter fichParse = new System.IO.StreamWriter(@"parse.txt", true))
            {
                fichParse.Write(parse);
            }
            System.IO.File.Delete("tokens.txt");
            using (System.IO.StreamWriter fichTokens = new System.IO.StreamWriter(@"tokens.txt", true))
            {
                fichTokens.Write(listaTokens);
            }

            
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
