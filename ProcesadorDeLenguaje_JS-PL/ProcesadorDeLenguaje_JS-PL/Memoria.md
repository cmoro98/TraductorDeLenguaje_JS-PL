# Memoria Procesadores de Lenguajes.



​	**Grupo**: 159
​	**Nombre**: Carlos 
​	**Apellidos**: Moro Jiménez
​	**Matrícula**: y160055 
​	**DNI**: 53939068X



























































































## Índice:

[TOC]

### Introducción:

Esta es una memoria sobre la creación de un traductor del lenguaje Javascript-PDL  para la asignatura de Procesadores de lenguajes. Este proyecto ha sido realizado en C#.

## Analizador Léxico .

### Tokens:


	             <MAS,>
	             <MASMAS,>
	             <AND,>
	             <IGUAL,>
	             <IGUALIGUAL,>
	             <CierraParent,>
	             <AbreParent,>
		         <AbreCorchetes>
		         <CierraCorchetes>	
	             <cadena,lex>
	             <digito,valor>
	             <ID,posTS> 
	             <Coma,>
	             <PuntoComa>
	            --------Palabras Reservadas-----------	
		         <if,>
	             <function,>
	             <int,>
	             <boolean,>
	             <string,>
	             <true,>
	             <false,>
	             <input,>
	             <print,>
	             <var,>
	             <do,>
		         <while,>
	             <return,>

### Gramática:


    S-> &A |=B |( |) || ,|; |{|}|lC|dD|"E|+F|/G|del S
    A-> &
    B-> = | LAMBDA
    C-> lC |dC |_C | LAMBDA
    D-> dD|LAMBDA
    E-> c1E |"
    F-> + | LAMBDA
    G-> /H
    H-> c2 H |saltolinea S


### Acciones semánticas:
**L:** lee
**C**: Concatena
**T**: valor = digito
**T’**:valor= valor*10+digito
**GT**: if(valor.is16bytes){ GenToken(ENT,valor) }
**GID**: 
   p= buscaPR(cadena);
   if (P!=null){GenToken(cadena,p)}   // Palabras reservadas
   p=buscaTS(cadena);
   if(p!=null){GenToken(ID,p) }               //Notese que p es la posTS
   else (p=insertaTS(cadena);
           GenToken(ID,p)
   }

 **GAND**: GenToken(AND,-)
 **GI**: GenToken(Igual,-)
**GII**: GenToken(IgualIgual,-)
**GM**: GenToken(MAS,-)
**GMM**: GenToken(MASMAS,-)

**G’** : GenToken(AbreParent,-)
**G’’**: GenToken(CierraParent,-)
**G’’’**: GenToken(Coma,-)
**G’’’’**: GenToken(PuntoComa,-)
**GC**: GenToken(AbreCorchetes,-)
**GC’**: GenToken(CierraCorchetes,-)
**GCA**: GenToken(Cadena,cadena)

**Matriz de transición**

Cuando se pasa a pdf no consigue pasarse completo. Se puede ver completo aqui: https://drive.google.com/file/d/122UzqtM8jQj_bEE4L1Bk5OvO-x-M86rK/view?usp=sharing

| Terminales→ | &      | &      | =      | =      | \(     | \(     | \)     | \)     |        |        | ;      | ;       | \{     | \{     | \}     | \}     | letra  | letra  | digito | digito | “      | “      | \+     | \+     | /      | /      | Caracter1 | Caracter1 | Caracter2 | Caracter2 | \_     | \_     | del    | del    | \\n    | \\n    | OC     | OC     |
|-------------|--------|--------|--------|--------|--------|--------|--------|--------|--------|--------|--------|---------|--------|--------|--------|--------|--------|--------|--------|--------|--------|--------|--------|--------|--------|--------|-----------|-----------|-----------|-----------|--------|--------|--------|--------|--------|--------|--------|--------|
| 0           | 1      | L      | 2      |        | 12     | G’ L   | 13     | G’’ L  | 14     | G’’’ L | 15     | G’’’’ L | 16     | GC L   | 17     | GC’ L  | 3      | C L    | 4      | T L    | 5      | L      | 6      | L      | 7      | L      |           |           |           |           |        |        |        |        |        |        |        |        |
| 1           | 9      | GAND L |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        |        |        |
| 2           |        |        | 11     | GII L  |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        | 10     | GI     |
| 3           |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        | 3      | C L    | 3      | C L    |        |        |        |        |        |        |           |           |           |           | 3      | C L    |        |        |        |        | 18     | GID    |
| 4           |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        | 4      | T’ L   |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        | 19     | GT     |
| 5           |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        | 20     | GCA    |        |        |        |        | 5         | C L       |           |           |        |        |        |        |        |        |        |        |
| 6           |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        | 21     | GMM L  |        |        |           |           |           |           |        |        |        |        |        |        | 22     | GM     |
| 7           |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        | 8      | L      |           |           |           |           |        |        |        |        |        |        |        |        |
| 8           |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           | 8         | L         |        |        |        |        | 0      | L      |        |        |
| 9           |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        |        |        |
| 10          |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        |        |        |
| 11          |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        |        |        |
| 12          |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        |        |        |
| 13          |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        |        |        |
| 14          |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        |        |        |
| 15          |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        |        |        |
| 16          |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        |        |        |
| 17          |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        |        |        |
| 18          |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        |        |        |
| 19          |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        |        |        |
| 20          |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        |        |        |
| 21          |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        |        |        |
| 22          |        |        |        |        |        |        |        |        |        |        |        |         |        |        |        |        |        |        |        |        |        |        |        |        |        |        |           |           |           |           |        |        |        |        |        |        |        |        |
| Estados     | estado | accion | estado | accion | estado | accion | estado | accion | estado | accion | estado | accion  | estado | accion | estado | accion | estado | accion | estado | accion | estado | accion | estado | accion | estado | accion | estado    | accion    | estado    | accion    | estado | accion | estado | accion | estado | accion | estado | accion |



