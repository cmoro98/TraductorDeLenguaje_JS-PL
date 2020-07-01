using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ProcesadorDeLenguaje_JS_PL
{
    public class GCO
    {
        private string ensambladorFich = "\n";
        private string cadenasFich = "\n; zona con cadenas \n";
        private string funcionesFich;
        // funciones + ensamblador+ cadenas. 
        private int tamZonaEstatica = 10;
        private int num_cadena;
        private string cadena_etiq;
        public GCO()
        {
            //ensambladorFich = 
        }

        public void pon_etiqueta(string etiqueta)
        {
            ensambladorFich += etiqueta + ": \n";
        }

        public int TamZonaEstatica
        {
            get => tamZonaEstatica;
            set => tamZonaEstatica = value;
        }

        public string dameNombreEtiqCadena()
        {
            num_cadena++;
            return "nuevaCadena" + num_cadena;
            
        }
        public int codegen(Operador op, Atributo arg1, Atributo arg2, Atributo dest)
        {
            Console.WriteLine("Se ejecuta el cuarteto: %d, arg1: %s, arg2: %s, dest: %s", op, arg1, arg2, dest);
            // metodo Preparar operandos VIABLE ?

            switch (op)
            {
                /**
                 * code: input(a)
                 * quartet: (OP_INPUT, NULL, NULL, dest)
                 * asm:
                 *  MOVE `PTR+dest`, .R0
                 *  INSTR .R0
                */
                case  Operador.OP_INPUT:
                    /* code */
                    ensambladorFich += "; Input:  \n";
                    if (dest.Tipo == Tipo.@int)
                    {
                        ensambladorFich += "ININT " + asm[dest.TipoOperando](dest.Operando) +"\n";
                    }else if (dest.Tipo == Tipo.@string)
                    {
                        cadena_etiq = dameNombreEtiqCadena();
                        cadenasFich+=  cadena_etiq + ": RES " + 66 + " ; \n";
                        /*INSTR /aprueba
                        MOVE #aprueba,#0[.IY]*/
                        ensambladorFich += "INSTR /" + cadena_etiq +"\n";
                        ensambladorFich += "MOVE #" + cadena_etiq +", " + asm[dest.TipoOperando](dest.Operando) +"\n";
                    }
                    break;

                /**
                 * code: print(a)
                 * quartet: (OP_PRINT, NULL, NULL, dest)
                 * asm:
                 *  MOVE `PTR+dest`, .R0
                 *  WRSTR .R0
                */
                case Operador.OP_PRINT:
                    /* code */
                    ensambladorFich += "; Print:  \n";
                    if (dest.Tipo == Tipo.@int)
                    {
                        ensambladorFich += "WRINT " + asm[dest.TipoOperando](dest.Operando) +"\n";
                    }else if (dest.Tipo == Tipo.@string)
                    {
                        if ( dest.TipoOperando==TipoOperando.Inmediato )
                        {
                            // crear el temporal. arg1.lexema DATA "operando"
                            cadena_etiq = dameNombreEtiqCadena();
                            cadenasFich += cadena_etiq + ": DATA \"" + dest.Operando + "\" ; \n";
                           // ensambladorFich += "MOVE #" + cadena_etiq+ " , "+ asm[dest.TipoOperando](dest.Operando) +"\n";
                            
                            ensambladorFich +=  "MOVE #" + cadena_etiq+ " , .R1" +"\n";
                            ensambladorFich += "WRSTR [.R1]" +"\n"; 
                        }
                        else
                        {
                            ensambladorFich += "MOVE "  + asm[dest.TipoOperando](dest.Operando)  + ", .R1" +"\n";
                            ensambladorFich += "WRSTR [.R1]" +"\n"; 
                        }

                    }
                    
                    break;

                /**
                 * code: a + b
                 * quartet: (OP_ADD, arg1, arg2, dest)
                 * asm:
                 *  MOVE `PTR1+dest`, .R0
                 *  MOVE `PTR2+arg1`, .R1
                 *  MOVE `PTR3+arg2`, .R2 
                 *  ADD [.R1], [.R2]
                 *  MOVE .A, [.R0]
                */
                case Operador.OP_PLUS:
                    // inmediato e inmediato. ID e inmediato .. 4 posibilidades.
                    asm[arg1.TipoOperando](arg1.Operando);
                    ensambladorFich += "; Suma a continuacion \n";
                    ensambladorFich += "ADD " + asm[arg1.TipoOperando](arg1.Operando) +" , "+ asm[arg2.TipoOperando](arg2.Operando) +"\n";
                    ensambladorFich += "MOVE " + ".A , " + asm[dest.TipoOperando](dest.Operando) +"\n";
                    
                    break;

                /**
                 * code: a & b
                 * quartet: (OP_AND, arg1, arg2, dest)
                 * asm:
                 *  MOVE `PTR1+dest`, .R0
                 *  MOVE `PTR2+arg1`, .R1
                 *  MOVE `PTR3+arg2`, .R2 
                 *  AND [.R1], [.R2]
                 *  MOVE .A, [.R0]
                */
                case Operador.OP_AND:
                    /* code */
                    break;

                /**
                 * FIXME
                 * code: a == b
                 * quartet: (OP_EQUALS, arg1, arg2, dest)
                 * asm:
                 *  MOVE `PTR1+dest`, .R0
                 *  MOVE `PTR2+arg1`, .R1
                 *  MOVE `PTR3+arg2`, .R2 
                */
                case Operador.OP_EQUALS:
                    /* code */
                    break;

                /**
                 * code: if (cond) then x
                 * quartet: (OP_IF, arg1, NULL, dest)
                 * asm:
                 *  MOVE `PTR1+arg1`, .R1
                 *  CMP #0, [.R1]
                 *  BZ /dest
                */
                case Operador.OP_IF_N:
                    /* Si arg1 es != arg2. SALTA A ETIQUETA. N*/
                    ensambladorFich += ";un IF: \n";
                    ensambladorFich += "CMP " + asm[arg1.TipoOperando](arg1.Operando) +" , "+ asm[arg2.TipoOperando](arg2.Operando) +"\n";
                    ensambladorFich += "BNZ " + asm[dest.TipoOperando](dest.Operando)+"\n"; 
                   
                    break;
                case Operador.OP_IF:
                    /* Si arg1 es == arg2. SALTA A ETIQUETA. N*/
                    ensambladorFich += ";un IF Si  \n";
                    ensambladorFich += "CMP " + asm[arg1.TipoOperando](arg1.Operando) +" , "+ asm[arg2.TipoOperando](arg2.Operando) +"\n";
                    ensambladorFich += "BZ " + asm[dest.TipoOperando](dest.Operando)+"\n"; 
                   
                    break;
                /**
                 * code: goto L
                 * quartet: (OP_GOTO, NULL, NULL, dest)
                 * asm:
                 *  BR /dest
                */
                case Operador.OP_GOTO:
                    /* code */
                    break;

                /**
                 * code: a--
                 * quartet: (OP_POST_DEC, arg1, NULL, dest)
                 * asm:
                 *  MOVE `PTR1+dest`, .R0
                 *  MOVE `PTR1+arg1`, .R1
                 *  MOVE [.R1], [.R0]
                 *  MOVE [.R1], .R9
                 *  DEC [.R0]
                */
                case Operador.OP_POST_INC:
                    /* code */
                    ensambladorFich += ";Incremento: \n";
                    ensambladorFich += "INC " + asm[dest.TipoOperando](dest.Operando) +"\n";
                    break;

                /**
                 * code: a = b
                 * quartet: (OP_ASIGN, arg1, NULL, dest)
                 * asm:
                 *  MOVE `PTR1+dest`, .R0
                 *  MOVE `PTR2+arg1`, .R1
                 *  MOVE [.R1], [.R0]
                */
                case Operador.OP_ASIG:
                    // check if is a ID or inmediate
                    // if inmediate check if int or string.
                    // store de id or the inmediate in a register and put on the stack
                    // existe_entrada(arg1, "");
                    //dprintf(_fd, "MOVE %s, %s\n", atoi(yytext));
                    //  write(_fd,"MOVE , op2",3);

                    //dest = arg1;
                    // TODO: Esto no se si funcionaría en strings
                  
                    ensambladorFich += "; Asignacion   \n";
                    if (dest.Tipo == Tipo.@string && arg1.TipoOperando==TipoOperando.Inmediato )
                    {
                        // crear el temporal. arg1.lexema DATA "operando"
                        cadena_etiq = dameNombreEtiqCadena();
                        cadenasFich += cadena_etiq + ": DATA \"" + arg1.Operando + "\" ; \n";
                        ensambladorFich += "MOVE #" + cadena_etiq+ " , "+ asm[dest.TipoOperando](dest.Operando) +"\n";
                    }
                    else 
                    {
                        
                        ensambladorFich += "MOVE " + asm[arg1.TipoOperando](arg1.Operando) +" , "+ asm[dest.TipoOperando](dest.Operando) +"\n";
                    }
                    /*switch (arg1.TipoOperando)
                    {
                        // TODO: Resuelve el destiono en un string. o ANTES
                        case TipoOperando.Inmediato:
                            if (arg1.Tipo == Tipo.@string)
                            {
                                ensambladorFich += "MOVE" + arg1.Operando + "," + dest.Operando;
                            }
                            else if (arg1.Tipo == Tipo.@int)
                            {
                                Console.WriteLine("asignando un int a un id");
                                ensambladorFich+="MOVE #"+arg1.Operando +"," +"#" + dest.Operando + "[.IY]"; // mal solo sirve para un DESTINO estatico.
                                
                                
                            }

                            break;
                        case TipoOperando.Local:
                            break;
                        case TipoOperando.Global:
                            if (arg1.Tipo == Tipo.@string)
                            {
                                ensambladorFich += "MOVE" + arg1.Operando + "," + dest.Operando;
                            }
                            else if (arg1.Tipo == Tipo.@int)
                            {
                                Console.WriteLine("asignando un ID int a un id");
                                ensambladorFich+="MOVE #"+arg1.Operando+"[IY]" +"," +"#" + dest.Operando + "[.IY]"; // mal solo sirve para un DESTINO estatico.
                                
                                
                            }
                            break;
                    }*/
                    break;
                case Operador.OP_ETIQ:
                    /* code */
                    pon_etiqueta(dest.Operando);
                    
                    
                    break;

                default:
                    return -1;
                    
            }

            return 0;
        }
        private Dictionary<TipoOperando, Func<string, string>> asm =
            new Dictionary<TipoOperando, Func<string, string>>
            {
                { TipoOperando.Global, ( a) => "#"+a+"[.IY]" },
                { TipoOperando.Local, ( a ) => "#"+a+"[.IX]" },
                { TipoOperando.Inmediato, ( a ) => "#"+a },
                { TipoOperando.Etiqueta, ( a ) => "$"+a },
            };

        public string ensamblate(List<Cuarteto> pAxiomaCodigo)
        {
            for (int i = 0; i < pAxiomaCodigo.Count; i++)
            {
                codegen(pAxiomaCodigo[i].Operador,pAxiomaCodigo[i].Arg1,pAxiomaCodigo[i].Arg2,pAxiomaCodigo[i].Dest);
            }
            return ensambladorFich;
        }

        public void finaliza()
        {
            ensambladorFich = "ORG 0 \n" + "inicio_estaticas: RES " + tamZonaEstatica + " \n" +"MOVE #inicio_estaticas, .IY \n"+ funcionesFich +ensambladorFich +"HALT \n"+ cadenasFich+ "END ";
        }

        public string EnsambladorFich
        {
            get => ensambladorFich;
            set => ensambladorFich = value;
        }


        /*private Dictionary<Operador, Func<int, int, double>> operators =
            new Dictionary<Operador, Func<int, int, double>>
            {
                { Operador.PLUS, ( a, b ) => a + b },
                { Operador.MINUS, ( a, b ) => a - b },
                { Operador.MULTIPLY, ( a, b ) => a * b },
                { Operador.DIVIDE ( a, b ) => (double)a / b },
            };*/
    }
}