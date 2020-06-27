using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ProcesadorDeLenguaje_JS_PL
{
    public class GCO
    {
        private string ensambladorFich;
        public int codegen(Operador op, Atributo arg1, Atributo arg2, Atributo dest)
        {
            Console.WriteLine("Se ejecuta el cuarteto: %d, arg1: %s, arg2: %s, dest: %s", op, arg1, arg2, dest);

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
                case Operador.OP_IF:
                    /* code */
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
                case Operador.OP_POST_DEC:
                    /* code */
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
                    switch (arg1.TipoOperando)
                    {
                        case TipoOperando.Inmediato:
                            if (arg1.Tipo == Tipo.@string)
                            {
                                ensambladorFich += "MOVE" + arg1.Operando + "," + dest.Operando;
                            }
                            else if (arg1.Tipo == Tipo.@int)
                            {
                                Console.WriteLine("asignando un int a un id");
                                ensambladorFich+="MOVE #"+arg1.Operando +"," +"#" + dest.Operando + "[.IY]"; // mal solo sirve pa estatico.
                                
                            }

                            break;
                        case TipoOperando.Local:
                            break;
                        case TipoOperando.Global:
                            break;
                    }
                    
                    
                    
                    break;

                default:
                    return -1;
                    
            }

            return 0;
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