### Automata finito:
L se aplica a todas las transiciones menos aquellas que son transiciones “otro carácter”. 
Con acciones semánticas

![AFinitoConAccSem](/home/krls/DriveBrujo/ING_Informatica/PDL/Imagenes_Memoria/AFinitoConAccSem.png)

Sin acciones semánticas:

![AF](/home/krls/DriveBrujo/ING_Informatica/PDL/Imagenes_Memoria/AF.png)







### Errores:

Cualquier acción que se salga de lo previsto por el autómata se considerará un error. Los errores estan gestionadas por el gestor de errores .
Ejemplo de error: NO existe el componente léxico & " + "pruebe con &&"  .
También detecta como error si el número es mayor a 2 bytes. He utilizado el tipo Int16 
para solventar directamente el problema o si una cadena excede los 64 caracteres.

------



##  Analizador sintáctico: 

El analizador que nos tocó es el ascendente, esto nos ha traido dificultades a la hora de 
la creación de las tablas acción y goto. Por suerte no hemos tenido demadiados 
problemas en la resolución de conflictos reducción-reducción o 
reducción-desplazamiento. 
Hemos implementado el analizador siguiendo la estructura presentada en clase. 
Este analizador recibe las tablas acción, goto, las reglas y realiza el análisis ascendente, 
de esta forma un cambio en la gramática sería muy fácil de solucionar.

### Gramática del sintáctico:

Como los nombres en la gramática coinciden con los códigos de los token la
implementación ha sido más fácil.

```
Terminales = { var ID if AbreParent CierraParent AbreCorchetes while CierraCorchetes int boolean string IGUAL PuntoComa return print input COMA do  function AND IGUALIGUAL Suma MASMAS digito cadena true false }
NoTerminales = { PAxioma P B F T E C S L Q X  H A K R U V }
Axioma = PAxioma
Producciones = {
   PAxioma -> P
   P -> B P
   P -> F P
   P -> lambda     
   B -> var T ID PuntoComa
   B -> if AbreParent E CierraParent S
   B -> do AbreCorchetes C CierraCorchetes while AbreParent E CierraParent PuntoComa
   B -> S
   T -> int
   T -> boolean
   T -> string
   S -> ID IGUAL E PuntoComa
   S -> return X PuntoComa
   S -> print AbreParent E CierraParent PuntoComa
   S -> input AbreParent ID CierraParent PuntoComa
   S -> ID AbreParent L CierraParent PuntoComa
   X -> E
   X -> lambda
   L -> E Q 
   L -> lambda
   Q -> COMA E Q
   Q -> lambda
   F -> function H ID AbreParent A CierraParent AbreCorchetes C CierraCorchetes
   H -> T
   H -> lambda
   C -> B C 
   C -> lambda
   A -> T ID K
   A -> lambda
   K -> COMA T ID K
   K -> lambda
   E -> E AND R
   E -> R
   R -> R IGUALIGUAL U 
   R -> U
   U -> U Suma V
   U -> V
   V -> ID
   V -> digito
   V -> true
   V -> false
   V -> cadena
   V -> ID AbreParent L CierraParent
   V -> AbreParent E CierraParent
   V -> ID MASMAS 
}
```









### Gramática válida:

Para asegurarnos que la gramática era válida hemos hecho uso de la herramienta 
online : http://jsmachines.sourceforge.net/machines/lr1.html debido a que la 
comprobación manual se volvio inabarcable por la gran cantidad de reglas. 
Por suerte dada la naturaleza del analizador ascendente solo hemos tenido que evitar 
los conflictos r-r y r-d. 

### Errores:

De momento cuando se produce un error sintáctico se indica el número de línea donde 
se ha producido y el último token. Dejando indicado que es en esa zona donde está el 
error.

### Tablas:

Las tablas acción y goto se han realizado en un archivo csv que le pasamos 
directamente al programa junto con el archivo reglas que contiene la gramática. 
Colocamos un enlace a las tablas dado que por motivos de tamaño no es válido 
colocarlas aquí. Enlace a las tablas: https://drive.google.com/drive/folders/1vRX-zgrh0OHHAYFmvGuJdX05u_ocW4IG

### Implementación:

