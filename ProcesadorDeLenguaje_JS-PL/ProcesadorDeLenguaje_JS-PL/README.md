## Memoria Procesadores de Lenguajes.

Grupo 159:
Nombre: Carlos Apellidos: Moro Jiménez. 



####Introducción:
Esta es una memoria sobre la creación de un traductor del lenguaje Javascript-PDL  para la asignatura de Procesadores de lenguajes. Este proyecto ha sido realizado en C#.

### Subtitulo.

	Tokens:
	             <MAS,->
	             <MASMAS,->
	             <AND,->
	             <IGUAL,->
	             <IGUALIGUAL,->
	             <CierraParent,->
	             <AbreParent,->
		         <AbreCorchetes>
		         <CierraCorchetes>	
	             <cadena,lex>
	             <digito,valor>
	             <ID,posTS> 
	             <Coma,->
	             <PuntoComa>
	             …………….Palabras Reservadas Van en la TS-----------SI	
		         <if,->
	             <function,->
	             <int,->
	             <boolean,->
	             <string,->
	             <true,->
	             <false,->
	             <input,->
	             <print,->
	             <var,->
	             <do->
		         <while,->
	             <return,->

###Gramática:


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


 **   Matriz de transición**
 
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







 


















Procesador parcial del lenguaje Javascript-PL para la asignatura de Procesadores de Lenguajes



Foto de la estructura de la etapa de analisis. y fase de sintesis : INSERTAR

                  Fase se Analisis : Fase de sintesis
             A.lex->A.sint->A.sem -> Generacion Codigo intermedio-> gen codigo Final-Obj->Fich Obj

#### Analizador Léxico:

#### Analizador Sintáctico:

##### Implementación
Un analisis ascendente. Ninguna complicación. Usamos la tabla GOTO y ACCION calculada en esta pagina:
y la metemos en un csv (correctamente) y usamos el algoritmo visto en clase. No tiene más.
En los desplazamientos y reducciones se va a llamar al semántico para que haga sus cosas. Esta marcado claramente.


#### Analizador Semántico:

El analisis semantico es la tercera fase de la etapa de analisis para la construcción de un compilador y en ella vamos a asegurnos que los tipos de los datos estén bien. Suele enmarcarse dentro del contexto de un proceso llamado traducción dirigida por la sintaxis, en la que :
Cuando llegamos al analizador semantico ya hemos comprobado que los tokens que nos han pasado son lexicamente y sintacticamente correctos.
En esta fase se da un significado semantico a la estructura sintactica previamente definida. Es decir comprobamos que los tipos son correctos.

Ejemplo:

string A= "casa";
correcta lexica, sintactica y semanticamente

int a ="casa"; correcta lexica y sintacticamente INCORRECTA semanticamente

int a=5; 
boolean b=true; 
a==b; 

Estas estructuras son correctas sintacticamente pero incorrectas desde el punto de vista semántico.
Entonces en el analisis semantico nos aseguramos de que el codigo sea coherente  tenga significado.


Ahora , para realizar el analisis semantico de un lenguaje partimos de la gramatica del sintactico, que es una gramatica de contexto libre o tipo 2 y a esta gramatica la vamos añadir reglas semanticas. A este proceso se lo conoce como traducción dirigida por la sintaxis. 
Podemos recurrir que yo sepa a **dos tipos de notaciones** para representar las acciones semanticas: 

DDS: Notación de alto nivel:  No importa el orden.

EDT: Notación de bajo nivel: Importa el orden
Yo he elegido EDT para tener un diseño con todos los detalles.


##### Implementación


Acciones semánticas en el Analizador Semántico (Una por cada regla del sintáctico.)
Dado el atributo ID para acceder a la tabla de simbolos usamos la propiedad .Lexema y no .pos 
ya que la tabla de simbolos usa como clave el lexema. 

##### Atributo
