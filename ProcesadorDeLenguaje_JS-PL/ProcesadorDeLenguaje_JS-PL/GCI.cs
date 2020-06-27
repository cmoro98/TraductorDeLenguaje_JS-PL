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

        public GCI(GestorTS gesTS, AnalisisLexico alex, GestorDeErrores gestorDeErrores)
        {
            this.alex = alex;
            this.gesTS = gesTS;
            this.gestorDeErrores = gestorDeErrores;
            generadorDeCodigoObjeto = new GCO();
        }

        // TODO: 1,2,4,5,8,9,12,33,35,36,37,38 
        /** IMPLEMENTACION. Tenemos las reglas. pasamos los atributos como objetos y ya. POrque estamos pasando un obj y se pasan por def por referencia jeje.
         * Cuando tenemos un cuarteto :
         * 
         */
        public void regla_1() {
        }

        public void regla_2()
        {
        }

        public void regla_4()
        {
        }

        public void regla_5()
        {
        }
        
        public void regla_8(Atributo B, Atributo S)
        {
            /*B->S
                | B.lugar = S.lugar
                | B.cod = S.cod*/
            B.TipoOperando = S.TipoOperando;
            B.Operando = S.Operando;


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
            
            generadorDeCodigoObjeto.codegen(Operador.OP_ASIG, E, null, ID);
        }

        public void regla_33(Atributo E, Atributo R)
        {
            /*
             E->R
                | E.lugar = R.lugar
	            | E.cod = R.cod*/
            E.TipoOperando = R.TipoOperando;
            E.Operando = R.Operando;

        }

        public void regla_35(Atributo R, Atributo U)
        {
            /*
             R->U
                | R.lugar = U.lugar
	            | R.cod = U.cod*/
           
            R.TipoOperando = U.TipoOperando;
            R.Operando = U.Operando;
        }

        public void regla_36(Atributo U, Atributo U1, Atributo Suma, Atributo V, Desplazamiento despl, int size_int)
        {
            /*U->U Suma V
                      | U.lugar = nuevoTemp()
                      | U.cod = U1.cod || V.cod || gen(U.lugar, "=", U1.lugar, "+", V.lugar)*/


            gesTS.crearNuevaTemporal(despl.Despl, U.Tipo.ToString());
            despl.Despl += size_int;
            despl.update();

            generadorDeCodigoObjeto.codegen(Operador.OP_PLUS, U1, V, U);
        }

        public void regla_37(Atributo U, Atributo V)
        {
            /*
             U->V
                | U.lugar = V.lugar
                | U.cod = V.cod*/
            U.TipoOperando = V.TipoOperando;
            U.Operando = V.Operando;


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
        }


        public void regla_39(Atributo V, Atributo digito)
        {
            /*V.lugar = Inmedioato,digito*/
            
            V.TipoOperando = TipoOperando.Inmediato;
            V.Operando = digito.Digito.ToString();
            //V.Digito = digito.Digito;
        }


        public void regla_40(Atributo V, Atributo @true)
        {
            /*V->true
                | V.cod = gen(V.lugar, "=", 1)*/
        }

        public void regla_41(Atributo V, Atributo @false)
        {
            /*V->false
                | V.cod = gen(V.lugar, "=", 0)*/
        }

        public void regla_42(Atributo V, Atributo cadena)
        {
            /*V->cadena
                V.lugar = Inmediato cadena
                V.Cadena =*/
           // V.Lugar = new Tuple<TipoOperando, string>(TipoOperando.Inmediato, cadena.Cadena);
            V.TipoOperando = TipoOperando.Inmediato;
            V.Operando = cadena.Cadena;
            //V.Cadena = cadena.Cadena;
        }


        public void ponerEtiqueta()
        {
        }


    }
}