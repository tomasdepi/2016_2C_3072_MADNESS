using Microsoft.DirectX;
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
        private TgcArrow arrowX;
        private TgcArrow arrowZ;

        private bool orientacionX;
        private bool orientacionY;

        public Oponente(string mediaPath, Vector3 posInicial) : base(mediaPath, posInicial)
        {
            MediaDir = mediaPath;
            arrowX = new TgcArrow();
            arrowZ = new TgcArrow();
        }

        public void seguirObjetivo(Moto moto, float ElepsedTime)
        {

            Vector3 vectorEnX = new Vector3(moto.getPosicion().X, -5000, this.getPosicion().Z);
            Vector3 vectorEnZ = new Vector3(this.getPosicion().X, -5000, moto.getPosicion().Z);
            //Vector3 direccion = new Vector3(moto.getPosicion().X - this.getPosicion().X, 0, moto.getPosicion().Z - this.getPosicion().Z);

            var distanciaEnX = moto.getPosicion().X - this.getPosicion().X; 
            var distanciaEnZ = moto.getPosicion().Z - this.getPosicion().Z;

            //var angulo1 = Vector3.Dot(vectorEnX, direccion);
            //var angulo2 = Vector3.Dot(vectorEnZ, direccion);
            
            arrowX.PStart = this.getPosicion();
            arrowX.PEnd = vectorEnX;
            arrowZ.PStart = this.getPosicion();
            arrowZ.PEnd = vectorEnZ;

            arrowX.updateValues();
            arrowZ.updateValues();
            

            if (!moto.getPosicion().Equals(new Vector3(0, -5000, 0)))
            { //posicion inicial

                this.acelerar(ElepsedTime);


                if (Math.Abs(distanciaEnX) > Math.Abs(distanciaEnZ))
                {
                    if (distanciaEnX > 0)
                    {
                        //if (this.getOrientacion() == 1) this.girarIzquierda();
                        //if (this.getOrientacion() == 3) this.girarDerecha();
                    }
                    else
                    {
                        //if (this.getOrientacion() == 1) this.girarDerecha();
                        //if (this.getOrientacion() == 3) this.girarIzquierda();
                    }

                }
                else
                {
                    /*if (distanciaEnZ > 0)
                    {
                        if (this.getOrientacion() == 2) this.girarDerecha();
                        if (this.getOrientacion() == 3) this.girarIzquierda();
                    }
                    else
                    {
                        if (this.getOrientacion() == 1) this.girarIzquierda();
                        if (this.getOrientacion() == 3) this.girarDerecha();
                    }*/
                }
                    



            }

        }

        public new void render()
        {
            base.render();
            arrowX.render();
            arrowZ.render();
        }

        public new void dispose()
        {
            base.render();
            arrowX.render();
            arrowZ.render();
        }

    }
}
