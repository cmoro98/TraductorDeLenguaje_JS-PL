## Memoria Procesadores de Lenguajes.

Grupo 159:
Nombre: Carlos Apellidos: Moro Jiménez. 





# Table of contents
1. [Introduction](#introduction)
2. [Some paragraph](#paragraph1)
    1. [Sub paragraph](#subparagraph1)
3. [Another paragraph](#paragraph2)

## This is the introduction <a name="introduction"></a>
Some introduction text, formatted in heading 2 style

## Some paragraph <a name="paragraph1"></a>
The first paragraph text

### Sub paragraph <a name="subparagraph1"></a>
This is a sub paragraph, formatted in heading 3 style

## Another paragraph <a name="paragraph2"></a>
The second paragraph text




















Nuestra opcion es 111
Características del lengujae:
	- Funciones , declaraciones y sentencias.
	- Definición de funciones.
	- Tipos enteros, lógicos y cadenas.
	- Variables y su declaración.
	- Constantes enteras y cadenas de caracteres.
	- Sentencias: asignación, condicional simple , llamada a funciones y retorno.
	- Expresiones.
	- Comentarion //
	- Operaciones de E/S 
		prompt
		print
	- Operadores:
		+
		==
		&&
	- Sentencia repetitiva (for)
	- Operadores especiales: 
		Asignación con resto (%=)


Tecnica de Análisis:
	Analisis Descendente con Tablas.

Análisis Léxico:


	1. Determinar tokens

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
	             <ENT,valor>
	             <ID,posTS>           
	             <COMA,->
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

	2. Construir la gramática (Gram T3 regular)
	
	Gramatica:
			S-> &A|=B|(|)||,|;|{|}|lC|dD|"E|+F|/G|del S
		            A-> &
		            B-> =|LAMBDA
		            C-> lC|dC|_C|LAMBDA
		            D-> dD|LAMBDA
		            E-> c1E|"
		            F-> +|LAMBDA
		            G-> /H
		            H-> c2 H|saltolinea S
		           

	3. Construir AFD Hecho
	4. Incorporar acc semánticas 
	5. Tratar errores
	6. Implementar el AFD 



Pruebas
prueba 01:

Fichero Correcto:


Fichero 1:

	var int c;
	c =893;
	c = -32;
	var int b;
	b = 40;
	b=b+c;
	print(casa);
	input(casa);
	return a;
	if(a=="cadena")


Fichero 2:
	var int a=8;
	a++;
	do{
	var string casa
	casa="cadenitas"; // Comentario 1
	// comentario2
	var int b;
	b=12;
	boolean tr = true;
	boolean fr = false;
	}
	while(tr&&fr)


Fichero 3:

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


Ficheros Incorrectos:
Fichero 1:
	var int c;
	c =893;
	c = -32;
	var int b;

Fichero 2:
var bool booleano; //comentario.
var string ok &/ 
var int cas0 = 12;


Fichero 3:
	input (b);
	function int suma (int num1, int num2)**/
	{
		return num1+num2;
	}




An sem
Reduccion es cuando se ejecuta una accion semantica.Pues es cuando se ha resuelto una regla
Evidentemente también se ejecuta una acc semantica en la aceptacion. Pues se resuelve una regla (es la ultima reduccion)