Tal como ya hemos mencionado hemos seguido la estructura presentada en la 
documentación de la asignatura. Tenemos desplazamientos y reducciones. En los desplazamientos vamos expandiendo las reglas y en las reducciones, reducimos, es decir, eliminamos la regla de la pila del sintáctico dejando solo el antecedente. La última "reducción" es la llamada aceptación que marca el final del análisis sintáctico.

 Para el manejo de la tabla acción y goto se recurre a 
archivos csv que son convertidos a una lista bidimensional con las acciones y lo mismo 
con la tabla goto. Para más detalles el código fuente está disponible en el repositorio 
privado (Tendría que agregar a quien lo desease visitar) : https://github.com/krlsm/TraductorDeLenguaje_JS-PL.git

------



## Analizador semántico



### Introducción

Para la realización del semántico se ha necesitado utilizar una pila con los terminales y no terminales que funciona de forma paralela a la pila del sintáctico. Los símbolos, ya sean terminales o no terminales, se van añadiendo como atributos en los desplazamientos y en las reducciones se ejecuta la acción semántica acorde a la regla que se reduzca.

Se ha escogido una especificación de las acciones semánticas EDT para ya disponer desde el diseño toda la información necesaria para su implementación. Aun así han surgido problemas en el proceso de implementación ya que esta es muy dependiente de la gramática del sintáctico y para declarar funciones se han tenido que crer una especie de regla intermedia que funciona de forma interna. Se ha evitado utilizar los atributos heredados ya que suponian aumentar la complejidad frente a los sintetizados, recordemos que al utilizar un análisis sintáctico ascendente primero tratamos con los hijos y ya después con los padres de los nodos del arbol sintáctico.

### Atributos creados:

**Ancho**: Indica el tamaño del tipo de turno. En este caso sirve para los tipos int,string,boolean y siempre va a terminar valiendo 1.

**Simbolo**: Sirve para identificar al simbolo, ya sea terminal o no terminal de forma facilmente legible.

**Lexema**: Sirve para guarda el nombre de un ID. Para mayor legibilidad y para acceder a los elementos de la tabla de símbolos ya que esta usa el lexema como clave.

**TipoRet**: Sirve para guardar el tipo de retorno en el caso de una función. 

**ListaVar**: Sirve para guardar la lista de tipos de los parámetros de una función.

**numLineaCodigo**: Indica la linea de código en el que apareción el token correspondiente en el léxico. Sirve para la gestion de errores.





















### Acciones semánticas EDT:

