using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AnalizadorLexico;


namespace ProcesadorDeLenguaje_JS_PL {
    class Program {
        static void Main(string[] args) {
            
            Console.WriteLine("Hola usuario que tal estas?      ");
            GestorDeErrores gsErrores;
            GestorTS gestorTs = new GestorTS();
            /*
            string pathSourceCode="/home/krls/Programacion/C#/PDL/ProcesadorDeLenguaje_JS-PL/ProcesadorDeLenguaje_JS-PL/Codigos_de_Prueba/prueba0.txt";
            string pathAstTablaAccion = "/home/krls/Programacion/C#/PDL/ProcesadorDeLenguaje_JS-PL/ProcesadorDeLenguaje_JS-PL/Ficheros_Del_Traductor/Sintactico/TablaACCION.csv";
            string pathAstTablaGoto = "/home/krls/Programacion/C#/PDL/ProcesadorDeLenguaje_JS-PL/ProcesadorDeLenguaje_JS-PL/Ficheros_Del_Traductor/Sintactico/TablaGOTO.csv";
            string pathReglas = "/home/krls/Programacion/C#/PDL/ProcesadorDeLenguaje_JS-PL/ProcesadorDeLenguaje_JS-PL/Ficheros_Del_Traductor/Sintactico/Reglas.txt";
            */
            string pathSourceCode=@".."+Path.DirectorySeparatorChar+"../Codigos_de_Prueba/prueba0.txt";
            string pathAstTablaAccion = @".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+"Ficheros_Del_Traductor"+Path.DirectorySeparatorChar+"Sintactico"+Path.DirectorySeparatorChar+"TablaACCION.csv";
            string pathAstTablaGoto = @".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+"Ficheros_Del_Traductor"+Path.DirectorySeparatorChar+"Sintactico"+Path.DirectorySeparatorChar+"TablaGOTO.csv";
            string pathReglas =  @".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+"Ficheros_Del_Traductor"+Path.DirectorySeparatorChar+"Sintactico"+Path.DirectorySeparatorChar+"Reglas.txt";
            AnalisisLexico alex= new AnalisisLexico(pathSourceCode,gestorTs);
            
            
            // LLamar An. SINTÁCTICO
            AnalizadorSintactico ast= new AnalizadorSintactico(alex, gestorTs, pathAstTablaAccion, pathAstTablaGoto,pathReglas);
            string parse = ast.GetParse();
            string listaTokens = ast.GetFichTokens();
            string tablaSimbolos = gestorTs.getFichTS();
            
            // Ficheros donde guardamos el resultado.
            string pathParse = "../../Resultados/parse.txt";
            string pathTokens = "../../Resultados/tokens.txt";
            string pathTablaSimbolos = @"../../Resultados/TS.txt";
            
            // ESCRIBIR FICHERO PARSE.
            System.IO.File.Delete(pathParse);
            using (System.IO.StreamWriter fichParse = new System.IO.StreamWriter(pathParse, true))
            {
                fichParse.Write(parse);
            }
            // ESCRIBIR FICHERO TOKENS
            System.IO.File.Delete(pathTokens);
            using (System.IO.StreamWriter fichTokens = new System.IO.StreamWriter(pathTokens, true))
            {
                fichTokens.Write(listaTokens);
            }
            // ESCRIBIR FICHERO TABLA SIMBOLOS
            System.IO.File.Delete(pathTablaSimbolos);
            using (System.IO.StreamWriter fichTS = new System.IO.StreamWriter(pathTablaSimbolos, true))
            {
                fichTS.WriteLine(tablaSimbolos); 
            }
            
          
          
            // TODO Verificar que SO estamos usando. Para monstrar interfaz si windows. NO TIENE UTILIDAD de momento.
            var os = Environment.OSVersion;
            Console.WriteLine("SO: "+os.Platform);


        }
    }
}
