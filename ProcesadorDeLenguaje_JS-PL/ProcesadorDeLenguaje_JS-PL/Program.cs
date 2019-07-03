using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalizadorLexico;

namespace ProcesadorDeLenguaje_JS_PL {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hola usuario que tal estas?      ");
            Console.ReadKey();
            string ruta="/home/krls/Programacion/C#/PDL/ProcesadorDeLenguaje_JS-PL/ProcesadorDeLenguaje_JS-PL/Codigos_de_Prueba/prueba0.txt";
            //ruta=Console.ReadLine();
            AnalisisLexico alex= new AnalisisLexico(ruta);
            Token tokenDevuelto = null;
            do {
                tokenDevuelto = alex.GetToken();
                using (System.IO.StreamWriter fichTokens = new System.IO.StreamWriter(@"tokens.txt", true)) {
                    if (tokenDevuelto != null) {
                        fichTokens.WriteLine(tokenDevuelto.toString()); 
                    }
                }
            }
            while (tokenDevuelto != null);
            
        }
    }
}