```
    1 PAxioma -> {Crear TSG, DesplG=0} P {imprimirTS,DestruirTSG}
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
     	} S
    	if(S.tipo=TIPO_ERROR){B.tipo = TIPO_ERROR (Hay un error en el cuerpo del if.)}
    	B.tipoRet = S.tipoRet
        
    7 B -> do AbreCorchetes C CierraCorchetes while AbreParent E CierraParent{
    	B.tipo=if(E.tipo==boolean) then C.tipo 
            else Tipo_error "Entre los parentesis del while se espera una expresion logica "}
        B.tipoRet = C.tipo
        }PuntoComa
             
	8 B -> S {
		B.Tipo = S.Tipo;
        B.TipoRet = S.TipoRet;
        }
	9 T -> int {T.tipo = int,T.ancho=size_int}
	10 T -> boolean{T.tipo=boolean,T.ancho=size_boolean}
	11 T -> string {T.tipo=string,T.ancho=size_string}
	12 S -> ID IGUAL E PuntoComa{
		if(BuscarTipoTS(ID.lexema) ==null) insertarTSG(ID.lexema,despl,int) despl+=size_int;
    	S.tipo=if(E.tipo==BuscarTipoTS(ID.lexema))
    		else ERROR En una asignación los tipos han de ser iguales.
    	}
    	
	13 S -> return X PuntoComa{
		S.tipo=if(TSL!=null) then TIPO_OK and S.tipoRet=X.tipo
               else TIPO_ERROR S.TipoRet=tipo_Vacio}
               
	14 S -> print AbreParent E CierraParent PuntoComa{
		S.Tipo =Tipo.TIPO_OK;
        S.TipoRet = Tipo.vacio;
		}
		
	15 S -> input AbreParent ID CierraParent {
		S.tipo = if(BuscarTipo_TS(ID.lexema)==cadena or digito) TIPO_OK S.TipoRet=tipo_Vacio
        else TIPO_ERROR "input solo admite variables de tipo int o string."}PuntoComa
        
	16 S -> ID AbreParent L CierraParent{
		S.Tipo=if(L.Tipo!=TIPO_ERROR and BuscarTSGListaVar(TS,ID.lexema)==L.listaVar->t)then TIPO_OK 
                else TIPO_ERROR
        S.TipoRet=tipo_Vacio
        } PuntoComa
        
	17 X -> E{X.tipo=E.tipo}
	18 X -> lambda{X.tipo=Tipo_Vacio}
	19 L -> E Q {
		L.tipo=if(Q.tipo=TIPO_ERROR or E.tipo=TIPO_ERROR then TIPO_ERROR
               else TIPO_OK)
        L.ListaVar=E.tipo x Q.ListaVar}
              
	20 L -> lambda {L.tipo=Tipo_OK, L.ListaVar=""}
	21 Q -> COMA E Q{
		Q.tipo=if(Q1.tipo=tipo_error or E.tipo=TIPO_ERROR) then TIPO_ERROR
                else TIPO_OK
         Q.ListaVar=E.tipo x Q1.ListaVar}
         
	22 Q -> lambda{Q.tipo=Tipo_OK, Q.ListaVar="" }
	23 F -> function H ID {
		F.tipo = if(BuscarTS(ID.lexema)!=null) Then TIPO_ERROR "Variable ya declarada"
                 else InsertaTS(ID.lexema)
                      TSL=CreaTS()
                      TablaLocalActiva=true;
                      DesplL:=0 } 
      AbreParent A CierraParent{
      	insertaTipoTSG(ID.lexema,A.listaVar->H.tipo)
        InsertaEtiqTS(ID.lexema,nueva_et())} 
      AbreCorchetes C CierraCorchetes{
      	if(C.TipoRet!=H.tipo) then TIPO_ERROR "la funcion esperaba devolver otro tipo de varametro."
        else Tipo_ok
        DestruyeTS(TSL)
        }
                                                      
	24 H -> T {H.tipo=T.tipo }
	25 H -> lambda {H.tipo= Tipo_vacio}
	26 C -> B C {
		C.tipo=if(B.tipo=Tipo_OK) then C.tipo
                     else TIPO_ERROR
        if (B.TipoRet == C1.TipoRet){
           C.TipoRet = C1.TipoRet;
        }else if (B.TipoRet == Tipo.vacio){
            C.TipoRet = C1.TipoRet;
        }else if (C1.TipoRet == Tipo.vacio){
          	C.TipoRet = B.TipoRet;
        }
        else{
           C.TipoRet = Tipo.TIPO_ERROR;           
        }}
        
	27 C -> lambda {C.tipo=TIPO_OK, C.tipoRet=Tipo_Vacio}
	28 A -> T ID K{
		A.tipo=if(K.tipo=TIPO_ERROR or buscaTipoTSL(ID.lexema)!= null) then TIPO_ERROR "Argumento incorrecto"
               else insertaTS(ID.lexema,T.tipo,desplL)
         desplL=desplL+T.ancho
         A.listaVar=T.tipo x K.listaVar
         }
         
	29 A -> lambda    {A.tipo=TIPO_OK,A.listaVar=""}
	30 K -> COMA T ID K {K.tipo= if(K.tipo=TIPO_ERROR or BuscarTS(ID.lexema)!=null) then TIPO_ERROR "Argumento incorrecto"
                             else TIPO_OK
                             K.listaVar=T.tipo x K1.listaVar}
	31 K -> lambda{K.tipo=TIPO_OK,K.listaVar=""}
	32 E -> E AND R{
		E.tipo=if(E1.tipo==boolean and R.tipo==boolean then boolean 
               else TIPO_ERROR}
               
	33 E -> R  {E.tipo=R.tipo}
	34 R -> R IGUALIGUAL U {
		R.tipo=if(R1.tipo==U.tipo) then R1.tipo
               else TIPO_ERROR
        }
               
	35 R -> U {R.tipo=U.tipo}
	36 U -> U Suma V{
		U.tipo=if(U1.tipo==int and V.tipo==int) then int 
                         else TIPO_ERROR
        }
        
	37 U -> V  {U.tipo=V.tipo}
	38 V -> ID {V.tipo=BuscaTipoTS(ID.lexema)}
	39 V -> digito {V.tipo=int}
	40 V -> true {V.tipo=boolean}
	41 V -> false {V.tipo=boolean}
	42 V -> cadena {V.tipo=string}
	43 V -> ID AbreParent L CierraParent {
		if(buscarTS(ID.lexema)==null){then ERROR}
		else if{buscarTipoParametros(ID.lexema) == A.TipoParametros) V.tipo = Tipo devuelto}}else error.
	
	44 V -> AbreParent E CierraParent{V.tipo=E.tipo}
	45 V -> ID MASMAS {
		V.tipo=if (BuscarTipoTS(ID.lexema)==int) then int
               else TIPO_ERROR "Se esperaba una variable de tipo int."
        }
```

------



## Diseño de la tabla de símbolos:



### La tabla de símbolos:  Estructura y organización: 

La tabla de símbolos consiste en un Hashtable. El acceso por tanto es prácticamente O(1).  Se utiliza como clave el  lexema del identificador  y como valor un objeto llamado ObjetoTS que contiene todos los parámetros necesarios: desplazamiento, tipo, etiqueta, num de parámetros etc. Se ha escogido esta implementación porque de esta  forma ampliar una columna en la tabla de símbolos es trivial. Se  añadiría una propiedad al objetoTS y un método del tipo ts[lexema].propiedad. Para la impresión de la tabla de símbolos, cada objeto tiene un método  Imprimir de forma que todo el código resulta fácilmente modificable y el diseño es absolutamente modular.

### Gestor de la Tabla de símbolos:

