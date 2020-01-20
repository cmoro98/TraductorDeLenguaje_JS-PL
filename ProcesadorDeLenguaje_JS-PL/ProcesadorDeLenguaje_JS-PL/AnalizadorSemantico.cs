namespace ProcesadorDeLenguaje_JS_PL
{
    public class AnalizadorSemantico
    {
        private bool tsLocalActiva = false;
        
        
        
        
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
    43 V -> ID AbreParent L CierraParent
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

    }
}