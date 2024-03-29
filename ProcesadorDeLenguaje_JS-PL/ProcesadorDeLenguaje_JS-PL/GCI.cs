using System;
using System.Collections.Generic;

namespace ProcesadorDeLenguaje_JS_PL
{
    public class GCI
    {
        private GestorDeErrores gestorDeErrores;
        private GestorTS gesTS;
        private readonly AnalisisLexico alex;
        private GCO generadorDeCodigoObjeto;
        private Atributo cero;
        private Atributo uno;
        private int numEtiq = 0;

        public GCI(GestorTS gesTS, AnalisisLexico alex, GestorDeErrores gestorDeErrores)
        {
            this.alex = alex;
            this.gesTS = gesTS;
            this.gestorDeErrores = gestorDeErrores;
            generadorDeCodigoObjeto = new GCO();
            cero = new Atributo();
            cero.TipoOperando = TipoOperando.Inmediato;
            cero.Operando = "0";
            uno = new Atributo();
            uno.TipoOperando = TipoOperando.Inmediato;
            uno.Operando = "1";
            
        }

        public Atributo nuevaEtiqueta(string prefijo_etiqueta,string sufijo_etiqueta)
        {
            Atributo etiqueta = new Atributo();
            etiqueta.TipoOperando = TipoOperando.Etiqueta;
            etiqueta.Operando = prefijo_etiqueta + numEtiq+ sufijo_etiqueta;
            numEtiq++;
            return etiqueta;
        }
        // HECHAS: 1,2,4,5,8,9,12,33,35,36,37,38   
        // TODO:  6,7,10,11,13,14,15,16,17,18,19,20,21,22,23,24...32,34,40,41,42,43  
        
        /** IMPLEMENTACION. Tenemos las reglas. pasamos los atributos como objetos y ya. POrque estamos pasando un obj y se pasan por def por referencia jeje.
         * Cuando tenemos un cuarteto :
         * 
         */
        public string regla_1(Atributo P,Desplazamiento despl)
        {
            generadorDeCodigoObjeto.TamZonaEstatica = despl.Despl;
            generadorDeCodigoObjeto.ensamblate(P.Codigo);
            generadorDeCodigoObjeto.finaliza();
            return generadorDeCodigoObjeto.EnsambladorFich;
        }

        public void regla_2(Atributo P,Atributo B,Atributo P1)
        {//| P.cod = B.cod || P1.cod
            P.Codigo = B.Codigo;
            P.Codigo.AddRange(P1.Codigo);
        }

        public void regla_3(Atributo P,Atributo F,Atributo P1)
        {
            // TODO:
            P.Codigo = new List<Cuarteto>();
            P.Codigo.AddRange(P1.Codigo);
        }

        public void regla_4(Atributo P)
        {/* P->
                | P.cod = vacío*/
            P.Codigo = new List<Cuarteto>();
        }

        public void regla_5(Atributo B)
        {// | B.cod = vacío
            B.Codigo = new List<Cuarteto>();
        }
        
     

        public void regla_6(Atributo B,Atributo E,Atributo S)
        { //R6: B->if AbreParent E CierraParent S
            //| SC.cod = T.cod || gen("if", T.lugar, "=", "0", "goto", SS.siguiente) || SS.cod || gen(SS.siguiente, ":")
            B.Codigo = E.Codigo;
            Atributo sSiguiente = new Atributo();
            sSiguiente.Operando = "s_" + numEtiq+ "_siguiente";
            numEtiq++;
            sSiguiente.TipoOperando = TipoOperando.Etiqueta;
            B.Codigo.Add(new Cuarteto(Operador.OP_IF_N,E,uno,sSiguiente));
            B.Codigo.AddRange(S.Codigo);
            B.Codigo.Add(new Cuarteto(Operador.OP_ETIQ,null,null,sSiguiente));
                       


        }