El usuario, en este caso yo, solo interactúa con el gestor de forma que la tabla de símbolos es transparente al usuario. El gestor en este caso se compone de dos tablas de símbolos, una global y una local. La global se mantiene desde el momento de su creación  hasta su destrucción en la regla semántica 1. La local por el contrario  se irá creando y destruyendo. Este objeto, también posee un método imprimir que se ocupa de guardar las tablas de símbolos en el formato especificado. Este método debe ser llamado cuando se destruye una tabla de símbolos.

Las **palabras reservadas** se almacenan en el gestor de la tabla de símbolos en otra estructura de tipo Hashtable, por tanto, cuando se imprime la tabla de símbolos, las palabras  reservadas no aparecen dentro de ninguna de las tablas. Si se desease  otro comportamiento sería tan sencillo como concatenar una impresión de las palabras reservadas en la tabla de símbolos global.

Es importante reseñar que es el **analizador semántico** quién añade los identificadores a la tabla de símbolos y no el analizador léxico como en anteriores entregas. Esto es así porque se ha intentado seguir una implementación lo más acorde posible con la teoría vista en clase. Además, aunque se mantiene un contador para saber que identificador se ha añadido primero, esto es irrelevante en un sentido práctico, ya que una tabla hash no ordena los elementos según se insertan.

De esta forma el acceso a los elementos de la tabla de símbolos es aproximadamente O(1).

------



## Gestión de errores:

Para la **gestión de errores** se diferencian los errores léxicos,  sintácticos y semánticos. Los dos primeros bloquean la compilación  mientras que los errores semánticos se muestran todos los que se  encuentran. La excepción aquí es el error semántico sobre la declaración de una función. Esto provoca un error que sí para la compilación. Esto  es así porque de otra forma se meterían los valores de la función dentro de la tabla de símbolos global y el error sería fatal.

En el caso de que no se haya declarado una variable, se muestra un **warning** indicando que la variable no ha sido previamente declarada, ya que se considera una mala práctica o un despiste.

Cuando ocurre un error, se muestra un pequeño texto descriptivo y el  número de linea en el que se da el error. El número de línea esta  asociado a los tokens por lo que si bien el semántico o léxico siempre  va a ser correcto, en el sintáctico cuando falta un ";" el error se marca en la linea siguiente.

------

## Instrucciones para ejecutar el código:

1. Descomprimir el zip.

2. Acceder a la ruta "ProcesadorDeLenguaje_JS-PL/ProcesadorDeLenguaje_JS-PL/bin/Debug"    Desde el directorio raiz podría mediante el comando:

   ```
    cd ProcesadorDeLenguaje_JS-PL/bin/Debug
   ```

3. En DOS:

   ```
    ProcesadorDeLenguaje_JS-PL.exe [nombre de fichero]
   ```

4. Los resultados estaran en la ruta "ProcesadorDeLenguaje_JS-PL/ProcesadorDeLenguaje_JS-PL/Resultados"

5. El fichero tokens.txt contiene los tokens, parse.txt contiene el parse y   TS.txt contiene la tabla de símbolos.

------



## Anexo:

A continuación se presentan 5 casos correctos y 5 casos incorrectos.

### Casos correctos:

#### Caso  1:

Código:

```javascript
var int a;
var int b;
var int c;
print ("Introduce el primer operando");
input (a);
print ("Introduce el segundo operando");
input (b);
function int suma (int num1, int num2)
{
	return num1+num2;
}
c = suma (a, b);
print (c);
```

Tokens:

```
<var,>
<int,>
<ID,1>
<PuntoComa,>
<var,>
<int,>
<ID,2>
<PuntoComa,>
<var,>
<int,>
<ID,3>
<PuntoComa,>
<print,>
<AbreParent,>
<cadena,"Introduce el primer operando">
<CierraParent,>
<PuntoComa,>
<input,>
<AbreParent,>
<ID,4>
<CierraParent,>
<PuntoComa,>
<print,>
<AbreParent,>
<cadena,"Introduce el segundo operando">
<CierraParent,>
<PuntoComa,>
<input,>
<AbreParent,>
<ID,5>
<CierraParent,>
<PuntoComa,>
<function,>
<int,>
<ID,6>
<AbreParent,>
<int,>
<ID,7>
<COMA,>
<int,>
<ID,8>
<CierraParent,>
<AbreCorchetes,>
<return,>
<ID,9>
<Suma,>
<ID,10>
<PuntoComa,>
<CierraCorchetes,>
<ID,11>
<IGUAL,>
<ID,12>
<AbreParent,>
<ID,13>
<COMA,>
<ID,14>
<CierraParent,>
<PuntoComa,>
<print,>
<AbreParent,>
<ID,15>
<CierraParent,>
<PuntoComa,>
```

Parse:

```
Ascendente 9 5 9 5 9 5 42 37 35 33 14 8 15 8 42 37 35 33 14 8 15 8 9 24 9 9 31 30 28 38 37 38 36 35 33 17 13 8 27 26 23 38 37 35 33 38 37 35 33 22 21 19 43 37 35 33 12 8 38 37 35 33 14 8 4 2 2 3 2 2 2 2 2 2 2 1
```

Tabla de símbolos:

