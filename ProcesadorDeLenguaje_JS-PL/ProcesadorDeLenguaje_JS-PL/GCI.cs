using System;
using System.Collections.Generic;

namespace ProcesadorDeLenguaje_JS_PL
{
    public class GCI
    {
        private GestorDeErrores gestorDeErrores;
        private GestorTS gesTS;
        private readonly AnalisisLexico alex;
        
        GCI(GestorTS gesTS, AnalisisLexico alex, GestorDeErrores gestorDeErrores)
        {
            this.alex = alex;
            this.gesTS = gesTS;
            this.gestorDeErrores = gestorDeErrores;
            
        }
        // TODO: 1,2,4,5,8,9,12,33,35,36,37,38 
        /** IMPLEMENTACION. Tenemos las reglas. pasamos los atributos como objetos y ya. POrque estamos pasando un obj y se pasan por def por referencia jeje.
         * Cuando tenemos un cuarteto :
         * 
         */
        public void regla_1()
        {
            
        }
        public void regla_2()
        {
            
        }
        

        
        /*
         R38:
        V->ID
            | V.lugar = buscaLugarTS(ID.pos)
            | V.cod =  λ*/
        public void regla_38(Atributo V,Atributo ID)
        {
            //V.Lugar = gesTS.buscarLugarTS(ID.Lexema);// buscaLugarTS(ID.pos);
        }
    }
}