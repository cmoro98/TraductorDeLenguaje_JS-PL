using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcesadorDeLenguaje_JS_PL
{
    public class AnalizadorSemantico
    {
        private GestorDeErrores gestorDeErrores;
        GestorTS gesTS;
        private Desplazamiento despl;
        private readonly AnalisisLexico alex;
        private bool errorDeclFuncion = false;
        private Atributo id_funcion;
        private readonly int size_int = 1;
        private readonly int size_string = 1;
        private readonly int size_bool= 1;
        
        
        public AnalizadorSemantico(GestorTS gesTS, AnalisisLexico alex, GestorDeErrores gestorDeErroreserrores)
        {
            this.alex = alex;
            this.gesTS = gesTS;
            this.gestorDeErrores = gestorDeErroreserrores;
            despl = new Desplazamiento();
            id_funcion = null;
        }

        // Declaramos Nuestros atributos de los no terminales.(que son los q tienen atributos)
        
        // La pila la utilizamos para en el sintactico guardar y ahora, en el semantico sacar atributos de ella.pu

        public  string ejecAccSemantica(int numRegla,Stack<Atributo> pilaSemantico)
        {   
            //Console.WriteLine("Acc Sem: "+ numRegla );
            Atributo PAxioma, P, B , F, T, E, C, S, L, Q, X, H, A, K, R, U, V; //No terminales
            Atributo  var, ID, @if, AbreParent, CierraParent, AbreCorchetes ,@while ,CierraCorchetes,  @int, boolean ,@string ,IGUAL ,PuntoComa ,@return ,print ,input ,COMA, @do  ,function, AND, IGUALIGUAL, Suma ,MASMAS, digito, cadena ,@true, @false;
            Atributo P1;
            switch (numRegla)
            {
                case 1: // 1 PAxioma -> {Crear TSG, DesplG=0}P{imprimirTS,DestruirTSG}#2#
                   // gesTS.crearTS(true);
                   gesTS.imprimirTS("");
                   gesTS.destruirTS();
                    break;
                case 2:
                    // Antecedente
                    P = pilaSemantico.Pop();
                    // Consecuentes
                    P1 = pilaSemantico.Pop();
                    B = pilaSemantico.Pop();
                    pilaSemantico.Push(P);
                    break;
                case 3:
                    //  3 P -> F P  //Nada
                    // Antecedente
                    P = pilaSemantico.Pop();
                    // Consecuentes
                    P1 = pilaSemantico.Pop();
                    F = pilaSemantico.Pop();
                    pilaSemantico.Push(P);
                    
                    break;
                case 4:
                    break;
                case 5://5 B -> var T ID PuntoComa {
                    //if(TSL==Null) then TS<-TSG; Despl<-DesplG
                    //else TS<-TSL  Despl<-DesplLocal
                    
                    
                    // sacamos elems de la pila
                    // Antecedente
                    B = pilaSemantico.Pop();
                    // Consecuentes
                    PuntoComa = pilaSemantico.Pop();
                    ID = pilaSemantico.Pop();
                    T = pilaSemantico.Pop();
                    var = pilaSemantico.Pop();
                    
                    // Ejecutamos la accion semántica en si:
                    
                    /*if (gesTS.TablaLocalActiva) 
                    {
                        despl.useDesplL();
                    }*/
                   

                    if (gesTS.buscarTSDeclarar(ID.Lexema) != null)
                    {
                        //ERROR variable ya declarada. 
                        gestorDeErrores.ErrSemantico(2,"ERROR, la variable: "+ID.Lexema+" ya esta declarada",ID.NumLineaCodigo);
                        B.Tipo = Tipo.TIPO_ERROR;
                    }
                    else
                    {
                        B.Tipo = Tipo.TIPO_OK;
                        B.TipoRet = Tipo.vacio;
                        gesTS.insertarTS(ID.Lexema,despl.Despl,T.Tipo.ToString());
                        //gesTS.in
                        despl.Despl += T.Ancho;
                    }
                    pilaSemantico.Push(B);
                    despl.update();
                    //if(buscaTS(ID.lexema!=null)) then ERROR  Variable ya declarada
                    //else id.pos=InsertaTS(TS,id.lexema,T.tipo,Despl)
                    //despl+=T.ancho} //Declaracion de variable#3#
            
                    break;
                case 6: // TODO:
                    /*
                     *  6 B -> if AbreParent E CierraParent
                     * {
                     *     B.tipo=if(E.tipo==boolean) then Ok_tipo
                                else Tipo_error (Entre los parentesis del if se espera una expresion logica)
                       } S// comprobar que E es un booleano.
                     */
                    // sacar de la pila:
                    
                    // Antecedente
                    B = pilaSemantico.Pop();
                    // Consecuente
                    S = pilaSemantico.Pop(); // TODO: revisar
                    CierraParent = pilaSemantico.Pop();
                    E = pilaSemantico.Pop();
                    AbreParent = pilaSemantico.Pop();
                    @if = pilaSemantico.Pop();
                    
                    // Accion semantica 
                    if (E.Tipo == Tipo.boolean && S.Tipo == Tipo.TIPO_OK)
                    {
                        B.Tipo = Tipo.TIPO_OK;
                        B.TipoRet = S.TipoRet;
                    }
                    else
                    {
                        B.Tipo = Tipo.TIPO_ERROR; // Entre los parentesis del if se espera una expresion lógica.
                        if (E.Tipo == Tipo.boolean)
                        {
                            gestorDeErrores.ErrSemantico(2," Entre en el cuerpo del if. Probablemente veas un error anterior a este con más informacion.",@if.NumLineaCodigo);
                        }
                        else {gestorDeErrores.ErrSemantico(2," Entre los parentesis del if se espera una expresion lógica. No un: " + E.Tipo,E.NumLineaCodigo);}
                    }
                    pilaSemantico.Push(B);
                    // S 
                    // no se si hay que hacer algo con el S este.
                    
                    
                    break;
                case 7: // TODO: revisar
                    /*7 B -> do AbreCorchetes C CierraCorchetes while AbreParent E CierraParent{
                    B.tipo=if(E.tipo==boolean) then C.tipo
                    else Tipo_error "Entre los parentesis del while se espera una expresion logica "}
                    PuntoComa// Un do while*/
                    // antecedente
                    B = pilaSemantico.Pop();
                    // consecuente
                    PuntoComa = pilaSemantico.Pop();
                    CierraParent = pilaSemantico.Pop();
                    E = pilaSemantico.Pop();
                    AbreParent = pilaSemantico.Pop();
                    @while = pilaSemantico.Pop();
                    CierraCorchetes = pilaSemantico.Pop();
                    C = pilaSemantico.Pop();
                    AbreCorchetes = pilaSemantico.Pop();
                    @do = pilaSemantico.Pop();
                    if (E.Tipo == Tipo.boolean)
                    {
                        B.Tipo = C.Tipo;
                        B.TipoRet = C.TipoRet;
                    }
                    else
                    {
                        B.Tipo = Tipo.TIPO_ERROR; 
                        gestorDeErrores.ErrSemantico(2,"Entre los parentesis del while se espera una expresion logica ",E.NumLineaCodigo);
                    }
                    pilaSemantico.Push(B);
                    
                    break;
                case 8: // TODO: revisar
                    // 8 B -> S 
                    // antecedente
                    B = pilaSemantico.Pop();
                    // consecuente
                    S = pilaSemantico.Pop();
                    B.Tipo = S.Tipo;
                    B.TipoRet = S.TipoRet;
                    pilaSemantico.Push(B);
                    break;
                case 9:  //T-> int {T.tipo = int,T.ancho=2}
 
                    // Antecedente
                    T = pilaSemantico.Pop();
                    // Consecuente
                    @int = pilaSemantico.Pop();
                    T.Tipo = Tipo.@int;
                    T.Ancho = size_int;
                    pilaSemantico.Push(T);
                    break;
                case 10: // TODO: revisar
                         // T -> boolean{T.tipo=boolean,T.ancho=1}
                         //antecedente
                         T = pilaSemantico.Pop();
                         // consecuente
                         boolean = pilaSemantico.Pop();
                         T.Tipo = Tipo.boolean;
                         T.Ancho = size_bool;
                         pilaSemantico.Push(T);
                         
                    break;
                case 11:   //11 T -> string {T.tipo=string,T.ancho=2} // TODO: revisar
                    T = pilaSemantico.Pop();
                    @string =pilaSemantico.Pop();
                    T.Tipo =Tipo.@string; T.Ancho= size_string;
                    pilaSemantico.Push(T);
                    
                    break;
                case 12: // TODO: EL SIGUIENTE
                    /*  12 S -> ID IGUAL E PuntoComa{
                       S.tipo=if(E.tipo==BuscarTipoTS(ID.lexema))
                       }
                    */
                    // antecedente
                    S = pilaSemantico.Pop();
                    // consecuente
                    PuntoComa = pilaSemantico.Pop();
                    E = pilaSemantico.Pop();
                    IGUAL = pilaSemantico.Pop();
                    ID = pilaSemantico.Pop();
                    if (gesTS.buscarTS(ID.Lexema) == null)
                    {
                       
                        gestorDeErrores.WarningSemantico("No has declarado la variable \"" + ID.Lexema + "\". Por defecto se asume que es un int con valor 0.",ID.NumLineaCodigo);
                        // Guardar el estado anterior.
                        //---------------------------------------------------------------------------------------------------------  SI NO DECLARADA VARIABLE GLOBAL.
                        bool estadoAnteriorTS = gesTS.TablaLocalActiva;
                        bool estadoAnteriorDesplisLocal = despl.isLocal();
                        // pasar a global . Meter y recuperar estado anterior.
                        gesTS.TablaLocalActiva = false;
                        despl.useDesplG();
                        gesTS.insertarTS(ID.Lexema, despl.Despl, Tipo.@int.ToString());
                        despl.Despl += size_int;
                        despl.update();
                        
                        gesTS.TablaLocalActiva = estadoAnteriorTS;
                        if(estadoAnteriorDesplisLocal){ despl.useDesplL();}
                        //--------------------------------------------------------------------------------------------------------
                       
                        
                    }
                    if (  E.Tipo.ToString() == gesTS.buscaTipoTS(ID.Lexema))
                    {
                        S.Tipo = Tipo.TIPO_OK;
                        S.TipoRet = Tipo.vacio;
                    }
                    else
                    {
                        S.Tipo = Tipo.TIPO_ERROR;
                        gestorDeErrores.ErrSemantico(2,"En una asignacion los tipos han de ser iguales. Estas asignando  "+ gesTS.buscaTipoTS(ID.Lexema) +  " = " + E.Tipo,ID.NumLineaCodigo);
                    }
                    pilaSemantico.Push(S);
                    break;
                case 13:// TODO: 
                    /*13 S -> return X PuntoComa{S.tipo=if(TSL!=null) then TIPO_OK and S.tipoRet=X.tipo
                    else TIPO_ERROR S.TipoRet=tipo_Vacio}*/
                    //Console.WriteLine("AQUI SQUIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIi");
                    // antecedente
                    S = pilaSemantico.Pop();
                    // consecuente
                    PuntoComa = pilaSemantico.Pop();
                    X = pilaSemantico.Pop();
                    @return = pilaSemantico.Pop();
                    if (gesTS.TablaLocalActiva)
                    {
                        S.Tipo = Tipo.TIPO_OK;
                        S.TipoRet = X.Tipo;
                    }
                    else
                    {
                        S.Tipo = Tipo.TIPO_ERROR;
                        gestorDeErrores.ErrSemantico(2,"Solo se puede hacer return dentro de una funcion. ",@return.NumLineaCodigo);
                        S.TipoRet = Tipo.vacio;
                    }
                    pilaSemantico.Push(S);
                    break;
                case 14:// TODO:
                    //   14 S -> print AbreParent E CierraParent PuntoComa // No tengo claro si hace falta algo.
                    // antecedente
                    S = pilaSemantico.Pop();
                    // consecuente
                    PuntoComa = pilaSemantico.Pop();
                    CierraParent = pilaSemantico.Pop();
                    E = pilaSemantico.Pop();
                    AbreParent = pilaSemantico.Pop();
                    print = pilaSemantico.Pop();
                    S.Tipo =Tipo.TIPO_OK;
                    S.TipoRet = Tipo.vacio;
                    pilaSemantico.Push(S);
                    
                    break;
                case 15: // TODO:
                    /*15 S -> input AbreParent ID CierraParent {S.tipo = if(BuscarTipo_TS(ID.lexema)==cadena or digito) TIPO_OK S.TipoRet=tipo_Vacio
                    else TIPO_ERROR "input solo admite variables de tipo int o string."}PuntoComa*/
                    // antecedente
                    S = pilaSemantico.Pop();
                    // consecuente
                    PuntoComa = pilaSemantico.Pop();
                    CierraParent = pilaSemantico.Pop();
                    ID = pilaSemantico.Pop();
                    AbreParent = pilaSemantico.Pop();
                    input = pilaSemantico.Pop();
                    if (gesTS.buscarTS(ID.Lexema) == null)
                    {
                        //---------------------------------------------------------------------------------------------------------  SI NO DECLARADA VARIABLE GLOBAL.
                        bool estadoAnteriorTS = gesTS.TablaLocalActiva;
                        bool estadoAnteriorDesplisLocal = despl.isLocal();
                        // pasar a global . Meter y recuperar estado anterior.
                        gesTS.TablaLocalActiva = false;
                        despl.useDesplG();
                        gesTS.insertarTS(ID.Lexema, despl.Despl, Tipo.@int.ToString());
                        despl.Despl += size_int;
                        despl.update();
                        gesTS.TablaLocalActiva = estadoAnteriorTS;
                        if(estadoAnteriorDesplisLocal){ despl.useDesplL();}
                        //--------------------------------------------------------------------------------------------------------
                    }
                    if (
                        gesTS.buscaTipoTS(ID.Lexema) == Tipo.@string.ToString() ||
                        gesTS.buscaTipoTS(ID.Lexema) == Tipo.@int.ToString())
                    {
                        S.Tipo = Tipo.TIPO_OK;
                        S.TipoRet = Tipo.vacio;
                    }
                    else
                    {
                        S.Tipo = Tipo.TIPO_ERROR;
                        gestorDeErrores.ErrSemantico(2,"input solo admite variables de tipo int o string.",input.NumLineaCodigo);
                    }
                    pilaSemantico.Push(S);

                    break;
                case 16: // TODO:
                    /*16 S -> ID AbreParent L CierraParent{S.Tipo=if(L.Tipo!=TIPO_ERROR and BuscarTS(TS,ID.lexema)==L.listaVar->t)then TIPO_ERROR 
                    else TIPO_OK
                    S.TipoRet=tipo_Vacio
                } PuntoComa// llamada a una funcion el id debe estar en la TSG */
                    // antecedente
                    S = pilaSemantico.Pop();
                    // consecuente
                    PuntoComa = pilaSemantico.Pop();
                    CierraParent = pilaSemantico.Pop();
                    L = pilaSemantico.Pop();
                    AbreParent = pilaSemantico.Pop();
                    ID = pilaSemantico.Pop();
                    if (L.Tipo != Tipo.TIPO_ERROR && gesTS.buscarTSGlobal(ID.Lexema) != null &&
                        gesTS.buscaTipoParametrosTS(ID.Lexema).SequenceEqual(L.ListaVar))
                    {
                        S.Tipo = Tipo.TIPO_OK;
                       // S.Tipo =(Tipo)Enum.Parse(typeof(Tipo),gesTS.buscaTipoRetornoTS(ID.Lexema));
                        S.TipoRet = Tipo.vacio;
                    }
                    else
                    {
                        S.Tipo = Tipo.TIPO_ERROR;
                        gestorDeErrores.ErrSemantico(2,"LLamada a funcion incorrecta.",ID.NumLineaCodigo);
                    }
                    pilaSemantico.Push(S);
                    break;
                case 17: // TODO: Revisar
                    // 17 X -> E{X.tipo=E.tipo}
                    // antecedente
                    X = pilaSemantico.Pop();
                    // consecuente
                    E = pilaSemantico.Pop();
                    X.Tipo = E.Tipo;
                    pilaSemantico.Push(X);
                    break;
                case 18: // TODO:
                    // 18 X -> lambda{X.tipo=Tipo_Vacio}
                    // antecedente
                    X = pilaSemantico.Pop();
                    X.Tipo = Tipo.vacio;
                    pilaSemantico.Push(X);
                    break;
                case 19: // TODO:
                    /*19 L -> E Q {L.tipo=if(Q.tipo=tipo_error or E.tipo=TIPO_ERROR then TIPO_ERROR
                    else TIPO_OK)
                    L.ListaVar=E.tipo x Q.ListaVar}*/
                    // antecedente
                    L = pilaSemantico.Pop();
                    // consecuente 
                    Q = pilaSemantico.Pop();
                    E = pilaSemantico.Pop();
                    if (Q.Tipo == Tipo.TIPO_ERROR || E.Tipo == Tipo.TIPO_ERROR)
                    {
                        L.Tipo = Tipo.TIPO_ERROR;
                        gestorDeErrores.ErrSemantico(2,"Error en los tipos de los argumentos.",E.NumLineaCodigo);
                    }
                    else
                    {
                        L.Tipo = Tipo.TIPO_OK;
                    }
                    L.ListaVar = new List<Tipo>();
                    L.ListaVar.Add(E.Tipo);
                    L.ListaVar.AddRange(Q.ListaVar);
                    pilaSemantico.Push(L);
                    
                    
                    break;
                case 20: // TODO:
                    // 20 L -> lambda {L.tipo=Tipo_OK, L.ListaVar=""// o empty}
                    L = pilaSemantico.Pop();
                    L.Tipo = Tipo.TIPO_OK;
                    L.ListaVar = new List<Tipo>();
                    pilaSemantico.Push(L);
                    break;
                case 21: // TODO:
                    /*21 Q -> COMA E Q{Q.tipo=if(Q1.tipo=tipo_error or E.tipo=TIPO_ERROR) then TIPO_ERROR
                    else TIPO_OK
                    Q.ListaVar=E.tipoxQ1.ListaVar}*/
                    // antecedente
                    Q = pilaSemantico.Pop();
                    // consecuente 
                    Atributo Q1 = pilaSemantico.Pop();
                    E = pilaSemantico.Pop();
                    COMA = pilaSemantico.Pop();
                    if (Q1.Tipo == Tipo.TIPO_ERROR || E.Tipo == Tipo.TIPO_ERROR)
                    {
                        gestorDeErrores.ErrSemantico(2, "Error en los argumentos para la llamada a la función.",COMA.NumLineaCodigo);
                        Q.Tipo = Tipo.TIPO_ERROR;
                    }
                    else
                    {
                        Q.Tipo = Tipo.TIPO_OK;
                        
                    }
                    Q.ListaVar = new List<Tipo>();
                    Q.ListaVar.Add(E.Tipo);
                    Q.ListaVar.AddRange(Q1.ListaVar);
                    pilaSemantico.Push(Q);
                    break;
                case 22: // TODO:
                    // 22 Q -> lambda{Q.tipo=Tipo_OK,Q.ListaVar=""}
                    Q = pilaSemantico.Pop();
                    Q.Tipo = Tipo.TIPO_OK;
                    Q.ListaVar = new List<Tipo>();
                    pilaSemantico.Push(Q);
                    break;
                case 23: // TODO:
                    /*23
                     F -> function H ID {if(BuscarTS(ID.lexema)!=null) Then TIPO_ERROR "Variable ya declarada"
                    else InsertaTS(ID.lexema)
                    TSL=CreaTS()
                    TablaLocalActiva=true;
                    DesplL:=0} 
                    AbreParent A CierraParent{insertaTipoTSG(ID.lexema,A.listaVar->H.tipo)
                    InsertaEtiqTS(ID.lexema,nueva_et())} AbreCorchetes C CierraCorchetes
                {if(C.TipoRet!=H.tipo) THen TIPO_ERROR "la funcion esperaba devolver otro tipo de varametro."
                    else Tipo_ok
                    DestruyeTS(TSL)}
                    */
                    // antecedente
                    F = pilaSemantico.Pop();
                    // consecuente F->function H ID AbreParent A CierraParent AbreCorchetes C CierraCorchetes
                    CierraCorchetes = pilaSemantico.Pop();
                    C = pilaSemantico.Pop();
                    AbreCorchetes = pilaSemantico.Pop();
                    CierraParent = pilaSemantico.Pop();
                    A = pilaSemantico.Pop();
                    AbreParent= pilaSemantico.Pop();
                    ID = pilaSemantico.Pop();
                    H = pilaSemantico.Pop();
                    function = pilaSemantico.Pop();
                    if (errorDeclFuncion)
                    {
                        F.Tipo = Tipo.TIPO_ERROR;
                    }
                    else
                    {
                        if (C.TipoRet != H.Tipo)
                        {
                            F.Tipo = Tipo.TIPO_ERROR;
                            gestorDeErrores.ErrSemantico(2,"La funcion esperaba devolver un: " +H.Tipo +" No un: "+ C.TipoRet,function.NumLineaCodigo);
                        }
                        gesTS.imprimirTS(ID.Lexema);
                        gesTS.destruirTS();
                        despl.Despl = 0;
                        despl.update();
                        despl.useDesplG();
                        F.Tipo = Tipo.TIPO_OK;
                        //gesTS.crearTS(false);
                        //despl.useDesplL();
                        //despl.Despl = 0;
                    }
                    pilaSemantico.Push(F);

                    // gesTS.crearTS(false);
                    break;
                case 24: // TODO: Revisar
                    // 24 H -> T {H.tipo=T.tipo }
                    // antecedente
                    H = pilaSemantico.Pop();
                    // consecuente
                    T = pilaSemantico.Pop();
                    H.Tipo = T.Tipo;
                    pilaSemantico.Push(H);
                    break;
                case 25: // TODO: Revisar
                    // 25 H -> lambda {H.tipo= Tipo_vacio}
                    // antecedente
                    H = pilaSemantico.Pop();
                    H.Tipo = Tipo.vacio;
                    pilaSemantico.Push(H);
                    break;
                case 26: // TODO: Revisar
                    /*26 C -> B C {C.tipo=if(B.tipo=Tipo_OK) then C.tipo
                    else TIPO_ERROR
                    C.tipoRet=C.tipoREt}????
                    */
                    // antecedente
                    C = pilaSemantico.Pop();
                    // consecuente
                    Atributo C1 = pilaSemantico.Pop();
                    B = pilaSemantico.Pop();
                    if (B.Tipo == Tipo.TIPO_OK)
                    {
                        C.Tipo = Tipo.TIPO_OK;
                    }
                    else
                    {
                        C.Tipo = Tipo.TIPO_ERROR;
                        
                    }
                    if (B.TipoRet == C1.TipoRet)
                    {
                        C.TipoRet = C1.TipoRet;
                    }else if (B.TipoRet == Tipo.vacio)
                    {
                        C.TipoRet = C1.TipoRet;
                    }else if (C1.TipoRet == Tipo.vacio)
                    {
                        C.TipoRet = B.TipoRet;
                    }
                    else
                    {
                        C.TipoRet = Tipo.TIPO_ERROR;
                        gestorDeErrores.ErrSemantico(2,"Tipos devueltos erroneos.",B.NumLineaCodigo);
                    }
                    pilaSemantico.Push(C);
                    
                   
                    
                    break;
                case 27: // TODO:
                    // 27 C -> lambda {C.tipo=TIPO_OK, C.tipoRet=Tipo_Vacio}
                    // antecedente
                    C = pilaSemantico.Pop();
                    C.Tipo = Tipo.TIPO_OK;
                    C.TipoRet = Tipo.vacio;
                    pilaSemantico.Push(C);
                    
     
                    break;
                case 28: // TODO:
                    /*28 A -> T ID K{A.tipo=if(K.tipo=TIPO_ERROR or buscaTipoTSL(ID.lexema)!= null) then TIPO_ERROR "Argumento incorrecto"
                    else insertaTS(ID.lexema,T.tipo,desplL)
                    desplL=desplL+T.ancho
                    A.listaVar=T.tipoxK.listaVar}*/
                    // antecedente
                    A = pilaSemantico.Pop();
                    // consecuente
                    K = pilaSemantico.Pop();
                    ID = pilaSemantico.Pop();
                    T = pilaSemantico.Pop();
                    if (K.Tipo == Tipo.TIPO_ERROR || gesTS.buscarTSLocal(ID.Lexema)!=null)
                    {
                        A.Tipo = Tipo.TIPO_ERROR;
                        gestorDeErrores.ErrSemantico(2,"Argumento incorrecto",ID.NumLineaCodigo);
                    }
                    else
                    {
                        
                        gesTS.insertarTS(ID.Lexema, despl.Despl, T.Tipo.ToString());
                        despl.Despl += T.Ancho;
                        despl.update();
                        A.ListaVar = new List<Tipo>();
                        A.ListaVar.Add(T.Tipo);
                        A.ListaVar.AddRange(K.ListaVar);
                        gesTS.TablaLocalActiva = false;
                        // Num de parametros = 0
                        gesTS.insertarNumParametrosTS(id_funcion.Lexema, A.ListaVar.Count);
                        // string[] listaTipoVariables = A.ListaVar.Select(x => x.ToString()).ToArray();//  
                        gesTS.insertarTipoParametrosTS(id_funcion.Lexema, A.ListaVar);
                        gesTS.TablaLocalActiva = true;
                    }
                    pilaSemantico.Push(A);
                    break;
                case 29: // TODO:
                    //  29 A -> lambda    {A.tipo=TIPO_OK,A.listaVar=""}
                    // antecedente
                    A = pilaSemantico.Pop();
                    A.Tipo = Tipo.TIPO_OK;
                    //A.ListaVar = "";
                    A.ListaVar = new List<Tipo>();
                   
                    gesTS.TablaLocalActiva = false;
                    // Num de parametros = 0
                    gesTS.insertarNumParametrosTS(id_funcion.Lexema, 0);
                    gesTS.insertarTipoParametrosTS(id_funcion.Lexema, A.ListaVar);
                    
                    gesTS.TablaLocalActiva = true;
                    pilaSemantico.Push(A);
                    break;
                case 30: // TODO:
                    /*30 K -> COMA T ID K {K.tipo= if(K.tipo=TIPO_ERROR or BuscarTS(ID.lexema)!=null) then TIPO_ERROR "Argumento incorrecto"
                      else TIPO_OK
                      K.listaVar=T.tipo x K1.listaVar}
                    */
                    K = pilaSemantico.Pop();
                    // consecuente
                    Atributo K1 = pilaSemantico.Pop();
                    ID = pilaSemantico.Pop();
                    T = pilaSemantico.Pop();
                    COMA = pilaSemantico.Pop();
                    if (K1.Tipo == Tipo.TIPO_ERROR || gesTS.buscarTS(ID.Lexema) != null)
                    {
                        K.Tipo = Tipo.TIPO_ERROR;
                        gestorDeErrores.ErrSemantico(2, "Argumento Incorrecto",COMA.NumLineaCodigo);
                    }
                    else
                    {
                        K.Tipo = Tipo.TIPO_OK;
                        // Declarar la variable
                        gesTS.insertarTS(ID.Lexema, despl.Despl, T.Tipo.ToString());
                        despl.Despl += T.Ancho;
                        despl.update();
                        
                    }
                    K.ListaVar = new List<Tipo>();
                    K.ListaVar.Add(T.Tipo);
                    K.ListaVar.AddRange(K1.ListaVar);
                    pilaSemantico.Push(K);
                    break;
                case 31: // TODO: Revisar
                    // 31 K -> lambda{K.tipo=TIPO_OK,K.listaVar=""}
                    K = pilaSemantico.Pop();
                    K.Tipo = Tipo.TIPO_OK;
                   // K.ListaVar ya esta vacía por defecto.
                    K.ListaVar = new List<Tipo>();
                    pilaSemantico.Push(K);
                    break;
                case 32: // TODO:
                    /*32 E -> E AND R{E.tipo=if(E1.tipo==boolean and R.tipo==boolean then boolean 
                    else TIPO_ERROR}*/
                    E = pilaSemantico.Pop();
                    // consecuente
                    R = pilaSemantico.Pop();
                    AND = pilaSemantico.Pop();
                    Atributo E1 = pilaSemantico.Pop();
                    if (E1.Tipo == Tipo.boolean && R.Tipo == Tipo.boolean)
                    {
                        E.Tipo = Tipo.boolean;
                    }
                    else
                    {
                        E.Tipo = Tipo.TIPO_ERROR;
                        gestorDeErrores.ErrSemantico(2,"El operador relacional AND se esperaba tipo boolean AND tipo boolean. NO: "+ E1.Tipo + " AND "+ R.Tipo,E1.NumLineaCodigo);
                    }
                    pilaSemantico.Push(E);
                    break;
                case 33: // TODO:
                    // 33 E -> R  {E.tipo=R.tipo}
                    E = pilaSemantico.Pop();
                    // consecuente
                    R = pilaSemantico.Pop();
                    E.Tipo = R.Tipo;
                    pilaSemantico.Push(E);
                    break;
                case 34: // TODO:
                    /*34  R -> R IGUALIGUAL U {R.tipo=if(R1.tipo==U.tipo) then R1.tipo
                         else TIPO_ERROR}
                    */
                    // antecedente
                    R = pilaSemantico.Pop();
                    // consecuente
                    U = pilaSemantico.Pop();
                    IGUALIGUAL = pilaSemantico.Pop();
                    Atributo R1 = pilaSemantico.Pop();
                    if (R1.Tipo == U.Tipo && R1.Tipo == Tipo.@int)
                    {
                        R.Tipo = Tipo.boolean;
                    }
                    else
                    {
                        R.Tipo = Tipo.TIPO_ERROR;
                        gestorDeErrores.ErrSemantico(2,"No se puede comparar un: "+ R1.Tipo + " con un: " + U.Tipo + ". Solo se pueden comparar enteros.",R1.NumLineaCodigo);
                    }
                    pilaSemantico.Push(R);
                    break;
                case 35: // TODO: Revisar
                    // 35 R -> U {R.tipo=U.tipo}
                    R = pilaSemantico.Pop();
                    // consecuente
                    U = pilaSemantico.Pop();
                    R.Tipo = U.Tipo;
                    pilaSemantico.Push(R);
                    break;
                case 36: // TODO:  Revisar
                    /*36 U -> U Suma V{U.tipo=if(U1.tipo==int and V.tipo==int) then int 
                          else TIPO_ERROR}
                    */
                    // antecedente
                    U = pilaSemantico.Pop();
                    // consecuente
                    V = pilaSemantico.Pop();
                    Suma = pilaSemantico.Pop();
                    Atributo U1 = pilaSemantico.Pop();
                    
                   
                    if (U1.Tipo == Tipo.@int && V.Tipo == Tipo.@int)
                    {
                        U.Tipo = Tipo.@int;
                    }
                    else
                    {
                        U.Tipo = Tipo.TIPO_ERROR;
                        gestorDeErrores.ErrSemantico(2,"Solo se pueden sumar dos numeros. No puedes sumar: "+U1.Tipo.ToString() +" + " + V.Tipo.ToString(),U.NumLineaCodigo);
                    }
                    pilaSemantico.Push(U);
                    break;
                case 37: // TODO: Revisar
                    // 37 U -> V  {U.tipo=V.tipo}
                    // antecedente
                    U = pilaSemantico.Pop();
                    // consecuente
                    V = pilaSemantico.Pop();
                    U.Tipo = V.Tipo;
                    pilaSemantico.Push(U);
                    break;
                case 38: // TODO: Revisar
                   // 38  V -> ID {V.tipo=BuscaTipoTS(ID.lexema)}
                   // antecedente
                   V = pilaSemantico.Pop();
                   // consecuente
                   ID = pilaSemantico.Pop();
                   if (gesTS.buscarTS(ID.Lexema)==null)
                   {
                       //gestorDeErrores.ErrSemantico(2,"No has declarado la variable: " + ID.Lexema);
                       gestorDeErrores.WarningSemantico("No has declarado la variable \"" + ID.Lexema + "\". Por defecto se asume que es un int con valor 0.",ID.NumLineaCodigo);
                       // Guardar el estado anterior.
                       //---------------------------------------------------------------------------------------------------------  SI NO DECLARADA VARIABLE GLOBAL.
                       bool estadoAnteriorTS = gesTS.TablaLocalActiva;
                       bool estadoAnteriorDesplisLocal = despl.isLocal();
                       // pasar a global . Meter y recuperar estado anterior.
                       gesTS.TablaLocalActiva = false;
                       despl.useDesplG();
                       gesTS.insertarTS(ID.Lexema, despl.Despl, Tipo.@int.ToString());
                       despl.Despl += size_int;
                       despl.update();
                        
                       gesTS.TablaLocalActiva = estadoAnteriorTS;
                       if(estadoAnteriorDesplisLocal){ despl.useDesplL();}
                       //--------------------------------------------------------------------------------------------------------
                       V.Tipo = (Tipo)Enum.Parse(typeof(Tipo),gesTS.buscaTipoTS(ID.Lexema)) ;
                   }
                   else
                   {
                       V.Tipo = (Tipo)Enum.Parse(typeof(Tipo),gesTS.buscaTipoTS(ID.Lexema)) ;
                   }
                  
                   pilaSemantico.Push(V);
                   break;
                case 39: // TODO: Revisar
                    // 39 V -> digito {V.tipo=int}
                    // antecedente
                    V = pilaSemantico.Pop();
                    // consecuente
                    digito = pilaSemantico.Pop();
                    V.Tipo = Tipo.@int;
                    pilaSemantico.Push(V);
                    break;
                case 40: // TODO: Revisar
                    // 40 V -> true {V.tipo=boolean}
                    // antecedente
                    V = pilaSemantico.Pop();
                    // consecuente
                    @true = pilaSemantico.Pop();
                    V.Tipo = Tipo.boolean;
                    pilaSemantico.Push(V);
                    break;
                case 41: // TODO: Revisar
                    // 41 V -> false {V.tipo=boolean}
                    // antecedente
                    V = pilaSemantico.Pop();
                    // consecuente
                    @false = pilaSemantico.Pop();
                    V.Tipo = Tipo.boolean;
                    pilaSemantico.Push(V);
                    break;
                case 42: // TODO:
                    // 42 V -> cadena {V.tipo=string}
                    // antecedente
                    V = pilaSemantico.Pop();
                    // consecuente
                    cadena = pilaSemantico.Pop();
                    V.Tipo = Tipo.@string;
                    pilaSemantico.Push(V);
                    break;
                case 43: // TODO:                                                                                                                    REVISAR importante
                /* 43 V -> ID AbreParent L CierraParent{V.Tipo=if(L.Tipo!=TIPO_ERROR and BuscarTS(TS,ID.lexema)==L.listaVar->t)then TIPO_ERROR 
                    else TIPO_OK
                    V.TipoRet=buscarTipoRetornoTS(ID.lexema);
                } PuntoComa// llamada a una funcion el id debe estar en la TSG */
                    // antecedente
                    V = pilaSemantico.Pop();
                    // consecuente
                    CierraParent = pilaSemantico.Pop();
                    L = pilaSemantico.Pop();
                    AbreParent = pilaSemantico.Pop();
                    ID = pilaSemantico.Pop();
                    if (L.Tipo != Tipo.TIPO_ERROR && gesTS.buscarTSGlobal(ID.Lexema) != null &&
                    gesTS.buscaTipoParametrosTS(ID.Lexema).SequenceEqual(L.ListaVar))
                    {
                        //V.Tipo = Tipo.TIPO_OK;
                        V.Tipo =(Tipo)Enum.Parse(typeof(Tipo),gesTS.buscaTipoRetornoTS(ID.Lexema));
                    }
                    else
                    {
                        V.Tipo = Tipo.TIPO_ERROR;
                        gestorDeErrores.ErrSemantico(2,"LLamada a funcion incorrecta.",ID.NumLineaCodigo);
                    }
                    pilaSemantico.Push(V);
                
                    /* */
                    break;
                case 44: // TODO:
                    // 44 V -> AbreParent E CierraParent{V.tipo=E.tipo}
                    // antecedente
                    V = pilaSemantico.Pop();
                    // consecuente
                    CierraParent = pilaSemantico.Pop();
                    E = pilaSemantico.Pop();
                    AbreParent = pilaSemantico.Pop();
                    V.Tipo = E.Tipo;
                    pilaSemantico.Push(V);
                    break;
                case 45: // TODO:
                    /*45 V -> ID MASMAS V.tipo=if (BuscarTipoTS(ID.lexema)==int) then int
                       else TIPO_ERROR "Se esperaba una variable de tipo int."
                    */
                    // antecedente
                    V = pilaSemantico.Pop();
                    // consecuente
                    MASMAS = pilaSemantico.Pop();
                    ID = pilaSemantico.Pop();
                    if (gesTS.buscaTipoTS(ID.Lexema) == Tipo.@int.ToString())
                    {
                        V.Tipo = Tipo.@int;
                    }
                    else
                    {
                        V.Tipo = Tipo.TIPO_ERROR;
                        gestorDeErrores.ErrSemantico(2,"Se esperaba una variable de tipo int.",ID.NumLineaCodigo);
                    }
                    pilaSemantico.Push(V);
                    break;
                
                    
                default:
                    //llamar a gestor de errores
                    
                    break;

            }

            return  "";
        }

        public void declararFuncion(List<Atributo> detectarFuncionUltimosTresElementos)
        {
            Atributo ID = detectarFuncionUltimosTresElementos[0];
            Atributo H = detectarFuncionUltimosTresElementos[1];
            Atributo function = detectarFuncionUltimosTresElementos[2];
            if (gesTS.buscarTS(ID.Lexema) != null)
            {
                errorDeclFuncion = true;
                gestorDeErrores.ErrSemantico(1,"Funcion ya declarada.  " + ID.Lexema,ID.NumLineaCodigo);
            }
            else
            {
                gesTS.insertarTS(ID.Lexema, 0,Tipo.funcion.ToString()); // declaramos la funcion. // tipo, tipo retorno. H.tipo, num de parametros.
                gesTS.insertarTipoRetornoTS(ID.Lexema,H.Tipo.ToString());
                gesTS.crearTS(false); // crear Tabla de simbolos Local
                despl.useDesplL();
                despl.Despl =0;
                despl.update();
            }

            id_funcion = ID;
        }
        
        
        
        /*
        1 PAxioma -> {Crear TSG, DesplG=0}P{imprimirTS,DestruirTSG}
        2 P -> B P  // Nada
        3 P -> F P  //Nada
        4 P -> lambda    // nada 
        5 B -> var T ID PuntoComa {
            if(TSL==Null) then TS<-TSG; Despl<-DesplG
            else TS<-TSL  Despl<-DesplLocal
            
            if(buscaTS(ID.lexema!=null)) then ERROR  Variable ya declarada
            else id.pos=InsertaTS(TS,id.lexema,T.tipo,Despl)
            despl+=T.ancho}//Declaracion de variable
            
        6 B -> if AbreParent E CierraParent{
            B.tipo=if(E.tipo==boolean) then Ok_tipo
                   else Tipo_error (Entre los parentesis del if se espera una expresion logica)
         } S// comprobar que E es un booleano.
            
        7 B -> do AbreCorchetes C CierraCorchetes while AbreParent E CierraParent{
        B.tipo=if(E.tipo==boolean) then C.tipo
                else Tipo_error "Entre los parentesis del while se espera una expresion logica "}
                PuntoComa// Un do while
    8 B -> S // Sentencias sencillas asignacion,return,print,input,if simple
    9 T -> int {T.tipo = int,T.ancho=2}
    10 T -> boolean{T.tipo=boolean,T.ancho=1}
    11 T -> string {T.tipo=string,T.ancho=2}
    12 S -> ID IGUAL E PuntoComa{
        S.tipo=if(E.tipo==BuscarTipoTS(ID.lexema))
        }
    13 S -> return X PuntoComa{S.tipo=if(TSL!=null) then TIPO_OK and S.tipoRet=X.tipo
                                      else TIPO_ERROR S.TipoRet=tipo_Vacio}
    14 S -> print AbreParent E CierraParent PuntoComa // No tengo claro si hace falta algo.
    15 S -> input AbreParent ID CierraParent {S.tipo = if(BuscarTipo_TS(ID.lexema)==cadena or digito) TIPO_OK S.TipoRet=tipo_Vacio
                                                else TIPO_ERROR "input solo admite variables de tipo int o string."}PuntoComa
    16 S -> ID AbreParent L CierraParent{S.Tipo=if(L.Tipo!=TIPO_ERROR and BuscarTS(TS,ID.lexema)==L.listaVar->t)then TIPO_ERROR 
                                                else TIPO_OK
                                                S.TipoRet=tipo_Vacio
                                                 } PuntoComa// llamada a una funcion el id debe estar en la TSG 
    17 X -> E{X.tipo=E.tipo}
    18 X -> lambda{X.tipo=Tipo_Vacio}
    19 L -> E Q {L.tipo=if(Q.tipo=tipo_error or E.tipo=TIPO_ERROR then TIPO_ERROR
                        else TIPO_OK)
                  L.ListaVar=E.tipo x Q.ListaVar}
    20 L -> lambda {L.tipo=Tipo_OK, L.ListaVar=""// o empty}
    21 Q -> COMA E Q{Q.tipo=if(Q1.tipo=tipo_error or E.tipo=TIPO_ERROR) then TIPO_ERROR
                            else TIPO_OK
                            Q.ListaVar=E.tipoxQ1.ListaVar}
    22 Q -> lambda{Q.tipo=Tipo_OK,Q.ListaVar=""}
    



    23 F -> function H ID {if(BuscarTS(ID.lexema)!=null) Then TIPO_ERROR "Variable ya declarada"
                          else InsertaTS(ID.lexema)
                          TSL=CreaTS()
                          TablaLocalActiva=true;
                          DesplL:=0} 
        AbreParent A CierraParent{insertaTipoTSG(ID.lexema,A.listaVar->H.tipo)
                                  InsertaEtiqTS(ID.lexema,nueva_et())} AbreCorchetes C CierraCorchetes
                                  {if(C.TipoRet!=H.tipo) THen TIPO_ERROR "la funcion esperaba devolver otro tipo de varametro."
                                  else Tipo_ok
                                  DestruyeTS(TSL)}
                                  
                            
    24 H -> T {H.tipo=T.tipo }
    25 H -> lambda {H.tipo= Tipo_vacio}
    26 C -> B C {C.tipo=if(B.tipo=Tipo_OK) then C.tipo
                         else TIPO_ERROR
                 C.tipoRet=C.tipoREt}????
    27 C -> lambda {C.tipo=TIPO_OK, C.tipoRet=Tipo_Vacio}
    28 A -> T ID K{A.tipo=if(K.tipo=TIPO_ERROR or buscaTipoTSL(ID.lexema)!= null) then TIPO_ERROR "Argumento incorrecto"
                          else insertaTS(ID.lexema,T.tipo,desplL)
                          desplL=desplL+T.ancho
                          A.listaVar=T.tipoxK.listaVar}
    29 A -> lambda    {A.tipo=TIPO_OK,A.listaVar=""}
    30 K -> COMA T ID K {K.tipo= if(K.tipo=TIPO_ERROR or BuscarTS(ID.lexema)!=null) then TIPO_ERROR "Argumento incorrecto"
                                 else TIPO_OK
                                 K.listaVar=T.tipo x K1.listaVar}
    31 K -> lambda{K.tipo=TIPO_OK,K.listaVar=""}
    32 E -> E AND R{E.tipo=if(E1.tipo==boolean and R.tipo==boolean then boolean 
                           else TIPO_ERROR}
    33 E -> R  {E.tipo=R.tipo}
    34 R -> R IGUALIGUAL U {R.tipo=if(R1.tipo==U.tipo) then R1.tipo
                                   else TIPO_ERROR}
    35 R -> U {R.tipo=U.tipo}
    36 U -> U Suma V{U.tipo=if(U1.tipo==int and V.tipo==int) then int 
                             else TIPO_ERROR}
    37 U -> V  {U.tipo=V.tipo}
    38 V -> ID {V.tipo=BuscaTipoTS(ID.lexema)}
    39 V -> digito {V.tipo=int}
    40 V -> true {V.tipo=boolean}
    41 V -> false {V.tipo=boolean}
    42 V -> cadena {V.tipo=string}
    43 V -> ID AbreParent L CierraParent {if(buscarTS(ID.lexema)==null){then ERROR}
    else if{buscarTipoParametros(ID.lexema) == A.TipoParametros) V.tipo = Tipo devuelto}}else error.
    44 V -> AbreParent E CierraParent{V.tipo=E.tipo}
    45 V -> ID MASMAS V.tipo=if (BuscarTipoTS(ID.lexema)==int) then int
                             else TIPO_ERROR "Se esperaba una variable de tipo int."
       
       Nota evitamos atrivutos sintetizados.
       Trabajamos con:
        Constantes
        Operadores
        Identificadores
        Declaraciones
        Tipos de Datos
        Instrucciones de entrada/salida
        Sentencias
        Funciones
        
        Esto es: 
        Expresiones
        Declaraciones
        Sentencias
    */

        public void regla_0()
        {
            
        }

    }

    //Por facilidad de uso hemos tratado de utilizar solo atributos sintetizados
    /*El tipo puede ser TIPO_OK,TIPO_ERROR, string, int,boolean, Tipo_vacio
     * 
     */
    
    /*Para un analizador sintactico ascendente nos interesa usar una gramática S-atribuida es decir un gramatica L-atribuida
     (Viene por la izquierda) que además solo utiliza atributos sintetizados(Atributos de hijos a padres) esto
     es así porque va acorde con el sentido ascendente de nuestro analisis sintactico..*/

    class Desplazamiento
    {
        private int desplL;
        private int desplG;
        private int despl;
        private bool usarDesplL;
        public Desplazamiento()
        {
            desplG = 0;
            desplL = 0;
            usarDesplL = false;
        }

        public void useDesplL()
        {
            usarDesplL = true;
            despl = desplL;
        }
        
        public void useDesplG()
        {
            usarDesplL = false;
            despl = desplG;
        }

        public bool isLocal()
        {
            return usarDesplL;
        }

        /*update:
         DESCRIPTION: Update values and gives to desplL or Global.*/
        public void update()
        {
            if (usarDesplL)
                desplL = despl;
            else
            {
                desplG = despl;
            } 
            // usarDesplL = false;  // probably this should not be here.
        }

        public int Despl
        {
            get => despl;
            set => despl = value;
        }
    }
}