        public void regla_7(Atributo B,Atributo C,Atributo E)
        { /*B->do AbreCorchetes C CierraCorchetes while AbreParent E CierraParent PuntoComa
                B.code = gen("B. Inicio") || C.code || E. Code || gen(If E.value != 0 goto B.inicio)*/
            B.Codigo = new List<Cuarteto>();
            Atributo inicio = nuevaEtiqueta("B_", "Inicio");
            B.Codigo.Add(new Cuarteto(Operador.OP_ETIQ,null,null,inicio));
            B.Codigo.AddRange(C.Codigo);
            B.Codigo.AddRange(E.Codigo);
            B.Codigo.Add(new Cuarteto(Operador.OP_IF_N,E,cero,inicio));
            
        }
        public void regla_8(Atributo B, Atributo S)
        {
            /*B->S
                | B.lugar = S.lugar
                | B.cod = S.cod*/
            B.TipoOperando = S.TipoOperando;
            B.Operando = S.Operando;
            B.Codigo = S.Codigo;


        }

        public void regla_9(Atributo T, Atributo @int)
        {
            //T->int

        }


        public void regla_12(Atributo S, Atributo ID, Atributo IGUAL, Atributo E, Atributo PuntoComa)
        {
            /*S->ID IGUAL E PuntoComa
                | S.cod = E.cod || gen(buscaLugarTS(ID.pos), "=", E.lugar)*/
            //gesTS.buscarDesplazamientoTS(ID.Lexema) = E.Lugar;
            //generadorDeCodigoObjeto.codegen()

            var aux = gesTS.buscarDesplazamientoTS(ID.Lexema);
            ID.TipoOperando = aux.Item1;
            ID.Operando = aux.Item2;
            S.Codigo = E.Codigo;
            S.Codigo.Add(new Cuarteto(Operador.OP_ASIG, E, null, ID));
           // generadorDeCodigoObjeto.codegen(Operador.OP_ASIG, E, null, ID);
        }

        public void regla_14(Atributo S,Atributo E)
        {// S->print(E) 
            
            S.Codigo = E.Codigo;
            S.Codigo.Add(new Cuarteto(Operador.OP_PRINT, null, null, E));
           // generadorDeCodigoObjeto.codegen(Operador.OP_PRINT, null, null, E);

        }
        
        public void regla_15(Atributo S,Atributo ID)
        {
            /*S->input AbreParent ID CierraParent PuntoComa
            S.cod = gen("input" , ID)*/
            S.Codigo = new List<Cuarteto>();
            var aux = gesTS.buscarDesplazamientoTS(ID.Lexema);
            ID.TipoOperando = aux.Item1;
            ID.Operando = aux.Item2;
          
            S.Codigo.Add(new Cuarteto(Operador.OP_INPUT,null,null,ID));
            
        }

        public void regla_26(Atributo C,Atributo B,Atributo C1)
        { //C->B C
            //C.Codigo = B.Codigo || C.Codigo
            
            // TODO: quitar esto cuando este completado. SIrve para q no pete.
            if(B.Codigo == null){B.Codigo = new List<Cuarteto>();}
            C.Codigo = B.Codigo;
            C.Codigo.AddRange(C1.Codigo);



        }
        public void regla_27(Atributo C)
        { //C->
             //C.Codigo = "";

            C.Codigo = new List<Cuarteto>();
        }

