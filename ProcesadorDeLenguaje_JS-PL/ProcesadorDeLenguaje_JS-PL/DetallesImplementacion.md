## Memoria Procesadores de Lenguajes.









Procesador parcial del lenguaje Javascript-PL para la asignatura de Procesadores de Lenguajes



Foto de la estructura de la etapa de analisis. y fase de sintesis : INSERTAR

                  Fase se Analisis : Fase de sintesis
             A.lex->A.sint->A.sem -> Generacion Codigo intermedio-> gen codigo Final-Obj->Fich Obj



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
