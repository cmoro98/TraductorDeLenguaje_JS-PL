using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AnalizadorLexico;


namespace ProcesadorDeLenguaje_JS_PL {
    class Program {
        public static void funcion()
        {
            
        }
        // ruta para ficheros internos
       
        static void Main(string[] args) {
            
            //Console.WriteLine("Hola usuario que tal estas?      ");
            //Console.WriteLine(args.Length);
           
            GestorDeErrores gestorDeErrores = new GestorDeErrores();
            GestorTS gestorTs = new GestorTS();
           
            string pathSourceCode = @".."+Path.DirectorySeparatorChar+"../Codigos_de_Prueba/prueba0.txt";
            if (args.Length >= 1)
            {
                //Console.WriteLine(args[0]);
                pathSourceCode = (File.Exists(args[0]) ?  args[0] : @".." + Path.DirectorySeparatorChar + "../Codigos_de_Prueba/prueba0.txt");
                if (!File.Exists(args[0]))
                {
                    Console.WriteLine("El fichero no existe.");
                }
                //Console.WriteLine(File.Exists(args[0]) ? "File exists." : "File does not exist.");
               
            }
            
            string pathAstTablaAccion = @".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+"Ficheros_Del_Traductor"+Path.DirectorySeparatorChar+"Sintactico"+Path.DirectorySeparatorChar+"TablaACCION.csv";
            string pathAstTablaGoto = @".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+"Ficheros_Del_Traductor"+Path.DirectorySeparatorChar+"Sintactico"+Path.DirectorySeparatorChar+"TablaGOTO.csv";
            string pathReglas =  @".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+"Ficheros_Del_Traductor"+Path.DirectorySeparatorChar+"Sintactico"+Path.DirectorySeparatorChar+"Reglas.txt";
            AnalisisLexico alex= new AnalisisLexico(pathSourceCode,gestorTs,gestorDeErrores);
            gestorDeErrores.SetAnalizadorLexico(alex);
            
            // LLamar An. SINTÁCTICO
            AnalizadorSintactico ast= new AnalizadorSintactico(alex, gestorTs, pathAstTablaAccion, pathAstTablaGoto,pathReglas,gestorDeErrores);
            string parse = ast.GetParse();
            string listaTokens = ast.GetFichTokens();
            string tablaSimbolos = gestorTs.getFichTS();
            string codigoEnsamblador = ast.CodigoEnsamblador;
            
            // Ficheros donde guardamos el resultado.
            string pathParse = "../../Resultados/parse.txt";
            string pathTokens = "../../Resultados/tokens.txt";
            string pathTablaSimbolos = @"../../Resultados/TS.txt";
            string pathObjectCode = @"../../Resultados/a.ens";
            
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
            // ESCRIBIR CODIGO ENSAMBLADOR
            System.IO.File.Delete(pathObjectCode);
            using (System.IO.StreamWriter fichENS = new System.IO.StreamWriter(pathObjectCode, true))
            {
                fichENS.WriteLine(codigoEnsamblador); 
            }
            
          
          
            // TODO Verificar que SO estamos usando. Para monstrar interfaz si windows. NO TIENE UTILIDAD de momento.
            var os = Environment.OSVersion;
            //Console.WriteLine("SO: "+os.Platform);
           

        }
        
    }
}