```
![Screenshot_2020-06-24 Arbol](/home/krls/DriveBrujo/ING_Informatica/PDL/Imagenes_Memoria/Screenshot_2020-06-24 Arbol.png)Tabla Global   # 1:
* LEXEMA: 'a' 
ATRIBUTOS: 
+ tipo: 'int' 
+ despl: '0' 

 * LEXEMA: 'b' 
ATRIBUTOS: 
+ tipo: 'int' 
+ despl: '1' 

 * LEXEMA: 'suma' 
ATRIBUTOS: 
+ tipo: 'function' 
+ numParam: 2
+ TipoParam1: 'int' 
+ TipoParam2: 'int' 
+ TipoRetorno: 'int' 
+ EtiqFuncion: 'Etsuma1' 

 * LEXEMA: 'c' 
ATRIBUTOS: 
+ tipo: 'int' 
+ despl: '2' 

  TABLA Local de la FUNCION suma    # 2:
* LEXEMA: 'num1' 
ATRIBUTOS: 
+ tipo: 'int' 
+ despl: '1' 

 * LEXEMA: 'num2' 
ATRIBUTOS: 
+ tipo: 'int' 
+ despl: '0' 

 
```

Arbol VASt:

![Screenshot_2020-06-24 Arbol](/home/krls/DriveBrujo/ING_Informatica/PDL/Imagenes_Memoria/Screenshot_2020-06-24 Arbol.png)

#### Caso 2:

Código:

```javascript
var string s;	
var int    uno;
var int    UNO;
function int Factorial (int n)
{
	var int factorial;
	factorial = 0 + uno + 1;	
	do
	{
		factorial = factorial + n; n = n + 1;
	} while (n == 0);		
	return factorial;	
}
var int For;
var int functional;
var int While;	

function imprime (string s, string msg, int f)	
{
	print (s); print (msg); print (f);
	return;	
}
function string cadena (boolean log)
{
	do {
		imprime (s, "hola", 33);
		if (uno == UNO)	return s;
	}
	while (log);
}	
s = "El factorial ";	

print (s);
print ("Introduce un numero.");
input (num);
var 
boolean 
booleano;
if (num == 0)		print ("No existe el factorial de un negativo.");
imprime (cadena (booleano), "recursivo es: ", Factorial (num));
```

Tokens:

```
<var,>
<string,>
<ID,1>
<PuntoComa,>
<var,>
<int,>
<ID,2>
<PuntoComa,>
<var,>
<int,>
<ID,3>
<PuntoComa,>
<function,>
<int,>
<ID,4>
<AbreParent,>
<int,>
<ID,5>
<CierraParent,>
<AbreCorchetes,>
<var,>
<int,>
<ID,6>
<PuntoComa,>
<ID,7>
<IGUAL,>
<digito,0>
<Suma,>
<ID,8>
<Suma,>
<digito,1>
<PuntoComa,>
<do,>
<AbreCorchetes,>
<ID,9>
<IGUAL,>
<ID,10>
<Suma,>
<ID,11>
<PuntoComa,>
<ID,12>
<IGUAL,>
<ID,13>
<Suma,>
<digito,1>
<PuntoComa,>
<CierraCorchetes,>
<while,>
<AbreParent,>
<ID,14>
<IGUALIGUAL,>
<digito,0>
<CierraParent,>
<PuntoComa,>
<return,>
<ID,15>
<PuntoComa,>
<CierraCorchetes,>
<var,>
<int,>
<ID,16>
<PuntoComa,>
<var,>
<int,>
<ID,17>
<PuntoComa,>
<var,>
<int,>
<ID,18>
<PuntoComa,>
<function,>
<ID,19>
<AbreParent,>
<string,>
<ID,20>
<COMA,>
<string,>
<ID,21>
<COMA,>
<int,>
<ID,22>
<CierraParent,>
<AbreCorchetes,>
<print,>
<AbreParent,>
<ID,23>
<CierraParent,>
<PuntoComa,>
<print,>
<AbreParent,>
<ID,24>
<CierraParent,>
<PuntoComa,>
<print,>
<AbreParent,>
<ID,25>
<CierraParent,>
<PuntoComa,>
<return,>
<PuntoComa,>
<CierraCorchetes,>
<function,>
<string,>
<ID,26>
<AbreParent,>
<boolean,>
<ID,27>
<CierraParent,>
<AbreCorchetes,>
<do,>
<AbreCorchetes,>
<ID,28>
<AbreParent,>
<ID,29>
<COMA,>
<cadena,"hola">
<COMA,>
<digito,33>
<CierraParent,>
<PuntoComa,>
<if,>
<AbreParent,>
<ID,30>
<IGUALIGUAL,>
<ID,31>
<CierraParent,>
<return,>
<ID,32>
<PuntoComa,>
<CierraCorchetes,>
<while,>
<AbreParent,>
<ID,33>
<CierraParent,>
<PuntoComa,>
<CierraCorchetes,>
<ID,34>
<IGUAL,>
<cadena,"El factorial ">
<PuntoComa,>
<print,>
<AbreParent,>
<ID,35>
<CierraParent,>
<PuntoComa,>
<print,>
<AbreParent,>
<cadena,"Introduce un numero.">
<CierraParent,>
<PuntoComa,>
<input,>
<AbreParent,>
<ID,36>
<CierraParent,>
<PuntoComa,>
<var,>
<boolean,>
<ID,37>
<PuntoComa,>
<if,>
<AbreParent,>
<ID,38>
<IGUALIGUAL,>
<digito,0>
<CierraParent,>
<print,>
<AbreParent,>
<cadena,"No existe el factorial de un negativo.">
<CierraParent,>
<PuntoComa,>
<ID,39>
<AbreParent,>
<ID,40>
<AbreParent,>
<ID,41>
<CierraParent,>
<COMA,>
<cadena,"recursivo es: ">
<COMA,>
<ID,42>
<AbreParent,>
<ID,43>
<CierraParent,>
<CierraParent,>
<PuntoComa,>
```