        public void regla_32(Atributo E, Atributo E1, Atributo R,Desplazamiento despl, int size_int)
        {// E -> E1 && R
            /*| E.siguiente = nuevaEtiq()
              | E.cod = E1.cod ||gen(E.lugar, "=", "1")|| gen("if", E1.lugar, "=", 1, "goto", E.siguiente) || gen (E.lugar, "=", "0") || 
              | gen(E.siguiente, ":") || R.cod || gen("if", R.lugar, "=", 1, "goto", E.fin) || gen(E.lugar, "=", "0") ||
              | gen(E.fin, ":")  */
            
            // crear temp y confis.
            E.Lexema = gesTS.crearNuevaTemporal(despl.Despl, R.Tipo.ToString());
            var aux = gesTS.buscarDesplazamientoTS(E.Lexema);
            E.TipoOperando = aux.Item1;
            E.Operando = aux.Item2;
            despl.Despl += size_int;
            despl.update();
            // fin crear temp
            Atributo etiq_siguiente = nuevaEtiqueta("E_", "siguiente");
            Atributo etiq_fin = nuevaEtiqueta("E_", "fin");
            E.Codigo = E1.Codigo;
            E.Codigo.Add(new Cuarteto(Operador.OP_ASIG,uno,null,E));
            E.Codigo.Add(new Cuarteto(Operador.OP_IF,E1,uno,etiq_siguiente));
            E.Codigo.Add(new Cuarteto(Operador.OP_ASIG,cero,null,E));
            E.Codigo.Add(new Cuarteto(Operador.OP_ETIQ,null,null,etiq_siguiente));
            E.Codigo.AddRange(R.Codigo);
            E.Codigo.Add(new Cuarteto(Operador.OP_IF,R,uno,etiq_fin));
            E.Codigo.Add(new Cuarteto(Operador.OP_ASIG,cero,null,E));
            E.Codigo.Add(new Cuarteto(Operador.OP_ETIQ,null,null,etiq_fin));

        }

        public void regla_33(Atributo E, Atributo R)
        {
            /*
             E->R
                | E.lugar = R.lugar
	            | E.cod = R.cod*/
            E.TipoOperando = R.TipoOperando;
            E.Operando = R.Operando;
            E.Codigo = R.Codigo;

        }
        
        public void regla_34(Atributo R, Atributo R1, Atributo U,Desplazamiento despl, int size_int)
        {
            //R->R IGUALIGUAL U
            //| R.cod = gen("if", R1.lugar, "=", U.lugar, "goto", R.siguiente) || gen(R.lugar, "=", "0") || gen(R.siguiente, ":") || gen(R.lugar, "=", "1")
            
            R.Lexema = gesTS.crearNuevaTemporal(despl.Despl, U.Tipo.ToString());
            var aux = gesTS.buscarDesplazamientoTS(R.Lexema);
            R.TipoOperando = aux.Item1;
            R.Operando = aux.Item2;
            despl.Despl += size_int;
            despl.update();
            
            R.Codigo = new List<Cuarteto>();
            
            R.Codigo.AddRange(R1.Codigo);
            R.Codigo.AddRange(U.Codigo);
            
            Atributo rSiguiente = new Atributo();
            rSiguiente.TipoOperando = TipoOperando.Etiqueta;
            rSiguiente.Operando = "r_" + numEtiq+ "_siguiente";
            numEtiq++;
            R.Codigo.Add(new Cuarteto(Operador.OP_ASIG,cero,null,R));
            R.Codigo.Add(new Cuarteto(Operador.OP_IF_N,R1,U,rSiguiente));
            R.Codigo.Add(new Cuarteto(Operador.OP_ASIG,uno,null,R));
            R.Codigo.Add(new Cuarteto(Operador.OP_ETIQ,null,null,rSiguiente));
            
            
            

        }

        public void regla_35(Atributo R, Atributo U)
        {
            /*
             R->U
                | R.lugar = U.lugar
	            | R.cod = U.cod*/
           
            R.TipoOperando = U.TipoOperando;
            R.Operando = U.Operando;
            R.Codigo = U.Codigo;
        }

        public void regla_36(Atributo U, Atributo U1, Atributo Suma, Atributo V, Desplazamiento despl, int size_int)
        {
            /*U->U Suma V
                      | U.lugar = nuevoTemp()
                      | U.cod = U1.cod || V.cod || gen(U.lugar, "=", U1.lugar, "+", V.lugar)*/
            
            U.Lexema = gesTS.crearNuevaTemporal(despl.Despl, U.Tipo.ToString());
            var aux = gesTS.buscarDesplazamientoTS(U.Lexema);
            U.TipoOperando = aux.Item1;
            U.Operando = aux.Item2;
            despl.Despl += size_int;
            despl.update();
            U.Codigo = U1.Codigo;
            U.Codigo.AddRange(V.Codigo);
            U.Codigo.Add(new Cuarteto(Operador.OP_PLUS, U1, V, U));
          
        }

