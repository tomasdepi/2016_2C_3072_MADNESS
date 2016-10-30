using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Text;
using TGC.Core.Utils;

namespace TGC.Group.Model
{
    public class Oponente : Moto
    {

        private String MediaDir;

        private bool orientacionX;
        private bool orientacionY;

        private float tiempo;

        public Oponente(string mediaPath, Vector3 posInicial) : base(mediaPath, posInicial)
        {
            MediaDir = mediaPath;

            tiempo = 0;
        }

        public void seguirObjetivo(Moto moto, float ElepsedTime, List<CustomVertex.PositionColored[]> obstaculos)
        {

            if (!moto.getPosicion().Equals(new Vector3(0, 0, 0)))
            { //posicion inicial

                this.acelerar(ElepsedTime);
              
                if (verificarGiro(ElepsedTime) && comprobarColisionSiguienteUpdate(obstaculos))
                {
                    if (verificarGiroDerecha(moto, ElepsedTime)) this.girarDerecha();
                    if (verificarGiroIzquierda(moto, ElepsedTime)) this.girarIzquierda();
                }
                  
            }

        }


        private bool comprobarColisionSiguienteUpdate(List<CustomVertex.PositionColored[]> obstaculos)
        {
            this.avanzar((float)0.1);
            var resAvanzar = this.coomprobarColisionPathLight(obstaculos);
            this.retroceder((float)0.1);

            if (!resAvanzar) return true;

            Random random = new Random();
            int p = random.Next();

            if(p%2 == 0)
            {
                this.girarDerecha();
                this.avanzar((float)0.1);
                var res = this.coomprobarColisionPathLight(obstaculos);
                this.retroceder((float)0.1);

                if (!res) return true;

                this.girarIzquierda();
                this.girarIzquierda();
            }
            else
            {
                this.girarIzquierda();
                this.avanzar((float)0.1);
                var res = this.coomprobarColisionPathLight(obstaculos);
                this.retroceder((float)0.1);

                if (!res) return true;

                this.girarDerecha();
                this.girarDerecha();

            }
            return false;
        }

        private bool verificarGiro(float ElepsedTime)
        {
            tiempo += ElepsedTime;
            if (tiempo > 2)
            {
                tiempo = 0;
                return true;
            }
            return false;
        }

        private bool verificarPosAdelante(Moto moto, float ElepsedTime)
        {
            double distAhora = this.distancia(moto.getPosicion());
            this.avanzar(ElepsedTime);
            double distDespues = this.distancia(moto.getPosicion());
            this.retroceder(ElepsedTime);

            return distDespues < distAhora ? true : false;
        }

        private bool verificarGiroIzquierda(Moto moto, float ElepsedTime)
        {
            this.girarIzquierda();
            bool resu = verificarPosAdelante(moto, ElepsedTime);
            this.girarDerecha();
            this.retroceder(ElepsedTime);
            return resu;
        }

        private bool verificarGiroDerecha(Moto moto, float ElepsedTime)
        {
            this.girarDerecha();
            bool resu = verificarPosAdelante(moto, ElepsedTime);
            this.girarIzquierda();
            this.retroceder(ElepsedTime);
            return resu;
        }

        public new void render()
        {
            base.render();
        }

        public new void dispose()
        {
            base.render();
        }

    }
}
