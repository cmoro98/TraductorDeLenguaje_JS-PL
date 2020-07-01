###### Generacion de codigo intermedio

Objetivo1-> SUMA: 1,2,4,5,8,9,12,33,35,36,37,38


R1:
P'->P

	| P'.cod = P.cod
	
R2:
P->B P

	| P.cod = B.cod || P1.cod
R3:
P->F P

	| P.cod = FUN.cod || P1.cod

R4:
P->

	| P.cod = vacío
	
R5: #DECLARACION:   
B->var T ID PuntoComa

	| B.cod = vacío

R6:
B->if AbreParent E CierraParent S

R7:
B->do AbreCorchetes C CierraCorchetes while AbreParent E CierraParent PuntoComa

    B.code = gen("B. Inicio") || C.code || E. Code || gen(If E.value != 0 goto B.inicio)
    
R8:
B->S

	| B.lugar = S.lugar
	| B.cod = S.cod

R9:
T->int


R10:
T->boolean

R11:
T->string

R12: # Asignación:
S->ID IGUAL E PuntoComa

    | S.cod = E.cod || gen(buscaLugarTS(ID.pos), "=", E.lugar)

R13:
S->return X PuntoComa

R14:
S->print AbreParent E CierraParent PuntoComa

	| S.cod = E.cod || gen("print", E.lugar)

R15:
S->input AbreParent ID CierraParent PuntoComa
   
    S.cod = gen("input" , ID)
    	

R16:
S->ID AbreParent L CierraParent PuntoComa

R17:
X->E

R18:
X->

R19:
L->E Q

R20:
L->

R21:
Q->COMA E Q

R22:
Q->

R23:
F->function H ID AbreParent A CierraParent AbreCorchetes C CierraCorchetes

R24:
H->T

R25:
H->

R26:
C->B C

    C.Codigo = B.Codigo || C.Codigo

R27:
C->

    C.Codigo = "";

R28:
A->T ID K


R29:
A->

R30:
K->COMA T ID K

R31:
K->

R32:
E->E1 AND R
      
    | gen(E.lugar, "=", "1")
	| E.siguiente = nuevaEtiq()
	| E.cod = E1.cod || gen("if", E1.lugar, "=", 1, "goto", E.siguiente) || gen (E.lugar, "=", "0") || 
	| gen(E.siguiente, ":") || T.cod || gen("if", R.lugar, "=", 1, "goto", E.fin) || gen(E.lugar, "=", "0") ||
	| gen(E.fin, ":") 

R33:
E->R

	| E.lugar = R.lugar
	| E.cod = R.cod
	| E.cadena = R.cadena
    | E.digito = R.digito	

R34:
R->R IGUALIGUAL U
    
    | R.cod = gen("if", R1.lugar, "=", U.lugar, "goto", R.siguiente) || gen(R.lugar, "=", "0") || gen(R.siguiente, ":") || gen(R.lugar, "=", "1")

R35:
R->U

	| R.lugar = U.lugar
	| R.cod = U.cod
	| R.cadena = U.cadena
    | R.digito = U.digito

R36:
U->U Suma V

	| U.lugar = nuevoTemp()
	| U.cod = U1.cod || V.cod || gen(U.lugar, "=", U1.lugar, "+", V.lugar)

R37:
U->V

	|  U.TipoOperando = V.TipoOperando;
	|  U.Operando = V.Operando;
   
   

R38:
V->ID
	
	| V.Operando =  buscaLugarTS(ID.pos)
	| V.TipoOperando =  buscaTipoOperandoTS(ID.pos)
	| V.cod =  λ

R39:
V->digito

	| V.lugar = Inmediato digito
	

R40:
V->true

    | V.cod = gen(V.lugar, "=", 1)

R41:
V->false

    | V.cod = gen(V.lugar, "=", 0)

R42:
V->cadena

	| V.lugar = Inmediato cadena
	

R43:
V->ID AbreParent L CierraParent

R44:
V->AbreParent E CierraParent

R45:
V->ID MASMAS

    |V = crearTemp
    | gen(V = buscaLugarTS(ID)) || gen(ID = G.)