        public void regla_37(Atributo U, Atributo V)
        {
            /*
             U->V
                | U.lugar = V.lugar
                | U.cod = V.cod*/
            U.TipoOperando = V.TipoOperando;
            U.Operando = V.Operando;
            U.Codigo = V.Codigo;


        }

        /*
         R38:
        V->ID
            | V.lugar = buscaLugarTS(ID.pos)
            | V.cod =  λ*/
        public void regla_38(Atributo V, Atributo ID) {
            //V.Lugar = gesTS.buscarLugarTS(ID.Lexema);// buscaLugarTS(ID.pos);
            var aux = gesTS.buscarDesplazamientoTS(ID.Lexema);
            V.TipoOperando = aux.Item1;
            V.Operando = aux.Item2;
            V.Codigo = new List<Cuarteto>(); // V.cod = λ
        }


        public void regla_39(Atributo V, Atributo digito)
        {
            /*V.lugar = Inmedioato,digito*/
            
            V.TipoOperando = TipoOperando.Inmediato;
            V.Operando = digito.Digito.ToString();
            V.Codigo = new List<Cuarteto>();
            //V.Digito = digito.Digito;
        }


        public void regla_40(Atributo V, Atributo @true)
        {
            /*V->true
                | V.cod = gen(V.lugar, "=", 1)*/
            V.TipoOperando = TipoOperando.Inmediato;
            V.Operando = "1";
            V.Codigo = new List<Cuarteto>();

        }

        public void regla_41(Atributo V, Atributo @false)
        {
            /*V->false
                | V.cod = gen(V.lugar, "=", 0)*/
            V.TipoOperando = TipoOperando.Inmediato;
            V.Operando = "0";
            V.Codigo = new List<Cuarteto>();
        }

        public void regla_42(Atributo V, Atributo cadena)
        {
            /*V->cadena
                V.lugar = Inmediato cadena
                V.Cadena =*/
           // V.Lugar = new Tuple<TipoOperando, string>(TipoOperando.Inmediato, cadena.Cadena);
            V.TipoOperando = TipoOperando.Inmediato;
            V.Operando = cadena.Cadena;
            V.Codigo = new List<Cuarteto>();
            //V.Cadena = cadena.Cadena;
        }

        public void regla_45(Atributo V,Atributo ID,Atributo MASMAS,Desplazamiento despl,int size_int) // TODO: Revisar NO TESTEADA Y TENGO SUEÑO.
        { //V->ID MASMAS
           
            
            //Crea un temporal
            V.Lexema = gesTS.crearNuevaTemporal(despl.Despl, V.Tipo.ToString());
            var aux1 = gesTS.buscarDesplazamientoTS(V.Lexema);
            V.TipoOperando = aux1.Item1;
            V.Operando = aux1.Item2;
            despl.Despl += size_int;
            despl.update();
            
            var aux = gesTS.buscarDesplazamientoTS(ID.Lexema);
            ID.TipoOperando = aux.Item1;
            ID.Operando = aux.Item2;
            
            V.Codigo = new List<Cuarteto>();
            V.Codigo.Add(new Cuarteto(Operador.OP_ASIG, ID, null, V));
            
            // asignamos a V el valor del ID antes de incrementar.
           // generadorDeCodigoObjeto.codegen(Operador.OP_ASIG, ID, null, V);
            
            V.Codigo.Add(new Cuarteto(Operador.OP_POST_INC,null,null,ID));
            // ahora si incrementamos
           // generadorDeCodigoObjeto.codegen(Operador.OP_POST_INC,null,null,ID);
    

        }


        public void ponerEtiqueta()
        {
        }


    }
}