Parse:

```
Ascendente 11 5 9 5 9 5 9 24 9 31 28 9 5 39 37 38 36 39 36 35 33 12 8 38 37 38 36 35 33 12 8 38 37 39 36 35 33 12 8 27 26 26 38 37 35 39 37 34 33 7 38 37 35 33 17 13 8 27 26 26 26 26 23 9 5 9 5 9 5 25 11 11 9 31 30 30 28 38 37 35 33 14 8 38 37 35 33 14 8 38 37 35 33 14 8 18 13 8 27 26 26 26 26 23 11 24 10 31 28 38 37 35 33 42 37 35 33 39 37 35 33 22 21 21 19 16 8 38 37 35 38 37 34 33 38 37 35 33 17 13 6 27 26 26 38 37 35 33 7 27 26 23 42 37 35 33 12 8 38 37 35 33 14 8 42 37 35 33 14 8 15 8 10 5 38 37 35 39 37 34 33 42 37 35 33 14 6 38 37 35 33 22 19 43 37 35 33 42 37 35 33 38 37 35 33 22 19 43 37 35 33 22 21 21 19 16 8 4 2 2 2 2 2 2 2 3 3 2 2 2 3 2 2 2 1
```

Tabla de símbolos:

```
Tabla Global   # 1:
* LEXEMA: 's' 
ATRIBUTOS: 
+ tipo: 'string' 
+ despl: '0' 

 * LEXEMA: 'cadena' 
ATRIBUTOS: 
+ tipo: 'function' 
+ numParam: 1
+ TipoParam1: 'boolean' 
+ TipoRetorno: 'string' 
+ EtiqFuncion: 'Etcadena3' 

 * LEXEMA: 'For' 
ATRIBUTOS: 
+ tipo: 'int' 
+ despl: '3' 

 * LEXEMA: 'UNO' 
ATRIBUTOS: 
+ tipo: 'int' 
+ despl: '2' 

 * LEXEMA: 'imprime' 
ATRIBUTOS: 
+ tipo: 'function' 
+ numParam: 3
+ TipoParam1: 'string' 
+ TipoParam2: 'string' 
+ TipoParam3: 'int' 
+ TipoRetorno: 'vacio' 
+ EtiqFuncion: 'Etimprime2' 

 * LEXEMA: 'uno' 
ATRIBUTOS: 
+ tipo: 'int' 
+ despl: '1' 

 * LEXEMA: 'functional' 
ATRIBUTOS: 
+ tipo: 'int' 
+ despl: '4' 

 * LEXEMA: 'While' 
ATRIBUTOS: 
+ tipo: 'int' 
+ despl: '5' 

 * LEXEMA: 'Factorial' 
ATRIBUTOS: 
+ tipo: 'function' 
+ numParam: 1
+ TipoParam1: 'int' 
+ TipoRetorno: 'int' 
+ EtiqFuncion: 'EtFactorial1' 

 * LEXEMA: 'booleano' 
ATRIBUTOS: 
+ tipo: 'boolean' 
+ despl: '7' 

 * LEXEMA: 'num' 
ATRIBUTOS: 
+ tipo: 'int' 
+ despl: '6' 

  TABLA Local de la FUNCION Factorial    # 2:
* LEXEMA: 'factorial' 
ATRIBUTOS: 
+ tipo: 'int' 
+ despl: '1' 

 * LEXEMA: 'n' 
ATRIBUTOS: 
+ tipo: 'int' 
+ despl: '0' 

  TABLA Local de la FUNCION imprime    # 3:
* LEXEMA: 'msg' 
ATRIBUTOS: 
+ tipo: 'string' 
+ despl: '1' 

 * LEXEMA: 's' 
ATRIBUTOS: 
+ tipo: 'string' 
+ despl: '2' 

 * LEXEMA: 'f' 
ATRIBUTOS: 
+ tipo: 'int' 
+ despl: '0' 

  TABLA Local de la FUNCION cadena    # 4:
* LEXEMA: 'log' 
ATRIBUTOS: 
+ tipo: 'boolean' 
+ despl: '0' 

 
```

Arbol VASt:

![arbol2](/home/krls/DriveBrujo/ING_Informatica/PDL/Imagenes_Memoria/arbol2.png)

#### Caso 3:

Código:

```javascript

var int x;  
var string cadena;
cadena = "hola mundo";
print(cadena);
input(casa);
function int factorial (int x) 
   //  se define la función recursiva con un parámetro,  que oculta a la variable global de igual nombre 
{
  if (x == 1)
	return x + factorial (x + 1);
  

}	

var boolean alfa;
alfa = true;  
function boolean holaMUndo2(){

return true;
}

function int holaMUndo22(){
z = z+98+j;
return a+4;
}

function int suma(int a,int b){
return a+b;
}

function Imprime (int a)
{

	do{print (a);} while(6==6);
    print (a);
    return ;	// esta instrucción se podría omitir
}	

Imprime(suma(6,8));

```

Warnings:

```ini
Warning Linea(24) No has declarado la variable "z". Por defecto se asume que es un int con valor 0.
Warning Linea(24) No has declarado la variable "j". Por defecto se asume que es un int con valor 0.
Warning Linea(25) No has declarado la variable "a". Por defecto se asume que es un int con valor 0.
```



#### Caso 4:

Código:

```javascript
var int a;
var int b;
a = 3;
b = a;
var boolean c;
c = a == b;
if (c) b = 1;
if (b == a) b = 4;
a = a + b;
print (a);
print (b);
```



#### Caso 5:

Código:

```javascript
var string texto;
function imprime (string msg)
{
	print (msg);
}
function pideTexto ()
{
	print ("Introduce un texto");
	input (texto);
}
pideTexto();
var string textoAux;
textoAux = texto;
imprime (textoAux);
```



------



### Casos incorrectos:

#### Caso 1:

Código:

```javascript

var int x;  
var string cadena;
cadena = "hola mundo";
print(cadena);
input(casa);
function int factorial (int x) 
   //  se define la función recursiva con un parámetro,  que oculta a la variable global de igual nombre 
{
  if (x == 1)
	return x + factorial (x + 1);
  

}	

var boolean alfa;
alfa = true;  
function boolean holaMUndo2(){

return true;
}

function int holaMUndo22(){
z = z+98+j;
return a+4;
}

function int suma(int a,int b){
return a+b;
}

function Imprime (int a)
{

	do{print (a);} while(6==6);
    print (a);
    return ;	// esta instrucción se podría omitir
}	

ImprimeALFA(suma(6,8));

```

Errores:

```ini
Warning Linea(24) No has declarado la variable "z". Por defecto se asume que es un int con valor 0.
Warning Linea(24) No has declarado la variable "j". Por defecto se asume que es un int con valor 0.
Warning Linea(25) No has declarado la variable "a". Por defecto se asume que es un int con valor 0.
Error SEMANTICO: Linea(40) LLamada a funcion incorrecta.
```



#### Caso 2:

Código:

```javascript
function int Imprime  (int a)
{

	do{print (a);} while(1==1);
	do{print (a);} while("casa"=="casa");
	do{print (a);} while(true==true);
	print("hola mundo");
	if(1==1 && 2==3) print("hola mundo");//i= 7;
    print (a);
    return 1;	// esta instrucción se podría omitir
}	

numero = Imprime(6);
Imprime(6);
```

Errores:

```ini
Error SEMANTICO: Linea(6) No se puede comparar un: string con un: string. Solo se pueden comparar enteros.
Error SEMANTICO: Linea(6) Entre los parentesis del while se espera una expresion logica 
Error SEMANTICO: Linea(7) No se puede comparar un: boolean con un: boolean. Solo se pueden comparar enteros.
Error SEMANTICO: Linea(7) Entre los parentesis del while se espera una expresion logica 
Error SEMANTICO: Linea(8) Tipos devueltos erroneos.
Error SEMANTICO: Linea(7) Tipos devueltos erroneos.
Error SEMANTICO: Linea(2) La funcion esperaba devolver un: int No un: TIPO_ERROR
Warning Linea(14) No has declarado la variable "numero". Por defecto se asume que es un int con valor 0.

```



#### Caso 3:

Código:

```javascript
var string texto;
function imprime (string msg)
{
	print (msg);
}
function pideTexto ()
{
	print ("Introduce un texto");
	input (texto);
}
pideTexto((;
var string textoAux;
textoAux = texto;
imprime (textoAux);
```

Errores:

```ini
Error SINTACTICO: Linea(11) Tiene que estar cerca del simbolo: PuntoComa: 
```



#### Caso 4:

Código:

```javascript
function imprime (string msg)
{
	print (msg);
}
function pideTexto ()
{
	print ("Introduce un texto");
	input (texto);
}
pideTexto();
var string masde64caracteressssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss;
var string textoAux;
textoAux = texto;
imprime (textoAux);
```

Errores:

```ini
Error Lexico: Linea(11)  Una constante tiene como max 64 caracteres
```



#### Caso 5:

Código:

```javascript
function imprime (string msg)
{
	print (msg);
}
function pideTexto ()
{
	print ("Introduce un texto");
	input (texto);
}
pideTexto();
var int aspa;
aspa = 12852387038457308573087234095872305987;
var string textoAux;
textoAux = texto;
imprime (textoAux);
```

Errores:

```ini
Error Lexico: Linea(12) El rango de numeros es (-32768,32767)

```

